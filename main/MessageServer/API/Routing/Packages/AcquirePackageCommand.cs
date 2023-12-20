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

namespace SWE1.MessageServer.API.Routing.Packages
{
    internal class AquirePackageCommand : IRouteCommand
    {
        private readonly ICardManager _cardManager;
        private readonly User _currentUser;
        public AquirePackageCommand(ICardManager cardManager, User currentUser)
        {
            _cardManager = cardManager;
            _currentUser = currentUser;
        }

        public HttpResponse Execute(){
            bool success;
            
            success = _cardManager.AquirePackage(_currentUser);

            HttpResponse response;
            if(!success){
                response = new HttpResponse(StatusCode.BadRequest);
            }
            else{
                response = new HttpResponse(StatusCode.Ok);
            }
            return response;
        }
    }
}
