using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Game
{
    class SkillContainer
    {
        private Vector2 position;
        private Rectangle containerRect;
        private Texture2D containerTexture;
        private String containerText;
        private SpriteFont font;

        public SkillContainer(String text, int x, int y)
        {
        }

        public void AddSpriteFont(SpriteFont spriteFont)
        {
            font = spriteFont;
        }

        
    }
}
