
namespace FlockBuddy
{
	/// <summary>
	/// All teh different types of steering behaviors in the flock buddy!
	/// These are sorted by priority, so be careful about changing the order
	/// </summary>
	public enum EBehaviorType
	{
		wall_avoidance,
		obstacle_avoidance,
		evade,
		flee,
		direction,
		separation,
		alignment,
		cohesion,
		seek,
		arrive,
		wander,
		pursuit,
		offset_pursuit,
		interpose,
		guard_separation,
		guard_alignment,
		guard_cohesion,
		hide,
		follow_path,
	};
}