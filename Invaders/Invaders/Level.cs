using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using XNALib;

namespace Invaders
{
    public class Level : IActiveState
    {
        #region Members
        /// <summary>
        /// Constant string to prevent garbage
        /// </summary>
        private const string LIVES_STR = "Lives: ";

        /// <summary>
        /// The font for the gui
        /// </summary>
        public static readonly SpriteFont GuiFont = Common.str2Font("Gui");

        /// <summary>
        /// Singleton instance
        /// </summary>
        public static Level Instance;

        #region Collections
        /// <summary>
        /// The player(s)
        /// </summary>
        public List<Player> Players = new List<Player>();

        /// <summary>
        /// The enemies
        /// </summary>
        public List<Enemy> Enemies = new List<Enemy>();

        /// <summary>
        /// The projectiles
        /// </summary>
        public List<Projectile> Projectiles = new List<Projectile>();

        /// <summary>
        /// The walls
        /// </summary>
        public List<Wall> Walls = new List<Wall>();
        #endregion

        private int m_Lives = 3;
        /// <summary>
        /// The remaining lives for the player(s)
        /// </summary>
        public int Lives
        {
            get { return m_Lives; }
            set
            {
                m_Lives = value;
                LivesSB.ClearCompact();
                LivesSB.Append(LIVES_STR);
                LivesSB.Append(value);
            }
        }
       
        /// <summary>
        /// The remaining lives formatted and in StringBuilder format to prevent garbage.
        /// </summary>
        private StringBuilder LivesSB = new StringBuilder("Lives: 3");

        /// <summary>
        /// The level number
        /// </summary>
        public int LevelNr = 0;

        #region enemy location/spacing constants
        const int ENEMY_SPACING_X = 10;
        const int ENEMY_SPACING_Y = 2;
        const int ENEMY_OFFSET_TOP = 96;
        #endregion

        /// <summary>
        /// The Y-line that, if should be reached by an enemy, will lose the player(s) the game.
        /// </summary>
        public const int DEAD_LINE = 720 - Player.HEIGHT - Player.PLAYER_OFFSET_BOTTOM;

        #region Bonus
        /// <summary>
        /// The current active bonus enemy
        /// </summary>
        public BonusEnemy BonusEnemy = null;

        /// <summary>
        /// The amount of shots needed for the bonus enemy to appear
        /// </summary>
        int ShotsForBonus;

        /// <summary>
        /// The amount of shots fired since the last bonus
        /// </summary>
        public int ShotCnt;

        /// <summary>
        /// Indicates if the bonus has already appeared this round
        /// </summary>
        bool BonusAppearedThisRound;
        #endregion

        /// <summary>
        /// Background texture
        /// </summary>
        Texture2D BG;
        #endregion

        public Level(int playerCnt)
        {
            #region Add player(s)
            Players.Add(new Player(PlayerIndex.One));
            if(playerCnt == 2)
                Players.Add(new Player(PlayerIndex.Two));
            #endregion

            #region Walls
            Walls.Add(new Wall(new Vector2(200, DEAD_LINE - 4 * Wall.WALL_SEGMENT_SIZE)));
            Walls.Add(new Wall(new Vector2(400, DEAD_LINE - 4 * Wall.WALL_SEGMENT_SIZE)));
            Walls.Add(new Wall(new Vector2(600, DEAD_LINE - 4 * Wall.WALL_SEGMENT_SIZE)));
            Walls.Add(new Wall(new Vector2(800, DEAD_LINE - 4 * Wall.WALL_SEGMENT_SIZE)));
            Walls.Add(new Wall(new Vector2(1000, DEAD_LINE - 4 * Wall.WALL_SEGMENT_SIZE)));
            #endregion
        }

        /// <summary>
        /// Proceeds to game to the next level
        /// </summary>
        /// <param name="currentLvlNr"></param>
        public void NextLevel(int currentLvlNr)
        {
            LevelNr = currentLvlNr + 1;

            #region Clear
            Enemies.Clear();
            Projectiles.Clear();
            BonusEnemy = null;
            #endregion

            #region Bonus
            ShotsForBonus = Maths.RandomNr(15, 23); // Sets the amount of shots needed before the bonus appears
            BonusAppearedThisRound = false;
            ShotCnt = 0;
            #endregion

            // Heal the Walls
            Walls.ForEach(w => w.Heal());

            // Set random background
            BG = Common.str2Tex("BG/invaderBG0" + Maths.RandomNr(1, 3));

            #region Load the enemies
            for (int x = 0; x < 11; x++)
                Enemies.Add(new Enemy(LevelNr, new Vector2(100 + x * (Enemy.WIDTH + ENEMY_SPACING_X), ENEMY_OFFSET_TOP), eEnemyType.Top));
            for (int x = 0; x < 11; x++)
            {
                for (int y = 1; y < 3; y++)
                    Enemies.Add(new Enemy(LevelNr, new Vector2(100 + x * (Enemy.WIDTH + ENEMY_SPACING_X), ENEMY_OFFSET_TOP + y * (ENEMY_SPACING_Y + Enemy.HEIGHT)), eEnemyType.Mid));
            }
            for (int x = 0; x < 11; x++)
            {
                for (int y = 3; y < 5; y++)
                    Enemies.Add(new Enemy(LevelNr, new Vector2(100 + x * (Enemy.WIDTH + ENEMY_SPACING_X), ENEMY_OFFSET_TOP + y * (ENEMY_SPACING_Y + Enemy.HEIGHT)), eEnemyType.Bottom));
            }
            #endregion

            // Clean up
            GC.Collect();
        }

        public void Update(GameTime gameTime)
        {
            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;

            Players.ForEach(p => p.Update(gameTime, elapsed));
            Projectiles.ForEach(p => p.Update(gameTime, elapsed));
            Projectiles.RemoveAll(p=>p.IsDisposed);

            // Enemies
            if (BonusEnemy != null)
            {
                BonusEnemy.Update(gameTime, elapsed);
                if (BonusEnemy.IsDisposed)
                    BonusEnemy = null;
            }
            Enemies.ForEach(e => e.Update(gameTime, elapsed));
            Enemies.RemoveAll(e=>e.IsDisposed);
            if(Enemies.Any(e=>e.ReachedEndOfScreen))
                Enemies.ForEach(e=>e.TurnAround());

            // Bonus
            if (!BonusAppearedThisRound && ShotCnt >= ShotsForBonus)
            {
                BonusAppearedThisRound = true;
                BonusEnemy = new BonusEnemy();
            }

            // Next level check/handling
            if (Enemies.Count == 0 && BonusEnemy == null)
                NextLevel(LevelNr);

            // Gameover check/handling
            if (!Players.Any(p => p.IsAlive) || Enemies.Any(e => e.CollisionRect.Bottom >= DEAD_LINE))
                GameOver();

            // Music
            if (InputMgr.Instance.IsPressed(null,Keys.M, Buttons.RightShoulder))
                Engine.Instance.Audio.PlayMusic("music", true);
        }

        /// <summary>
        /// Call this method when the player(s) lost the game
        /// </summary>
        private void GameOver()
        {
            Engine.Instance.ActiveState = new MainMenu();
        }

        public void Draw()
        {
            Engine.Instance.SpriteBatch.Draw(BG, Engine.Instance.ScreenArea, Color.White);

            Walls.ForEach(w => w.Draw());
            if (Enemies.Any(e => Math.Abs(DEAD_LINE - e.CollisionRect.Bottom) <= Enemy.HEIGHT))
                Engine.Instance.SpriteBatch.Draw(Common.White1px50Trans, new Rectangle(0, DEAD_LINE, Engine.Instance.Width, 2), Color.Red);

            Players.ForEach(p => p.Draw());
            if (BonusEnemy != null)
                BonusEnemy.Draw();
            Enemies.ForEach(e => e.Draw());
            Projectiles.ForEach(p => p.Draw());

            #region GUI
            // Draw lives
            Engine.Instance.SpriteBatch.DrawString(GuiFont, LivesSB, new Vector2(Engine.Instance.Width / 2 - GuiFont.MeasureString(LivesSB).X / 2, 0), Color.Goldenrod);

            // Draw Score
            foreach (Player p in Players)
                p.DrawGUI();
            #endregion
        }
    }
}
