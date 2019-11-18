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

        Vector2[] defaultUnitLocations = new Vector2[SIZE];

        // unit panel consts
        const int unitPanelWidth = 700;
        const int unitPanelHeight = 130;
        const int unitPanelWidthHalf = unitPanelWidth / 2;
        const int unitPanelPadding = 25; // padding on left, right, top
        const int skillWidth = 200;
        const int skillHeight = 45;
        const int skillPadding = 13;

        // unit preview variables
        Rectangle allyUnitPreviewRect;
        Rectangle enemyUnitPreviewRect;

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
        //Rectangle[] unitHealthRects = new Rectangle[SIZE];
        //Rectangle[] unitHealthRectsPrev = new Rectangle[SIZE];
        //int[] healthBarDelay = new int[SIZE];
        HealthBar[] healthBars = new HealthBar[SIZE];
        Vector2[] unitLocs = new Vector2[SIZE];
        Unit selectedUnit = null;
        int selectedIndex = 0;
        bool unitSelected = false;
        Texture2D blankTexture, healthBarTexture, skillTexture;
        SpriteFont defaultFont;
        Rectangle unitRect;

        //int[] unitHealth = new int[SIZE];
        AnimatedSprite fireSprite;
        AnimatedSprite poisonedSprite;

        Queue<int> roundOrder;

        public BattleManager(GraphicsDeviceManager graphics)
        {
            int viewportWidth = graphics.GraphicsDevice.Viewport.Width;
            int viewportHeight = graphics.GraphicsDevice.Viewport.Height;

            for(int i = 0; i < SIZE; i++)
            {
                int posX = viewportWidth / 2 - 300;
                int posY = (int)((i % 4) * (SPRITE_HEIGHT * 1.0) + TOP_OFFSET);

                if(i >= SIZE / 2)
                {
                    posX = viewportWidth / 2 + 300 - SPRITE_WIDTH;
                }

                unitRects[i] = new Rectangle(posX, posY, SPRITE_WIDTH, SPRITE_HEIGHT);
                unitLocs[i] = new Vector2(posX, posY);
                defaultUnitLocations[i] = new Vector2(posX, posY);
                //unitHealthRects[i] = new Rectangle(posX + HEALTH_BAR_PADDING_X, 
                //    posY + HEALTH_BAR_PADDING_Y + HEALTH_BAR_OFFSET, 
                //    HEALTH_BAR_WIDTH, HEALTH_BAR_HEIGHT);
                //unitHealthRectsPrev[i] = new Rectangle(posX + HEALTH_BAR_PADDING_X, 
                //    posY + HEALTH_BAR_PADDING_Y + HEALTH_BAR_OFFSET, 
                //    HEALTH_BAR_WIDTH, HEALTH_BAR_HEIGHT);
                //healthBarDelay[i] = -1;
                //healthBars[i] = new HealthBar(healthBarTexture, blankTexture, unitLocs[i].X, unitLocs[i].Y + HEALTH_BAR_OFFSET);
            }

            // position unit panel rect
            unitRect = new Rectangle(viewportWidth / 2 - unitPanelWidthHalf,
                viewportHeight - unitPanelHeight,
                unitPanelWidthHalf * 2, unitPanelHeight);

            // position unit preview panel rects
            allyUnitPreviewRect = new Rectangle(0, viewportHeight - unitPanelHeight, 200, unitPanelHeight);
            enemyUnitPreviewRect = new Rectangle(viewportWidth - 200, viewportHeight - unitPanelHeight, 200, unitPanelHeight);

            // position skill rects and text
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

        Texture2D panelCornerTexture;

        public void AddTextures(Texture2D blankTexture, Texture2D healthBarTexture, Texture2D skillTexture, Texture2D panelCornerTexture)
        {
            this.blankTexture = blankTexture;
            this.healthBarTexture = healthBarTexture;
            this.skillTexture = skillTexture;
            this.panelCornerTexture = panelCornerTexture;
        }

        public void AddSprites(AnimatedSprite fireSprite, AnimatedSprite poisonedSprite)
        {
            this.fireSprite = fireSprite;
            this.poisonedSprite = poisonedSprite;
        }

        public void AddFonts(SpriteFont defaultFont)
        {
            this.defaultFont = defaultFont;
        }

        public bool AddUnit(BaseUnit newUnit, int index)
        {
            if(index < SIZE)
            {
                units[index] = new Unit(newUnit);

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
        public bool Start()
        {
            for(int i = 0; i < SIZE; i++)
            {
                int posX = (int)unitLocs[i].X;
                int posY = (int)unitLocs[i].Y + HEALTH_BAR_OFFSET;
                healthBars[i] = new HealthBar(healthBarTexture, blankTexture, posX, posY);
            }

            // TODO: check that all required textures are loaded before starting

            // update state
            battleState = BattleState.RoundStart;

            return true;
        }

        //private void SetUnitHealth(float percentMax, int unitIndex)
        //{
        //    percentMax = MathHelper.Clamp(percentMax, 0, 1);

        //    unitHealthRects[unitIndex].Width = (int) (HEALTH_BAR_WIDTH * percentMax);
        //    //unitHealthRects[unitIndex].X = (int)unitLocs[unitIndex].X + HEALTH_BAR_PADDING_X + HEALTH_BAR_WIDTH - (int)unitHealthRects[unitIndex].Width;
        //}

        int attackFrame = 0;

        public void Update(Cursor cursor)
        {
            // update animations
            for(int i = 0; i < units.Length; i++)
            {
                if(units[i].IsAlive())
                {
                    units[i].Update();
                }
            }

            // check cursor hover
            skillHover = -1;
            for(int i = 0; i < 4; i++)
            {
                if(cursor.Rect.Intersects(skillRects[i]))
                {
                    skillHover = i;
                    break;
                }
            }

            // check health bar updates
            //int healthDelayConst = 30;
            for(int i = 0; i < healthBars.Length; i++)
            {
                healthBars[i].Update();
            }

            fireSprite.Update();
            poisonedSprite.Update();

            // right click unit preview
            if(cursor.RightClick)
            {
                int unitClicked = CheckUnitIntersect(cursor.Rect);
                //int skillClicked = CheckSkillIntersect(cursor.Rect);
                //skillIndex = skillClicked;


                if(unitClicked != -1)
                {
                    if(units[unitClicked].IsEnemy)
                    {
                        previewEnemy = units[unitClicked];
                    }
                    else
                    {
                        previewAlly = units[unitClicked];
                    }
                }
                else
                {
                    previewEnemy = null;
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

                    for(int i = 0; i < units.Length; i++)
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
                            int unitIndex = (int)tiedUnits[randomIndex];
                            // add to move order and set speed to -1
                            moveOrder[index] = unitIndex;
                            roundOrder.Enqueue(unitIndex);
                            speeds[unitIndex] = -1;
                        }
                        else if(tiedUnits.Count == 1)
                        {
                            int unitIndex = (int)tiedUnits[0];
                            // add to move order and set speed to -1
                            moveOrder[index] = unitIndex;
                            roundOrder.Enqueue(unitIndex);
                            speeds[unitIndex] = -1;
                        }

                        index++;
                    }

                    // decrement status effects
                    for(int i = 0; i < units.Length; i++)
                    {
                        if(units[i].IsAlive())
                        {
                            if(units[i].StatusEffects.Count > 0)
                            {
                                units[i].StatusEffects.ForEach((statusEffect) =>
                                {
                                    if(statusEffect.Turns > 0)
                                        statusEffect.Turns--;
                                });

                                units[i].StatusEffects.RemoveAll(status => status.Turns <= 0);
                            }
                        }
                    }

                    // update battle state
                    SetState(BattleState.RoundNext);
                    break;
                case BattleState.RoundNext:
                    int nextIndex;

                    if(BattleOver())
                    {
                        SetState(BattleState.BattleEnd);
                        break;
                    }
                    else if(roundOrder.Count > 0)
                    {
                        nextIndex = roundOrder.Dequeue();
                    }
                    else
                    {
                        SetState(BattleState.RoundEnd);
                        break;
                    }

                    selectedUnit = units[nextIndex];
                    selectedIndex = nextIndex;
                    unitSelected = true;

                    bool nextIsAlive = units[nextIndex].IsAlive();

                    // update battle state
                    if(selectedUnit.IsEnemy && nextIsAlive)
                    {
                        //enemy
                        SetState(BattleState.RoundAI);
                    }
                    else if(nextIsAlive)
                    {
                        previewAlly = selectedUnit;
                        SetState(BattleState.Wait);
                    }
                    else
                    {
                        SetState(BattleState.RoundNext);
                    }
                    break;
                case BattleState.Wait:
                    if(cursor.LeftClick)
                    {
                        int unitClicked = CheckUnitIntersect(cursor.Rect);
                        int skillClicked = CheckSkillIntersect(cursor.Rect);
                        skillIndex = skillClicked;

                        // attack unit with selected skill
                        if(unitSelected && skillSelected && unitClicked != -1)
                        {
                            int damage = CombatCalculation(units[unitClicked], selectedUnit, selectedSkill);
                            healthBars[unitClicked].Set(units[unitClicked].PercentHealth());
                            healthBars[selectedIndex].Set(selectedUnit.PercentHealth());

                            // next unit's turn
                            // run animation
                            selectedUnit.StartAttackAnimation();

                            // update game state
                            SetState(BattleState.RoundRun);
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
                            selectedSkill = (Skill)selectedUnit.Skills[skillClicked];
                            skillIndex = skillClicked;
                            skillSelected = true;
                        }
                    }
                    break;
                case BattleState.RoundAI:
                    // deal damage
                    Skill aiSkill = (Skill)selectedUnit.Skills[rnd.Next(4)];
                    int targetUnit = FindTarget(selectedUnit, aiSkill);

                    int aiDamage = CombatCalculation(units[targetUnit], selectedUnit, aiSkill);

                    healthBars[targetUnit].Set(units[targetUnit].PercentHealth());
                    healthBars[selectedIndex].Set(selectedUnit.PercentHealth());

                    selectedUnit.StartAttackAnimation();
                    SetState(BattleState.RoundRun);
                    break;
                case BattleState.RoundRun:
                    // check for animations to wait for
                    bool nextState = true;

                    // check if health bars are animating
                    for(int i = 0; i < healthBars.Length; i++)
                    {
                        if(healthBars[i].IsAnimating())
                        {
                            nextState = false;
                        }
                    }

                    // check for attack animation
                    for(int i = 0; i < units.Length; i++)
                    {
                        if(units[i].IsAnimating())
                        {
                            nextState = false;
                        }
                    }

                    if(nextState)
                    {
                        SetState(BattleState.RoundNext);
                    }
                    break;
                case BattleState.RoundEnd:
                    // calculate end round damage (bleed, poison, etc)
                    for(int i = 0; i < units.Length; i++)
                    {
                        if(units[i].IsAlive())
                        {
                            if(units[i].StatusEffects.Count > 0)
                            {
                                // handle round end status effects
                                for(int j = 0; j < units[i].StatusEffects.Count; j++)
                                {
                                    int damage = 0;
                                    switch(units[i].StatusEffects[j].Type)
                                    {
                                        case StatusEffect.Bleed:
                                            damage = units[i].StatusEffects[j].Damage;
                                            units[i].CurrHP -= damage;
                                            Console.WriteLine($"{units[i].Name} takes {damage} damage from Bleed!");
                                            break;
                                        case StatusEffect.Burn:
                                            damage = units[i].StatusEffects[j].Damage;
                                            units[i].CurrHP -= damage;
                                            Console.WriteLine($"{units[i].Name} takes {damage} damage from Burn!");
                                            break;
                                        case StatusEffect.Poison:
                                            damage = units[i].StatusEffects[j].Damage;
                                            units[i].CurrHP -= damage;
                                            Console.WriteLine($"{units[i].Name} takes {damage} damage from Poison!");
                                            break;
                                    }

                                    healthBars[i].Set(units[i].PercentHealth());

                                    //SetUnitHealth(units[i].PercentHealth(), i);
                                }
                            }
                        }
                    }
                    SetState(BattleState.RoundStart);
                    break;
                case BattleState.BattleEnd:
                    Console.WriteLine("Battle over, ");
                    bool win = false;
                    for(int i = 0; i < SIZE / 2; i++)
                    {
                        if(units[i].IsAlive())
                        {
                            Console.WriteLine(" you win!");
                            win = true;
                            break;
                        }
                    }
                    if(!win)
                        Console.WriteLine(" you lose!");
                    break;
            }
        }

        private int CheckUnitIntersect(Rectangle cursorRect)
        {
            for(int i = 0; i < unitRects.Length; i++)
            {
                if(cursorRect.Intersects(unitRects[i]))
                {
                    return i;
                }
            }

            return -1;
        }

        private int FindTarget(Unit unit, Skill skill)
        {
            // TODO: add move advanced target selecting logic
            //if(skill.Type )
            while(true)
            {
                int target = rnd.Next(4);

                if(units[target].IsAlive())
                    return target;
            }
        }

        private int CombatCalculation(Unit target, Unit attacker, Skill skill)
        {
            int damage = 0;
            int attack = 0;
            int defense = 0;
            double crit = 1.0;

            if(skill.Type == Skill.SkillType.Physical)
            {
                attack = attacker.Str;
                defense = target.Amr;
            }
            else if(skill.Type == Skill.SkillType.Magical)
            {
                attack = attacker.Fcs;
                defense = target.Res;
            }
            else if(skill.Type == Skill.SkillType.Effect)
            {
                skill.Effect?.Invoke(attacker, target);

                Console.WriteLine($"{attacker.Name} used {skill.Name} on {target.Name}!");

                return 0;
            }

            if(rnd.Next(100) < 5)
            {
                crit = 1.5; // Also check for skill crit modifier
            }

            //damage = (int)((skill.Power * attack * (attacker.Level * attacker.Level / 18 + 1)) / (defense * defense * skill.Penetration) * (rnd.Next(15) / 100 + 0.85) * crit);
            //damage = (int)((skill.Power * attack / defense * (attacker.Level / 3 + 1) / 30) * (rnd.Next(15) / 100 + 0.85) * crit);
            damage = (int)((skill.Power * attack / defense) * (rnd.Next(15) / 100 + 0.85) * crit);

            if(damage < 1)
                damage = 1;

            target.CurrHP -= damage;

            skill.Effect?.Invoke(attacker, target);

            Console.WriteLine($"{attacker.Name} used {skill.Name} on {target.Name} for {damage} damage!");
            if(crit > 1.0)
                Console.WriteLine($"Critical hit!");

            return damage;
        }

        private bool BattleOver()
        {
            int count = 0;

            for(int i = 0; i < SIZE / 2; i++)
            {
                if(!units[i].IsAlive())
                {
                    count++;
                }
            }
            if(count == SIZE / 2)
            {
                return true;
            }

            count = 0;

            for(int i = SIZE / 2; i < SIZE; i++)
            {
                if(!units[i].IsAlive())
                {
                    count++;
                }
            }
            if(count == SIZE / 2)
            {
                return true;
            }

            return false;
        }

        private int CheckSkillIntersect(Rectangle cursorRect)
        {
            // if no unit is selected, a skill cannot be selected
            if(!unitSelected)
            {
                return -1;
            }

            for(int i = 0; i < skillRects.Length; i++)
            {
                if(cursorRect.Intersects(skillRects[i]))
                {
                    return i;
                }
            }

            return -1;
        }

        private BattleState SetState(BattleState state)
        {
            battleState = state;

            return state;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            // draw units
            for(int i = 0; i < 8; i++)
            {
                Color defaultColor = Color.White;
                if(selectedIndex == i)
                {
                    defaultColor = Color.MediumPurple;
                }

                Color unitColor = Color.White;
                if(units[i].IsAlive() || healthBars[i].Width > 0)
                {
                    units[i].Draw(spriteBatch, unitLocs[i]);

                    healthBars[i].Draw(spriteBatch, defaultColor);

                    if(units[i].StatusEffects.Count > 0)
                    {
                        // handle round end status effects
                        for(int j = 0; j < units[i].StatusEffects.Count; j++)
                        {
                            switch(units[i].StatusEffects[j].Type)
                            {
                                case StatusEffect.Bleed:
                                    break;
                                case StatusEffect.Burn:
                                    fireSprite.Draw(spriteBatch, unitLocs[i]);
                                    break;
                                case StatusEffect.Poison:
                                    poisonedSprite.Draw(spriteBatch, unitLocs[i]);
                                    break;
                            }

                            healthBars[i].Set(units[i].PercentHealth());
                        }
                    }
                }

            }

            // draw unit panel if player unit is selected
            if(unitSelected && !selectedUnit.IsEnemy)
            {
                DrawUnitPanel(spriteBatch);

                spriteBatch.DrawString(Game1.font, selectedUnit.Name.ToUpper(), new Vector2(unitRect.X + unitPanelPadding, unitRect.Y + unitPanelPadding - 5), Color.Black);

                ArrayList skills = selectedUnit.Skills;
                for(int i = 0; i < 4; i++)
                {
                    Color skillColor = Color.White;
                    String skillText = ((Skill)skills[i]).Name;
                    if(skillHover == i || skillIndex == i)
                        skillColor = Color.LightGray;
                    if(((Skill)skills[i]).Type != Skill.SkillType.Effect)
                        skillText += $" {((Skill)skills[i]).Power}";
                    spriteBatch.Draw(skillTexture, skillRects[i], skillColor);
                    spriteBatch.DrawString(Game1.FontSmallBold, skillText, skillTextLocs[i], Color.Black);
                }
            }

            if(previewAlly != null)
            {
                DrawUnitPreview(spriteBatch);
            }
            if(previewEnemy != null)
            {
                DrawEnemyPreview(spriteBatch);
            }
        }

        Unit previewAlly = null;
        Unit previewEnemy = null;

        private void DrawUnitPanel(SpriteBatch spriteBatch)
        {
            Rectangle borderRect = new Rectangle(unitRect.X - 4, unitRect.Y - 4, unitRect.Width + 8, unitRect.Height + 4);
            Rectangle leftCornerRect = new Rectangle(unitRect.X, unitRect.Y, 30, 30);
            Rectangle rightCornerRect = new Rectangle(unitRect.X + unitRect.Width - 30, unitRect.Y, 30, 30);

            spriteBatch.Draw(blankTexture, borderRect, new Color(64, 64, 64));
            spriteBatch.Draw(blankTexture, unitRect, Color.SlateGray);

            spriteBatch.Draw(panelCornerTexture, leftCornerRect, Color.White);
            spriteBatch.Draw(panelCornerTexture, rightCornerRect, null, Color.White, 0f, Vector2.Zero, SpriteEffects.FlipHorizontally, 1f);
        }

        private void DrawUnitPreview(SpriteBatch spriteBatch)
        {
            int unitPreviewPadding = 20;
            Rectangle borderRect = new Rectangle(0, allyUnitPreviewRect.Y - 4, allyUnitPreviewRect.Width + 4, allyUnitPreviewRect.Height + 4);
            Rectangle rightCornerRect = new Rectangle(allyUnitPreviewRect.Width - 30, allyUnitPreviewRect.Y, 30, 30);
            Vector2 nameLoc = new Vector2(unitPreviewPadding, allyUnitPreviewRect.Y + unitPreviewPadding);
            Vector2 spriteLoc = new Vector2(unitPreviewPadding + 50, allyUnitPreviewRect.Y + unitPreviewPadding);
            Vector2 levelLoc = new Vector2(unitPreviewPadding, nameLoc.Y + 17);
            Vector2 hpLoc = new Vector2(unitPreviewPadding, levelLoc.Y + 29);
            Vector2 spdLoc = new Vector2(unitPreviewPadding + 80, hpLoc.Y);
            Vector2 strLoc = new Vector2(unitPreviewPadding, spdLoc.Y + 17);
            Vector2 fcsLoc = new Vector2(unitPreviewPadding + 80, strLoc.Y);
            Vector2 amrLoc = new Vector2(unitPreviewPadding, fcsLoc.Y + 17);
            Vector2 resLoc = new Vector2(unitPreviewPadding + 80, amrLoc.Y);
            
            spriteBatch.Draw(blankTexture, borderRect, new Color(64, 64, 64));
            spriteBatch.Draw(blankTexture, allyUnitPreviewRect, Color.SlateGray);

            spriteBatch.DrawString(Game1.FontSmallBold, previewAlly.Name, nameLoc, Color.Black);
            spriteBatch.DrawString(Game1.FontSmallBold, $"HP  {previewAlly.HP}", hpLoc, Color.Black);
            spriteBatch.DrawString(Game1.FontSmallBold, $"LVL  {previewAlly.Level}", levelLoc, Color.Black);
            spriteBatch.DrawString(Game1.FontSmallBold, $"SPD  {previewAlly.Spd}", spdLoc, Color.Black);
            spriteBatch.DrawString(Game1.FontSmallBold, $"STR  {previewAlly.Str}", strLoc, Color.Black);
            spriteBatch.DrawString(Game1.FontSmallBold, $"FCS  {previewAlly.Fcs}", fcsLoc, Color.Black);
            spriteBatch.DrawString(Game1.FontSmallBold, $"AMR  {previewAlly.Amr}", amrLoc, Color.Black);
            spriteBatch.DrawString(Game1.FontSmallBold, $"RES  {previewAlly.Res}", resLoc, Color.Black);

            spriteBatch.Draw(panelCornerTexture, rightCornerRect, null, Color.White, 0f, Vector2.Zero, SpriteEffects.FlipHorizontally, 1f);

            //previewAlly.Draw(spriteBatch, spriteLoc);
        }

        private void DrawEnemyPreview(SpriteBatch spriteBatch)
        {
            int unitPreviewPadding = 20;
            int unitPreviewPaddingLeft = 25;
            Rectangle borderRect = new Rectangle(enemyUnitPreviewRect.X - 4, enemyUnitPreviewRect.Y - 4, enemyUnitPreviewRect.Width + 4, enemyUnitPreviewRect.Height + 4);
            Rectangle leftCornerRect = new Rectangle(enemyUnitPreviewRect.X, enemyUnitPreviewRect.Y, 30, 30);
            Vector2 nameLoc = new Vector2(enemyUnitPreviewRect.X + unitPreviewPaddingLeft, enemyUnitPreviewRect.Y + unitPreviewPadding);
            Vector2 spriteLoc = new Vector2(nameLoc.X + 50, enemyUnitPreviewRect.Y + unitPreviewPadding);
            Vector2 levelLoc = new Vector2(nameLoc.X, nameLoc.Y + 17);
            Vector2 hpLoc = new Vector2(nameLoc.X, levelLoc.Y + 29);
            Vector2 spdLoc = new Vector2(nameLoc.X + 80, hpLoc.Y);
            Vector2 strLoc = new Vector2(nameLoc.X, spdLoc.Y + 17);
            Vector2 fcsLoc = new Vector2(nameLoc.X + 80, strLoc.Y);
            Vector2 amrLoc = new Vector2(nameLoc.X, fcsLoc.Y + 17);
            Vector2 resLoc = new Vector2(nameLoc.X + 80, amrLoc.Y);

            spriteBatch.Draw(blankTexture, borderRect, new Color(64, 64, 64));
            spriteBatch.Draw(blankTexture, enemyUnitPreviewRect, Color.SlateGray);

            spriteBatch.DrawString(Game1.FontSmallBold, previewEnemy.Name, nameLoc, Color.Black);
            spriteBatch.DrawString(Game1.FontSmallBold, $"HP  {previewEnemy.HP}", hpLoc, Color.Black);
            spriteBatch.DrawString(Game1.FontSmallBold, $"LVL  {previewEnemy.Level}", levelLoc, Color.Black);
            spriteBatch.DrawString(Game1.FontSmallBold, $"SPD  {previewEnemy.Spd}", spdLoc, Color.Black);
            spriteBatch.DrawString(Game1.FontSmallBold, $"STR  {previewEnemy.Str}", strLoc, Color.Black);
            spriteBatch.DrawString(Game1.FontSmallBold, $"FCS  {previewEnemy.Fcs}", fcsLoc, Color.Black);
            spriteBatch.DrawString(Game1.FontSmallBold, $"AMR  {previewEnemy.Amr}", amrLoc, Color.Black);
            spriteBatch.DrawString(Game1.FontSmallBold, $"RES  {previewEnemy.Res}", resLoc, Color.Black);

            spriteBatch.Draw(panelCornerTexture, leftCornerRect, Color.White);

            //previewAlly.Draw(spriteBatch, spriteLoc);
        }
    }
}
