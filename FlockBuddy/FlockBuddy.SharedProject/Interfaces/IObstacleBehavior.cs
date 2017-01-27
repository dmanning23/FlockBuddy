using System;
using System.Collections.Generic;
using System.Text;

namespace FlockBuddy
{
	/// <summary>
	/// This is a beavior that uses obstacles somehow
	/// </summary>
	interface IObstacleBehavior
	{
		List<IBaseEntity> Obstacles
		{
			get;
		}
	}
}
