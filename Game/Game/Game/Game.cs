using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Game
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Texture2D knightTexture;
        Texture2D knightNoHelmetTexture;
        Texture2D crosshairTexture;
        Texture2D backgroundTexture;
        Texture2D cursorTexture;
        Rectangle donutRect;
        Rectangle crosshairRect;
        Rectangle cursorRect;
        Rectangle backgroundRect;
        Vector2 donutPosition;
        Vector2 donutVelocity;
        Vector2 crosshairPosition;
        Vector2 cursorPosition;
        Color cursorColor;
        SpriteFont font;
        Vector2 fontPosition;
        Random rnd = new Random();
        protected int score;

        protected Texture2D tileTexture;
        protected Rectangle[] tileRectangles = new Rectangle[25];
        private Tile[] tileArray = new Tile[49];
        private bool skillsOpen = false;

        public Game()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = 1920;
            graphics.PreferredBackBufferHeight = 1048;
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            donutRect = new Rectangle(200, 200, 80, 70);
            donutPosition.X = 200.0f;
            donutPosition.Y = 200.0f;
            donutVelocity.X = 1.0f;
            donutVelocity.Y = 1.0f;

            crosshairRect = new Rectangle(50, 50, 30, 30);
            crosshairPosition.X = 400;
            crosshairPosition.Y = 400;

            cursorRect = new Rectangle(0, 0, 30, 30);
            cursorPosition.X = 0;
            cursorPosition.Y = 0;

            cursorColor = Color.White;

            backgroundRect = new Rectangle(0, 0,
                graphics.GraphicsDevice.Viewport.Width,
                graphics.GraphicsDevice.Viewport.Height);

            fontPosition = new Vector2(10.0f, 10.0f);

            score = 0;

            Vector2 tileStart = new Vector2();
            tileStart.X = graphics.GraphicsDevice.Viewport.Width / 2 - 420;
            tileStart.Y = graphics.GraphicsDevice.Viewport.Height / 2 - 450;

            int i = 0;

            for (int x = 0; x < 7; x++)
            {
                for(int y = 0; y < 7; y++)
                {
                    tileArray[i] = new Tile((int)tileStart.X + 120 * x, (int)tileStart.Y + 120 * y);
                    i++;
                }
            }

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            knightTexture = Content.Load<Texture2D>("knight-small");
            knightNoHelmetTexture = Content.Load<Texture2D>("knight-no-helmet");
            backgroundTexture = Content.Load<Texture2D>("fantasy-background");
            crosshairTexture = Content.Load<Texture2D>("crosshair");
            cursorTexture = Content.Load<Texture2D>("cursor");

            font = Content.Load<SpriteFont>("SpriteFont1");

            tileTexture = Content.Load<Texture2D>("skill-square");

            // assign textures to objects
            for (int i = 0; i < tileArray.Length; i++)
            {
                tileArray[i].SetTexture(tileTexture);
            }

            tileArray[9].AddUnit(knightTexture);
            tileArray[14].AddUnit(knightNoHelmetTexture);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            // TODO: Add your update logic here

            int viewportWidth = graphics.GraphicsDevice.Viewport.Width;
            int viewportHeight = graphics.GraphicsDevice.Viewport.Height;

            // update crosshair
            if (Mouse.GetState().LeftButton == ButtonState.Pressed)
            {
                cursorColor = Color.Red;
            }
            else
            {
                cursorColor = Color.White;
            }

            // moving donut
            if (donutPosition.X + donutRect.Width > viewportWidth || donutPosition.X < 0)
            {
                donutVelocity.X *= -1;
            }

            if (donutPosition.Y + donutRect.Height > viewportHeight || donutPosition.Y < 0)
            {
                donutVelocity.Y *= -1;
            }

            donutPosition += donutVelocity;
            donutRect.X = (int)donutPosition.X;
            donutRect.Y = (int)donutPosition.Y;

            // crosshair
            cursorPosition.X = Mouse.GetState().X;
            cursorPosition.Y = Mouse.GetState().Y;

            cursorPosition.X = MathHelper.Clamp(cursorPosition.X, 0, viewportWidth - cursorRect.Width);
            cursorPosition.Y = MathHelper.Clamp(cursorPosition.Y, 0, viewportHeight - cursorRect.Height);

            cursorRect.X = (int)cursorPosition.X;
            cursorRect.Y = (int)cursorPosition.Y;

            // check if user clicked unit
            for (int i = 0; i < tileArray.Length; i++)
            {
                if (cursorRect.Intersects(tileArray[i].GetUnitRectangle()) && Mouse.GetState().LeftButton == ButtonState.Pressed)
                {
                    cursorColor = Color.LightGreen;
                    skillsOpen = true;
                }
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            spriteBatch.Begin();
            spriteBatch.Draw(backgroundTexture, backgroundRect, Color.White);

            for (int i = 0; i < tileArray.Length; i++)
            {
                Tile tile = tileArray[i];
                spriteBatch.Draw(tile.GetTexture(), tile.GetRectangle(), Color.White);
                if(tile.HasUnit())
                {
                    spriteBatch.Draw(tile.GetUnitTexture(), tile.GetUnitRectangle(), Color.White);
                }
            }

            if (skillsOpen)
            {
                spriteBatch.Draw()
            }

            spriteBatch.Draw(cursorTexture, cursorRect, cursorColor);
            spriteBatch.DrawString(font, "Score: " + score, fontPosition, Color.White);

            spriteBatch.End();

            base.Draw(gameTime);
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

                if (lineWidth + size.X < maxLineWidth)
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