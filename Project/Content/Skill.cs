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

        //public enum TargetType
        //{
        //    Single,
        //    All,
        //    FrontLine,
        //    Adjacent
        //}
        public static int TargetSingle = 0;
        public static int TargetAll = 1;
        public static int TargetFront = 2;
        public static int TargetAdjacent = 3;
        public static int TargetAlly = 3;
        public static int TargetSelf = 4;

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
