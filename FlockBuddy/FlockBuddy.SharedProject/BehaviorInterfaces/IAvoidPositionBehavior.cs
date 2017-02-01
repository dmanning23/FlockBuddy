using Microsoft.Xna.Framework;

namespace FlockBuddy
{
	/// <summary>
	/// This is a behavior where the boid tries to run away from a set position
	/// </summary>
	public interface IAvoidPositionBehavior
	{
		Vector2 AvoidPosition { set; }
	}
}
