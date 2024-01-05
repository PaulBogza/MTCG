

using System.Text.Json.Serialization;

namespace SWE1.MessageServer.Models{
    public class Card{
        public override string? ToString()
        {
            return $"Card{{Id='{this.Id}', Name='{this.Name}', Damage='{this.Damage}'}}";
        }
        public string Id { get; set; } = "0";
        public string Name { get; set; } = "0";
        public double Damage { get; set; }

        [JsonIgnore]
        public ElementType Element { get; set; } 
        
        [JsonIgnore]
        public string Type { get; set; } = "0";
    }
}
