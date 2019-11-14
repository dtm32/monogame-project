using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2D_Game.Content
{
    class Unit
    {
        private Skill skill1;
        private string unitName;
        private String unitType;
        private String unitFaction;
        private int statHP;
        private int statSpd;
        private int statStr;
        private int statFcs;
        private int statAmr;
        private int statRes;

        public Unit(String name, String type, String faction, ArrayList skills, int hp, int spd, int str, int fcs, int amr, int res)
        {
            unitName = name;
            unitType = type;
            unitFaction = faction;
            skill1 = (Skill) skills[0];
            statHP = hp;
            statSpd = spd;
            statStr = str;
            statFcs = fcs;
            statAmr = amr;
            statRes = res;
        }
    }
}
