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
        private Texture2D defaultTexture;
        private Texture2D clickedTexture;
        private Vector2 cursorPosition;
        private Rectangle cursorRect;
        private int viewportWidth, viewportHeight;
        //private MouseState mouseState;
        private ButtonState leftMouseState;
        private ButtonState rightMouseState;
        private bool leftMousePressedLast;
        private bool rightMousePressedLast;

        public Cursor(Texture2D defaultTexture, Texture2D clickedTexture, int viewportWidth, int viewportHeight)
        {
            this.defaultTexture = defaultTexture;
            this.clickedTexture = clickedTexture;
            this.viewportWidth = viewportWidth;
            this.viewportHeight = viewportHeight;
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
            }

            // Update mouse events
            leftMouseState = mouseState.LeftButton;
            rightMouseState = mouseState.RightButton;
            //if (mouseState.LeftButton == ButtonState.Pressed)
            //{
            //    if (!leftMousePressedLast)
            //    {
            //        leftMouseState = mouseState.LeftButton;
            //    }
            //    else
            //    {
            //        leftMouseState = MouseState.None;
            //    }

            //    leftMousePressedLast = true;
            //}
            //else
            //{
            //    if (leftMousePressedLast)
            //    {
            //        leftMouseState = MouseState.MouseUp;
            //    }
            //    else
            //    {
            //        leftMouseState = MouseState.None;
            //    }

            //    leftMousePressedLast = false;
            //}

            //if (mouseState.RightButton == ButtonState.Pressed)
            //{
            //    if (!rightMousePressedLast)
            //    {
            //        rightMouseState = MouseState.MouseDown;
            //    }
            //    else
            //    {
            //        rightMouseState = MouseState.None;
            //    }

            //    rightMousePressedLast = true;
            //}
            //else
            //{
            //    if (rightMousePressedLast)
            //    {
            //        rightMouseState = MouseState.MouseUp;
            //    }
            //    else
            //    {
            //        rightMouseState = MouseState.None;
            //    }

            //    rightMousePressedLast = false;
            //}

            // update cursor on click
            //if (mouseState.LeftButton == ButtonState.Pressed)
            //{
            //    cursorColor = Color.LightSlateGray;
            //}
            //else if (selectedTile != null)
            //{
            //    // Signal unit can attack
            //    cursorColor = Color.Red;
            //}
            //else
            //{
            //    cursorColor = Color.White;
            //}
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
