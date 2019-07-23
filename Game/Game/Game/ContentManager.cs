using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Game
{
    public class ContentManager
    {
        // public Vector2 Position;
        // public Texture2D Texutre;

        Texture2D knightTexture;
        Texture2D knightNoHelmetTexture;
        Texture2D crimsonKnightTexture;
        Texture2D darkKnightTexture;

        // GraphicsDeviceManager graphics;
        // SpriteBatch spriteBatch;

        Texture2D cursorTexture;
        Texture2D backgroundTexture;
        Texture2D tileTexture;
        Texture2D unitPanelTexture;
        Texture2D unitPanelTexture1;
        Texture2D skillActive, skillPassive, unitPanelSolid;                          

        public void LoadAll(GameTime gameTime)
        {
          // GAME
          cursorTexture = Content.Load<Texture2D>("cursor");
          backgroundTexture = Content.Load<Texture2D>("fantasy-background");
          tileTexture = Content.Load<Texture2D>("skill-square");
          unitPanelTexture = Content.Load<Texture2D>("unit-panel-background");
          unitPanelTexture1 = Content.Load<Texture2D>("path843");
          skillActive = Content.Load<Texture2D>("skill-active");
          skillPassive = Content.Load<Texture2D>("skill-passive");
          unitPanelSolid = Content.Load<Texture2D>("unit-panel-solid");

          // UNITS
          knightTexture = Content.Load<Texture2D>("knight-small");
          knightNoHelmetTexture = Content.Load<Texture2D>("knight-no-helmet");
          crimsonKnightTexture = Content.Load<Texture2D>("crimson-knight-0");
          darkKnightTexture = Content.Load<Texture2D>("dark-knight-0");
        }

        public void Draw(SpriteBatch spriteBatch);
    }
}
