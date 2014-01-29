
namespace FlockBuddy
{
	/// <summary>
	/// this returns a steering force which will keep the agent away from any walls it may encounter
	/// </summary>
	public class WallAvoidance : BaseBehavior
	{
		#region Members

		#endregion //Members

		#region Methods

		/// <summary>
		/// Initializes a new instance of the <see cref="FlockBuddy.Evade"/> class.
		/// </summary>
		public WallAvoidance(Boid dude)
			: base(dude, EBehaviorType.wall_avoidance)
		{
		}

		#endregion //Methods
	}
}