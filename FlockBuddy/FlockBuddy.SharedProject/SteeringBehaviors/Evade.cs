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
		public Evade(IBoid dude)
			: base(dude, EBehaviorType.evade, BoidDefaults.EvadeWeight)
		{
			FleeAction = new Flee(dude)
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
			if (null == Pursuer)
			{
				return Vector2.Zero;
			}

			Vector2 toPursuer = Pursuer.Position - Owner.Position;

			//the lookahead time is propotional to the distance between the pursuer and the pursuer; 
			//and is inversely proportional to the sum of the agents' velocities
			float lookAheadTime = toPursuer.Length() / (Owner.MaxSpeed + Pursuer.Speed);

			//now flee away from predicted future position of the pursuer
			FleeAction.AvoidPosition = Pursuer.Position + (Pursuer.Velocity * lookAheadTime);
			return FleeAction.GetSteering() * Weight;
		}

		#endregion //Methods
	}
}