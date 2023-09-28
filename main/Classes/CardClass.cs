using System;

namespace CardClass{
    public enum ElementType{
        Normal, 
        Fire, 
        Water
    };
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

        public virtual Card Attack(Card Target){
            System.Console.WriteLine("{0} Attacking {1}",this.Name, Target.Name);
            if(this.Damage > Target.Damage){
                return this; 
            }
            else if(this.Damage == Target.Damage){
                return null;
            }
            else{
                return Target;
            }
        }
    }
}
