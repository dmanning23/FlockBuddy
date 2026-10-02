using Microsoft.Xna.Framework;
using PrimitiveBuddy;
using FlockBuddy.Interfaces;

namespace FlockBuddy.Drawers
{
    public interface IFlockDrawer
    {
        void Draw(IFlock flock, IPrimitive prim, Color color);
        void DrawCells(IFlock flock, IPrimitive prim);
        void DrawWhiskers(IFlock flock, IPrimitive prim, Color color);
    }
}