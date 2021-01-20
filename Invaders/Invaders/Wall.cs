using System.Collections.Generic;
using Microsoft.Xna.Framework;
using XNALib;

namespace Invaders
{
    public class Wall
    {
        #region Members
        /// <summary>
        /// The size of a segment in the wall
        /// </summary>
        public const int WALL_SEGMENT_SIZE = 32;

        /// <summary>
        /// The list of segments in this wall
        /// </summary>
        List<WallSegment> Segments = new List<WallSegment>();
        
        /// <summary>
        /// The top-left location of this wall
        /// </summary>
        Vector2 Location;

        /// <summary>
        /// The total AABB of this entire wall
        /// </summary>
        FRect WallBoundary;
        #endregion

        public Wall(Vector2 location)
        {
            Location = location;
            WallBoundary = new FRect(Location, 3 * WALL_SEGMENT_SIZE, 3 * WALL_SEGMENT_SIZE);

            for (int y = 0; y < 3; y++)
            {
                for (int x = 0; x < 3; x++)
                    Segments.Add(new WallSegment(Location + new Vector2(x * WALL_SEGMENT_SIZE, y * WALL_SEGMENT_SIZE)));
            }
        }

        /// <summary>
        /// Heals every segment in the wall. Destroyed segments will not be 'resurrected'.
        /// </summary>
        public void Heal()
        {
            foreach (WallSegment s in Segments)
            {
                if (s.HP > 0 && s.HP < 3)
                    s.HP++;
            }
        }

        /// <summary>
        /// Determines collision with the Wall AABB and if that yields true it will check the supplied FRect against it's wall segments. If any of those also collides then that one will take damage.
        /// </summary>
        /// <param name="collisionRect">The AABB of the collider</param>
        /// <returns>true if any segment collides with the supplied AABB</returns>
        public bool Collides(FRect collisionRect)
        {
            if (WallBoundary.Intersects(collisionRect))
            {
                foreach (WallSegment segment in Segments)
                {
                    if (segment.CollisionRect.Intersects(collisionRect) && segment.HP > 0)
                    {
                        segment.HP--;
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// Renders every segment in this wall
        /// </summary>
        public void Draw()
        {
            Segments.ForEach(s => s.Draw());
        }
    }
}