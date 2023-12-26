using SWE1.MessageServer.DAL;
using SWE1.MessageServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWE1.MessageServer.BLL
{
    internal class UserManager : IUserManager
    {
        private readonly IUserDao _userDao;

        public UserManager(IUserDao userDao)
        {
            _userDao = userDao;
        }

        public bool UpdateUser(User user, Dictionary<string, string> ?UserInfo){
            return _userDao.UpdateUser(user, UserInfo);
        }
        public User GetUserByAuthToken(string authToken)
        {
            return _userDao.GetUserByAuthToken(authToken) ?? throw new UserNotFoundException();
        }

        public User LoginUser(Credentials credentials, ICardManager cardManager)
        {
            return _userDao.GetUserByCredentials(credentials.Username, credentials.Password) ?? throw new UserNotFoundException();
        }

        public void RegisterUser(Credentials credentials)
        {
            var user = new User(credentials.Username, credentials.Password);
            if (_userDao.InsertUser(user) == false)
            {
                throw new DuplicateUserException();
            }
        }
        public User ShowStats(User user){
            return _userDao.ShowStats(user);
        }
        public List<UserStats> ShowScoreboard(){
            return _userDao.ShowScoreboard();
        }
    }
}
