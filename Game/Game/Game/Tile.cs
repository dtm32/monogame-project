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


        public void AddTerrain(Terrain newTerrain)
        {
            hasTerrain = true;
            terrain = newTerrain;
        }

        public void RemoveTerrain()
        {
            hasTerrain = false;
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

        //public Texture2D GetUnitTexture()
        //{
        //    return Units.;
        //}

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
            //throw new NotImplementedException();
        }


        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(tileTexture, position, Color.White);

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
