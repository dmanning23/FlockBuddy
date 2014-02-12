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

		protected Vector2 _velocity;

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

		#endregion //Members

		#region Properties

		public Vector2 Velocity
		{
			get
			{
				return _velocity;
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
			_velocity = heading * speed;
			Mass = mass;
			MaxSpeed = max_speed;
			MaxTurnRate = max_turn_rate;
			MaxForce = max_force;

			BoidTimer = new GameClock();
			BoidTimer.Start();
		}

		public bool IsSpeedMaxedOut()
		{
			return MaxSpeed * MaxSpeed >= Velocity.LengthSquared();
		}

		public float Speed()
		{
			return Velocity.Length();
		}

		public float SpeedSq()
		{
			return Velocity.LengthSquared();
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

			Vector2 toTarget = Vector2.Subtract(target, Position);
			toTarget.Normalize();

			//first determine the angle between the heading vector and the target
			float dotP = Vector2.Dot(_heading, toTarget);
			dotP = MathHelper.Clamp(dotP, -1.0f, 1.0f);
			float angle = (float)Math.Acos(dotP);

			//return true if the player is facing the target
			if (angle < 0.00001)
			{
				return true;
			}

			//clamp the amount to turn to the max turn rate
			angle = MathHelper.Clamp(dotP, -MaxTurnRate, MaxTurnRate);
			angle *= BoidTimer.TimeDelta;

			//The next few lines use a rotation matrix to rotate the player's heading vector accordingly
			Matrix RotationMatrix = MatrixExt.Orientation(angle * Heading.Sign(toTarget));

			//notice how the direction of rotation has to be determined when creating the rotation matrix
			Heading = RotationMatrix.Multiply(Heading);
			//_velocity = RotationMatrix.Multiply(Velocity);

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