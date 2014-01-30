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
		/// Called every fram to get the steering direction from this behavior
		/// </summary>
		/// <returns></returns>
		protected override Vector2 GetSteering()
		{
			return Vector2.Zero * Weight;
		}

		#endregion //Methods
	}
}