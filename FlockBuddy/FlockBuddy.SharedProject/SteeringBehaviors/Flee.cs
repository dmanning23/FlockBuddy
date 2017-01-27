using Microsoft.Xna.Framework;

namespace FlockBuddy
{
	/// <summary>
	/// this behavior returns a vector that moves the agent away from a target position
	/// </summary>
	public class Flee : BaseBehavior, IAvoidPositionBehavior
	{
		#region Members

		/// <summary>
		/// The position to run away from!
		/// </summary>
		public Vector2 AvoidPosition { get; private set; }

		/// <summary>
		/// how far out the flee behvaior should watch before panicking
		/// </summary>
		public float PanicDistance { get; set; }

		#endregion //Members

		#region Methods

		/// <summary>
		/// Initializes a new instance of the <see cref="FlockBuddy.Flee"/> class.
		/// </summary>
		public Flee(Boid dude)
			: base(dude, EBehaviorType.flee, dude.MyFlock.BoidTemplate)
		{
			PanicDistance = 100f;
		}

		/// <summary>
		/// run away from a point
		/// </summary>
		/// <param name="target"></param>
		/// <returns></returns>
		public Vector2 GetSteering(Vector2 target)
		{
			AvoidPosition = target;
			return GetSteering();
		}

		/// <summary>
		/// Called every fram to get the steering direction from this behavior
		/// </summary>
		/// <returns></returns>
		public override Vector2 GetSteering()
		{
			//only flee if the target is within 'panic distance'. Work in distance squared space.
			if (Vector2.DistanceSquared(Owner.Position, AvoidPosition) > (PanicDistance * PanicDistance))
			{
				return Vector2.Zero;
			}

			Vector2 desiredVelocity = Vector2.Normalize(Owner.Position - AvoidPosition) * Owner.MaxSpeed;
			return (desiredVelocity - Owner.Velocity) * Weight;
		}

		#endregion //Methods
	}
}