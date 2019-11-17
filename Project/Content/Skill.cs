using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2D_Game.Content
{
    class Skill
    {
        public enum SkillType
        {
            Physical,
            Magical,
            Buff
        }

        public delegate void SkillEffect(Unit self, Unit target);

        public string Name;
        private int tier;
        private int tiers;
        public int Power { get; }
        public double Penetration { get; set; }
        public SkillType Type { get; set; }
        public SkillEffect Effect { get; set; }

        public Skill(String name, int power)
        {
            tier = 0;
            Name = name;
            this.Power = power;
            Penetration = 1.0;
            Type = SkillType.Physical;
        }

        //public void SetEffect(SkillEffect effect)
        //{
        //    Effect = effect;
        //}

        //public void Effect(Unit self, Unit target)
        //{

        //}
    }
}
