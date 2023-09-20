using System;
using CardClasses;
using PlayerClasses;

namespace myMTCG{
    class Program{
        static void Main(string[] args){
            MonsterCard GigaGoblin = new MonsterCard(5, 10, "Fire", "Goblin");
            Print(GigaGoblin);
        }
        static void Print(MonsterCard GigaGoblin){
            Console.WriteLine(GigaGoblin.healthPoints);
        }
    }
}