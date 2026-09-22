using FlockBuddy.Interfaces;
using FlockBuddy.SteeringBehaviors;
using Microsoft.Xna.Framework;
using NUnit.Framework;
using Shouldly;
using System.Collections.Generic;

namespace FlockBuddy.Tests
{
	[TestFixture]
	public class ObstacleAvoidanceTests
	{
		Flock _flock;
		TestBoid _owner;

		[SetUp]
		public void Setup()
		{
			_flock = new Flock();
			//Heading is UnitX, so local space lines up with world space, keeping the numbers simple
			_owner = new TestBoid(_flock, Vector2.Zero, 10f, Vector2.UnitX, 0f,
				mass: 1f, minSpeed: 0f, walkSpeed: 0f, maxSpeed: 100f, maxTurnRate: 1f, maxForce: 1f)
			{
				ObstacleQueryRadius = 100f
			};
		}

		[Test]
		public void GetSteering_NoObstacles_ReturnsZero()
		{
			var obstacleAvoidance = new ObstacleAvoidance(_owner) { Weight = 1f };
			obstacleAvoidance.GetSteering().ShouldBe(Vector2.Zero);
		}

		[Test]
		public void GetSteering_ObstacleBehindOwner_IsIgnored()
		{
			var obstacleAvoidance = new ObstacleAvoidance(_owner)
			{
				Weight = 1f,
				Obstacles = new List<IBaseEntity> { new BaseEntity(new Vector2(-50f, 0f), 5f) }
			};

			obstacleAvoidance.GetSteering().ShouldBe(Vector2.Zero);
		}

		[Test]
		public void GetSteering_ObstacleAhead_StreersAwayFromIt()
		{
			var obstacle = new BaseEntity(new Vector2(50f, 0f), 5f);

			var obstacleAvoidance = new ObstacleAvoidance(_owner)
			{
				Weight = 1f,
				Obstacles = new List<IBaseEntity> { obstacle }
			};

			var result = obstacleAvoidance.GetSteering();

			//expandedRadius = obstacle.Radius(5) + owner.Radius(10) + 1 = 16; obstacle is dead ahead
			//(local Y=0), so the intersection point is cX - sqrtPart = 50 - 16 = 34.
			//toAgent = normalize(owner.Position - obstacle.Position) = (-1,0).
			//dist = (toAgent*obstacle.Radius) - localPos = (-5,0) - (50,0) = (-55,0).
			//multiplier = 1 + (ObstacleQueryRadius(100) - dist.X(-55)) / 100 = 2.55
			//steeringForce = (-1,0) * 2.55 = (-2.55, 0)
			result.X.ShouldBe(-2.55f, 0.0001f);
			result.Y.ShouldBe(0f, 0.0001f);
		}

		[Test]
		public void GetSteering_TwoObstaclesAhead_OnlyClosestOneCounts()
		{
			var farObstacle = new BaseEntity(new Vector2(50f, 0f), 5f);
			var nearObstacle = new BaseEntity(new Vector2(30f, 0f), 5f);

			var obstacleAvoidance = new ObstacleAvoidance(_owner)
			{
				Weight = 1f,
				//farObstacle listed first, to prove selection isn't just "first in the list"
				Obstacles = new List<IBaseEntity> { farObstacle, nearObstacle }
			};

			var result = obstacleAvoidance.GetSteering();

			//nearObstacle's intersection point (30 - 16 = 14) is closer than farObstacle's (50 - 16 = 34),
			//so only nearObstacle should drive the steering force:
			//toAgent = normalize((0,0)-(30,0)) = (-1,0); dist = (-5,0)-(30,0) = (-35,0);
			//multiplier = 1 + (100-(-35))/100 = 2.35; steeringForce = (-1,0)*2.35 = (-2.35,0)
			result.X.ShouldBe(-2.35f, 0.0001f);
			result.Y.ShouldBe(0f, 0.0001f);
		}
	}
}
