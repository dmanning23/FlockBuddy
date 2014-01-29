
namespace FlockBuddy
{
	/// <summary>
	/// given a series of Vector2Ds, this method produces a force that will move the agent along the waypoints in order
	/// </summary>
	public class FollowPath : BaseBehavior
	{
		#region Members

		#endregion //Members

		#region Methods

		/// <summary>
		/// Initializes a new instance of the <see cref="FlockBuddy.Evade"/> class.
		/// </summary>
		public FollowPath(Boid dude)
			: base(dude, EBehaviorType.follow_path)
		{
		}

		#endregion //Methods
	}
}