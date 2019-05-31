using Microsoft.Xna.Framework;

namespace FlockBuddy
{
	public class GuardCohesion : BaseBehavior, IGuardBehavior
	{
		#region Properties

		public virtual IMover Vip { protected get; set; }

		private Seek SeekBehavior { get; set; }

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
				return .5f;
			}
		}

		#endregion //Properties

		#region Methods

		/// <summary>
		/// Initializes a new instance of the <see cref="FlockBuddy.Evade"/> class.
		/// </summary>
		public GuardCohesion(IBoid owner)
			: base(owner, EBehaviorType.guard_cohesion, BoidDefaults.CohesionWeight)
		{
			SeekBehavior = new Seek(owner)
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
			//first find the center of mass of all the agents
			Vector2 centerOfMass = Vector2.Zero;
			Vector2 steeringForce = Vector2.Zero;

			if (null != Vip)
			{
				centerOfMass = Vip.Position;

				//now seek towards that position
				SeekBehavior.TargetPosition = centerOfMass;
				steeringForce = SeekBehavior.GetSteering();
			}

			//the magnitude of cohesion is usually much larger than separation or
			//allignment so it usually helps to normalize it.
			if (steeringForce.LengthSquared() > 0.0f)
			{
				steeringForce.Normalize();
			}
			return steeringForce * Weight;
		}

		#endregion //Methods
	}
}
