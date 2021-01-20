using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using XNALib;

namespace Invaders
{
    public class BonusEnemy : IXNADispose
    {
        #region Members
        private bool m_IsDisposed = false;
        /// <summary>
        /// Indicates if this enemy is alive
        /// </summary>
        public bool IsDisposed
        {
            get { return m_IsDisposed; }
            set { m_IsDisposed = value; }
        }

        /// <summary>
        /// This enemies AABB collision rectangle
        /// </summary>
        public FRect CollisionRect;

        /// <summary>
        /// Velocity
        /// </summary>
        private float Velocity = Maths.RandomNr(100, 200);
       
        /// <summary>
        /// Move direction
        /// </summary>
        private int MoveDir;

        /// <summary>
        /// The size
        /// </summary>
        private const int SIZE = 48;

        /// <summary>
        /// Texture
        /// </summary>
        private static readonly Texture2D Texture = Common.str2Tex("Enemies/bonusEnemy");

        /// <summary>
        /// Sprite effects (for left / right)
        /// </summary>
        private SpriteEffects Effects = SpriteEffects.None;
        #endregion

        public BonusEnemy()
        {
            if (Maths.RandomNr(0, 1) == 0)
            {
                // Move right
                MoveDir = 1;
                CollisionRect = new FRect(-SIZE, 0, SIZE, SIZE);
            }
            else // Move left
            {
                MoveDir = -1;
                CollisionRect = new FRect(Engine.Instance.Width, 32, SIZE, SIZE);
                Effects = SpriteEffects.FlipHorizontally;
            }
        }

        public void Update(GameTime gameTime, float elapsed)
        {
            CollisionRect.SetNewLocation(CollisionRect.TopLeft + new Vector2(MoveDir * Velocity * elapsed, 0));

            if ((MoveDir > 0 && CollisionRect.Left > Engine.Instance.Width) ||
                (MoveDir < 0 && CollisionRect.Right < 0))
                IsDisposed = true;
        }

        public void Draw()
        {
            Engine.Instance.SpriteBatch.Draw(Texture, CollisionRect.ToRect(), null, Color.White, 0f, Vector2.Zero, Effects, 1f);
        }
    }
}