using FakeItEasy;
using FlockBuddy.Interfaces;
using FlockBuddy.SteeringBehaviors;
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
	public class BehaviorTests
	{
		IBoid testBoid;

		[SetUp]
		public void Setup()
		{
			testBoid = A.Fake<IBoid>();
		}

		[Test]
		public void AlignmentTest()
		{
			var behavior = new Alignment(testBoid);
			behavior.BehaviorType.ShouldBe(BehaviorType.Alignment);
			behavior.Weight.ShouldBe(BoidDefaults.AlignmentWeight);
		}

		[Test]
		public void ArriveTest()
		{
			var behavior = new Arrive(testBoid);
			behavior.BehaviorType.ShouldBe(BehaviorType.Arrive);
			behavior.Weight.ShouldBe(BoidDefaults.ArriveWeight);
		}

		[Test]
		public void CohesionTest()
		{
			var behavior = new Cohesion(testBoid);
			behavior.BehaviorType.ShouldBe(BehaviorType.Cohesion);
			behavior.Weight.ShouldBe(BoidDefaults.CohesionWeight);
		}

		[Test]
		public void DirectionTest()
		{
			var behavior = new Direction(testBoid);
			behavior.BehaviorType.ShouldBe(BehaviorType.Direction);
			behavior.Weight.ShouldBe(BoidDefaults.DirectionWeight);
		}

		[Test]
		public void EvadeTest()
		{
			var behavior = new Evade(testBoid);
			behavior.BehaviorType.ShouldBe(BehaviorType.Evade);
			behavior.Weight.ShouldBe(BoidDefaults.EvadeWeight);
		}

		[Test]
		public void FleeTest()
		{
			var behavior = new Flee(testBoid);
			behavior.BehaviorType.ShouldBe(BehaviorType.Flee);
			behavior.Weight.ShouldBe(BoidDefaults.FleeWeight);
		}

		[Test]
		public void FollowPathTest()
		{
			var behavior = new FollowPath(testBoid);
			behavior.BehaviorType.ShouldBe(BehaviorType.FollowPath);
			behavior.Weight.ShouldBe(BoidDefaults.FollowPathWeight);
		}

		[Test]
		public void GuardAlignmentTest()
		{
			var behavior = new GuardAlignment(testBoid);
			behavior.BehaviorType.ShouldBe(BehaviorType.GuardAlignment);
			behavior.Weight.ShouldBe(BoidDefaults.AlignmentWeight);
		}

		[Test]
		public void GuardCohesionTest()
		{
			var behavior = new GuardCohesion(testBoid);
			behavior.BehaviorType.ShouldBe(BehaviorType.GuardCohesion);
			behavior.Weight.ShouldBe(BoidDefaults.CohesionWeight);
		}

		[Test]
		public void GuardSeparationTest()
		{
			var behavior = new GuardSeparation(testBoid);
			behavior.BehaviorType.ShouldBe(BehaviorType.GuardSeparation);
			behavior.Weight.ShouldBe(BoidDefaults.SeparationWeight);
		}

		[Test]
		public void HideTest()
		{
			var behavior = new Hide(testBoid);
			behavior.BehaviorType.ShouldBe(BehaviorType.Hide);
			behavior.Weight.ShouldBe(BoidDefaults.HideWeight);
		}

		[Test]
		public void InterposeTest()
		{
			var behavior = new Interpose(testBoid);
			behavior.BehaviorType.ShouldBe(BehaviorType.Interpose);
			behavior.Weight.ShouldBe(BoidDefaults.InterposeWeight);
		}

		[Test]
		public void ObstacleAvoidanceTest()
		{
			var behavior = new ObstacleAvoidance(testBoid);
			behavior.BehaviorType.ShouldBe(BehaviorType.ObstacleAvoidance);
			behavior.Weight.ShouldBe(BoidDefaults.ObstacleAvoidanceWeight);
		}

		[Test]
		public void OffsetPursuitTest()
		{
			var behavior = new OffsetPursuit(testBoid);
			behavior.BehaviorType.ShouldBe(BehaviorType.OffsetPursuit);
			behavior.Weight.ShouldBe(BoidDefaults.OffsetPursuitWeight);
		}

		[Test]
		public void PursuitTest()
		{
			var behavior = new Pursuit(testBoid);
			behavior.BehaviorType.ShouldBe(BehaviorType.Pursuit);
			behavior.Weight.ShouldBe(BoidDefaults.PursuitWeight);
		}

		[Test]
		public void SeekTest()
		{
			var behavior = new Seek(testBoid);
			behavior.BehaviorType.ShouldBe(BehaviorType.Seek);
			behavior.Weight.ShouldBe(BoidDefaults.SeekWeight);
		}

		[Test]
		public void SeparationTest()
		{
			var behavior = new Separation(testBoid);
			behavior.BehaviorType.ShouldBe(BehaviorType.Separation);
			behavior.Weight.ShouldBe(BoidDefaults.SeparationWeight);
		}

		[Test]
		public void WallAvoidanceTest()
		{
			var behavior = new WallAvoidance(testBoid);
			behavior.BehaviorType.ShouldBe(BehaviorType.WallAvoidance);
			behavior.Weight.ShouldBe(BoidDefaults.WallAvoidanceWeight);
		}

		[Test]
		public void WanderTest()
		{
			var behavior = new Wander(testBoid);
			behavior.BehaviorType.ShouldBe(BehaviorType.Wander);
			behavior.Weight.ShouldBe(BoidDefaults.WanderWeight);
		}
	}
}
