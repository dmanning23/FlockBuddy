using GameTimer;
using Microsoft.Xna.Framework;
using NUnit.Framework;
using System;

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

		#endregion //construction tests

		#region method tests


		#endregion method tests

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

		#region test class 

		class TestBoid : Boid
		{
			public new float Speed
			{
				get
				{
					return base.Speed;
				}
				set
				{
					base.Speed = value;
				}
			}

			public GameClock Timer
			{
				get
				{
					return BoidTimer;
				}
			}

			public TestBoid(IFlock flock, Vector2 position, float radius, Vector2 heading, float speed, float mass, float minSpeed, float walkSpeed, float maxSpeed, float maxTurnRate, float maxForce) : base(flock, position, heading, speed, radius, mass, minSpeed, walkSpeed, maxSpeed, maxTurnRate, maxForce)
			{
				BoidTimer.TimeDelta = 1f;
			}

			public new void RotateHeading(float angle)
			{
				base.RotateHeading(angle);
			}

			public new void UpdateSpeed(Vector2 targetHeading)
			{
				base.UpdateSpeed(targetHeading);
			}

			public new bool GetAmountToTurn(Vector2 targetHeading, ref float angle)
			{
				return base.GetAmountToTurn(targetHeading, ref angle);
			}

			public new float GetSpeedChange(Vector2 targetHeading)
			{
				return base.GetSpeedChange(targetHeading);
			}
		}

		#endregion //test class 
	}
}
