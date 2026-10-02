using Microsoft.Xna.Framework;
using PrimitiveBuddy;
using FlockBuddy.Interfaces;

namespace FlockBuddy.Drawers
{
    public interface IMoverDrawer : IBaseEntityDrawer
    {
        void Draw(IMover mover, IPrimitive prim, Color color);

        void DrawVelocity(IMover mover, IPrimitive prim, Color color);

        void DrawSpeedForce(IMover mover, IPrimitive prim, Color color);

        void DrawTotalForce(IMover mover, IPrimitive prim, Color color);

        void DrawWallFeelers(IMover mover, IPrimitive prim, Color color);

        void DrawNeigborQuery(IMover mover, IPrimitive prim, Color color);

        void DrawPursuitQuery(IMover mover, IPrimitive prim);
    }
}