using System;
using System.Globalization;
using CardClasses;
using PlayerClasses;

namespace myMTCG{
    class Program{
        static void Main(string[] args){
            Player Player1 = new Player(100, 20);

            MonsterCard Goblin = new MonsterCard("Goblin", 5, 10, ElementType.Water, "Goblin");
            MonsterCard Lizard = new MonsterCard("Lizard", 5, 10, ElementType.Fire, "Reptilian");
            SpellCard FrostRay = new SpellCard("FrostRay", 10, ElementType.Water, "Magic");

            Goblin.Attack(Lizard);
            FrostRay.Attack(Goblin);
        }
    }
}