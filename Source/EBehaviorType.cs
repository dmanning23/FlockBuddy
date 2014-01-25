using System;

namespace FlockBuddy
{
	[Flags]
	public enum EBehaviorType
	{
		none,
		seek,
		flee,
		arrive,
		wander,
		cohesion,
		separation,
		allignment,
		obstacle_avoidance,
		wall_avoidance,
		follow_path,
		pursuit,
		evade,
		interpose,
		hide,
		flock,
		offset_pursuit
	};
}