using Microsoft.Xna.Framework;

namespace FlockBuddy
{
	/// <summary>
	/// this behavior moves the agent towards a target position
	/// </summary>
	public class Seek : BaseBehavior, ITargetPositionBehavior
	{
		#region Properties

		/// <summary>
		/// The target position
		/// </summary>
		public Vector2 TargetPosition { private get; set; }

		public override float DirectionChange
		{
			get
			{
				return 1f;
			}
		}

		public override float SpeedChange
		{
			get
			{
				return 1f;
			}
		}

		#endregion //Properties

		#region Methods

		/// <summary>
		/// Initializes a new instance of the <see cref="FlockBuddy.Seek"/> class.
		/// </summary>
		public Seek(IBoid dude)
			: base(dude, EBehaviorType.seek, BoidDefaults.SeekWeight)
		{
		}

		/// <summary>
		/// Called every fram to get the steering direction from this behavior
		/// </summary>
		/// <returns></returns>
		public override Vector2 GetSteering()
		{
			//get the direction to the target position
			Vector2 desiredVelocity = (TargetPosition - Owner.Position);
			if (desiredVelocity.LengthSquared() > 0.0f)
			{
				desiredVelocity.Normalize();

				//move towards the target as fast as possible
				desiredVelocity *= Owner.MaxSpeed;
			}

			return (desiredVelocity - Owner.Velocity) * Weight;
		}

		#endregion //Methods
	}
}