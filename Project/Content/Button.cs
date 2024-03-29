﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2D_Game.Content
{
    class Button
    {
        public static int AlignLeft = 0;
        public static int AlignCenter = 1;
        public static int AlignRight = 2;

        //Texture2D texture;
        public Texture2D Texture { get; set; }

        Rectangle positionRect;
        Color buttonColor;
        Color hoverColor;
        Color textColor = new Color(33, 33, 33);
        SpriteFont spriteFont;
        Vector2 textPos;
        string text;
        public bool Hover { get; private set; }
        int align;

        public Button(Rectangle positionRect, string text, int align)
        {
            this.positionRect = positionRect;
            this.text = text;
            this.align = align;

            //hoverColor = Color.White;
            //textPos = new Vector2(positionRect.X + 10, positionRect.Y + 10);
        }

        public Button(Texture2D texture, Rectangle positionRect, string text)
        {
            this.Texture = texture;
            this.positionRect = positionRect;
            this.text = text;

            //hoverColor = Color.White;
        }

        public Button Initialize()
        {
            Vector2 textPadding = new Vector2(0, 0);
            spriteFont = FontManager.Default_Regular_11;
            Vector2 spriteSize = spriteFont.MeasureString(text);
            textPadding.Y = (positionRect.Height - (int)spriteSize.Y) / 2;

            if(align == AlignCenter)
            {
                textPadding.X = (positionRect.Width - spriteSize.X) / 2;
            }
            else
            {
                textPadding.X = textPadding.Y;
            }

            //Console.WriteLine($"{textPadding.X}:{textPadding.Y}");

            textPos = new Vector2(positionRect.X + (int)textPadding.X, positionRect.Y + (int)textPadding.Y);
            hoverColor = Color.LightGray;
            Hover = false;

            return this;
        }

        public void SetText(string text)
        {
            this.text = text;
            Vector2 textPadding = new Vector2(0, 0);
            spriteFont = FontManager.Default_Regular_11;
            Vector2 spriteSize = spriteFont.MeasureString(text);
            textPadding.Y = (positionRect.Height - (int)spriteSize.Y) / 2;

            if(align == AlignCenter)
            {
                textPadding.X = (positionRect.Width - spriteSize.X) / 2;
            }
            else
            {
                textPadding.X = textPadding.Y;
            }

            textPos = new Vector2(positionRect.X + (int)textPadding.X, positionRect.Y + (int)textPadding.Y);

        }


        /// <summary>
        /// Check for button itersect with mouse cursor.
        /// </summary>
        /// <returns>Returns bool value represeting whether button was clicked.</returns>
        public bool Update(Cursor cursor)
        {
            if(cursor.Rect.Intersects(positionRect))
            {
                Hover = true;
                if(cursor.LeftClick)
                {
                    //StartBattle = true;
                    return true;
                }
            }
            else
            {
                Hover = false;
            }

            return false;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if(Texture != null)
            {
                if(Hover)
                {
                    spriteBatch.Draw(Texture, positionRect, hoverColor);
                }
                else
                {
                    spriteBatch.Draw(Texture, positionRect, Color.White);
                }
            }

            if(spriteFont != null)
            {
                spriteBatch.DrawString(spriteFont, text, textPos, textColor);
            }
        }

        public void Draw(SpriteBatch spriteBatch, Color color)
        {
            if(Texture != null)
            {
                spriteBatch.Draw(Texture, positionRect, color);
            }

            if(spriteFont != null)
            {
                spriteBatch.DrawString(spriteFont, text, textPos, textColor);
            }
        }
    }
}
