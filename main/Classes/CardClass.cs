using System;

namespace CardClasses{
    public enum ElementType{
        Normal, 
        Fire, 
        Water
    };
    abstract public class Card{
        protected string _name= "TBD";
        public string name{
            get{return _name;}
            set{_name = value;}
        }
        protected int _damage;
        public int damage{
            get{return _damage;}
            set{_damage = value;}
        }
        protected ElementType _element;
        public ElementType element{
            get{return _element;}
            set{_element = value;}
        }
        protected string _type = "TBD";
        public string type{
            get{return _type;}
            set{_type = value;}
        }
        public abstract void Attack(Card Target);
    }

    public class MonsterCard : Card{
        private int _healthPoints;
        public int healthPoints {
            get{return _healthPoints;}
            set{_healthPoints = value;}
        }
        private bool _isScared = false;
        public bool isScared{
            get{return _isScared;}
            set{_isScared = value;}
        }
        public MonsterCard(string name, int damage, int healthPoints, ElementType element, string type){
            _name = name;
            _damage = damage;
            _healthPoints = healthPoints;
            _element = element;
            _type = type;
        }
        public override void Attack(Card Target){
            System.Console.WriteLine("{0} attacked {1}", this.name, Target.name);
        }
    }

    public class SpellCard : Card{
        private bool _effective = true;
        public bool effective{
            get{return _effective;}
            set{_effective = value;}
        }
        private bool _effect = true;
        public bool effect{
            get{return _effect;}
            set{_effect = value;}
        }
        public SpellCard(string name, int damage, ElementType element, string type){
            _name = name;
            _damage = damage;
            _element = element;
            _type = type;
        }
        public override void Attack(Card Target){
            System.Console.WriteLine("{0} used against {1}", this.name, Target.name);
        }
    }
}
