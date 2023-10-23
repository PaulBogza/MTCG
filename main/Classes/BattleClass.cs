using GameClass;
using CardClass;
using ElementTypeEnum;
using MonsterCardClass;
using SpellCardClass;
using PlayerClass;

namespace BattleClass{
    public class Battle{

        public Card? losingCard = null;


        public void InitBattle(){
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
        }
        public Card? Fight(Card Card1, Card Card2){
            int tempDmg1 = Card1.Damage;
            int tempDmg2 = Card2.Damage;

            if(Card1.Type == "Dragon" || Card1.Type == "Knight" || Card1.Type == "Ork" || Card1.Type == "Wizard" || Card1.Type == "Kraken" || Card1.Type == "FireElf"){
                tempDmg2 = checkSpecialInteraction(Card2, Card1);
            }

            if(Card2.Type == "Dragon" || Card2.Type == "Knight" || Card2.Type == "Ork" || Card2.Type == "Wizard" || Card2.Type == "Kraken" || Card2.Type == "FireElf"){
                tempDmg1 = checkSpecialInteraction(Card1, Card2);
            }

            if(Card1.Type == "Spell"){
                tempDmg1 = checkEffect(Card1, Card2);
            }

            if(Card2.Type == "Spell"){
                tempDmg2 = checkEffect(Card2, Card1);
            }

            if(tempDmg1 == tempDmg2){
                return null;
            }
            else if(tempDmg1 > tempDmg2){
                return Card2; 
            }
            else{
                return Card1;
            }
        }
        public int checkEffect(Card Card1, Card Card2){
            if(Card1.Element == ElementType.Water && Card2.Element == ElementType.Fire){
                return Card1.Damage*2;
            }
            else if(Card1.Element == ElementType.Fire && Card2.Element == ElementType.Normal){
                return Card1.Damage*2;
            }
            else if(Card1.Element == ElementType.Normal && Card2.Element == ElementType.Water){
                return Card1.Damage*2;
            }
            else if(Card1.Element == ElementType.Fire && Card2.Element == ElementType.Water){
                return Card1.Damage/2;
            }
            else{
                return Card1.Damage;
            }
        }

        public int checkSpecialInteraction(Card Card1, Card Card2){
            if(Card1.Type == "Goblin" && Card2.Type == "Dragon"){
                return Card1.Damage*0;
            }
            else if(Card1.Type == "Ork" && Card2.Type == "Wizard"){
                return Card1.Damage*0;
            }
            else if(Card1.Element == ElementType.Water && Card1.Type == "Spell" && Card2.Type == "Knight"){
                return Card1.Damage*1000;
            }
            else if(Card1.Type == "Spell" && Card2.Type == "Kraken"){
                return Card1.Damage*0;
            }
            else if(Card1.Type == "Dragon" && Card2.Type == "FireElf"){
                return Card1.Damage*0;
            }
            else{
                return Card1.Damage;
            }
        }
    }
}