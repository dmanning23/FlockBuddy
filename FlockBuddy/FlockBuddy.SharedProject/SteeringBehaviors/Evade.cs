using Microsoft.Xna.Framework;

namespace FlockBuddy
{
	/// <summary>
	/// this behavior attempts to evade a pursuer
	/// </summary>
	public class Evade : BaseBehavior, IPreyBehavior
	{
		#region Properties

		/// <summary>
		/// A dude chasing this dude
		/// </summary>
		public IMover Pursuer { private get; set; }

		/// <summary>
		/// Used to run away from bad guys
		/// </summary>
		private Flee FleeAction { get; set; }

		#endregion //Properties

		#region Methods

		/// <summary>
		/// Initializes a new instance of the <see cref="FlockBuddy.Evade"/> class.
		/// </summary>
		public Evade(Boid dude)
			: base(dude, EBehaviorType.evade, 1.0f)
		{
			FleeAction = new Flee(dude);
		}

		/// <summary>
		/// Called every fram to get the steering direction from this behavior
		/// </summary>
		/// <returns></returns>
		public override Vector2 GetSteering()
		{
			if (null == Pursuer)
			{
				return Vector2.Zero;
			}

			/* Not necessary to include the check for facing direction this time */

			Vector2 toPursuer = Pursuer.Position - Owner.Position;

			//uncomment the following two lines to have Evade only consider pursuers within a 'threat range'
			if (toPursuer.LengthSquared() > (Owner.QueryRadius * Owner.QueryRadius))
			{
				return Vector2.Zero;
			}

			//the lookahead time is propotional to the distance between the pursuer and the pursuer; 
			//and is inversely proportional to the sum of the agents' velocities
			float lookAheadTime = toPursuer.Length() / 
								   (Owner.MaxSpeed + Pursuer.Speed);

			//now flee away from predicted future position of the pursuer
			FleeAction.AvoidPosition = Pursuer.Position + (Pursuer.Velocity * lookAheadTime);
			return FleeAction.GetSteering() * Weight;
		}

		#endregion //Methods
	}
}