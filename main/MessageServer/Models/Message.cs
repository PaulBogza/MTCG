namespace SWE1.MessageServer.Models
{
    public class Message
    {
        public int Id { get; set; }
        public string Content { get; set; }

        public Message(string content) : this(0, content) { }

        public Message(int id, string content)
        {
            Id = id;
            Content = content;
        }
    }
}
