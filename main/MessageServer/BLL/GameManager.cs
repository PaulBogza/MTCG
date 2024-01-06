using SWE1.MessageServer.Models;
using SWE1.MessageServer.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
[assembly: InternalsVisibleTo("UnitTests")]

namespace SWE1.MessageServer.BLL
{
    internal class GameManager : IGameManager
    {
        private readonly IGameDao _gameDao;
        public GameManager(IGameDao gameDao){
            _gameDao = gameDao;
        }
        public User StartGame(User player1, User player2){
            return _gameDao.StartGame(player1, player2);
        }
        public Card? Fight(Card card1, Card card2){
            return _gameDao.Fight(card1, card2);
        }
        public double checkEffect(Card card1, Card card2){
            return _gameDao.checkEffect(card1, card2);
        }
        public double checkSpecialInteraction(Card card1, Card card2){
            return _gameDao.checkSpecialInteraction(card1, card2);
        }
    }
}
