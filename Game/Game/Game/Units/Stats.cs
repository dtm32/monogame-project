using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Game
{
    class Stats
    {
        private int health;
        private int speed;
        private int strength;
        private int focus;
        private int armor;
        private int resistance;

        private int modHealth;
        private int modSpeed;
        private int modStrength;
        private int modFocus;
        private int modArmor;
        private int modResistance;

        private int currentHealth;

        public Stats(int hp, int spd, int str, int fcs, int amr, int res)
        {
            health = hp;
            speed = spd;
            strength = str;
            focus = fcs;
            armor = amr;
            resistance = res;

            modHealth = 0;
            modSpeed = 0;
            modStrength = 0;
            modFocus = 0;
            modArmor = 0;
            modResistance = 0;

            currentHealth = hp;
        }

        public int GetHealth()
        {
            return health + modHealth;
        }

        public int GetSpd()
        {
            return speed + modSpeed;
        }

        public int GetStr()
        {
            return strength + modStrength;
        }

        public int GetFcs()
        {
            return focus + modFocus;
        }

        public int GetAmr()
        {
            return armor + modArmor;
        }

        public int GetRes()
        {
            return resistance + modResistance;
        }

        public bool DecreaseHealth(int amount)
        {
            currentHealth -= amount;

            if (currentHealth > 0)
            {
                return true;
            }
            else
            {
                currentHealth = 0;
                return false;
            }
        }

        public void ResetBuffs()
        {
            modHealth = 0;
            modSpeed = 0;
            modStrength = 0;
            modFocus = 0;
            modArmor = 0;
            modResistance = 0;
        }

        public void LevelUp(int hpChange, int spdChange, int strChange, 
            int fcsChange, int amrChange, int resChange)
        {
            health += hpChange;
            speed += spdChange;
            strength += strChange;
            focus += fcsChange;
            armor += amrChange;
            resistance += resChange;
        }
    }
}
