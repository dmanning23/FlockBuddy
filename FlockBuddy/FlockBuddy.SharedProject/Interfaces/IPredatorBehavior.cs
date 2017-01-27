using System;
using System.Collections.Generic;
using System.Text;

namespace FlockBuddy
{
	/// <summary>
	/// A behavior where the boid tries to get another one
	/// </summary>
	public interface IPredatorBehavior
	{
		IMover Prey
		{
			get;
		}
	}
}
