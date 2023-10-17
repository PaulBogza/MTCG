using System;
using CardClass;
using MonsterCardClass;
using SpellCardClass;
using PlayerClass;
using GameClass;
using ElementTypeEnum;
using BattleClass;
using Npgsql;
using System.Runtime.ConstrainedExecution;


//TRY RACE ENUM INSTEAD OF TYPE STRINGP

namespace myMTCG{
    class Program{
        static void Main(string[] args){
            System.Console.WriteLine("test");
            /*
            //singleton implementation
            Game newGame = Game.GetInstance();
            Battle newBattle = new Battle();
            Player Player1 = new Player(100, 20);
            Player Player2 = new Player(100, 20);

            newGame.StartGame(Player1, Player2);

            MonsterCard Goblin = new MonsterCard("WaterGoblin", 1, ElementType.Water, "Goblin");
            MonsterCard Elf = new MonsterCard("FireElf", 1, ElementType.Fire, "Elf");
            SpellCard FrostRay = new SpellCard("FrostRay", 1, ElementType.Water, "Spell");
            MonsterCard Dragon = new MonsterCard("Fortisax", 1, ElementType.Fire, "Dragon");
            MonsterCard Knight = new MonsterCard("Black Knight", 1, ElementType.Normal, "Knight");
            MonsterCard Ork = new MonsterCard("Ork", 1, ElementType.Normal, "Ork");
            MonsterCard Kraken = new MonsterCard("Takoyaki", 1, ElementType.Water, "Kraken");
            MonsterCard Wizard = new MonsterCard("Saruman", 1, ElementType.Normal, "Wizard");
            MonsterCard FireElf = new MonsterCard("FireElf", 1 ,ElementType.Fire, "FireElf");

            newBattle.losingCard = newBattle.Fight(Dragon, FireElf);

            if(newBattle.losingCard == null){
                System.Console.WriteLine("Draw");
            }
            else{
                System.Console.WriteLine("{0} lost this battle",newBattle.losingCard.Name);
            }
            */
            
            var connString = "Host=127.0.0.1;Port=5432;Database=mydb;Username=postgres;Password=postgres;Persist Security Info=True";
            using var conn = new NpgsqlConnection(connString);
            conn.Open();
            Console.WriteLine("Opened");
                        

            using (var cmd = new NpgsqlCommand("CREATE TABLE IF NOT EXISTS test (id INT PRIMARY KEY);", conn))
            {
                cmd.ExecuteNonQuery();
                Console.WriteLine("Executed");
            }

            conn.Close(); 
        }
    }
}