using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2D_Game.Content
{
    class BattleManager
    {
        enum BattleState
        {
            Wait, // wait for user input
            BattleStart, // display units
            BattleEnd, // calc winner, show end battle screen
            RoundStart, // calc turn order, update status/stats
            RoundRun, // runs anim
            RoundNext, // called after anims finish
            RoundAI,
            RoundEnd,
            AttackAnimation
        }

        // unit consts
        const int SIZE = 8;
        // unit sprite consts
        const int SPRITE_WIDTH = 96;
        const int SPRITE_HEIGHT = 128;
        const int TOP_OFFSET = 30;
        const int HEALTH_BAR_OFFSET = SPRITE_HEIGHT - 16;
        const int HEALTH_BAR_PADDING_X = 2;
        const int HEALTH_BAR_PADDING_Y = 6;
        const int HEALTH_BAR_WIDTH = 72;
        const int HEALTH_BAR_HEIGHT = 6;

        // unit panel consts
        const int unitPanelWidth = 700;
        const int unitPanelHeight = 130;
        const int unitPanelWidthHalf = unitPanelWidth / 2;
        const int unitPanelPadding = 25; // padding on left, right, top
        const int skillWidth = 200;
        const int skillHeight = 45;
        const int skillPadding = 13;

        BattleState battleState = BattleState.BattleStart;

        private Random rnd = new Random();

        Rectangle[] skillRects = new Rectangle[4];
        Vector2[] skillTextLocs = new Vector2[4];
        int skillHover = -1;
        //int skillSelected = -1;
        Skill selectedSkill;
        int skillIndex = -1;
        bool skillSelected = false;


        Unit[] units = new Unit[SIZE];
        Rectangle[] unitRects = new Rectangle[SIZE];
        Rectangle[] unitHealthRects = new Rectangle[SIZE];
        Vector2[] unitLocs = new Vector2[SIZE];
        Unit selectedUnit = null;
        int selectedIndex = 0;
        bool unitSelected = false;
        Texture2D blankTexture, healthBarTexture, skillTexture;
        SpriteFont defaultFont;
        Rectangle unitRect;

        int[] unitHealth = new int[SIZE];

        Queue<int> roundOrder;

        public BattleManager(GraphicsDeviceManager graphics)
        {
            int viewportWidth = graphics.GraphicsDevice.Viewport.Width;
            int viewportHeight = graphics.GraphicsDevice.Viewport.Height;

            for (int i = 0; i < SIZE; i++)
            {
                int posX = viewportWidth / 2 - 300;
                int posY = (int)((i % 4) * (SPRITE_HEIGHT * 1.0) + TOP_OFFSET);

                if (i >= SIZE / 2)
                {
                    posX = viewportWidth / 2 + 300 - SPRITE_WIDTH;
                }

                unitRects[i] = new Rectangle(posX, posY, SPRITE_WIDTH, SPRITE_HEIGHT);
                unitLocs[i] = new Vector2(posX, posY);
                unitHealthRects[i] = new Rectangle(posX + HEALTH_BAR_PADDING_X + HEALTH_BAR_WIDTH, 
                    posY + HEALTH_BAR_PADDING_Y + HEALTH_BAR_OFFSET, 0, HEALTH_BAR_HEIGHT);
            }

            unitRect = new Rectangle(viewportWidth / 2 - unitPanelWidthHalf,
                viewportHeight - unitPanelHeight,
                unitPanelWidthHalf * 2, unitPanelHeight);

            skillRects[0] = new Rectangle(viewportWidth / 2 + unitPanelWidthHalf - unitPanelPadding - skillWidth * 2,
                viewportHeight - unitPanelHeight + unitPanelPadding,
                skillWidth, skillHeight);
            skillRects[1] = new Rectangle(viewportWidth / 2 + unitPanelWidthHalf - unitPanelPadding - skillWidth * 2,
                viewportHeight - unitPanelHeight + unitPanelPadding + skillHeight,
                skillWidth, skillHeight);
            skillRects[2] = new Rectangle(viewportWidth / 2 + unitPanelWidthHalf - unitPanelPadding - skillWidth,
                viewportHeight - unitPanelHeight + unitPanelPadding,
                skillWidth, skillHeight);
            skillRects[3] = new Rectangle(viewportWidth / 2 + unitPanelWidthHalf - unitPanelPadding - skillWidth,
                viewportHeight - unitPanelHeight + unitPanelPadding + skillHeight,
                skillWidth, skillHeight);
            skillTextLocs[0] = new Vector2(viewportWidth / 2 + unitPanelWidthHalf - unitPanelPadding - skillWidth * 2 + skillPadding,
                viewportHeight - unitPanelHeight + unitPanelPadding + skillPadding);
            skillTextLocs[1] = new Vector2(viewportWidth / 2 + unitPanelWidthHalf - unitPanelPadding - skillWidth * 2 + skillPadding,
                viewportHeight - unitPanelHeight + unitPanelPadding + skillHeight + skillPadding);
            skillTextLocs[2] = new Vector2(viewportWidth / 2 + unitPanelWidthHalf - unitPanelPadding - skillWidth + skillPadding,
                viewportHeight - unitPanelHeight + unitPanelPadding + skillPadding);
            skillTextLocs[3] = new Vector2(viewportWidth / 2 + unitPanelWidthHalf - unitPanelPadding - skillWidth + skillPadding,
                viewportHeight - unitPanelHeight + unitPanelPadding + skillHeight + skillPadding);
        }

        public void AddTextures(Texture2D blankTexture, Texture2D healthBarTexture, Texture2D skillTexture)
        {
            this.blankTexture = blankTexture;
            this.healthBarTexture = healthBarTexture;
            this.skillTexture = skillTexture;
        }

        public void AddFonts(SpriteFont defaultFont)
        {
            this.defaultFont = defaultFont;
        }

        public bool AddUnit(Unit newUnit, int index)
        {
            if(index < SIZE)
            {
                units[index] = newUnit;
                unitHealth[index] = newUnit.HP;

                if(index >= SIZE / 2)
                {
                    units[index].IsEnemy = true;
                }
            }

            return false;
        }

        /// <summary>
        /// Run after passing all required resources to BattleManager.
        /// </summary>
        public void Start()
        {
            battleState = BattleState.RoundStart;
        }

        private void SetUnitHealth(float percentMax, int unitIndex)
        {
            percentMax = MathHelper.Clamp(percentMax, 0, 1);

            unitHealthRects[unitIndex].Width = (int) (HEALTH_BAR_WIDTH * (1.0 - percentMax));
            unitHealthRects[unitIndex].X = (int)unitLocs[unitIndex].X + HEALTH_BAR_PADDING_X + HEALTH_BAR_WIDTH - (int)unitHealthRects[unitIndex].Width;
        }

        int attackFrame = 0;

        public void Update(Cursor cursor)
        {
            // update animations
            for(int i = 0; i < units.Length; i++)
            {
                units[i].Texture.Update();
            }

            // check cursor hover
            skillHover = -1;
            for(int i = 0; i < 4; i++)
            {
                if (cursor.Rect.Intersects(skillRects[i]))
                {
                    skillHover = i;
                    break;
                }
            }

            // check mouse cursor position
            if(cursor.LeftClick && battleState == BattleState.Wait) // TODO: move to switch inside "wait" case
            {
                int unitClicked = CheckUnitIntersect(cursor.Rect);
                int skillClicked = CheckSkillIntersect(cursor.Rect);
                skillIndex = skillClicked;

                // attack unit with selected skill
                if (unitSelected && skillSelected && unitClicked != -1)
                {
                    int damage = selectedSkill.Power;
                    unitHealth[unitClicked] -= damage;
                    float percentHealth = unitHealth[unitClicked] / (float)units[unitClicked].HP;
                    SetUnitHealth(percentHealth, unitClicked);

                    // next unit's turn
                    // run animation
                    battleState = BattleState.RoundNext;
                }

                // not clicking on unit panel
                if(!cursor.Rect.Intersects(unitRect))
                {
                    //unitSelected = false;
                    skillSelected = false;
                }

                if(unitClicked != -1)
                {
                }
                else if(skillClicked != -1)
                {
                    //skillSelected = i;
                    selectedSkill = (Skill) selectedUnit.GetSkills()[skillClicked];
                    skillIndex = skillClicked;
                    skillSelected = true;
                }
            }

            switch(battleState)
            {
                case BattleState.RoundStart:
                    // TODO: round start damage/healing
                    // TODO: update/remove stat changes
                    // TODO: update/remove status effects

                    // calculate character move order
                    //ArrayList moveOrder = new ArrayList();
                    ArrayList unitsArray = new ArrayList();
                    int[] moveOrder = new int[units.Length];
                    int[] speeds = new int[units.Length];
                    int index = 0;

                    roundOrder = new Queue<int>();

                    for (int i = 0; i < units.Length; i++)
                    {
                        speeds[i] = units[i].Spd; // TODO: make unit manager that will calc spd w/ modifiers
                    }

                    for(int i = 0; i < units.Length; i++)
                    {
                        ArrayList tiedUnits = new ArrayList();
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
                            int unitIndex = (int) tiedUnits[randomIndex];
                            // add to move order and set speed to -1
                            moveOrder[index] = unitIndex;
                            roundOrder.Enqueue(unitIndex);
                            speeds[unitIndex] = -1;
                        }
                        else if(tiedUnits.Count == 1)
                        {
                            int unitIndex = (int) tiedUnits[0];
                            // add to move order and set speed to -1
                            moveOrder[index] = unitIndex;
                            roundOrder.Enqueue(unitIndex);
                            speeds[unitIndex] = -1;
                        }

                        index++;
                    }

                    //for (int i = 0; i < moveOrder.Length; i++)
                    //{
                    //    Console.Write(moveOrder[i] + " ");
                    //}
                    //Console.WriteLine("Checking queue l " + roundOrder.Count);
                    //Console.WriteLine("Checking queue " + roundOrder.Peek());
                    //for (int i = 0; i < moveOrder.Length; i++)
                    //{
                    //    Console.WriteLine("Next: " + roundOrder.Dequeue() + " ");
                    //}
                    
                    // update battle state
                    battleState = BattleState.RoundNext;
                    break;
                case BattleState.RoundNext:
                    int nextIndex;

                    if (roundOrder.Count > 0)
                    {
                        nextIndex = roundOrder.Dequeue();
                    }
                    else
                    {
                        battleState = BattleState.RoundEnd;
                        break;
                    }

                    selectedUnit = units[nextIndex];
                    selectedIndex = nextIndex;
                    Console.WriteLine("Current selectedIndex = " + selectedIndex);
                    unitSelected = true;

                    // update battle state
                    if(selectedUnit.IsEnemy)
                    {
                        battleState = BattleState.RoundAI;
                    }
                    else
                    {
                        battleState = BattleState.Wait;
                    }
                    break;
                case BattleState.RoundAI:
                    if(attackFrame < 50)
                    {
                        // move enemy unit left
                        unitLocs[selectedIndex].X = unitRects[selectedIndex].X - (attackFrame / 5);
                        //unitHealthRects[selectedIndex].X;
                    }
                    else if(attackFrame == 50)
                    {
                        // deal damage
                        int targetUnit = rnd.Next(4);
                        int damage = selectedSkill.Power;
                        unitHealth[targetUnit] -= damage;
                        float percentHealth = unitHealth[targetUnit] / (float)units[targetUnit].HP;
                        SetUnitHealth(percentHealth, targetUnit);

                    }
                    else if(attackFrame < 100)
                    {
                        unitLocs[selectedIndex].X = unitRects[selectedIndex].X - ((100 - attackFrame) / 5);
                    }
                    else if(attackFrame == 100)
                    {
                        unitLocs[selectedIndex].X = unitRects[selectedIndex].X;
                    }
                    else
                    {
                        attackFrame = 0;
                        battleState = BattleState.RoundNext;
                        break;
                    }
                    attackFrame++;
                    break;
            }
        }

        private int CheckUnitIntersect(Rectangle cursorRect)
        {
            for (int i = 0; i < unitRects.Length; i++)
            {
                if (cursorRect.Intersects(unitRects[i]))
                {
                    return i;
                }
            }

            return -1;
        }

        private int CheckSkillIntersect(Rectangle cursorRect)
        {
            // if no unit is selected, a skill cannot be selected
            if(!unitSelected)
            {
                return -1;
            }

            for (int i = 0; i < skillRects.Length; i++)
            {
                if (cursorRect.Intersects(skillRects[i]))
                {
                    return i;
                }
            }

            return -1;
        }

        //double healthbarFrames = 0;

        public void Draw(SpriteBatch spriteBatch)
        {
            // draw units
            for (int i = 0; i < 8; i++)
            {
                Color defaultColor = Color.White;
                if (selectedIndex == i)
                {
                    //float r = (float) (204.0 * Math.Sin(healthbarFrames));
                    //float g = (float) (255.0 * Math.Sin(healthbarFrames));
                    defaultColor = Color.Red;
                    //defaultColor = Color.Orange;

                    //if(healthbarFrames < Math.PI)
                    //{
                    //    healthbarFrames += Math.PI / 100;
                    //}
                    //else
                    //{
                    //    healthbarFrames = 0;
                    //}
                }

                if(i >= SIZE / 2)
                {
                    units[i].Texture.Draw(spriteBatch, unitLocs[i], SpriteEffects.FlipHorizontally);
                    //spriteBatch.Draw(units[i].Texture, unitRects[i], null, temp, 0f, Vector2.Zero, SpriteEffects.FlipHorizontally, 1f);
                }
                else
                {
                    units[i].Texture.Draw(spriteBatch, unitLocs[i]);
                }
                spriteBatch.Draw(healthBarTexture, new Rectangle((int)unitLocs[i].X, (int)unitLocs[i].Y + HEALTH_BAR_OFFSET, SPRITE_WIDTH, 18), defaultColor);
                spriteBatch.Draw(blankTexture, unitHealthRects[i], Color.Red);
            }

            // draw unit panel if player unit is selected
            if(unitSelected && !selectedUnit.IsEnemy)
            {
                spriteBatch.Draw(blankTexture, unitRect, Color.DarkBlue);

                ArrayList skills = selectedUnit.GetSkills();
                for(int i = 0; i < 4; i++)
                {
                    Color skillColor = Color.White;
                    if (skillHover == i || skillIndex == i)
                        skillColor = Color.LightGray;
                    spriteBatch.Draw(skillTexture, skillRects[i], skillColor);
                    spriteBatch.DrawString(defaultFont, ((Skill)skills[i]).Name + "  " + ((Skill)skills[i]).Power.ToString(), skillTextLocs[i], Color.Black);
                }
            }
        }
    }
}
