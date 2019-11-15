using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2D_Game.Content
{
    class Skill
    {
        public string Name;
        private int tier;
        private int tiers;
        public int Power { get; }

        public Skill(String name, int power)
        {
            tier = 0;
            Name = name;
            this.Power = power;
        }

        //public Boolean setTier(int tier, int power)
        //{
        //    if(tier < tiers)
        //    {
        //        this.power[tier] = power;
        //    }

        //    return false;
        //}

        //public int getPower()
        //{
        //    return power;
        //}
    }
}
