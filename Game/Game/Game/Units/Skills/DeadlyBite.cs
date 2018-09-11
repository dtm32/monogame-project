using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Game.Units.Skills
{
    class DeadlyBite : Skill
    {
        public DeadlyBite(int rank)
            : base(rank)
        {
            this.description = new string[] {
                "Deal damage = 105% Strength.",
                "Deal damage = 110% Strength.",
                "Deal damage = 115% Strength.",
                "Deal damage = 120% Strength."
            };
            this.passive = false;
            this.target = Target.Enemy;
            this.cooldown = 2;
            this.cooldownTimer = 0;
            this.range = 1;
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
            unit.StartCombat(target, range, 1.2f, DamageType.Physical);
        }

        public override void TriggerPassive()
        {
            throw new NotImplementedException();
        }

        public override void TriggerRoundEnd()
        {
            if (this.cooldownTimer > 0)
            {
                cooldownTimer--;
            }
        }

        public override void TriggerStartCombat()
        {
            throw new NotImplementedException();
        }
    }
}
