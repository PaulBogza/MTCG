using Newtonsoft.Json;
using SWE1.MessageServer.BLL;
using SWE1.MessageServer.HttpServer.Response;
using SWE1.MessageServer.HttpServer.Routing;
using SWE1.MessageServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWE1.MessageServer.API.Routing.Users
{
    internal class RegisterCommand : IRouteCommand
    {
        private readonly IUserManager _userManager;
        private readonly Credentials _credentials;

        public RegisterCommand(IUserManager userManager, Credentials credentials)
        {
            _userManager = userManager;
            _credentials = credentials;
        }

        public HttpResponse Execute()
        {
            HttpResponse response;

            try
            {
                _userManager.RegisterUser(_credentials);
                response = new HttpResponse(StatusCode.Created, JsonConvert.SerializeObject(_credentials.Username).ToString());
            }
            catch (DuplicateUserException)
            {
                response = new HttpResponse(StatusCode.Conflict);
            }

            return response;
        }
    }
}
