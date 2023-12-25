using Microsoft.VisualBasic;
using Newtonsoft.Json;
using SWE1.MessageServer.BLL;
using SWE1.MessageServer.HttpServer.Response;
using SWE1.MessageServer.HttpServer.Routing;
using SWE1.MessageServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace SWE1.MessageServer.API.Routing.Users
{
    internal class ShowStatsCommand : IRouteCommand
    {
        private readonly IUserManager _userManager;
        private readonly User _currentUser;
  
        public ShowStatsCommand(IUserManager userManager, User currentUser)
        {
            _userManager = userManager;
            _currentUser = currentUser;
        }

        public HttpResponse Execute()
        {
            string token = $"{_currentUser.Username}-mtcgToken";
            //string adminToken = "admin-mtcgToken";
            Dictionary<string, string> ?Stats = new();
            User? user;
            try
            {
                user = _userManager.ShowStats(_currentUser);
            }
            catch (UserNotFoundException)
            {
                user = null;
            }

            HttpResponse response;
            if (user == null)
            {
                response = new HttpResponse(StatusCode.NotFound);
            }
            else if(_currentUser.Token != token){
                response = new HttpResponse(StatusCode.Unauthorized);
            }
            else
            {   
                Stats?.Add("Name:" , _currentUser.Username);
                Stats?.Add("Elo:", _currentUser.Elo.ToString());
                Stats?.Add("Wins:", _currentUser.Wins.ToString());
                Stats?.Add("Losses:", _currentUser.Losses.ToString());
                response = new HttpResponse(StatusCode.Ok, JsonConvert.SerializeObject(Stats).ToString());
            }

            return response;
        }

    }
}
