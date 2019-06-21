using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Game.GameData;
using Game.GameData.Units;
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
        //public static SpriteFont fontTimesNewRoman;
        //public static SpriteFont DamageFont;

        public struct Font
        {
            public static SpriteFont DamageFont;
            public static SpriteFont TimesNewRoman;
            public static SpriteFont TimesNewRoman14;
            public static SpriteFont Cambira;
            public static SpriteFont CambiraHalf;
        }

        Texture2D knightTexture;
        Texture2D knightNoHelmetTexture;
        Texture2D crimsonKnightTexture;
        Texture2D darkKnightTexture;

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Texture2D cursorTexture;
        Texture2D backgroundTexture;
        Texture2D tileTexture;
        Texture2D unitPanelTexture;
        Texture2D unitPanelTexture1;

        private struct TerrainTexture
        {
            public static Texture2D simpleRock;
        }

        Rectangle cursorRect;
        Rectangle backgroundRect;
        Vector2 cursorPosition;
        Color cursorColor;
        Vector2 fontPosition;
        Random rnd = new Random();
        protected int score;
        private Vector2 gridSize;

        private Tile[] tileArray;
        private Unit selectedUnit;
        private Tile selectedTile;
        private Unit hoveredUnit;

        // Button press variables
        private bool leftMousePressed = false;
        private bool leftMousePressedLast = false;
        private bool rightMousePressed = false;
        private bool rightMousePressedLast = false;
        private MouseState leftMouseState = MouseState.None;
        private MouseState rightMouseState = MouseState.None;

        private UnitPanel selectedUnitPanel;
        private UnitPanel hoveredUnitPanel;

        private enum MouseState
        {
            None,
            MouseDown,
            MouseUp
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

            // GAME
            cursorTexture     = Content.Load<Texture2D>("cursor");
            backgroundTexture = Content.Load<Texture2D>("fantasy-background");
            tileTexture       = Content.Load<Texture2D>("skill-square");
            unitPanelTexture  = Content.Load<Texture2D>("unit-panel-background");
            unitPanelTexture1 = Content.Load<Texture2D>("path843");

            // UNITS
            knightTexture         = Content.Load<Texture2D>("knight-small");
            knightNoHelmetTexture = Content.Load<Texture2D>("knight-no-helmet");
            crimsonKnightTexture = Content.Load<Texture2D>("crimson-knight-0");
            darkKnightTexture = Content.Load<Texture2D>("dark-knight-0");

            // TERRAIN
            TerrainTexture.simpleRock = Content.Load<Texture2D>("simple-rock");

            // FONTS
            font              = Content.Load<SpriteFont>("SpriteFont1");
            Font.TimesNewRoman = Content.Load<SpriteFont>("TimesNewRomanSmall");
            Font.TimesNewRoman14 = Content.Load<SpriteFont>("TimesNewRoman14");
            Font.DamageFont = Content.Load<SpriteFont>("DamageFont");
            Font.Cambira = Content.Load<SpriteFont>("Cambria");
            Font.CambiraHalf = Content.Load<SpriteFont>("CambriaHalf");

            // ASSIGN TEXTURES

            // Assign textures to Tile objects
            for (int i = 0; i < tileArray.Length; i++)
            {
                tileArray[i].SetTexture(tileTexture);
            }

            // Add Unit(s) to random Tile(s)
            tileArray[9].AddUnit(new GreyKnight(knightTexture));
            tileArray[14].AddUnit(new RenegadeKnight(knightNoHelmetTexture));
            tileArray[21].AddUnit(new Crimson_Knight(crimsonKnightTexture));
            tileArray[2].AddUnit(new Dark_Knight(darkKnightTexture));

            // AddTerrain() to random Tile(s)
            tileArray[10].AddTerrain(new Terrain(TerrainTexture.simpleRock));

            // Add texture to unit panels
            selectedUnitPanel = new UnitPanel(unitPanelTexture1, new Vector2(graphics.GraphicsDevice.Viewport.Width, graphics.GraphicsDevice.Viewport.Height), true);
            hoveredUnitPanel = new UnitPanel(unitPanelTexture1, new Vector2(graphics.GraphicsDevice.Viewport.Width, graphics.GraphicsDevice.Viewport.Height), false);
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

            // Update mouse events
            if (Mouse.GetState().LeftButton == ButtonState.Pressed)
            {
                if(!leftMousePressedLast)
                {
                    leftMouseState = MouseState.MouseDown;
                }
                else
                {
                    leftMouseState = MouseState.None;
                }

                leftMousePressedLast = true;
            }
            else
            {
                if(leftMousePressedLast)
                {
                    leftMouseState = MouseState.MouseUp;
                }
                else
                {
                    leftMouseState = MouseState.None;
                }

                leftMousePressedLast = false;
            }

            if (Mouse.GetState().RightButton == ButtonState.Pressed)
            {
                if (!rightMousePressedLast)
                {
                    rightMouseState = MouseState.MouseDown;
                }
                else
                {
                    rightMouseState = MouseState.None;
                }

                rightMousePressedLast = true;
            }
            else
            {
                if (rightMousePressedLast)
                {
                    rightMouseState = MouseState.MouseUp;
                }
                else
                {
                    rightMouseState = MouseState.None;
                }

                rightMousePressedLast = false;
            }

            // update cursor on click
            if (Mouse.GetState().LeftButton == ButtonState.Pressed)
            {
                cursorColor = Color.LightSlateGray;
            }
            else if (selectedTile != null)
            {
                // Signal unit can attack
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


            // Handle clicking/selecting/targeting units

            hoveredUnit = null;

            // Check each tile
            for (int i = 0; i < tileArray.Length; i++)
            {
                // Mousedown on unit
                if (cursorRect.Intersects(tileArray[i].GetUnitRectangle()) && leftMouseState == MouseState.MouseDown)
                {
                    cursorColor = Color.LightGreen;

                    // Click on Unit to select it
                    // First check that unit isn't already selected
                    if (!tileArray[i].GetUnit().Equals(selectedUnit))
                    {
                        if (selectedTile != null)
                        {
                            //selectedTile.RemoveTerrain();
                        }
                        if (selectedUnit != null)
                        {
                            // Unit is selected, clicked on another unit = attack
                            Tile targetTile = tileArray[i];
                            Unit targetUnit = targetTile.GetUnit();
                            selectedUnit.StartCombat(targetUnit, 1, 1.2f, Skill.DamageType.Physical);

                        }
                        else
                        {
                            // Select unit
                            //selectedTile = tileArray[i];
                            //selectedUnit = tileArray[i].GetUnit();
                            //tileArray[i].AddTerrain(new Terrain(TerrainTexture.simpleRock));
                            SelectTile(tileArray[i]);
                        }
                    }
                    else // Clicked on selected unit
                    {
                        // Deselect unit/tile
                        DeselectTile();
                    }
                }

                // Move unit
                // Mousedown on empty tile with unit selected
                if (cursorRect.Intersects(tileArray[i].GetRectangle()) // cursor rect intersects tile rect
                    && rightMouseState.Equals(MouseState.MouseDown) // mousedown
                    && !tileArray[i].HasUnit() // tile doesn't have unit
                    && !tileArray[i].HasTerrain() // tile doesn't have terrain
                    && selectedUnit != null) // there is a unit selected
                {
                    tileArray[i].AddUnit(selectedTile.RemoveUnit());

                    DeselectTile();
                }
           
                if (cursorRect.Intersects(tileArray[i].GetUnitRectangle())
                    && selectedUnit != null
                    && (tileArray[i].HasUnit() && !tileArray[i].GetUnit().Equals(selectedUnit)))
                {
                    hoveredUnit = tileArray[i].GetUnit();
                }
                else if (!cursorRect.Intersects(tileArray[i].GetUnitRectangle()) && leftMouseState == MouseState.MouseDown)
                {
                    // Deselect unit
                    //selectedTile = null;
                    //selectedUnit = null;
                    //tileArray[i].RemoveTerrain();
                }
                else if(!cursorRect.Intersects(tileArray[i].GetUnitRectangle()))
                {
                    

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
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
            spriteBatch.Draw(backgroundTexture, backgroundRect, Color.White);

            foreach(Tile tile in tileArray)
            {
                tile.Draw(spriteBatch);
            }

            if (selectedUnit != null)
            {
                Skill[] skillArray = selectedUnit.GetSkillList();

                

                // Draw unit panel
                selectedUnitPanel.SetUnit(selectedUnit);
                selectedUnitPanel.Draw(spriteBatch);
            }

            if (hoveredUnit != null)
            {
                hoveredUnitPanel.SetUnit(hoveredUnit);
                hoveredUnitPanel.Draw(spriteBatch);
            }

            spriteBatch.Draw(cursorTexture, cursorRect, cursorColor);
            //spriteBatch.DrawString(font, "Score: " + score, fontPosition, Color.White);

            spriteBatch.End();

            base.Draw(gameTime);
        }


        // OTHER METHODS

        private void SelectTile(Tile tile)
        {
            selectedTile = tile;

            selectedTile.HighlightTile(Color.LightGreen);

            if(selectedTile.HasUnit())
            {
                selectedUnit = selectedTile.GetUnit();
            }
            else
            {
                selectedUnit = null;
            }
        }

        private void DeselectTile()
        {
            selectedTile.RemoveHighlight();

            selectedTile = null;
            selectedUnit = null;
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