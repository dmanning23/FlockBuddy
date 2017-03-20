using Microsoft.Xna.Framework;

namespace FlockBuddy
{
	public class GuardSeparation : BaseBehavior, IGuardBehavior
	{
		#region Properties

		public IMover Vip { private get; set; }

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
				return 0.5f;
			}
		}

		#endregion //Properties

		#region Methods

		/// <summary>
		/// Initializes a new instance of the <see cref="FlockBuddy.Evade"/> class.
		/// </summary>
		public GuardSeparation(IBoid dude)
			: base(dude, EBehaviorType.guard_separation, BoidDefaults.SeparationWeight)
		{
		}

		/// <summary>
		/// Called every fram to get the steering direction from this behavior
		/// </summary>
		/// <returns></returns>
		public override Vector2 GetSteering()
		{
			Vector2 steeringForce = Vector2.Zero;

			if (null != Vip)
			{
				Vector2 toAgent = Owner.Position - Vip.Position;
				float length = toAgent.Length();

				if (length != 0.0f)
				{
					//normalize the vector
					toAgent /= length;

					//scale the force inversely proportional to the agents distance from its neighbor.
					steeringForce += toAgent / length;
				}
			}

			return steeringForce * Weight;
		}

		#endregion //Methods
	}
}
