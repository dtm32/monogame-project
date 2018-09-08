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
        private Vector2   unitPos;
        private Rectangle unitRect;
        private Texture2D unitTexture;
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

        public void AddUnit(Texture2D texture)
        {
            hasUnit = true;
            unitPos.X = position.X + 2;
            unitPos.Y = position.Y - 40;
            unitRect = new Rectangle((int) unitPos.X, (int) unitPos.Y, 100, 138);
            unitTexture = texture;
        }

        public bool HasUnit()
        {
            return hasUnit;
        }

        public Rectangle GetRectangle()
        {
            return tileRect;
        }

        public Texture2D GetTexture()
        {
            return tileTexture;
        }

        public Rectangle GetUnitRectangle()
        {
            return unitRect;
        }

        public Texture2D GetUnitTexture()
        {
            return unitTexture;
        }
    }
}
