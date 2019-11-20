using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2D_Game.Content
{
    class FontManager
    {
        public static SpriteFont Default_Regular_9;
        public static SpriteFont Default_Regular_11;
        public static SpriteFont Default_Bold_11;
        public static SpriteFont Default_Bold_15;

        public FontManager(ContentManager Content)
        {
            Default_Regular_9 = Content.Load<SpriteFont>("SpriteFonts/CenturySchoolbookRegular_9");
            Default_Regular_11 = Content.Load<SpriteFont>("SpriteFonts/CenturySchoolbookRegular_11");
            Default_Bold_11 = Content.Load<SpriteFont>("SpriteFonts/CenturySchoolbookBold_11");
            Default_Bold_15 = Content.Load<SpriteFont>("SpriteFonts/CenturySchoolbookBold_15");
        }

        public static string WrapText(SpriteFont spriteFont, string text, float maxLineWidth)
        {
            string[] words = text.Split(' ');
            StringBuilder sb = new StringBuilder();
            float lineWidth = 0f;
            float spaceWidth = spriteFont.MeasureString(" ").X;

            foreach(string word in words)
            {
                Vector2 size = spriteFont.MeasureString(word);

                if(word.Contains("\n"))
                {
                    lineWidth = size.X + spaceWidth;
                    sb.Append(word + " ");
                }
                else if(lineWidth + size.X < maxLineWidth)
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
    }
}
