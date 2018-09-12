using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Game.GameData.Skills
{
    class Steady_March : Skill
    {
        public Steady_March(int rank)
            : base(rank)
        {
            this.description = new string[] {
                "Gain 10 Strength.",
                "Gain 12 Strength.",
                "Gain 15 Strength.",
                "Gain Health +20 and Strength/Defense +8."
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
            throw new NotImplementedException();
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
