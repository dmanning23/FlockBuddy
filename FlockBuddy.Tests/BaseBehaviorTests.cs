using FlockBuddy.Interfaces;
using Microsoft.Xna.Framework;
using Moq;
using NUnit.Framework;
using Shouldly;
using System;
using System.Linq;

namespace FlockBuddy.Tests
{
	[TestFixture]
	public class BaseBehaviorTests
	{
		[Test]
		public void SetDude()
		{
			var boid = new Mock<IBoid>();
			var behavior = new TestBehavior(boid.Object, BehaviorType.WallAvoidance, 10f);

			Assert.AreEqual(boid.Object, behavior.Owner);
		}

		[Test]
		public void SetBehaviorType()
		{
			var boid = new Mock<IBoid>();
			var behavior = new TestBehavior(boid.Object, BehaviorType.WallAvoidance, 10f);

			Assert.AreEqual(BehaviorType.WallAvoidance, behavior.BehaviorType);
		}

		[Test]
		public void Setweight()
		{
			var boid = new Mock<IBoid>();
			var behavior = new TestBehavior(boid.Object, BehaviorType.WallAvoidance, 10f);

			Assert.AreEqual(10f, behavior.Weight);
		}

		[Test]
		public void FactoryTest()
		{
			var flock = new Flock();
			var boid = new TestBoid(flock, Vector2.Zero, 1f, Vector2.UnitX, -1f);
			foreach (var behaviorType in Enum.GetValues(typeof(BehaviorType)).OfType<BehaviorType>())
			{
				var behavior = BaseBehavior.BehaviorFactory(behaviorType, boid);
				if (null == behavior)
				{
					throw new NotImplementedException($"{behaviorType.ToString()}");
				}
			}
		}
	}
}
