using Microsoft.Xna.Framework;

namespace FlockBuddy
{
	/// <summary>
	/// Tthis behavior predicts where an agent will be in time T and seeks towards that point to intercept it.
	/// </summary>
	public class Pursuit : BaseBehavior, IPredatorBehavior
	{
		#region Members

		/// <summary>
		/// A dude we are chasing
		/// </summary>
		private IMover Prey { get; set; }

		/// <summary>
		/// Used to chase dudes
		/// </summary>
		private Seek SeekAction { get; set; }

		/// <summary>
		/// If this is set to true, predators will head straight at prey instead of looking ahead
		/// </summary>
		public bool ViciousPursuit { get; set; }

		#endregion //Members

		#region Methods

		/// <summary>
		/// Initializes a new instance of the <see cref="FlockBuddy.Pursuit"/> class.
		/// </summary>
		public Pursuit(Boid dude)
			: base(dude, EBehaviorType.pursuit, 0.1f)
		{
			SeekAction = new Seek(dude);
			ViciousPursuit = false;
		}

		/// <summary>
		/// this behavior creates a force that steers the agent towards the evader
		/// </summary>
		/// <param name="prey"></param>
		/// <returns></returns>
		public Vector2 GetSteering(IMover prey)
		{
			Prey = prey;
			if (null == Prey)
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
			//if the evader is ahead and facing the agent then we can just seek for the evader's current position.
			Vector2 toEvader = Prey.Position - Owner.Position;

			float relativeHeading = Vector2.Dot(Owner.Heading, Prey.Heading);

			if (ViciousPursuit ||
				((Vector2.Dot(toEvader, Owner.Heading) > 0.0f) &&
				 (relativeHeading < -0.95f)))  //acos(0.95)=18 degs
			{
				return SeekAction.GetSteering(Prey.Position);
			}

			//Not considered ahead so we predict where the evader will be.

			//the lookahead time is propotional to the distance between the evader
			//and the pursuer; and is inversely proportional to the sum of the
			//agent's velocities
			float lookAheadTime = toEvader.Length() / (Owner.MaxSpeed + Prey.Speed);

			//now seek to the predicted future position of the evader
			return SeekAction.GetSteering(Prey.Position + (Prey.Velocity * lookAheadTime)) * Weight;
		}

		#endregion //Methods
	}
}