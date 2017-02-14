using System;
using System.Collections.Generic;
using System.Text;

namespace FlockBuddy
{
	public interface IBoid : IMover
	{
		ESummingMethod SummingMethod { set; }

		float QueryRadius { get; set; }

		float RetargetTime { set; }

		void AddBehavior(EBehaviorType behaviorType, float weight);

		void AddBehavior(IBehavior behavior);

		void RemoveBehavior(EBehaviorType behaviorType);
	}
}
