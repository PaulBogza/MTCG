using System;
using Npgsql;
using System.Runtime.ConstrainedExecution;
using SWE1.MessageServer.API.Routing;
using SWE1.MessageServer.BLL;
using SWE1.MessageServer.DAL;
using SWE1.MessageServer.HttpServer;
using SWE1.MessageServer.Models;
using System.Net;
using System.Runtime.ExceptionServices;

namespace myMTCG{
    class Program{
        static void Main(string[] args){
            // Careful: right now, this program will not do anything due to the null-conditional operators (but it will not crash either)
            // https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/operators/member-access-operators#null-conditional-operators--and-

            var connectionString = "Host=localhost;Username=postgres;Password=postgres;Database=mydb";
            var conn = new NpgsqlConnection(connectionString);
            conn.Open();
            using var cmd = new NpgsqlCommand("DROP TABLE IF EXISTS packages; DROP TABLE IF EXISTS deck; DROP TABLE IF EXISTS cards; DROP TABLE IF EXISTS users;", conn);
            cmd.ExecuteNonQuery();

            IGameDao gameDao = new DatabaseGameDao(connectionString);
            IUserDao userDao = new DatabaseUserDao(connectionString);
            ICardDao cardDao =  new DatabaseCardDao(connectionString);
            
            IGameManager gameManager = new GameManager(gameDao);
            IUserManager userManager = new UserManager(userDao);
            ICardManager cardManager = new CardManager(cardDao);

            var router = new MessageRouter(userManager, cardManager, gameManager);
            var server = new HttpServer(router, IPAddress.Any, 10001);
            server.Start();


        }
    }
}