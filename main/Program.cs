using System;
using CardClass;
using PlayerClass;
using GameClass;

namespace myMTCG{
    class Program{
        static void Main(string[] args){
            //singleton implementation
            Game newGame = Game.GetInstance();

            Player Player1 = new Player(100, 20);
            Player Player2 = new Player(100, 20);

            newGame.StartGame(Player1, Player2);

            MonsterCard Goblin = new MonsterCard("WaterGoblin", 5, 10, ElementType.Water, "Goblin");
            MonsterCard Elf = new MonsterCard("FireElf", 5, 10, ElementType.Fire, "Elf");
            SpellCard FrostRay = new SpellCard("FrostRay", 10, ElementType.Water, "Spell");
            MonsterCard Dragon = new MonsterCard("Fortisax", 15, 30, ElementType.Fire, "Dragon");
            MonsterCard Knight = new MonsterCard("Black Knight", 10, 15, ElementType.Normal, "Knight");
            MonsterCard Ork = new MonsterCard("Ork", 10, 15, ElementType.Normal, "Ork");
            MonsterCard Kraken = new MonsterCard("Takoyaki", 25, 50, ElementType.Water, "Special");
            MonsterCard Wizard = new MonsterCard("Saruman", 20, 15, ElementType.Normal, "Special");

            FrostRay.Attack(Goblin);
            Goblin.Attack(Elf);
        }
    }
}