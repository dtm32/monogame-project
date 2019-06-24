using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Game.GameData
{
    class UnitManagerStatus
    {
        private bool action;
        private bool sleep;
        private bool alive;
        private int speed;

        public UnitManagerStatus(int speed)
        {
            action = false;
            sleep = false;
            alive = true;
            this.speed = speed;
        }

        public void SetAction(bool action)
        {
            this.action = action;
        }

        public bool GetAction()
        {
            return action;
        }

        public int GetSpeed()
        {
            return speed;
        }

        public void Reset()
        {
            if(alive)
            {
                action = false;
                // TODO: sleep counter
            }
        }
    }
}
