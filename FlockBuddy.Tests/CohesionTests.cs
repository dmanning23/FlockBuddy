using FlockBuddy.Interfaces;
using FlockBuddy.SteeringBehaviors;
using Microsoft.Xna.Framework;
using NUnit.Framework;
using Shouldly;
using System.Collections.Generic;

namespace FlockBuddy.Tests
{
	[TestFixture]
	public class CohesionTests
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

			var cohesion = new Cohesion(owner) { Weight = 1f };

			cohesion.GetSteering().ShouldBe(Vector2.Zero);
		}

		[Test]
		public void GetSteering_TwoBuddies_SeeksNormalizedDirectionToCenterOfMass()
		{
			var owner = new TestBoid(_flock, Vector2.Zero, 1f, Vector2.UnitX, 0f,
				mass: 1f, minSpeed: 0f, walkSpeed: 0f, maxSpeed: 100f, maxTurnRate: 1f, maxForce: 1f);

			//center of mass of these two buddies is (10, 0)
			var buddy1 = new TestMover(new Vector2(20f, 0f), 1f, Vector2.UnitX, 0f, 0f, 0f, 0f, 0f, 0f, 0f);
			var buddy2 = new TestMover(Vector2.Zero, 1f, Vector2.UnitX, 0f, 0f, 0f, 0f, 0f, 0f, 0f);

			var cohesion = new Cohesion(owner)
			{
				Weight = 3f,
				Buddies = new List<IMover> { buddy1, buddy2 }
			};

			var result = cohesion.GetSteering();

			//seeking (10,0) from the origin at MaxSpeed=100 gives an un-normalized force of (100,0);
			//cohesion normalizes that to (1,0) before applying its own Weight (3) -> (3,0)
			result.ShouldBe(new Vector2(3f, 0f));
		}
	}
}
