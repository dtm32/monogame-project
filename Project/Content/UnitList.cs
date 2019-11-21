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
            List<Unit> tempList = new List<Unit>(this);
            Queue<int> returnList = new Queue<int>();

            //for(int i = 0; i < units.Length; i++)
            //{
            //    ArrayList tiedUnits = new ArrayList();
            //    int highestSpeed = 0;

            //    for(int j = 0; j < speeds.Length; j++)
            //    {
            //        if(speeds[j] > highestSpeed)
            //        {
            //            highestSpeed = speeds[j];
            //            tiedUnits.Clear();
            //            tiedUnits.Add(j);
            //        }
            //        else if(speeds[j] == highestSpeed)
            //        {
            //            tiedUnits.Add(j);
            //        }
            //    }

            //    if(tiedUnits.Count > 1)
            //    {
            //        // randomly pick between tied units
            //        int randomIndex = rnd.Next(tiedUnits.Count);
            //        int unitIndex = (int)tiedUnits[randomIndex];
            //        // add to move order and set speed to -1
            //        moveOrder[index] = unitIndex;
            //        roundOrder.Enqueue(unitIndex);
            //        speeds[unitIndex] = -1;
            //    }
            //    else if(tiedUnits.Count == 1)
            //    {
            //        int unitIndex = (int)tiedUnits[0];
            //        // add to move order and set speed to -1
            //        moveOrder[index] = unitIndex;
            //        roundOrder.Enqueue(unitIndex);
            //        speeds[unitIndex] = -1;
            //    }

            //    index++;
            //}

            List<Unit> sortedList = tempList.OrderByDescending(unit => unit.Spd).ToList();

            for(int i = 0; i < sortedList.Count; i++)
            {
                returnList.Enqueue(sortedList[i].Index);
            }

            roundOrder = returnList;

            return returnList;
        }

        public Unit Next()
        {
            //if(BattleOver())
            //{
            //    //SetState(BattleState.BattleEnd);
            //    //break;
            //    return null;
            //}
            //else 
            if(roundOrder.Count > 0)
            {
                while(roundOrder.Count > 0)
                {
                    selectedUnitIndex = roundOrder.Dequeue();
                    if(base[selectedUnitIndex].IsAlive())
                    {
                        if(!base[selectedUnitIndex].IsEnemy)
                        {
                            previewAlly = selectedUnitIndex;
                        }
                        return base[selectedUnitIndex];
                    }

                }
            }

            selectedUnitIndex = -1;
            return null;
        }

        public bool BattleOver()
        {
            int count = 0;

            for(int i = 0; i < base.Capacity / 2; i++)
            {
                if(!base[i].IsAlive())
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
                if(!base[i].IsAlive())
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

        public void RoundStart()
        {
            // decrement status effects
            base.ForEach((unit) =>
            {
                if(unit.IsAlive())
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
                if(unit.IsAlive())
                {
                    unit.Update();
                }
            });
        }
    }
}
