using GameTimer;
using Microsoft.Xna.Framework;
using NUnit.Framework;
using System;

namespace FlockBuddy.Tests
{
	[TestFixture]
	public class MoverTests
	{
		#region construction tests

		[Test]
		public void Constructor_position()
		{
			var mover = new TestMover(Vector2.UnitX, 1f, Vector2.UnitY, 2f, 3f, 4f, 4f, 4f, 5f, 6f);
			Assert.AreEqual(Vector2.UnitX, mover.Position);
		}

		[Test]
		public void Constructor_radius()
		{
			var mover = new TestMover(Vector2.UnitX, 1f, Vector2.UnitY, 2f, 3f, 4f, 4f, 4f, 5f, 6f);
			Assert.AreEqual(1f, mover.Radius);
		}

		[Test]
		public void Constructor_heading()
		{
			var mover = new TestMover(Vector2.UnitX, 1f, Vector2.UnitY, 2f, 3f, 4f, 4f, 4f, 5f, 6f);
			Assert.AreEqual(Vector2.UnitY, mover.Heading);
		}

		[Test]
		public void Constructor_speed()
		{
			var mover = new TestMover(Vector2.UnitX, 1f, Vector2.UnitY, 2f, 3f, 4f, 4f, 4f, 5f, 6f);
			Assert.AreEqual(2f, mover.Speed);
		}

		#endregion //construction tests
		
		#region update heading tests

		[Test]
		public void UpdateHeading90()
		{
			var dude = new TestMover(Vector2.Zero, 10.0f, new Vector2(0.0f, -1.0f), 100.0f, 1.0f, 25f, 50f, 100.0f, 4.0f, 100.0f);
			dude.RotateHeading(MathHelper.ToRadians(90.0f));

			Assert.AreEqual(1.0f, Math.Round(dude.Heading.X, 3));
			Assert.AreEqual(0.0f, Math.Round(dude.Heading.Y, 3));
		}

		[Test]
		public void UpdateHeadingMinus90()
		{
			var dude = new TestMover(Vector2.Zero, 10.0f, new Vector2(0.0f, -1.0f), 100.0f, 1.0f, 25f, 50f, 100.0f, 4.0f, 100.0f);
			dude.RotateHeading(MathHelper.ToRadians(-90.0f));

			Assert.AreEqual(-1.0f, Math.Round(dude.Heading.X, 3));
			Assert.AreEqual(0.0f, Math.Round(dude.Heading.Y, 3));
		}

		[Test]
		public void UpdateHeading90_1()
		{
			var dude = new TestMover(Vector2.Zero, 10.0f, new Vector2(-1.0f, 0.0f), 100.0f, 1.0f, 25f, 50f, 100.0f, 4.0f, 100.0f);
			dude.RotateHeading(MathHelper.ToRadians(90.0f));

			Assert.AreEqual(0.0f, Math.Round(dude.Heading.X, 3));
			Assert.AreEqual(-1.0f, Math.Round(dude.Heading.Y, 3));
		}

		[Test]
		public void UpdateHeadingMinus90_1()
		{
			var dude = new TestMover(Vector2.Zero, 10.0f, new Vector2(-1.0f, 0.0f), 100.0f, 1.0f, 25f, 50f, 100.0f, 4.0f, 100.0f);
			dude.RotateHeading(MathHelper.ToRadians(-90.0f));

			Assert.AreEqual(0.0f, Math.Round(dude.Heading.X, 3));
			Assert.AreEqual(1.0f, Math.Round(dude.Heading.Y, 3));
		}

		#endregion //update heading tests

		#region test class 

		class TestMover : Mover
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
	
			public TestMover(Vector2 position, float radius, Vector2 heading, float speed, float mass, float minSpeed, float walkSpeed, float maxSpeed, float maxTurnRate, float maxForce) : base(position, radius, heading, speed)
			{
				BoidTimer.TimeDelta = 1f;
			}

			public new void RotateHeading(float angle)
			{
				base.RotateHeading(angle);
			}
		}

		#endregion //test class 
	}
}
