using SWE1.MessageServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWE1.MessageServer.BLL
{
    internal interface IUserManager
    {
        User LoginUser(Credentials credentials, ICardManager cardManager);
        void RegisterUser(Credentials credentials);
        User GetUserByAuthToken(string authToken);
        bool UpdateUser(User user, Dictionary<string, string> UserInfo);
        User ShowStats(User user);
        List<UserStats> ShowScoreboard();
    }
}
