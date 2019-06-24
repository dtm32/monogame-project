using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Game.GameData
{
    class UnitManager
    {
        private Unit[] units;
        private UnitManagerStatus[] unitStatusArray;
        private int arrayLength;
        private int highestSpeed;

        public UnitManager(int size)
        {
            units = new Unit[size];
            arrayLength = 0;
            highestSpeed = 0;
        }

        public int AddUnit(Unit newUnit)
        {
            units[arrayLength] = newUnit;

            return arrayLength++;
        }

        public Unit GetUnit(int index)
        {
            return units[index];
        }

        public Unit GetNext()
        {
            int nextIndex = -1;

            for(int i = 0; i < arrayLength; i++)
            {
                if(!unitStatusArray[i].GetAction()
                    && unitStatusArray[i].GetSpeed() > highestSpeed)
                {
                    highestSpeed = unitStatusArray[i].GetSpeed();
                    nextIndex = i;
                }
            }

            if (nextIndex != -1)
            {
                return units[nextIndex];
            }
            else
            {
                return null;
            }
        }

        public void NewRound()
        {
            for(int i = 0; i < arrayLength; i++)
            {
                unitStatusArray[i].Reset();
            }
        }
    }
}
