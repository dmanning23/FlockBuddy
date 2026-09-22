using FlockBuddy.SteeringBehaviors;
using Microsoft.Xna.Framework;
using NUnit.Framework;
using Shouldly;

namespace FlockBuddy.Tests
{
	[TestFixture]
	public class InterposeTests
	{
		Flock _flock;

		[SetUp]
		public void Setup()
		{
			_flock = new Flock();
		}

		[Test]
		public void GetSteering_SeeksMidpointOfPursuerAndVip_NotJustPursuer()
		{
			var owner = new TestBoid(_flock, Vector2.Zero, 1f, Vector2.UnitX, 0f,
				mass: 1f, minSpeed: 0f, walkSpeed: 0f, maxSpeed: 100f, maxTurnRate: 1f, maxForce: 1f);

			//both stationary, so the lookahead time doesn't affect their predicted positions
			var pursuer = new TestMover(new Vector2(6f, 0f), 1f, Vector2.UnitX, 0f, 0f, 0f, 0f, 0f, 0f, 0f);
			var vip = new TestMover(new Vector2(0f, 8f), 1f, Vector2.UnitX, 0f, 0f, 0f, 0f, 0f, 0f, 0f);

			var interpose = new Interpose(owner)
			{
				Weight = 2f,
				Pursuer = pursuer,
				Vip = vip
			};

			var result = interpose.GetSteering();

			//the true midpoint of pursuer (6,0) and vip (0,8) is (3,4), a unit vector of (0.6, 0.8);
			//seeking that at MaxSpeed=100 with Weight=2 gives (120,160). A version that (bugfully)
			//seeks only the pursuer's own predicted position would instead produce (200, 0).
			result.X.ShouldBe(120f, 0.001f);
			result.Y.ShouldBe(160f, 0.001f);
		}
	}
}
