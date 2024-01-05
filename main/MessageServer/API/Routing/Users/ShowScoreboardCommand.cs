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
    internal class ShowScoreboardCommand : IRouteCommand
    {
        private readonly IUserManager _userManager;
        private readonly User _currentUser;
  
        public ShowScoreboardCommand(IUserManager userManager, User currentUser)
        {
            _userManager = userManager;
            _currentUser = currentUser;
        }

        public HttpResponse Execute()
        {
            string token = $"{_currentUser.Username}-mtcgToken";
            //string adminToken = "admin-mtcgToken";
            List<UserStats> ?Scoreboard;
            try
            {
                Scoreboard = _userManager.ShowScoreboard();
            }
            catch 
            {
                Scoreboard = null;
            }

            HttpResponse response;
            if (Scoreboard == null)
            {
                response = new HttpResponse(StatusCode.BadRequest);
            }
            else if(_currentUser.Token != token){
                response = new HttpResponse(StatusCode.Unauthorized);
            }
            else
            {   
                response = new HttpResponse(StatusCode.Ok, JsonConvert.SerializeObject(Scoreboard, Formatting.Indented).ToString());
            }

            return response;
        }

    }
}
