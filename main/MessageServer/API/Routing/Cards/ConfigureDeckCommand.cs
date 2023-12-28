using Microsoft.VisualBasic;
using Newtonsoft.Json;
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

        public ConfigureDeckCommand(ICardManager cardManager, User currentUser, List<string> payload)
        {
            _cardManager = cardManager;
            _currentUser = currentUser;
            _payload = payload;
        }
        public HttpResponse Execute()
        {
            string token = $"{_currentUser.Username}-mtcgToken";
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
            else if(_currentUser.Token != token){
                response = new HttpResponse(StatusCode.Unauthorized);
            }
            else if(Deck.ElementAt(0).Id == "666"){
                response = new HttpResponse(StatusCode.Forbidden);
            }
            else{
                response = new HttpResponse(StatusCode.Ok, JsonConvert.SerializeObject(Deck).ToString());
            }
            return response;
        }
    }
}