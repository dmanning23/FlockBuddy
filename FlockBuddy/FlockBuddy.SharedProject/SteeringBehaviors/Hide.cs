using Microsoft.Xna.Framework;

namespace FlockBuddy
{
	/// <summary>
	/// given another agent position to hide from and a list of BaseGameEntitys this
	/// method attempts to put an obstacle between itself and its opponent
	/// </summary>
	public class Hide : BaseBehavior
	{
		#region Members

		#endregion //Members

		#region Methods

		/// <summary>
		/// Initializes a new instance of the <see cref="FlockBuddy.Hide"/> class.
		/// </summary>
		public Hide(Boid dude)
			: base(dude, EBehaviorType.hide, dude.MyFlock.BoidTemplate)
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