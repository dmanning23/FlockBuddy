using Microsoft.Xna.Framework;

namespace FlockBuddy
{
	/// <summary>
	/// Flocking behavior for guarding another guy
	/// </summary>
	public class GuardAlignment : BaseBehavior, IGuardBehavior
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
				return 0f;
			}
		}

		#endregion //Properties

		#region Methods

		/// <summary>
		/// Initializes a new instance of the <see cref="FlockBuddy.Alignment"/> class.
		/// </summary>
		public GuardAlignment(IBoid dude)
			: base(dude, EBehaviorType.guard_alignment, BoidDefaults.AlignmentWeight)
		{
		}

		/// <summary>
		/// Called every frame to get the steering direction from this behavior
		/// </summary>
		/// <param name="time"></param>
		/// <returns></returns>
		public override Vector2 GetSteering()
		{
			//used to record the average heading of the neighbors
			Vector2 AverageHeading = Vector2.Zero;

			//if the neighborhood contained one or more vehicles, average their heading vectors.
			if (null != Vip)
			{
				AverageHeading = Vip.Heading;
				AverageHeading -= Owner.Heading;
			}

			//always multiply the return value by the weight
			return AverageHeading * Weight;
		}

		#endregion //Methods
	}
}

