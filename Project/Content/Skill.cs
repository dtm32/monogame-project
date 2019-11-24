using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2D_Game.Content
{
    // TODO:
    // Add Print() method
    // Move damage calculations to static func here
    // Damage calc type variable ?
    class Skill
    {
        public enum SkillType
        {
            Physical,
            Magical,
            Effect,
            Buff
        }

        public enum Elements
        {
            Common,
            Wild,
            Light,
            Dark,
            Spirit,
            Demon,
            Mythic
        }

        public static int TargetSingle = 0;
        public static int TargetAll = 1;
        public static int TargetFront = 2;
        public static int TargetAdjacent = 3;
        public static int TargetAlly = 3;
        public static int TargetSelf = 4;

        public static Skill SoulSiphon = new Skill("Soul Siphon", 55, SkillType.Magical,
            (self, target) =>
            {
                if(!target.IsAlive)
                {
                    self.CurrHP += target.HP / 2;
                }
            }, "If this move kills an enemy, heal self for 50% of target's max health.");
        public static Skill Nullify = new Skill("Nullify", 20, SkillType.Magical,
            (self, target) =>
            {
                for(int i = 0; i < target.StatTiers.Length; i++)
                {
                    if(target.StatTiers[i] > 0)
                        target.StatTiers[i] = 0;
                }
            }, "Removes all stat bonuses from target.");
        public static Skill Sacrifice = new Skill("Sacrifice", 0, SkillType.Buff,
            (self, target, units) =>
            {
                int healAmount = (int)(self.Str * 0.5);
                for(int i = 0; i < units.Length / 2; i++)
                {
                    if(units[i].IsAlive)
                        units[i].CurrHP += healAmount;
                }
                self.Inflict(new StatusEffect(StatusEffect.Bleed, 2, self, self));
            }, "Heal all allies for 50% of unit's Strength. Inflicts Bleed(2) on self.");
        public static Skill Reprisal = new Skill("Reprisal", 50, SkillType.Physical,
            (self, target) =>
            {
                if(self.StatusEffects.Count > 0)
                {
                    target.CurrHP -= target.LastDamageTaken;
                }
            }, "Deals double damage if self is affected by a status condition.");
        public static Skill TongueLash = new Skill("Tongue Lash", 55, SkillType.Physical,
            (self, target) =>
            {
                if(Game1.random.Next(3) == 0)
                {
                    target.DebuffStat(Unit.Speed, 4);
                }
            }, "Has a chance to lower target Speed by 4.");
        public static Skill AdaptiveBlow = new Skill("Adaptive Blow", 40, SkillType.Physical,
            (self, target) =>
            {
                if(self.PercentHealth() >= 0.5)
                {
                    target.CurrHP -= target.LastDamageTaken;
                }
                else
                {
                    self.CurrHP += (int)(0.8 * target.LastDamageTaken);
                }
            }, "If self HP is < 50%, deals double damage. Otherwise, heals unit for 80% damage dealt.");
        public static Skill Impede = new Skill("Impede", SkillType.Effect,
            (self, target) =>
            {
                target.Inflict(new StatusEffect(StatusEffect.Stun, 4));
                target.BuffStat(Unit.Armor, 2);
                target.BuffStat(Unit.Resistance, 2);
            }, "Inflicts Stun(4). Increases target Amr/Res by 2.");
        public static Skill PiercingGaze = new Skill("Piercing Gaze", 50, Skill.SkillType.Physical,
            "Ignores 30% of target Armor.").SetPenetration(0.3);
        public static Skill SideWhip = new Skill("Side Whip", SkillType.Physical,
            (self, target, units) =>
            {
                int topIndex = units.Length / 2;
                int botIndex = units.Length - 1;

                while(true)
                {
                    if(!units[topIndex].IsAlive)
                    {
                        topIndex++;
                    }
                    else
                    {
                        break;
                    }
                    if(topIndex == botIndex)
                    {
                        break;
                    }
                }

                while(true)
                {
                    if(!units[botIndex].IsAlive)
                    {
                        botIndex--;
                    }
                    else
                    {
                        break;
                    }
                    if(topIndex == botIndex)
                    {
                        break;
                    }
                }

                int power = 40;
                int attack = self.Str;
                double crit = 1.0;

                if(Game1.random.Next(25) < 5)
                {
                    crit = 1.5; // Also check for skill crit modifier
                }

                if(topIndex == botIndex)
                {
                    int defense = units[topIndex].Amr;
                    //int crit = 1.0 * 
                    power *= 2;
                    int damage = (int)((power * attack / defense * self.Level / 100) *
                        (Game1.random.Next(15) / 100 + 1.35) * crit);
                    units[topIndex].CurrHP -= damage;
                }
                else
                {
                    int topDefense = units[topIndex].Amr;
                    int botDefense = units[botIndex].Amr;

                    int damage = (int)((power * attack / topDefense * self.Level / 100) *
                        (Game1.random.Next(15) / 100 + 1.35) * crit);
                    units[topIndex].CurrHP -= damage;

                    damage = (int)((power * attack / botDefense * self.Level / 100) *
                        (Game1.random.Next(15) / 100 + 1.35) * crit);
                    units[botIndex].CurrHP -= damage;
                }
            }, "Damages the top and bottom enemy. Has a boosted crit chance.");

        public delegate void SkillEffect(Unit self, Unit target);
        public delegate void SkillEffectAll(Unit self, Unit target, UnitList units);

        public string Name;
        private int tier;
        private int tiers;
        private string description = "";
        public int Power { get; }
        public double Penetration { get; set; }
        public SkillType Type { get; set; }
        public SkillEffect Effect { get; set; }
        public SkillEffectAll EffectAll { get; set; }

        private int targetType = 0;

        //public static CalculateDamage()

        public string Description
        {
            get
            {
                string result = $"{Name}";
                if(Power > 0)
                    result += $"\n      P {Power}";
                else
                    result += $"\n      P --";
                if(description.Length > 0)
                    result += "\n" + description;

                return result;
            }
            set { description = value; }
        }

        public int GetTargetType
        {
            get { return targetType; }
        }

        public Skill SetTargetType(int targetType)
        {
            this.targetType = targetType;

            return this;
        }

        public Skill SetPenetration(double pen)
        {
            this.Penetration = pen;

            return this;
        }

        public Skill(String name, int power)
        {
            tier = 0;
            Name = name;
            this.Power = power;
            Penetration = 0.0;
            Type = SkillType.Physical;
        }

        public Skill(String name, int power, SkillEffect effect)
        {
            tier = 0;
            Name = name;
            this.Power = power;
            Penetration = 0.0;
            Type = SkillType.Physical;
            Effect = effect;
        }

        public Skill(String name, int power, SkillType type)
        {
            tier = 0;
            Name = name;
            this.Power = power;
            Penetration = 0.0;
            Type = type;
            //Effect = effect;
        }

        public Skill(String name, int power, SkillType type, SkillEffect effect)
        {
            tier = 0;
            Name = name;
            this.Power = power;
            Penetration = 0.0;
            Type = type;
            Effect = effect;
        }

        public Skill(String name, SkillEffect effect)
        {
            tier = 0;
            Name = name;
            Power = 0;
            Penetration = 0.0;
            Type = SkillType.Effect;
            Effect = effect;
        }

        public Skill(String name, SkillType type, SkillEffect effect)
        {
            tier = 0;
            Name = name;
            Power = 0;
            Penetration = 0.0;
            Type = type;
            Effect = effect;
        }

        public Skill(String name, SkillType type, SkillEffect effect, string description)
        {
            tier = 0;
            Name = name;
            Power = 0;
            Penetration = 0.0;
            Type = type;
            Effect = effect;
            this.description = description;
        }

        public Skill(String name, int power, SkillType type, SkillEffect effect, string description)
        {
            tier = 0;
            Name = name;
            Power = power;
            Penetration = 0.0;
            Type = type;
            Effect = effect;
            this.description = description;
        }

        public Skill(String name, SkillEffectAll effect)
        {
            tier = 0;
            Name = name;
            Power = 0;
            Penetration = 0.0;
            Type = SkillType.Effect;
            EffectAll = effect;
        }

        public Skill(String name, SkillType type, SkillEffectAll effect)
        {
            tier = 0;
            Name = name;
            Power = 0;
            Penetration = 0.0;
            Type = type;
            EffectAll = effect;
        }

        public Skill(String name, SkillType type, SkillEffectAll effect, string description)
        {
            tier = 0;
            Name = name;
            Power = 0;
            Penetration = 0.0;
            Type = type;
            EffectAll = effect;
            this.description = description;
        }

        public Skill(String name, int power, SkillType type, string description)
        {
            tier = 0;
            Name = name;
            Power = power;
            Penetration = 0.0;
            Type = type;
            //EffectAll = effect;
            this.description = description;
        }

        public Skill(String name, int power, SkillType type, SkillEffectAll effect, string description)
        {
            tier = 0;
            Name = name;
            Power = power;
            Penetration = 0.0;
            Type = type;
            EffectAll = effect;
            this.description = description;
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
