using System;
using CardClasses;

namespace PlayerClasses{
    class Player{
        private int ELO {get; set;} = 100;
        private int coins {get; set;} = 20;
        private Card[] Stack = new Card[64];
        private Card[] Deck = new Card[4];
    }
}