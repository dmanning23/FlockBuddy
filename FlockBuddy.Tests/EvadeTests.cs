using FlockBuddy.SteeringBehaviors;
using Microsoft.Xna.Framework;
using NUnit.Framework;
using Shouldly;

namespace FlockBuddy.Tests
{
	[TestFixture]
	public class EvadeTests
	{
		Flock _flock;

		[SetUp]
		public void Setup()
		{
			_flock = new Flock();
		}

		[Test]
		public void GetSteering_NoPursuer_ReturnsZero()
		{
			var owner = new TestBoid(_flock, Vector2.Zero, 1f, Vector2.UnitX, 0f,
				mass: 1f, minSpeed: 0f, walkSpeed: 0f, maxSpeed: 100f, maxTurnRate: 1f, maxForce: 1f);

			var evade = new Evade(owner) { Weight = 1f };

			evade.GetSteering().ShouldBe(Vector2.Zero);
		}

		/// <summary>
		/// Mirrors PursuitTests.GetSteering_PreyNotAheadAndMoving_SeeksPredictedPosition: Evade
		/// predicts the pursuer's future position the same way Pursuit predicts prey, then flees
		/// from that predicted point instead of the pursuer's current position.
		/// </summary>
		[Test]
		public void GetSteering_PursuerMoving_FleesPredictedPosition()
		{
			var owner = new TestBoid(_flock, Vector2.Zero, 1f, Vector2.UnitX, 0f,
				mass: 1f, minSpeed: 0f, walkSpeed: 0f, maxSpeed: 100f, maxTurnRate: 1f, maxForce: 1f)
			{
				PredatorsQueryRadius = 100f
			};

			var pursuer = new TestMover(new Vector2(0f, 40f), 1f, Vector2.UnitX, 300f, 0f, 0f, 0f, 0f, 0f, 0f);

			var evade = new Evade(owner)
			{
				Weight = 2f,
				Pursuer = pursuer
			};

			var result = evade.GetSteering();

			//predicted pursuer position is (30,40) (same lookahead math as Pursuit's mirror test);
			//fleeing away from it gives unit vector (-0.6,-0.8)*MaxSpeed(100) = (-60,-80);
			//Evade's own Weight (2) on top gives (-120,-160)
			result.X.ShouldBe(-120f, 0.001f);
			result.Y.ShouldBe(-160f, 0.001f);
		}
	}
}
