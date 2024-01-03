

namespace SWE1.MessageServer.Models{
    public class Card{
        public string Id { get; set; } = "0";
        public string Name { get; set; } = "0";
        public double Damage { get; set; }
        public ElementType Element { get; set; }
        public string Type { get; set; } = "0";
    }
}
