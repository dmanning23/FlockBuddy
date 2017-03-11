using Microsoft.Xna.Framework;

namespace FlockBuddy
{
	/// <summary>
	/// this results in a steering force that attempts to steer the vehicle to the center
	/// of the vector connecting two moving agents.
	/// </summary>
	public class Interpose : BaseBehavior, IPreyBehavior, IGuardBehavior
	{
		#region Properties

		public IMover Pursuer { private get; set; }

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
		public Interpose(IBoid dude)
			: base(dude, EBehaviorType.interpose, BoidDefaults.InterposeWeight)
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