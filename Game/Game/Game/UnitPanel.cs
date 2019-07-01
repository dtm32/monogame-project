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
    private Texture2D skillPassive;
    private Texture2D skillActive;
    private Vector2 position;
    private Unit unit;

    private bool leftPanel;
    private int unitSkillsLength = 5;

    public UnitPanel(Texture2D skillActive, Texture2D skillPassive, Texture2D dummyTexture, Vector2 windowSize)
    {
      this.skillPassive = skillPassive;
      this.skillActive = skillActive;
      this.texture = dummyTexture;

      //int unitSkillsLength = 5;
      int statPanelWidth = 200;

      position.X = windowSize.X / 2 - (statPanelWidth + unitSkillsLength * 20);
      position.Y = windowSize.Y - 100;

      panelRect = new Rectangle((int)position.X, (int)position.Y, 2 * (statPanelWidth + unitSkillsLength * 20), 100);
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
      //if(leftPanel)
      //{
      //Texture2D dummyTexture = new Texture2D(GraphicsDevice, 1, 1);
      //dummyTexture.SetData(new Color[] { Color.White });

      int panelLeft = panelRect.Left;
      int panelRight = panelRect.Right;
      int panelTop = panelRect.Top;
      int skillTop = panelTop + 20;


      spriteBatch.Draw(texture, panelRect, Color.White);

      // Draw stats
      for (int i = 0; i < 6; i++)
      {
        switch (i)
        {
          case 0:
          case 2:
          case 4:
            spriteBatch.DrawString(Game.Font.Cambira,
                WrapText(Game.Font.CambiraHalf, "HP: 29", 238),
                new Vector2(panelLeft + 40, position.Y + 28 + 12 * (i - 1)),
                Color.Black,
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
                WrapText(Game.Font.CambiraHalf, "Str: 30", 238),
                new Vector2(panelLeft + 110, position.Y + 28 + 12 * (i - 2)),
                Color.Black,
                0f,
                Vector2.Zero,
                0.5f,
                SpriteEffects.None,
                1f);
            break;
        }
      }

      // Draw skills


      for(int i = 0; i < unitSkillsLength; i++)
      {
        //panelRect = new Rectangle((int)position.X, (int)position.Y, 2 * (statPanelWidth + unitSkillsLength * 20), 100);

        Rectangle skillRectangle = new Rectangle(panelRight - (i * 80) - 80, skillTop, 60, 60);

        spriteBatch.Draw(skillActive, skillRectangle, Color.White);
      }
      //for (int i = 0; i < unit.GetSkillList().Length; i++)
      //{
      //  spriteBatch.DrawString(Game.Font.Cambira,
      //      WrapText(Game.Font.CambiraHalf, unit.GetSkillList()[i].GetText(), 238),
      //      new Vector2(10, position.Y + 115 + 75 * i),
      //      Color.White,
      //      0f,
      //      Vector2.Zero,
      //      0.5f,
      //      SpriteEffects.None,
      //      1f);
      //}



      //}
      //else
      //{
      //    spriteBatch.Draw(texture, panelRect, null, Color.White, 0f, Vector2.Zero, SpriteEffects.FlipHorizontally, 1f);

      //    // Draw stats
      //    for (int i = 0; i < unit.GetStats().ToArray().Length; i++)
      //    {
      //        switch (i)
      //        {
      //            case 0:
      //            case 2:
      //            case 4:
      //                spriteBatch.DrawString(Game.Font.Cambira,
      //                    WrapText(Game.Font.CambiraHalf, unit.GetStats().ToArray()[i], 238),
      //                    new Vector2(position.X + 15, position.Y + 28 + 12 * (i - 1)),
      //                    Color.White,
      //                    0f,
      //                    Vector2.Zero,
      //                    0.5f,
      //                    SpriteEffects.None,
      //                    1f);
      //                break;
      //            case 1:
      //            case 3:
      //            case 5:
      //                spriteBatch.DrawString(Game.Font.Cambira,
      //                    WrapText(Game.Font.CambiraHalf, unit.GetStats().ToArray()[i], 238),
      //                    new Vector2(position.X + 115, position.Y + 28 + 12 * (i - 2)),
      //                    Color.White,
      //                    0f,
      //                    Vector2.Zero,
      //                    0.5f,
      //                    SpriteEffects.None,
      //                    1f);
      //                break;
      //        }

      //    }

      //    // Draw skills
      //    for (int i = 0; i < unit.GetSkillList().Length; i++)
      //    {
      //        spriteBatch.DrawString(Game.Font.Cambira,
      //            WrapText(Game.Font.CambiraHalf, unit.GetSkillList()[i].GetText(), 238),
      //            new Vector2(position.X + 15, position.Y + 115 + 75 * i),
      //            Color.White,
      //            0f,
      //            Vector2.Zero,
      //            0.5f,
      //            SpriteEffects.None,
      //            1f);
      //    }
      //}
    }

    public override void Update(GameTime gameTime)
    {
      throw new NotImplementedException();
    }
  }
}
