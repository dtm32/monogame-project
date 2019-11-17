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

        Vector2 cursorPosition;
        Texture2D cursorTexture, cursorClickedTexture;
        Texture2D blankTexture;
        Texture2D puffFlyTexture, spikePigTexture, featherRaptorTexture, woodThumbTexture;
        Texture2D fireTexture;
        Texture2D skillTexture;
        Texture2D panelCornerTexture;
        Cursor cursor;

        Rectangle[] tileRects = new Rectangle[8];
        Vector2[] tilePos = new Vector2[8];

        GameState gameState;
        BattleManager battleManager;

        AnimatedSprite puffFlySprite;
        AnimatedSprite spikePigSprite;
        AnimatedSprite featherRaptorSprite;
        AnimatedSprite woodThumbSprite;
        AnimatedSprite fireSprite;

        Texture2D healthBarTexture;

        // sprite fonts
        private SpriteFont font;


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
            //cursorRect = new Rectangle(0, 0, 30, 30);
            cursorPosition.X = 0;
            cursorPosition.Y = 0;
            //cursorColor = Color.White;

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

            //C:\Users\Dylan\source\repos\2D Game\bin\Windows\x86\Debug\Content\Resources\cursor.png

            // cursor textures
            cursorTexture = LoadTexturePNG("cursor");
            cursorClickedTexture = LoadTexturePNG("cursor_clicked");
            cursor = new Cursor(cursorTexture, cursorClickedTexture, graphics.GraphicsDevice.Viewport.Width, graphics.GraphicsDevice.Viewport.Height);

            // setup unit sprites
            puffFlyTexture = LoadTexturePNG("anim_puff_fly");
            puffFlySprite = new AnimatedSprite(puffFlyTexture, 1, 2);

            spikePigTexture = LoadTexturePNG("anim_spike_pig");
            spikePigSprite = new AnimatedSprite(spikePigTexture, 1, 5);
            spikePigSprite.UpdateSpeed = 8;
            spikePigSprite.Idle = 200;

            featherRaptorTexture = LoadTexturePNG("anim_feather_raptor");
            featherRaptorSprite = new AnimatedSprite(featherRaptorTexture, 1, 2);
            featherRaptorSprite.UpdateSpeed = 80;

            woodThumbTexture = LoadTexturePNG("anim_wood_thumb");
            woodThumbSprite = new AnimatedSprite(woodThumbTexture, 1, 4);
            woodThumbSprite.UpdateSpeed = 5;
            woodThumbSprite.Idle = 200;

            // combat animation sprites
            fireTexture = LoadTexturePNG("anim_fire");
            fireSprite = new AnimatedSprite(fireTexture, 1, 7);
            fireSprite.Idle = 150;
            fireSprite.UpdateSpeed = 5;

            // additional textures
            blankTexture = LoadTexturePNG("square");
            healthBarTexture = LoadTexturePNG("health_bar");
            skillTexture = LoadTexturePNG("skill_texture");
            panelCornerTexture = LoadTexturePNG("panel_corner");

            // fonts
            font = Content.Load<SpriteFont>("SpriteFonts/Score");
        }

        private Texture2D LoadTexturePNG(string fileName)
        {
            FileStream fileStream = new FileStream("../../../../Content/Resources/" + fileName + ".png", FileMode.Open);
            Texture2D texture = Texture2D.FromStream(graphics.GraphicsDevice, fileStream);
            fileStream.Dispose();
            return texture;
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
                skills.Add(new Skill("Assault", 45));
                skills.Add(new Skill("Assault", 50));
                skills.Add(new Skill("Tap", 10));
                ArrayList skillSet2 = new ArrayList();
                skillSet2.Add(new Skill("Assault", 40));
                Skill falseSwipe = new Skill("False Swipe", 10, 
                    (self, target) =>
                    {
                        target.CurrHP = 1;
                    });
                skillSet2.Add(falseSwipe);
                skillSet2.Add(new Skill("Tap", 10));
                skillSet2.Add(new Skill("Gouge", 55,
                    (self, target) =>
                    {
                        target.Inflict(Unit.StatusEffect.Bleed);
                    }));


                ArrayList woodysSkills = new ArrayList();
                Skill highFive = new Skill("High Five", 5);
                highFive.Effect = (self, target) =>
                {
                    target.CurrHP -= 100;
                };
                Skill finger = new Skill("Finger", 150);
                finger.Effect = (self, target) =>
                {
                    target.Inflict(Unit.StatusEffect.Burn);
                };
                Skill thumb = new Skill("Middle Finger", 200);
                thumb.Effect = (self, target) =>
                {
                    self.CurrHP += 200;
                };
                Skill asdf = new Skill("DMG_OP_1.1", 801);
                woodysSkills.Add(highFive);
                woodysSkills.Add(finger);
                woodysSkills.Add(thumb);
                woodysSkills.Add(asdf);


                BaseUnit puffFly = new BaseUnit(puffFlySprite, "Puff Fly", "common", "common", skills, 100, 98, 75, 50, 55, 74);
                BaseUnit spikePig = new BaseUnit(new AnimatedSprite(spikePigSprite), "Spike Pig", "common", "common", skills, 150, 70, 100, 100, 250, 110);
                //BaseUnit spikePig = new BaseUnit(spikePigSprite, "Spike Pig", "common", "common", skills, 150, 70, 100, 100, 250, 110);
                BaseUnit featherRaptor = new BaseUnit(new AnimatedSprite(featherRaptorSprite), "Feather Raptor", "common", "common", skillSet2, 110, 105, 98, 50, 95, 60);
                BaseUnit WOODY_HAHA_XD = new BaseUnit(woodThumbSprite, "Woody", "Dragon", "Spirit", woodysSkills, 100, 999, 50, 50, 110, 80);

                for (int i = 0; i < 8; i++)
                {
                    if (i < 3)
                        battleManager.AddUnit(featherRaptor, i);
                    else if (i == 3)
                        battleManager.AddUnit(WOODY_HAHA_XD, i);
                    else if (i == 6)
                        battleManager.AddUnit(puffFly, i);
                    else
                        battleManager.AddUnit(spikePig, i);
                }

                battleManager.AddTextures(blankTexture, healthBarTexture, skillTexture, panelCornerTexture);
                battleManager.AddSprites(fireSprite);
                battleManager.AddFonts(font);

                gameState = GameState.Run;
                battleManager.Start();
            }


            cursor.Update(Mouse.GetState());

            battleManager.Update(cursor);


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
