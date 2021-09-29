using FlockBuddy.Interfaces;
using Microsoft.Xna.Framework;
using System;

namespace FlockBuddy.Tests
{
	public class TestBehavior : BaseBehavior
	{
		public TestBehavior(IBoid dude, BehaviorType behaviorType, float weight) : base(dude, behaviorType, weight)
		{
		}

		public override float DirectionChange
		{
			get
			{
				throw new NotImplementedException();
			}
		}

		public override float SpeedChange
		{
			get
			{
				throw new NotImplementedException();
			}
		}

		public override Vector2 GetSteering()
		{
			throw new NotImplementedException();
		}
	}
}
