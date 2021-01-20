using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using XNALib;

namespace Invaders
{
    public enum eEnemyType { Top = 0, Mid = 1, Bottom = 2 }
  
    public class Enemy : IXNADispose
    {
        #region Members
        /// <summary>
        /// The percentage of chance for this enemy to shoot when the shootDelay timer is done.
        /// </summary>
        private int ShootChance;

        /// <summary>
        /// The shoot delay timer
        /// </summary>
        private SimpleTimer ShootDelay = new SimpleTimer(Maths.RandomNr(900, 1100));

        private bool m_IsDisposed = false;
        /// <summary>
        /// IDisposable
        /// </summary>
        public bool IsDisposed
        {
            get { return m_IsDisposed; }
            set { m_IsDisposed = value; }
        }

        /// <summary>
        /// AABB collision rectangle (STRUCT)
        /// </summary>
        public FRect CollisionRect;

        /// <summary>
        /// Returns whether or not this enemy reached the end of the screen (left/right sides).
        /// </summary>
        public bool ReachedEndOfScreen { get { return CollisionRect.Left <= 0 || CollisionRect.Right >= Engine.Instance.Width; } }

        #region Movement
        /// <summary>
        /// The moving/facing direction enum
        /// </summary>
        private enum eFaceDirection { Left = -1, Right = 1 }
        
        /// <summary>
        /// The moving/facing direction
        /// </summary>
        private eFaceDirection FaceDirection = eFaceDirection.Right;

        /// <summary>
        /// The velocity for this enemy
        /// </summary>
        private float Velocity;
        #endregion

        /// <summary>
        /// Width of the enemy
        /// </summary>
        public const int WIDTH = 48;

        /// <summary>
        /// Height of the enemy
        /// </summary>
        public const int HEIGHT = 48;

        /// <summary>
        /// Texture for this enemy
        /// </summary>
        private Texture2D Texture;

        /// <summary>
        /// Amount of hitpoints
        /// </summary>
        private int HP;

        /// <summary>
        /// Indicates if this enemy is moving down the next update-cycle
        /// </summary>
        private bool IsMovingDown = false;
        
        /// <summary>
        /// The amount of distance that this enemy moved down since it was last required to move down.
        /// </summary>
        private float MovedDownDistance = 0f;

        /// <summary>
        /// The kill bounty
        /// </summary>
        public int ScoreValue;
        #endregion

        public Enemy(int levelNr, Vector2 location, eEnemyType type)
        {
            #region Assign velocity
            Velocity = 15f + levelNr * 3.0f;
            if (Velocity > 160.0f)
                Velocity = 160.0f;
            #endregion

            CollisionRect = new FRect(location, WIDTH, HEIGHT);

            #region Create enemy by type
            switch (type)
            {
                case eEnemyType.Top:
                    Texture = Common.str2Tex("Enemies/enemy01");
                    HP = 2;
                    ScoreValue = 30;
                    ShootChance = 4+levelNr/2;
                    if (ShootChance > 25)
                        ShootChance = 25;
                    break;
                case eEnemyType.Mid:
                    Texture = Common.str2Tex("Enemies/enemy02");
                    ScoreValue = 20;
                    HP = 1;
                    ShootChance = 2 + levelNr/2;
                    if (ShootChance > 15)
                        ShootChance = 15;
                    break;
                case eEnemyType.Bottom:
                    Texture = Common.str2Tex("Enemies/enemy03");
                    ScoreValue = 10;
                    HP = 1;
                    ShootChance = 1 + levelNr/2;
                    if (ShootChance > 10)
                        ShootChance = 10;
                    break;
            }
            #endregion
        }

        /// <summary>
        /// Call this method to make this enemy take damage
        /// </summary>
        /// <returns>Score value (0 when not killed)</returns>
        public int TakeDamage()
        {
            HP--;
            if (HP == 0)
            {
                IsDisposed = true;
                return ScoreValue;
            }
            return 0;
        }

        /// <summary>
        /// Turns this enemy around, forces it to move down the next update-cycle and increases it's velocity a bit.
        /// </summary>
        public void TurnAround()
        {
            if (!IsMovingDown) // Only turn around enemies that are not moving down.
            {
                FaceDirection = (eFaceDirection)((int)FaceDirection * -1);
                Velocity += 0.1f;
                IsMovingDown = true;
            }
        }

        public void Update(GameTime gameTime, float elapsed)
        {
            // Y-movement
            if (IsMovingDown)
            {
                CollisionRect.SetNewLocation(CollisionRect.TopLeft + new Vector2(0, Velocity * elapsed));
                MovedDownDistance += Velocity * elapsed;
                if (MovedDownDistance >= HEIGHT)
                {
                    MovedDownDistance = 0f;
                    IsMovingDown = false;

                    // Perform one X-movement to get away from the edge of the screen to prevent turning around again
                    CollisionRect.SetNewLocation(CollisionRect.TopLeft + new Vector2((float)FaceDirection * Velocity * elapsed, 0));
                }
            }
            else
            {                
                // X-movement
                CollisionRect.SetNewLocation(CollisionRect.TopLeft + new Vector2((float)FaceDirection * Velocity * elapsed, 0));
            }

            #region Shooting
            ShootDelay.Update(gameTime);
            if (ShootDelay.IsDone)
            {
                ShootDelay.Reset();
                if (Maths.Chance(ShootChance))
                {
                    Level.Instance.Projectiles.Add(new Projectile(CollisionRect.CenterLoc, null));
                }
            }
            #endregion
        }

        public void Draw()
        {
            Engine.Instance.SpriteBatch.Draw(Texture, CollisionRect.ToRect(), Color.White);
        }
    }
}