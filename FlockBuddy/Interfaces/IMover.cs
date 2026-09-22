using Microsoft.Xna.Framework;
using PrimitiveBuddy;

namespace FlockBuddy.Interfaces
{
    /// <summary>
    /// An interface defining an entity that moves.
    /// </summary>
    public interface IMover : IBaseEntity
    {
        Vector2 Heading { get; }

        float Speed { get; }

        Vector2 Velocity { get; }

        void Draw(IPrimitive prim, Color color);

        void DrawVelocity(IPrimitive prim, Color color);

        void DrawSpeedForce(IPrimitive prim, Color color);

        void DrawTotalForce(IPrimitive prim, Color color);

        void DrawWallFeelers(IPrimitive prim, Color color);

        void DrawNeigborQuery(IPrimitive prim, Color color);

        void DrawPursuitQuery(IPrimitive prim);
    }
}