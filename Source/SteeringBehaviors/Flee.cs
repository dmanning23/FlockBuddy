using Microsoft.Xna.Framework;

namespace FlockBuddy
{
	/// <summary>
	/// this behavior returns a vector that moves the agent away from a target position
	/// </summary>
	public class Flee : BaseBehavior
	{
		#region Members

		#endregion //Members

		#region Methods

		/// <summary>
		/// Initializes a new instance of the <see cref="FlockBuddy.Flee"/> class.
		/// </summary>
		public Flee(Boid dude):base(dude, EBehaviorType.flee)
		{
		}

		/// <summary>
		/// Called every fram to get the steering direction from this behavior
		/// </summary>
		/// <param name="time"></param>
		/// <returns></returns>
		protected override Vector2 GetSteering(GameTime time)
		{
			return Vector2.Zero;
		}

		#endregion //Methods
	}
}