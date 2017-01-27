using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace FlockBuddy
{
	/// <summary>
	/// this is a behavior that has a point the boid moves toward
	/// </summary>
	public interface ITargetPositionBehavior
	{
		Vector2 TargetPosition
		{
			get;
		}
	}
}
