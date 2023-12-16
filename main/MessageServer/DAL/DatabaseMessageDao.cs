using Npgsql;
using SWE1.MessageServer.API.Routing.Messages;
using SWE1.MessageServer.BLL;
using SWE1.MessageServer.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWE1.MessageServer.DAL
{
    internal class DatabaseMessageDao : IMessageDao
    {
        // see also https://www.postgresql.org/docs/current/ddl-constraints.html
        // TODO: Properly handle implicit dependency on UserDao/Table
        private const string CreateMessageTableCommand = @"CREATE TABLE IF NOT EXISTS messages (id serial PRIMARY KEY, content varchar, username varchar REFERENCES users ON DELETE CASCADE);";
        private const string InsertMessageCommand = "INSERT INTO messages(content, username) VALUES (@content, @username) RETURNING id";
        private const string DeleteMessageCommand = "DELETE FROM messages WHERE id=@id AND username=@username";
        private const string UpdateMessageCommand = "UPDATE messages SET content=@content WHERE id=@id AND username=@username";
        private const string SelectMessageByIdCommand = "SELECT id, content FROM messages WHERE id=@id AND username=@username";
        private const string SelectMessagesCommand = "SELECT id, content FROM messages WHERE username=@username";

        private readonly string _connectionString;

        public DatabaseMessageDao(string connectionString)
        {
            _connectionString = connectionString;
            EnsureTables();
        }

        public void DeleteMessage(string username, int messageId)
        {
            using var connection = new NpgsqlConnection(_connectionString);
            connection.Open();
            using var cmd = new NpgsqlCommand(DeleteMessageCommand, connection);
            cmd.Parameters.AddWithValue("id", messageId);
            cmd.Parameters.AddWithValue("username", username);
            if(cmd.ExecuteNonQuery() <= 0){
                throw new MessageNotFoundException();
            }
        }

        public Message? GetMessageById(string username, int messageId)
        {
            Message? message = null;

            using var connection = new NpgsqlConnection(_connectionString);
            connection.Open();
            using var cmd = new NpgsqlCommand(SelectMessageByIdCommand, connection);
            cmd.Parameters.AddWithValue("id", messageId);
            cmd.Parameters.AddWithValue("username", username);
            // take the first row, if any
            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                message = ReadMessage(reader);
            }
            else{
                throw new MessageNotFoundException();
            }

            return message;
        }

        public IEnumerable<Message> GetMessages(string username)
        {
            var messages = new List<Message>();

            using var connection = new NpgsqlConnection(_connectionString);
            connection.Open();

            using var cmd = new NpgsqlCommand(SelectMessagesCommand, connection);
            cmd.Parameters.AddWithValue("username", username);

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                var message = ReadMessage(reader);
                messages.Add(message);
            }

            return messages;
        }

        public void InsertMessage(string username, Message message)
        {
            using var connection = new NpgsqlConnection(_connectionString);
            connection.Open();

            using var cmd = new NpgsqlCommand(InsertMessageCommand, connection);
            cmd.Parameters.AddWithValue("content", message.Content);
            cmd.Parameters.AddWithValue("username", username);

            // ExecuteScalar returns a single value (see InsertMessageCommand, it's the newly assigned ID)
            var result = cmd.ExecuteScalar();

            message.Id = Convert.ToInt32(result);
        }

        public bool UpdateMessage(string username, Message message)
        {
            using var connection = new NpgsqlConnection(_connectionString);
            connection.Open();

            using var cmd = new NpgsqlCommand(UpdateMessageCommand, connection);
            cmd.Parameters.AddWithValue("id", message.Id);
            cmd.Parameters.AddWithValue("content", message.Content);
            cmd.Parameters.AddWithValue("username", username);

            // ExecuteNonQuery returns the number of affected rows
            return cmd.ExecuteNonQuery() > 0;
   
        }

        private void EnsureTables()
        {
            using var connection = new NpgsqlConnection(_connectionString);
            connection.Open();

            using var cmd = new NpgsqlCommand(CreateMessageTableCommand, connection);
            cmd.ExecuteNonQuery();
        }

        private Message ReadMessage(IDataRecord record)
        {
            return new Message(Convert.ToInt32(record["id"]), Convert.ToString(record["content"])!);
        }
    }
}
