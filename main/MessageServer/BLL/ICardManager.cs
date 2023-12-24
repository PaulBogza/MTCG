using CardClass;
using PlayerClass;
using SWE1.MessageServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWE1.MessageServer.BLL
{
    internal interface ICardManager
    {
        List<Card>? ShowCards(User user);
        List<Card>? ShowDeck(User user);
        List<Card>? CreatePackage(List<Card> Package);
        bool AquirePackage(User user);
        List<Card>? UpdateDeck(User user, List<string> payload);
        void initDeck(User user);
    }
}
