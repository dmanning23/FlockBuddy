using FlockBuddy.Interfaces;
using FlockBuddy.SteeringBehaviors;
using Microsoft.Xna.Framework;
using NUnit.Framework;
using Shouldly;
using System.Collections.Generic;

namespace FlockBuddy.Tests
{
	[TestFixture]
	public class AlignmentTests
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

			var alignment = new Alignment(owner) { Weight = 1f };

			alignment.GetSteering().ShouldBe(Vector2.Zero);
		}

		[Test]
		public void GetSteering_TwoBuddies_SteersTowardAverageHeading()
		{
			var owner = new TestBoid(_flock, Vector2.Zero, 1f, Vector2.UnitX, 0f,
				mass: 1f, minSpeed: 0f, walkSpeed: 0f, maxSpeed: 100f, maxTurnRate: 1f, maxForce: 1f);

			var buddy1 = new TestMover(Vector2.Zero, 1f, Vector2.UnitY, 0f, 0f, 0f, 0f, 0f, 0f, 0f);
			var buddy2 = new TestMover(Vector2.Zero, 1f, -Vector2.UnitY, 0f, 0f, 0f, 0f, 0f, 0f, 0f);

			var alignment = new Alignment(owner)
			{
				Weight = 2f,
				Buddies = new List<IMover> { buddy1, buddy2 }
			};

			var result = alignment.GetSteering();

			//average heading of (0,1) and (0,-1) is (0,0); minus the owner's own heading (1,0)
			//gives (-1,0); times Weight (2) gives (-2,0)
			result.ShouldBe(new Vector2(-2f, 0f));
		}
	}
}
