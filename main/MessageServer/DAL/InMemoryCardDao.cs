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
                user.Coins += -5;
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
            if(user.Deck != null){
                System.Console.WriteLine(user.Deck.ElementAt(0).Id);
            }
            return user.Deck;
        }
        public List<Card>? UpdateDeck(User user, string payload){
            if(user.Stack == null){
                return null;
            }
            System.Console.WriteLine(user.Stack.ElementAt(0).Id);
            return user.Deck;
        }
    }
}
