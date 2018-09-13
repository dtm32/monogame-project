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
        protected int         damageTextPos = 0;


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
            float temp = 0f;
            float speedMult = 1 + (stats.GetSpd() - target.GetStats().GetSpd()) / 200f;
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

            temp = damage * damageMult;

            damage = (int)(temp * speedMult);

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

            if(target.GetStats().IsAlive())
            {
                target.Retaliate(this);
            }
        }

        public void Retaliate(Unit target)
        {
            // TODO: update use weapon 
            // TODO: check range

            float temp = 0f;
            float speedMult = 1 + (stats.GetSpd() - target.GetStats().GetSpd()) / 200f;
            int damage = 0;

            // TODO: use weapon damageType
            DamageType damageType = DamageType.Physical;

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

            // TODO: change 1 to weapon damage mult
            temp = damage * 1;

            // Factor in speed difference
            damage = (int)(temp * speedMult);

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

        public bool DealDamage(int damage)
        {
            if (stats.GetCurrentHealth() > damage)
            {
                overlayText = (damage * -1).ToString();
                dispOverlayText = true;
            }
            else if (stats.GetCurrentHealth() > 0)
            {
                damage = stats.GetCurrentHealth();
                overlayText = (damage * -1).ToString();
                dispOverlayText = true;
            }

            return stats.DecreaseHealth(damage);
        }

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

        public virtual void Draw(SpriteBatch spriteBatch, Vector2 tilePos)
        {
            // All drawing is relative to parent tile

            rect = new Rectangle((int)tilePos.X, (int)tilePos.Y + (100 - texture.Height), texture.Width, texture.Height);

            if(stats.IsAlive())
            {
                spriteBatch.Draw(texture, rect, Color.White);
            }
            else
            {
                spriteBatch.Draw(texture, rect, Color.Red * 0.5f);
            }

            if(dispOverlayText)
            {
                Vector2 textPos;
                textPos.X = tilePos.X + 50;
                textPos.Y = tilePos.Y + 50 - damageTextPos;
                spriteBatch.DrawString(Game.Font.DamageFont, overlayText, textPos, Color.Red);
            }
        }
    }
}
