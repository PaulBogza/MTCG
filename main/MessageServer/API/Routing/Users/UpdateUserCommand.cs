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
using System.Xml;

namespace SWE1.MessageServer.API.Routing.Users
{
    internal class UpdateUserCommand : IRouteCommand
    {
        private User _currentUser;
        private string? _parameter;
        private string _payload;
        private readonly IUserManager _userManager;
        private readonly Credentials? _credentials;

        public UpdateUserCommand(IUserManager userManager, User currentUser, string payload, string? parameter)
        {
            _userManager = userManager;
            _currentUser = currentUser;
            _payload = payload;
            _currentUser.UserInfo = JsonConvert.DeserializeObject<Dictionary<string, string>>(_payload)!;
            _parameter = parameter;
        }

        public HttpResponse Execute()
        {
            string token = $"{_parameter}-mtcgToken";
            string adminToken = "admin-mtcgToken";

            User? user;
            try
            {   
                user = _userManager.GetUserByAuthToken(_currentUser.Token);
            }
            catch (UserNotFoundException)
            {
                user = null;
            }

            HttpResponse response;
            if (user != null && (token == user.Token || adminToken == user.Token))
            {
                _userManager.UpdateUserInfo(_currentUser, _currentUser.UserInfo);
                response = new HttpResponse(StatusCode.Ok, JsonConvert.SerializeObject(_currentUser.UserInfo, Newtonsoft.Json.Formatting.Indented).ToString());
            }
            else if(user == null){
                response = new HttpResponse(StatusCode.NotFound);
            }
            else
            {
                response = new HttpResponse(StatusCode.Unauthorized);
            }

            return response;
        }

    }
}
