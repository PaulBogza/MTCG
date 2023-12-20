using SWE1.MessageServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CardClass;
using SWE1.MessageServer.API.Routing.Users;
using System.Runtime.ConstrainedExecution;
using System.Reflection.Metadata.Ecma335;

namespace SWE1.MessageServer.DAL
{
    internal class InMemoryCardDao : ICardDao
    {
        private readonly List<List<Card>> PackageCollection = new();

        public List<Card>? CreatePackage(List<Card> Package){
            var Packages = new List<Card>();
            PackageCollection.Add(Package);
            
            return Package;
        }
        public bool AquirePackage(User user){
            if(user.Coins > 4 && (PackageCollection.Count >= 1)){
                user.Coins = user.Coins-5;
                user.Stack = PackageCollection.ElementAt(0);
                PackageCollection.RemoveAt(0);
                return true;
            }
            else{
                return false;
            }
        }
        public List<Card>? ShowCards(User user){
            return user.Stack;
        }
        public List<Card>? ShowDeck(User user){
            user.Deck[0] = user.Stack.ElementAt(0);
            user.Deck[1] = user.Stack.ElementAt(1);
            user.Deck[2] = user.Stack.ElementAt(2);
            user.Deck[3] = user.Stack.ElementAt(3);
            return user.Deck;
        }
        public List<Card>? UpdateDeck(User user, string payload){
            user.Deck[0] = user.Stack.FirstOrDefault(payload => user.Stack.Contains(payload));
            return user.Deck;
        }
    }
}
