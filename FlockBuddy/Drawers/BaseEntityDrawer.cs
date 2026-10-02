using FlockBuddy.Interfaces;
using Microsoft.Xna.Framework;
using PrimitiveBuddy;

namespace FlockBuddy.Drawers
{
    public class BaseEntityDrawer : IBaseEntityDrawer
    {
        public BaseEntityDrawer()
        {
        }

        /// <summary>
        /// Draw the physics info for this entity
        /// </summary>
        /// <param name="prim">basic ptimitive to draw this dude</param>
        /// <param name="color">teh color to draw him</param>
        public virtual void DrawPhysics(IBaseEntity entity, IPrimitive prim, Color color)
        {
            prim.Circle(entity.Position, entity.Radius, color);
        }
    }
}