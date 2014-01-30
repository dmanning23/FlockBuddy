using Microsoft.Xna.Framework;

namespace FlockBuddy
{
	/// <summary>
	/// this results in a steering force that attempts to steer the vehicle to the center of the vector connecting two moving agents.
	/// </summary>
	public class Interpose : BaseBehavior
	{
		#region Members

		#endregion //Members

		#region Methods

		/// <summary>
		/// Initializes a new instance of the <see cref="FlockBuddy.Evade"/> class.
		/// </summary>
		public Interpose(Boid dude)
			: base(dude, EBehaviorType.interpose)
		{
		}

		/// <summary>
		/// Called every fram to get the steering direction from this behavior
		/// </summary>
		/// <param name="time"></param>
		/// <returns></returns>
		protected override Vector2 GetSteering()
		{
			return Vector2.Zero * Weight;
		}

		#endregion //Methods
	}
}