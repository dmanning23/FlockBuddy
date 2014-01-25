using Microsoft.Xna.Framework;

namespace FlockBuddy
{
	/// <summary>
	/// This is the Seek steering class
	/// this behavior moves the agent towards a target position
	/// </summary>
	public class Seek : BaseBehavior
	{
		#region Members

		/// <summary>
		/// The target position
		/// </summary>
		Vector2 TargetPos { get; set; }

		#endregion //Members

		#region Methods

		/// <summary>
		/// Initializes a new instance of the <see cref="FlockBuddy.Seek"/> class.
		/// </summary>
		public Seek() : base(EBehaviorType.seek)
		{
		}

		#endregion //Methods
	}
}