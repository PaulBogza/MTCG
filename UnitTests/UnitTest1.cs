using NUnit.Framework;
using SWE1.MessageServer.Models;
using SWE1.MessageServer.API.Routing;
using SWE1.MessageServer.BLL;
using SWE1.MessageServer.DAL;
using SWE1.MessageServer.HttpServer;

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
            Card? losingCard;
            IGameDao gameDao = new InMemoryGameDao();
            IGameManager gameManager = new GameManager(gameDao);

            MonsterCard Dragon = new MonsterCard("1", "Fortisax", 10.0 , ElementType.Fire, "Dragon");
            MonsterCard FireElf = new MonsterCard("2", "FireElf", 5.0 ,ElementType.Fire, "FireElf");

            losingCard = gameManager.Fight(Dragon, FireElf);

            Assert.That(losingCard?.Name, Is.SameAs(Dragon.Name));
        }

       [Test]
        public void Kraken_Immune_To_Spells_Test(){
            Card? losingCard;
            IGameDao gameDao = new InMemoryGameDao();
            IGameManager gameManager = new GameManager(gameDao);

            SpellCard FireBolt = new SpellCard("1", "FireBolt", 7.5, ElementType.Fire, "Spell");
            MonsterCard Kraken = new MonsterCard("69", "Kraken", 15.0, ElementType.Water, "Kraken");

            losingCard = gameManager.Fight(Kraken, FireBolt);

            Assert.That(losingCard?.Name, Is.SameAs(FireBolt.Name));
        }

        [Test]
        public void Knights_Drown_To_WaterSpells(){
            //Card? losingCard;
            double tmpdmg;
            IGameDao gameDao = new InMemoryGameDao();
            IGameManager gameManager = new GameManager(gameDao);

            SpellCard WaterSpell = new SpellCard("1", "WaterSpell", 10.0, ElementType.Water, "Spell");
            MonsterCard Knight = new MonsterCard("1", "Knight", 15.0, ElementType.Normal, "Knight");

            tmpdmg = gameManager.checkSpecialInteraction(WaterSpell, Knight);

            Assert.That(tmpdmg, Is.EqualTo(WaterSpell.Damage*1000));
        }
        [Test]
        public void Wizard_Controlling_Ork_Test(){
            Card? losingCard;            
            IGameDao gameDao = new InMemoryGameDao();
            IGameManager gameManager = new GameManager(gameDao);  
            MonsterCard Wizard = new MonsterCard("1", "Wizard", 20.0, ElementType.Normal, "Wizard");
            MonsterCard Ork = new MonsterCard("1", "Ork", 10.0, ElementType.Normal, "Ork");

           losingCard = gameManager.Fight(Wizard, Ork);

            Assert.That(losingCard?.Name, Is.SameAs(Ork.Name));
        }
        [Test]
        public void Goblin_Afraid_Of_Dragon_Test(){
            Card? losingCard;            
            IGameDao gameDao = new InMemoryGameDao();
            IGameManager gameManager = new GameManager(gameDao);  
            MonsterCard Goblin = new MonsterCard("1", "Goblin", 5.0, ElementType.Normal, "Goblin");
            MonsterCard Dragon = new MonsterCard("1", "Dragon", 15.0, ElementType.Fire, "Dragon");

            losingCard = gameManager.Fight(Dragon, Goblin);

            Assert.That(losingCard?.Name, Is.SameAs(Goblin.Name));
        }

        [Test]
        public void Draw_Is_Possible_Test(){
            Card? losingCard;            
            
            IGameDao gameDao = new InMemoryGameDao();
            IGameManager gameManager = new GameManager(gameDao);  

            MonsterCard Goblin = new MonsterCard("1", "Goblin", 5.0, ElementType.Water, "Goblin");
            MonsterCard Elf = new MonsterCard("1", "Elf", 5.0, ElementType.Normal, "Elf");

            losingCard = gameManager.Fight(Goblin, Elf);

            Assert.That(losingCard, Is.SameAs(null));
        }

        [Test]
        public void Check_Water_Double_Damage_Against_Fire_Test(){
            double tmpDmg;            
            IGameDao gameDao = new InMemoryGameDao();
            IGameManager gameManager = new GameManager(gameDao);              
            SpellCard WaterSpell = new SpellCard("1", "WaterSpell", 9.0, ElementType.Water, "WaterSpell");
            SpellCard FireSpell = new SpellCard("1", "FireSpell", 10.0, ElementType.Fire, "FireSpell");

      
            tmpDmg = gameManager.checkEffect(WaterSpell, FireSpell);
             
            Assert.That(tmpDmg, Is.EqualTo(WaterSpell.Damage*2));
        }

        [Test]
        public void Check_Fire_Half_Damage_Against_Water_Test(){
            double tmpDmg;
            IGameDao gameDao = new InMemoryGameDao();
            IGameManager gameManager = new GameManager(gameDao);   
            SpellCard WaterSpell = new SpellCard("1", "WaterSpell", 9.0, ElementType.Water, "WaterSpell");
            SpellCard FireSpell = new SpellCard("1", "FireSpell", 10.0, ElementType.Fire, "FireSpell");


            tmpDmg = gameManager.checkEffect(FireSpell, WaterSpell);
             
            Assert.That(tmpDmg, Is.EqualTo(FireSpell.Damage/2));
        }

        [Test]
        public void Check_Fire_Double_Damage_Against_Normal_Test(){
            double tmpDmg;
            IGameDao gameDao = new InMemoryGameDao();
            IGameManager gameManager = new GameManager(gameDao);    
            SpellCard RegularSpell = new SpellCard("1", "RegularSpell", 10.0, ElementType.Normal, "RegularSpell");
            SpellCard FireSpell = new SpellCard("1", "FireSpell", 10.0, ElementType.Fire, "FireSpell");

            tmpDmg = gameManager.checkEffect(FireSpell, RegularSpell);
             
            Assert.That(tmpDmg, Is.EqualTo(FireSpell.Damage*2));
        }

        [Test]
        public void Check_Normal_Double_Damage_Against_Water_Test(){
            double tmpDmg;
            IGameDao gameDao = new InMemoryGameDao();
            IGameManager gameManager = new GameManager(gameDao);
            SpellCard WaterSpell = new SpellCard("1", "WaterSpell", 9.0, ElementType.Water, "WaterSpell");
            SpellCard RegularSpell = new SpellCard("1", "Regular", 10.0, ElementType.Normal, "Regular");

            tmpDmg = gameManager.checkEffect(RegularSpell, WaterSpell);

            Assert.That(tmpDmg, Is.EqualTo(RegularSpell.Damage*2));
        }

        [Test]
        public void SpellCard_VS_SpellCard_Test1(){
            Card? losingCard;
            IGameDao gameDao = new InMemoryGameDao();
            IGameManager gameManager = new GameManager(gameDao);
            SpellCard WaterSpell = new SpellCard("1", "WaterSpell", 9.0, ElementType.Water, "WaterSpell");
            SpellCard FireSpell = new SpellCard("1", "FireSpell", 10.0, ElementType.Fire, "FireSpell");

            losingCard = gameManager.Fight(FireSpell, WaterSpell);
            
            Assert.That(losingCard?.Name, Is.EqualTo(WaterSpell.Name));
        }
        
        [Test]
        public void SpellCard_VS_SpellCard_Test2(){
            Card? losingCard;
            IGameDao gameDao = new InMemoryGameDao();
            IGameManager gameManager = new GameManager(gameDao);

            SpellCard WaterSpell = new SpellCard("1", "WaterSpell", 10.0, ElementType.Water, "WaterSpell");
            SpellCard FireSpell = new SpellCard("1", "FireSpell", 9.0, ElementType.Fire, "FireSpell");

            losingCard = gameManager.Fight(FireSpell, WaterSpell);
            
            Assert.That(losingCard?.Name, Is.EqualTo(FireSpell.Name));
        }

        [Test]
        public void Check_Elo_Calculation_Test(){
            IGameDao gameDao = new InMemoryGameDao();
            IGameManager gameManager = new GameManager(gameDao);

            User? Loser = new("name", "123");

            MonsterCard Dragon = new MonsterCard("1", "Fortisax", 10.0, ElementType.Fire, "Dragon");
            MonsterCard FireElf = new MonsterCard("1", "FireElf", 5.0 ,ElementType.Fire, "FireElf");


            User player1 = new User("name", "123");
            User player2 = new User("name2", "123");

            player1.Deck?.Add(Dragon);
            player2.Deck?.Add(FireElf);

            Loser = gameManager.StartGame(player1, player2);

            Assert.That(Loser?.Elo, Is.EqualTo(95));
        }
    }
}