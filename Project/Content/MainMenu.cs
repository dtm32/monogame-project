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

        Texture2D blankTexture, skillTexture;

        private Rectangle backgroundRect;
        private Rectangle startRect;

        const int skillWidth = 200;
        const int skillHeight = 45;

        bool startHover = false;


        public MainMenu(GraphicsDeviceManager graphics)
        {
            int viewportWidth = graphics.GraphicsDevice.Viewport.Width;
            int viewportHeight = graphics.GraphicsDevice.Viewport.Height;

            backgroundRect = new Rectangle(0, 0, viewportWidth, viewportHeight);
            startRect = new Rectangle(viewportWidth / 2 - skillWidth / 2, viewportHeight / 2,
                skillWidth, skillHeight);
        }

        public void AddTextures(Texture2D blankTexture, Texture2D skillTexture)
        {
            this.blankTexture = blankTexture;
            this.skillTexture = skillTexture;
        }

        public void Update(Cursor cursor)
        {
            //for(int i = 0; i < 4; i++)
            //{
            if(cursor.Rect.Intersects(startRect))
            {
                startHover = true;
                if(cursor.LeftClick)
                {
                    StartBattle = true;
                }
            }
            else
            {
                startHover = false;
            }
            //}
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(blankTexture, backgroundRect, Color.SlateGray);
            Color hoverColor = Color.White;
            if(startHover)
                hoverColor = Color.LightGray;

            spriteBatch.Draw(skillTexture, startRect, hoverColor);

        }
    }
}
