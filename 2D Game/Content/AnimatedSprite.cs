using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2D_Game.Content
{
    class AnimatedSprite
    {
        public Texture2D Texture { get; set; }
        public int Rows { get; set; }
        public int Columns { get; set; }
        public int UpdateSpeed { get; set; }
        private int currentFrame;
        private int totalFrames;
        private int delta = 0;

        public AnimatedSprite(Texture2D texture, int rows, int columns)
        {
            Texture = texture;
            Rows = rows;
            Columns = columns;
            UpdateSpeed = 50;
            currentFrame = 0;
            totalFrames = Rows * Columns;
        }

        public void Update()
        {
            delta++;

            if(delta > UpdateSpeed)
            {
                currentFrame++;
                delta = 0;
            }
            if (currentFrame == totalFrames)
                currentFrame = 0;
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 location)
        {
            int width = Texture.Width / Columns;
            int height = Texture.Height / Rows;
            int row = (int)((float)currentFrame / (float)Columns);
            int column = currentFrame % Columns;

            Rectangle sourceRectangle = new Rectangle(width * column, height * row, width, height);
            Rectangle destinationRectangle = new Rectangle((int)location.X, (int)location.Y, width, height);

            //spriteBatch.Begin();
            //if(delta > 10)
            //{
                spriteBatch.Draw(Texture, destinationRectangle, sourceRectangle, Color.White);
            //    delta = 0;
            //}
            //spriteBatch.End();
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 location, SpriteEffects spriteEffect)
        {
            int width = Texture.Width / Columns;
            int height = Texture.Height / Rows;
            int row = (int)((float)currentFrame / (float)Columns);
            int column = currentFrame % Columns;

            Rectangle sourceRectangle = new Rectangle(width * column, height * row, width, height);
            Rectangle destinationRectangle = new Rectangle((int)location.X, (int)location.Y, width, height);

            //spriteBatch.Begin();
            //if(delta > 10)
            //{
            //spriteBatch.Draw(Texture, destinationRectangle, sourceRectangle, Color.White);
            spriteBatch.Draw(Texture, destinationRectangle, sourceRectangle, Color.White, 0f, Vector2.Zero, spriteEffect, 1f);
            //    delta = 0;
            //}
            //spriteBatch.End();
        }
    }
}
