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
        private static readonly ConcurrentDictionary<User, User?>_results = new();
        private static readonly ConcurrentQueue<User> _waitingRoom = new(); 
        private static readonly object _lockObject = new();
        public EnterLobbyCommand(ICardManager cardManager, IGameManager gameManager, User player)
        {
            _cardManager = cardManager;
            _gameManager = gameManager;
            _player = player;
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
                                var Winner = _gameManager.StartGame(player1, player2);
                                _results.TryAdd(player1, Winner);
                                _results.TryAdd(player2, Winner);
                            }
                        }
                    }
                    catch(Exception e){
                        System.Console.WriteLine(e);
                        return new HttpResponse(StatusCode.InternalServerError);
                    }
                }
                if(_results.TryGetValue(_player, out var result) && result != null){
                    _results.Remove(_player, out _);
                    return new HttpResponse(StatusCode.Ok, (result.Username != "") ? $"{result.Username} won the battle" : "Draw");
                }
                
                Thread.Sleep(100);
            }
        }
    }
}
