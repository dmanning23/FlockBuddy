using FlockBuddy.SteeringBehaviors;
using Microsoft.Xna.Framework;
using NUnit.Framework;
using Shouldly;

namespace FlockBuddy.Tests
{
	[TestFixture]
	public class GuardAlignmentTests
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

			var guardAlignment = new GuardAlignment(owner) { Weight = 1f };

			guardAlignment.GetSteering().ShouldBe(Vector2.Zero);
		}

		[Test]
		public void GetSteering_WithVip_SteersTowardVipHeading()
		{
			var owner = new TestBoid(_flock, Vector2.Zero, 1f, Vector2.UnitX, 0f,
				mass: 1f, minSpeed: 0f, walkSpeed: 0f, maxSpeed: 100f, maxTurnRate: 1f, maxForce: 1f);

			var vip = new TestMover(Vector2.Zero, 1f, Vector2.UnitY, 0f, 0f, 0f, 0f, 0f, 0f, 0f);

			var guardAlignment = new GuardAlignment(owner)
			{
				Weight = 2f,
				Vip = vip
			};

			var result = guardAlignment.GetSteering();

			//VIP's heading (0,1) minus the owner's own heading (1,0) is (-1,1); times Weight (2)
			//gives (-2,2)
			result.ShouldBe(new Vector2(-2f, 2f));
		}
	}
}
