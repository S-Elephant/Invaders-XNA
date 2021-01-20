using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using XNALib;
using System.Text;

namespace Invaders
{
    public class Player
    {
        #region Members
        #region GUI
        /// <summary>
        /// Contanst string to prevent garbage
        /// </summary>
        private const string SCORE_STR = "Score: ";
        
        /// <summary>
        /// The location of the "Score " string
        /// </summary>
        private Vector2 ScoreLoc1;

        /// <summary>
        /// The location of the actual score number
        /// </summary>
        private Vector2 ScoreLoc2;
        #endregion

        /// <summary>
        /// The players ship texture
        /// </summary>
        private static readonly Texture2D Texture = Common.str2Tex("player");

        /// <summary>
        /// The players velocity
        /// </summary>
        private const float Velocity = 200f;
        
        /// <summary>
        /// Top-left location of the players ship
        /// </summary>
        private Vector2 Location
        {
            get { return CollisionRect.TopLeft; }
            set { CollisionRect.SetNewLocation(value); }
        }
       
        // Input
        Keys ShootKey, LeftKey, RightKey;

        /// <summary>
        /// Player index
        /// </summary>
        PlayerIndex PlayerIdx;

        /// <summary>
        /// The projectile that this player might have fired.
        /// </summary>
        Projectile Bullet = null;

        /// <summary>
        /// Indicates if the player is still alive.
        /// </summary>
        public bool IsAlive = true;

        /// <summary>
        /// AABB Collision rectangle (STRUCT)
        /// </summary>
        public FRect CollisionRect;

        private int m_Score = 0;
        /// <summary>
        /// The players score
        /// </summary>
        public int Score
        {
            get { return m_Score; }
            set
            {
                m_Score = value;
                ScoreSB.ClearCompact();
                ScoreSB.Append(value);
                if (m_Score >= NextScoreTarget)
                {
                    Level.Instance.Lives++;
                    NextScoreTarget += 1500;
                    Engine.Instance.Audio.PlaySound("bonusHit");
                }
            }
        }

        /// <summary>
        /// The score for this player in StringBuilder format.
        /// </summary>
        private StringBuilder ScoreSB = new StringBuilder("0");

        /// <summary>
        /// The amount of score to achieve in order to receive a free life.
        /// </summary>
        private int NextScoreTarget = 1500;

        /// <summary>
        /// Height of the players ship
        /// </summary>
        public const int HEIGHT = 64;

        /// <summary>
        /// The offset from the bottom of the ship with the bottom of the screen.
        /// </summary>
        public const int PLAYER_OFFSET_BOTTOM = 32;
        #endregion

        public Player(PlayerIndex playerIdx)
        {
            #region assign player index & input
            PlayerIdx = playerIdx;

            switch (PlayerIdx)
            {
                case PlayerIndex.One:
                    ShootKey = Keys.Space;
                    LeftKey = Keys.A;
                    RightKey = Keys.D;

                    ScoreLoc1 = new Vector2(5, 5);
                    ScoreLoc2 = new Vector2(95, 5);
                    break;
                case PlayerIndex.Two:
                    ShootKey = Keys.NumPad0;
                    LeftKey = Keys.NumPad4;
                    RightKey = Keys.NumPad6;

                    ScoreLoc1 = new Vector2(Engine.Instance.Width - 200, 5);
                    ScoreLoc2 = new Vector2(ScoreLoc1.X + Level.GuiFont.MeasureString(SCORE_STR).X, 5);
                    break;
                default:
                    break;
            }
            #endregion

            // Sets the collision rectangle and location
            CollisionRect = new FRect(Engine.Instance.Width / 2 - Texture.Width / 2, Level.DEAD_LINE, Texture.Width, Texture.Height);
        }

        /// <summary>
        /// Call this method to make the player take a hit.
        /// </summary>
        public void TakeDamage()
        {
            if (Level.Instance.Lives > 0)
                Level.Instance.Lives--;
            else
                IsAlive = false;
        }

        public void Update(GameTime gameTime, float elapsed)
        {
            if (IsAlive)
            {
                #region Input
                if (InputMgr.Instance.Keyboard.State.IsKeyDown(LeftKey)) // Move left
                {
                    Location += new Vector2(-Velocity * elapsed, 0);
                    if (Location.X < 0)
                        Location = new Vector2(0, Location.Y);
                }
                else if (InputMgr.Instance.Keyboard.State.IsKeyDown(RightKey)) // Move right
                {
                    Location += new Vector2(Velocity * elapsed, 0);
                    if(Location.X+CollisionRect.Width > Engine.Instance.Width)
                        Location = new Vector2(Engine.Instance.Width-Texture.Width, Location.Y);
                }

                // Attempt Shoot
                if (InputMgr.Instance.Keyboard.State.IsKeyDown(ShootKey))
                    Shoot();
                #endregion
            }

            // Update the players projectile, if any.
            if(Bullet != null && !Bullet.IsDisposed)
                Bullet.Update(gameTime, elapsed);
        }

        /// <summary>
        /// Attempts to fire a laser
        /// </summary>
        private void Shoot()
        {
            if (Bullet == null || Bullet.IsDisposed)
            {
                Level.Instance.ShotCnt++;
                Bullet = new Projectile(CollisionRect.CenterLoc, this);
            }
        }

        public void Draw()
        {
            if (Bullet != null && !Bullet.IsDisposed)
                Bullet.Draw();
         
            if(IsAlive)
                Engine.Instance.SpriteBatch.Draw(Texture, Location, Color.White);
        }

        public void DrawGUI()
        {
            // Draw score
            Engine.Instance.SpriteBatch.DrawString(Level.GuiFont, SCORE_STR, ScoreLoc1, Color.Goldenrod);
            Engine.Instance.SpriteBatch.DrawString(Level.GuiFont, ScoreSB, ScoreLoc2, Color.Goldenrod);
        }
    }
}