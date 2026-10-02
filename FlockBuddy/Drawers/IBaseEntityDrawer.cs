using PrimitiveBuddy;
using Microsoft.Xna.Framework;
using FlockBuddy.Interfaces;

namespace FlockBuddy.Drawers
{
    public interface IBaseEntityDrawer
    {
        void DrawPhysics(IBaseEntity entity, IPrimitive prim, Color color);
    }
}