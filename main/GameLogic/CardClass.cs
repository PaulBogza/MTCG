using System;
using ElementTypeEnum;
using BattleClass;

namespace CardClass{
    public class Card{
        public string? Id { get; set; }
        public string? Name { get; set; }
        public float Damage { get; set; }
        public ElementType? Element { get; set; }
        public string? Type { get; set; }
    }
}
