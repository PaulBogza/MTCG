using System;
using CardClasses;
using PlayerClasses;
using GameClass;

namespace myMTCG{
    class Program{
        static void Main(string[] args){
            //singleton implementation
            Game newGame = Game.GameInstance("value");
            System.Console.WriteLine(newGame.value);

            Player Player1 = new Player(100, 20);
            Player Player2 = new Player(100, 20);

            MonsterCard Goblin = new MonsterCard("Goblin", 5, 10, ElementType.Water, "Goblin");
            MonsterCard Lizard = new MonsterCard("Lizard", 5, 10, ElementType.Fire, "Reptilian");
            SpellCard FrostRay = new SpellCard("FrostRay", 10, ElementType.Water, "Magic");

            
        }
    }
}