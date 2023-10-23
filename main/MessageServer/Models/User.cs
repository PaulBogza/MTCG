namespace SWE1.MessageServer.Models
{
    public class User
    {
        public string Username { get; private set; }
        public string Password { get; private set; }
        public string Token => $"{Username}-msgToken";

        public User(string username, string password)
        {
            Username = username;
            Password = password;
        }
    }
}
