using _2D_Game.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections;
using System.IO;

namespace _2D_Game
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        enum GameState
        {
            Run,
            InitBattleManager
        }

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Color cursorColor;
        Rectangle cursorRect;
        Vector2 cursorPosition;
        Texture2D cursorTexture, cursorClickedTexture;
        Texture2D blankTexture;
        Texture2D puffFlyTexture, spikePigTexture;
        Texture2D animTest;
        Cursor cursor;

        Rectangle[] tileRects = new Rectangle[8];
        Vector2[] tilePos = new Vector2[8];

        GameState gameState;
        BattleManager battleManager;

        AnimatedSprite animSprite;
        AnimatedSprite spikePigSprite;


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

            gameState = GameState.InitBattleManager;
            battleManager = new BattleManager(graphics);

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
            cursorClickedTexture = LoadTexturePNG("cursor_clicked");
            fileStream = new FileStream("../../../../Content/Resources/square.png", FileMode.Open);
            blankTexture = Texture2D.FromStream(graphics.GraphicsDevice, fileStream);
            fileStream = new FileStream("../../../../Content/Resources/puff_fly.png", FileMode.Open);
            puffFlyTexture = Texture2D.FromStream(graphics.GraphicsDevice, fileStream);
            fileStream = new FileStream("../../../../Content/Resources/spike_pig.png", FileMode.Open);
            spikePigTexture = Texture2D.FromStream(graphics.GraphicsDevice, fileStream);

            animTest = LoadTexturePNG("anim_puff_fly");
            animSprite = new AnimatedSprite(animTest, 1, 2);
            spikePigSprite = new AnimatedSprite(spikePigTexture, 1, 1);

            cursor = new Cursor(cursorTexture, cursorClickedTexture, graphics.GraphicsDevice.Viewport.Width, graphics.GraphicsDevice.Viewport.Height);


            fileStream.Dispose();
        }

        private Texture2D LoadTexturePNG(string fileName)
        {
            FileStream fileStream = new FileStream("../../../../Content/Resources/" + fileName + ".png", FileMode.Open);
            return Texture2D.FromStream(graphics.GraphicsDevice, fileStream);
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

            if (gameState == GameState.InitBattleManager)
            {
                ArrayList skills = new ArrayList();
                skills.Add(new Skill("Assault", 40));
                Unit defaultUnit1 = new Unit(animSprite, "Puff Fly", "common", "common", skills, 100, 100, 100, 100, 100, 100);
                Unit defaultUnit2 = new Unit(spikePigSprite, "Spike Pig", "common", "common", skills, 100, 100, 100, 100, 100, 100);

                for (int i = 0; i < 8; i++)
                {
                    if (i % 2 == 0)
                        battleManager.AddUnit(defaultUnit1, i);
                    else
                        battleManager.AddUnit(defaultUnit2, i);
                }

                gameState = GameState.Run;
            }

            battleManager.Update();

            cursor.Update(Mouse.GetState());

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


            if (battleManager != default(BattleManager)) // if initialized, draw; also check state
                battleManager.Draw(spriteBatch);

            cursor.Draw(spriteBatch);

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
