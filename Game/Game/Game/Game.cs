using System;
using System.Collections.Generic;
using System.Linq;
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
        Texture2D crosshairTexture;
        Texture2D backgroundTexture;
        Rectangle donutRect;
        Rectangle crosshairRect;
        Rectangle backgroundRect;
        Vector2 donutPosition;
        Vector2 donutVelocity;
        Vector2 crosshairPosition;
        Color crosshairColor;
        SpriteFont font;
        Vector2 fontPosition;
        Random rnd = new Random();
        protected int score;

        protected Texture2D tileTexture;
        protected Rectangle[] tileRectangles = new Rectangle[25];
        private Tile[] tileArray = new Tile[25];

        public Game()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = 1280;
            graphics.PreferredBackBufferHeight = 720;
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

            crosshairColor = Color.White;

            backgroundRect = new Rectangle(0, 0,
                graphics.GraphicsDevice.Viewport.Width,
                graphics.GraphicsDevice.Viewport.Height);

            fontPosition = new Vector2(10.0f, 10.0f);

            score = 0;

            Vector2 tileStart = new Vector2();
            tileStart.X = graphics.GraphicsDevice.Viewport.Width / 2 - 250;
            tileStart.Y = graphics.GraphicsDevice.Viewport.Height / 2 - 250;

            int i = 0;

            for (int x = 0; x < 5; x++)
            {
                for(int y = 0; y < 5; y++)
                {
                    tileRectangles[i] = new Rectangle((int)tileStart.X + 100 * x, (int)tileStart.Y + 100 * y, 100, 100);
                    tileArray[i] = new Tile((int)tileStart.X + 100 * x, (int)tileStart.Y + 100 * y);
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
            backgroundTexture = Content.Load<Texture2D>("fantasy-background");
            crosshairTexture = Content.Load<Texture2D>("crosshair");
            font = Content.Load<SpriteFont>("SpriteFont1");

            tileTexture = Content.Load<Texture2D>("skill-square");

            // assign textures to objects
            for (int i = 0; i < 25; i++)
            {
                tileArray[i].SetTexture(tileTexture);
            }

            tileArray[7].AddUnit(knightTexture);
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
                crosshairColor = Color.Red;
            }
            else
            {
                crosshairColor = Color.White;
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
            crosshairPosition.X = Mouse.GetState().X;
            crosshairPosition.Y = Mouse.GetState().Y;

            crosshairPosition.X = MathHelper.Clamp(crosshairPosition.X, 0, viewportWidth - crosshairRect.Width);
            crosshairPosition.Y = MathHelper.Clamp(crosshairPosition.Y, 0, viewportHeight - crosshairRect.Height);

            crosshairRect.X = (int)crosshairPosition.X;
            crosshairRect.Y = (int)crosshairPosition.Y;

            // check if user clicked clown
            if (crosshairRect.Intersects(donutRect) && crosshairColor.Equals(Color.Red))
            {
                // clown gets shot
                donutPosition.X = rnd.Next(viewportWidth - donutRect.Width);
                donutPosition.Y = rnd.Next(viewportHeight - donutRect.Height);
                donutVelocity.X *= 1.1f;
                donutVelocity.Y *= 1.1f;
                score++;
            }

            if(crosshairRect.Intersects(tileArray[7].GetUnitRectangle()))
            {
                crosshairColor = Color.Blue;
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

            for (int i = 0; i < 25; i++)
            {
                Tile tile = tileArray[i];
                spriteBatch.Draw(tile.GetTexture(), tile.GetRectangle(), Color.White);
                if(tile.HasUnit())
                {
                    spriteBatch.Draw(tile.GetUnitTexture(), tile.GetUnitRectangle(), Color.White);
                }
            }

            spriteBatch.Draw(knightTexture, donutRect, Color.White);
            spriteBatch.Draw(crosshairTexture, crosshairRect, crosshairColor);
            spriteBatch.DrawString(font, "Score: " + score, fontPosition, Color.White);

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}