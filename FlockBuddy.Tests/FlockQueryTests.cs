using FlockBuddy.Interfaces;
using Microsoft.Xna.Framework;
using NUnit.Framework;
using Shouldly;
using System.Collections.Generic;

namespace FlockBuddy.Tests
{
	/// <summary>
	/// Flock's neighbor/predator/prey/VIP query methods - the brute-force (no CellSpace) path.
	/// Previously untested despite being the thing every steering behavior's Buddies/Prey/Pursuer/Vip
	/// data ultimately comes from.
	/// </summary>
	[TestFixture]
	public class FlockQueryTests
	{
		Flock _flock;

		[SetUp]
		public void Setup()
		{
			_flock = new Flock();
		}

		[Test]
		public void FindBoidsInRange_ExcludesSelf_AndOutOfRangeBoids()
		{
			var center = new TestBoid(_flock, Vector2.Zero, 1f, Vector2.UnitX, 0f);
			var near = new TestBoid(_flock, new Vector2(10f, 0f), 1f, Vector2.UnitX, 0f);
			var far = new TestBoid(_flock, new Vector2(1000f, 0f), 1f, Vector2.UnitX, 0f);

			var inRange = _flock.FindBoidsInRange(center, 50f);

			inRange.ShouldContain(near);
			inRange.ShouldNotContain(center);
			inRange.ShouldNotContain(far);
		}

		[Test]
		public void FindClosestBoidInRange_ReturnsNearestOfSeveral()
		{
			var center = new TestBoid(_flock, Vector2.Zero, 1f, Vector2.UnitX, 0f);
			var closer = new TestBoid(_flock, new Vector2(10f, 0f), 1f, Vector2.UnitX, 0f);
			var farther = new TestBoid(_flock, new Vector2(20f, 0f), 1f, Vector2.UnitX, 0f);

			var closest = _flock.FindClosestBoidInRange(center, 50f);

			closest.ShouldBe(closer);
			closest.ShouldNotBe(farther);
		}

		[Test]
		public void FindObstaclesInRange_RespectsRadius()
		{
			var boid = new TestBoid(_flock, Vector2.Zero, 1f, Vector2.UnitX, 0f);
			var nearObstacle = new BaseEntity(new Vector2(10f, 0f), 2f);
			var farObstacle = new BaseEntity(new Vector2(1000f, 0f), 2f);
			_flock.Obstacles = new List<IBaseEntity> { nearObstacle, farObstacle };

			var inRange = _flock.FindObstaclesInRange(boid, 50f);

			inRange.ShouldContain(nearObstacle);
			inRange.ShouldNotContain(farObstacle);
		}

		[Test]
		public void FindBoidAtPosition_ReturnsClosestToArbitraryPoint()
		{
			var closeBoid = new TestBoid(_flock, new Vector2(10f, 0f), 1f, Vector2.UnitX, 0f);
			var farBoid = new TestBoid(_flock, new Vector2(1000f, 0f), 1f, Vector2.UnitX, 0f);

			var found = _flock.FindBoidAtPosition(new Vector2(11f, 0f), 5f);

			found.ShouldBe(closeBoid);
			found.ShouldNotBe(farBoid);
		}

		[Test]
		public void FindBoidsAtPosition_RespectsBoidRadius_NotJustDistance()
		{
			//boid's own radius (20) is what matters here, not the query radius argument
			var wideBoid = new TestBoid(_flock, new Vector2(15f, 0f), 20f, Vector2.UnitX, 0f);

			var found = _flock.FindBoidsAtPosition(Vector2.Zero, 1f);

			found.ShouldContain(wideBoid);
		}

		[Test]
		public void FindBoidsAtPosition_AlsoRespectsTheQueryRadiusArgument()
		{
			//30 units away with a tiny radius of its own (1) - only findable if the query's own
			//radius argument (50) is actually added to the boid's radius, the same way
			//FindBoidsInRange/FindObstaclesInRange combine queryRadius + dude.Radius
			var farBoid = new TestBoid(_flock, new Vector2(30f, 0f), 1f, Vector2.UnitX, 0f);

			var found = _flock.FindBoidsAtPosition(Vector2.Zero, 50f);

			found.ShouldContain(farBoid);
		}

		[Test]
		public void PredatorPreyVip_AreOneDirectional_AndMutuallyExclusive()
		{
			var deer = new Flock();
			var wolves = new Flock();

			deer.AddFlockToGroup(wolves, FlockGroup.Predator);

			deer.IsFlockInGroup(wolves, FlockGroup.Predator).ShouldBeTrue();
			//the relationship is one-directional: wolves doesn't automatically consider deer prey
			wolves.IsFlockInGroup(deer, FlockGroup.Prey).ShouldBeFalse();

			//re-registering the same flock under a different group clears the old one
			deer.AddFlockToGroup(wolves, FlockGroup.Vip);
			deer.IsFlockInGroup(wolves, FlockGroup.Predator).ShouldBeFalse();
			deer.IsFlockInGroup(wolves, FlockGroup.Vip).ShouldBeTrue();

			deer.RemoveFlock(wolves);
			deer.IsFlockInGroup(wolves, FlockGroup.None).ShouldBeTrue();
		}

		[Test]
		public void FindClosestPredatorInRange_OnlyLooksAtRegisteredPredatorFlocks()
		{
			var preyFlock = _flock;
			var predatorFlock = new Flock();
			var bystanderFlock = new Flock();

			preyFlock.AddFlockToGroup(predatorFlock, FlockGroup.Predator);

			var preyBoid = new TestBoid(preyFlock, Vector2.Zero, 1f, Vector2.UnitX, 0f);
			var predatorBoid = new TestBoid(predatorFlock, new Vector2(10f, 0f), 1f, Vector2.UnitX, 0f);
			//in range, but its flock was never registered as a predator - must be ignored
			var bystanderBoid = new TestBoid(bystanderFlock, new Vector2(5f, 0f), 1f, Vector2.UnitX, 0f);

			var closestPredator = preyFlock.FindClosestPredatorInRange(preyBoid, 50f);

			closestPredator.ShouldBe(predatorBoid);
			closestPredator.ShouldNotBe(bystanderBoid);
		}

		[Test]
		public void FindClosestPreyInRange_OnlyLooksAtRegisteredPreyFlocks()
		{
			var wolfFlock = _flock;
			var deerFlock = new Flock();

			wolfFlock.AddFlockToGroup(deerFlock, FlockGroup.Prey);

			var wolfBoid = new TestBoid(wolfFlock, Vector2.Zero, 1f, Vector2.UnitX, 0f);
			var deerBoid = new TestBoid(deerFlock, new Vector2(10f, 0f), 1f, Vector2.UnitX, 0f);

			wolfFlock.FindClosestPreyInRange(wolfBoid, 50f).ShouldBe(deerBoid);
		}

		[Test]
		public void FindClosestVipInRange_OnlyLooksAtRegisteredVipFlocks()
		{
			var guardFlock = _flock;
			var vipFlock = new Flock();

			guardFlock.AddFlockToGroup(vipFlock, FlockGroup.Vip);

			var guardBoid = new TestBoid(guardFlock, Vector2.Zero, 1f, Vector2.UnitX, 0f);
			var vipBoid = new TestBoid(vipFlock, new Vector2(10f, 0f), 1f, Vector2.UnitX, 0f);

			guardFlock.FindClosestVipInRange(guardBoid, 50f).ShouldBe(vipBoid);
		}
	}
}
