using Microsoft.Xna.Framework;
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
        public const int Health = 0;
        public const int Speed = 1;
        public const int Strength = 2;
        public const int Focus = 3;
        public const int Armor = 4;
        public const int Resistance = 5;

        AnimatedSprite Texture { get; }
        public ArrayList Skills { get; }
        public List<StatusEffect> StatusEffects { get; }
        public string Name { get; }
        public String unitType;
        public String unitFaction;
        public int HP { get; private set; }
        private int currHP;
        public int Spd { get; private set; }
        public int Str { get; private set;  }
        public int Fcs { get; private set;  }
        public int Amr { get; private set;  }
        public int Res { get; private set; }
        public bool IsEnemy { get; set; }
        public int LastDamageTaken { get; set; }

        public int[] StatTiers = new int[6];

        public int Index { get; set; }

        private BaseUnit baseUnit;

        // TODO: directly add all textures and animations to unit/skill
        public Unit(BaseUnit baseUnit, int index)
        {
            this.Texture     = baseUnit.Texture;
            this.Name        = baseUnit.Name;
            this.unitType    = baseUnit.Type;
            this.unitFaction = baseUnit.Faction;
            this.Skills      = baseUnit.GetSkills();
            this.HP          = baseUnit.CalcHP;
            this.Spd         = baseUnit.CalcSpd;
            this.Str         = baseUnit.CalcStr;
            this.Fcs         = baseUnit.CalcFcs;
            this.Amr         = baseUnit.CalcAmr;
            this.Res         = baseUnit.CalcRes;
            this.IsEnemy     = false;
            this.Index       = index;
            this.baseUnit    = new BaseUnit(baseUnit);

            currHP = this.HP;
            StatusEffects = new List<StatusEffect>();
            LastDamageTaken = 0;

            for(int i = 0; i < 6; i++)
            {
                StatTiers[i] = 0;
            }
        }

        public int CurrHP
        {
            get { return currHP; }
            set
            {
                value = MathHelper.Clamp(value, 0, HP);
                if (value >= currHP)
                    Console.WriteLine($"{Name} healed {value - currHP} Health!");
                else
                    LastDamageTaken = currHP - value > 0 ? CurrHP - value : currHP;
                currHP = MathHelper.Clamp(value, 0, HP);
                if(!IsAlive)
                    Console.WriteLine($"{Name} is defeated!");
            }

        }

        public bool IsStunned
        {
            get
            {
                bool value = false;

                StatusEffects.ForEach((status) =>
                {
                    if(status.Type == StatusEffect.Stun)
                        value = true;
                });

                return value;
            }
        }

        public void Inflict(StatusEffect effect) // TODO: overload for poison/bleed
        {
            StatusEffects.Add(effect);
        }

        public int DebuffStat(int stat, int tiers)
        {
            Console.WriteLine($"Debuffing {Name} stat by {tiers}");

            int newStat = 0;

            StatTiers[stat] -= tiers;

            if(StatTiers[stat] < -8)
                StatTiers[stat] = -8;

            switch(stat)
            {
                case Unit.Speed:
                    newStat = (int)(CalcStatMod(StatTiers[stat]) * baseUnit.CalcSpd);
                    Spd = newStat;
                    break;
                case Unit.Strength:
                    newStat = (int)(CalcStatMod(StatTiers[stat]) * baseUnit.CalcStr);
                    Str = newStat;
                    break;
                case Unit.Focus:
                    newStat = (int)(CalcStatMod(StatTiers[stat]) * baseUnit.CalcFcs);
                    Fcs = newStat;
                    break;
                case Unit.Armor:
                    newStat = (int)(CalcStatMod(StatTiers[stat]) * baseUnit.CalcAmr);
                    Amr = newStat;
                    break;
                case Unit.Resistance:
                    newStat = (int)(CalcStatMod(StatTiers[stat]) * baseUnit.CalcRes);
                    Res = newStat;
                    break;
            }

            return newStat;
        }

        public int BuffStat(int stat, int tiers)
        {
            Console.WriteLine($"Buffing {Name} stat by {tiers}");
            int newStat = 0;

            StatTiers[stat] += tiers;

            if(StatTiers[stat] > 8)
                StatTiers[stat] = 8;

            switch(stat)
            {
                case Unit.Speed:
                    newStat = (int)(CalcStatMod(StatTiers[stat]) * baseUnit.CalcSpd);
                    Spd = newStat;
                    break;
                case Unit.Strength:
                    newStat = (int)(CalcStatMod(StatTiers[stat]) * baseUnit.CalcStr);
                    Str = newStat;
                    break;
                case Unit.Focus:
                    newStat = (int)(CalcStatMod(StatTiers[stat]) * baseUnit.CalcFcs);
                    Fcs = newStat;
                    break;
                case Unit.Armor:
                    newStat = (int)(CalcStatMod(StatTiers[stat]) * baseUnit.CalcAmr);
                    Amr = newStat;
                    break;
                case Unit.Resistance:
                    newStat = (int)(CalcStatMod(StatTiers[stat]) * baseUnit.CalcRes);
                    Res = newStat;
                    break;
            }

            return newStat;
        }

        private float CalcStatMod(int tier)
        {
            float statMod = 1.0f;

            if(tier < 0)
            {
                statMod = 8f / (-1 * tier + 8);
            }
            else if(tier > 0)
            {
                statMod = (tier + 8) / 8f;
            }

            return statMod;
        }

        public Color StatColor(int stat)
        {
            Color color = Color.Black;

            if(StatTiers[stat] > 0)
            {
                color = Color.Green;
            }
            else if(StatTiers[stat] < 0)
            {
                color = Color.DarkRed;
            }

            return color;
        }


        public int Level
        {
            get
            {
                return baseUnit.Level;
            }
        }

        public void Update()
        {
            Texture.Update();

            if(attackFrame > 0)
            {
                UpdateAttackAnimation();
            }
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 location)
        {
            if(attackFrame > 0)
            {
                location.X += attackFrameOffset.X;
            }

            if(IsEnemy)
            {
                Texture.Draw(spriteBatch, location, SpriteEffects.FlipHorizontally, Color.White);
            }
            else
            {
                Texture.Draw(spriteBatch, location, Color.White);
            }
        }

        private int attackFrame = 0;
        Vector2 attackFrameOffset = new Vector2();

        public void StartAttackAnimation()
        {
            attackFrame = 1;
        }

        public void UpdateAttackAnimation()
        {
            attackFrame++;

            if(attackFrame < 10)
            {
                // move unit right
                attackFrameOffset.X = attackFrame * 2;
                //unitHealthRects[selectedIndex].X;
            }
            else if(attackFrame == 10)
            {
                attackFrameOffset.X = attackFrame * 2;
                // deal damage ??
                //int targetUnit = FindTarget(selectedUnit);
                //Skill selectedSkill = (Skill)selectedUnit.Skills[rnd.Next(4)];

                //int damage = CombatCalculation(units[targetUnit], selectedUnit, selectedSkill);

                //healthBars[targetUnit].Set(units[targetUnit].PercentHealth());
                //healthBars[selectedIndex].Set(selectedUnit.PercentHealth());

            }
            else if(attackFrame < 20)
            {
                attackFrameOffset.X = (20 - attackFrame) * 2;
                //unitLocs[selectedIndex].X = unitRects[selectedIndex].X - ((100 - attackFrame) / 5);
            }
            else if(attackFrame == 20)
            {
                attackFrameOffset.X = 0;
                attackFrame = 0;
                //unitLocs[selectedIndex].X = unitRects[selectedIndex].X;
            }
            else
            {
                attackFrameOffset.X = 0;
                attackFrame = 0;
                //battleState = BattleState.RoundNext;
                //SetState(BattleState.RoundNext);
            }

            if(IsEnemy)
            {
                attackFrameOffset.X *= -1.0f;
            }
        }

        public bool IsAnimating()
        {
            if(attackFrame > 0)
            {
                return true;
            }

            return false;
        }

        //public AnimatedSprite Texture
        //{
        //    get { return unitTexture; }
        //    //set { _type = value; }
        //}

        public bool IsAlive
        {
            get { return currHP > 0; }
        }

        public float PercentHealth()
        {
            return (float)currHP / (float)HP;
        }
    }
}
