using Microsoft.Xna.Framework;

namespace FlockBuddy
{
	/// <summary>
	/// this behavior returns a vector that moves the agent away from a target position
	/// </summary>
	public class Flee : BaseBehavior
	{
		#region Members

		/// <summary>
		/// The position to run away from!
		/// </summary>
		public Vector2 TargetPos;

		/// <summary>
		/// How far to look out for bad guys
		/// </summary>
		const float PanicDistance = 100.0f;

		#endregion //Members

		#region Methods

		/// <summary>
		/// Initializes a new instance of the <see cref="FlockBuddy.Flee"/> class.
		/// </summary>
		public Flee(Boid dude):base(dude, EBehaviorType.flee)
		{
		}

		/// <summary>
		/// run away from a point
		/// </summary>
		/// <param name="target"></param>
		/// <returns></returns>
		public Vector2 GetSteering(Vector2 target)
		{
			TargetPos = target;
			return GetSteering();
		}

		/// <summary>
		/// Called every fram to get the steering direction from this behavior
		/// </summary>
		/// <returns></returns>
		protected override Vector2 GetSteering()
		{
			//only flee if the target is within 'panic distance'. Work in distance squared space.
			if (Vector2.DistanceSquared(Owner.Position, TargetPos) > (PanicDistance * PanicDistance))
			{
				return Vector2.Zero;
			}

			Vector2 desiredVelocity = Vector2.Normalize(Owner.Position - TargetPos) * Owner.MaxSpeed;
			return (desiredVelocity - Owner.Velocity) * Weight;
		}

		#endregion //Methods
	}
}