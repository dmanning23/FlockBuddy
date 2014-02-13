using GameTimer;
using MatrixExtensions;
using Microsoft.Xna.Framework;
using System;
using System.Diagnostics;
using Vector2Extensions;

namespace FlockBuddy
{
	/// <summary>
	/// A base class defining an entity that moves. 
	/// The entity has a local coordinate system and members for defining its mass and velocity.
	/// </summary>
	public class Mover : BaseEntity
	{
		#region Members

		/// <summary>
		/// a normalized vector pointing in the direction the entity is heading. 
		/// </summary>
		protected Vector2 _heading;

		/// <summary>
		/// a vector perpendicular to the heading vector
		/// </summary>
		protected Vector2 _side;

		/// <summary>
		/// keeps a track of the most recent update time. 
		/// (some of the steering behaviors make use of this - see Wander)
		/// </summary>
		protected GameClock BoidTimer { get; set; }

		/// <summary>
		/// the speed of this dude in pixels/sec
		/// </summary>
		public float Speed { get; set; }

		#endregion //Members

		#region Properties

		public Vector2 Velocity
		{
			get
			{
				return Heading * Speed;
			}
		}

		public Vector2 Heading
		{
			get
			{
				return _heading;
			}
			set
			{
				//first checks that the given heading is not a vector of zero length. 
				Debug.Assert(!float.IsNaN(value.X));
				Debug.Assert(!float.IsNaN(value.Y));
				Debug.Assert(value.LengthSquared() > 0.0f);

				// If the new heading is valid this fumction sets the entity's heading and side vectors accordingly
				_heading = value;

				//the side vector must always be perpendicular to the heading
				_side = _heading.Perp();
			}
		}

		public Vector2 Side
		{
			get
			{
				return _side;
			}
		}

		public float Mass { get; set; }

		/// <summary>
		/// the maximum speed this entity may travel at.
		/// </summary>
		public float MaxSpeed { get; set; }

		/// <summary>
		/// the maximum force this entity can produce to power itself (think rockets and thrust)
		/// </summary>
		public float MaxForce { get; set; }

		/// <summary>
		/// the maximum rate (radians per second)this vehicle can rotate
		/// </summary>
		public float MaxTurnRate { get; set; }

		#endregion //Properties

		#region Methods

		public Mover(Vector2 position,
					float radius,
					Vector2 heading,
					float speed,
					float mass,
					float max_speed,
					float max_turn_rate,
					float max_force)
			: base(position, radius)
		{
			Heading = heading;
			Speed = speed;
			Mass = mass;
			MaxSpeed = max_speed;
			MaxTurnRate = max_turn_rate;
			MaxForce = max_force;

			BoidTimer = new GameClock();
			BoidTimer.Start();
		}

		public bool IsSpeedMaxedOut()
		{
			return MaxSpeed * MaxSpeed >= SpeedSq();
		}

		public float SpeedSq()
		{
			return (Speed * Speed);
		}

		/// <summary>
		/// given a target position, this method rotates the entity's heading and
		/// side vectors by an amount not greater than m_dMaxTurnRate until it
		/// directly faces the target.
		/// </summary>
		/// <param name="target"></param>
		/// <returns>returns true when the heading is facing in the desired direction</returns>
		public bool RotateHeadingToFacePosition(Vector2 target)
		{
			if (target == Position)
			{
				//we are at the target :P
				return true;
			}
			
			//get the point we are currently going to end up at
			Vector2 currenTarget = Position + (Speed * Heading);

			//first determine the angle between the heading vector and the target
			float angle = Vector2Ext.AngleBetweenVectors(currenTarget, target);
			angle = Math.Abs(angle);

			//return true if the player is facing the target
			if (angle < 0.1)
			{
				return true;
			}

			//clamp the amount to turn to the max turn rate
			angle = MathHelper.Clamp(angle, -MaxTurnRate, MaxTurnRate);
			angle *= BoidTimer.TimeDelta;

			//The next few lines use a rotation matrix to rotate the player's heading vector accordingly
			Matrix RotationMatrix = MatrixExt.Orientation(angle * -Heading.Sign(currenTarget - target));

			//notice how the direction of rotation has to be determined when creating the rotation matrix
			Heading = RotationMatrix.Multiply(Heading);

			return false;
		}

		/// <summary>
		/// Called every frame to update this thing
		/// </summary>
		/// <param name="time_elapsed"></param>
		public override void Update(GameClock curTime)
		{
			base.Update(curTime);

			//update the time elapsed
			BoidTimer.Update(curTime);
		}

		#endregion //Methods
	}
}