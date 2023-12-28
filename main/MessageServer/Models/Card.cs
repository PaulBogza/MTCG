

namespace SWE1.MessageServer.Models{
    public class Card{
        public string? Id { get; set; }
        public string? Name { get; set; }
        public double Damage { get; set; }
        public ElementType? Element { get; set; }
        public string? Type { get; set; }
    }
}
