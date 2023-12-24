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
    internal class LoginCommand : IRouteCommand
    {
        private readonly IUserManager _userManager;
        private readonly Credentials _credentials;
        private readonly ICardManager _cardManager;

        public LoginCommand(IUserManager userManager, Credentials credentials, ICardManager cardManager)
        {
            _credentials = credentials;
            _userManager = userManager;
            _cardManager = cardManager;
        }

        public HttpResponse Execute()
        {
            User? user;
            try
            {
                user = _userManager.LoginUser(_credentials, _cardManager);
            }
            catch (UserNotFoundException)
            {
                user = null;
            }

            HttpResponse response;
            if (user == null)
            {
                response = new HttpResponse(StatusCode.Unauthorized);
            }
            else
            {   
                _cardManager.initDeck(user);
                response = new HttpResponse(StatusCode.Ok, JsonConvert.SerializeObject(user.Username).ToString());
            }

            return response;
        }

    }
}
