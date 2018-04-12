using System;
using System.Collections;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using MonoGame.Extended.Tiled;
using MonoGame.Extended.Tiled.Graphics;
using MonoGame.Extended.ViewportAdapters;
using Microsoft.Xna.Framework.Media;
using System.Collections.Generic;

namespace KnightPlatformer
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        public Player player = new Player();

        Song gameMusic;

        SpriteFont lucidaFont;
        Texture2D heart = null;
        Texture2D coin = null;
        public int lives = 3;
        public int coins = 0;

        Camera2D camera = null;
        TiledMap map = null;
        TiledMapRenderer mapRenderer = null;
        TiledMapTileLayer collisionLayer;
        TiledMapTileLayer spikesLayer;
        public ArrayList allCollisionTiles = new ArrayList();
        public ArrayList allSpikeTiles = new ArrayList();
        public Sprite[,] levelGrid;
        public Spikes[,]  spikesLevelGrid;

        public int tileHeight = 0;
        public int levelTileWidth = 0;
        public int levelTileHeight = 0;

        public List<Enemy> enemies = new List<Enemy>();
        public Chest goal = null;

        public Vector2 gravity = new Vector2(0, 1500);

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
            // TODO: Add your initialization logic here

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
            player.Load(Content, this);

            lucidaFont = Content.Load<SpriteFont>("fonts/Lucida");
            heart = Content.Load<Texture2D>("sprites/heart");
            coin = Content.Load<Texture2D>("sprites/coin");

            var viewportAdapter = new BoxingViewportAdapter(Window, GraphicsDevice, graphics.GraphicsDevice.Viewport.Width, graphics.GraphicsDevice.Viewport.Height);
            camera = new Camera2D(viewportAdapter);
            camera.Position = new Vector2(0, graphics.GraphicsDevice.Viewport.Height);

            map = Content.Load<TiledMap>("Level1");
            mapRenderer = new TiledMapRenderer(GraphicsDevice);

            SetUpTiles();
            LoadObjects();

            gameMusic = Content.Load<Song>("sounds/harp");
            MediaPlayer.Play(gameMusic);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        public void SetUpTiles()
        {
            tileHeight = map.TileHeight;
            levelTileHeight = map.Height;
            levelTileWidth = map.Width;
            levelGrid = new Sprite[levelTileWidth, levelTileHeight];
            spikesLevelGrid= new Spikes[levelTileWidth, levelTileHeight];

            foreach (TiledMapTileLayer layer in map.TileLayers)
            {
                if(layer.Name == "Collision")
                {
                    collisionLayer = layer;
                }

                if (layer.Name == "Spikes")
                {
                    spikesLayer = layer;
                }
            }

            int columns = 0;
            int rows = 0;
            int loopCount = 0;
            while(loopCount < collisionLayer.Tiles.Count)
            {
                if(collisionLayer.Tiles[loopCount].GlobalIdentifier != 0)
                {
                    Sprite tileSprite = new Sprite();
                    tileSprite.position.X = columns * tileHeight;
                    tileSprite.position.Y = rows * tileHeight;
                    tileSprite.tileCoordinates = new Vector2(columns, rows);

                    tileSprite.width = tileHeight;
                    tileSprite.height = tileHeight;

                    tileSprite.UpdateHitBox();

                    allCollisionTiles.Add(tileSprite);
                    levelGrid[columns, rows] = tileSprite;

                    Console.WriteLine(player.playerSprite.position);
                }

                columns++;

                if(columns == levelTileWidth)
                {
                    columns = 0;
                    rows++;
                }

                loopCount++;
            }

            columns = 0;
            rows = 0;
            loopCount = 0;
            while (loopCount < spikesLayer.Tiles.Count)
            {
                if (spikesLayer.Tiles[loopCount].GlobalIdentifier != 0)
                {
                    Spikes spike = new Spikes();
                    spike.spikeSprite.position.X = columns * tileHeight;
                    spike.spikeSprite.position.Y = rows * tileHeight;
                    spike.spikeSprite.tileCoordinates = new Vector2(columns, rows);

                    spike.Load(Content, this);

                    spike.spikeSprite.width = spike.spikeSprite.texture.Bounds.Width;
                    spike.spikeSprite.height = spike.spikeSprite.texture.Bounds.Height;

                    spike.spikeSprite.UpdateHitBox();

                    allSpikeTiles.Add(spike);
                    //spikesLevelGrid[columns, rows] = spike;
                }

                columns++;

                if (columns == levelTileWidth)
                {
                    columns = 0;
                    rows++;
                }

                loopCount++;
            }
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
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            foreach (Enemy enemy in enemies)
            {
                enemy.Update(deltaTime);
            }

            player.Update(deltaTime);

            if(allSpikeTiles.Count > 0)
            {
                foreach (Spikes spike in allSpikeTiles)
                {
                    spike.Update(deltaTime);
                }
            }            

            camera.Position = player.playerSprite.position - new
                Vector2(graphics.GraphicsDevice.Viewport.Width / 2,
                graphics.GraphicsDevice.Viewport.Height / 2);

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            var viewMatrix = camera.GetViewMatrix();
            var projectionMatrix = Matrix.CreateOrthographicOffCenter(0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height, 0, 0f, -1f);
            
            // TODO: Add your drawing code here
            spriteBatch.Begin(transformMatrix: viewMatrix);
            mapRenderer.Draw(map, ref viewMatrix, ref projectionMatrix);
            player.Draw(spriteBatch);

            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            goal.Draw(spriteBatch);

            foreach (Enemy enemy in enemies)
            {
                enemy.Draw(spriteBatch);
            }
            
            // draw all the GUI components in a separate SpritebatchBatch  section
            // Lives
            spriteBatch.Draw(heart, new Vector2(player.playerSprite.position.X - 390, player.playerSprite.position.Y - 235));
            spriteBatch.DrawString(lucidaFont, lives.ToString("00"), new Vector2(player.playerSprite.position.X - 350, player.playerSprite.position.Y - 230), Color.Red);

            // Coins
            spriteBatch.DrawString(lucidaFont, coins.ToString("00"), new Vector2(player.playerSprite.position.X - 280, player.playerSprite.position.Y - 230), Color.Gold);
            spriteBatch.Draw(coin, new Vector2(player.playerSprite.position.X - 320, player.playerSprite.position.Y - 235));

            spriteBatch.End();

            base.Draw(gameTime);
        }

        void LoadObjects()
        {
            foreach(TiledMapObjectLayer layer in map.ObjectLayers)
            {
                if(layer.Name == "Enemies")
                {
                    foreach(TiledMapObject thing in layer.Objects)
                    {
                        Enemy enemy = new Enemy();
                        enemy.enemySprite.position = new Vector2(thing.Position.X, thing.Position.Y);
                        enemy.Load(Content, this);
                        enemies.Add(enemy);
                    }
                    
                }
                if(layer.Name == "Goal")
                {
                    TiledMapObject thing = layer.Objects[0];
                    if(thing != null)
                    {
                        Chest chest = new Chest();
                        chest.chestSprite.position = new Vector2(thing.Position.X, thing.Position.Y);
                        chest.Load(Content, this);
                        goal = chest;
                    }
                }
            }
        }
    }
}
