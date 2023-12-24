using CardClass;
using Newtonsoft.Json;
using SWE1.MessageServer.BLL;
using SWE1.MessageServer.DAL;
using SWE1.MessageServer.HttpServer.Response;
using SWE1.MessageServer.HttpServer.Routing;
using SWE1.MessageServer.Models;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace SWE1.MessageServer.API.Routing.Packages
{
    internal class CreatePackageCommand : IRouteCommand
    {
        private readonly string? _adminUserToken;
        private readonly List<Card> _payload;
        private readonly ICardManager _cardManager;

        public CreatePackageCommand(ICardManager cardManager, User currentUser, List<Card> payload)
        {
            _cardManager = cardManager;
            _adminUserToken = currentUser.Token;
            _payload = payload;
        }

        public HttpResponse Execute(){
            string adminToken = "admin-mtcgToken";
            List<Card>? Package = _payload;
            try{
                Package = _cardManager.CreatePackage(Package);
            }
            catch(UserNotFoundException){
                Package = null;
            }
            HttpResponse response;
            if(Package != null && (adminToken == _adminUserToken)){
                response = new HttpResponse(StatusCode.Created, JsonConvert.SerializeObject(Package));
            }
            else if(Package != null && adminToken == null){
                response = new HttpResponse(StatusCode.Unauthorized);
            }
            else if(Package != null && adminToken != _adminUserToken){
                response = new HttpResponse(StatusCode.Forbidden);
            }
            else{
                response = new HttpResponse(StatusCode.BadRequest);
            }
            return response;
        }
    }
}
