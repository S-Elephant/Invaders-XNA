using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using XNALib;

namespace Invaders
{
    public class Engine
    {
        /// <summary>
        /// Singleton instance
        /// </summary>
        public static Engine Instance;

        /// <summary>
        /// The active entity
        /// </summary>
        public IActiveState ActiveState { get; set; }

        /// <summary>
        /// The reference to the Game
        /// </summary>
        public Game Game { get; set; }

        /// <summary>
        /// Handles all audio playback
        /// </summary>
        public XactMgr Audio = new XactMgr("Invaders", "Default", "Music");

        /// <summary>
        /// The GraphicsDeviceManager
        /// </summary>
        public GraphicsDeviceManager Graphics;

        /// <summary>
        /// The screen width in pixels
        /// </summary>
        public int Width { get { return Graphics.PreferredBackBufferWidth; } }

        /// <summary>
        /// The screen height in pixels
        /// </summary>
        public int Height { get { return Graphics.PreferredBackBufferHeight; } }

        /// <summary>
        /// The screen area as a Rectangle
        /// </summary>
        public Rectangle ScreenArea { get { return new Rectangle(0, 0, Graphics.PreferredBackBufferWidth, Graphics.PreferredBackBufferHeight); } }

        /// <summary>
        /// The spritebatch used to render everything.
        /// </summary>
        public SpriteBatch SpriteBatch { get; set; }
    }
}
