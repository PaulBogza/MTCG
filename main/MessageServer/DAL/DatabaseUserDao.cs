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
    internal class DatabaseUserDao : IUserDao
    {
        private const string CreateUserTableCommand = @"CREATE TABLE IF NOT EXISTS users (id serial PRIMARY KEY, username varchar, password varchar, elo int, 
                                                        wins int, losses int, coins int, userinfo text, stats text);";
        private const string CheckIfUserExists = @"SELECT username FROM users WHERE username = @username";
        private const string SelectAllUsersCommand = @"SELECT username, password FROM users";
        private const string SelectUserByCredentialsCommand = "SELECT * FROM users WHERE username=@username AND password=@password";
        private const string InsertUserCommand = @"INSERT INTO users(username, password, elo, wins, losses, coins) VALUES (@username, @password, @elo, @wins, @losses, @coins)";
        private const string UpdateUserCommand = @"UPDATE users SET elo=@elo, wins=@wins, losses=@losses WHERE username=@username";
        private const string UpdateUserInfoCommand = @"UPDATE users SET userinfo=@userinfo WHERE username=@username";
        private const string UpdateUserStatsCommand = @"UPDATE users SET stats=@stats WHERE username=@username";
        private const string SelectStatsCommand = @"SELECT * FROM users WHERE username=@username";
        private const string SelectInfoCommand = @"SELECT userinfo FROM users WHERE username=@username";
        private const string SelectScoreboardCommand = @"SELECT * FROM users ORDER BY elo DESC";

        private readonly string _connectionString;
        private static readonly object _lockObject = new();
        public DatabaseUserDao(string connectionString)
        {
            _connectionString = connectionString;
            EnsureTables();
        }

        public bool UpdateUser(User user){
            return false;
        }
        public User? GetUserByAuthToken(string authToken)
        {
            return GetAllUsers().SingleOrDefault(u => u.Token == authToken);
        }

        public User? GetUserByCredentials(string username, string password)
        {
            User? user = null;

            using var connection = new NpgsqlConnection(_connectionString);
            connection.Open();

            using var cmd = new NpgsqlCommand(SelectUserByCredentialsCommand, connection);
            cmd.Parameters.AddWithValue("username", username);
            cmd.Parameters.AddWithValue("password", password);

            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                user = ReadUser(reader);
                return user;
            }
            else{
                throw new UserNotFoundException();
            }
        }
        public bool UpdateUserInfo(User user, Dictionary<string, string> userinfo){
            try{
                using var connection = new NpgsqlConnection(_connectionString);
                connection.Open();

                using var cmd = new NpgsqlCommand(UpdateUserInfoCommand, connection);
                cmd.Parameters.AddWithValue("userinfo", userinfo);
                cmd.Parameters.AddWithValue("username", user.Username);
                var result = cmd.ExecuteNonQuery();
                if(result > 0){
                    return true;
                }
                return false;
            }
            catch(Exception e){
                System.Console.WriteLine(e);
                return false;
            }
        }
        public User ShowStats(User user){
            try{
                using var connection = new NpgsqlConnection(_connectionString);
                connection.Open();
                
                using var cmd = new NpgsqlCommand(SelectStatsCommand, connection);
                cmd.Parameters.AddWithValue("username", user.Username);
                using var reader = cmd.ExecuteReader();
                
                while(reader.Read()){
                    user.Stats["Name"] = reader["username"];
                    user.Stats["Elo"] = reader["elo"];
                    user.Stats["Wins"] = reader["wins"];
                    user.Stats["Losse"] = reader["losses"];
                }
            }
            catch(Exception e){
                System.Console.WriteLine(e);
            }
            return user;
        }
        public User ShowUserinfo(User user){
            try{
                using var connection = new NpgsqlConnection(_connectionString);
                connection.Open();
                
                using var cmd = new NpgsqlCommand(SelectInfoCommand, connection);
                cmd.Parameters.AddWithValue("username", user.Username);
                using var reader = cmd.ExecuteReader();
                
                if(reader.Read()){
                    user.UserInfo = (Dictionary<string, string>)reader["userinfo"];
                }
            }
            catch(Exception e){
                System.Console.WriteLine(e);
            }
            return user;
        }
        public List<UserStats> ShowScoreboard(){
            List<UserStats> Scoreboard = new();
            try{
                using var connection = new NpgsqlConnection(_connectionString);
                connection.Open();
                
                using var cmd = new NpgsqlCommand(SelectScoreboardCommand, connection);
                using var reader = cmd.ExecuteReader();
                
                string username;
                int elo;
                int wins;
                int losses;
                while(reader.Read()){
                    username = (string)reader["username"];
                    elo = (int)reader["elo"];
                    wins = (int)reader["wins"];
                    losses = (int)reader["losses"];
                    UserStats temp = new(username, elo, wins, losses);
                    Scoreboard.Add(temp);
                }   
            }
            catch(Exception e){
                System.Console.WriteLine(e);
            }
            return Scoreboard;
        }
        public bool InsertUser(User user)
        {
            using var connection = new NpgsqlConnection(_connectionString);
            connection.Open();
            
            using var check = new NpgsqlCommand(CheckIfUserExists, connection);
            check.Parameters.AddWithValue("username", user.Username);
            var rows = check.ExecuteReader();
            if(rows.Read()){
                throw new DuplicateUserException();
            }
            
            using var connection2 = new NpgsqlConnection(_connectionString);
            connection2.Open();
            
            using var cmd = new NpgsqlCommand(InsertUserCommand, connection2);
            cmd.Parameters.AddWithValue("username", user.Username);
            cmd.Parameters.AddWithValue("password", user.Password);
            cmd.Parameters.AddWithValue("elo", 100);
            cmd.Parameters.AddWithValue("wins", 0);
            cmd.Parameters.AddWithValue("losses", 0);
            cmd.Parameters.AddWithValue("coins", 20);
            var affectedRows = cmd.ExecuteNonQuery(); //gets stuck here
            
            if(affectedRows > 0){
                return affectedRows > 0;
            }
            else{
                throw new DuplicateUserException();
            }

        }

        private void EnsureTables()
        {
            using var connection = new NpgsqlConnection(_connectionString);
            connection.Open();
            using var cmd = new NpgsqlCommand(CreateUserTableCommand, connection);
            cmd.ExecuteNonQuery();
        }

        private IEnumerable<User> GetAllUsers() 
        {
            var users = new List<User>();

            using var connection = new NpgsqlConnection(_connectionString);
            connection.Open();

            using var cmd = new NpgsqlCommand(SelectAllUsersCommand, connection);
            using var reader = cmd.ExecuteReader();
            if(reader.Read()){
                do{
                    var user = ReadUser(reader);
                    users.Add(user);
                }while (reader.Read());

                return users;
            }
            else{
                throw new UserNotFoundException();
            }
        }

        private User ReadUser(IDataRecord record)
        {
            var username = Convert.ToString(record["username"])!;
            var password = Convert.ToString(record["password"])!;

            return new User(username, password);
        }
    }
}
