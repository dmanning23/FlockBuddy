
namespace FlockBuddy
{
	/// <summary>
	/// This is the base class for all behaviors
	/// </summary>
	public class BaseBehavior
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

		#endregion //Members

		#region Methods

		/// <summary>
		/// Initializes a new instance of the <see cref="FlockBuddy.BaseBehavior"/> class.
		/// </summary>
		public BaseBehavior(EBehaviorType behaviorType)
		{
			BehaviorType = behaviorType;
			Active = false;
		}

		#endregion //Methods
	}
}