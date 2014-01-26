using System.Collections.Generic;
using CollisionBuddy;

namespace FlockBuddy
{
	/// <summary>
	/// This is the interface for a steering beahvior that takes a list of walls
	/// </summary>
	public interface IWallsBehavior
	{
		void SetWalls(List<Line> walls);
	}
}