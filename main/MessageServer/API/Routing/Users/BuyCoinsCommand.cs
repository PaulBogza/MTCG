using Newtonsoft.Json;
using SWE1.MessageServer.BLL;
using SWE1.MessageServer.HttpServer.Response;
using SWE1.MessageServer.HttpServer.Routing;
using SWE1.MessageServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace SWE1.MessageServer.API.Routing.Users
{
    internal class BuyCoinsCommand : IRouteCommand
    {
        private readonly IUserManager _userManager;
        private readonly User _currentUser;

        public BuyCoinsCommand(IUserManager userManager, User currentUser)
        {
            _userManager = userManager;
            _currentUser = currentUser;
        }

        public HttpResponse Execute()
        {
            bool success = new();
            try
            {
                success = _userManager.BuyCoins(_currentUser);
            }
            catch (Exception e)
            {
                System.Console.WriteLine(e);
            }

            HttpResponse response;
            if (!success)
            {
                response = new HttpResponse(StatusCode.InternalServerError);
            }
            else
            {   
                response = new HttpResponse(StatusCode.Ok, JsonConvert.SerializeObject($"{_currentUser.Username} bought {_currentUser.Coins} more coins", Formatting.Indented).ToString());
            }

            return response;
        }

    }
}
