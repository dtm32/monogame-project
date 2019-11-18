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
        Texture2D puffFlyTexture, spikePigTexture, featherRaptorTexture, woodThumbTexture, pinkScytheTexture;
        Texture2D redMothTexture;
        Texture2D fireTexture;
        Texture2D poisonedTexture;
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
        AnimatedSprite pinkScytheSprite;
        AnimatedSprite redMothSprite;

        AnimatedSprite fireSprite, poisonedSprite;

        Texture2D healthBarTexture;

        // sprite fonts
        public static SpriteFont font;
        public static SpriteFont FontSmallBold;


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

            pinkScytheTexture = LoadTexturePNG("anim_pink_scythe");
            pinkScytheSprite = new AnimatedSprite(pinkScytheTexture, 1, 4);

            redMothTexture = LoadTexturePNG("anim_red_moth");
            redMothSprite = new AnimatedSprite(redMothTexture, 1, 4);
            redMothSprite.UpdateSpeed = 5;

            // combat animation sprites
            fireTexture = LoadTexturePNG("anim_fire");
            fireSprite = new AnimatedSprite(fireTexture, 1, 7);
            fireSprite.Idle = 150;
            fireSprite.UpdateSpeed = 5;

            poisonedTexture = LoadTexturePNG("anim_poisoned");
            poisonedSprite = new AnimatedSprite(poisonedTexture, 1, 9);
            poisonedSprite.Idle = 150;
            poisonedSprite.UpdateSpeed = 4;

            // additional textures
            blankTexture = LoadTexturePNG("square");
            healthBarTexture = LoadTexturePNG("health_bar");
            skillTexture = LoadTexturePNG("skill_texture");
            panelCornerTexture = LoadTexturePNG("panel_corner");

            // fonts
            font = Content.Load<SpriteFont>("SpriteFonts/Score");
            FontSmallBold = Content.Load<SpriteFont>("SpriteFonts/SmallBold");
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
                ArrayList skillSet1 = new ArrayList();
                skillSet1.Add(new Skill("Assault", 40));
                skillSet1.Add(new Skill("Assault", 45));
                skillSet1.Add(new Skill("Assault", 50));
                skillSet1.Add(new Skill("Tap", 10));
                ArrayList skillSet2 = new ArrayList();
                skillSet2.Add(new Skill("Assault", 40));
                Skill falseSwipe = new Skill("Fast Swipe", 30, 
                    (self, target) =>
                    {
                        self.BuffStat(Unit.Speed, 1);
                    });
                skillSet2.Add(falseSwipe);
                skillSet2.Add(new Skill("Tap", 10));
                skillSet2.Add(new Skill("Gouge", 55,
                    (self, target) =>
                    {
                        target.Inflict(new StatusEffect(StatusEffect.Poison, 2, self, target));
                    }));


                ArrayList woodysSkills = new ArrayList();
                Skill highFive = new Skill("High Five", 50);
                Skill finger = new Skill("Hot Hands", 80);
                finger.Effect = (self, target) =>
                {
                    target.Inflict(new StatusEffect(StatusEffect.Burn, 2, self, target));
                };
                Skill thumb = new Skill("The Bird", 65);
                thumb.Effect = (self, target) =>
                {
                    self.CurrHP += self.CurrHP;
                };
                Skill dmgOP1_1 = new Skill("DMG_OP_1.1", 801);
                woodysSkills.Add(highFive);
                woodysSkills.Add(finger);
                woodysSkills.Add(thumb);
                woodysSkills.Add(dmgOP1_1);

                ArrayList skillSet3 = new ArrayList();
                skillSet3.Add(new Skill("Reap",
                    (self, target) =>
                    {
                        target.CurrHP = (int)(target.CurrHP * 0.75);
                    }));
                skillSet3.Add(new Skill("Drain", 60, Skill.SkillType.Magical,
                    (self, target) =>
                    {
                        self.CurrHP += target.LastDamageTaken / 2;
                    }));
                skillSet3.Add(new Skill("Smooch", 100, Skill.SkillType.Magical,
                    (self, target) =>
                    {
                        target.DebuffStat(Unit.Resistance, 2);
                    }));
                skillSet3.Add(new Skill("Big Magic Churro", 
                    (self, target) =>
                    {
                        int heal = (int)(self.Fcs * 0.45);
                        target.CurrHP += heal;
                        self.CurrHP += heal;
                        target.BuffStat(Unit.Speed, 1);
                    }));

                ArrayList skillSet4 = new ArrayList();
                skillSet4.Add(new Skill("Swarm", 65));
                skillSet4.Add(new Skill("Corroding Bite", 40,
                    (self, target) =>
                    {
                        target.DebuffStat(Unit.Armor, 2);
                        target.DebuffStat(Unit.Resistance, 2);
                    }));
                skillSet4.Add(new Skill("Incantation", Skill.SkillType.Buff,
                    (self, target) =>
                    {
                        target.CurrHP += (int)(target.HP * 0.15);
                        target.BuffStat(Unit.Strength, 1);
                        target.BuffStat(Unit.Focus, 1);
                        target.BuffStat(Unit.Armor, 1);
                        target.BuffStat(Unit.Resistance, 1);
                    }));
                skillSet4.Add(new Skill("Toxic Cloud", Skill.SkillType.Magical,
                    (self, units) =>
                    {
                        for(int i = units.Length/2; i < units.Length; i++)
                        {
                            units[i].CurrHP -= (int)(self.Fcs * 0.50); // fix this xd
                        }
                    }));

                BaseUnit puffFly = new BaseUnit(puffFlySprite, "Puff Fly", "common", "common", skillSet1, 
                    100, 100, 100, 100, 100, 100);
                BaseUnit spikePig = new BaseUnit(new AnimatedSprite(spikePigSprite), "Spike Pig", "common", "common", skillSet1, 
                    150, 70, 100, 100, 175, 110);
                BaseUnit pinkScythe = new BaseUnit(pinkScytheSprite, "Pink Scythe", "Mage", "Spirit", skillSet3, 
                    97, 104, 109, 133, 100, 185);
                BaseUnit featherRaptor = new BaseUnit(new AnimatedSprite(featherRaptorSprite), "Feather Raptor", "common", "common", skillSet2, 
                    110, 105, 98, 70, 95, 60);
                BaseUnit WOODY_HAHA_XD = new BaseUnit(woodThumbSprite, "Woody", "Dragon", "Spirit", woodysSkills, 
                    100, 187, 85, 75, 110, 80);
                BaseUnit redMoth = new BaseUnit(redMothSprite, "Red Moth", "Beast", "Wild", skillSet4,
                    168, 151, 69, 103, 168, 169);

                battleManager.AddUnit(featherRaptor, 0);
                battleManager.AddUnit(pinkScythe, 1);
                battleManager.AddUnit(redMoth, 2);
                battleManager.AddUnit(WOODY_HAHA_XD, 3);

                battleManager.AddUnit(puffFly, 4);
                battleManager.AddUnit(spikePig, 5);
                battleManager.AddUnit(pinkScythe, 6);
                battleManager.AddUnit(spikePig, 7);

                battleManager.AddTextures(blankTexture, healthBarTexture, skillTexture, panelCornerTexture);
                battleManager.AddSprites(fireSprite, poisonedSprite);
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
