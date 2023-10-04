using System;
using ElementTypeEnum;
using BattleClass;

namespace CardClass{
    abstract public class Card{
        protected string _Name= "TBD";
        public string Name{
            get{return _Name;}
            set{_Name = value;}
        }
        protected int _Damage;
        public int Damage{
            get{return _Damage;}
            set{_Damage = value;}
        }
        protected ElementType _Element;
        public ElementType Element{
            get{return _Element;}
            set{_Element = value;}
        }
        protected string _Type = "TBD";
        public string Type{
            get{return _Type;}
            set{_Type = value;}
        }
    }
}
