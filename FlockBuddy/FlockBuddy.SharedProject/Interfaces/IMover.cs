using Microsoft.Xna.Framework;
using PrimitiveBuddy;

namespace FlockBuddy
{
	/// <summary>
	/// An interface defining an entity that moves.
	/// </summary>
	public interface IMover : IBaseEntity
	{
		Vector2 Heading { get; }

		Vector2 Side { get; }

		float Speed { get; }

		Vector2 Velocity { get; }

		float Mass { get; set; }

		float MaxSpeed { get; set; }

		float MaxForce { get; set; }

		float MaxTurnRate { get; set; }
	}
}