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

        private Queue<Unit> queue;

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
            return queue.Dequeue();
        }

        public void NewRound()
        {
            bool[] inQueue = new bool[arrayLength];
            int maxSpeed;
            int maxSpeedIndex = -1;

            while(queue.Count < arrayLength)
            {
                maxSpeed = 0;
                maxSpeedIndex = -1;

                for(int i = 0; i < arrayLength; i++)
                {
                    if(!inQueue[i] && units[i].GetStats().GetSpd() > maxSpeed)
                    {
                        maxSpeed = units[i].GetStats().GetSpd();
                        maxSpeedIndex = i;
                    }
                }

                queue.Enqueue(units[maxSpeedIndex]);
            }
        }
    }
}
