using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2D_Game.Content
{
    class Unit : BaseUnit
    {
        public enum StatusEffect
        {
            Bleed,
            Stun,
            Sleep,
            Silence,
            Burn,
            Poison,
            Bind,
            Curse,
            Fortified
        }

        AnimatedSprite unitTexture;
        public ArrayList Skills { get; }
        public ArrayList StatusEffects { get; }
        public string Name { get; }
        public String unitType;
        public String unitFaction;
        public int HP { get; }
        private int currHP;
        public int Spd { get; }
        public int Str { get; }
        public int Fcs { get; }
        public int Amr { get; }
        public int Res { get; }
        public bool IsEnemy { get; set; }

        private BaseUnit baseUnit;

        // TODO: directly add all textures and animations to unit/skill

        public int CurrHP
        {
            get { return currHP; }
            set
            {
                Console.WriteLine("Setting " + Name + " CurrHP to " + value);
                currHP = value;
            }
            
        }

        public void Inflict(StatusEffect effect) // TODO: overload for poison/bleed
        {
            StatusEffects.Add(effect);
        }

        //public int DecreaseHP


        public Unit(BaseUnit unit)
        {
            this.unitTexture = unit.Texture;
            Console.WriteLine("new Unit " + unitTexture);
            this.Name        = unit.Name;
            this.unitType    = unit.unitType;
            this.unitFaction = unit.unitFaction;
            this.Skills  = unit.GetSkills();
            this.HP          = unit.HP;
            this.Spd         = unit.Spd;
            this.Str         = unit.Str;
            this.Fcs         = unit.Fcs;
            this.Amr         = unit.Amr;
            this.Res         = unit.Res;
            this.IsEnemy     = false;

            currHP = unit.HP;
            baseUnit = unit;
            StatusEffects = new ArrayList();
        }

        public AnimatedSprite Texture
        {
            get { return unitTexture; }
            //set { _type = value; }
        }

        public bool IsAlive()
        {
            return currHP > 0;
        }

        public float PercentHealth()
        {
            return (float)currHP / (float)HP;
        }
    }
}
