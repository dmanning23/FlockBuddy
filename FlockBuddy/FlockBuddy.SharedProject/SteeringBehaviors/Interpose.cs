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

		public IMover Vip { private get; set; }

		private Seek _seek;

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
			_seek = new Seek(dude)
			{
				Weight = 1f
			};
		}

		/// <summary>
		/// Called every fram to get the steering direction from this behavior
		/// </summary>
		/// <param name="time"></param>
		/// <returns></returns>
		public override Vector2 GetSteering()
		{
			//if either of these are missing there is no steering from this behavrior
			if (null == Pursuer || null == Vip)
			{
				return Vector2.Zero;
			}

			//first find the midpoint
			var midPoint = (Pursuer.Position + Vip.Position) / 2f;

			//get the time to reach the midpoint
			var timeToMidPoint = (midPoint - Owner.Position).Length() / Owner.MaxSpeed;

			//get the position of pred and prey after timeToMid
			var predPos = Pursuer.Position + (Pursuer.Velocity * timeToMidPoint);
			var preyPos = Vip.Position + (Vip.Velocity * timeToMidPoint);

			//calc the midpoint of those preditced positions
			_seek.TargetPosition = (predPos + predPos) / 2f;

			return _seek.GetSteering() * Weight;
		}

		#endregion //Methods
	}
}