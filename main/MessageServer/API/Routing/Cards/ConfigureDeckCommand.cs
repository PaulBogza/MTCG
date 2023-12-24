using CardClass;
using Microsoft.VisualBasic;
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
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;

namespace SWE1.MessageServer.API.Routing.Messages{

    internal class ConfigureDeckCommand : IRouteCommand
    {
        private readonly ICardManager _cardManager;
        private readonly User _currentUser;
        private readonly List<string> _payload;

        public ConfigureDeckCommand(ICardManager cardManager, User currentUser, List<string> payload) //add authtoken
        {
            _cardManager = cardManager;
            _currentUser = currentUser;
            _payload = payload;
        }
        public HttpResponse Execute()
        {
            List<Card>? Deck = null;
            try{
                Deck = _cardManager.UpdateDeck(_currentUser, _payload);
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