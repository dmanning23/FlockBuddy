using Microsoft.Xna.Framework;

namespace FlockBuddy
{
	/// <summary>
	/// this returns a steering force which will attempt to keep the agent away from any obstacles it may encounter
	/// </summary>
	public class ObstacleAvoidance : BaseBehavior
	{
		#region Members

		#endregion //Members

		#region Methods

		/// <summary>
		/// Initializes a new instance of the <see cref="FlockBuddy.ObstacleAvoidance"/> class.
		/// </summary>
		public ObstacleAvoidance(Boid dude)
			: base(dude, EBehaviorType.obstacle_avoidance)
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