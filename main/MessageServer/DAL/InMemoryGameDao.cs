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
using System.Xml.Serialization;
[assembly: InternalsVisibleTo("UnitTests")]

namespace SWE1.MessageServer.DAL
{
    internal class InMemoryGameDao : IGameDao
    {
        public int Rounds { get; set; } = 1;
        public string? Winner { get; set; } = null;
        
        public bool UpdateUser(User user){
            return true;
        }
        public List<string> StartGame(User Player1, User Player2){
             List<string> Winner = new();
            Card? losingCard;
            Card card1 = new();
            Card card2 = new();

            while(Player1.Deck.Count > 0 && Player2.Deck.Count > 0 && Rounds <= 20){
                card1 = Player1.Deck.First();
                card2 = Player2.Deck.First();
                losingCard = Fight(card1, card2);
                Rounds++;
                if(losingCard == null){
                    System.Console.WriteLine("Round was a draw\n");
                    continue;
                }
                else if(losingCard.Id == card1.Id){
                    Player2.Deck.Add(card1);
                    Player1.Deck.Remove(card1);
                }
                else if(losingCard.Id == card2.Id){
                    Player1.Deck.Add(card2);
                    Player2.Deck.Remove(card2);
                }
                else{
                    break;
                }
            }

            if(Player1.Deck.Count == 0){
                Player1.Elo += -5;
                Player2.Elo += 3;
                Player1.Losses += 1;
                Player2.Wins += 1;
                System.Console.WriteLine($"{Player2.Username} won battle\r\n");
            }
            else if(Player2.Deck.Count == 0){
                Player2.Elo += -5;
                Player1.Elo += 3;
                Player2.Losses += 1;
                Player1.Wins += 1;
                
                System.Console.WriteLine($"{Player1.Username} won battle\r\n");
            }
            else{
                System.Console.WriteLine("Draw\r\n");
            }
            return Winner;
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
