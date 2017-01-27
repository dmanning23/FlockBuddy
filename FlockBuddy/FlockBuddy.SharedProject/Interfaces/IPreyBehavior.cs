using System;
using System.Collections.Generic;
using System.Text;

namespace FlockBuddy
{
	/// <summary>
	/// This is a behavior that has a guy who is trying to catch it
	/// </summary>
	public interface IPreyBehavior
	{
		IMover Pursuer
		{
			get;
		}
	}
}
