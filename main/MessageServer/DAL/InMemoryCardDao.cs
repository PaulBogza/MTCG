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
using System.ComponentModel;
using Microsoft.Extensions.Logging.Abstractions;

namespace SWE1.MessageServer.DAL
{
    internal class InMemoryCardDao : ICardDao
    {
        private readonly List<List<Card>> PackageCollection = new();
        public List<Card>? tmpDeck = new();
        public List<Card>? tmpStack = new();

        public List<Card>? CreatePackage(List<Card> Package){
            PackageCollection.Add(Package);
            return Package;
        }
        public void initDeck(User user){
            if(user.Stack != null && user.Stack.Any()){
                for(int i = 0; i < 4; i++){
                    tmpDeck?.Add(user.Stack.ElementAt(i));
                }
                user.Deck = tmpDeck;
                tmpDeck = null;
            }
        }
        public bool AquirePackage(User user){
            if(user.Coins > 4 && (PackageCollection.Count >= 1)){
                user.Coins += -5;
                tmpStack = PackageCollection.ElementAt(0);
                user.Stack = tmpStack;
                tmpStack = null;
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
            if(user.Deck == null){
                initDeck(user);
            }
            return user.Deck;
        }
        public List<Card>? UpdateDeck(User user, List<string> payload){
            Card forbiddenCard = new Card() {Id = "666"};
            List<Card>? forbiddenList = null;
            tmpDeck = null;
            if(payload.Count == 4 && user.Stack != null){
                for(int i = 0; i < user.Stack?.Count; i++){
                    for(int j = 0; j < payload.Count; j++){
                        if(payload.ElementAt(j) == user.Stack.ElementAt(i).Id){
                            tmpDeck?.Add(user.Stack.ElementAt(i));
                        }
                    }
                }
            }
            if(tmpDeck?.Count == 4){
                foreach(var card in tmpDeck){
                    user.Deck?.Add(card);
                }
                return user.Deck;
            }
            else if(tmpDeck?.Count != 4){
                forbiddenList?.Add(forbiddenCard);
                return forbiddenList;
            }
            else{ //unchanged version of user deck
                return user.Deck;
            }
        }
    }
}
