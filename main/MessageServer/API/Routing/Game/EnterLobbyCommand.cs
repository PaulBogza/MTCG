using Microsoft.VisualBasic;
using Newtonsoft.Json;
using SWE1.MessageServer.BLL;
using SWE1.MessageServer.HttpServer.Response;
using SWE1.MessageServer.HttpServer.Routing;
using SWE1.MessageServer.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace SWE1.MessageServer.API.Routing.Game
{
    internal class EnterLobbyCommand : IRouteCommand
    {
        private readonly ICardManager _cardManager;
        private readonly IGameManager _gameManager;
        private readonly User _player;
        private List<string> _log = new();
        private static readonly ConcurrentDictionary<User, List<string>>_battleLog = new();
        private static readonly ConcurrentQueue<User> _waitingRoom = new(); 
        private static readonly object _lockObject = new();
        public EnterLobbyCommand(ICardManager cardManager, IGameManager gameManager, User player)
        {
            _cardManager = cardManager;
            _gameManager = gameManager;
            _player = player;
               foreach(var item in player.Deck){
                                System.Console.WriteLine(item);
                            }
            _waitingRoom.Enqueue(player);
        }

        public HttpResponse Execute()
        {
            while(true){
                lock(_lockObject){  
                    try{
                        if(_waitingRoom.Count >= 2){            
                            _waitingRoom.TryDequeue(out var player1);
                            _waitingRoom.TryDequeue(out var player2);
                         
                            if(player1 != null && player2 != null){
                                player1.Deck = _cardManager.ParseCards(player1);
                                player2.Deck = _cardManager.ParseCards(player2);
                                _log = _gameManager.StartGame(player1, player2);
                                _battleLog.TryAdd(player1, _log);
                                _battleLog.TryAdd(player2, _log);
                            }
                        }
                    }
                    catch(Exception e){
                        System.Console.WriteLine(e);
                        return new HttpResponse(StatusCode.InternalServerError);
                    }
                }
                if(_battleLog.TryGetValue(_player, out var result) && result != null){
                    _battleLog.Remove(_player, out _);
                    return new HttpResponse(StatusCode.Ok, JsonConvert.SerializeObject(_log, Formatting.Indented).ToString());
                }
                
                Thread.Sleep(100);
            }
        }
    }
}
