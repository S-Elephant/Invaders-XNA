using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using XNALib;

namespace Invaders
{
    public class Projectile : IXNADispose
    {
        #region Members
        private bool m_IsDisposed = false;
        /// <summary>
        /// Indicates if this projectile is still 'alive'
        /// </summary>
        public bool IsDisposed
        {
            get { return m_IsDisposed; }
            set { m_IsDisposed = value; }
        }

        /// <summary>
        /// The texture for this projectile
        /// </summary>
        private Texture2D Texture;

        /// <summary>
        /// The owner. Null indicates the AI.
        /// </summary>
        private Player Owner;

        /// <summary>
        /// The direction this projectile is moving in
        /// </summary>
        private Vector2 MoveDir;

        /// <summary>
        /// The velocity
        /// </summary>
        private float Velocity;

        /// <summary>
        /// The AABB Collision Rectangle
        /// </summary>
        private FRect CollisionRect;

        /// <summary>
        /// The width
        /// </summary>
        private const int WIDTH = 8;

        /// <summary>
        /// Thw height
        /// </summary>
        private const int HEIGHT = 10;
        #endregion

        public Projectile(Vector2 centerLoc, Player owner)
        {
            Owner = owner;
            if (Owner == null)
            {// enemy
                MoveDir = new Vector2(0, 1);
                Texture = Common.str2Tex("projectile01");
                Velocity = 220;
            }
            else
            {// player
                MoveDir = new Vector2(0, -1);
                Texture = Common.str2Tex("projectile02");
                Velocity = 350;
            }

            CollisionRect = new FRect(centerLoc.X - WIDTH / 2, centerLoc.Y - HEIGHT / 2, WIDTH, HEIGHT);
        }

        public void Update(GameTime gameTime, float elapsed)
        {
            // Move
            CollisionRect.SetNewLocation(CollisionRect.TopLeft + Velocity * MoveDir * elapsed);

            #region Check collision

            // Collision with walls
            foreach (Wall w in Level.Instance.Walls)
            {
                if (w.Collides(CollisionRect))
                {
                    IsDisposed = true;
                    Engine.Instance.Audio.PlaySound("enemyHit");
                    return;
                }
            }

            if (Owner == null)
            {
                // Outside screen
                if (CollisionRect.Top > Engine.Instance.Height)
                {
                    IsDisposed = true;
                    return;
                }

                // Collision with player(s)
                foreach (Player p in Level.Instance.Players)
                {
                    if (p.CollisionRect.Intersects(CollisionRect))
                    {
                        p.TakeDamage();
                        IsDisposed = true;
                        Level.Instance.Projectiles.Remove(this);
                        Engine.Instance.Audio.PlaySound("enemyHit");
                        return;
                    }
                }
            }
            else
            {
                // Outside screen
                if (CollisionRect.Bottom < 0)
                {
                    IsDisposed = true;
                    return;
                }

                // Collision with enemies
                foreach (Enemy e in Level.Instance.Enemies)
                {
                    if (e.CollisionRect.Intersects(CollisionRect))
                    {
                        Owner.Score += e.TakeDamage();
                        IsDisposed = true;
                        Engine.Instance.Audio.PlaySound("enemyHit");
                        return;
                    }
                }

                // Collision with bonus enemy
                if (Level.Instance.BonusEnemy != null)
                {
                    if ((Level.Instance.BonusEnemy.CollisionRect.Intersects(CollisionRect)))
                    {
                        Level.Instance.BonusEnemy.IsDisposed = true;
                        Owner.Score+=300;
                        IsDisposed = true;
                        Engine.Instance.Audio.PlaySound("bonusHit");
                        return;
                    }
                }
            }
            #endregion
        }

        public void Draw()
        {
            Engine.Instance.SpriteBatch.Draw(Texture, CollisionRect.TopLeft, Color.White);
        }
    }
}