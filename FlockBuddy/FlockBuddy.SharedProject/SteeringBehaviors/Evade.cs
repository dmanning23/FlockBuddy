using Microsoft.Xna.Framework;

namespace FlockBuddy
{
	/// <summary>
	/// this behavior attempts to evade a pursuer
	/// </summary>
	public class Evade : BaseBehavior, IPreyBehavior
	{
		#region Members

		/// <summary>
		/// A dude chasing this dude
		/// </summary>
		public IMover Pursuer { get; private set; }

		/// <summary>
		/// how far the evade behvaior should look out for threats
		/// </summary>
		public float ThreatRange { get; set; }

		/// <summary>
		/// Used to run away from bad guys
		/// </summary>
		private Flee FleeAction { get; set; }

		#endregion //Members

		#region Methods

		/// <summary>
		/// Initializes a new instance of the <see cref="FlockBuddy.Evade"/> class.
		/// </summary>
		public Evade(Boid dude)
			: base(dude, EBehaviorType.evade, 1.0f)
		{
			FleeAction = new Flee(dude);
			ThreatRange = 80f;
		}

		/// <summary>
		/// similar to pursuit except the agent Flees from the estimated future position of the pursuer
		/// </summary>
		/// <param name="pursuer"></param>
		/// <returns></returns>
		public Vector2 GetSteering(IMover pursuer)
		{
			Pursuer = pursuer;
			if (null == Pursuer)
			{
				return Vector2.Zero;
			}
			else
			{
				return GetSteering();
			}
		}

		/// <summary>
		/// Called every fram to get the steering direction from this behavior
		/// </summary>
		/// <returns></returns>
		public override Vector2 GetSteering()
		{
			/* Not necessary to include the check for facing direction this time */

			Vector2 toPursuer = Pursuer.Position - Owner.Position;

			//uncomment the following two lines to have Evade only consider pursuers within a 'threat range'
			if (toPursuer.LengthSquared() > (ThreatRange * ThreatRange))
			{
				return Vector2.Zero;
			}

			//the lookahead time is propotional to the distance between the pursuer and the pursuer; 
			//and is inversely proportional to the sum of the agents' velocities
			float lookAheadTime = toPursuer.Length() / 
								   (Owner.MaxSpeed + Pursuer.Speed);

			//now flee away from predicted future position of the pursuer
			return FleeAction.GetSteering(Pursuer.Position + (Pursuer.Velocity * lookAheadTime)) * Weight;
		}

		#endregion //Methods
	}
}