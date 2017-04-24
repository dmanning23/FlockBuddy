using Microsoft.Xna.Framework;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlockBuddy.Tests
{
	[TestFixture]
	public class FlockManagerTests
	{
		IFlockManager test;

		[SetUp]
		public void Setup()
		{
			var manager = new Mock<IFlockManager>();
			manager.Setup(x => x.Id).Returns(2);
			manager.Setup(x => x.Name).Returns("catpants");
			manager.Setup(x => x.BoidMass).Returns(3);
			manager.Setup(x => x.BoidMaxForce).Returns(4);
			manager.Setup(x => x.BoidMinSpeed).Returns(5);
			manager.Setup(x => x.BoidWalkSpeed).Returns(6);
			manager.Setup(x => x.BoidMaxSpeed).Returns(7);
			manager.Setup(x => x.BoidMaxTurnRate).Returns(8);
			manager.Setup(x => x.BoidNeighborQueryRadius).Returns(9);
			manager.Setup(x => x.BoidPredatorQueryRadius).Returns(10);
			manager.Setup(x => x.BoidPreyQueryRadius).Returns(11);
			manager.Setup(x => x.BoidVipQueryRadius).Returns(12);
			manager.Setup(x => x.BoidWallQueryRadius).Returns(13);
			manager.Setup(x => x.BoidObstacleQueryRadius).Returns(14);
			manager.Setup(x => x.BoidWaypointQueryRadius).Returns(15);
			manager.Setup(x => x.BoidRadius).Returns(16);
			manager.Setup(x => x.BoidRetargetTime).Returns(17);
			manager.Setup(x => x.SummingMethod).Returns(ESummingMethod.dithered);
			manager.Setup(x => x.Walls).Returns(DefaultWalls.All);
			test = manager.Object;
		}

		[Test]
		public void Constructor_Id()
		{
			var manager = new FlockManager(test);
			Assert.AreEqual(test.Id, manager.Id);
		}

		[Test]
		public void Constructor_Name()
		{
			var manager = new FlockManager(test);
			Assert.AreEqual(test.Name, manager.Name);
		}

		[Test]
		public void Constructor_BoidMass()
		{
			var manager = new FlockManager(test);
			Assert.AreEqual(test.BoidMass, manager.BoidMass);
		}

		[Test]
		public void Constructor_BoidMaxForce()
		{
			var manager = new FlockManager(test);
			Assert.AreEqual(test.BoidMaxForce, manager.BoidMaxForce);
		}

		[Test]
		public void Constructor_BoidMinSpeed()
		{
			var manager = new FlockManager(test);
			Assert.AreEqual(test.BoidMinSpeed, manager.BoidMinSpeed);
		}

		[Test]
		public void Constructor_BoidWalkSpeed()
		{
			var manager = new FlockManager(test);
			Assert.AreEqual(test.BoidWalkSpeed, manager.BoidWalkSpeed);
		}

		[Test]
		public void Constructor_BoidMaxSpeed()
		{
			var manager = new FlockManager(test);
			Assert.AreEqual(test.BoidMaxSpeed, manager.BoidMaxSpeed);
		}

		[Test]
		public void Constructor_BoidMaxTurnRate()
		{
			var manager = new FlockManager(test);
			Assert.AreEqual(test.BoidMaxTurnRate, manager.BoidMaxTurnRate);
		}

		[Test]
		public void Constructor_BoidNeighborQueryRadius()
		{
			var manager = new FlockManager(test);
			Assert.AreEqual(test.BoidNeighborQueryRadius, manager.BoidNeighborQueryRadius);
		}

		[Test]
		public void Constructor_BoidPredatorQueryRadius()
		{
			var manager = new FlockManager(test);
			Assert.AreEqual(test.BoidPredatorQueryRadius, manager.BoidPredatorQueryRadius);
		}

		[Test]
		public void Constructor_BoidPreyQueryRadius()
		{
			var manager = new FlockManager(test);
			Assert.AreEqual(test.BoidPreyQueryRadius, manager.BoidPreyQueryRadius);
		}

		[Test]
		public void Constructor_BoidVipQueryRadius()
		{
			var manager = new FlockManager(test);
			Assert.AreEqual(test.BoidVipQueryRadius, manager.BoidVipQueryRadius);
		}

		[Test]
		public void Constructor_BoidWallQueryRadius()
		{
			var manager = new FlockManager(test);
			Assert.AreEqual(test.BoidWallQueryRadius, manager.BoidWallQueryRadius);
		}

		[Test]
		public void Constructor_BoidObstacleQueryRadius()
		{
			var manager = new FlockManager(test);
			Assert.AreEqual(test.BoidObstacleQueryRadius, manager.BoidObstacleQueryRadius);
		}

		[Test]
		public void Constructor_BoidWaypointQueryRadius()
		{
			var manager = new FlockManager(test);
			Assert.AreEqual(test.BoidWaypointQueryRadius, manager.BoidWaypointQueryRadius);
		}

		[Test]
		public void Constructor_BoidRadius()
		{
			var manager = new FlockManager(test);
			Assert.AreEqual(test.BoidRadius, manager.BoidRadius);
		}

		[Test]
		public void Constructor_BoidRetargetTime()
		{
			var manager = new FlockManager(test);
			Assert.AreEqual(test.BoidRetargetTime, manager.BoidRetargetTime);
		}

		[Test]
		public void Constructor_SummingMethod()
		{
			var manager = new FlockManager(test);
			Assert.AreEqual(test.SummingMethod, manager.SummingMethod);
		}

		[Test]
		public void Constructor_Walls()
		{
			var manager = new FlockManager(test);
			Assert.AreEqual(test.Walls, manager.Walls);
		}

		[Test]
		public void DebugColors()
		{
			var manager = new FlockManager(new Flock());

			Assert.AreEqual(Color.Red, manager.DebugColor);
		}

		[Test]
		public void DebugColors2()
		{
			var manager = new FlockManager(new Flock());
			var manager2 = new FlockManager(new Flock());
			
			Assert.AreEqual(Color.Orange, manager2.DebugColor);
		}

		[Test]
		public void DebugColors3()
		{
			var manager = new FlockManager(new Flock());
			var manager2 = new FlockManager(new Flock());

			Assert.AreEqual(Color.Red, manager.DebugColor);
			Assert.AreEqual(Color.Orange, manager2.DebugColor);
		}
	}
}
