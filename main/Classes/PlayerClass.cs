using System;
using CardClasses;

namespace PlayerClasses{
    public class Player{
        private int _elo = 100;
        public int elo{
            get{return _elo;}
            set{_elo = value;}
        }
        private int _coins = 20;
        public int coins{
            get{return _coins;}
            set{_coins = value;}
        }

        public Player(int elo, int coins){
            _elo = elo;
            _coins = coins;
        }
        private Card[] Stack = new Card[64];
        private Card[] Deck = new Card[4];

        public void Attack(Card Target){

        }

        public void Trade(Player OtherPlayer){

        }
    }
}