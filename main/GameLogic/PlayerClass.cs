using System;
using System.ComponentModel.DataAnnotations;
using CardClass;
using SWE1.MessageServer.Models;

namespace PlayerClass{
    public class Player{
        public string Name = "TBD";
        private int _Elo = 100;
        public int Elo{
            get{return _Elo;}
            set{_Elo = value;}
        }
        private int _Coins = 20;
        public int Coins{
            get{return _Coins;}
            set{_Coins = value;}
        }

        public Player(int Elo, int Coins){
            _Elo = Elo;
            _Coins = Coins;
        }
        private List<Card> ?_Stack;
        public List<Card> ?Stack{
            get{return _Stack;}
            set{_Stack = value;}
        }
        private List<Card> ?_Deck;
        public List<Card> ?Deck{
            get{return _Deck;}
            set{_Deck = value;}
        }
    }
}