using Microsoft.Xna.Framework;

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
			: base(dude, EBehaviorType.wander, dude.MyFlock.BoidTemplate)
		{
		}

		/// <summary>
		/// Called every fram to get the steering direction from this behavior
		/// </summary>
		/// <param name="time"></param>
		/// <returns></returns>
		public override Vector2 GetSteering()
		{
			//TODO:
			return Vector2.Zero * Weight;
		}

		#endregion //Methods
	}
}