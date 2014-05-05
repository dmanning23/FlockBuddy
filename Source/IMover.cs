using BasicPrimitiveBuddy;
using Microsoft.Xna.Framework;

namespace FlockBuddy
{
	/// <summary>
	/// An interface defining an entity that moves.
	/// </summary>
	public interface IMover : IBaseEntity
	{
		Vector2 Heading { get; }

		float Speed { get; }

		Vector2 Velocity { get; }

		void Render(IBasicPrimitive prim, Color color);
	}
}