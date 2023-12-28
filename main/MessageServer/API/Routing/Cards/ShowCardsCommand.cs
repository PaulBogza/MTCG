using Newtonsoft.Json;
using SWE1.MessageServer.BLL;
using SWE1.MessageServer.DAL;
using SWE1.MessageServer.HttpServer.Response;
using SWE1.MessageServer.HttpServer.Routing;
using SWE1.MessageServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWE1.MessageServer.API.Routing.Messages{

    internal class ShowCardsCommand : IRouteCommand
    {
        private readonly ICardManager _cardManager;
        private readonly User _currentUser;

        public ShowCardsCommand(ICardManager cardManager, User currentUser)
        {
            _cardManager = cardManager;
            _currentUser = currentUser;
        }
        public HttpResponse Execute()
        {
            string token = $"{_currentUser.Username}-mtcgToken";
            List<Card>? Stack = null;
            try{
                Stack = _cardManager.ShowCards(_currentUser);
            }
            catch (UserNotFoundException){  
                Stack = null;
            }
            HttpResponse response;
            if(Stack == null){
                response = new HttpResponse(StatusCode.NoContent);
            }
            else if(_currentUser.Token != token){
                response = new HttpResponse(StatusCode.Unauthorized);
            }
            else{
                response = new HttpResponse(StatusCode.Ok, JsonConvert.SerializeObject(Stack).ToString());
            }
            return response;
        }
    }
}