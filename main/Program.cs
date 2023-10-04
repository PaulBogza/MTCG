using System;
using CardClass;
using MonsterCardClass;
using SpellCardClass;
using PlayerClass;
using GameClass;
using ElementTypeEnum;
using BattleClass;
using System.Runtime.ConstrainedExecution;

namespace myMTCG{
    class Program{
        static void Main(string[] args){
            //singleton implementation
            Game newGame = Game.GetInstance();
            Battle newBattle = new Battle();
            Player Player1 = new Player(100, 20);
            Player Player2 = new Player(100, 20);

            newGame.StartGame(Player1, Player2);

            MonsterCard Goblin = new MonsterCard("WaterGoblin", 5, 10, ElementType.Water, "Goblin");
            MonsterCard Elf = new MonsterCard("FireElf", 5, 10, ElementType.Fire, "Elf");
            SpellCard FrostRay = new SpellCard("FrostRay", 10, ElementType.Water, "Spell");
            MonsterCard Dragon = new MonsterCard("Fortisax", 15, 30, ElementType.Fire, "Dragon");
            MonsterCard Knight = new MonsterCard("Black Knight", 10, 15, ElementType.Normal, "Knight");
            MonsterCard Ork = new MonsterCard("Ork", 10, 15, ElementType.Normal, "Ork");
            MonsterCard Kraken = new MonsterCard("Takoyaki", 25, 50, ElementType.Water, "Wizard");
            MonsterCard Wizard = new MonsterCard("Saruman", 20, 15, ElementType.Normal, "Kraken");

            newBattle.losingCard = newBattle.Fight(Goblin, Dragon);

            if(newBattle.losingCard == null){
                System.Console.WriteLine("Draw");
            }
            else{
                System.Console.WriteLine("{0} lost this battle",newBattle.losingCard.Name);
            }

        }
    }
}