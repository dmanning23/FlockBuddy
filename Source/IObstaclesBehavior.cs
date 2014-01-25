using System.Collections.Generic;

namespace FlockBuddy
{
	/// <summary>
	/// This is the interface for a steering beahvior that takes a list of obstacles
	/// </summary>
	public interface IObstaclesBehavior
	{
		void SetObstacles(List<BaseEntity> obstacles);
	}
}