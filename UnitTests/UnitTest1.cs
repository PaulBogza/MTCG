using NUnit.Framework;
using GameClass;
using CardClass;
using BattleClass;
using PlayerClass;
using MonsterCardClass;
using  SpellCardClass;
using ElementTypeEnum;
using SWE1.MessageServer.Models;
using SWE1.MessageServer.API.Routing;
using SWE1.MessageServer.BLL;
using SWE1.MessageServer.DAL;
using SWE1.MessageServer.HttpServer;
/*
    MonsterCard Goblin = new MonsterCard(1, "WaterGoblin", 1, ElementType.Water, "Goblin");
    MonsterCard Elf = new MonsterCard(1, "FireElf", 1, ElementType.Fire, "Elf");
    SpellCard FrostRay = new SpellCard(1, "FrostRay", 1, ElementType.Water, "Spell");
    MonsterCard Dragon = new MonsterCard(1, "Fortisax", 1, ElementType.Fire, "Dragon");
    MonsterCard Knight = new MonsterCard(1, "Black Knight", 1, ElementType.Normal, "Knight");
    MonsterCard Ork = new MonsterCard(1, "Ork", 1, ElementType.Normal, "Ork");
    MonsterCard Kraken = new MonsterCard(1, "Takoyaki", 1, ElementType.Water, "Kraken");
    MonsterCard Wizard = new MonsterCard(1, "Saruman", 1, ElementType.Normal, "Wizard");
    MonsterCard FireElf = new MonsterCard(1, "FireElf", 1 ,ElementType.Fire, "FireElf");
*/

namespace UnitTests{
    public class Tests
    {
        [Test]
        public void Test1()
        {
            Assert.Pass();
        }
        
        [Test]
        public void FireElf_Dragon_SpecialInteraction_Test(){
            //Game newGame = Game.GetInstance();
            Card? losingCard = new();
            Battle newBattle = new Battle();
            Player Player1 = new Player(100, 20);
            Player Player2 = new Player(100, 20);

            //newGame.StartGame(Player1, Player2);

            MonsterCard Dragon = new MonsterCard(1, "Fortisax", 10.0 , ElementType.Fire, "Dragon");
            MonsterCard FireElf = new MonsterCard(2, "FireElf", 5.0 ,ElementType.Fire, "FireElf");

            Dragon.Damage = 10.0;
            FireElf.Damage = 5.0;

            losingCard = newBattle.Fight(Dragon, FireElf);

            Assert.AreSame(Dragon.Name, losingCard.Name);
        }

    }
}