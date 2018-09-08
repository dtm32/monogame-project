using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Game
{
    public class Tile
    {
        private Vector2   position;
        private Rectangle tileRect;
        private Texture2D tileTexture;
        private bool      hasUnit = false;
        private bool      hasTerrain = false;
         

        public Tile(int x, int y)
        {
            position.X = x;
            position.Y = y;
            tileRect = new Rectangle(x, y, 100, 100);
        }

        public void SetTexture(Texture2D texture)
        {
            tileTexture = texture;
        }

        public void AddUnit(Rectangle unit, Texture2D texture)
        {
            hasUnit = true;
        }

        public Rectangle GetRectangle()
        {
            return tileRect;
        }

        public Texture2D GetTexture()
        {
            return tileTexture;
        }
    }
}
