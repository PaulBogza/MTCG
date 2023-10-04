using System;
using CardClass;
using ElementTypeEnum;

namespace MonsterCardClass{
    public class MonsterCard : Card{
        private int _HealthPoints;
        public int HealthPoints {
            get{return _HealthPoints;}
            set{_HealthPoints = value;}
        }
        public MonsterCard(string Name, int Damage, int HealthPoints, ElementType Element, string Type){
            _Name = Name;
            _Damage = Damage;
            _HealthPoints = HealthPoints;
            _Element = Element;
            _Type = Type;
        }
    }
}