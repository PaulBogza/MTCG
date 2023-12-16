using System;
using CardClass;
using ElementTypeEnum;

namespace SpellCardClass{
    public class SpellCard : Card{
        public SpellCard(string Name, int Damage, ElementType Element, string Type){
            _Name = Name;
            _Damage = Damage;
            _Element = Element;
            _Type = Type;
        }
    }
}