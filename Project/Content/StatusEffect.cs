using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2D_Game.Content
{
    class StatusEffect
    {
        public enum StatusType
        {
            Burn,
            Bleed,
            Stun,
            Sleep,
            Silence,
            Poison,
            Bind,
            Curse,
            Fortified
        }

        public const int Burn   = 0;
        public const int Bleed  = 1;
        public const int Poison = 2;
        public const int Stun   = 3;

        public int Type { get; }
        public int Turns { get; set; }
        public int Damage { get; }

        public StatusEffect(int type, int turns)
        {
            Type = type;
            Turns = turns;
            Damage = 0;

            //switch(type)
            //{
            //    case StatusType.Burn:
            //        break;
            //    case StatusType.Bleed:
            //        break;
            //    case StatusType.Stun:
            //        break;
            //    case StatusType.Sleep:
            //        break;
            //    case StatusType.Poison:
            //        break;
            //    case StatusType.Bind:
            //        break;
            //}
        }

        public StatusEffect(int type, int turns, double damage)
        {
            Type = type;
            Turns = turns;
            Damage = (int)damage;
        }

        public StatusEffect(int type, int turns, Unit self, Unit target)
        {
            Type = type;
            Turns = turns;

            double damage = 0.0;

            switch(type)
            {
                case Burn:
                    damage = target.HP * 0.1;
                    break;
                case Bleed:
                    damage = self.Str * 0.15;
                    break;
                case Poison:
                    damage = self.Fcs * 0.15;
                    break;
                default:
                    Damage = 0;
                    break;
            }

            if(damage < 1)
                damage = 1;

            Damage = (int)damage;
        }
    }
}
