using Microsoft.Xna.Framework;

namespace FlockBuddy
{
	/// <summary>
	/// Tthis behavior predicts where an agent will be in time T and seeks towards that point to intercept it.
	/// </summary>
	public class Pursuit : BaseBehavior
	{
		#region Members

		#endregion //Members

		#region Methods

		/// <summary>
		/// Initializes a new instance of the <see cref="FlockBuddy.Pursuit"/> class.
		/// </summary>
		public Pursuit(Boid dude)
			: base(dude, EBehaviorType.pursuit)
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