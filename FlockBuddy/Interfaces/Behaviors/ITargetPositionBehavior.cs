using Microsoft.Xna.Framework;

namespace FlockBuddy.Interfaces.Behaviors
{
	/// <summary>
	/// this is a behavior that has a point the boid moves toward
	/// </summary>
	public interface ITargetPositionBehavior
	{
		Vector2 TargetPosition { set; }
	}
}
