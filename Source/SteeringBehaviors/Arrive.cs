
namespace FlockBuddy
{
	/// <summary>
	/// this behavior is similar to seek but it attempts to arrive at the target position with a zero velocity
	/// </summary>
	public class Arrive : BaseBehavior
	{
		#region Members

		#endregion //Members

		#region Methods

		/// <summary>
		/// Initializes a new instance of the <see cref="FlockBuddy.Evade"/> class.
		/// </summary>
		public Arrive(Boid dude)
			: base(dude, EBehaviorType.arrive)
		{
		}

		#endregion //Methods
	}
}