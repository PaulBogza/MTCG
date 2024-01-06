using SWE1.MessageServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWE1.MessageServer.DAL
{
    internal interface IUserDao
    {
        User? GetUserByAuthToken(string authToken);
        User? GetUserByCredentials(string username, string password);
        bool InsertUser(User user);
        bool UpdateUserInfo(User user, Dictionary<string, string> userinfo);
        User ShowStats(User user);
        List<UserStats> ShowScoreboard();
        bool BuyCoins(User user);
    }
}
