using Microsoft.Xna.Framework;

namespace FlockBuddy
{
	/// <summary>
	/// This is the base class for all behaviors
	/// </summary>
	public abstract class BaseBehavior : IBehavior
	{
		#region Properties

		/// <summary>
		/// The type of this beahvior
		/// </summary>
		public EBehaviorType BehaviorType { get; private set; }

		/// <summary>
		/// a pointer to the owner of this instance
		/// </summary>
		public IBoid Owner { get; set; }

		/// <summary>
		/// How much weight to apply to this beahvior
		/// </summary>
		public float Weight { get; set; }

		public abstract float DirectionChange { get; }

		public abstract float SpeedChange { get; }

		#endregion //Properties

		#region Methods

		/// <summary>
		/// Initializes a new instance of the <see cref="FlockBuddy.BaseBehavior"/> class.
		/// </summary>
		public BaseBehavior(IBoid owner, EBehaviorType behaviorType, float weight)
		{
			BehaviorType = behaviorType;
			Owner = owner;
			Weight = weight;
		}

		/// <summary>
		/// Called every frame to get the steering direction from this behavior
		/// Dont call this for inactive behaviors
		/// </summary>
		/// <returns></returns>
		public abstract Vector2 GetSteering();

		#endregion //Methods
	}
}