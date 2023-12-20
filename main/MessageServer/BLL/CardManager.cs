using CardClass;
using PlayerClass;
using SWE1.MessageServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SWE1.MessageServer.DAL;


namespace SWE1.MessageServer.BLL
{
    internal class CardManager : ICardManager
    {
        private readonly ICardDao _cardDao;
        public CardManager(ICardDao cardDao){
            _cardDao = cardDao;
        }
        public List<Card>? ShowCards(User user){
            return _cardDao.ShowCards(user) ?? throw new UserNotFoundException("No Cards found");
        }
        public List<Card>? ShowDeck(User user){
            return _cardDao.ShowDeck(user) ?? throw new UserNotFoundException("No Deck found");
        }
        public List<Card>? UpdateDeck(User user, string payload){
            return user.Deck;
        }
        public List<Card>? CreatePackage(List<Card> Package){
            return _cardDao.CreatePackage(Package) ?? throw new UserNotFoundException();
        }
        public bool AquirePackage(User user){
            return _cardDao.AquirePackage(user);
        }
    }
}
