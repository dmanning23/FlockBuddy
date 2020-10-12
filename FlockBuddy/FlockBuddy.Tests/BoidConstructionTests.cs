using Microsoft.Xna.Framework;
using Moq;
using NUnit.Framework;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlockBuddy.Tests
{
	[TestFixture]
	public class BoidConstructionTests
	{
		IFlockManager test;
		FlockManager manager;

		[SetUp]
		public void Setup()
		{
			var testFlock = new Mock<IFlockManager>();
			testFlock.Setup(x => x.Id).Returns(2);
			testFlock.Setup(x => x.Name).Returns("catpants");
			testFlock.Setup(x => x.BoidMass).Returns(3);
			testFlock.Setup(x => x.BoidMaxForce).Returns(4);
			testFlock.Setup(x => x.BoidMinSpeed).Returns(5);
			testFlock.Setup(x => x.BoidWalkSpeed).Returns(6);
			testFlock.Setup(x => x.BoidMaxSpeed).Returns(7);
			testFlock.Setup(x => x.BoidMaxTurnRate).Returns(8);
			testFlock.Setup(x => x.BoidNeighborQueryRadius).Returns(9);
			testFlock.Setup(x => x.BoidPredatorQueryRadius).Returns(10);
			testFlock.Setup(x => x.BoidPreyQueryRadius).Returns(11);
			testFlock.Setup(x => x.BoidVipQueryRadius).Returns(12);
			testFlock.Setup(x => x.BoidWallQueryRadius).Returns(13);
			testFlock.Setup(x => x.BoidObstacleQueryRadius).Returns(14);
			testFlock.Setup(x => x.BoidWaypointQueryRadius).Returns(15);
			testFlock.Setup(x => x.BoidRadius).Returns(16);
			testFlock.Setup(x => x.BoidRetargetTime).Returns(17);
			testFlock.Setup(x => x.BoidLaziness).Returns(18);
			testFlock.Setup(x => x.SummingMethod).Returns(ESummingMethod.dithered);
			testFlock.Setup(x => x.Walls).Returns(DefaultWalls.All);
			test = testFlock.Object;

			manager = new FlockManager(test);
		}

		[Test]
		public void Constructor_BoidMass()
		{
			var boid = manager.AddBoid(Vector2.Zero, Vector2.UnitX, 0f);
			boid.Mass.ShouldBe(test.BoidMass);
		}

		[Test]
		public void Constructor_BoidMaxForce()
		{
			var boid = manager.AddBoid(Vector2.Zero, Vector2.UnitX, 0f);
			boid.MaxForce.ShouldBe(test.BoidMaxForce);
		}

		[Test]
		public void Constructor_BoidMinSpeed()
		{
			var boid = manager.AddBoid(Vector2.Zero, Vector2.UnitX, 0f);
			boid.Mass.ShouldBe(test.BoidMass);
		}

		[Test]
		public void Constructor_BoidWalkSpeed()
		{
			var boid = manager.AddBoid(Vector2.Zero, Vector2.UnitX, 0f);
			boid.WalkSpeed.ShouldBe(test.BoidWalkSpeed);
		}

		[Test]
		public void Constructor_BoidMaxSpeed()
		{
			var boid = manager.AddBoid(Vector2.Zero, Vector2.UnitX, 0f);
			boid.MaxSpeed.ShouldBe(test.BoidMaxSpeed);
		}

		[Test]
		public void Constructor_BoidMaxTurnRate()
		{
			var boid = manager.AddBoid(Vector2.Zero, Vector2.UnitX, 0f);
			boid.MaxTurnRate.ShouldBe(test.BoidMaxTurnRate);
		}

		[Test]
		public void Constructor_BoidNeighborQueryRadius()
		{
			var boid = manager.AddBoid(Vector2.Zero, Vector2.UnitX, 0f);
			boid.NeighborsQueryRadius.ShouldBe(test.BoidNeighborQueryRadius);
		}

		[Test]
		public void Constructor_BoidPredatorQueryRadius()
		{
			var boid = manager.AddBoid(Vector2.Zero, Vector2.UnitX, 0f);
			boid.PredatorsQueryRadius.ShouldBe(test.BoidPredatorQueryRadius);
		}

		[Test]
		public void Constructor_BoidPreyQueryRadius()
		{
			var boid = manager.AddBoid(Vector2.Zero, Vector2.UnitX, 0f);
			boid.PreyQueryRadius.ShouldBe(test.BoidPreyQueryRadius);
		}

		[Test]
		public void Constructor_BoidVipQueryRadius()
		{
			var boid = manager.AddBoid(Vector2.Zero, Vector2.UnitX, 0f);
			boid.VipQueryRadius.ShouldBe(test.BoidVipQueryRadius);
		}

		[Test]
		public void Constructor_BoidWallQueryRadius()
		{
			var boid = manager.AddBoid(Vector2.Zero, Vector2.UnitX, 0f);
			boid.WallQueryRadius.ShouldBe(test.BoidWallQueryRadius);
		}

		[Test]
		public void Constructor_BoidObstacleQueryRadius()
		{
			
			var boid = manager.AddBoid(Vector2.Zero, Vector2.UnitX, 0f);
			boid.ObstacleQueryRadius.ShouldBe(test.BoidObstacleQueryRadius);
		}

		[Test]
		public void Constructor_BoidWaypointQueryRadius()
		{
			var boid = manager.AddBoid(Vector2.Zero, Vector2.UnitX, 0f);
			boid.WaypointQueryRadius.ShouldBe(test.BoidWaypointQueryRadius);
		}

		[Test]
		public void Constructor_BoidRadius()
		{
			var boid = manager.AddBoid(Vector2.Zero, Vector2.UnitX, 0f);
			boid.Radius.ShouldBe(test.BoidRadius);
		}

		[Test]
		public void Constructor_BoidRetargetTime()
		{
			var boid = manager.AddBoid(Vector2.Zero, Vector2.UnitX, 0f);
			boid.RetargetTime.ShouldBe(test.BoidRetargetTime);
		}

		[Test]
		public void Constructor_BoidLaziness()
		{
			var boid = manager.AddBoid(Vector2.Zero, Vector2.UnitX, 0f);
			boid.Laziness.ShouldBe(test.BoidLaziness);
		}

		[Test]
		public void Constructor_SummingMethod()
		{
			var boid = manager.AddBoid(Vector2.Zero, Vector2.UnitX, 0f);
			boid.SummingMethod.ShouldBe(test.SummingMethod);
		}
	}
}
