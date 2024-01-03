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
        private const string CreateUserTableCommand = @"CREATE TABLE IF NOT EXISTS users (id serial primary key, username varchar, password varchar, elo int not null, 
                                                        wins int not null, losses int not null, coins int not null, userinfo text, stats text);";
        private const string CheckIfUserExists = @"SELECT username FROM users WHERE username = @username";
        private const string SelectAllUsersCommand = @"SELECT username, password FROM users";
        private const string SelectUserByCredentialsCommand = "SELECT username, password FROM users WHERE username=@username AND password=@password";
        private const string InsertUserCommand = @"INSERT INTO users(username, password, elo, wins, losses, coins) VALUES (@username, @password, @elo, @wins, @losses, @coins)";
        private const string UpdateUserInfo = @"INSERT INTO users (userinfo) WHERE username = @user.username VALUES (@UserInfo)";

        private readonly string _connectionString;

        public DatabaseUserDao(string connectionString)
        {
            _connectionString = connectionString;
            EnsureTables();
        }

        public bool UpdateUser(User user, Dictionary<string, string> UserInfo){
            using var connection = new NpgsqlConnection(_connectionString);
            connection.Open();

            using var cmd = new NpgsqlCommand(UpdateUserInfo, connection);

            cmd.Parameters.AddWithValue("userinfo", UserInfo);

            return true;
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
        public User ShowStats(User user){
            return user;
        }
        public List<UserStats> ShowScoreboard(){
            //select * from users order by elo descending
            List<UserStats> Scoreboard = new();
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

            using var cmd = new NpgsqlCommand(InsertUserCommand, connection);
            cmd.Parameters.AddWithValue("username", user.Username);
            cmd.Parameters.AddWithValue("password", user.Password);
            cmd.Parameters.AddWithValue("elo", 100);
            cmd.Parameters.AddWithValue("wins", 0);
            cmd.Parameters.AddWithValue("losses", 0);
            cmd.Parameters.AddWithValue("coins", 20);
            var affectedRows = cmd.ExecuteNonQuery();
            
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
                while (reader.Read())
                {
                    var user = ReadUser(reader);
                    users.Add(user);
                }

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
