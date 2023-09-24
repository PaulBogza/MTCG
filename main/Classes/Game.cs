using System;
using PlayerClasses;
using CardClasses;

namespace Game{
    public class GameClass{
        private int _rounds;
        public int rounds{
            get{return _rounds;}
            set{_rounds = value;}
        }

        private string _winner;
        public string winner{
            get{return _winner;}
            set{_winner = value;}
        }
    }

    public void StartGame(Player Player1, Player Player2){
        System.Console.WriteLine("Get ready for the next battle");
    }
}