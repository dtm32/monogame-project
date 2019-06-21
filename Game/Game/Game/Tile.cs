using Game.GameData;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Game
{
    class Tile : Drawable
    {
        private Vector2   position;
        private Rectangle tileRect;
        private Texture2D tileTexture;
        private bool      hasUnit = false;
        private Unit      unit;
        private Vector2   unitPos;
        private Rectangle unitRect;
        private bool      hasTerrain = false;
        private Terrain   terrain;
        private bool      hasHighlight = false;
        private Color     highlight = Color.White;
         

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

        public void AddUnit(Unit newUnit)
        {
            hasUnit = true;
            unit = newUnit;
            unitPos.X = position.X + 2;
            unitPos.Y = position.Y - 40;
            unitRect = new Rectangle((int) unitPos.X, (int) unitPos.Y, 100, 138);
        }

        public Unit RemoveUnit()
        {
            Unit tempUnit = unit;

            hasUnit = false;
            unit = null;
            unitRect = Rectangle.Empty;

            return tempUnit;
        }

        public bool HasUnit()
        {
            return hasUnit;
        }

        public void AddTerrain(Terrain newTerrain)
        {
            hasTerrain = true;
            terrain = newTerrain;
        }

        public void RemoveTerrain()
        {
            hasTerrain = false;
        }

        public bool HasTerrain()
        {
            return hasTerrain;
        }

        public void HighlightTile(Color highlightColor)
        {
            hasHighlight = true;
            highlight = highlightColor;
        }

        public void RemoveHighlight()
        {
            hasHighlight = false;
            highlight = Color.White;
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

        public Unit GetUnit()
        {
            return unit;
        }

        public override void Update(GameTime gameTime)
        {
            if(hasUnit)
            {
                unit.Update(gameTime);
            }
        }


        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(tileTexture, position, highlight);

            if (hasTerrain)
            {
                terrain.Draw(spriteBatch, position);
            }
            if (hasUnit)
            {
                unit.Draw(spriteBatch, position);
            }

        }
    }
}
