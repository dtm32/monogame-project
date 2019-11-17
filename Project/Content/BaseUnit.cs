using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2D_Game.Content
{
    class BaseUnit
    {
        AnimatedSprite unitTexture;
        public ArrayList unitSkills;
        public Skill skill1;
        public string Name { get; }
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

        public BaseUnit(AnimatedSprite texture, String name, String type, String faction, ArrayList skills, int hp, int spd, int str, int fcs, int amr, int res)
        {
            unitTexture = texture;
            Name = name;
            unitType = type;
            unitFaction = faction;
            skill1 = (Skill)skills[0];
            unitSkills = skills;
            HP = hp;
            Spd = spd;
            Str = str;
            Fcs = fcs;
            Amr = amr;
            Res = res;
            IsEnemy = false;
        }

        public BaseUnit(Unit unit)
        {
            this.unitTexture = unit.Texture;
            this.Name = unit.Name;
            this.unitType = unit.unitType;
            this.unitFaction = unit.unitFaction;
            //this.skill1      = (Skill)skills[0];
            this.unitSkills = unit.GetSkills();
            this.HP = unit.HP;
            this.Spd = unit.Spd;
            this.Str = unit.Str;
            this.Fcs = unit.Fcs;
            this.Amr = unit.Amr;
            this.Res = unit.Res;
            this.IsEnemy = false;
        }

        public BaseUnit()
        {
            //this.unitTexture = unit.Texture;
            this.Name = "";
            this.unitType = "";
            this.unitFaction = "common";
            //this.unitSkills = unit.GetSkills();
            this.HP = 0;
            this.Spd = 0;
            this.Str = 0;
            this.Fcs = 0;
            this.Amr = 0;
            this.Res = 0;
            this.IsEnemy = false;
        }

        public AnimatedSprite Texture
        {
            get {
                //Console.WriteLine("Getting texture");
                return unitTexture; }
            //set { _type = value; }
        }

        public ArrayList GetSkills()
        {
            return unitSkills;
        }
    }
}
