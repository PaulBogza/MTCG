using Newtonsoft.Json;
using SWE1.MessageServer.BLL;
using SWE1.MessageServer.DAL;
using SWE1.MessageServer.HttpServer.Response;
using SWE1.MessageServer.HttpServer.Routing;
using SWE1.MessageServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;

namespace SWE1.MessageServer.API.Routing.Cards{

    internal class ShowDeckCommand : IRouteCommand
    {
        private readonly ICardManager _cardManager;
        private readonly User _currentUser;
        private readonly string _state;
        public ShowDeckCommand(ICardManager cardManager, User currentUser, string state)
        {
            _cardManager = cardManager;
            _currentUser = currentUser;
            _state = state;
        }
        public HttpResponse Execute()
        {   
            string token = $"{_currentUser.Username}-mtcgToken";
            List<Card>? Deck;
            try{
                Deck = _cardManager.ShowDeck(_currentUser);
            }
            catch (Exception e){
                System.Console.WriteLine(e);  
                Deck = null;
            }
            HttpResponse response;
            if(Deck == null || Deck.Count == 0){
                response = new HttpResponse(StatusCode.NoContent);
            }
            else if(_currentUser.Token != token){
                response = new HttpResponse(StatusCode.Unauthorized);
            }
            else if(_state == "text"){
                response = new HttpResponse(StatusCode.Ok, _currentUser.ToString());
            }
            else{
                response = new HttpResponse(StatusCode.Ok, JsonConvert.SerializeObject(Deck, Formatting.Indented).ToString());
            }
            return response;
        }
    }
}