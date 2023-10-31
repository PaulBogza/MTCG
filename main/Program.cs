using System;
using CardClass;
using MonsterCardClass;
using SpellCardClass;
using PlayerClass;
using GameClass;
using ElementTypeEnum;
using BattleClass;
using Npgsql;
using System.Runtime.ConstrainedExecution;
using SWE1.MessageServer.API.Routing;
using SWE1.MessageServer.BLL;
using SWE1.MessageServer.DAL;
using SWE1.MessageServer.HttpServer;
using SWE1.MessageServer.Models;
using System.Net;


//TRY RACE ENUM INSTEAD OF TYPE STRINGP

namespace myMTCG{
    class Program{
        static void Main(string[] args){
            // Careful: right now, this program will not do anything due to the null-conditional operators (but it will not crash either)
            // https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/operators/member-access-operators#null-conditional-operators--and-

            var connectionString = "Host=localhost;Username=postgres;Password=postgres;Database=mydb";
            //IMessageDao messageDao = new InMemoryMessageDao();
            //IUserDao userDao = new InMemoryUserDao();
            IUserDao userDao = new DatabaseUserDao(connectionString);
            IMessageDao messageDao = new DatabaseMessageDao(connectionString);

            IMessageManager messageManager = new MessageManager(messageDao);
            IUserManager userManager = new UserManager(userDao);

            /*
            // Test: Register User
            var credentials = new Credentials("user1", "pass");
            userManager.RegisterUser(credentials);
            var registeredUser = userDao.GetUserByCredentials(credentials.Username, credentials.Password);
            Console.WriteLine($"Username: {registeredUser?.Username}, Password: {registeredUser?.Password}");

            // Test: Login User
            var user = userManager.LoginUser(credentials);
            Console.WriteLine($"Username: {user.Username}, Token: {user.Token}");

            // Test: Add Messages
            var contents = Enumerable.Range(1, 10).Select(i => $"message {i}").ToList();
            contents.ForEach(m => messageManager.AddMessage(user, m));
            messageDao.GetMessages(user.Username).ToList().ForEach(m => Console.WriteLine($"Id: {m.Id}, Content: {m.Content}"));

            // Test: List Messages
            var messages = messageManager.ListMessages(user).ToList();
            messages.ForEach(m => Console.WriteLine($"Id: {m.Id}, Content: {m.Content}"));

            // Test: Update Message
            messageManager.UpdateMessage(user, 2, "new message 2");

            // Test: Show Message
            var message = messageManager.ShowMessage(user, 2);
            Console.WriteLine($"Id: {message.Id}, Content: {message.Content}");

            // Test: Delete Message & List Messages
            messageManager.RemoveMessage(user, 2);
            messages = messageManager.ListMessages(user).ToList();
            messages.ForEach(m => Console.WriteLine($"Id: {m.Id}, Content: {m.Content}"));
            */
            var router = new MessageRouter(userManager, messageManager);
            var server = new HttpServer(router, IPAddress.Any, 10001);
            server.Start(); 
        }
    }
}