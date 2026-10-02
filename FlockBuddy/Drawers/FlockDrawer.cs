using CollisionBuddy;
using FlockBuddy.Interfaces;
using Microsoft.Xna.Framework;
using PrimitiveBuddy;

namespace FlockBuddy.Drawers
{
    public class FlockDrawer : IFlockDrawer
    {
        IMoverDrawer MoverDrawer { get; set; } = new MoverDrawer();

        public void Draw(IFlock flock, IPrimitive prim, Color color)
        {
            lock (flock.ListLock)
            {
                foreach (var boid in flock.Boids)
                {
                    MoverDrawer.Draw(boid, prim, color);
                    MoverDrawer.DrawSpeedForce(boid, prim, Color.White);
                }
            }
        }

        /// <summary>
        /// draw a bunch of debug info
        /// </summary>
        /// <param name="prim"></param>
        public void DrawCells(IFlock flock, IPrimitive prim)
        {
            lock (flock.ListLock)
            {
                if (flock.UseCellSpace)
                {
                    flock.CellSpace.RenderCells(prim);
                }
            }
        }

        /// <summary>
        /// draw the vectors of all the boids
        /// </summary>
        /// <param name="prim"></param>
        public void DrawTotalForce(IFlock flock, IPrimitive prim, Color color)
        {
            lock (flock.ListLock)
            {
                foreach (var boid in flock.Boids)
                {
                    MoverDrawer.DrawTotalForce(boid, prim, color);
                }
            }
        }

        /// <summary>
        /// draw the wall whiskers of all the boids
        /// </summary>
        /// <param name="prim"></param>
        public void DrawWhiskers(IFlock flock, IPrimitive prim, Color color)
        {
            lock (flock.ListLock)
            {
                foreach (var boid in flock.Boids)
                {
                    MoverDrawer.DrawWallFeelers(boid, prim, color);
                }
            }
        }

        /// <summary>
        /// draw all the walls 
        /// </summary>
        /// <param name="prim"></param>
        public void DrawWalls(IFlock flock, IPrimitive prim)
        {
            foreach (var wall in flock.Walls)
            {
                var line = wall as Line;
                if (null != line)
                {
                    line.Draw(prim, Color.Black);
                }
            }
        }
    }
}