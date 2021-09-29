using GameTimer;
using Microsoft.Xna.Framework;
using NUnit.Framework;
using System;
using FlockBuddy;
using FlockBuddy.Interfaces;
using Shouldly;
using FlockBuddy.SteeringBehaviors;

namespace FlockBuddy.Tests
{
	[TestFixture]
	public class BoidTests
	{
		#region setup

		Flock _flock;

		[SetUp]
		public void Setup()
		{
			_flock = new Flock();
		}

		#endregion setup

		#region construction tests

		[Test]
		public void Constructor_position()
		{
			var mover = new TestBoid(_flock, Vector2.UnitX, 1f, Vector2.UnitY, 2f, 3f, 4f, 4f, 4f, 5f, 6f);
			Assert.AreEqual(Vector2.UnitX, mover.Position);
		}

		[Test]
		public void Constructor_radius()
		{
			var mover = new TestBoid(_flock, Vector2.UnitX, 1f, Vector2.UnitY, 2f, 3f, 4f, 4f, 4f, 5f, 6f);
			Assert.AreEqual(1f, mover.Radius);
		}

		[Test]
		public void Constructor_heading()
		{
			var mover = new TestBoid(_flock, Vector2.UnitX, 1f, Vector2.UnitY, 2f, 3f, 4f, 4f, 4f, 5f, 6f);
			Assert.AreEqual(Vector2.UnitY, mover.Heading);
		}

		[Test]
		public void Constructor_speed()
		{
			var mover = new TestBoid(_flock, Vector2.UnitX, 1f, Vector2.UnitY, 2f, 3f, 4f, 4f, 4f, 5f, 6f);
			Assert.AreEqual(2f, mover.Speed);
		}

		[Test]
		public void Constructor_mass()
		{
			var mover = new TestBoid(_flock, Vector2.UnitX, 1f, Vector2.UnitY, 2f, 3f, 4f, 4f, 4f, 5f, 6f);
			Assert.AreEqual(3f, mover.Mass);
		}

		[Test]
		public void Constructor_maxSpeed()
		{
			var mover = new TestBoid(_flock, Vector2.UnitX, 1f, Vector2.UnitY, 2f, 3f, 4f, 4f, 4f, 5f, 6f);
			Assert.AreEqual(4f, mover.MaxSpeed);
		}

		[Test]
		public void Constructor_maxTurnRate()
		{
			var mover = new TestBoid(_flock, Vector2.UnitX, 1f, Vector2.UnitY, 2f, 3f, 4f, 4f, 4f, 5f, 6f);
			Assert.AreEqual(5f, mover.MaxTurnRate);
		}

		[Test]
		public void Constructor_maxForce()
		{
			var mover = new TestBoid(_flock, Vector2.UnitX, 1f, Vector2.UnitY, 2f, 3f, 4f, 4f, 4f, 5f, 6f);
			Assert.AreEqual(6f, mover.MaxForce);
		}

		[Test]
		public void Constructor_NeighborsQueryRadius()
		{
			var boid = new Boid(_flock, Vector2.Zero, Vector2.UnitY, -1f);
			Assert.AreEqual(BoidDefaults.BoidQueryRadius, boid.NeighborsQueryRadius);
		}

		[Test]
		public void Constructor_PredatorsQueryRadius()
		{
			var boid = new Boid(_flock, Vector2.Zero, Vector2.UnitY, -1f);
			Assert.AreEqual(BoidDefaults.BoidQueryRadius, boid.PredatorsQueryRadius);
		}

		[Test]
		public void Constructor_PreyQueryRadius()
		{
			var boid = new Boid(_flock, Vector2.Zero, Vector2.UnitY, -1f);
			Assert.AreEqual(BoidDefaults.BoidQueryRadius, boid.PreyQueryRadius);
		}

		[Test]
		public void Constructor_VipQueryRadius()
		{
			var boid = new Boid(_flock, Vector2.Zero, Vector2.UnitY, -1f);
			Assert.AreEqual(BoidDefaults.BoidQueryRadius, boid.VipQueryRadius);
		}

		[Test]
		public void Constructor_ObstacleQueryRadius()
		{
			var boid = new Boid(_flock, Vector2.Zero, Vector2.UnitY, -1f);
			Assert.AreEqual(BoidDefaults.BoidQueryRadius, boid.ObstacleQueryRadius);
		}

		[Test]
		public void Constructor_WallQueryRadius()
		{
			var boid = new Boid(_flock, Vector2.Zero, Vector2.UnitY, -1f);
			Assert.AreEqual(BoidDefaults.BoidQueryRadius, boid.WallQueryRadius);
		}

		[Test]
		public void Constructor_WaypointQueryRadius()
		{
			var boid = new Boid(_flock, Vector2.Zero, Vector2.UnitY, -1f);
			Assert.AreEqual(BoidDefaults.BoidQueryRadius, boid.WaypointQueryRadius);
		}

		[Test]
		public void Constructor_Mass()
		{
			var boid = new Boid(_flock, Vector2.Zero, Vector2.UnitY, -1f);
			Assert.AreEqual(BoidDefaults.BoidMass, boid.Mass);
		}

		[Test]
		public void Constructor_MinSpeed()
		{
			var boid = new Boid(_flock, Vector2.Zero, Vector2.UnitY, -1f);
			Assert.AreEqual(BoidDefaults.BoidMinSpeed, boid.MinSpeed);
		}

		[Test]
		public void Constructor_WalkSpeed()
		{
			var boid = new Boid(_flock, Vector2.Zero, Vector2.UnitY, -1f);
			Assert.AreEqual(BoidDefaults.BoidWalkSpeed, boid.WalkSpeed);
		}

		[Test]
		public void Constructor_Laziness()
		{
			var boid = new Boid(_flock, Vector2.Zero, Vector2.UnitY, -1f);
			Assert.AreEqual(BoidDefaults.BoidLaziness, boid.Laziness);
		}

		[Test]
		public void Constructor_MaxSpeed()
		{
			var boid = new Boid(_flock, Vector2.Zero, Vector2.UnitY, -1f);
			Assert.AreEqual(BoidDefaults.BoidMaxSpeed, boid.MaxSpeed);
		}

		[Test]
		public void Constructor_MaxTurnRate()
		{
			var boid = new Boid(_flock, Vector2.Zero, Vector2.UnitY, -1f);
			Assert.AreEqual(BoidDefaults.BoidMaxTurnRate, boid.MaxTurnRate);
		}

		[Test]
		public void Constructor_MaxForce()
		{
			var boid = new Boid(_flock, Vector2.Zero, Vector2.UnitY, -1f);
			Assert.AreEqual(BoidDefaults.BoidMaxForce, boid.MaxForce);
		}

		[Test]
		public void Constructor_RetargetTime()
		{
			var boid = new Boid(_flock, Vector2.Zero, Vector2.UnitY, -1f);
			Assert.AreEqual(BoidDefaults.BoidRetargetTime, boid.RetargetTime);
		}

		[Test]
		public void Constructor_RetargetTimer()
		{
			var boid = new Boid(_flock, Vector2.Zero, Vector2.UnitY, -1f);
			boid.RetargetTimer.RemainingTime.ShouldBe(BoidDefaults.BoidRetargetTime);
		}

		[Test]
		public void Constructor_SummingMethod()
		{
			var boid = new Boid(_flock, Vector2.Zero, Vector2.UnitY, -1f);
			Assert.AreEqual(BoidDefaults.DefaultSummingMethod, boid.SummingMethod);
		}

		#endregion //construction tests

		#region Add Behavior

		public void DefaultBehaviors()
		{
			var boid = new TestBoid(_flock, Vector2.UnitX, 1f, Vector2.UnitY, -1f);
			boid.GetBehaviors().ShouldBeEmpty();
		}

		[TestCase(BehaviorType.WallAvoidance)]
		[TestCase(BehaviorType.ObstacleAvoidance)]
		[TestCase(BehaviorType.Evade)]
		[TestCase(BehaviorType.Flee)]
		[TestCase(BehaviorType.Direction)]
		[TestCase(BehaviorType.Separation)]
		[TestCase(BehaviorType.Alignment)]
		[TestCase(BehaviorType.Cohesion)]
		[TestCase(BehaviorType.Seek)]
		[TestCase(BehaviorType.Arrive)]
		[TestCase(BehaviorType.Wander)]
		[TestCase(BehaviorType.Pursuit)]
		[TestCase(BehaviorType.OffsetPursuit)]
		[TestCase(BehaviorType.Interpose)]
		[TestCase(BehaviorType.GuardSeparation)]
		[TestCase(BehaviorType.GuardAlignment)]
		[TestCase(BehaviorType.GuardCohesion)]
		[TestCase(BehaviorType.Hide)]
		[TestCase(BehaviorType.FollowPath)]
		public void AddBehavior(BehaviorType behavior)
		{
			var boid = new TestBoid(_flock, Vector2.UnitX, 1f, Vector2.UnitY, -1f);
			boid.AddBehavior(behavior, -100f);

			boid.GetBehaviors().Count.ShouldBe(1);
			boid.GetBehaviors()[0].BehaviorType.ShouldBe(behavior);
			boid.GetBehaviors()[0].Weight.ShouldBe(-100f);
		}

		[TestCase(BehaviorType.WallAvoidance)]
		[TestCase(BehaviorType.ObstacleAvoidance)]
		[TestCase(BehaviorType.Evade)]
		[TestCase(BehaviorType.Flee)]
		[TestCase(BehaviorType.Direction)]
		[TestCase(BehaviorType.Separation)]
		[TestCase(BehaviorType.Alignment)]
		[TestCase(BehaviorType.Cohesion)]
		[TestCase(BehaviorType.Seek)]
		[TestCase(BehaviorType.Arrive)]
		[TestCase(BehaviorType.Wander)]
		[TestCase(BehaviorType.Pursuit)]
		[TestCase(BehaviorType.OffsetPursuit)]
		[TestCase(BehaviorType.Interpose)]
		[TestCase(BehaviorType.GuardSeparation)]
		[TestCase(BehaviorType.GuardAlignment)]
		[TestCase(BehaviorType.GuardCohesion)]
		[TestCase(BehaviorType.Hide)]
		[TestCase(BehaviorType.FollowPath)]
		public void AddTwoBehaviors(BehaviorType behavior)
		{
			var boid = new TestBoid(_flock, Vector2.UnitX, 1f, Vector2.UnitY, -1f);
			boid.AddBehavior(behavior, -100f);

			boid.AddBehavior(behavior, -200f);

			boid.GetBehaviors().Count.ShouldBe(1);
			boid.GetBehaviors()[0].BehaviorType.ShouldBe(behavior);
			boid.GetBehaviors()[0].Weight.ShouldBe(-200f);
		}

		[TestCase(BehaviorType.WallAvoidance)]
		[TestCase(BehaviorType.ObstacleAvoidance)]
		[TestCase(BehaviorType.Evade)]
		[TestCase(BehaviorType.Flee)]
		[TestCase(BehaviorType.Direction)]
		[TestCase(BehaviorType.Separation)]
		[TestCase(BehaviorType.Alignment)]
		[TestCase(BehaviorType.Cohesion)]
		[TestCase(BehaviorType.Seek)]
		[TestCase(BehaviorType.Arrive)]
		[TestCase(BehaviorType.Wander)]
		[TestCase(BehaviorType.Pursuit)]
		[TestCase(BehaviorType.OffsetPursuit)]
		[TestCase(BehaviorType.Interpose)]
		[TestCase(BehaviorType.GuardSeparation)]
		[TestCase(BehaviorType.GuardAlignment)]
		[TestCase(BehaviorType.GuardCohesion)]
		[TestCase(BehaviorType.Hide)]
		[TestCase(BehaviorType.FollowPath)]
		public void AddBehavior_ReturnValue(BehaviorType behavior)
		{
			var boid = new TestBoid(_flock, Vector2.UnitX, 1f, Vector2.UnitY, -1f);
			var value = boid.AddBehavior(behavior, -100f);

			value.BehaviorType.ShouldBe(behavior);
			value.Weight.ShouldBe(-100f);
		}

		[TestCase(BehaviorType.WallAvoidance)]
		[TestCase(BehaviorType.ObstacleAvoidance)]
		[TestCase(BehaviorType.Evade)]
		[TestCase(BehaviorType.Flee)]
		[TestCase(BehaviorType.Direction)]
		[TestCase(BehaviorType.Separation)]
		[TestCase(BehaviorType.Alignment)]
		[TestCase(BehaviorType.Cohesion)]
		[TestCase(BehaviorType.Seek)]
		[TestCase(BehaviorType.Arrive)]
		[TestCase(BehaviorType.Wander)]
		[TestCase(BehaviorType.Pursuit)]
		[TestCase(BehaviorType.OffsetPursuit)]
		[TestCase(BehaviorType.Interpose)]
		[TestCase(BehaviorType.GuardSeparation)]
		[TestCase(BehaviorType.GuardAlignment)]
		[TestCase(BehaviorType.GuardCohesion)]
		[TestCase(BehaviorType.Hide)]
		[TestCase(BehaviorType.FollowPath)]
		public void AddBehaviorObject(BehaviorType behaviorType)
		{
			var boid = new TestBoid(_flock, Vector2.UnitX, 1f, Vector2.UnitY, -1f);

			IBehavior behavior = BaseBehavior.BehaviorFactory(behaviorType, boid);
			behavior.Weight = -100f;
			boid.AddBehavior(behavior);

			boid.GetBehaviors().Count.ShouldBe(1);
			boid.GetBehaviors()[0].BehaviorType.ShouldBe(behaviorType);
			boid.GetBehaviors()[0].Weight.ShouldBe(-100f);
		}

		[TestCase(BehaviorType.WallAvoidance)]
		[TestCase(BehaviorType.ObstacleAvoidance)]
		[TestCase(BehaviorType.Evade)]
		[TestCase(BehaviorType.Flee)]
		[TestCase(BehaviorType.Direction)]
		[TestCase(BehaviorType.Separation)]
		[TestCase(BehaviorType.Alignment)]
		[TestCase(BehaviorType.Cohesion)]
		[TestCase(BehaviorType.Seek)]
		[TestCase(BehaviorType.Arrive)]
		[TestCase(BehaviorType.Wander)]
		[TestCase(BehaviorType.Pursuit)]
		[TestCase(BehaviorType.OffsetPursuit)]
		[TestCase(BehaviorType.Interpose)]
		[TestCase(BehaviorType.GuardSeparation)]
		[TestCase(BehaviorType.GuardAlignment)]
		[TestCase(BehaviorType.GuardCohesion)]
		[TestCase(BehaviorType.Hide)]
		[TestCase(BehaviorType.FollowPath)]
		public void AddTwoBehaviorObject(BehaviorType behaviorType)
		{
			var boid = new TestBoid(_flock, Vector2.UnitX, 1f, Vector2.UnitY, -1f);

			IBehavior behavior = BaseBehavior.BehaviorFactory(behaviorType, boid);
			behavior.Weight = -100f;
			boid.AddBehavior(behavior);

			IBehavior behavior2 = BaseBehavior.BehaviorFactory(behaviorType, boid);
			behavior2.Weight = -200f;
			boid.AddBehavior(behavior2);

			boid.GetBehaviors().Count.ShouldBe(1);
			boid.GetBehaviors()[0].BehaviorType.ShouldBe(behaviorType);
			boid.GetBehaviors()[0].Weight.ShouldBe(-200f);
		}

		#endregion //Add Behavior

		#region Remove Behavior

		[TestCase(BehaviorType.WallAvoidance)]
		[TestCase(BehaviorType.ObstacleAvoidance)]
		[TestCase(BehaviorType.Evade)]
		[TestCase(BehaviorType.Flee)]
		[TestCase(BehaviorType.Direction)]
		[TestCase(BehaviorType.Separation)]
		[TestCase(BehaviorType.Alignment)]
		[TestCase(BehaviorType.Cohesion)]
		[TestCase(BehaviorType.Seek)]
		[TestCase(BehaviorType.Arrive)]
		[TestCase(BehaviorType.Wander)]
		[TestCase(BehaviorType.Pursuit)]
		[TestCase(BehaviorType.OffsetPursuit)]
		[TestCase(BehaviorType.Interpose)]
		[TestCase(BehaviorType.GuardSeparation)]
		[TestCase(BehaviorType.GuardAlignment)]
		[TestCase(BehaviorType.GuardCohesion)]
		[TestCase(BehaviorType.Hide)]
		[TestCase(BehaviorType.FollowPath)]
		public void RemoveBehavior(BehaviorType behavior)
		{
			var boid = new TestBoid(_flock, Vector2.UnitX, 1f, Vector2.UnitY, -1f);
			boid.AddBehavior(behavior, -100f);

			boid.RemoveBehavior(behavior);

			boid.GetBehaviors().Count.ShouldBe(0);
		}

		[TestCase(BehaviorType.WallAvoidance)]
		[TestCase(BehaviorType.ObstacleAvoidance)]
		[TestCase(BehaviorType.Evade)]
		[TestCase(BehaviorType.Flee)]
		[TestCase(BehaviorType.Direction)]
		[TestCase(BehaviorType.Separation)]
		[TestCase(BehaviorType.Alignment)]
		[TestCase(BehaviorType.Cohesion)]
		[TestCase(BehaviorType.Seek)]
		[TestCase(BehaviorType.Arrive)]
		[TestCase(BehaviorType.Wander)]
		[TestCase(BehaviorType.Pursuit)]
		[TestCase(BehaviorType.OffsetPursuit)]
		[TestCase(BehaviorType.Interpose)]
		[TestCase(BehaviorType.GuardSeparation)]
		[TestCase(BehaviorType.GuardAlignment)]
		[TestCase(BehaviorType.GuardCohesion)]
		[TestCase(BehaviorType.Hide)]
		[TestCase(BehaviorType.FollowPath)]
		public void RemoveTwoBehaviors(BehaviorType behavior)
		{
			var boid = new TestBoid(_flock, Vector2.UnitX, 1f, Vector2.UnitY, -1f);
			boid.AddBehavior(behavior, -100f);

			boid.AddBehavior(behavior, -200f);

			boid.RemoveBehavior(behavior);

			boid.GetBehaviors().Count.ShouldBe(0);
		}

		[TestCase(BehaviorType.WallAvoidance)]
		[TestCase(BehaviorType.ObstacleAvoidance)]
		[TestCase(BehaviorType.Evade)]
		[TestCase(BehaviorType.Flee)]
		[TestCase(BehaviorType.Direction)]
		[TestCase(BehaviorType.Separation)]
		[TestCase(BehaviorType.Alignment)]
		[TestCase(BehaviorType.Cohesion)]
		[TestCase(BehaviorType.Seek)]
		[TestCase(BehaviorType.Arrive)]
		[TestCase(BehaviorType.Wander)]
		[TestCase(BehaviorType.Pursuit)]
		[TestCase(BehaviorType.OffsetPursuit)]
		[TestCase(BehaviorType.Interpose)]
		[TestCase(BehaviorType.GuardSeparation)]
		[TestCase(BehaviorType.GuardAlignment)]
		[TestCase(BehaviorType.GuardCohesion)]
		[TestCase(BehaviorType.Hide)]
		[TestCase(BehaviorType.FollowPath)]
		public void RemoveBehaviorObject(BehaviorType behaviorType)
		{
			var boid = new TestBoid(_flock, Vector2.UnitX, 1f, Vector2.UnitY, -1f);

			IBehavior behavior = BaseBehavior.BehaviorFactory(behaviorType, boid);
			behavior.Weight = -100f;
			boid.AddBehavior(behavior);

			boid.RemoveBehavior(behavior);

			boid.GetBehaviors().Count.ShouldBe(0);
		}

		[TestCase(BehaviorType.WallAvoidance)]
		[TestCase(BehaviorType.ObstacleAvoidance)]
		[TestCase(BehaviorType.Evade)]
		[TestCase(BehaviorType.Flee)]
		[TestCase(BehaviorType.Direction)]
		[TestCase(BehaviorType.Separation)]
		[TestCase(BehaviorType.Alignment)]
		[TestCase(BehaviorType.Cohesion)]
		[TestCase(BehaviorType.Seek)]
		[TestCase(BehaviorType.Arrive)]
		[TestCase(BehaviorType.Wander)]
		[TestCase(BehaviorType.Pursuit)]
		[TestCase(BehaviorType.OffsetPursuit)]
		[TestCase(BehaviorType.Interpose)]
		[TestCase(BehaviorType.GuardSeparation)]
		[TestCase(BehaviorType.GuardAlignment)]
		[TestCase(BehaviorType.GuardCohesion)]
		[TestCase(BehaviorType.Hide)]
		[TestCase(BehaviorType.FollowPath)]
		public void RemoveTwoBehaviorObject(BehaviorType behaviorType)
		{
			var boid = new TestBoid(_flock, Vector2.UnitX, 1f, Vector2.UnitY, -1f);

			IBehavior behavior = BaseBehavior.BehaviorFactory(behaviorType, boid);
			behavior.Weight = -100f;
			boid.AddBehavior(behavior);

			IBehavior behavior2 = BaseBehavior.BehaviorFactory(behaviorType, boid);
			behavior2.Weight = -200f;
			boid.AddBehavior(behavior2);

			boid.RemoveBehavior(behavior);

			boid.GetBehaviors().Count.ShouldBe(0);
		}

		#endregion //Add Behavior

		#region update speed tests

		[Test]
		public void UpdateSpeed_SpeedUp()
		{
			var dude = new TestBoid(_flock, Vector2.Zero, 10.0f, new Vector2(1.0f, 0.0f), 100.0f, 1.0f, 25f, 50f, 100.0f, 4.0f, 100.0f);
			var change = dude.GetSpeedChange(new Vector2(1.0f, 0.0f));

			change = (float)Math.Round(change, 4);

			Assert.AreEqual(100.0f, change);
		}

		[Test]
		public void UpdateSpeed_SlowDown()
		{
			var dude = new TestBoid(_flock, Vector2.Zero, 10.0f, new Vector2(1.0f, 0.0f), 100.0f, 1.0f, 25f, 50f, 100.0f, 4.0f, 100.0f);
			var change = dude.GetSpeedChange(new Vector2(-1.0f, 0.0f));

			change = (float)Math.Round(change, 4);

			Assert.AreEqual(-100.0f, change);
		}

		[Test]
		public void UpdateSpeed_MaxSpeed()
		{
			var dude = new TestBoid(_flock, Vector2.Zero, 10.0f, new Vector2(1.0f, 0.0f), 100.0f, 1.0f, 25f, 50f, 1000.0f, 4.0f, 100.0f);
			dude.Speed = 950f;
			dude.UpdateSpeed(new Vector2(1.0f, 0.0f));
			var speed = dude.Speed;

			speed = (float)Math.Round(speed, 4);

			Assert.AreEqual(1000.0f, speed);
		}

		[Test]
		public void UpdateSpeed_HalfSpeed()
		{
			var dude = new TestBoid(_flock, Vector2.Zero, 10.0f, new Vector2(1.0f, 0.0f), 100.0f, 1.0f, 25f, 50f, 100.0f, 4.0f, 100.0f);
			dude.Timer.TimeDelta = 0.5f;
			var change = dude.GetSpeedChange(new Vector2(1.0f, 0.0f));

			change = (float)Math.Round(change, 4);

			Assert.AreEqual(50f, change);
		}

		[Test]
		public void UpdateSpeed_Stopped_SlowDown()
		{
			var dude = new TestBoid(_flock, Vector2.Zero, 10.0f, new Vector2(1.0f, 0.0f), 100.0f, 1.0f, 25f, 50f, 100.0f, 4.0f, 100.0f);
			dude.Speed = 10f;
			dude.UpdateSpeed(new Vector2(-1.0f, 0.0f));
			var speed = dude.Speed;

			speed = (float)Math.Round(speed, 4);

			//will be set to the walkspeed
			Assert.AreEqual(25f, speed);
		}

		#endregion //update speed tests

		#region update heading tests

		[Test]
		public void GetNewHeadingMinus90()
		{
			var dude = new TestBoid(_flock, Vector2.Zero, 10.0f, new Vector2(0.0f, -1.0f), 100.0f, 1.0f, 25f, 50f, 100.0f, 4.0f, 100.0f);
			float heading = 0.0f;
			dude.GetAmountToTurn(new Vector2(-1.0f, 0.0f), ref heading);

			heading = MathHelper.ToDegrees(heading);
			heading = (float)Math.Round(heading, 4);

			Assert.AreEqual(-90.0f, heading);
		}

		[Test]
		public void GetNewHeadingMinus90_1()
		{
			var dude = new TestBoid(_flock, Vector2.Zero, 10.0f, new Vector2(1.0f, 0.0f), 100.0f, 1.0f, 25f, 50f, 100.0f, 4.0f, 100.0f);
			float heading = 0.0f;
			dude.GetAmountToTurn(new Vector2(0.0f, -1.0f), ref heading);

			heading = MathHelper.ToDegrees(heading);
			heading = (float)Math.Round(heading, 4);

			Assert.AreEqual(-90.0f, heading);
		}

		[Test]
		public void GetNewHeadingMinus90_2()
		{
			var dude = new TestBoid(_flock, Vector2.Zero, 10.0f, new Vector2(0.0f, 1.0f), 100.0f, 1.0f, 25f, 50f, 100.0f, 4.0f, 100.0f);
			float heading = 0.0f;
			dude.GetAmountToTurn(new Vector2(1.0f, 0.0f), ref heading);

			heading = MathHelper.ToDegrees(heading);
			heading = (float)Math.Round(heading, 4);

			Assert.AreEqual(-90.0f, heading);
		}

		[Test]
		public void GetNewHeadingMinus90_3()
		{
			var dude = new TestBoid(_flock, Vector2.Zero, 10.0f, new Vector2(-1.0f, 0.0f), 100.0f, 1.0f, 25f, 50f, 100.0f, 4.0f, 100.0f);
			float heading = 0.0f;
			dude.GetAmountToTurn(new Vector2(0.0f, 1.0f), ref heading);

			heading = MathHelper.ToDegrees(heading);
			heading = (float)Math.Round(heading, 4);

			Assert.AreEqual(-90.0f, heading);
		}

		[Test]
		public void GetNewHeading90()
		{
			var dude = new TestBoid(_flock, Vector2.Zero, 10.0f, new Vector2(0.0f, -1.0f), 100.0f, 1.0f, 25f, 50f, 100.0f, 4.0f, 100.0f);
			float heading = 0.0f;
			dude.GetAmountToTurn(new Vector2(1.0f, 0.0f), ref heading);

			heading = MathHelper.ToDegrees(heading);
			heading = (float)Math.Round(heading, 4);

			Assert.AreEqual(90.0f, heading);
		}

		[Test]
		public void GetNewHeading90_withTimeDelta()
		{
			var dude = new TestBoid(_flock, Vector2.Zero, 10.0f, new Vector2(0.0f, -1.0f), 100.0f, 1.0f, 25f, 50f, 100.0f, MathHelper.ToRadians(90f), 100.0f);
			dude.Timer.TimeDelta = 0.5f;
			float heading = 0.0f;
			dude.GetAmountToTurn(new Vector2(1.0f, 0.0f), ref heading);

			heading = MathHelper.ToDegrees(heading);
			heading = (float)Math.Round(heading, 4);

			Assert.AreEqual(45.0f, heading);
		}

		[Test]
		public void GetHeading180()
		{
			var dude = new TestBoid(_flock, Vector2.Zero, 10.0f, new Vector2(0.0f, -1.0f), 100.0f, 1.0f, 25f, 50f, 100.0f, MathHelper.ToRadians(180f), 100.0f);
			float heading = 0.0f;
			dude.GetAmountToTurn(new Vector2(0.0f, 1.0f), ref heading);

			heading = MathHelper.ToDegrees(heading);
			heading = (float)Math.Round(heading, 4);

			Assert.AreEqual(180f, heading);
		}

		[Test]
		public void GetHeading_OverMaxTurnRate()
		{
			var dude = new TestBoid(_flock, Vector2.Zero, 10.0f, new Vector2(0.0f, -1.0f), 100.0f, 1.0f, 25f, 50f, 100.0f, MathHelper.ToRadians(90f), 100.0f);
			float heading = 0.0f;
			dude.GetAmountToTurn(new Vector2(0.0f, 1.0f), ref heading);

			heading = MathHelper.ToDegrees(heading);
			heading = (float)Math.Round(heading, 4);

			Assert.AreEqual(90f, heading);
		}

		[Test]
		public void GetNewHeading90_1()
		{
			var dude = new TestBoid(_flock, Vector2.Zero, 10.0f, new Vector2(1.0f, 0.0f), 100.0f, 1.0f, 25f, 50f, 100.0f, 4.0f, 100.0f);
			float heading = 0.0f;
			dude.GetAmountToTurn(new Vector2(0.0f, 1.0f), ref heading);

			heading = MathHelper.ToDegrees(heading);
			heading = (float)Math.Round(heading, 4);

			Assert.AreEqual(90.0f, heading);
		}

		[Test]
		public void GetNewHeading90_2()
		{
			var dude = new TestBoid(_flock, Vector2.Zero, 10.0f, new Vector2(0.0f, 1.0f), 100.0f, 1.0f, 25f, 50f, 100.0f, 4.0f, 100.0f);
			float heading = 0.0f;
			dude.GetAmountToTurn(new Vector2(-1.0f, 0.0f), ref heading);

			heading = MathHelper.ToDegrees(heading);
			heading = (float)Math.Round(heading, 4);

			Assert.AreEqual(90.0f, heading);
		}

		[Test]
		public void GetNewHeading90_3()
		{
			var dude = new TestBoid(_flock, Vector2.Zero, 10.0f, new Vector2(-1.0f, 0.0f), 100.0f, 1.0f, 25f, 50f, 100.0f, 4.0f, 100.0f);
			float heading = 0.0f;
			dude.GetAmountToTurn(new Vector2(0.0f, -1.0f), ref heading);

			heading = MathHelper.ToDegrees(heading);
			heading = (float)Math.Round(heading, 4);

			Assert.AreEqual(90.0f, heading);
		}

		#endregion //update heading tests
	}
}
