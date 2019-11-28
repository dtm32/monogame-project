using _2D_Game.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections;
using System.Collections.Generic;
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
            InitMainMenu,
            RunMainMenu,
            InitBattleManager,
            RunBattleManager,
            ContinueBattleManager
        }

        public static Random random = new Random();
        public static int SpriteWidth = 96;
        public static int SpriteHeight = 128;

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        // cursor variables
        Cursor cursor;
        //Vector2 cursorPosition;

        Texture2D cursorTexture, cursorClickedTexture;
        Texture2D blankTexture;
        Texture2D skillTexture;
        Texture2D arrowButtonTexture;
        Texture2D panelCornerTexture;
        Texture2D poisonIconTexture, burnIconTexture, bleedIconTexture, stunIconTexture;
        Texture2D physicalIconTexture, magicalIconTexture, statusIconTexture, buffIconTexture;
        IconManager iconManager;

        Rectangle[] tileRects = new Rectangle[8];
        Vector2[] tilePos = new Vector2[8];

        GameState gameState;
        BattleManager battleManager;
        MainMenu mainMenu;

        List<AnimatedSprite> spriteList;
        List<BaseUnit> baseUnitList;
        List<BaseUnit> enemyUnitList;
        Texture2D puffFlyTexture, spikePigTexture, featherRaptorTexture, woodThumbTexture, pinkScytheTexture,
            redMothTexture, longHairTexture, psychicHandsTexture, snowSpiritTexture, zigZagTexture,
            beanSproutTexture, eyeLizardTexture, zombieFishTexture, ancientFishTexture, cursedTomeTexture;
        AnimatedSprite puffFlySprite;
        AnimatedSprite spikePigSprite;
        AnimatedSprite featherRaptorSprite;
        AnimatedSprite woodThumbSprite;
        AnimatedSprite pinkScytheSprite;
        AnimatedSprite redMothSprite;
        AnimatedSprite longHairSprite;
        AnimatedSprite psychicHandsSprite;
        AnimatedSprite snowSpiritSprite;
        AnimatedSprite zigZagSprite;
        AnimatedSprite beanSproutSprite;
        AnimatedSprite eyeLizardSprite;
        AnimatedSprite zombieFishSprite;
        AnimatedSprite ancientFishSprite;
        AnimatedSprite cursedTomeSprite;

        Texture2D burnTexture, poisonedTexture, bleedTexture, stunTexture;
        AnimatedSprite burnSprite, poisonedSprite, bleedSprite, stunSprite;

        // health bar textures
        Texture2D healthBarTexture;
        Texture2D healthBarHighlightedTexture;
        Texture2D animHealthBarTexture;
        AnimatedSprite animHealthBarSprite;
        Texture2D animHealthBarEnemyTexture;
        AnimatedSprite animHealthBarEnemySprite;

        Texture2D grassBackgroundTexture;

        // sprite fonts
        FontManager fontManager;
        public static SpriteFont font;
        public static SpriteFont FontSmallBold;

        int[] team = new int[4];
        int gauntletTier = 0;

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

            gameState = GameState.InitMainMenu;
            battleManager = new BattleManager(graphics);
            mainMenu = new MainMenu(graphics);

            // TODO: Add your initialization logic here
            //cursorRect = new Rectangle(0, 0, 30, 30);
            //cursorPosition.X = 0;
            //cursorPosition.Y = 0;
            //cursorColor = Color.White;

            Rectangle backgroundRect = new Rectangle(0, 0,
                graphics.GraphicsDevice.Viewport.Width,
                graphics.GraphicsDevice.Viewport.Height);
            Color backgroundColor = Color.Blue;
            spriteList = new List<AnimatedSprite>();
            baseUnitList = new List<BaseUnit>();
            enemyUnitList = new List<BaseUnit>();

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
            //this.Content = Content;

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

            longHairTexture = LoadTexturePNG("anim_long_hair");
            longHairSprite = new AnimatedSprite(longHairTexture, 1, 6);
            longHairSprite.UpdateSpeed = 5;
            longHairSprite.Idle = 200;

            psychicHandsTexture = LoadTexturePNG("anim_psychic_hands");
            psychicHandsSprite = new AnimatedSprite(psychicHandsTexture, 1, 4);
            psychicHandsSprite.UpdateSpeed = 10;

            snowSpiritTexture = LoadTexturePNG("anim_snow_spirit");
            snowSpiritSprite = new AnimatedSprite(snowSpiritTexture, 1, 2);

            zigZagTexture = Content.Load<Texture2D>("Resources/anim_zig_zag");
            zigZagSprite = new AnimatedSprite(zigZagTexture, 1, 6);
            psychicHandsSprite.UpdateSpeed = 6;

            beanSproutTexture = Content.Load<Texture2D>("Resources/anim_bean_sprout");
            beanSproutSprite = new AnimatedSprite(beanSproutTexture, 1, 4);
            beanSproutSprite.UpdateSpeed = 8;

            eyeLizardTexture = Content.Load<Texture2D>("Resources/anim_eye_lizard");
            eyeLizardSprite = new AnimatedSprite(eyeLizardTexture, 4, 4);
            eyeLizardSprite.UpdateSpeed = 13;
            eyeLizardSprite.Idle = 200;

            zombieFishTexture = Content.Load<Texture2D>("Resources/anim_zombie_fish");
            zombieFishSprite = new AnimatedSprite(zombieFishTexture, 2, 3);
            zombieFishSprite.UpdateSpeed = 25;

            ancientFishTexture = Content.Load<Texture2D>("Resources/anim_ancient_fish");
            ancientFishSprite = new AnimatedSprite(ancientFishTexture, 1, 1);

            cursedTomeTexture = Content.Load<Texture2D>("Resources/anim_cursed_tome");
            cursedTomeSprite = new AnimatedSprite(cursedTomeTexture, 2, 4);
            cursedTomeSprite.UpdateSpeed = 13;

            spriteList.Add(puffFlySprite);
            spriteList.Add(spikePigSprite);
            spriteList.Add(featherRaptorSprite);
            spriteList.Add(woodThumbSprite);
            spriteList.Add(pinkScytheSprite);
            spriteList.Add(redMothSprite);
            spriteList.Add(longHairSprite);
            spriteList.Add(psychicHandsSprite);
            spriteList.Add(snowSpiritSprite);
            spriteList.Add(beanSproutSprite);
            spriteList.Add(eyeLizardSprite);
            spriteList.Add(zombieFishSprite);

            // combat animation sprites
            burnTexture = LoadTexturePNG("anim_fire");
            burnSprite = new AnimatedSprite(burnTexture, 1, 7);
            burnSprite.Idle = 150;
            burnSprite.UpdateSpeed = 5;

            poisonedTexture = LoadTexturePNG("anim_poisoned");
            poisonedSprite = new AnimatedSprite(poisonedTexture, 1, 9);
            poisonedSprite.Idle = 150;
            poisonedSprite.UpdateSpeed = 4;

            bleedTexture = LoadTexturePNG("anim_bleed");
            bleedSprite = new AnimatedSprite(bleedTexture, 2, 5);
            bleedSprite.Idle = 150;
            bleedSprite.UpdateSpeed = 3;

            stunTexture = LoadTexturePNG("anim_stun");
            stunSprite = new AnimatedSprite(stunTexture, 4, 5);
            stunSprite.Idle = 150;
            stunSprite.UpdateSpeed = 1;

            // icon textures
            bleedIconTexture = LoadTexturePNG("icon_bleed");
            burnIconTexture = LoadTexturePNG("icon_burned");
            poisonIconTexture = LoadTexturePNG("icon_poisoned");
            stunIconTexture = LoadTexturePNG("icon_stunned");
            physicalIconTexture = LoadTexturePNG("icon_physical");
            magicalIconTexture = LoadTexturePNG("icon_magical");
            statusIconTexture = LoadTexturePNG("icon_status");
            buffIconTexture = LoadTexturePNG("icon_buff");
            iconManager = new IconManager(poisonIconTexture, burnIconTexture, bleedIconTexture,
                stunIconTexture, physicalIconTexture, magicalIconTexture, statusIconTexture, 
                buffIconTexture);

            // health bar textures
            healthBarHighlightedTexture = LoadTexturePNG("health_bar_highlighted");
            animHealthBarTexture = LoadTexturePNG("anim_health_bar");
            animHealthBarSprite = new AnimatedSprite(animHealthBarTexture, 4, 1);
            animHealthBarSprite.UpdateSpeed = 5;
            animHealthBarSprite.Idle = 40;
            animHealthBarEnemyTexture = LoadTexturePNG("anim_health_bar_enemy");
            animHealthBarEnemySprite = new AnimatedSprite(animHealthBarEnemyTexture, 4, 1);
            animHealthBarEnemySprite.UpdateSpeed = 5;
            animHealthBarEnemySprite.Idle = 40;

            // additional textures
            blankTexture = LoadTexturePNG("square");
            healthBarTexture = LoadTexturePNG("health_bar");
            skillTexture = LoadTexturePNG("skill_texture");
            arrowButtonTexture = LoadTexturePNG("next_button");
            panelCornerTexture = LoadTexturePNG("panel_corner");
            grassBackgroundTexture = LoadTexturePNG("battle_background");

            // fonts
            //font = Content.Load<SpriteFont>("SpriteFonts/Score");
            //FontSmallBold = Content.Load<SpriteFont>("SpriteFonts/SmallBold");
            fontManager = new FontManager(Content);
        }

        //private ContentManager Content;

        private Texture2D LoadTexturePNG(string fileName)
        {
            //FileStream fileStream = new FileStream("../../../../Content/Resources/" + fileName + ".png", FileMode.Open);
            //Texture2D texture = Texture2D.FromStream(graphics.GraphicsDevice, fileStream);
            //fileStream.Dispose();
            //return texture;
            return Content.Load<Texture2D>($"Resources/{fileName}");
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

            // update cursor
            cursor.Update(Mouse.GetState());

            if(gameState == GameState.InitMainMenu)
            {
                mainMenu.AddTextures(blankTexture, skillTexture, arrowButtonTexture, grassBackgroundTexture);
                mainMenu.AddUnitSprites(spriteList);
                gameState = GameState.RunMainMenu;

                // puff fly
                ArrayList skillSet1 = new ArrayList();
                skillSet1.Add(Skill.Swarm);
                skillSet1.Add(Skill.Assault);
                skillSet1.Add(Skill.BugBite);
                skillSet1.Add(Skill.BuzzBy);

                // feather raptor
                ArrayList skillSet2 = new ArrayList();
                skillSet2.Add(Skill.Assault);
                skillSet2.Add(Skill.Charge);
                skillSet2.Add(Skill.FastSwipe);
                skillSet2.Add(Skill.Gouge);

                // Woody skill set
                ArrayList woodysSkills = new ArrayList();
                woodysSkills.Add(new Skill("High Five", 60));
                woodysSkills.Add(new Skill("Hot Hands", 75, Skill.SkillType.Physical,
                    (self, target) =>
                    {
                        target.Inflict(new StatusEffect(StatusEffect.Burn, 2, self, target));
                    }, "Inflicts Burn(2)."));
                woodysSkills.Add(new Skill("The Bird", 60, Skill.SkillType.Physical,
                    (self, target) =>
                    {
                        self.CurrHP += (int)((self.HP - self.CurrHP) * 0.20);
                    }, "Heal unit for 20% of missing health."));
                woodysSkills.Add(new Skill("Roulette", Skill.SkillType.Physical,
                    (self, target, units) =>
                    {
                        int count = 0;
                        while(count < 3)
                        {
                            int index = Game1.random.Next(units.Length);
                            if(units[index].IsAlive)
                            {
                                units[index].CurrHP -= (int)(units[index].HP * 0.34);
                                count++;
                            }
                        }
                    }, "Randomly damages 3 units."));

                // Pink Scythe skill set
                ArrayList skillSet3 = new ArrayList();
                skillSet3.Add(new Skill("Reap", Skill.SkillType.Effect,
                    (self, target) =>
                    {
                        target.CurrHP = (int)(target.CurrHP * 0.75);
                    }, "Deals damage = 25% target's current health."));
                skillSet3.Add(Skill.Drain);
                skillSet3.Add(Skill.Smooch);
                skillSet3.Add(Skill.BigMagicChurro);

                // Red Moth skill set
                ArrayList skillSet4 = new ArrayList();
                skillSet4.Add(Skill.SonicBuzz);
                skillSet4.Add(Skill.CorrodingBite);
                skillSet4.Add(Skill.Incantation);
                skillSet4.Add(Skill.ToxicCloud);

                ArrayList skillSet5 = new ArrayList(); // long hair
                skillSet5.Add(Skill.VorpalStrike);
                skillSet5.Add(Skill.Backlash);
                skillSet5.Add(Skill.Nullify);
                skillSet5.Add(Skill.PiercingGaze);

                // psychic hands
                ArrayList skillSet6 = new ArrayList();
                skillSet6.Add(Skill.Retaliate);
                skillSet6.Add(Skill.Torment);
                skillSet6.Add(Skill.Ignite);
                skillSet6.Add(Skill.PsychicShackle);

                // snow spirit
                ArrayList skillSet7 = new ArrayList();
                skillSet7.Add(Skill.FlashFreeze);
                skillSet7.Add(new Skill("Arcane Blast", 70, Skill.SkillType.Magical));
                skillSet7.Add(Skill.Assault);
                skillSet7.Add(Skill.FallingIce);

                // spike pig
                ArrayList skillSet8 = new ArrayList();
                skillSet8.Add(Skill.Assault);
                skillSet8.Add(Skill.HeatUp);
                skillSet8.Add(Skill.Frenzy);
                skillSet8.Add(Skill.AdaptiveBlow);

                // zig zag
                ArrayList skillSet9 = new ArrayList();
                skillSet9.Add(Skill.RecklessCharge);
                skillSet9.Add(Skill.TailWag);
                skillSet9.Add(Skill.Bite);
                skillSet9.Add(Skill.FastSwipe);

                // bean sprout
                ArrayList skillSet10 = new ArrayList();
                skillSet10.Add(new Skill("Sprout", 0, Skill.SkillType.Buff,
                    (self, target) =>
                    {
                        self.CurrHP += (int)(self.Fcs * 0.2);
                        self.BuffStat(Unit.Speed, 1);
                        self.BuffStat(Unit.Strength, 1);
                        self.BuffStat(Unit.Focus, 1);
                    }, "Heal self = 20% Focus and increase Spd/Str/Fcs by 1."));
                skillSet10.Add(Skill.PoisonSpike);
                skillSet10.Add(Skill.SideWhip);
                skillSet10.Add(Skill.Ingrain);

                // eye lizard
                ArrayList skillSet11 = new ArrayList();
                skillSet11.Add(Skill.TongueLash);
                skillSet11.Add(Skill.AdaptiveBlow);
                skillSet11.Add(Skill.Impede);
                skillSet11.Add(Skill.PiercingGaze);

                // zombie fish
                ArrayList skillSet12 = new ArrayList();
                skillSet12.Add(Skill.SoulSiphon);
                skillSet12.Add(Skill.Nullify);
                skillSet12.Add(Skill.Sacrifice);
                skillSet12.Add(Skill.Reprisal);

                // ancient fish
                ArrayList skillSet13 = new ArrayList();
                skillSet13.Add(Skill.RockSmash);
                skillSet13.Add(Skill.Maelstrom);
                skillSet13.Add(Skill.Earthquake);
                skillSet13.Add(Skill.Reprisal);

                // cursed tome
                ArrayList skillSet14 = new ArrayList();
                skillSet14.Add(Skill.Reprisal);
                skillSet14.Add(Skill.PhantasmalRend);
                skillSet14.Add(Skill.Infuse);
                skillSet14.Add(Skill.Torment);


                BaseUnit puffFly = new BaseUnit(puffFlySprite, "Puff Fly", "Beast", "Common", skillSet1, 
                    100, 110, 105, 95, 95, 95);
                BaseUnit spikePig = new BaseUnit(spikePigSprite, "Spike Pig", "Beast", "Common", skillSet8, 
                    164, 70, 125, 80, 135, 95);
                BaseUnit pinkScythe = new BaseUnit(pinkScytheSprite, "Pink Scythe", "Mage", "Spirit", skillSet3, 
                    97, 104, 109, 133, 100, 185);
                BaseUnit featherRaptor = new BaseUnit(featherRaptorSprite, "Feather Raptor", "Beast", "Wild", skillSet2, 
                    110, 130, 118, 70, 115, 60);
                BaseUnit WOODY_HAHA_XD = new BaseUnit(woodThumbSprite, "Woody", "Dragon", "Spirit", woodysSkills, 
                    80, 157, 100, 90, 110, 70);
                BaseUnit redMoth = new BaseUnit(redMothSprite, "Red Moth", "Beast", "Wild", skillSet4,
                    168, 64, 69, 103, 168, 169);
                BaseUnit longHair = new BaseUnit(longHairSprite, "Long Hair", "Infantry", "Dark", skillSet5,
                    105, 80, 145, 45, 60, 110);
                BaseUnit psychicHands = new BaseUnit(psychicHandsSprite, "Psychic Hands", "Mage", "Dark", skillSet6,
                    90, 95, 80, 165, 85, 80);
                BaseUnit snowSpirit = new BaseUnit(snowSpiritSprite, "Snow Spirt", "Mage", "Mythic", skillSet7,
                    85, 90, 125, 125, 95, 90);
                BaseUnit zigZag = new BaseUnit(zigZagSprite, "Zig-Zag", "Beast", "Common", skillSet9,
                    76, 120, 60, 60, 82, 122);
                BaseUnit beanSprout = new BaseUnit(beanSproutSprite, "Whamoosh", "Mage", "Wild", skillSet10,
                    80, 110, 105, 115, 70, 95);
                BaseUnit eyeLizard = new BaseUnit(eyeLizardSprite, "Eye Lizard", "Beast", "Wild", skillSet11,
                    85, 65, 120, 85, 95, 145);
                BaseUnit zombieFish = new BaseUnit(zombieFishSprite, "Zombie Fish", "Beast", "Spirit", skillSet12,
                    135, 84, 120, 60, 79, 125);
                BaseUnit ancientFish = new BaseUnit(ancientFishSprite, "Depth Dweller", "Beast", "Mythic", skillSet13,
                    150, 60, 145, 85, 210, 80);
                BaseUnit cursedTome = new BaseUnit(cursedTomeSprite, "Cursed Tome", "Mage", "Spirit", skillSet14,
                    120, 98, 75, 162, 80, 90);

                baseUnitList.Add(puffFly);       // 0
                baseUnitList.Add(spikePig);      // 1
                baseUnitList.Add(featherRaptor); // 2
                baseUnitList.Add(WOODY_HAHA_XD); // 3
                baseUnitList.Add(pinkScythe);    // 4
                baseUnitList.Add(redMoth);       // 5
                baseUnitList.Add(longHair);      // 6
                baseUnitList.Add(psychicHands);  // 7
                baseUnitList.Add(snowSpirit);    // 8
                baseUnitList.Add(beanSprout);    // 9
                baseUnitList.Add(eyeLizard);     // 10
                baseUnitList.Add(zombieFish);    // 11
                baseUnitList.Add(ancientFish);   // 12
                baseUnitList.Add(cursedTome);    // 13

                enemyUnitList.Add(zigZag);
                enemyUnitList.Add(featherRaptor);
                enemyUnitList.Add(zigZag);
                enemyUnitList.Add(featherRaptor);

                enemyUnitList.Add(featherRaptor.SetLevel(79));
                enemyUnitList.Add(longHair);
                enemyUnitList.Add(snowSpirit);
                enemyUnitList.Add(zombieFish);
                
                enemyUnitList.Add(snowSpirit);
                enemyUnitList.Add(spikePig);
                enemyUnitList.Add(snowSpirit);
                enemyUnitList.Add(spikePig);
                
                enemyUnitList.Add(cursedTome);
                enemyUnitList.Add(eyeLizard);
                enemyUnitList.Add(zombieFish);
                enemyUnitList.Add(cursedTome);

                mainMenu.AddBaseUnits(baseUnitList);
                mainMenu.AddIcons(iconManager);
            }
            else if(gameState == GameState.RunMainMenu)
            {
                mainMenu.Update(cursor);

                if(mainMenu.StartBattle)
                {
                    gameState = GameState.InitBattleManager;
                    team = mainMenu.SelectedUnits;

                }
            }
            else if(gameState == GameState.InitBattleManager)
            {

                //battleManager.AddUnit(baseUnitList[0], 0);
                //battleManager.AddUnit(baseUnitList[2], 1);
                //battleManager.AddUnit(baseUnitList[5], 2);
                //battleManager.AddUnit(baseUnitList[4], 3);
                battleManager.AddUnit(baseUnitList[team[0]].SetLevel(75), 0);
                battleManager.AddUnit(baseUnitList[team[1]].SetLevel(75), 1);
                battleManager.AddUnit(baseUnitList[team[2]].SetLevel(75), 2);
                battleManager.AddUnit(baseUnitList[team[3]].SetLevel(75), 3);

                battleManager.AddUnit(enemyUnitList[0].SetLevel(77), 4);
                battleManager.AddUnit(baseUnitList[2].SetLevel(75), 5);
                battleManager.AddUnit(enemyUnitList[0].SetLevel(77), 6);
                battleManager.AddUnit(baseUnitList[0].SetLevel(72), 7);

                battleManager.AddTextures(blankTexture, healthBarTexture, healthBarHighlightedTexture, 
                    skillTexture, panelCornerTexture, grassBackgroundTexture);
                battleManager.AddSprites(burnSprite, poisonedSprite, bleedSprite, stunSprite,
                    animHealthBarSprite, animHealthBarEnemySprite);
                battleManager.AddFonts(font);
                battleManager.AddIcons(iconManager);

                gameState = GameState.RunBattleManager;
                battleManager.Start();
            }
            else if(gameState == GameState.RunBattleManager)
            {
                battleManager.Update(cursor);

                if(battleManager.ExitBattleManager)
                {
                    //gameState = GameState.RunMainMenu;
                    gameState = GameState.ContinueBattleManager;
                }
            }
            else if(gameState == GameState.ContinueBattleManager)
            {
                
                //battleManager = new 
                battleManager.ClearUnits();
                battleManager.AddUnit(baseUnitList[team[0]].SetLevel(75), 0);
                battleManager.AddUnit(baseUnitList[team[1]].SetLevel(75), 1);
                battleManager.AddUnit(baseUnitList[team[2]].SetLevel(75), 2);
                battleManager.AddUnit(baseUnitList[team[3]].SetLevel(75), 3);

                int index = gauntletTier * 4;

                battleManager.AddUnit(enemyUnitList[index], 4);
                battleManager.AddUnit(enemyUnitList[index + 1], 5);
                battleManager.AddUnit(enemyUnitList[index + 2], 6);
                battleManager.AddUnit(enemyUnitList[index + 3], 7);

                gauntletTier++;

                gameState = GameState.RunBattleManager;
                battleManager.Start();
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

            switch(gameState)
            {
                case GameState.RunMainMenu:
                    mainMenu.Draw(spriteBatch);
                    break;
                case GameState.InitBattleManager:
                    break;
                case GameState.RunBattleManager:
                    if(battleManager != default(BattleManager)) // if initialized, draw; also check state
                        battleManager.Draw(spriteBatch);
                    break;
            }


            cursor.Draw(spriteBatch);

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
