using System;
using PlayerClasses;
using CardClasses;
using System.ComponentModel;

namespace GameClass{
    //sealed = class can not be inherited from or extended
    public sealed class Game{
        //private constructor to enforce the GameInstance method 
        private Game(){}
        private static Game _gameInstance;
        //Thread safe implementation?, idk found it online
        private static readonly object _lock = new object();
        public static Game GameInstance(string value){
            if(_gameInstance == null){
                lock(_lock){
                    if(_gameInstance == null){
                        _gameInstance = new Game();
                        _gameInstance.value = value;
                    }
                }
            }
            return _gameInstance;
        }
        public string value { get; set; }
        
        private int _rounds;
        public int rounds{
            get{return _rounds;}
            set{_rounds = value;}
        }

        private string _winner = "TBD";
        public string winner{
            get{return _winner;}
            set{_winner = value;}
        }
        public void StartGame(Player Player1, Player Player2){
            System.Console.WriteLine("Get ready for the next battle");
        }
    }
}