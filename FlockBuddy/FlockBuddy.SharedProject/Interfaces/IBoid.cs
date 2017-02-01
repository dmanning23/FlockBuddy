using System;
using System.Collections.Generic;
using System.Text;

namespace FlockBuddy
{
	public interface IBoid : IMover
	{
		ESummingMethod SumMethod { set; }

		float QueryRadius { get; set; }

		float RetargetTime { set; }

		void AddBehavior(IBehavior behavior);
	}
}
