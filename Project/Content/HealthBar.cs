using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2D_Game.Content
{
    class HealthBar
    {
        private const int TEXTURE_WIDTH = 96;
        private const int TEXTURE_HEIGHT = 18;
        private const int HEALTH_BAR_PADDING_X = 2;
        private const int HEALTH_BAR_PADDING_Y = 6;
        private const int HEALTH_BAR_WIDTH = 72;
        private const int HEALTH_BAR_HEIGHT = 6;
        private const int HEALTH_DELAY = 30;

        private Texture2D texture, blankTexture;
        private AnimatedSprite highlightSprite;
        private Rectangle textureRect;
        private Rectangle rect;
        private Rectangle prevRect;
        private int delay;
        private int setWidth;

        public HealthBar(Texture2D healthBarTexture, AnimatedSprite highlightSprite, Texture2D blankTexture, int x, int y)
        {
            texture = healthBarTexture;
            this.blankTexture = blankTexture;
            this.highlightSprite = highlightSprite;
            textureRect = new Rectangle(x, y, TEXTURE_WIDTH, TEXTURE_HEIGHT);
            rect = new Rectangle(x + HEALTH_BAR_PADDING_X, y + HEALTH_BAR_PADDING_Y,
                HEALTH_BAR_WIDTH, HEALTH_BAR_HEIGHT);
            prevRect = new Rectangle(x + HEALTH_BAR_PADDING_X, y + HEALTH_BAR_PADDING_Y,
                HEALTH_BAR_WIDTH, HEALTH_BAR_HEIGHT);
            delay = -1;
            setWidth = 0;
        }

        public int Width
        {
            get { return prevRect.Width; }
            set { rect.Width = value; }
        }

        public void Set(float percentMax)
        {
            percentMax = MathHelper.Clamp(percentMax, 0, 1);

            setWidth = (int)(HEALTH_BAR_WIDTH * percentMax);

            if(setWidth > rect.Width)
            {
                
            }
            else
            {
                rect.Width = setWidth;
            }
        }

        public bool IsAnimating()
        {
            if(rect.Width < prevRect.Width || rect.Width < setWidth)
            {
                return true;
            }

            return false;
        }

        public void Reset()
        {
            highlightSprite.Reset();
        }

        public void Update()
        {
            highlightSprite.Update();

            if (rect.Width < prevRect.Width)
            {
                if (delay == -1)
                {
                    delay = HEALTH_DELAY;
                }
                else if (delay > 0)
                {
                    delay--;
                }
                else
                {
                    prevRect.Width--;
                    if (rect.Width == prevRect.Width)
                    {
                        delay = -1;
                    }
                }
            }
            else if(rect.Width < setWidth)
            {
                if (delay == -1)
                {
                    delay = HEALTH_DELAY;
                }
                else if (delay > 0)
                {
                    delay--;
                }
                else
                {
                    rect.Width++;
                    prevRect.Width++;
                    if (rect.Width == setWidth)
                    {
                        delay = -1;
                    }
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch, bool highlighted)
        {
            if(highlighted)
            {
                Rectangle highlightRect = new Rectangle(textureRect.X - 2, textureRect.Y - 2,
                    textureRect.Width + 4, textureRect.Height + 4);
                Vector2 highlightLoc = new Vector2(textureRect.X - 2, textureRect.Y - 2);
                highlightSprite.Draw(spriteBatch, highlightLoc);
                //spriteBatch.Draw(highlightTexture, highlightRect, Color.White);
                spriteBatch.Draw(blankTexture, prevRect, Color.Orange);
                spriteBatch.Draw(blankTexture, rect, Color.Green);
            }
            else
            {
                spriteBatch.Draw(texture, textureRect, Color.White);
                spriteBatch.Draw(blankTexture, prevRect, Color.Orange);
                spriteBatch.Draw(blankTexture, rect, Color.Green);
            }
        }
    }
}
