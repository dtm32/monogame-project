﻿using Microsoft.Xna.Framework.Graphics;
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
        public String Type;
        public String Faction;
        public int HP { get; }
        public int Spd { get; }
        public int Str { get; }
        public int Fcs { get; }
        public int Amr { get; }
        public int Res { get; }
        public bool IsEnemy { get; set; }
        public int Experience { get; set; }
        private const int expPerLevel = 200;

        public int StatTotal
        {
            get
            {
                int sum = HP + Spd + Str + Fcs + Amr + Res;
                return sum;
            }
        }

        // TODO: directly add all textures and animations to unit/skill

        public BaseUnit(AnimatedSprite texture, String name, String type, String faction, ArrayList skills, int hp, int spd, int str, int fcs, int amr, int res)
        {
            unitTexture = texture;
            Name = name;
            Type = type;
            Faction = faction;
            skill1 = (Skill)skills[0];
            unitSkills = skills;
            HP = hp;
            Spd = spd;
            Str = str;
            Fcs = fcs;
            Amr = amr;
            Res = res;
            IsEnemy = false;
            Experience = 20000;
        }

        public BaseUnit(BaseUnit unit)
        {
            this.unitTexture = unit.Texture;
            this.Name = unit.Name;
            this.Type = unit.Type;
            this.Faction = unit.Faction;
            this.unitSkills = unit.GetSkills();
            this.HP = unit.HP;
            this.Spd = unit.Spd;
            this.Str = unit.Str;
            this.Fcs = unit.Fcs;
            this.Amr = unit.Amr;
            this.Res = unit.Res;
            this.Experience = unit.Experience;
            this.IsEnemy = false;
        }

        public BaseUnit()
        {
            this.Name = "";
            this.Type = "Infantry";
            this.Faction = "Common";
            this.HP = 0;
            this.Spd = 0;
            this.Str = 0;
            this.Fcs = 0;
            this.Amr = 0;
            this.Res = 0;
            this.IsEnemy = false;
        }

        public BaseUnit SetLevel(int level)
        {
            Experience = level * expPerLevel;

            return this;
        }

        public AnimatedSprite Texture
        {
            get { return unitTexture; }
            //set { _type = value; }
        }

        public AnimatedSprite Sprite
        {
            get { return unitTexture; }
        }

        public ArrayList GetSkills()
        {
            return unitSkills;
        }

        public int Level
        {
            get
            {
                double level = 0;
                // level = (2.7exp)^2 - (0.364exp)^3
                //level = Math.Floor(1.55 * 100) ^ 2
                // level = 0.4 * exp ^ (1/1.83);
                level = Experience / expPerLevel;

                if(level < 1)
                    level = 1;
                else if(level > 100)
                    level = 100;
                
                return (int)level;
            }
        }

        private double CalcStatMult(int level)
        {
            double statMult = 1.0;

            if(level <= 30)
            {
                statMult = 0.15 * (level / 10) + 0.10;
            }
            else if(level <= 50)
            {
                statMult = 0.10 * (level / 10) + 0.25;
            }
            else
            {
                statMult = 0.05 * (level / 10) + 0.50;
            }

            statMult *= 1.5;

            return statMult;
        }

        public int CalcHP
        {
            get { return (int)(CalcStatMult(Level) * HP) * 2; }
        }

        public int CalcSpd
        {
            get { return (int)(CalcStatMult(Level) * Spd); }
        }

        public int CalcStr
        {
            get { return (int)(CalcStatMult(Level) * Str); }
        }

        public int CalcFcs
        {
            get { return (int)(CalcStatMult(Level) * Fcs); }
        }

        public int CalcAmr
        {
            get
            {
                float earlyDefenseMod = 1.0f;
                if(Level < 50)
                    earlyDefenseMod = 0.015f * Level + 0.25f;
                return (int)(CalcStatMult(Level) * Amr * earlyDefenseMod);
            }
        }

        public int CalcRes
        {
            get
            {
                float earlyDefenseMod = 1.0f;
                if(Level < 50)
                    earlyDefenseMod = 0.015f * Level + 0.25f;
                return (int)(CalcStatMult(Level) * Res * earlyDefenseMod);
            }
        }
    }
}
