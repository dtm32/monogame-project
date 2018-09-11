using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Game.Units
{
    class Weapon : Skill
    {
        public Weapon(int rank)
            : base(rank)
        {
            this.target = Target.Enemy;
            this.cooldown = 0;
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
