using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static Game.Units.Skill;

namespace Game.Units
{
    abstract class Unit : Drawable
    {
        protected Texture2D texture;
        //protected Vector2   position;
        protected Rectangle rect;
        protected Stats     stats;
        protected List<Skill> skillsList;
        protected int       level;
        protected bool      dispOverlayText;
        protected float     delta;
        protected string    overlayText;

        //public Unit(int lvl, int hp, int spd, int str, int fcs, int amr, int res)
        //{
        //    level = lvl;
        //    stats = new Stats(hp, spd, str, fcs, amr, res);
        //}

            /// <summary>
            /// Initialize unit by saving texture.
            /// </summary>
            /// <param name="unitTexture"></param>
        public Unit(Texture2D unitTexture)
        {
            texture = unitTexture;
        }

        // Update to (unit target, Skill attack)
        public void StartCombat(Unit target, int range, float damageMult, DamageType damageType)
        {
            int damage = 0;

            // Calculate total damage
            switch (damageType)
            {
                case DamageType.Physical:
                    damage = stats.GetStr();
                    break;
                case DamageType.Magical:
                    damage = stats.GetFcs();
                    break;
                default:
                    damage = 0;
                    break;
            }

            damage = (int) (damage * (1 + (stats.GetSpd() - target.GetStats().GetSpd()) / 200));

            // Calculate defense
            switch (damageType)
            {
                case DamageType.Physical:
                    damage -= target.GetStats().GetAmr();
                    break;
                case DamageType.Magical:
                    damage -= target.GetStats().GetRes();
                    break;
                default:
                    damage = 0;
                    break;
            }

            target.DealDamage(damage);
        }

        public Stats GetStats()
        {
            return stats;
        }

        public bool DealDamage(int damage)
        {
            overlayText = damage.ToString();
            dispOverlayText = true;

            return stats.DecreaseHealth(damage);
        }

        private int damageTextPos = 0;

        public override void Update(GameTime gameTime)
        {
            if (dispOverlayText)
            {
                delta += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                damageTextPos = (int) delta / 20;

                if (delta >= 800)
                {
                    delta = 0;
                    dispOverlayText = false;
                }
                //throw new NotImplementedException();
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            throw new NotImplementedException();
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 tilePos)
        {
            // All drawing is relative to parent tile

            rect = new Rectangle((int)tilePos.X, (int)tilePos.Y - 38, 100, 138);
            spriteBatch.Draw(texture, rect, Color.White);

            if(dispOverlayText)
            {
                Vector2 textPos;
                textPos.X = tilePos.X + 50;
                textPos.Y = tilePos.Y + 50 - damageTextPos;
                spriteBatch.DrawString(Game.DamageFont, overlayText, textPos, Color.Red);
            }
        }


    }
}
