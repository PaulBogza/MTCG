using SWE1.MessageServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
[assembly: InternalsVisibleTo("UnitTests")]

namespace SWE1.MessageServer.DAL
{
    internal interface IGameDao
    {
        User StartGame(User player1, User player2);
        Card? Fight(Card card1, Card card2);
        double checkEffect(Card card1, Card card2);
        double checkSpecialInteraction(Card card1, Card card2);
        bool UpdateUser(User user);
    }
}
