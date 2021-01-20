using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using XNALib;

namespace Invaders
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

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
            Global.Content = Content;
            Engine.Instance = new Engine();
            Engine.Instance.Graphics = graphics;
            Engine.Instance.Game = this;

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
            Engine.Instance.SpriteBatch = spriteBatch;

            #region Resolution
            Resolution.Init(ref graphics);
            Resolution.SetResolution(1280, 800, false);
            Resolution.SetVirtualResolution(1280, 720); // Don't change
            graphics.ApplyChanges();
            #endregion

            InputMgr.Instance = new InputMgr();
            Engine.Instance.ActiveState = new MainMenu();
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Get input
            InputMgr.Instance.Update(gameTime);

            // Exit game & return to menu
            if (InputMgr.Instance.IsPressed(null, InputMgr.Instance.DefaultCancelKey, InputMgr.Instance.DefaultCancelButton))
            {
                if (Engine.Instance.ActiveState is MainMenu)
                    Exit();
                else
                    Engine.Instance.ActiveState = new MainMenu();
            }

            // Update the active entity
            Engine.Instance.ActiveState.Update(gameTime);

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Resolution.GetScaleMatrix());
            Engine.Instance.ActiveState.Draw();
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
