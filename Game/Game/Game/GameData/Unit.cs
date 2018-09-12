using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static Game.GameData.Skill;

namespace Game.GameData
{
    abstract class Unit : Drawable
    {
        protected Texture2D   texture;
        protected Rectangle   rect;
        protected List<Skill> skillList;
        protected Stats       stats;
        protected int         level;
        protected bool        dispOverlayText;
        protected float       delta;
        protected string      overlayText;

        /// <summary>
        /// Initialize unit by saving texture.
        /// </summary>
        /// <param name="unitTexture"></param>
        public Unit(Texture2D unitTexture)
        {
            texture = unitTexture;

            skillList = new List<Skill>();
        }

        public Stats GetStats()
        {
            return stats;
        }

        public Skill[] GetSkillList()
        {
            return skillList.ToArray();
        }

        // Update to (unit target, Skill attack)
        public void StartCombat(Unit target, int range, float damageMult, DamageType damageType)
        {
            int damage = 0;

            float temp = 0f;

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

            Console.WriteLine("Strength = " + damage);

            temp = damage * damageMult;

            Console.WriteLine("Strength * Weapon = " + temp);

            damage = (int)(temp * (1 + (stats.GetSpd() - target.GetStats().GetSpd()) / 200));

            Console.WriteLine("Attack * Speed = " + damage);

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

            Console.WriteLine("Damage - defenses = " + damage);

            target.DealDamage(damage);
        }

        public bool DealDamage(int damage)
        {
            overlayText = (damage * -1).ToString();
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
