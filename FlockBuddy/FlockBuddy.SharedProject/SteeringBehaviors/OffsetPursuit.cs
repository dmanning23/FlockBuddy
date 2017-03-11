using Microsoft.Xna.Framework;

namespace FlockBuddy
{
	/// <summary>
	/// this behavior maintains a position, in the direction of offset from the target vehicle
	/// </summary>
	public class OffsetPursuit : BaseBehavior, IGuardBehavior
	{
		#region Properties

		public IBaseEntity Vip { private get; set; }

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
		public OffsetPursuit(IBoid dude)
			: base(dude, EBehaviorType.obstacle_avoidance, BoidDefaults.OffsetPursuitWeight)
		{
		}

		/// <summary>
		/// Called every fram to get the steering direction from this behavior
		/// </summary>
		/// <returns></returns>
		public override Vector2 GetSteering()
		{
			//TODO:
			return Vector2.Zero * Weight;
		}

		#endregion //Methods
	}
}