using Microsoft.Xna.Framework;

namespace FlockBuddy
{
	/// <summary>
	/// this behavior attempts to evade a pursuer
	/// </summary>
	public class Evade : BaseBehavior
	{
		#region Members

		#endregion //Members

		#region Methods

		/// <summary>
		/// Initializes a new instance of the <see cref="FlockBuddy.Evade"/> class.
		/// </summary>
		public Evade(Boid dude)
			: base(dude, EBehaviorType.evade)
		{
		}

		/// <summary>
		/// Called every fram to get the steering direction from this behavior
		/// </summary>
		/// <param name="time"></param>
		/// <returns></returns>
		public override Vector2 GetSteering(GameTime time)
		{
			return Vector2.Zero;
		}

		#endregion //Methods
	}
}