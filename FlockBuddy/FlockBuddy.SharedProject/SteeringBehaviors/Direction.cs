using FlockBuddy.Interfaces;
using Microsoft.Xna.Framework;

namespace FlockBuddy.SteeringBehaviors
{
	/// <summary>
	/// This is the simplest possible behavior: Just go in this direction.
	/// </summary>
	public class Direction : BaseBehavior
	{
		#region Properties

		public Vector2 SteeringDirection { get; set; }

		public override float DirectionChange => 1f;

		public override float SpeedChange => 1f;

		#endregion //Properties

		#region Methods

		public Direction(IBoid owner) : base(owner,BehaviorType.Direction, BoidDefaults.DirectionWeight)
		{
		}

		public override Vector2 GetSteering()
		{
			return SteeringDirection * Weight;
		}

		#endregion //Methods
	}
}
