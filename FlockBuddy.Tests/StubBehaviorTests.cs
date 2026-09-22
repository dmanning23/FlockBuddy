using FlockBuddy.SteeringBehaviors;
using Microsoft.Xna.Framework;
using NUnit.Framework;
using Shouldly;

namespace FlockBuddy.Tests
{
	/// <summary>
	/// Arrive, Wander, OffsetPursuit, and Hide are documented stubs: selectable and weighted, but
	/// GetSteering() should always return Vector2.Zero. Nothing previously pinned that contract down,
	/// so a careless partial implementation could start returning garbage with no test catching it.
	/// </summary>
	[TestFixture]
	public class StubBehaviorTests
	{
		Flock _flock;
		TestBoid _owner;

		[SetUp]
		public void Setup()
		{
			_flock = new Flock();
			_owner = new TestBoid(_flock, Vector2.Zero, 1f, Vector2.UnitX, 0f,
				mass: 1f, minSpeed: 0f, walkSpeed: 0f, maxSpeed: 100f, maxTurnRate: 1f, maxForce: 1f);
		}

		[Test]
		public void Arrive_AlwaysReturnsZero_EvenWithATargetSet()
		{
			var arrive = new Arrive(_owner)
			{
				Weight = 5f,
				TargetPosition = new Vector2(50f, 50f)
			};
			arrive.GetSteering().ShouldBe(Vector2.Zero);
		}

		[Test]
		public void Wander_AlwaysReturnsZero()
		{
			var wander = new Wander(_owner) { Weight = 5f };
			wander.GetSteering().ShouldBe(Vector2.Zero);
		}

		[Test]
		public void OffsetPursuit_AlwaysReturnsZero_EvenWithAVipSet()
		{
			var offsetPursuit = new OffsetPursuit(_owner) { Weight = 5f };
			var vip = new TestMover(new Vector2(50f, 0f), 1f, Vector2.UnitX, 10f, 0f, 0f, 0f, 0f, 0f, 0f);
			offsetPursuit.Vip = vip;
			offsetPursuit.GetSteering().ShouldBe(Vector2.Zero);
		}

		[Test]
		public void Hide_AlwaysReturnsZero_EvenWithAPursuerAndObstaclesSet()
		{
			var hide = new Hide(_owner) { Weight = 5f };
			var pursuer = new TestMover(new Vector2(50f, 0f), 1f, Vector2.UnitX, 10f, 0f, 0f, 0f, 0f, 0f, 0f);
			hide.Pursuer = pursuer;
			hide.Obstacles = new System.Collections.Generic.List<FlockBuddy.Interfaces.IBaseEntity>
			{
				new BaseEntity(new Vector2(25f, 0f), 5f)
			};
			hide.GetSteering().ShouldBe(Vector2.Zero);
		}
	}
}
