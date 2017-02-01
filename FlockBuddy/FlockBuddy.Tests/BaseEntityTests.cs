using Microsoft.Xna.Framework;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlockBuddy.Tests
{
	[TestFixture]
	public class BaseEntityTests
	{
		[Test]
		public void EquivalenceTest()
		{
			var dude1 = new BaseEntity(Vector2.Zero, 10f);
			var dude2 = dude1;
			Assert.AreEqual(dude1, dude2);
		}

		[Test]
		public void NonEquivalenceTest()
		{
			var dude1 = new BaseEntity(Vector2.Zero, 10f);
			var dude2 = new BaseEntity(Vector2.Zero, 10f);
			Assert.AreNotEqual(dude1, dude2);
		}

		[Test]
		public void EquivalenceTest_interface()
		{
			var dude1 = new BaseEntity(Vector2.Zero, 10f);
			IBaseEntity dude2 = dude1;
			Assert.AreEqual(dude1, dude2);
		}

		[Test]
		public void SetRadius()
		{
			var dude1 = new BaseEntity(Vector2.Zero, 10f);
			Assert.AreEqual(10f, dude1.Radius);
		}

		[Test]
		public void SetPosition()
		{
			var dude1 = new BaseEntity(Vector2.UnitX, 10f);
			Assert.AreEqual(Vector2.UnitX, dude1.Position);
		}

		[Test]
		public void SetOldPosition()
		{
			var dude1 = new BaseEntity(Vector2.UnitX, 10f);
			Assert.AreEqual(Vector2.UnitX, dude1.OldPosition);
		}

		[Test]
		public void UpdatePosition()
		{
			var dude1 = new BaseEntity(Vector2.Zero, 10f);
			Assert.AreEqual(Vector2.Zero, dude1.OldPosition);
			dude1.Position = Vector2.UnitX;
			Assert.AreEqual(Vector2.Zero, dude1.OldPosition);
			dude1.Position = Vector2.UnitY;
			Assert.AreEqual(Vector2.UnitX, dude1.OldPosition);
		}
	}
}
