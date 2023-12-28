

namespace SWE1.MessageServer.Models{
    public class SpellCard : Card{
        public SpellCard(string id, string name, double damage, ElementType element, string type){
            Id = id;
            Name = name;
            Damage = damage;
            Element = element;
            Type = type;
        }
    }
}