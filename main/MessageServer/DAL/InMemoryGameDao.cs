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
        public string? Winner { get; set; }

        public User StartGame(User Player1, User Player2){
            User Loser = new User("Placeholder", "Placeholder");
            Card? losingCard;
            Card card1 = new Card();
            Card card2 = new Card();
            if(Player1.Deck.Count != 0){
                card1 = Player1.Deck.ElementAt(0);
            }
            if(Player2.Deck.Count != 0){
                card2 = Player2.Deck.ElementAt(0);
            }
            int i = 0;
            while(Player1.Deck?.Count != 0 || Player2.Deck?.Count != 0 || Rounds < 20){
                losingCard = Fight(card1, card2);
                i++;
                Rounds++;
                if(losingCard?.Name == card1.Name){
                    Player1.Deck?.Remove(card1);
                    break;
                }
                else if(losingCard?.Name == card2.Name){
                    Player2.Deck?.Remove(card2);
                    break;
                }
                else{
                    continue;
                }
            }

            if(Player1.Deck?.Count == 0){
                Player1.Elo += -5;
                Player2.Elo += 3;
                Loser = Player1;
            }
            else if(Player2.Deck?.Count == 0){
                Player2.Elo += -5;
                Player1.Elo += 3;
                Loser = Player2;
            }
            else{
                System.Console.WriteLine("Draw");
            }
            return Loser;
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
