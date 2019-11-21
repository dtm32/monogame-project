using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
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

        int[] selectedUnits = new int[4];

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

            for(int i = 0; i < selectedUnits.Length; i++)
            {
                selectedUnits[i] = -1;
            }
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

            //spriteLocs = new Vector2[baseUnitList.Count];
            //spriteRects = new Rectangle[baseUnitList.Count];

            //for(int i = 0; i < spriteLocs.Length; i++)
            //{
            //    spriteLocs[i] = new Vector2(windowPadding + i * (Game1.SpriteWidth + spriteMargin), windowPadding);
            //    spriteRects[i] = new Rectangle((int)spriteLocs[i].X, (int)spriteLocs[i].Y, Game1.SpriteWidth, Game1.SpriteWidth);
            //}
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

            StartBattle = startButton.Update(cursor);



            for(int i = 0; i < 4; i++)
            {
                bool unitButtonClicked = unitButtons[i].Update(cursor);

                if(unitButtonClicked && cursor.LeftClick)
                {
                    unitButtonIndex = i;
                }
            }


            if(cursor.LeftClick)
            {
                for(int i = 0; i < spriteRects.Length; i++)
                {
                    if(cursor.Rect.Intersects(spriteRects[i]))
                    {
                        if(unitButtonIndex > -1)
                        {
                            selectedUnits[unitButtonIndex] = i;
                            unitButtons[unitButtonIndex].SetText(baseUnitList[i].Name);
                        }
                    }
                }
            }
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

        }
    }
}
