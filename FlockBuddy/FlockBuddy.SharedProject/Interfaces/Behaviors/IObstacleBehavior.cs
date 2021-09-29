using System.Collections.Generic;

namespace FlockBuddy.Interfaces.Behaviors
{
	/// <summary>
	/// This is a beavior that uses obstacles somehow
	/// </summary>
	interface IObstacleBehavior
	{
		List<IBaseEntity> Obstacles { set; }
	}
}
