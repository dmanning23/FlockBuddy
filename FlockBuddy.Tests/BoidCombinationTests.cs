using FlockBuddy.Interfaces;
using FlockBuddy.SteeringBehaviors;
using Microsoft.Xna.Framework;
using NUnit.Framework;
using Shouldly;
using System.Collections.Generic;

namespace FlockBuddy.Tests
{
	/// <summary>
	/// Covers Boid's own force-combination logic (GetForces/CalculateWeightedSum/CalculatePrioritized) -
	/// previously untested at every level, despite being the code that turns several active behaviors
	/// into the one steering force actually applied each frame.
	/// </summary>
	[TestFixture]
	public class BoidCombinationTests
	{
		Flock _flock;
		TestBoid _boid;

		[SetUp]
		public void Setup()
		{
			_flock = new Flock();
			_boid = new TestBoid(_flock, Vector2.Zero, 1f, Vector2.UnitX, 0f,
				mass: 1f, minSpeed: 0f, walkSpeed: 0f, maxSpeed: 100f, maxTurnRate: 1f, maxForce: 100f);

			//push the retarget timer's next tick far into the future so GetForces() doesn't
			//overwrite the Buddies/Vip/etc. we set directly on the behaviors below with whatever
			//the (empty, in these tests) real Flock would find
			_boid.RetargetTimer.Start(1000000f);
		}

		[Test]
		public void WeightedAverage_CombinesBehaviors_WeightedByDirectionAndSpeedChange()
		{
			//WeightedAverage is the default SummingMethod - no need to set it explicitly

			var direction = (Direction)_boid.AddBehavior(BehaviorType.Direction, 2f);
			direction.SteeringDirection = Vector2.UnitX;

			//Alignment has SpeedChange = 0, unlike Direction's 1, so it should show up in
			//TotalForce/DirectionForce but be absent from SpeedForce
			var alignment = (Alignment)_boid.AddBehavior(BehaviorType.Alignment, 5f);
			var buddy = new TestMover(Vector2.Zero, 1f, Vector2.UnitY, 0f, 0f, 0f, 0f, 0f, 0f, 0f);
			alignment.Buddies = new List<IMover> { buddy };

			_boid.CallGetForces();

			//Direction: (1,0) * 2 = (2,0). Alignment: average heading (0,1) minus owner heading
			//(1,0) = (-1,1), * 5 = (-5,5). Sum = (-3,5); Mass is 1, so no further scaling.
			_boid.TotalForce.ShouldBe(new Vector2(-3f, 5f));
			//both behaviors have DirectionChange = 1, so DirectionForce matches TotalForce
			_boid.DirectionForce.ShouldBe(new Vector2(-3f, 5f));
			//only Direction's SpeedChange is 1 - Alignment's is 0, so only its (2,0) counts
			_boid.SpeedForce.ShouldBe(new Vector2(2f, 0f));
		}

		[Test]
		public void Prioritized_LowerPriorityBehavior_StillContributes_UntilBudgetExhausted()
		{
			_boid.SummingMethod = SummingMethod.Prioritized;

			//Direction (BehaviorType 4) outranks Seek (BehaviorType 8), so it's applied first
			var direction = (Direction)_boid.AddBehavior(BehaviorType.Direction, 30f);
			direction.SteeringDirection = Vector2.UnitX;

			var seek = (Seek)_boid.AddBehavior(BehaviorType.Seek, 1f);
			seek.TargetPosition = new Vector2(0f, 10f);

			_boid.CallGetForces();

			//Direction's force is (30,0), magnitude 30 - well under MaxForce (100), so it's
			//applied in full, leaving 70 of budget. Seek's force is (0,100) magnitude 100, which
			//exceeds the remaining 70, so it gets truncated to (0,70). A version that stops
			//accumulating as soon as the first behavior *doesn't* exhaust the budget (rather than
			//when it does) would produce (30,0) instead, silently dropping Seek entirely.
			_boid.TotalForce.ShouldBe(new Vector2(30f, 70f));
		}

		[Test]
		public void Prioritized_HigherPriorityBehaviorAlone_ExhaustsBudget_LowerPriorityContributesNothing()
		{
			_boid.SummingMethod = SummingMethod.Prioritized;

			//Direction's force (150,0) alone exceeds MaxForce (100), so it should consume the
			//entire budget and Seek should never get a chance to contribute anything
			var direction = (Direction)_boid.AddBehavior(BehaviorType.Direction, 150f);
			direction.SteeringDirection = Vector2.UnitX;

			var seek = (Seek)_boid.AddBehavior(BehaviorType.Seek, 1f);
			seek.TargetPosition = new Vector2(0f, 10f);

			_boid.CallGetForces();

			_boid.TotalForce.ShouldBe(new Vector2(100f, 0f));
		}
	}
}
