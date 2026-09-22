using CellSpacePartitionLib;
using GameTimer;
using Microsoft.Xna.Framework;
using NUnit.Framework;
using Shouldly;

namespace FlockBuddy.Tests
{
	/// <summary>
	/// Flock's CellSpace-backed query path - previously untested, unlike the brute-force path
	/// exercised everywhere else. Confirms the cell-space route returns the same kind of results as
	/// the brute-force route, and that assigning it and updating the flock doesn't throw.
	/// </summary>
	[TestFixture]
	public class CellSpacePartitionTests
	{
		Flock _flock;

		[SetUp]
		public void Setup()
		{
			_flock = new Flock
			{
				//assign CellSpace before adding any boids, per docs/advanced-topics.md
				CellSpace = new CellSpacePartition<Interfaces.IMover>(new Vector2(1000f, 1000f), 10, 10, 50)
			};
		}

		[Test]
		public void UseCellSpace_IsTrueOnceCellSpaceIsAssigned()
		{
			_flock.UseCellSpace.ShouldBeTrue();
		}

		[Test]
		public void AddBoid_AutomaticallyRegistersItInTheCellSpace()
		{
			var center = new TestBoid(_flock, Vector2.Zero, 1f, Vector2.UnitX, 0f);
			var near = new TestBoid(_flock, new Vector2(10f, 0f), 1f, Vector2.UnitX, 0f);
			var far = new TestBoid(_flock, new Vector2(900f, 900f), 1f, Vector2.UnitX, 0f);

			var inRange = _flock.FindBoidsInRange(center, 50f);

			inRange.ShouldContain(near);
			inRange.ShouldNotContain(far);
		}

		[Test]
		public void FindBoidAtPosition_UsesCellSpaceNearestNeighbor()
		{
			var closeBoid = new TestBoid(_flock, new Vector2(10f, 0f), 1f, Vector2.UnitX, 0f);
			var farBoid = new TestBoid(_flock, new Vector2(900f, 900f), 1f, Vector2.UnitX, 0f);

			var found = _flock.FindBoidAtPosition(new Vector2(11f, 0f), 5f);

			found.ShouldBe(closeBoid);
			found.ShouldNotBe(farBoid);
		}

		[Test]
		public void Update_RefreshesCellMembership_WithoutThrowing_AndQueriesStillWork()
		{
			var mover = new TestBoid(_flock, Vector2.Zero, 1f, Vector2.UnitX, 100f,
				mass: 1f, minSpeed: 0f, walkSpeed: 0f, maxSpeed: 200f, maxTurnRate: 1f, maxForce: 50f);
			var neighbor = new TestBoid(_flock, new Vector2(10f, 0f), 1f, Vector2.UnitX, 0f);

			var clock = new GameClock();
			clock.Update(0.1f);
			Should.NotThrow(() => _flock.Update(clock));

			//the flock should still be queryable through the cell space after an update
			var inRange = _flock.FindBoidsInRange(mover, 50f);
			inRange.ShouldContain(neighbor);
		}
	}
}
