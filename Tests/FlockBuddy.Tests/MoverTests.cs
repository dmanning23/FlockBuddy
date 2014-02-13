using System;
using NUnit.Framework;
using FlockBuddy;
using Microsoft.Xna.Framework;

namespace FlockBuddy.Tests
{
	public class MoverTests
	{
		[Test]
		public void UpdateHeading90()
		{
			Mover dude = new Mover(Vector2.Zero, 10.0f, new Vector2(0.0f, -1.0f), 100.0f, 1.0f, 100.0f, 4.0f, 100.0f);
			dude.RotateHeading(MathHelper.ToRadians(90.0f));

			Assert.AreEqual(1.0f, Math.Round(dude.Heading.X, 3));
			Assert.AreEqual(0.0f, Math.Round(dude.Heading.Y, 3));
		}

		[Test]
		public void UpdateHeadingMinus90()
		{
			Mover dude = new Mover(Vector2.Zero, 10.0f, new Vector2(0.0f, -1.0f), 100.0f, 1.0f, 100.0f, 4.0f, 100.0f);
			dude.RotateHeading(MathHelper.ToRadians(-90.0f));

			Assert.AreEqual(-1.0f, Math.Round(dude.Heading.X, 3));
			Assert.AreEqual(0.0f, Math.Round(dude.Heading.Y, 3));
		}

		[Test]
		public void UpdateHeading90_1()
		{
			Mover dude = new Mover(Vector2.Zero, 10.0f, new Vector2(-1.0f, 0.0f), 100.0f, 1.0f, 100.0f, 4.0f, 100.0f);
			dude.RotateHeading(MathHelper.ToRadians(90.0f));

			Assert.AreEqual(0.0f, Math.Round(dude.Heading.X, 3));
			Assert.AreEqual(-1.0f, Math.Round(dude.Heading.Y, 3));
		}

		[Test]
		public void UpdateHeadingMinus90_1()
		{
			Mover dude = new Mover(Vector2.Zero, 10.0f, new Vector2(-1.0f, 0.0f), 100.0f, 1.0f, 100.0f, 4.0f, 100.0f);
			dude.RotateHeading(MathHelper.ToRadians(-90.0f));

			Assert.AreEqual(0.0f, Math.Round(dude.Heading.X, 3));
			Assert.AreEqual(1.0f, Math.Round(dude.Heading.Y, 3));
		}

		[Test]
		public void GetNewHeadingMinus90()
		{
			Mover dude = new Mover(Vector2.Zero, 10.0f, new Vector2(0.0f, -1.0f), 100.0f, 1.0f, 100.0f, 4.0f, 100.0f);
			float heading = 0.0f;
			dude.GetAmountToTurn(new Vector2(-1.0f, 0.0f), ref heading);

			heading = MathHelper.ToDegrees(heading);
			heading = (float)Math.Round(heading, 4);

			Assert.AreEqual(-90.0f, heading);
		}

		[Test]
		public void GetNewHeadingMinus90_1()
		{
			Mover dude = new Mover(Vector2.Zero, 10.0f, new Vector2(1.0f, 0.0f), 100.0f, 1.0f, 100.0f, 4.0f, 100.0f);
			float heading = 0.0f;
			dude.GetAmountToTurn(new Vector2(0.0f, -1.0f), ref heading);

			heading = MathHelper.ToDegrees(heading);
			heading = (float)Math.Round(heading, 4);

			Assert.AreEqual(-90.0f, heading);
		}

		[Test]
		public void GetNewHeadingMinus90_2()
		{
			Mover dude = new Mover(Vector2.Zero, 10.0f, new Vector2(0.0f, 1.0f), 100.0f, 1.0f, 100.0f, 4.0f, 100.0f);
			float heading = 0.0f;
			dude.GetAmountToTurn(new Vector2(1.0f, 0.0f), ref heading);

			heading = MathHelper.ToDegrees(heading);
			heading = (float)Math.Round(heading, 4);

			Assert.AreEqual(-90.0f, heading);
		}

		[Test]
		public void GetNewHeadingMinus90_3()
		{
			Mover dude = new Mover(Vector2.Zero, 10.0f, new Vector2(-1.0f, 0.0f), 100.0f, 1.0f, 100.0f, 4.0f, 100.0f);
			float heading = 0.0f;
			dude.GetAmountToTurn(new Vector2(0.0f, 1.0f), ref heading);

			heading = MathHelper.ToDegrees(heading);
			heading = (float)Math.Round(heading, 4);

			Assert.AreEqual(-90.0f, heading);
		}

		[Test]
		public void GetNewHeading90()
		{
			Mover dude = new Mover(Vector2.Zero, 10.0f, new Vector2(0.0f, -1.0f), 100.0f, 1.0f, 100.0f, 4.0f, 100.0f);
			float heading = 0.0f;
			dude.GetAmountToTurn(new Vector2(1.0f, 0.0f), ref heading);

			heading = MathHelper.ToDegrees(heading);
			heading = (float)Math.Round(heading, 4);

			Assert.AreEqual(90.0f, heading);
		}

		[Test]
		public void GetNewHeading90_1()
		{
			Mover dude = new Mover(Vector2.Zero, 10.0f, new Vector2(1.0f, 0.0f), 100.0f, 1.0f, 100.0f, 4.0f, 100.0f);
			float heading = 0.0f;
			dude.GetAmountToTurn(new Vector2(0.0f, 1.0f), ref heading);

			heading = MathHelper.ToDegrees(heading);
			heading = (float)Math.Round(heading, 4);

			Assert.AreEqual(90.0f, heading);
		}

		[Test]
		public void GetNewHeading90_2()
		{
			Mover dude = new Mover(Vector2.Zero, 10.0f, new Vector2(0.0f, 1.0f), 100.0f, 1.0f, 100.0f, 4.0f, 100.0f);
			float heading = 0.0f;
			dude.GetAmountToTurn(new Vector2(-1.0f, 0.0f), ref heading);

			heading = MathHelper.ToDegrees(heading);
			heading = (float)Math.Round(heading, 4);

			Assert.AreEqual(90.0f, heading);
		}

		[Test]
		public void GetNewHeading90_3()
		{
			Mover dude = new Mover(Vector2.Zero, 10.0f, new Vector2(-1.0f, 0.0f), 100.0f, 1.0f, 100.0f, 4.0f, 100.0f);
			float heading = 0.0f;
			dude.GetAmountToTurn(new Vector2(0.0f, -1.0f), ref heading);

			heading = MathHelper.ToDegrees(heading);
			heading = (float)Math.Round(heading, 4);

			Assert.AreEqual(90.0f, heading);
		}
	}
}
