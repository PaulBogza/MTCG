namespace SWE1.MessageServer.Models
{
    public class UserStats
    {
        public string Username { get; set; }
        public int Elo { get; set; }
        public int Wins { get; set; }
        public int Losses { get; set; }

        public UserStats(string username, int elo, int wins, int losses)
        {
            Username = username;
            Elo = elo;
            Wins = wins;
            Losses = losses;
        }
    }
}
