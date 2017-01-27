using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace FlockBuddy
{
	/// <summary>
	/// This is a behavior where the boid tries to run away from a set position
	/// </summary>
	public interface IAvoidPositionBehavior
	{
		Vector2 AvoidPosition { get; }
	}
}
