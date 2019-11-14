using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2D_Game.Content
{
    class BattleManager
    {
        const int SIZE = 8;

        Unit[] units = new Unit[SIZE];
        Rectangle[] tileRects = new Rectangle[SIZE];
        Vector2[] unitLocs = new Vector2[SIZE];

        public BattleManager(GraphicsDeviceManager graphics)
        {
            int viewportWidth = graphics.GraphicsDevice.Viewport.Width;
            int viewportHeight = graphics.GraphicsDevice.Viewport.Height;

            int SPRITE_WIDTH = 96;
            int SPRITE_HEIGHT = 128;
            int TOP_OFFSET = 80;

            for (int i = 0; i < SIZE; i++)
            {
                int posX = viewportWidth / 2 - 300;
                int posY = (int)((i % 4) * (SPRITE_HEIGHT * 0.9) + TOP_OFFSET);

                if (i >= SIZE / 2)
                {
                    posX = viewportWidth / 2 + 300 - SPRITE_WIDTH;
                }

                tileRects[i] = new Rectangle(posX, posY, SPRITE_WIDTH, SPRITE_HEIGHT);
                unitLocs[i] = new Vector2(posX, posY);
            }
        }

        public bool AddUnit(Unit newUnit, int index)
        {
            if(index < SIZE)
            {
                units[index] = newUnit;
            }

            return false;
        }

        public void Update()
        {
            for(int i = 0; i < units.Length; i++)
            {
                units[i].Texture.Update();
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < 8; i++)
            {
                Color temp = Color.White;
                if (i % 2 == 0)
                    temp = Color.White;

                if(i >= SIZE / 2)
                {
                    units[i].Texture.Draw(spriteBatch, unitLocs[i], SpriteEffects.FlipHorizontally);
                    //spriteBatch.Draw(units[i].Texture, tileRects[i], null, temp, 0f, Vector2.Zero, SpriteEffects.FlipHorizontally, 1f);
                }
                else
                {
                    units[i].Texture.Draw(spriteBatch, unitLocs[i]);
                }
            }
        }
    }
}
