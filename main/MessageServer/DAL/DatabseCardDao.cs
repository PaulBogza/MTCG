using Npgsql;
using SWE1.MessageServer.BLL;
using SWE1.MessageServer.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
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
        private const string InsertCardsCommand = @"INSERT INTO cards (id, name, damage, element, type) VALUES (@id, @name, @damage, @element, @type)";
        private const string SelectFromCardsForPackageCommand = @"SELECT card1_id, card2_id, card3_id, card4_id, card5_id FROM cards
                                                                WHERE card1_id = @card1_id, card2_id = @card2_id, card3_id = @card3_id, card4_id = @card4_id, card5_id = @card5_id";
        private const string InsertCardIntoPackageCommand = @"INSERT INTO packages (card1_id, card2_id, card3_id, card4_id) VALUES (@card1_id, @card2_id, 
                                                            @card3_id, @card4_id, @card5_id)";
        private const string InsertCardsIntoDeckCommand = @"INSERT INTO deck (card1_id varchar, card2_id varchar, card3_id varchar, card4_id varchar, owner_id int,    
                                                            VALUES (@card1_id, @card2_id, @card3_id, @card4_id, @owner_id))";

        private const string GetOwnerId = @"SELECT id FROM users WHERE username=@username";
        private const string GetUserCoins = @"SELECT coins FROM users WHERE username = @username";
        private const string SetUserCoins = @"INSERT INTO users (coins) VALUES (@coins) WHERE username = @username ";

        private readonly string _connectionString;

        public DatabaseCardDao(string connectionString)
        {
            _connectionString = connectionString;
            EnsureTables();
        }

        private readonly Queue<List<Card>> PackageCollection = new();
        public List<Card> tmpDeck = new();
        public List<Card> tmpStack = new();

        public void initCardsTable(List<Card> Package){
            using var connection = new NpgsqlConnection(_connectionString);
            connection.Open();

            foreach(var elem in Package){
                if(elem != null){
                    using var cmd = new NpgsqlCommand(InsertCardsCommand, connection);
                    cmd.Parameters.AddWithValue("id", elem.Id);
                    cmd.Parameters.AddWithValue("name", elem.Name);
                    cmd.Parameters.AddWithValue("damage", elem.Damage);
                    cmd.Parameters.AddWithValue("element", elem.Element);
                    cmd.Parameters.AddWithValue("type", elem.Type);
                    var rows = cmd.ExecuteNonQuery();

                    if(rows <= 0){
                        throw new Exception("Card duplicate");
                    }
                }
            }
        }
        public List<Card>? CreatePackage(List<Card> Package){
            try{
                initCardsTable(Package);
            }
            catch(Exception e){
                System.Console.WriteLine(e);
            }
            
            using var connection = new NpgsqlConnection(_connectionString);
            connection.Open();
            int index = 1;
            foreach(var elem in Package){
                if(elem.Id != null){
                    using var cmd = new NpgsqlCommand(InsertCardIntoPackageCommand, connection);
                    cmd.Parameters.AddWithValue($"card{index}_id", elem.Id);
                    
                    index++;
                }
            }
            //PackageCollection.Enqueue(Package);
            return Package;
        }
        public void initDeck(User user){
            if(user.Stack.Count > 0 && user.Stack.Any()){
                for(int i = 0; i < 4; i++){
                    tmpDeck.Add(user.Stack.ElementAt(i));
                }
                user.Deck.AddRange(tmpDeck);
                tmpDeck.Clear();
            }
        }
        public bool AquirePackage(User user){
            using var connection = new NpgsqlConnection(_connectionString);
            connection.Open();

            using var cmd = new NpgsqlCommand(GetUserCoins, connection);
            cmd.Parameters.AddWithValue("username", user.Username);
            using var reader = cmd.ExecuteReader();

            if(reader.Read()){  
                int coins = reader.GetInt32(reader.GetOrdinal("coins"));

                Console.WriteLine("Coins: " + coins);
            }
            else{
                System.Console.WriteLine("NOT OPENED");
            }


            if(user.Coins > 4 && (PackageCollection.Count >= 1)){
                user.Coins += -5;
                tmpStack.AddRange(PackageCollection.Dequeue());
                user.Stack.AddRange(tmpStack);
                tmpStack.Clear();
                return true;
            }
            else{
                return false;
            }
        }
        public List<Card>? ShowCards(User user){
            return user.Stack;
        }
        public List<Card>? ShowDeck(User user){
            if(!user.Deck.Any()){
                initDeck(user);
            }
            return user.Deck;
        }
        public List<Card>? UpdateDeck(User user, List<string> payload){
            Card forbiddenCard = new Card() {Id = "666"};
            List<Card> forbiddenDeck = new();
            tmpDeck.Clear();
            if(payload.Count == 4 && user.Stack.Any()){
                for(int i = 0; i < user.Stack.Count; i++){
                    for(int j = 0; j < payload.Count; j++){
                        if(payload.ElementAt(j) == user.Stack.ElementAt(i).Id){
                            tmpDeck.Add(user.Stack.ElementAt(i));
                        }
                    }
                }
            }
            if(tmpDeck.Count == 4){
                user.Deck.Clear();
                user.Deck.AddRange(tmpDeck);
                return user.Deck;
            }
            else if(tmpDeck.Count != 4){
                forbiddenDeck.Add(forbiddenCard);
                return forbiddenDeck;
            }
            else{ //unchanged version of user deck
                return user.Deck;
            }
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
