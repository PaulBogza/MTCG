using System;
using PlayerClass;
using CardClass;
using System.ComponentModel;

namespace GameClass{
    //sealed = class can not be inherited from or extended
    public sealed class Game{
        //private constructor to enforce the GameInstance method 
        private Game(){}
        private static Game? _GameInstance;
        //Thread safe implementation?, idk found it online
        private static readonly object _lock = new object();
        public static Game GetInstance(){
            if(_GameInstance == null){
                lock(_lock){
                    if(_GameInstance == null){
                        _GameInstance = new Game();
                    }
                }
            }
            return _GameInstance;
        }
        public int Rounds { get; set; }
        public string Winner{ get; set; } = "TBD";
        public void StartGame(Player Player1, Player Player2){
            System.Console.WriteLine("==================Monster Trading Carde Game=================");
            System.Console.WriteLine("1.) Play");
            System.Console.WriteLine("2.) Trade");
            System.Console.WriteLine("=============================================================");
            
        }
        public void Trade(Player Player1, Player Player2){
            if(Player1.Stack == null || Player2.Stack == null){
                return;
            }
            System.Console.WriteLine("==================Marketplace=================");
            System.Console.WriteLine("{0} wants to trade with {1}! Trade {2} for {3}?", Player1.Name, Player2.Name, Player1.Stack.ElementAt(1), Player2.Stack.ElementAt(1));
            System.Console.WriteLine("==============================================");
        }
    }
}