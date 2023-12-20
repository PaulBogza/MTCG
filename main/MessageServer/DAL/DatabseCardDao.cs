using CardClass;
using Npgsql;
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
        public List<Card>? CreatePackage(List<Card> Package){
            Dictionary<int, List<Card>> GamePackage = new();
            return Package;
        }
        public bool AquirePackage(User user){
            return user.Coins < 4 ? false : true;
        }
        public List<Card>? ShowCards(User user){
            if(user.Stack == null){
                throw new Exception();
            }
            //TODO Get Users cards out of database
            for(int i = 0; i < user.Stack.Count; i++){
                System.Console.WriteLine(user.Stack[i].Name);
            }
            return user.Stack;
        }

        public List<Card>? ShowDeck(User user){
            if(user.Deck == null){
                throw new Exception();
            }
            //TODO Get Users cards out of database
            for(int i = 0; i < user.Deck.Count; i++){
                System.Console.WriteLine(user.Deck[i].Name);
            }
            return user.Deck;
        }

        public List<Card>? UpdateDeck(User user, string payload){

            return user.Deck;
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
