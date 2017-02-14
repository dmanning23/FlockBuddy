using Microsoft.Xna.Framework;

namespace FlockBuddy
{
	public interface IBehavior
	{
		EBehaviorType BehaviorType { get; }
		IBoid Owner { get; set; }
		float Weight { get; set; }

		Vector2 GetSteering();

		float DirectionChange { get; }

		float SpeedChange { get; }
	}
}