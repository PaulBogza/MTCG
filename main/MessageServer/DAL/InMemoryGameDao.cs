using Npgsql;
using SWE1.MessageServer.BLL;
using SWE1.MessageServer.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
[assembly: InternalsVisibleTo("UnitTests")]

namespace SWE1.MessageServer.DAL
{
    internal class InMemoryGameDao : IGameDao
    {
        public int Rounds { get; set; } = 1;
        public string? Winner { get; set; }

        public User? StartGame(User Player1, User Player2){
            Card? losingCard;
            int i = 0;
            while(Player1.Deck != null || Player2.Deck != null || Rounds < 20){
                losingCard = Fight(Player1.Deck.ElementAt(i), Player2.Deck.ElementAt(i));
                i++;
                Rounds++;
            }

            if(Player1.Deck == null){
                Winner = Player2.Username;
                Player1.Elo += -5;
                Player2.Elo += 3;
            }
            else if(Player2.Deck == null){
                Winner = Player1.Username;                
                Player2.Elo += -5;
                Player1.Elo += 3;
            }
            else{
                Winner = "Draw";
            }
            System.Console.WriteLine(Player1.Elo);
            System.Console.WriteLine(Player2.Elo);
            return Player1;
        }

        public void Trade(User player1, User player2){}

        public Card? Fight(Card Card1, Card Card2){
            double tempDmg1 = Card1.Damage;
            double tempDmg2 = Card2.Damage;

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
        public double checkEffect(Card Card1, Card Card2){
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

        public double checkSpecialInteraction(Card Card1, Card Card2){
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
