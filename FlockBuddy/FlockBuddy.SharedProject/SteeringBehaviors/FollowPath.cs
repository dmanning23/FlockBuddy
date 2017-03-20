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

		private int CurrentWaypoint { get; set; }

		private List<Vector2> _path;
		public List<Vector2> Path
		{
			private get
			{
				return _path;
			}
			set
			{
				_path = value;
				CurrentWaypoint = 0;
			}
		}

		private Seek SeekBehavior { get; set; }

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
			Path = new List<Vector2>();
			SeekBehavior = new Seek(dude)
			{
				Weight = 1f
			};
		}

		/// <summary>
		/// Called every fram to get the steering direction from this behavior
		/// </summary>
		/// <returns></returns>
		public override Vector2 GetSteering()
		{
			//are there any waypoints, or at the end of the path?
			if (null == Path || Path.Count == 0 || (CurrentWaypoint >= Path.Count))
			{
				return Vector2.Zero;
			}

			//get the vector to the current target
			var targetVect = Path[CurrentWaypoint] - Owner.Position;

			//move at the next target if we've reached this one
			if (targetVect.LengthSquared() >= (Owner.WaypointQueryRadius * Owner.WaypointQueryRadius))
			{
				CurrentWaypoint++;
			}

			//check if that puts us at the end
			if (CurrentWaypoint >= Path.Count)
			{
				return Vector2.Zero;
			}
			else
			{
				SeekBehavior.TargetPosition = Path[CurrentWaypoint];
				return SeekBehavior.GetSteering() * Weight;
			}
		}

		#endregion //Methods
	}
}