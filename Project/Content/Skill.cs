using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2D_Game.Content
{
    class Skill
    {
        private String name;
        private int tier;
        private int tiers;
        private int power;

        public Skill(String name, int power)
        {
            tier = 0;
            this.name = name;
            this.power = power;
        }

        //public Boolean setTier(int tier, int power)
        //{
        //    if(tier < tiers)
        //    {
        //        this.power[tier] = power;
        //    }

        //    return false;
        //}

        public int getPower()
        {
            return power;
        }
    }
}
