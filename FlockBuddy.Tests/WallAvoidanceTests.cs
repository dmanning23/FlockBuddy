using CollisionBuddy;
using FlockBuddy.SteeringBehaviors;
using Microsoft.Xna.Framework;
using NUnit.Framework;
using Shouldly;
using System.Collections.Generic;

namespace FlockBuddy.Tests
{
	[TestFixture]
	public class WallAvoidanceTests
	{
		Flock _flock;

		[SetUp]
		public void Setup()
		{
			_flock = new Flock();
		}

		/// <summary>
		/// When the boid's front feeler crosses two walls, only the closer wall should contribute a
		/// steering force - not both summed together. The walls here are short segments straddling
		/// only the x-axis, so the diagonal (left/right) feelers don't intersect either of them and
		/// only the straight-ahead feeler is in play.
		/// </summary>
		[Test]
		public void GetSteering_TwoWallsOnOneFeeler_OnlyClosestWallContributes()
		{
			var owner = new TestBoid(_flock, Vector2.Zero, 1f, Vector2.UnitX, 0f,
				mass: 1f, minSpeed: 0f, walkSpeed: 0f, maxSpeed: 100f, maxTurnRate: 1f, maxForce: 1f)
			{
				WallQueryRadius = 100f
			};

			//nearer wall, straddling the x-axis at x=30
			var nearWall = new Line(new Vector2(30f, -2f), new Vector2(30f, 2f));
			//farther wall, straddling the x-axis at x=60
			var farWall = new Line(new Vector2(60f, -2f), new Vector2(60f, 2f));

			var wallAvoidance = new WallAvoidance(owner)
			{
				Weight = 1f,
				Walls = new List<ILine> { nearWall, farWall }
			};

			var result = wallAvoidance.GetSteering();

			//front feeler runs from (0,0) to (100,0); it hits nearWall at (30,0) first.
			//Only that intersection should count: overshoot = (100,0) - (30,0) = (70,0).
			var expected = nearWall.Normal * 70f;
			result.ShouldBe(expected);
		}
	}
}
