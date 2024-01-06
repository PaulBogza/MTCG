using SWE1.MessageServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWE1.MessageServer.DAL
{
    internal class InMemoryUserDao : IUserDao
    {
        private readonly List<User> _users = new();

        public bool BuyCoins(User user){
            return true;
        }
        public bool UpdateUserInfo(User user, Dictionary<string, string> userinfo){
            //user.UserInfo = UserInfo;
            return true;
        }
        public User? GetUserByAuthToken(string authToken)
        {
            return _users.SingleOrDefault(u => u.Token == authToken);
        }

        public User? GetUserByCredentials(string username, string password)
        {
            return _users.SingleOrDefault(u => u.Username == username && u.Password == password);
        }

        public bool InsertUser(User user)
        {
            bool inserted = false;

            if (GetUserByUsername(user.Username) == null)
            {
                _users.Add(user);
                inserted = true;
            }

            return inserted;
        }

        private User? GetUserByUsername(string username)
        {
            return _users.SingleOrDefault(u => u.Username == username);
        }
        public User ShowStats(User user){
            return user;
        }
        public List<UserStats> ShowScoreboard(){
            List<UserStats> Scoreboard = new();
            UserStats? stats;

            foreach(var elem in _users){
                stats = new(elem.Username, elem.Elo, elem.Wins, elem.Losses);
                Scoreboard.Add(stats);
            }

            return Scoreboard;
        }
    }
}
