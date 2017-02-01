using Microsoft.Xna.Framework;
using Moq;
using NUnit.Framework;
using System;

namespace FlockBuddy.Tests
{
	[TestFixture]
	public class BaseBehaviorTests
	{
		[Test]
		public void SetDude()
		{
			var boid = new Mock<IBoid>();
			var behavior = new TestBehavior(boid.Object, EBehaviorType.wall_avoidance, 10f);

			Assert.AreEqual(boid.Object, behavior.Owner);
		}

		[Test]
		public void SetBehaviorType()
		{
			var boid = new Mock<IBoid>();
			var behavior = new TestBehavior(boid.Object, EBehaviorType.wall_avoidance, 10f);

			Assert.AreEqual(EBehaviorType.wall_avoidance, behavior.BehaviorType);
		}

		[Test]
		public void Setweight()
		{
			var boid = new Mock<IBoid>();
			var behavior = new TestBehavior(boid.Object, EBehaviorType.wall_avoidance, 10f);

			Assert.AreEqual(10f, behavior.Weight);
		}


		class TestBehavior : BaseBehavior
		{
			public TestBehavior(IBoid dude, EBehaviorType behaviorType, float weight) : base(dude, behaviorType, weight)
			{
			}

			public override Vector2 GetSteering()
			{
				throw new NotImplementedException();
			}
		}
	}
}
