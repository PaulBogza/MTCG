using System;
using CardClass;
using ElementTypeEnum;

namespace MonsterCardClass{
    public class MonsterCard : Card{
        public int _Id { get; set; }
        public string _Name { get; set; }
        public double _Damage { get; set; }
        public ElementType _Element { get; set; }
        public string _Type { get; set; }
        public MonsterCard(int Id, string Name, double Damage, ElementType Element, string Type){
            _Id = Id;
            _Name = Name;
            _Damage = Damage;
            _Element = Element;
            _Type = Type;
        }
    }
}