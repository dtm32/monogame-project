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
    class MainMenu
    {

        public bool StartBattle = false;

        Texture2D blankTexture, skillTexture, grassBackgroundTexture;

        private Rectangle backgroundRect;
        private Rectangle coverRect;
        private Rectangle borderRect;
        private Rectangle startRect;

        const int skillWidth = 200;
        const int skillHeight = 45;
        const int windowPadding = 100;

        bool startHover = false;

        List<AnimatedSprite> spriteList;
        List<BaseUnit> baseUnitList;
        Vector2[] spriteLocs;
        Rectangle[] spriteRects;

        public int[] SelectedUnits = new int[4];

        Button startButton;

        Rectangle[] unitButtonRects = new Rectangle[4];
        Button[] unitButtons = new Button[4];
        int unitButtonIndex = -1;


        public MainMenu(GraphicsDeviceManager graphics)
        {
            int viewportWidth = graphics.GraphicsDevice.Viewport.Width;
            int viewportHeight = graphics.GraphicsDevice.Viewport.Height;

            backgroundRect = new Rectangle(0, 0, viewportWidth, viewportHeight);
            borderRect = new Rectangle(0, windowPadding + Game1.SpriteHeight + 6, viewportWidth, 4);
            coverRect = new Rectangle(0, windowPadding + Game1.SpriteHeight + 10, viewportWidth, viewportHeight);
            startRect = new Rectangle(viewportWidth - windowPadding - skillWidth, 
                viewportHeight - windowPadding - skillHeight, skillWidth, skillHeight);
            startButton = new Button(startRect, "Start Game", Button.AlignCenter);
            for(int i = 0; i < 4; i++)
            {
                unitButtonRects[i] = new Rectangle(startRect.X, startRect.Y - skillHeight * (5 - i), 
                    skillWidth, skillHeight);
                unitButtons[i] = new Button(unitButtonRects[i], "--", Button.AlignCenter);
            }

            for(int i = 0; i < SelectedUnits.Length; i++)
            {
                SelectedUnits[i] = -1;
            }


            skillRects[0] = new Rectangle(425, 358, skillWidth, skillHeight);
            skillRects[1] = new Rectangle(425, skillRects[0].Y + 45, skillWidth, skillHeight);
            skillRects[2] = new Rectangle(425, skillRects[1].Y + 45, skillWidth, skillHeight);
            skillRects[3] = new Rectangle(425, skillRects[2].Y + 45, skillWidth, skillHeight);

        }

        public void Init()
        {

        }

        public void AddTextures(Texture2D blankTexture, Texture2D skillTexture,
            Texture2D grassBackgroundTexture)
        {
            this.blankTexture = blankTexture;
            this.skillTexture = skillTexture;
            this.grassBackgroundTexture = grassBackgroundTexture;

            startButton.Texture = skillTexture;
            startButton.Init();
            for(int i = 0; i < 4; i++)
            {
                unitButtons[i].Texture = skillTexture;
                unitButtons[i].Init();
            }
        }

        public void AddUnitSprites(List<AnimatedSprite> spriteList)
        {
            int spriteMargin = 16;
            this.spriteList = spriteList;

            spriteLocs = new Vector2[spriteList.Count];
            spriteRects = new Rectangle[spriteList.Count];

            for(int i = 0; i < spriteLocs.Length; i++)
            {
                spriteLocs[i] = new Vector2(windowPadding + i * (Game1.SpriteWidth + spriteMargin), windowPadding);
                spriteRects[i] = new Rectangle((int)spriteLocs[i].X, (int)spriteLocs[i].Y, Game1.SpriteWidth, Game1.SpriteWidth);
            }
        }

        public void AddBaseUnits(List<BaseUnit> baseUnitList)
        {
            //int spriteMargin = 16;
            this.baseUnitList = baseUnitList;

            previewUnit = baseUnitList[0];
        }

        IconManager iconManager;

        public void AddIcons(IconManager iconManager)
        {
            this.iconManager = iconManager;
        }

        public void Update(Cursor cursor)
        {
            //for(int i = 0; i < 4; i++)
            //{
            //if(cursor.Rect.Intersects(startRect))
            //{
            //    startHover = true;
            //    if(cursor.LeftClick)
            //    {

            spriteList.ForEach((sprite) =>
            {
                sprite.Update();
            });

            bool teamPicked = true;

            for(int i = 0; i < SelectedUnits.Length; i++)
            {
                if(SelectedUnits[i] == -1)
                {
                    teamPicked = false;
                }
            }

            if(teamPicked)
            {
                StartBattle = startButton.Update(cursor);
            }
            else
            {
                startButton.Update(cursor);
            }



            for(int i = 0; i < 4; i++)
            {
                bool unitButtonClicked = unitButtons[i].Update(cursor);

                if(unitButtonClicked && cursor.LeftClick)
                {
                    unitButtonIndex = i;
                }
            }

            // over unit sprite
            for(int i = 0; i < spriteRects.Length; i++)
            {
                if(cursor.Rect.Intersects(spriteRects[i]))
                {
                    if(cursor.LeftClick)
                    {
                        if(unitButtonIndex > -1)
                        {
                            SelectedUnits[unitButtonIndex] = i;
                            unitButtons[unitButtonIndex].SetText(baseUnitList[i].Name);
                        }
                    }
                    previewUnit = baseUnitList[i];
                }
            }

            int skillHovered = CheckSkillIntersect(cursor.Rect);

            if(skillHovered > -1)
            {
                if(prevSkillHovered == skillHovered)
                {
                    skillHoverFrames++;

                    if(skillHoverFrames > 20)
                    {
                        //skillPreview = (Skill)previewUnit.GetSkills()[skillHovered];

                        skillPreview = (Skill)previewUnit.GetSkills()[skillHovered];
                        skillDescriptionRect.X = cursor.Rect.X;

                        string description = skillPreview.Description;

                        description = FontManager.WrapText(FontManager.Default_Regular_9, description, skillDescriptionRect.Width - 2 * skillDescriptionRectPadding);

                        int numLines = description.Split('\n').Length;
                        int textHeight = FontManager.Default_Regular_9.LineSpacing * numLines;
                        int panelHeight = textHeight + 2 * skillDescriptionRectPadding + 1;

                        skillDescriptionRect.Y = cursor.Rect.Y - panelHeight;
                    }
                }
                else
                {
                    prevSkillHovered = skillHovered;
                    skillHoverFrames = 0;
                    skillPreview = null;
                }
            }
            else
            {
                skillPreview = null;
            }
        }

        Rectangle skillDescriptionRect = new Rectangle(10, 10, 200, 0);
        int skillDescriptionRectPadding = 10;
        Skill skillPreview;
        int prevSkillHovered = -1;
        int skillHoverFrames = 0;

        private int CheckSkillIntersect(Rectangle cursorRect)
        {
            // if no unit is selected, a skill cannot be selected
            if(previewUnit == null)
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

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(grassBackgroundTexture, backgroundRect, Color.White);
            spriteBatch.Draw(blankTexture, coverRect, Color.SlateGray);
            spriteBatch.Draw(blankTexture, borderRect, new Color(64, 64, 64));
            Color hoverColor = Color.White;
            if(startHover)
                hoverColor = Color.LightGray;

            //spriteBatch.Draw(skillTexture, startRect, hoverColor);
            startButton.Draw(spriteBatch);


            spriteBatch.DrawString(FontManager.Default_Bold_15, "Select Units",
                new Vector2(unitButtonRects[0].X + 10, unitButtonRects[0].Y - 25), Color.Black);
            for(int i = 0; i < 4; i++)
            {
                if(unitButtonIndex == i)
                {
                    unitButtons[i].Draw(spriteBatch, Color.LightGray);
                }
                else
                {
                    unitButtons[i].Draw(spriteBatch);
                }
            }


            for(int i = 0; i < spriteList.Count; i++)
            {
                spriteList[i].Draw(spriteBatch, spriteLocs[i]);
            }

            DrawUnitPreview(spriteBatch, previewUnit);
        }

        Rectangle statPreviewRect = new Rectangle(200, 350, 200, 200);
        BaseUnit previewUnit = null;

        private void DrawUnitPreview(SpriteBatch spriteBatch, BaseUnit unit)
        {
            if(unit != null)
            {
                BaseUnit previewAlly = previewUnit;
                int unitPreviewPadding = 18;
                int statRightMargin = 100;
                int startBotMargin = 24;
                Rectangle borderRect = new Rectangle(statPreviewRect.X - 2, statPreviewRect.Y - 2,
                    statPreviewRect.Width + 4, statPreviewRect.Height + 4);
                //Rectangle rightCornerRect = new Rectangle(allyUnitPreviewRect.Width - 30, allyUnitPreviewRect.Y, 30, 30);
                Vector2 nameLoc = new Vector2(statPreviewRect.X + unitPreviewPadding, 
                    statPreviewRect.Y + unitPreviewPadding);
                Vector2 spriteLoc = new Vector2(nameLoc.X + 50, statPreviewRect.Y + unitPreviewPadding);
                Vector2 levelLoc = new Vector2(nameLoc.X, nameLoc.Y + 25);
                Vector2 hpLoc = new Vector2(nameLoc.X, levelLoc.Y + startBotMargin);
                Vector2 spdLoc = new Vector2(nameLoc.X + statRightMargin, hpLoc.Y);
                Vector2 strLoc = new Vector2(nameLoc.X, spdLoc.Y + startBotMargin);
                Vector2 fcsLoc = new Vector2(nameLoc.X + statRightMargin, strLoc.Y);
                Vector2 amrLoc = new Vector2(nameLoc.X, fcsLoc.Y + startBotMargin);
                Vector2 resLoc = new Vector2(nameLoc.X + statRightMargin, amrLoc.Y);
                Vector2 totalLoc = new Vector2(nameLoc.X, amrLoc.Y + startBotMargin * 2);

                spriteBatch.Draw(blankTexture, borderRect, new Color(64, 64, 64));
                spriteBatch.Draw(blankTexture, statPreviewRect, Color.SlateGray);

                spriteBatch.DrawString(FontManager.Default_Bold_15, previewAlly.Name, nameLoc, Color.Black);
                spriteBatch.DrawString(FontManager.Default_Regular_11, $"HP  {previewAlly.HP}", hpLoc, Color.Black);
                spriteBatch.DrawString(FontManager.Default_Regular_11, $"LVL  {previewAlly.Level}", levelLoc, Color.Black);
                spriteBatch.DrawString(FontManager.Default_Regular_11, $"SPD  {previewAlly.Spd}", spdLoc, Color.Black);
                spriteBatch.DrawString(FontManager.Default_Regular_11, $"STR  {previewAlly.Str}", strLoc, Color.Black);
                spriteBatch.DrawString(FontManager.Default_Regular_11, $"FCS  {previewAlly.Fcs}", fcsLoc, Color.Black);
                spriteBatch.DrawString(FontManager.Default_Regular_11, $"AMR  {previewAlly.Amr}", amrLoc, Color.Black);
                spriteBatch.DrawString(FontManager.Default_Regular_11, $"RES  {previewAlly.Res}", resLoc, Color.Black);
                spriteBatch.DrawString(FontManager.Default_Regular_11, $"TOTAL  {previewAlly.StatTotal}", totalLoc, Color.Black);

                //spriteBatch.Draw(panelCornerTexture, rightCornerRect, null, Color.White, 0f, Vector2.Zero, SpriteEffects.FlipHorizontally, 1f);

                //previewAlly.Draw(spriteBatch, spriteLoc);

                DrawUnitSkills(spriteBatch, unit);
            }
        }

        Rectangle[] skillRects = new Rectangle[4];


        private void DrawUnitSkills(SpriteBatch spriteBatch, BaseUnit unit)
        {
            BaseUnit selectedUnit = unit;
            int textPadding = 13;
            //Rectangle borderRect = new Rectangle(unitRect.X - 4, unitRect.Y - 4, unitRect.Width + 8, unitRect.Height + 4);
            //Rectangle leftCornerRect = new Rectangle(unitRect.X, unitRect.Y, 30, 30);
            //Rectangle rightCornerRect = new Rectangle(unitRect.X + unitRect.Width - 30, unitRect.Y, 30, 30);

            //spriteBatch.Draw(blankTexture, borderRect, new Color(64, 64, 64));
            //spriteBatch.Draw(blankTexture, unitRect, Color.SlateGray);

            //spriteBatch.Draw(panelCornerTexture, leftCornerRect, Color.White);
            //spriteBatch.Draw(panelCornerTexture, rightCornerRect, null, Color.White, 0f, Vector2.Zero, SpriteEffects.FlipHorizontally, 1f);

            //spriteBatch.DrawString(FontManager.Default_Bold_15, selectedUnit.Name.ToUpper(),
            //    new Vector2(unitRect.X + unitPanelPadding, unitRect.Y + unitPanelPadding - 5), Color.Black);


            // 400, 200 -> 400, 400
            Vector2[] skillTextLocs = new Vector2[4];
            for(int i = 0; i < skillTextLocs.Length; i++)
            {
                skillTextLocs[i] = new Vector2(skillRects[i].X + textPadding, skillRects[i].Y + textPadding);
            }

            ArrayList skills = selectedUnit.GetSkills();
            for(int i = 0; i < skills.Count; i++)
            {
                Color skillColor = Color.White;
                String skillText = ((Skill)skills[i]).Name;
                //if(skillHover == i || skillIndex == i)
                //    skillColor = Color.LightGray;
                //if(((Skill)skills[i]).Type != Skill.SkillType.Effect &&
                //    ((Skill)skills[i]).Type != Skill.SkillType.Buff)
                //    skillText += $" {((Skill)skills[i]).Power}";
                spriteBatch.Draw(skillTexture, skillRects[i], skillColor);
                spriteBatch.DrawString(FontManager.Default_Regular_11, skillText, skillTextLocs[i], Color.Black);
            }

            if(skillPreview != null)
                DrawSkillDescription(spriteBatch);
        }

        private void DrawSkillDescription(SpriteBatch spriteBatch)
        {
            Rectangle skillDescriptionBorderRect = new Rectangle(
                skillDescriptionRect.X - 2, skillDescriptionRect.Y - 2,
                skillDescriptionRect.Width + 4, skillDescriptionRect.Height + 4);
            Vector2 textLoc = new Vector2(skillDescriptionRect.X + skillDescriptionRectPadding,
                skillDescriptionRect.Y + skillDescriptionRectPadding);
            string description = skillPreview.Description;

            description = FontManager.WrapText(FontManager.Default_Regular_9, description,
                skillDescriptionRect.Width - 2 * skillDescriptionRectPadding);

            int numLines = description.Split('\n').Length;
            int textHeight = FontManager.Default_Regular_9.LineSpacing * numLines;

            skillDescriptionRect.Height = textHeight + 2 * skillDescriptionRectPadding;

            //skillDescriptionRect.Y -= skillDescriptionRect.Height;

            // draw panel
            spriteBatch.Draw(blankTexture, skillDescriptionBorderRect, new Color(64, 64, 64));
            spriteBatch.Draw(blankTexture, skillDescriptionRect, Color.SlateGray);

            // draw text
            spriteBatch.DrawString(FontManager.Default_Regular_9, description, textLoc, Color.Black);

            // draw skill type icon
            Rectangle typeRect = new Rectangle(skillDescriptionRect.X + skillDescriptionRectPadding - 1,
                skillDescriptionRect.Y + skillDescriptionRectPadding + 13, 16, 16);
            spriteBatch.Draw(iconManager.SkillTypeIcon(skillPreview.Type), typeRect, Color.White);
        }
    }
}
