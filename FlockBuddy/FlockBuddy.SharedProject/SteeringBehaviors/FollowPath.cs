using Microsoft.Xna.Framework;

namespace FlockBuddy
{
	/// <summary>
	/// given a series of Vector2Ds, this method produces a force that will move the agent along the waypoints in order
	/// </summary>
	public class FollowPath : BaseBehavior, IPathBehavior
	{
		#region Members

		#endregion //Members

		#region Methods

		/// <summary>
		/// Initializes a new instance of the <see cref="FlockBuddy.Evade"/> class.
		/// </summary>
		public FollowPath(Boid dude)
			: base(dude, EBehaviorType.follow_path, dude.MyFlock.BoidTemplate)
		{
		}

		/// <summary>
		/// Called every fram to get the steering direction from this behavior
		/// </summary>
		/// <returns></returns>
		public override Vector2 GetSteering()
		{
			//TODO:
			return Vector2.Zero * Weight;
		}

		#endregion //Methods
	}
}