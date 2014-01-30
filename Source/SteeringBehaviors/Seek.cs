using Microsoft.Xna.Framework;

namespace FlockBuddy
{
	/// <summary>
	/// this behavior moves the agent towards a target position
	/// </summary>
	public class Seek : BaseBehavior
	{
		#region Members

		/// <summary>
		/// The target position
		/// </summary>
		Vector2 TargetPos { get; set; }

		#endregion //Members

		#region Methods

		/// <summary>
		/// Initializes a new instance of the <see cref="FlockBuddy.Seek"/> class.
		/// </summary>
		public Seek(Boid dude)
			: base(dude, EBehaviorType.seek)
		{
		}

		/// <summary>
		/// run towards a point
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
			//get the direction to the target position
			Vector2 desiredVelocity = (TargetPos - Owner.Position);
			desiredVelocity.Normalize();

			//move towards the target as fast as possible
			desiredVelocity *= Owner.MaxSpeed;

			return (desiredVelocity - Owner.Velocity) * Weight;
		}

		#endregion //Methods
	}
}