using CardClass;
using Npgsql;
using PlayerClass;
using SWE1.MessageServer.BLL;
using SWE1.MessageServer.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace SWE1.MessageServer.DAL
{
    internal class DatabaseCardDao : ICardDao
    {
        private const string CreateUserTableCommand = @"CREATE TABLE IF NOT EXISTS cards (card_id varchar PRIMARY KEY, name varchar, damage varchar);";
        private const string SelectAllUsersCommand = @"SELECT username, password FROM users";
        private const string SelectUserByCredentialsCommand = "SELECT username, password FROM users WHERE username=@username AND password=@password";
        private const string InsertUserCommand = @"INSERT INTO users(username, password) VALUES (@username, @password)";

        private readonly string _connectionString;

        public DatabaseCardDao(string connectionString)
        {
            _connectionString = connectionString;
            EnsureTables();
        }

        public List<Card>? showCards(Player player){
            if(player.Stack == null){
                throw new Exception();
            }
            //TODO Get Users cards out of database
            for(int i = 0; i < player.Stack.Count; i++){
                System.Console.WriteLine(player.Stack[i].Name);
            }
            return player.Stack;
        }

        public List<Card>? showDeck(Player player){
            if(player.Deck == null){
                throw new Exception();
            }
            //TODO Get Users cards out of database
            for(int i = 0; i < player.Deck.Count; i++){
                System.Console.WriteLine(player.Deck[i].Name);
            }
            return player.Deck;
        }

        public int updateDeck(){

            return 0;
        }

        private void EnsureTables()
        {
            using var connection = new NpgsqlConnection(_connectionString);
            connection.Open();
            using var cmd = new NpgsqlCommand(CreateUserTableCommand, connection);
            cmd.ExecuteNonQuery();
        }
    }
}
