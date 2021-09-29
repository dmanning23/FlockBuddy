
namespace FlockBuddy
{
	/// <summary>
	/// All teh different types of steering behaviors in the flock buddy!
	/// These are sorted by priority, so be careful about changing the order
	/// </summary>
	public enum BehaviorType
	{
		WallAvoidance,
		ObstacleAvoidance,
		Evade,
		Flee,
		Direction,
		Separation,
		Alignment,
		Cohesion,
		Seek,
		Arrive,
		Wander,
		Pursuit,
		OffsetPursuit,
		Interpose,
		GuardSeparation,
		GuardAlignment,
		GuardCohesion,
		Hide,
		FollowPath,
	};
}