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
            this.unit = newUnit;
            unitPos.X = position.X + 2;
            unitPos.Y = position.Y - 40;
            unitRect = new Rectangle((int) unitPos.X, (int) unitPos.Y, 100, 138);
        }


        public void AddTerrain()
        {
            hasTerrain = true;
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
                spriteBatch.Draw(tileTexture, position, Color.Black);
            }
            if (hasUnit)
            {
                unit.Draw(spriteBatch, position);
            }

        }
    }
}
