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
        }
        public void Trade(Player Player1, Player Player2){
        }
    }
}