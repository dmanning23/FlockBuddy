
namespace FlockBuddy
{
	/// <summary>
	/// this behavior makes the agent wander about randomly
	/// </summary>
	public class Wander : BaseBehavior
	{
		#region Members

		#endregion //Members

		#region Methods

		/// <summary>
		/// Initializes a new instance of the <see cref="FlockBuddy.Evade"/> class.
		/// </summary>
		public Wander(Boid dude)
			: base(dude, EBehaviorType.wander)
		{
		}

		#endregion //Methods
	}
}