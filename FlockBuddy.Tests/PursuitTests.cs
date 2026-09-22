using FlockBuddy.SteeringBehaviors;
using Microsoft.Xna.Framework;
using NUnit.Framework;
using Shouldly;

namespace FlockBuddy.Tests
{
	[TestFixture]
	public class PursuitTests
	{
		Flock _flock;

		[SetUp]
		public void Setup()
		{
			_flock = new Flock();
		}

		/// <summary>
		/// When the prey is ahead of and facing the pursuer (the "no lookahead needed" shortcut),
		/// GetSteering should still scale by the behavior's own Weight, the same way the
		/// lookahead-prediction branch below it does.
		/// </summary>
		[Test]
		public void GetSteering_PreyAheadAndFacing_AppliesWeight()
		{
			var owner = new TestBoid(_flock, Vector2.Zero, 1f, Vector2.UnitX, 0f,
				mass: 1f, minSpeed: 0f, walkSpeed: 0f, maxSpeed: 100f, maxTurnRate: 1f, maxForce: 1f);

			//prey is straight ahead of the owner, and facing back towards it (well within the ~18 degree cone)
			var prey = new TestMover(new Vector2(10f, 0f), 1f, new Vector2(-1f, 0f), 0f, 0f, 0f, 0f, 0f, 0f, 0f);

			var pursuit = new Pursuit(owner)
			{
				Weight = 5f,
				Prey = prey
			};

			var result = pursuit.GetSteering();

			//Seek towards the prey's current position (10,0) at MaxSpeed (100,0), owner velocity is
			//zero, so the un-weighted Seek force is (100,0). Pursuit should apply its own Weight (5) on top.
			result.ShouldBe(new Vector2(500f, 0f));
		}

		/// <summary>
		/// When the prey isn't ahead of the pursuer, GetSteering should predict the prey's future
		/// position (based on its current velocity and a lookahead time) and seek that instead of
		/// the prey's current position.
		/// </summary>
		[Test]
		public void GetSteering_PreyNotAheadAndMoving_SeeksPredictedPosition()
		{
			var owner = new TestBoid(_flock, Vector2.Zero, 1f, Vector2.UnitX, 0f,
				mass: 1f, minSpeed: 0f, walkSpeed: 0f, maxSpeed: 100f, maxTurnRate: 1f, maxForce: 1f);

			//prey is directly to the side (not ahead: dot(toPrey, ownerHeading) == 0), moving along
			//the owner's heading at speed 300
			var prey = new TestMover(new Vector2(0f, 40f), 1f, Vector2.UnitX, 300f, 0f, 0f, 0f, 0f, 0f, 0f);

			var pursuit = new Pursuit(owner)
			{
				Weight = 2f,
				Prey = prey
			};

			var result = pursuit.GetSteering();

			//lookAheadTime = |toPrey| / (MaxSpeed + prey.Speed) = 40 / (100+300) = 0.1
			//predicted position = (0,40) + (300,0)*0.1 = (30,40)
			//seeking (30,40) at MaxSpeed=100 (unit vector (0.6,0.8)*100) gives (60,80);
			//Pursuit's own Weight (2) on top gives (120,160)
			result.X.ShouldBe(120f, 0.001f);
			result.Y.ShouldBe(160f, 0.001f);
		}
	}
}
