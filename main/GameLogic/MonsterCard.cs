using System;
using CardClass;
using ElementTypeEnum;

namespace MonsterCardClass{
    public class MonsterCard : Card{
        public MonsterCard(string Name, int Damage, ElementType Element, string Type){
            _Name = Name;
            _Damage = Damage;
            _Element = Element;
            _Type = Type;
        }
    }
}