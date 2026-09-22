using Microsoft.Xna.Framework;
using NUnit.Framework;
using Shouldly;
using System.Linq;

namespace FlockBuddy.Tests
{
	/// <summary>
	/// FlockManager's behavior bookkeeping (AddBehavior/RemoveBehavior/SetBehaviorWeight/HasBehavior/
	/// GetAllBehaviors/GetBehaviorWeight) had no direct tests at all. In particular, this covers the
	/// regression the Dictionary&lt;BehaviorType, BehaviorTemplate&gt; refactor was meant to fix:
	/// calling AddBehavior twice for the same type used to leave two tracked entries.
	/// </summary>
	[TestFixture]
	public class FlockManagerBehaviorTests
	{
		FlockManager _manager;

		[SetUp]
		public void Setup()
		{
			_manager = new FlockManager(new Flock());
		}

		[Test]
		public void AddBehavior_CalledTwiceForSameType_RetunesInsteadOfDuplicating()
		{
			_manager.AddBehavior(BehaviorType.Separation, 10f);
			_manager.AddBehavior(BehaviorType.Separation, 20f);

			_manager.Behaviors.Count.ShouldBe(1);
			_manager.GetBehaviorWeight(BehaviorType.Separation).ShouldBe(20f);
		}

		[Test]
		public void RemoveBehavior_RemovesFromManagerBookkeeping()
		{
			_manager.AddBehavior(BehaviorType.Cohesion, 5f);
			_manager.HasBehavior(BehaviorType.Cohesion).ShouldBeTrue();

			_manager.RemoveBehavior(BehaviorType.Cohesion);

			_manager.HasBehavior(BehaviorType.Cohesion).ShouldBeFalse();
		}

		[Test]
		public void RemoveBehavior_ForABehaviorThatWasNeverAdded_DoesNotThrow()
		{
			Should.NotThrow(() => _manager.RemoveBehavior(BehaviorType.Wander));
		}

		[Test]
		public void SetBehaviorWeight_ForABehaviorThatWasNeverAdded_DoesNotThrow()
		{
			Should.NotThrow(() => _manager.SetBehaviorWeight(BehaviorType.Wander, 99f));
			_manager.HasBehavior(BehaviorType.Wander).ShouldBeFalse();
		}

		[Test]
		public void SetBehaviorWeight_RetunesAnAlreadyAddedBehavior()
		{
			_manager.AddBehavior(BehaviorType.Evade, 1f);

			_manager.SetBehaviorWeight(BehaviorType.Evade, 55f);

			_manager.GetBehaviorWeight(BehaviorType.Evade).ShouldBe(55f);
		}

		[Test]
		public void GetAllBehaviors_ReturnsEveryAddedType()
		{
			_manager.AddBehavior(BehaviorType.Separation, 1f);
			_manager.AddBehavior(BehaviorType.Alignment, 1f);

			var all = _manager.GetAllBehaviors().ToList();

			all.ShouldContain(BehaviorType.Separation);
			all.ShouldContain(BehaviorType.Alignment);
			all.Count.ShouldBe(2);
		}
	}
}
