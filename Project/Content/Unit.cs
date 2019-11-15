using Microsoft.Xna.Framework.Graphics;
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
        AnimatedSprite unitTexture;
        public ArrayList unitSkills;
        public Skill skill1;
        public string unitName;
        public String unitType;
        public String unitFaction;
        public int HP { get; }
        public int Spd { get; }
        public int Str { get; }
        public int Fcs { get; }
        public int Amr { get; }
        public int Res { get; }
        public bool IsEnemy { get; set; }

        // TODO: directly add all textures and animations to unit/skill

        public Unit(AnimatedSprite texture, String name, String type, String faction, ArrayList skills, int hp, int spd, int str, int fcs, int amr, int res)
        {
            unitTexture = texture;
            unitName = name;
            unitType = type;
            unitFaction = faction;
            skill1 = (Skill) skills[0];
            unitSkills = skills;
            HP = hp;
            Spd = spd;
            Str = str;
            Fcs = fcs;
            Amr = amr;
            Res = res;
            IsEnemy = false;
        }

        public Unit(Unit unit)
        {
            this.unitTexture = unit.Texture;
            this.unitName    = unit.unitName;
            this.unitType    = unit.unitType;
            this.unitFaction = unit.unitFaction;
            //this.skill1      = (Skill)skills[0];
            this.unitSkills  = unit.GetSkills();
            this.HP          = unit.HP;
            this.Spd         = unit.Spd;
            this.Str         = unit.Str;
            this.Fcs         = unit.Fcs;
            this.Amr         = unit.Amr;
            this.Res         = unit.Res;
            this.IsEnemy     = false;
        }

        public AnimatedSprite Texture
        {
            get { return unitTexture; }
            //set { _type = value; }
        }

        public ArrayList GetSkills()
        {
            return unitSkills;
        }
    }
}
