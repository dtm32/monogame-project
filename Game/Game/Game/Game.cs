using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Game.Units;
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
        public static SpriteFont font;
        public static SpriteFont fontTimesNewRoman;
        public static SpriteFont DamageFont;

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Texture2D knightTexture;
        Texture2D knightNoHelmetTexture;
        Texture2D backgroundTexture;
        Texture2D cursorTexture;
        Rectangle cursorRect;
        Rectangle backgroundRect;
        Vector2 cursorPosition;
        Color cursorColor;
        Vector2 fontPosition;
        Random rnd = new Random();
        protected int score;
        private Vector2 gridSize;

        protected Texture2D tileTexture;
        protected Rectangle[] tileRectangles = new Rectangle[25];
        private Tile[] tileArray;
        private bool skillsOpen = false;
        private Unit selectedUnit;

        private String feralSwipeText = 
            "Feral Swipe 4\n" +
            "Rng 1 CD 2\n" +
            "Deal damage = 80% Strength. Inflict (4) Bleed to target.";

        private bool mousePressed = false;

        private enum MouseState
        {
            NotClicked,
            Clicked
        }

        public Game()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = 1280;
            graphics.PreferredBackBufferHeight = 720;
            //graphics.PreferMultiSampling = true;
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
            cursorRect = new Rectangle(0, 0, 30, 30);
            cursorPosition.X = 0;
            cursorPosition.Y = 0;

            cursorColor = Color.White;

            backgroundRect = new Rectangle(0, 0,
                graphics.GraphicsDevice.Viewport.Width,
                graphics.GraphicsDevice.Viewport.Height);

            fontPosition = new Vector2(10.0f, 10.0f);

            score = 0;

            gridSize.X = 5;
            gridSize.Y = 5;

            tileArray = new Tile[(int) (gridSize.X * gridSize.Y)];

            Vector2 tileStart = new Vector2();
            tileStart.X = graphics.GraphicsDevice.Viewport.Width / 2 - (120 * gridSize.X / 2 - 20);
            tileStart.Y = graphics.GraphicsDevice.Viewport.Height / 2 - (120 * gridSize.Y / 2 - 20);

            int i = 0;

            for (int x = 0; x < gridSize.X; x++)
            {
                for(int y = 0; y < gridSize.Y; y++)
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
            cursorTexture = Content.Load<Texture2D>("cursor");

            font = Content.Load<SpriteFont>("SpriteFont1");
            fontTimesNewRoman = Content.Load<SpriteFont>("TimesNewRomanSmall");
            DamageFont = Content.Load<SpriteFont>("DamageFont");

            tileTexture = Content.Load<Texture2D>("skill-square");

            // assign textures to objects
            for (int i = 0; i < tileArray.Length; i++)
            {
                tileArray[i].SetTexture(tileTexture);
            }

            tileArray[9].AddUnit(new GreyKnight(knightTexture));
            tileArray[14].AddUnit(new GreyKnight(knightNoHelmetTexture));
            tileArray[10].AddTerrain();
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

            // update cursor on click
            if (Mouse.GetState().LeftButton == ButtonState.Pressed)
            {
                cursorColor = Color.Red;
            }
            else
            {
                cursorColor = Color.White;
            }

            // Cursor
            // TODO: Make cursor an object
            cursorPosition.X = Mouse.GetState().X;
            cursorPosition.Y = Mouse.GetState().Y;

            cursorPosition.X = MathHelper.Clamp(cursorPosition.X, 0, viewportWidth - cursorRect.Width);
            cursorPosition.Y = MathHelper.Clamp(cursorPosition.Y, 0, viewportHeight - cursorRect.Height);

            cursorRect.X = (int)cursorPosition.X;
            cursorRect.Y = (int)cursorPosition.Y;

            // check if user clicked unit
            for (int i = 0; i < tileArray.Length; i++)
            {
                //TODO: Register clicks on tile for anything?
                if (cursorRect.Intersects(tileArray[i].GetUnitRectangle()) && Mouse.GetState().LeftButton == ButtonState.Pressed)
                {
                    cursorColor = Color.LightGreen;
                    tileArray[i].GetUnit().DealDamage(1);
                    // Click on Unit to select
                    if ( !(tileArray[i].GetUnit().Equals(selectedUnit)) )
                    {
                        selectedUnit = tileArray[i].GetUnit();
                        Console.WriteLine("Selected unit ");
                        tileArray[i].AddTerrain();
                    }
                    else
                    {
                        selectedUnit = null;
                        Console.WriteLine("Unselected!");
                        tileArray[i].RemoveTerrain();
                    }
                }
                else if(Mouse.GetState().LeftButton != ButtonState.Pressed)
                {
                    MouseState =
                }

                tileArray[i].Update(gameTime);
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

            //for (int i = 0; i < tileArray.Length; i++)
            //{
                //Tile tile = tileArray[i];
                //spriteBatch.Draw(tile.GetTexture(), tile.GetRectangle(), Color.White);
                //if(tile.HasUnit())
                //{
                //    spriteBatch.Draw(tile.GetUnitTexture(), tile.GetUnitRectangle(), Color.White);
                //}

            //}

            foreach(Tile tile in tileArray)
            {
                tile.Draw(spriteBatch);
            }

            if (skillsOpen)
            {
                //spriteBatch.Draw()
                spriteBatch.DrawString(fontTimesNewRoman, WrapText(font, feralSwipeText, 300), new Vector2(10, 10), Color.Black);
            }

            spriteBatch.Draw(cursorTexture, cursorRect, cursorColor);
            //spriteBatch.DrawString(font, "Score: " + score, fontPosition, Color.White);

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

                Console.WriteLine(word + ": size.X = " + size.X + " curr size = " + lineWidth);

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
    }
}