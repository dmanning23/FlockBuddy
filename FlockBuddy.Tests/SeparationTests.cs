using FlockBuddy.Interfaces;
using FlockBuddy.SteeringBehaviors;
using Microsoft.Xna.Framework;
using NUnit.Framework;
using Shouldly;
using System.Collections.Generic;

namespace FlockBuddy.Tests
{
	[TestFixture]
	public class SeparationTests
	{
		Flock _flock;

		[SetUp]
		public void Setup()
		{
			_flock = new Flock();
		}

		[Test]
		public void GetSteering_NoBuddies_ReturnsZero()
		{
			var owner = new TestBoid(_flock, Vector2.Zero, 1f, Vector2.UnitX, 0f,
				mass: 1f, minSpeed: 0f, walkSpeed: 0f, maxSpeed: 100f, maxTurnRate: 1f, maxForce: 1f);

			var separation = new Separation(owner) { Weight = 1f };

			separation.GetSteering().ShouldBe(Vector2.Zero);
		}

		[Test]
		public void GetSteering_TwoBuddies_PushesAwayFromEachScaledByInverseDistance()
		{
			var owner = new TestBoid(_flock, Vector2.Zero, 1f, Vector2.UnitX, 0f,
				mass: 1f, minSpeed: 0f, walkSpeed: 0f, maxSpeed: 100f, maxTurnRate: 1f, maxForce: 1f);

			//buddy 1 is 5 units to the right, buddy 2 is 3 units below
			var buddy1 = new TestMover(new Vector2(5f, 0f), 1f, Vector2.UnitX, 0f, 0f, 0f, 0f, 0f, 0f, 0f);
			var buddy2 = new TestMover(new Vector2(0f, 3f), 1f, Vector2.UnitX, 0f, 0f, 0f, 0f, 0f, 0f, 0f);

			var separation = new Separation(owner)
			{
				Weight = 1f,
				Buddies = new List<IMover> { buddy1, buddy2 }
			};

			var result = separation.GetSteering();

			//force from buddy1: direction away = (-1,0), scaled by 1/distance (5) again -> (-1/5, 0)
			//force from buddy2: direction away = (0,-1), scaled by 1/distance (3) again -> (0, -1/3)
			result.X.ShouldBe(-0.2f, 0.0001f);
			result.Y.ShouldBe(-1f / 3f, 0.0001f);
		}
	}
}
