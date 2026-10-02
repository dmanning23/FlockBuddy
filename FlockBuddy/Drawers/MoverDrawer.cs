using FlockBuddy.Interfaces;
using Microsoft.Xna.Framework;
using PrimitiveBuddy;

namespace FlockBuddy.Drawers
{
    /// <summary>
    /// This is a helper class for drawing movers.
    /// </summary>
    public class MoverDrawer : BaseEntityDrawer, IMoverDrawer
    {
        public MoverDrawer()
        {
        }

        public virtual void Draw(IMover mover, IPrimitive prim, Color color)
        {
            DrawPhysics(mover, prim, color);
            prim.Line(mover.Position, mover.Position + (mover.Radius * mover.Heading), color);
        }

        /// <summary>
        /// draw the current velocity
        /// </summary>
        /// <param name="prim"></param>
        /// <param name="color"></param>
        public virtual void DrawVelocity(IMover mover, IPrimitive prim, Color color)
        {
            prim.Line(mover.Position, mover.Position + mover.Velocity, color);
        }

        public virtual void DrawSpeedForce(IMover mover, IPrimitive prim, Color color)
        {
        }

        public virtual void DrawTotalForce(IMover mover, IPrimitive prim, Color color)
        {
        }

        public virtual void DrawWallFeelers(IMover mover, IPrimitive prim, Color color)
        {
        }

        /// <summary>
        /// Draw the detection circle and point out all the neighbors
        /// </summary>
        /// <param name="curTime"></param>
        public virtual void DrawNeigborQuery(IMover mover, IPrimitive prim, Color color)
        {
        }

        public virtual void DrawPursuitQuery(IMover mover, IPrimitive prim)
        {
        }
    }
}