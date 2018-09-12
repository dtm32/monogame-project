using Game.GameData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Game.GameData.Skills
{
    class Strength : Skill
    {
        public Strength(int rank)
            : base(rank)
        {
            this.description = new string[] {
                "Gain Strength +10.",
                "Gain Strength +12.",
                "Gain Strength +15.",
                "Gain Strength +20."
            };
            this.passive = true;
            this.target = Target.None;
            this.cooldown = 0;
            this.cooldownTimer = 0;
            this.range = 0;
        }

        public override void TriggerAfterCombat()
        {
            throw new NotImplementedException();
        }

        public override void TriggerBattleStart()
        {
            throw new NotImplementedException();
        }

        public override void TriggerInCombat()
        {
            throw new NotImplementedException();
        }

        public override void TriggerOnDeath(Target unit)
        {
            throw new NotImplementedException();
        }

        public override void TriggerOnTarget(Unit target)
        {
            throw new NotImplementedException();
        }

        public override void TriggerPassive()
        {
            unit.GetStats().ModStrength(10);
        }

        public override void TriggerRoundEnd()
        {
            throw new NotImplementedException();
        }

        public override void TriggerStartCombat()
        {
            throw new NotImplementedException();
        }
    }
}
