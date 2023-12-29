using System;
using Npgsql;
using System.Runtime.ConstrainedExecution;
using SWE1.MessageServer.API.Routing;
using SWE1.MessageServer.BLL;
using SWE1.MessageServer.DAL;
using SWE1.MessageServer.HttpServer;
using SWE1.MessageServer.Models;
using System.Net;

namespace myMTCG{
    class Program{
        static void Main(string[] args){
            // Careful: right now, this program will not do anything due to the null-conditional operators (but it will not crash either)
            // https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/operators/member-access-operators#null-conditional-operators--and-

            //var connectionString = "Host=localhost;Username=postgres;Password=postgres;Database=mydb";
            IGameDao gameDao = new InMemoryGameDao();
            IMessageDao messageDao = new InMemoryMessageDao();
            IUserDao userDao = new InMemoryUserDao();
            ICardDao cardDao =  new InMemoryCardDao();
            //IUserDao userDao = new DatabaseUserDao(connectionString);
            //IMessageDao messageDao = new DatabaseMessageDao(connectionString);

            IGameManager gameManager = new GameManager(gameDao);
            IMessageManager messageManager = new MessageManager(messageDao);
            IUserManager userManager = new UserManager(userDao);
            ICardManager cardManager = new CardManager(cardDao);

            var router = new MessageRouter(userManager, messageManager, cardManager, gameManager);
            var server = new HttpServer(router, IPAddress.Any, 10001);
            server.Start();
        }
    }
}