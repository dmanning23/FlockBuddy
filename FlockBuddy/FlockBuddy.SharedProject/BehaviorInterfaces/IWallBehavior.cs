using CollisionBuddy;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace FlockBuddy
{
	/// <summary>
	/// This is a behavior that uses the walls.
	/// </summary>
	public interface IWallBehavior
	{
		List<ILine> Walls { set; }

		List<Vector2> Feelers { get; }
	}
}