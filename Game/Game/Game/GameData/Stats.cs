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

        public void ModHealth(float mod)
        {
            if (mod < 1)
            {
                modHealth += (int)(health * mod);
            }
            else
            {
                modHealth += (int)mod;
            }
        }

        public void ModSpeed(float mod)
        {
            if (mod < 1)
            {
                modSpeed += (int)(speed * mod);
            }
            else
            {
                modSpeed += (int)mod;
            }
        }

        public void ModStrength(float mod)
        {
            if (mod < 1)
            {
                modStrength += (int)(strength * mod);
            }
            else
            {
                modStrength += (int)mod;
            }
        }

        public void ModFocus(float mod)
        {
            if (mod < 1)
            {
                modFocus += (int)(focus * mod);
            }
            else
            {
                modFocus += (int)mod;
            }
        }

        public void ModArmor(float mod)
        {
            if (mod < 1)
            {
                modArmor += (int)(armor * mod);
            }
            else
            {
                modArmor += (int)mod;
            }
        }

        public void ModResistance(float mod)
        {
            if (mod < 1)
            {
                modResistance += (int)(resistance * mod);
            }
            else
            {
                modResistance += (int)mod;
            }
        }

        public int GetCurrentHealth()
        {
            return currentHealth;
        }

        public bool IsAlive()
        {
            return currentHealth > 0;
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

        public string[] ToArray()
        {
            string[] array =
            {
                "HP: " + currentHealth + "/" + (health + modHealth),
                "Spd: " + (speed + modSpeed),
                "Str: " + (strength + modStrength),
                "Fcs: " + (focus + modFocus),
                "Amr: " + (armor + modArmor),
                "Res: " + (resistance + modResistance)
            };

            return array;
        }
    }
}
