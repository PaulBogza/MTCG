using System;

namespace CardClasses{
    abstract public class Card{
        protected int Damage;
        protected string Element = "TBD";
        protected string Type = "TBD";
        public virtual void Attack(Card Target){
            System.Console.WriteLine("Attacking");
        }
    }

    public class MonsterCard : Card{
        private int HealthPoints;
        public int healthPoints {
            get{return HealthPoints;}
            set{HealthPoints = value;}
        }
        private bool IsScared = false;
        public bool isScared{
            get{return IsScared;}
            set{IsScared = value;}
        }
        public MonsterCard(int Damage, int HealthPoints, string Element, string Type){
            this.Damage = Damage;
            this.HealthPoints = HealthPoints;
            this.Element = Element;
            this.Type = Type;
        }
        public override void Attack(Card Target){}
    }

    public class SpellCard : Card{
        private bool Effective = true;
        public bool effective{
            get{return Effective;}
            set{Effective = value;}
        }
        private bool Effect = true;
        public bool effect{
            get{return Effect;}
            set{Effect = value;}
        }
        public SpellCard(int Damage, string Element, string Type){
            this.Damage = Damage;
            this.Element = Element;
            this.Type = Type;
        }
        public override void Attack(Card Target){}
    }
}
