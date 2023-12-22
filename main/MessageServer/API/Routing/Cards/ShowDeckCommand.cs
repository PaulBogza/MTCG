using CardClass;
using Newtonsoft.Json;
using PlayerClass;
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

    internal class ShowDeckCommand : IRouteCommand
    {
        private readonly ICardManager _cardManager;
        private readonly User _currentUser;

        public ShowDeckCommand(ICardManager cardManager, User currentUser)
        {
            _cardManager = cardManager;
            _currentUser = currentUser;
        }
        public HttpResponse Execute()
        {   
            _cardManager.initDeck(_currentUser);
            List<Card>? Deck = null;
            try{
                Deck = _cardManager.ShowDeck(_currentUser);
            }
            catch (UserNotFoundException){  
                Deck = null;
            }
            HttpResponse response;
            if(Deck == null){
                response = new HttpResponse(StatusCode.BadRequest);
            }
            else{
                response = new HttpResponse(StatusCode.Ok, JsonConvert.SerializeObject(Deck).ToString());
            }
            return response;
        }
    }
}