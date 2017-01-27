using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace FlockBuddy
{
	/// <summary>
	/// This is a behavior that includes a set of points
	/// </summary>
	public interface IPathBehavior
	{
		List<Vector2> Path { get; }
	}
}
