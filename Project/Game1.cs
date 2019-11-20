﻿using _2D_Game.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
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
            InitMainMenu,
            RunMainMenu,
            InitBattleManager,
            RunBattleManager
        }

        public static Random random = new Random();

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        // cursor variables
        Cursor cursor;
        //Vector2 cursorPosition;

        Texture2D cursorTexture, cursorClickedTexture;
        Texture2D blankTexture;
        Texture2D puffFlyTexture, spikePigTexture, featherRaptorTexture, woodThumbTexture, pinkScytheTexture;
        Texture2D redMothTexture;
        Texture2D burnTexture;
        Texture2D poisonedTexture;
        Texture2D bleedTexture;
        Texture2D skillTexture;
        Texture2D panelCornerTexture;
        Texture2D poisonIconTexture, burnIconTexture, bleedIconTexture;
        Texture2D physicalIconTexture, magicalIconTexture, statusIconTexture, buffIconTexture;
        IconManager iconManager;

        Rectangle[] tileRects = new Rectangle[8];
        Vector2[] tilePos = new Vector2[8];

        GameState gameState;
        BattleManager battleManager;
        MainMenu mainMenu;

        AnimatedSprite puffFlySprite;
        AnimatedSprite spikePigSprite;
        AnimatedSprite featherRaptorSprite;
        AnimatedSprite woodThumbSprite;
        AnimatedSprite pinkScytheSprite;
        AnimatedSprite redMothSprite;

        AnimatedSprite burnSprite, poisonedSprite, bleedSprite;

        // health bar textures
        Texture2D healthBarTexture;
        Texture2D healthBarHighlightedTexture;
        Texture2D animHealthBarTexture;
        AnimatedSprite animHealthBarSprite;
        Texture2D animHealthBarEnemyTexture;
        AnimatedSprite animHealthBarEnemySprite;

        // sprite fonts
        FontManager fontManager;
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

            // icon textures
            bleedIconTexture = LoadTexturePNG("icon_bleed");
            burnIconTexture = LoadTexturePNG("icon_burned");
            poisonIconTexture = LoadTexturePNG("icon_poisoned");
            physicalIconTexture = LoadTexturePNG("icon_physical");
            magicalIconTexture = LoadTexturePNG("icon_magical");
            statusIconTexture = LoadTexturePNG("icon_status");
            buffIconTexture = LoadTexturePNG("icon_buff");
            iconManager = new IconManager(poisonIconTexture, burnIconTexture, bleedIconTexture,
                physicalIconTexture, magicalIconTexture, statusIconTexture, buffIconTexture);

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
            panelCornerTexture = LoadTexturePNG("panel_corner");

            // fonts
            //font = Content.Load<SpriteFont>("SpriteFonts/Score");
            //FontSmallBold = Content.Load<SpriteFont>("SpriteFonts/SmallBold");
            fontManager = new FontManager(Content);
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

            // update cursor
            cursor.Update(Mouse.GetState());

            if(gameState == GameState.InitMainMenu)
            {
                mainMenu.AddTextures(blankTexture, skillTexture);
                gameState = GameState.RunMainMenu;
            }
            else if(gameState == GameState.RunMainMenu)
            {
                mainMenu.Update(cursor);

                if(mainMenu.StartBattle)
                {
                    gameState = GameState.InitBattleManager;
                }
            }
            else if(gameState == GameState.InitBattleManager)
            {
                ArrayList skillSet1 = new ArrayList();
                skillSet1.Add(new Skill("Swarm", 0, Skill.SkillType.Physical,
                    (self, target, units) =>
                    {
                        //Random random = new Random();
                        int numAttacks = random.Next(4) + 5;
                        int power = 20;

                        float crit = 1.0f;

                        if(random.Next(100) < 5)
                        {
                            crit = 1.5f; // Also check for skill crit modifier
                        }

                        for(int i = 0; i < numAttacks; i++)
                        {
                            int targetOffset = 0;
                            if(!self.IsEnemy)
                                targetOffset += units.Length / 2;
                            int targetIndex = random.Next(units.Length / 2) + targetOffset;
                            int damage = (int)((power * self.Str / units[targetIndex].Amr * self.Level / 100) *
                                    (random.Next(15) / 100 + 1.35) * crit);
                            units[targetIndex].CurrHP -= damage;
                            Console.WriteLine($"{self.Name} damaged {units[targetIndex].Name} with Swarm for {damage} damage!");
                        }

                    }, "Randomly damages enemies 5-8 times.")
                    .SetTargetType(Skill.TargetAll));
                skillSet1.Add(new Skill("Assault", 45));
                skillSet1.Add(new Skill("Assault", 50));
                skillSet1.Add(new Skill("Buzzby", 10));
                ArrayList skillSet2 = new ArrayList();
                skillSet2.Add(new Skill("Assault", 40));
                skillSet2.Add(new Skill("Charge", 45, Skill.SkillType.Physical,
                    (self, target) =>
                    {
                        target.Inflict(new StatusEffect(StatusEffect.Bleed, 2, self, target));
                    }, "Inflicts Bleed(2)."));
                skillSet2.Add(new Skill("Fast Swipe", 30, Skill.SkillType.Physical,
                    (self, target) =>
                    {
                        self.BuffStat(Unit.Speed, 1);
                    }, "Increase self Speed(1)."));
                skillSet2.Add(new Skill("Gouge", 55, Skill.SkillType.Physical,
                    (self, target) =>
                    {
                        target.Inflict(new StatusEffect(StatusEffect.Poison, 2, self, target));
                    }, "Inflict Poison(2)."));

                // Woody skill set
                ArrayList woodysSkills = new ArrayList();
                woodysSkills.Add(new Skill("High Five", 50));
                woodysSkills.Add(new Skill("Hot Hands", 80, Skill.SkillType.Physical,
                    (self, target) =>
                    {
                        target.Inflict(new StatusEffect(StatusEffect.Burn, 2, self, target));
                    }, "Inflicts Burn(2)."));
                woodysSkills.Add(new Skill("The Bird", 65, Skill.SkillType.Physical,
                    (self, target) =>
                    {
                        self.CurrHP += (int)((self.HP - self.CurrHP) * 0.20);
                    }, "Heal unit for 20% of missing health."));
                woodysSkills.Add(new Skill("Psychic Shackle", 85, Skill.SkillType.Magical,
                    (self, target) =>
                    {
                        target.DebuffStat(Unit.Speed, 2); // TODO: UPDATE MOVE ORDER QUEUE TO REFLECT SPEED CHANGES
                    }, "Decrease Spd(2)."));

                // Pink Scythe skill set
                ArrayList skillSet3 = new ArrayList();
                skillSet3.Add(new Skill("Reap", Skill.SkillType.Effect,
                    (self, target) =>
                    {
                        target.CurrHP = (int)(target.CurrHP * 0.75);
                    }, "Deals damage = 25% target's current health."));
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
                skillSet3.Add(new Skill("Big Magic Churro", Skill.SkillType.Buff,
                    (self, target) =>
                    {
                        int heal = (int)(self.Fcs * 0.45);
                        target.CurrHP += heal;
                        self.CurrHP += heal;
                        target.BuffStat(Unit.Speed, 1);
                    }, "Heal self and target = 45% Focus and increase target Speed(1).")
                    .SetTargetType(Skill.TargetAlly));

                // Red Moth skill set
                ArrayList skillSet4 = new ArrayList();
                skillSet4.Add(new Skill("Sonic Buzz", 65, Skill.SkillType.Magical));
                skillSet4.Add(new Skill("Corroding Bite", 40, Skill.SkillType.Physical,
                    (self, target) =>
                    {
                        target.DebuffStat(Unit.Armor, 2);
                        target.DebuffStat(Unit.Resistance, 2);
                    }, "Decrease target Amr/Res(2)."));
                skillSet4.Add(new Skill("Incantation", Skill.SkillType.Buff,
                    (self, target) =>
                    {
                        target.CurrHP += (int)(target.HP * 0.15);
                        target.BuffStat(Unit.Strength, 1);
                        target.BuffStat(Unit.Focus, 1);
                        target.BuffStat(Unit.Armor, 1);
                        target.BuffStat(Unit.Resistance, 1);
                    }, "Heal target ally (15% unit's Health) and increase Str/Fcs/Amr/Res(1).")
                    .SetTargetType(Skill.TargetAlly));
                skillSet4.Add(new Skill("Toxic Cloud", 15, Skill.SkillType.Magical,
                    (self, target, units) =>
                    {
                        for(int i = units.Length/2; i < units.Length; i++)
                        {
                            units[i].Inflict(new StatusEffect(StatusEffect.Poison, 1, self, units[i]));


                            // TODO: MOVE DAMAGE LOGIC SOMEWHERE IT CAN BE ACCESSED
                            float crit = 1.0f;

                            if(random.Next(100) < 5)
                            {
                                crit = 1.5f; // Also check for skill crit modifier
                            }

                            int damage = (int)((15 * self.Fcs / units[i].Res * self.Level / 100) * 
                                (random.Next(15) / 100 + 1.35) * crit);

                            if(damage < 1)
                                damage = 1;

                            units[i].CurrHP -= damage;
                        }
                    }, "Damage all enemies and inflicts Poison(1).")
                    .SetTargetType(Skill.TargetAll));

                BaseUnit puffFly = new BaseUnit(puffFlySprite, "Puff Fly", "common", "common", skillSet1, 
                    100, 110, 105, 95, 95, 95);
                BaseUnit spikePig = new BaseUnit(new AnimatedSprite(spikePigSprite), "Spike Pig", "common", "common", skillSet1, 
                    164, 70, 125, 80, 175, 95);
                BaseUnit pinkScythe = new BaseUnit(pinkScytheSprite, "Pink Scythe", "Mage", "Spirit", skillSet3, 
                    97, 104, 109, 133, 100, 185);
                BaseUnit featherRaptor = new BaseUnit(new AnimatedSprite(featherRaptorSprite), "Feather Raptor", "common", "common", skillSet2, 
                    110, 105, 98, 70, 95, 60);
                BaseUnit WOODY_HAHA_XD = new BaseUnit(woodThumbSprite, "Woody", "Dragon", "Spirit", woodysSkills, 
                    100, 137, 85, 75, 110, 80);
                BaseUnit redMoth = new BaseUnit(redMothSprite, "Red Moth", "Beast", "Wild", skillSet4,
                    168, 64, 69, 103, 168, 169);

                battleManager.AddUnit(puffFly, 0);
                battleManager.AddUnit(pinkScythe, 1);
                battleManager.AddUnit(redMoth, 2);
                battleManager.AddUnit(WOODY_HAHA_XD, 3);

                battleManager.AddUnit(puffFly, 4);
                battleManager.AddUnit(featherRaptor, 5);
                battleManager.AddUnit(pinkScythe, 6);
                battleManager.AddUnit(spikePig, 7);

                battleManager.AddTextures(blankTexture, healthBarTexture, healthBarHighlightedTexture, skillTexture, panelCornerTexture);
                battleManager.AddSprites(burnSprite, poisonedSprite, bleedSprite, animHealthBarSprite, animHealthBarEnemySprite);
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
                    gameState = GameState.RunMainMenu;
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
