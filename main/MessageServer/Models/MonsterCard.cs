
namespace SWE1.MessageServer.Models{
    public class MonsterCard : Card{
        public MonsterCard(string id, string name, double damage, ElementType element, string type){
            Id = id;
            Name = name;
            Damage = damage;
            Element = element;
            Type = type;
        }
    }
}