using Microsoft.Xna.Framework;

namespace FlockBuddy
{
	/// <summary>
	/// this behavior is similar to seek but it attempts to arrive at the target position with a zero velocity
	/// </summary>
	public class Arrive : BaseBehavior, ITargetPositionBehavior
	{
		#region Properties

		/// <summary>
		/// The target position
		/// </summary>
		public Vector2 TargetPosition { private get; set; }

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
		public Arrive(IBoid dude)
			: base(dude, EBehaviorType.arrive, BoidDefaults.ArriveWeight)
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