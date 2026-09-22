using FlockBuddy.SteeringBehaviors;
using Microsoft.Xna.Framework;
using NUnit.Framework;
using Shouldly;

namespace FlockBuddy.Tests
{
	[TestFixture]
	public class GuardCohesionTests
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

			var guardCohesion = new GuardCohesion(owner) { Weight = 1f };

			guardCohesion.GetSteering().ShouldBe(Vector2.Zero);
		}

		[Test]
		public void GetSteering_WithVip_SeeksNormalizedDirectionToVip()
		{
			var owner = new TestBoid(_flock, Vector2.Zero, 1f, Vector2.UnitX, 0f,
				mass: 1f, minSpeed: 0f, walkSpeed: 0f, maxSpeed: 100f, maxTurnRate: 1f, maxForce: 1f);

			var vip = new TestMover(new Vector2(10f, 0f), 1f, Vector2.UnitX, 0f, 0f, 0f, 0f, 0f, 0f, 0f);

			var guardCohesion = new GuardCohesion(owner)
			{
				Weight = 3f,
				Vip = vip
			};

			var result = guardCohesion.GetSteering();

			//seeking (10,0) from the origin at MaxSpeed=100 gives an un-normalized force of (100,0);
			//GuardCohesion normalizes that to (1,0) before applying its own Weight (3) -> (3,0)
			result.ShouldBe(new Vector2(3f, 0f));
		}
	}
}
