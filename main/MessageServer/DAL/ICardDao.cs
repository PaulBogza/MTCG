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
        public List<Card>? ShowCards(User user);
        public List<Card>? ShowDeck(User user);
        public List<Card>? CreatePackage(List<Card> Package);
        bool AquirePackage(User user);
        public List<Card>? UpdateDeck(User user, string payload);
    }
}
