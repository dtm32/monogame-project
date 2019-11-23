using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2D_Game.Content
{
    class UnitList : List<Unit>
    {
        private Queue<int> roundOrder;
        private int selectedUnitIndex;
        private int previewAlly;
        private int previewEnemy;
        //List<Unit> unitList;

        public UnitList(int size) : base(size)
        {
            roundOrder = new Queue<int>();
            unitReady = new bool[size];
            selectedUnitIndex = -1;
            previewAlly = -1;
            previewEnemy = -1;
        }

        public int Length
        {
            get { return base.Capacity; }
        }

        public bool AddAt(Unit unit, int index)
        {
            if(index < base.Capacity)
            {
                if(index >= base.Capacity / 2)
                {
                    unit.IsEnemy = true;
                }

                base.Insert(index, unit);

                return true;
            }

            return false;
        }

        public List<Unit> AllyList
        {
            get
            {
                return base.GetRange(0, base.Capacity / 2); 
            }
        }

        public List<Unit> EnemyList
        {
            get
            {
                return base.GetRange(base.Capacity / 2, base.Capacity);
            }
        }

        public Unit Select(int index)
        {
            selectedUnitIndex = index;
            return base[index];
        }

        public Unit Selected
        {
            get
            {
                if(selectedUnitIndex == -1)
                    return null;
                return base[selectedUnitIndex];
            }
        }

        public bool SelectedIsEnemy
        {
            get
            {
                if(selectedUnitIndex == -1)
                    return false;
                return base[selectedUnitIndex].IsEnemy;
            }
        }

        public bool SelectedIsAlly
        {
            get
            {
                if(selectedUnitIndex == -1)
                    return false;
                return !base[selectedUnitIndex].IsEnemy;
            }
        }

        public int SelectedIndex
        {
            get { return selectedUnitIndex; }
        }

        public Unit SetPreviewUnit(int index)
        {
            if(index == -1)
            {
                previewEnemy = -1;
                return null;
            }

            if(base[index].IsEnemy)
            {
                previewEnemy = index;
            }
            else
            {
                previewAlly = index;
            }

            return base[index];
        }

        public Unit SetPreviewedAlly(int index)
        {
            previewAlly = index;
            return base[index];
        }

        public Unit PreviewedAlly
        {
            get
            {
                if(previewAlly == -1)
                    return null;
                return base[previewAlly];
            }
        }

        public Unit SetPreviewedEnemey(int index)
        {
            previewEnemy = index;
            return base[index];
        }

        public Unit PreviewedEnemy
        {
            get
            {
                if(previewEnemy == -1)
                    return null;
                return base[previewEnemy];
            }
        }

        public Queue<int> CalcMoveOrder()
        {
            List<Unit> tempList;
            //= new List<Unit>(this);

            //for(int i = 0; i < roundOrder.Count; i++)
            //{
            //    tempList.Add(base[roundOrder[i]]);
            //}
            if(roundOrder.Count == 0)
            {
                tempList = new List<Unit>(this);
            }
            else
            {
                tempList = new List<Unit>();
                for(int i = 0; i < roundOrder.Count; i++)
                {
                    int next = roundOrder.Dequeue();
                    tempList.Add(base[next]);
                    Console.Write($"{next} ");
                }
            }

            Queue<int> returnList = new Queue<int>();


            List<Unit> sortedList = tempList.OrderByDescending(unit => unit.Spd).ToList();

            Console.Write($"Round order:");
            sortedList.ForEach((unit) =>
            {
                Console.Write($" {unit.Name}");
            });
            Console.WriteLine();

            for(int i = 0; i < sortedList.Count; i++)
            {
                returnList.Enqueue(sortedList[i].Index);
            }

            roundOrder = returnList;

            return returnList;
        }

        private List<Unit> roundList = new List<Unit>();

        bool[] unitReady;

        /// <summary>
        /// Decrement status effects and setup internal round order array/queue.
        /// </summary>
        public void RoundStart()
        {
            // decrement status effects
            base.ForEach((unit) =>
            {
                if(unit.IsAlive)
                {
                    if(unit.StatusEffects.Count > 0)
                    {
                        unit.StatusEffects.ForEach((statusEffect) =>
                        {
                            statusEffect.Turns--;
                        });

                        unit.StatusEffects.RemoveAll(status => status.Turns <= 0);
                    }
                }
            });

            // calculate unit move order
            for(int i = 0; i < unitReady.Length; i++)
            {
                if(base[i].IsAlive)
                {
                    unitReady[i] = true;
                }
            }

            //Unit[] tempArray = new Unit[this.Capacity];
            //this.CopyTo(tempArray);
            //roundList = new List<Unit>(tempArray);
            //roundOrder = Sort(this);
        }

        /// <summary>
        /// Check for next alive unit to make a move this round; if no units
        /// left in internal Queue, returns null.
        /// </summary>
        /// <returns>Unit to move next; null if round is over.</returns>
        public Unit Next()
        {

            //if(roundList.Count > 0)
            //{
            //    roundOrder = Sort(roundList);

            //    while(roundOrder.Count > 0)
            //    {
            //        selectedUnitIndex = roundOrder.Dequeue();
            //        roundList.RemoveAt(selectedUnitIndex);
            //        if(base[selectedUnitIndex].IsAlive)
            //        {
            //            if(!base[selectedUnitIndex].IsEnemy)
            //            {
            //                previewAlly = selectedUnitIndex;
            //            }
            //            return base[nextIndex++];
            //        }

            //    }
            //}

            Queue<int> moveQueue = CalculateMoveQueue(this, unitReady);

            //if(nextIndex < base.Count)
            //{
            //    selectedUnitIndex = nextIndex;
            //    if(!base[nextIndex].IsEnemy)
            //        previewAlly = selectedUnitIndex;

            //    return base[nextIndex++];
            //}

            if(moveQueue.Count > 0)
            {
                int index = moveQueue.Dequeue();

                selectedUnitIndex = index;
                unitReady[index] = false;

                if(!base[index].IsEnemy)
                {
                    previewAlly = selectedUnitIndex;
                }

                return base[index];
            }

            selectedUnitIndex = -1;
            return null;
        }

        private Queue<int> CalculateMoveQueue(List<Unit> units, bool[] ready)
        {
            Queue<int> moveQueue = new Queue<int>();
            int[] speeds = new int[units.Count];

            // update ready queue
            for(int i = 0; i < ready.Length; i++)
            {
                if(!units[i].IsAlive)
                {
                    ready[i] = false;
                }
            }

            // add unit's speed to array if unit is ready
            for(int i = 0; i < units.Count; i++)
            {
                if(ready[i])
                {
                    speeds[i] = units[i].Spd;
                }
                else
                {
                    speeds[i] = -1;
                }
            }

            for(int i = 0; i < units.Count; i++)
            {
                List<int> tiedUnits = new List<int>();
                int highestSpeed = 0;

                for(int j = 0; j < speeds.Length; j++)
                {
                    if(speeds[j] > highestSpeed)
                    {
                        highestSpeed = speeds[j];
                        tiedUnits.Clear();
                        tiedUnits.Add(j);
                    }
                    else if(speeds[j] == highestSpeed)
                    {
                        tiedUnits.Add(j);
                    }
                }

                if(tiedUnits.Count > 1)
                {
                    // randomly pick between tied units
                    int randomIndex = rnd.Next(tiedUnits.Count);
                    int unitIndex = (int)tiedUnits[randomIndex];
                    // add to move order and set speed to -1
                    moveQueue.Enqueue(unitIndex);
                    speeds[unitIndex] = -1;
                }
                else if(tiedUnits.Count == 1)
                {
                    int unitIndex = (int)tiedUnits[0];
                    // add to move order and set speed to -1
                    moveQueue.Enqueue(unitIndex);
                    speeds[unitIndex] = -1;
                }
            }

            int[] tempArray = new int[moveQueue.Count];
            moveQueue.CopyTo(tempArray, 0);
            Console.WriteLine("Move queue");
            foreach(int i in tempArray)
            {
                Console.WriteLine($"  {base[i].Name}({base[i].Spd})");
            }
            Console.WriteLine();

            return moveQueue;
        }

        public Queue<int> GetRoundQueue()
        {
            return new Queue<int>(roundOrder);
        }

        Random rnd = new Random();

        private Queue<int> Sort(List<Unit> units)
        {
            Queue<int> moveQueue = new Queue<int>();
            int[] speeds = new int[units.Count];


            for(int i = 0; i < units.Count; i++)
            {
                speeds[i] = units[i].Spd; // TODO: make unit manager that will calc spd w/ modifiers
            }

            for(int i = 0; i < units.Count; i++)
            {
                List<int> tiedUnits = new List<int>();
                int highestSpeed = 0;

                for(int j = 0; j < speeds.Length; j++)
                {
                    if(speeds[j] > highestSpeed)
                    {
                        highestSpeed = speeds[j];
                        tiedUnits.Clear();
                        tiedUnits.Add(j);
                    }
                    else if(speeds[j] == highestSpeed)
                    {
                        tiedUnits.Add(j);
                    }
                }

                if(tiedUnits.Count > 1)
                {
                    // randomly pick between tied units
                    int randomIndex = rnd.Next(tiedUnits.Count);
                    int unitIndex = (int)tiedUnits[randomIndex];
                    // add to move order and set speed to -1
                    //moveOrder[index] = unitIndex;
                    moveQueue.Enqueue(unitIndex);
                    speeds[unitIndex] = -1;
                }
                else if(tiedUnits.Count == 1)
                {
                    int unitIndex = (int)tiedUnits[0];
                    // add to move order and set speed to -1
                    //moveOrder[index] = unitIndex;
                    moveQueue.Enqueue(unitIndex);
                    speeds[unitIndex] = -1;
                }

                Console.Write($"{moveQueue.Peek()} ");

                //index++;
            }


            return moveQueue;
        }

        public bool BattleOver()
        {
            int count = 0;

            for(int i = 0; i < base.Capacity / 2; i++)
            {
                if(!base[i].IsAlive)
                {
                    count++;
                }
            }
            if(count == base.Capacity / 2)
            {
                return true;
            }

            count = 0;

            for(int i = base.Capacity / 2; i < base.Capacity; i++)
            {
                if(!base[i].IsAlive)
                {
                    count++;
                }
            }
            if(count == base.Capacity / 2)
            {
                return true;
            }

            return false;
        }

        public bool IsAnimating()
        {
            for(int i = 0; i < base.Count; i++)
            {
                if(base[i].IsAnimating())
                {
                    return true;
                }
            }

            return false;
        }

        public void Update()
        {
            base.ForEach((unit) =>
            {
                if(unit.IsAlive)
                {
                    unit.Update();
                }
            });
        }
    }
}
