using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Game.GameData
{
    class Terrain : Drawable
    {
        private Texture2D texture;

        public Terrain(Texture2D terrainTexture)
        {
            texture = terrainTexture;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            throw new NotImplementedException();
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 tilePos)
        {
            // All drawing is relative to parent tile

            tilePos.X += 10;
            tilePos.Y += 20;

            spriteBatch.Draw(texture, tilePos, Color.White);
        }

        public override void Update(GameTime gameTime)
        {
            throw new NotImplementedException();
        }
    }
}
