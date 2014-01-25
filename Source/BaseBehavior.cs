using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace FlockBuddy
{
	/// <summary>
	/// This is the base class for all behaviors
	/// </summary>
	public abstract class BaseBehavior
	{
		#region Members

		/// <summary>
		/// The type of this beahvior
		/// </summary>
		public EBehaviorType BehaviorType { get; private set; }

		/// <summary>
		/// binary flag to indicate whether or not a behavior should be active
		/// </summary>
		public bool Active { get; set; }

		/// <summary>
		/// a pointer to the owner of this instance
		/// </summary>
		public Boid Owner { get; set; }

		/// <summary>
		/// is cell space partitioning to be used or not?
		/// </summary>
		public bool CellSpaceOn { get; set; }

		/// <summary>
		/// How much weight to apply to this beahvior
		/// </summary>
		public float Weight { get; set; }

		#endregion //Members

		#region Methods

		/// <summary>
		/// Initializes a new instance of the <see cref="FlockBuddy.BaseBehavior"/> class.
		/// </summary>
		public BaseBehavior(Boid dude, EBehaviorType behaviorType)
		{
			BehaviorType = behaviorType;
			Active = false;
			Weight = 1.0f;
			Owner = dude;
		}

		/// <summary>
		/// Called every fram to get the steering direction from this behavior
		/// Dont call this for inactive behaviors
		/// </summary>
		/// <param name="time"></param>
		/// <returns></returns>
		public abstract Vector2 GetSteering(GameTime time);

		#endregion //Methods
	}
}