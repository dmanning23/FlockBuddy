using System.Collections.Generic;

namespace FlockBuddy.Interfaces.Behaviors
{
	/// <summary>
	/// this is a behavior that takes a group of buddies that are going to flock together
	/// </summary>
	public interface IFlockingBehavior
	{
		List<IMover> Buddies { set; }
	}
}
