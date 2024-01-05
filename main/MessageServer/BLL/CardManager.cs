using SWE1.MessageServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SWE1.MessageServer.DAL;
using System.Net.Http.Headers;


namespace SWE1.MessageServer.BLL
{
    internal class CardManager : ICardManager
    {
        private readonly ICardDao _cardDao;
        public CardManager(ICardDao cardDao){
            _cardDao = cardDao;
        }
        public List<Card>? ShowCards(User user){
            return _cardDao.ShowCards(user) ?? throw new Exception("No Cards found");
        }
        public List<Card>? ShowDeck(User user){
            return _cardDao.ShowDeck(user) ?? throw new Exception("No Deck found");
        }
        public List<Card>? UpdateDeck(User user, List<string> payload){
            Card forbiddenCard = new Card() {Id = "666"};
            List<Card> forbiddenDeck = new();

            if(payload.Count == 4){
                user.Deck = _cardDao.UpdateDeck(user, payload)!;
                return user.Deck;
            }
            else if(payload.Count != 4){
                forbiddenDeck.Add(forbiddenCard);
                return forbiddenDeck;
            }
            else{
                return user.Deck;
            }
            
        }
        public List<Card>? CreatePackage(List<Card> Package){
            return _cardDao.CreatePackage(Package) ?? throw new Exception();
        }
        public bool AquirePackage(User user){
            return _cardDao.AquirePackage(user);
        }

        public List<Card> ParseCards(List<Card> Package){
              foreach(var card in Package){
                if(card.Name != null){
                    if(card.Name.Contains("Water")){
                        card.Element = ElementType.Water;
                    }
                    else if(card.Name.Contains("Fire")){
                        card.Element = ElementType.Fire;
                    }
                    else{
                        card.Element = ElementType.Normal;
                    }

                    if(card.Name.Contains("Goblin")){
                        card.Type = "Goblin";
                    }
                    else if(card.Name.Contains("FireElf")){
                        card.Type = "FireElf";
                    }
                    else if(card.Name.Contains("Dragon")){
                        card.Type = "Dragon";
                    }
                    else if(card.Name.Contains("Knight")){
                        card.Type = "Knight";
                    }
                    else if(card.Name.Contains("Kraken")){
                        card.Type = "Kraken";
                    }
                    else if(card.Name.Contains("Wizard")){
                        card.Type = "Wizard";
                    }
                    else if(card.Name.Contains("Ork")){
                        card.Type = "Ork";
                    }
                    else if(card.Name.Contains("Spell")){
                        card.Type = "Spell";
                    }
                }
            }
            return Package;
        }
    }
}
