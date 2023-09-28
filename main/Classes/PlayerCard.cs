using System;
using CardClass;

namespace SpellCard{
    public class SpellCard : Card{
        private bool _Effective = true;
        public bool Effective{
            get{return _Effective;}
            set{_Effective = value;}
        }
        private bool _Effect = true;
        public bool Effect{
            get{return _Effect;}
            set{_Effect = value;}
        }
        public SpellCard(string Name, int Damage, ElementType element, string Type){
            _Name = Name;
            _Damage = Damage;
            _Element = Element;
            _Type = Type;
        }
        public override Card Attack(Card Target){
            return Target;
        }
    }
}