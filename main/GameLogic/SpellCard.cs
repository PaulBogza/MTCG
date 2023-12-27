using System;
using CardClass;
using ElementTypeEnum;

namespace SpellCardClass{
    public class SpellCard : Card{
        private readonly int _Id;
        private readonly string _Name;
        private readonly double _Damage;
        private readonly ElementType _Element;
        private readonly string _Type;
        public SpellCard(int Id, string Name, double Damage, ElementType Element, string Type){
            _Id = Id;
            _Name = Name;
            _Damage = Damage;
            _Element = Element;
            _Type = Type;
        }
    }
}