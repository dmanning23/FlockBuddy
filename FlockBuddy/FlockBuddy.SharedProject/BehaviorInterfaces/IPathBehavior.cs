using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace FlockBuddy
{
	/// <summary>
	/// This is a behavior that includes a set of points
	/// </summary>
	public interface IPathBehavior
	{
		List<Vector2> Path { set; }
	}
}
