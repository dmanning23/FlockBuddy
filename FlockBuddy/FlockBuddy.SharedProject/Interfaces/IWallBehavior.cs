using CollisionBuddy;
using System;
using System.Collections.Generic;
using System.Text;

namespace FlockBuddy
{
	/// <summary>
	/// This is a behavior that uses the walls.
	/// </summary>
	public interface IWallBehavior
	{
		List<ILine> Walls
		{
			get;
		}
	}
}
