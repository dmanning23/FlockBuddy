using System.Collections.Generic;

namespace FlockBuddy
{
	/// <summary>
	/// This is the interface for a steering beahvior that takes a whole group boids
	/// </summary>
	public interface IGroupBehavior
	{
		void SetGroup(List<Boid> group);
	}
}