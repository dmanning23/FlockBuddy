using FlockBuddy.SteeringBehaviors;
using Microsoft.Xna.Framework;
using NUnit.Framework;
using Shouldly;
using System.Collections.Generic;

namespace FlockBuddy.Tests
{
	[TestFixture]
	public class FollowPathTests
	{
		Flock _flock;

		[SetUp]
		public void Setup()
		{
			_flock = new Flock();
		}

		[Test]
		public void GetSteering_FarFromCurrentWaypoint_KeepsSeekingIt_DoesNotAdvance()
		{
			var owner = new TestBoid(_flock, Vector2.Zero, 1f, Vector2.UnitX, 0f,
				mass: 1f, minSpeed: 0f, walkSpeed: 0f, maxSpeed: 100f, maxTurnRate: 1f, maxForce: 1f)
			{
				WaypointQueryRadius = 10f
			};

			var followPath = new FollowPath(owner)
			{
				Weight = 1f,
				//waypoint 0 is straight up, waypoint 1 is off to the right - far enough apart that
				//seeking one instead of the other produces a clearly different force
				Path = new List<Vector2> { new Vector2(0f, 100f), new Vector2(100f, 0f) }
			};

			var result = followPath.GetSteering();

			//owner is 100 units from waypoint 0, well outside the 10-unit query radius, so it
			//should still be seeking waypoint 0, not have skipped ahead to waypoint 1
			result.ShouldBe(new Vector2(0f, 100f));
		}

		[Test]
		public void GetSteering_WithinQueryRadiusOfCurrentWaypoint_AdvancesToNextWaypoint()
		{
			var owner = new TestBoid(_flock, new Vector2(0f, 100f), 1f, Vector2.UnitX, 0f,
				mass: 1f, minSpeed: 0f, walkSpeed: 0f, maxSpeed: 100f, maxTurnRate: 1f, maxForce: 1f)
			{
				WaypointQueryRadius = 10f
			};

			var followPath = new FollowPath(owner)
			{
				Weight = 1f,
				//owner starts exactly on top of waypoint 0
				Path = new List<Vector2> { new Vector2(0f, 100f), new Vector2(100f, 100f) }
			};

			var result = followPath.GetSteering();

			//owner has reached waypoint 0 (distance 0, inside the query radius), so this call
			//should advance to waypoint 1 and seek towards it instead of standing still
			result.ShouldBe(new Vector2(100f, 0f));
		}
	}
}
