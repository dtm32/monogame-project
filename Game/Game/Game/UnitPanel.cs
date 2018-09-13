using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Game.GameData;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Game
{
    class UnitPanel : Drawable
    {
        private Rectangle panelRect;
        private Texture2D texture;
        private Vector2 position;
        private Unit unit;

        private bool leftPanel;

        public UnitPanel(Texture2D texture, Vector2 windowSize, bool leftPanel)
        {
            this.texture = texture;
            this.leftPanel = leftPanel;

            if(leftPanel)
            {
                position.X = 0;
                position.Y = windowSize.Y / 2 - 500 / 2;
                panelRect = new Rectangle((int)position.X, (int)position.Y, 238, 500);
            }
            else
            {
                position.X = windowSize.X - 238;
                position.Y = windowSize.Y / 2 - 500 / 2;
                panelRect = new Rectangle((int)position.X, (int)position.Y, 238, 500);
            }
        }

        public void SetUnit(Unit unit)
        {
            this.unit = unit;
        }

        public string WrapText(SpriteFont spriteFont, string text, float maxLineWidth)
        {
            string[] words = text.Split(' ');
            StringBuilder sb = new StringBuilder();
            float lineWidth = 0f;
            float spaceWidth = spriteFont.MeasureString(" ").X;

            foreach (string word in words)
            {
                Vector2 size = spriteFont.MeasureString(word);

                if (word.Contains("\n"))
                {
                    lineWidth = size.X + spaceWidth;
                    sb.Append(word + " ");
                }
                else if (lineWidth + size.X < maxLineWidth)
                {
                    sb.Append(word + " ");
                    lineWidth += size.X + spaceWidth;
                }
                else
                {
                    sb.Append("\n" + word + " ");
                    lineWidth = size.X + spaceWidth;
                }
            }

            return sb.ToString();
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if(leftPanel)
            {
                spriteBatch.Draw(texture, panelRect, Color.White);

                // Draw stats
                for (int i = 0; i < unit.GetStats().ToArray().Length; i++)
                {
                    switch (i)
                    {
                        case 0:
                        case 2:
                        case 4:
                            spriteBatch.DrawString(Game.Font.Cambira,
                                WrapText(Game.Font.CambiraHalf, unit.GetStats().ToArray()[i], 238),
                                new Vector2(10, position.Y + 28 + 12 * (i - 1)),
                                Color.White,
                                0f,
                                Vector2.Zero,
                                0.5f,
                                SpriteEffects.None,
                                1f);
                            break;
                        case 1:
                        case 3:
                        case 5:
                            spriteBatch.DrawString(Game.Font.Cambira,
                                WrapText(Game.Font.CambiraHalf, unit.GetStats().ToArray()[i], 238),
                                new Vector2(110, position.Y + 28 + 12 * (i - 2)),
                                Color.White,
                                0f,
                                Vector2.Zero,
                                0.5f,
                                SpriteEffects.None,
                                1f);
                            break;
                    }
                    
                }

                // Draw skills
                for (int i = 0; i < unit.GetSkillList().Length; i++)
                {
                    spriteBatch.DrawString(Game.Font.Cambira, 
                        WrapText(Game.Font.CambiraHalf, unit.GetSkillList()[i].GetText(), 238), 
                        new Vector2(10, position.Y + 115 + 75 * i), 
                        Color.White,
                        0f,
                        Vector2.Zero,
                        0.5f,
                        SpriteEffects.None,
                        1f);
                }
            }
            else
            {
                spriteBatch.Draw(texture, panelRect, null, Color.White, 0f, Vector2.Zero, SpriteEffects.FlipHorizontally, 1f);

                // Draw stats
                for (int i = 0; i < unit.GetStats().ToArray().Length; i++)
                {
                    switch (i)
                    {
                        case 0:
                        case 2:
                        case 4:
                            spriteBatch.DrawString(Game.Font.Cambira,
                                WrapText(Game.Font.CambiraHalf, unit.GetStats().ToArray()[i], 238),
                                new Vector2(position.X + 15, position.Y + 28 + 12 * (i - 1)),
                                Color.White,
                                0f,
                                Vector2.Zero,
                                0.5f,
                                SpriteEffects.None,
                                1f);
                            break;
                        case 1:
                        case 3:
                        case 5:
                            spriteBatch.DrawString(Game.Font.Cambira,
                                WrapText(Game.Font.CambiraHalf, unit.GetStats().ToArray()[i], 238),
                                new Vector2(position.X + 115, position.Y + 28 + 12 * (i - 2)),
                                Color.White,
                                0f,
                                Vector2.Zero,
                                0.5f,
                                SpriteEffects.None,
                                1f);
                            break;
                    }

                }

                // Draw skills
                for (int i = 0; i < unit.GetSkillList().Length; i++)
                {
                    spriteBatch.DrawString(Game.Font.Cambira,
                        WrapText(Game.Font.CambiraHalf, unit.GetSkillList()[i].GetText(), 238),
                        new Vector2(position.X + 15, position.Y + 115 + 75 * i),
                        Color.White,
                        0f,
                        Vector2.Zero,
                        0.5f,
                        SpriteEffects.None,
                        1f);
                }
            }
        }

        public override void Update(GameTime gameTime)
        {
            throw new NotImplementedException();
        }
    }
}
