using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using XNALib;

namespace Invaders
{
    public class WallSegment
    {
        #region Members
        private int m_HP = 3;
        /// <summary>
        /// The amount of hits it can take before it is destroyed
        /// </summary>
        public int HP
        {
            get { return m_HP; }
            set
            {
                m_HP = value;
                if (value == 3)
                    DrawColor = Color.LimeGreen;
                else if (value == 2)
                    DrawColor = Color.Yellow;
                else
                    DrawColor = Color.Red;
            }
        }
       
        /// <summary>
        /// The AABB collision rectangle
        /// </summary>
        public FRect CollisionRect;
        
        /// <summary>
        /// The block background texture
        /// </summary>
        static readonly Texture2D BlockBG = Common.str2Tex("blockBG");
        
        /// <summary>
        /// The block shine texture. Gives the blocks a shining effect.
        /// </summary>
        static readonly Texture2D BlockShine = Common.str2Tex("blockShine");

        /// <summary>
        /// The draw color for this block
        /// </summary>
        Color DrawColor = Color.LimeGreen;
        #endregion

        public WallSegment(Vector2 location)
        {
            CollisionRect = new FRect(location, Wall.WALL_SEGMENT_SIZE, Wall.WALL_SEGMENT_SIZE);
        }

        public void Draw()
        {
            if (HP > 0)
            {
                Engine.Instance.SpriteBatch.Draw(BlockBG, CollisionRect.ToRect(), DrawColor);
                Engine.Instance.SpriteBatch.Draw(BlockShine, CollisionRect.ToRect(), Color.White);
            }
        }
    }
}