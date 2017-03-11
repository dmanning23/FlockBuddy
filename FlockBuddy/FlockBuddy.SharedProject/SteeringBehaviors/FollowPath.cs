using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace FlockBuddy
{
	/// <summary>
	/// given a series of Vector2Ds, this method produces a force that will move the agent along the waypoints in order
	/// </summary>
	public class FollowPath : BaseBehavior, IPathBehavior
	{
		#region Properties

		public List<Vector2> Path { private get; set; }

		public override float DirectionChange
		{
			get
			{
				return 1f;
			}
		}

		public override float SpeedChange
		{
			get
			{
				return 1f;
			}
		}

		#endregion //Properties

		#region Methods

		/// <summary>
		/// Initializes a new instance of the <see cref="FlockBuddy.Evade"/> class.
		/// </summary>
		public FollowPath(IBoid dude)
			: base(dude, EBehaviorType.follow_path, BoidDefaults.FollowPathWeight)
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