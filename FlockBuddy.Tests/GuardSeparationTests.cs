using FlockBuddy.SteeringBehaviors;
using Microsoft.Xna.Framework;
using NUnit.Framework;
using Shouldly;

namespace FlockBuddy.Tests
{
	[TestFixture]
	public class GuardSeparationTests
	{
		Flock _flock;

		[SetUp]
		public void Setup()
		{
			_flock = new Flock();
		}

		[Test]
		public void GetSteering_NoVip_ReturnsZero()
		{
			var owner = new TestBoid(_flock, Vector2.Zero, 1f, Vector2.UnitX, 0f,
				mass: 1f, minSpeed: 0f, walkSpeed: 0f, maxSpeed: 100f, maxTurnRate: 1f, maxForce: 1f);

			var guardSeparation = new GuardSeparation(owner) { Weight = 1f };

			guardSeparation.GetSteering().ShouldBe(Vector2.Zero);
		}

		[Test]
		public void GetSteering_WithVip_PushesAwayScaledByInverseDistance()
		{
			var owner = new TestBoid(_flock, Vector2.Zero, 1f, Vector2.UnitX, 0f,
				mass: 1f, minSpeed: 0f, walkSpeed: 0f, maxSpeed: 100f, maxTurnRate: 1f, maxForce: 1f);

			var vip = new TestMover(new Vector2(5f, 0f), 1f, Vector2.UnitX, 0f, 0f, 0f, 0f, 0f, 0f, 0f);

			var guardSeparation = new GuardSeparation(owner)
			{
				Weight = 1f,
				Vip = vip
			};

			var result = guardSeparation.GetSteering();

			//direction away from the VIP is (-1,0), scaled by 1/distance (5) again -> (-0.2, 0)
			result.X.ShouldBe(-0.2f, 0.0001f);
			result.Y.ShouldBe(0f, 0.0001f);
		}
	}
}
