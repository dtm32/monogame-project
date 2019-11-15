using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2D_Game.Content
{
    class Cursor
    {
        public Rectangle Rect;
        public bool LeftClick;
        public bool RightClick;
        private Texture2D defaultTexture;
        private Texture2D clickedTexture;
        private Rectangle cursorRect;
        private Vector2 cursorPosition;
        private int viewportWidth, viewportHeight;
        //private MouseState mouseState;
        private ButtonState leftMouseState;
        private ButtonState rightMouseState;
        private ButtonState leftMouseLastState;
        private ButtonState rightMouseLastState;
        private bool leftMousePressedLast;
        private bool rightMousePressedLast;

        public Cursor(Texture2D defaultTexture, Texture2D clickedTexture, int viewportWidth, int viewportHeight)
        {
            this.defaultTexture = defaultTexture;
            this.clickedTexture = clickedTexture;
            this.viewportWidth = viewportWidth;
            this.viewportHeight = viewportHeight;
            Rect = new Rectangle(0, 0, 1, 1);
            cursorRect = new Rectangle(0, 0, 26, 26);
            cursorPosition = new Vector2(0, 0);
        }

        public void Update(MouseState mouseState)
        {
            cursorPosition.X = mouseState.X;
            cursorPosition.Y = mouseState.Y;

            if (cursorPosition.X >= 0 && cursorPosition.X <= viewportWidth &&
                cursorPosition.Y >= 0 && cursorPosition.Y <= viewportHeight)
            {
                cursorPosition.X = MathHelper.Clamp(cursorPosition.X, 0, viewportWidth - cursorRect.Width);
                cursorPosition.Y = MathHelper.Clamp(cursorPosition.Y, 0, viewportHeight - cursorRect.Height);

                cursorRect.X = (int)cursorPosition.X;
                cursorRect.Y = (int)cursorPosition.Y;
                Rect.X = cursorRect.X;
                Rect.Y = cursorRect.Y;
            }

            // Update mouse events
            leftMouseState = mouseState.LeftButton;
            rightMouseState = mouseState.RightButton;

            if(leftMouseState == ButtonState.Released && leftMouseLastState == ButtonState.Pressed)
            {
                LeftClick = true;
            }
            else
            {
                LeftClick = false;
            }

            if(rightMouseState == ButtonState.Released && rightMouseLastState == ButtonState.Pressed)
            {
                RightClick = true;
            }
            else
            {
                RightClick = false;
            }

            leftMouseLastState = leftMouseState;
            rightMouseLastState = rightMouseState;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (leftMouseState == ButtonState.Pressed)
                spriteBatch.Draw(clickedTexture, cursorRect, Color.White);
            else
                spriteBatch.Draw(defaultTexture, cursorRect, Color.White);
        }
    }
}
