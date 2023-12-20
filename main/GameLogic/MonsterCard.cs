using System;
using CardClass;
using ElementTypeEnum;

namespace MonsterCardClass{
    public class MonsterCard : Card{
        private readonly int _Id;
        private readonly string _Name;
        private readonly float _Damage;
        private readonly ElementType _Element;
        private readonly string _Type;
        public MonsterCard(int Id, string Name, float Damage, ElementType Element, string Type){
            _Id = Id;
            _Name = Name;
            _Damage = Damage;
            _Element = Element;
            _Type = Type;
        }
    }
}