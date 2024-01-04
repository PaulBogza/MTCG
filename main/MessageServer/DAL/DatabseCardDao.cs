using Npgsql;
using NUnit.Framework.Internal;
using SWE1.MessageServer.BLL;
using SWE1.MessageServer.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Reflection.Metadata;
using System.Runtime.ConstrainedExecution;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace SWE1.MessageServer.DAL
{
    internal class DatabaseCardDao : ICardDao
    {
        private const string CreateCardsTableCommand = @"CREATE TABLE IF NOT EXISTS cards (id varchar PRIMARY KEY, name varchar, damage float,
                                                         element varchar, race varhchar, owner_id int, foreign key(owner_id) references users(id));";
        private const string CreatePackageTableCommand = @"CREATE TABLE IF NOT EXISTS packages(id serial PRIMARY KEY, card1_id varchar, card2_id varchar,
                                                            card3_id varchar, card4_id varchar, card5_id varchar, owner_id int,
                                                            foreign key (owner_id) references users (id),
                                                            foreign key (card1_id) references cards (id),
                                                            foreign key (card2_id) references cards (id),
                                                            foreign key (card3_id) references cards (id),
                                                            foreign key (card4_id) references cards (id),
                                                            foreign key (card5_id) references cards (id)
                                                            )";     
        private const string CreateDeckTableCommand = @"CREATE TABLE IF NOT EXISTS deck(id serial PRIMARY KEY, card1_id varchar, card2_id varchar,
                                                            card3_id varchar, card4_id varchar, card5_id varchar, owner_id int,
                                                            foreign key (owner_id) references users (id),
                                                            foreign key (card1_id) references cards (id),
                                                            foreign key (card2_id) references cards (id),
                                                            foreign key (card3_id) references cards (id),
                                                            foreign key (card4_id) references cards (id)
                                                            )";
        private const string InsertCardsCommand = @"INSERT INTO cards (id, name, damage, element, race) VALUES (@id, @name, @damage, @element, @type)";
        private const string SelectFromCardsForPackageCommand = @"SELECT card1_id, card2_id, card3_id, card4_id, card5_id FROM cards
                                                                WHERE card1_id = @card1_id, card2_id = @card2_id, card3_id = @card3_id, card4_id = @card4_id, card5_id = @card5_id";
        private const string InsertCardIntoPackageCommand = @"INSERT INTO packages (card1_id, card2_id, card3_id, card4_id) VALUES (@card1_id, @card2_id, 
                                                            @card3_id, @card4_id, @card5_id)";
        private const string InsertCardsIntoDeckCommand = @"INSERT INTO deck (card1_id varchar, card2_id varchar, card3_id varchar, card4_id varchar, owner_id int,    
                                                            VALUES (@card1_id, @card2_id, @card3_id, @card4_id, @owner_id))";
        private const string SelectCardsFromPackageCommand = @"SELECT card1_id, card2_id, card3_id, card4_id, card5_id FROM packages WHERE owner_id = @owner_id";
        private const string SetOwnerIdForCardsCommand = @"UPADTE cards SET owner_id=@owner_id WHERE id=@id";
        private const string SetOwnerIdForPackageCommand = @"UPADTE packages SET owner_id=@owner_id WHERE owner_id=null LIMIT 1";
        private const string GetUserIdCommand = @"SELECT id FROM users WHERE username=@username";
        private const string GetUserCoinsCommand = @"SELECT username FROM users WHERE username = @username";
        private const string SetUserCoinsCommand = @"UPDATE users SET coins=@coins WHERE username = @username ";

        private readonly string _connectionString;

        public DatabaseCardDao(string connectionString)
        {
            _connectionString = connectionString;
            EnsureTables();
        }

        public List<Card>? CreatePackage(List<Card> Package){      
            using var connection = new NpgsqlConnection(_connectionString);
            connection.Open();

            foreach(var card in Package){
                using var cmd = new NpgsqlCommand(InsertCardsCommand, connection);
                cmd.Parameters.AddWithValue("id", card.Id);
                cmd.Parameters.AddWithValue("name", card.Name);
                cmd.Parameters.AddWithValue("damage", card.Damage);
                cmd.Parameters.AddWithValue("element", card.Element);
                cmd.Parameters.AddWithValue("race", card.Type);
                var result = cmd.ExecuteNonQuery();
                if(result > 0){
                    continue;
                }
                else{
                    throw new Exception("Card Duplicate");
                }
            }

            int index = 0;
            foreach(var card in Package){
                using var cmd = new NpgsqlCommand(InsertCardIntoPackageCommand, connection);
                cmd.Parameters.AddWithValue($"card{index}_id", card.Id);
                var result = cmd.ExecuteNonQuery();
                if(result > 0){
                    index++;
                    continue;
                }
                else{
                    throw new Exception("Insert card into package failed");
                }
            }
            return Package;
        }
        public void initDeck(User user){

        }
        public int getUserId(User user){
            using var connection = new NpgsqlConnection(_connectionString);
            connection.Open();
            int userId = 0;
            using var cmd = new NpgsqlCommand(GetUserIdCommand, connection);
            cmd.Parameters.AddWithValue("id", user.Username);
            var reader = cmd.ExecuteReader();
            if(reader.Read()){
                userId = reader.GetOrdinal("id");
            }
            return userId;
        }
        public void setOwnerIdForCardsInPackage(User user){
            int userId = getUserId(user);
            using var connection = new NpgsqlConnection(_connectionString);
            connection.Open();
            
            using var cmd = new NpgsqlCommand(SelectCardsFromPackageCommand, connection);
            cmd.Parameters.AddWithValue("owner_id", userId);
            var reader = cmd.ExecuteReader();

            List<string>CardIds = new();
            int index = 0;
            while(reader.Read()){
                string id = reader.GetString(reader.GetOrdinal($"card{index}_id"));
                CardIds.Add(id); 
                index++;
            }

            foreach(string id in CardIds){
                using var cmd2 = new NpgsqlCommand(SetOwnerIdForCardsCommand, connection);
                cmd2.Parameters.AddWithValue("owner_id", userId);
                cmd2.Parameters.AddWithValue("id", id);
                var result = cmd2.ExecuteNonQuery();
                if(result > 0){
                    continue;
                }
                else{
                    throw new Exception("Setting owner_id for card failed");
                }
            }
        }
        public bool AquirePackage(User user){
            using var connection = new NpgsqlConnection(_connectionString);
            connection.Open();
            int userId = getUserId(user);
            using var cmd = new NpgsqlCommand(SetOwnerIdForPackageCommand, connection);
            cmd.Parameters.AddWithValue("owner_id", userId);
            var result = cmd.ExecuteNonQuery();
            setOwnerIdForCardsInPackage(user);

            if(result > 0){
                return true;
            }

            return false;
        }
        public List<Card>? ShowCards(User user){
            using var connection = new NpgsqlConnection(_connectionString);
            connection.Open(); 


            return user.Stack;
        }
        public List<Card>? ShowDeck(User user){
            return user.Deck;
        }
        public List<Card>? UpdateDeck(User user, List<string> payload){
            return user.Deck;
        }

        private void EnsureTables()
        {
            using var connection = new NpgsqlConnection(_connectionString);
            connection.Open();
            using var cmd = new NpgsqlCommand(CreateCardsTableCommand, connection);
            cmd.ExecuteNonQuery();
            using var cmd2 = new NpgsqlCommand(CreatePackageTableCommand, connection);
            cmd2.ExecuteNonQuery();
            using var cmd3 = new NpgsqlCommand(CreateDeckTableCommand, connection);
            cmd3.ExecuteNonQuery();
        }
    }
}
