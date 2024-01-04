using System.Reflection.Metadata.Ecma335;
using Microsoft.VisualBasic;

namespace SWE1.MessageServer.Models
{
    public class User
    {
        public string Username { get; private set; }
        public string Password { get; private set; }
        public int Elo { get; set; } = 100;
        public int Wins { get; set; } = 0;
        public int Losses { get; set; } = 0;
        public int Coins { get; set; } = 20;
        public List<Card> Stack{ get; set; } = new List<Card>();
        public List<Card> Deck{ get; set; } = new List<Card>();
        public Dictionary <string, string> UserInfo { get; set; } = new();
        public Dictionary <string, dynamic> Stats { get; set; }
        = new Dictionary<string, dynamic> {{"Name", ""}, {"Elo", 100}, {"Wins", 0}, {"Losses", 0}};
        public string Token => $"{Username}-mtcgToken";
        public User(string username, string password)
        {
            Username = username;
            Password = password;
        }
    }
}
