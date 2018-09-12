using Game.GameData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Game.GameData
{
    abstract class Skill
    {
        protected Unit unit;
        protected string[] description;
        protected string inheritType;
        protected bool passive;
        protected Target target;
        protected DamageType damageType;
        protected Attribute attribute;
        protected UnitType unitType;
        protected int rank;
        protected int cooldown;
        protected int cooldownTimer;
        protected int range;

        public enum Target
        {
            None,
            Enemy,
            Ally,
            Self,
            AllySelf,
            All
        }

        public enum DamageType
        {
            Physical,
            Magical,
            HighestAtk,
            LowestDef
        }

        public enum Attribute
        {
            All,
            Order,
            Chaos,
            Wild,
            Mythic
        }

        public enum UnitType
        {
            Infantry,
            Mage,
            Knight,
            Beast,
            Dragon
        }

        public Skill(int rank)
        {
            this.rank = rank;
 
        }

        public string GetDescription()
        {
            return description[rank];
        }

        public string GetText()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append(this.GetType().Name.Replace("_", " ") + " " + (rank + 1) + "\n");

            if(passive)
            {
                sb.Append("Passive\n");
            }
            else
            {
                sb.Append("Range " + range + " CD " + cooldown + "\n");
            }

            sb.Append(description[rank]);

            return sb.ToString();
        }

        // For passive stat changes
        public abstract void TriggerPassive();

        public abstract void TriggerBattleStart();

        public abstract void TriggerStartCombat();

        public abstract void TriggerInCombat();

        public abstract void TriggerAfterCombat();

        public abstract void TriggerOnDeath(Target unit);

        public abstract void TriggerRoundEnd();

        public abstract void TriggerOnTarget(Unit target);
    }
}
