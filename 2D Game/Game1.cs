using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.IO;

namespace _2D_Game
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Color cursorColor;
        Rectangle cursorRect;
        Vector2 cursorPosition;
        Texture2D cursorTexture;
        Texture2D blankTexture;

        Rectangle[] tileRects = new Rectangle[8];
        Vector2[] tilePos = new Vector2[8];

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
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
            graphics.PreferredBackBufferWidth = 1280;  // set this value to the desired width of your window
            graphics.PreferredBackBufferHeight = 720;   // set this value to the desired height of your window
            graphics.ApplyChanges();

            // TODO: Add your initialization logic here
            cursorRect = new Rectangle(0, 0, 30, 30);
            cursorPosition.X = 0;
            cursorPosition.Y = 0;
            cursorColor = Color.White;

            Rectangle backgroundRect = new Rectangle(0, 0,
                graphics.GraphicsDevice.Viewport.Width,
                graphics.GraphicsDevice.Viewport.Height);
            Color backgroundColor = Color.Blue;

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
            //cursorTexture = Content.Load<Texture2D>("Resources/cursor.png");
            //C: \Users\Dylan\source\repos\2D Game\bin\Windows\x86\Debug\Content\Resources\cursor.png
            FileStream fileStream = new FileStream("../../../../Content/Resources/cursor.png", FileMode.Open);
            cursorTexture = Texture2D.FromStream(graphics.GraphicsDevice, fileStream);
            fileStream = new FileStream("../../../../Content/Resources/square.png", FileMode.Open);
            blankTexture = Texture2D.FromStream(graphics.GraphicsDevice, fileStream);
            fileStream.Dispose();
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
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
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            int viewportWidth = graphics.GraphicsDevice.Viewport.Width;
            int viewportHeight = graphics.GraphicsDevice.Viewport.Height;

            // Update mouse events
            //if (Mouse.GetState().LeftButton == ButtonState.Pressed)
            //{
            //    if (!leftMousePressedLast)
            //    {
            //        leftMouseState = MouseState.MouseDown;
            //    }
            //    else
            //    {
            //        leftMouseState = MouseState.None;
            //    }

            //    leftMousePressedLast = true;
            //}
            //else
            //{
            //    if (leftMousePressedLast)
            //    {
            //        leftMouseState = MouseState.MouseUp;
            //    }
            //    else
            //    {
            //        leftMouseState = MouseState.None;
            //    }

            //    leftMousePressedLast = false;
            //}

            //if (Mouse.GetState().RightButton == ButtonState.Pressed)
            //{
            //    if (!rightMousePressedLast)
            //    {
            //        rightMouseState = MouseState.MouseDown;
            //    }
            //    else
            //    {
            //        rightMouseState = MouseState.None;
            //    }

            //    rightMousePressedLast = true;
            //}
            //else
            //{
            //    if (rightMousePressedLast)
            //    {
            //        rightMouseState = MouseState.MouseUp;
            //    }
            //    else
            //    {
            //        rightMouseState = MouseState.None;
            //    }

            //    rightMousePressedLast = false;
            //}

            // update cursor on click
            //if (Mouse.GetState().LeftButton == ButtonState.Pressed)
            //{
            //    cursorColor = Color.LightSlateGray;
            //}
            //else if (selectedTile != null)
            //{
            //    // Signal unit can attack
            //    cursorColor = Color.Red;
            //}
            //else
            //{
            //    cursorColor = Color.White;
            //}

            // Cursor
            // TODO: Make cursor an object
            cursorPosition.X = Mouse.GetState().X;
            cursorPosition.Y = Mouse.GetState().Y;

            if(cursorPosition.X >= 0 && cursorPosition.X <= viewportWidth &&
                cursorPosition.Y >= 0 && cursorPosition.Y <= viewportHeight)
            {
                cursorPosition.X = MathHelper.Clamp(cursorPosition.X, 0, viewportWidth - cursorRect.Width);
                cursorPosition.Y = MathHelper.Clamp(cursorPosition.Y, 0, viewportHeight - cursorRect.Height);

                cursorRect.X = (int)cursorPosition.X;
                cursorRect.Y = (int)cursorPosition.Y;
            }

            int SPRITE_WIDTH = 96;
            int SPRITE_HEIGHT = 128;
            int TOP_OFFSET = 80;

            for(int i = 0; i < 8; i++)
            {
                int posX = viewportWidth / 2 - 300;
                int posY = (int) ((i % 4) * (SPRITE_HEIGHT * 0.9) + TOP_OFFSET);

                if(i > 3)
                {
                    posX = viewportWidth / 2 + 300 - SPRITE_WIDTH;
                }

                tileRects[i] = new Rectangle(posX, posY, SPRITE_WIDTH, SPRITE_HEIGHT);
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
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
            Texture2D rect = new Texture2D(graphics.GraphicsDevice, 80, 30);

            for(int i = 0; i < 8; i++)
            {
                Color temp = Color.White;
                if (i % 2 == 0)
                    temp = Color.Gray;
                spriteBatch.Draw(blankTexture, tileRects[i], temp);
            }
            //spriteBatch.Draw()

            spriteBatch.Draw(cursorTexture, cursorRect, Color.White);

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
