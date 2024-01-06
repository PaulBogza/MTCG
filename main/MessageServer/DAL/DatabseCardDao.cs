using Npgsql;
using NUnit.Framework.Constraints;
using NUnit.Framework.Internal;
using SWE1.MessageServer.BLL;
using SWE1.MessageServer.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Net;
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
                                                        owner_id int, foreign key(owner_id) references users(id));";
        private const string CreatePackageTableCommand = @"CREATE TABLE IF NOT EXISTS packages(id serial PRIMARY KEY, card1_id varchar, card2_id varchar,
                                                            card3_id varchar, card4_id varchar, card5_id varchar,
                                                            foreign key (card1_id) references cards (id),
                                                            foreign key (card2_id) references cards (id),
                                                            foreign key (card3_id) references cards (id),
                                                            foreign key (card4_id) references cards (id),
                                                            foreign key (card5_id) references cards (id)
                                                            )";     
        private const string CreateDeckTableCommand = @"CREATE TABLE IF NOT EXISTS deck(id serial PRIMARY KEY, card1_id varchar, card2_id varchar,
                                                            card3_id varchar, card4_id varchar, owner_id int,
                                                            foreign key (owner_id) references users (id),
                                                            foreign key (card1_id) references cards (id),
                                                            foreign key (card2_id) references cards (id),
                                                            foreign key (card3_id) references cards (id),
                                                            foreign key (card4_id) references cards (id)
                                                            )";
        private const string InsertCardsCommand = @"INSERT INTO cards (id, name, damage) VALUES (@id, @name, @damage)";
        private const string InsertCardIntoPackageCommand = @"INSERT INTO packages (card1_id, card2_id, card3_id, card4_id, card5_id) VALUES (@card1_id, @card2_id, 
                                                            @card3_id, @card4_id, @card5_id)";
        private const string InsertCardsIntoDeckCommand = @"INSERT INTO deck (card1_id, card2_id, card3_id, card4_id, owner_id)    
                                                            VALUES (@card1_id, @card2_id, @card3_id, @card4_id, @owner_id)";
        private const string DeleteDeckCommand = @"DELETE FROM deck WHERE owner_id=@owner_id";
        private const string GetDeckCommand = @"SELECT * FROM deck WHERE owner_id=@owner_id";
        private const string SetOwnerIdForCardsCommand = @"UPDATE cards SET owner_id=@owner_id WHERE id=@id";
        private const string BuyPackageCommand = @"SELECT * FROM packages ORDER BY id ASC LIMIT 1";
        private const string DeletePackageCommand = @"DELETE FROM packages WHERE id=@id";
        private const string GetUserIdCommand = @"SELECT id FROM users WHERE username=@username";
        private const string SetUserCoinsCommand = @"UPDATE users SET coins=@coins WHERE username=@username";
        private const string GetUserCoinsCommand = @"SELECT coins FROM users WHERE username=@username";
        private const string ShowCardsCommand = @"SELECT * FROM cards WHERE owner_id=@owner_id";
        private const string GetRowsCommand = @"SELECT COUNT(*) FROM cards WHERE owner_id=@owner_id";
        private const string SelectCardsFromDeck = @"SELECT * FROM cards WHERE id=@id";
        private const string SelectCardsCommand = @"SELECT * FROM cards WHERE id=@id AND owner_id=@owner_id";
        private readonly string _connectionString;

        public DatabaseCardDao(string connectionString)
        {
            _connectionString = connectionString;
            EnsureTables();
        }
        public List<Card>? CreatePackage(List<Card> Package){      
            try{
                foreach(var card in Package){
                    using var connection = new NpgsqlConnection(_connectionString);
                    connection.Open();
                    using var cmd = new NpgsqlCommand(InsertCardsCommand, connection);
                    cmd.Parameters.AddWithValue("id", card.Id);
                    cmd.Parameters.AddWithValue("name", card.Name);
                    cmd.Parameters.AddWithValue("damage", card.Damage);
                    var result = cmd.ExecuteNonQuery();
                    if(result <= 0){
                        break;
                    }
                    connection.Close();
                }

                using var connection2 = new NpgsqlConnection(_connectionString);
                connection2.Open();
                using var cmd2 = new NpgsqlCommand(InsertCardIntoPackageCommand, connection2);
                int index = 1;
                foreach(var card in Package){
                    cmd2.Parameters.AddWithValue($"card{index}_id", card.Id);
                    index++;
                }
                cmd2.ExecuteNonQuery();
            }
            catch(Exception e){
                System.Console.WriteLine(e);
            }

            return Package;
        }
        public int getUserId(User user){
            using var connection = new NpgsqlConnection(_connectionString);
            connection.Open();
            int userId = 0;
            using var cmd = new NpgsqlCommand(GetUserIdCommand, connection);
            cmd.Parameters.AddWithValue("username", user.Username);
            var reader = cmd.ExecuteReader();
            if(reader.Read()){
                userId = (int)reader["id"];
            }
            return userId;
        }
        public void setOwnerIdForCards(User user, string CardId){
            int userId = getUserId(user);
            using var connection = new NpgsqlConnection(_connectionString);
            connection.Open();
     
            using var cmd = new NpgsqlCommand(SetOwnerIdForCardsCommand, connection);
            cmd.Parameters.AddWithValue("owner_id", userId);
            cmd.Parameters.AddWithValue("id", CardId);
            cmd.ExecuteNonQuery();
        }
        public bool AquirePackage(User user){
            using var connection = new NpgsqlConnection(_connectionString);
            connection.Open();
            using var cmd = new NpgsqlCommand(BuyPackageCommand, connection);
            var reader = cmd.ExecuteReader();

            List<string>CardIds = new();
            int PackageId = new();
            while(reader.Read()){
                PackageId = (int)reader["id"];
                for(int i = 1; i < 6; i++){
                    CardIds.Add((string)reader[$"card{i}_id"]);
                }
            }
            reader.Close();
            
            if(!CardIds.Any()){
                return false;
            }

            int userCoins = new();
            try{
                using var connection1 = new NpgsqlConnection(_connectionString);
                connection1.Open();
                using var readCoins = new NpgsqlCommand(GetUserCoinsCommand, connection1);
                readCoins.Parameters.AddWithValue("username", user.Username);
                var coins = readCoins.ExecuteReader();
                if(coins.Read()){
                    userCoins = (int)coins["coins"];
                }
                coins.Close();
                if(userCoins >= 5){
                    userCoins -= 5;
                }
                else{
                    return false;
                }
                using var cmd1 = new NpgsqlCommand(SetUserCoinsCommand, connection1);
                cmd1.Parameters.AddWithValue("coins", userCoins);
                cmd1.Parameters.AddWithValue("username", user.Username);
                var result = cmd1.ExecuteNonQuery();
            }
            catch(Exception e){
                System.Console.WriteLine(e);
            }

            foreach(var id in CardIds){
                setOwnerIdForCards(user, id);
            }

            try{
                using var connection2 = new NpgsqlConnection(_connectionString);
                connection2.Open();
                using var cmd2 = new NpgsqlCommand(DeletePackageCommand, connection2);
                cmd2.Parameters.AddWithValue("id", PackageId);
                var result = cmd2.ExecuteNonQuery();
                if(result <= 0){
                    return false;
                }
                else{
                    return true;
                }
            }
            catch(Exception e){
                System.Console.WriteLine(e);
                return false;
            }
        }
        public List<Card>? ShowCards(User user)
        {
            int userId = getUserId(user);
            List<Card> cardList = new List<Card>();

            using var connection = new NpgsqlConnection(_connectionString);
            connection.Open();

            using var cmd = new NpgsqlCommand(ShowCardsCommand, connection);
            cmd.Parameters.AddWithValue("owner_id", userId);

            using var reader = cmd.ExecuteReader();

            while (reader.Read()){
                Card tempCard = new Card{
                    Id = (string)reader["id"],
                    Name = (string)reader["name"],
                    Damage = (double)reader["damage"]
                };
                cardList.Add(tempCard);
            }
            user.Stack = cardList;
            return user.Stack;
        }
        public List<Card>? ShowDeck(User user){
            int userId = getUserId(user);
            List<string> tempDeck = new();
            List<Card> cardList = new List<Card>();

            try{
                using var connection = new NpgsqlConnection(_connectionString);
                connection.Open(); 
                using var cmd = new NpgsqlCommand(GetDeckCommand, connection);
                cmd.Parameters.AddWithValue("owner_id", userId);
                using var reader = cmd.ExecuteReader();

                if(reader.Read()){
                    for(int i = 1; i <= 4; i++){
                        tempDeck.Add((string)reader[$"card{i}_id"]);
                    }
                }
                else{
                    return null;
                }
            }
            catch(Exception e){
                System.Console.WriteLine(e);
            }
            
            try{
                using var connection2 = new NpgsqlConnection(_connectionString);
                connection2.Open(); 

                foreach(string id in tempDeck){
                    using var cmd2 = new NpgsqlCommand(SelectCardsFromDeck, connection2);
                    cmd2.Parameters.AddWithValue("id", id);
                    using var reader2 = cmd2.ExecuteReader();

                    while (reader2.Read()){ 
                        Card tempCard = new Card{
                            Id = (string)reader2["id"],
                            Name = (string)reader2["name"],
                            Damage = (double)reader2["damage"]
                        };
                    cardList.Add(tempCard);
                    }
                }   
                user.Deck = cardList;
                return user.Deck;
            }
            catch(Exception e){
                System.Console.WriteLine(e);
                return null;
            }
        }
        public List<Card>? UpdateDeck(User user, List<string> payload){
            int userId = getUserId(user);

            try{
                using var conn2 = new NpgsqlConnection(_connectionString);
                conn2.Open();
                for(int i = 0; i < 4; i++){
                    using var check = new NpgsqlCommand(SelectCardsCommand, conn2);
                    check.Parameters.AddWithValue("owner_id", userId);
                    check.Parameters.AddWithValue("id", payload.ElementAt(i));
                    var ok = check.ExecuteReader();
                    if(!ok.Read()){
                        return null;
                    }
                    ok.Close();
                }

                using var connection = new NpgsqlConnection(_connectionString);
                connection.Open(); 
                using var cmd = new NpgsqlCommand(InsertCardsIntoDeckCommand, connection);
                cmd.Parameters.AddWithValue("card1_id", payload.ElementAt(0));
                cmd.Parameters.AddWithValue("card2_id", payload.ElementAt(1));
                cmd.Parameters.AddWithValue("card3_id", payload.ElementAt(2));
                cmd.Parameters.AddWithValue("card4_id", payload.ElementAt(3));
                cmd.Parameters.AddWithValue("owner_id", userId);
                var result = cmd.ExecuteNonQuery();
                if(result > 0){
                    user.Deck = ShowDeck(user)!;
                    return user.Deck;
                }
                else{
                    return null;
                }
            }
            catch(Exception e){
                System.Console.WriteLine(e);
                return null;
            }
        }

        private void EnsureTables()
        {
            using var connection2 = new NpgsqlConnection(_connectionString);
            connection2.Open();
            using var cmd2 = new NpgsqlCommand(CreateCardsTableCommand, connection2);
            cmd2.ExecuteNonQuery();

            using var connection3 = new NpgsqlConnection(_connectionString);
            connection3.Open();
            using var cmd3 = new NpgsqlCommand(CreatePackageTableCommand, connection3);
            cmd3.ExecuteNonQuery();

            using var connection4 = new NpgsqlConnection(_connectionString);
            connection4.Open();
            using var cmd4 = new NpgsqlCommand(CreateDeckTableCommand, connection4);
            cmd4.ExecuteNonQuery();
        }
    }
}
