using CardClass;
using PlayerClass;
using SWE1.MessageServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWE1.MessageServer.DAL
{
    internal interface ICardDao
    {
        List<Card>? showCards(Player player);
        List<Card>? showDeck(Player player);
        int updateDeck();
    }
}
