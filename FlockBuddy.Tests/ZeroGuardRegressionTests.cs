using FlockBuddy.SteeringBehaviors;
using Microsoft.Xna.Framework;
using NUnit.Framework;
using Shouldly;

namespace FlockBuddy.Tests
{
	/// <summary>
	/// Regression tests for the zero-length-vector / divide-by-zero guards added to a handful of
	/// steering behaviors that previously produced NaN in degenerate (but reachable) states.
	/// </summary>
	[TestFixture]
	public class ZeroGuardRegressionTests
	{
		Flock _flock;

		[SetUp]
		public void Setup()
		{
			_flock = new Flock();
		}

		[Test]
		public void Flee_AvoidPositionEqualsOwnerPosition_DoesNotProduceNaN()
		{
			var owner = new TestBoid(_flock, Vector2.Zero, 1f, Vector2.UnitX, 0f,
				mass: 1f, minSpeed: 0f, walkSpeed: 0f, maxSpeed: 100f, maxTurnRate: 1f, maxForce: 1f)
			{
				PredatorsQueryRadius = 100f
			};

			//AvoidPosition sits exactly on top of the owner - Position - AvoidPosition is the zero vector
			var flee = new Flee(owner);
			var result = flee.GetSteering(Vector2.Zero);

			float.IsNaN(result.X).ShouldBeFalse();
			float.IsNaN(result.Y).ShouldBeFalse();
		}

		[Test]
		public void ObstacleAvoidance_ObstacleCenteredOnOwner_DoesNotProduceNaN()
		{
			var owner = new TestBoid(_flock, Vector2.Zero, 1f, Vector2.UnitX, 0f,
				mass: 1f, minSpeed: 0f, walkSpeed: 0f, maxSpeed: 100f, maxTurnRate: 1f, maxForce: 1f)
			{
				ObstacleQueryRadius = 100f
			};

			//an obstacle sitting exactly at the owner's own position - Position - Obstacle.Position is the zero vector
			var obstacle = new BaseEntity(Vector2.Zero, 5f);

			var obstacleAvoidance = new ObstacleAvoidance(owner)
			{
				Obstacles = new System.Collections.Generic.List<FlockBuddy.Interfaces.IBaseEntity> { obstacle }
			};

			var result = obstacleAvoidance.GetSteering();

			float.IsNaN(result.X).ShouldBeFalse();
			float.IsNaN(result.Y).ShouldBeFalse();
		}

		[Test]
		public void ObstacleAvoidance_ZeroQueryRadius_DoesNotProduceNaN()
		{
			var owner = new TestBoid(_flock, Vector2.Zero, 1f, Vector2.UnitX, 0f,
				mass: 1f, minSpeed: 0f, walkSpeed: 0f, maxSpeed: 100f, maxTurnRate: 1f, maxForce: 1f)
			{
				ObstacleQueryRadius = 0f
			};

			var obstacle = new BaseEntity(new Vector2(5f, 0f), 5f);

			var obstacleAvoidance = new ObstacleAvoidance(owner)
			{
				Obstacles = new System.Collections.Generic.List<FlockBuddy.Interfaces.IBaseEntity> { obstacle }
			};

			var result = obstacleAvoidance.GetSteering();

			float.IsNaN(result.X).ShouldBeFalse();
			float.IsNaN(result.Y).ShouldBeFalse();
		}

		[Test]
		public void Evade_BothSpeedsZero_DoesNotProduceNaN()
		{
			var owner = new TestBoid(_flock, Vector2.Zero, 1f, Vector2.UnitX, 0f,
				mass: 1f, minSpeed: 0f, walkSpeed: 0f, maxSpeed: 0f, maxTurnRate: 1f, maxForce: 1f)
			{
				PredatorsQueryRadius = 100f
			};
			var pursuer = new TestMover(new Vector2(10f, 0f), 1f, Vector2.UnitX, 0f, 0f, 0f, 0f, 0f, 0f, 0f);

			var evade = new Evade(owner) { Pursuer = pursuer };
			var result = evade.GetSteering();

			float.IsNaN(result.X).ShouldBeFalse();
			float.IsNaN(result.Y).ShouldBeFalse();
		}

		[Test]
		public void Pursuit_BothSpeedsZero_DoesNotProduceNaN()
		{
			var owner = new TestBoid(_flock, Vector2.Zero, 1f, Vector2.UnitX, 0f,
				mass: 1f, minSpeed: 0f, walkSpeed: 0f, maxSpeed: 0f, maxTurnRate: 1f, maxForce: 1f);
			//prey off to the side so the "ahead and facing" shortcut doesn't trigger and the
			//lookahead-time division actually runs
			var prey = new TestMover(new Vector2(0f, 10f), 1f, Vector2.UnitX, 0f, 0f, 0f, 0f, 0f, 0f, 0f);

			var pursuit = new Pursuit(owner) { Prey = prey };
			var result = pursuit.GetSteering();

			float.IsNaN(result.X).ShouldBeFalse();
			float.IsNaN(result.Y).ShouldBeFalse();
		}
	}
}
