using System.Collections.Generic;

namespace FlockBuddy
{
	/// <summary>
	/// This is a beavior that uses obstacles somehow
	/// </summary>
	interface IObstacleBehavior
	{
		List<IBaseEntity> Obstacles { set; }
	}
}
