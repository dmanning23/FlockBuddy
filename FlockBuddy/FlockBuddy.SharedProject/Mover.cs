using GameTimer;
using MatrixExtensions;
using Microsoft.Xna.Framework;
using PrimitiveBuddy;
using System;
using System.Diagnostics;
using Vector2Extensions;

namespace FlockBuddy
{
	/// <summary>
	/// A base class defining an entity that moves. 
	/// The entity has a local coordinate system and members for defining its mass and velocity.
	/// </summary>
	public class Mover : BaseEntity, IMover
	{
		#region Members

		/// <summary>
		/// a normalized vector pointing in the direction the entity is heading. 
		/// </summary>
		private Vector2 _heading;

		/// <summary>
		/// a vector perpendicular to the heading vector
		/// </summary>
		private Vector2 _side;

		/// <summary>
		/// keeps a track of the most recent update time. 
		/// (some of the steering behaviors make use of this - see Wander)
		/// </summary>
		protected GameClock BoidTimer { get; set; }

		#endregion //Members

		#region Properties

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

		/// <summary>
		/// the speed of this dude in pixels/sec
		/// </summary>
		public float Speed { get; set; }

		public Vector2 Velocity
		{
			get
			{
				return Heading * Speed;
			}
		}

		public Vector2 Side
		{
			get
			{
				return _side;
			}
		}

		/// <summary>
		/// Get the direction this dude is facing.
		/// </summary>
		public float Rotation
		{
			get
			{
				return Heading.Angle();
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
					float maxSpeed,
					float maxTurnRate,
					float maxForce)
			: base(position, radius)
		{
			Heading = heading;
			Speed = speed;
			Mass = mass;
			MaxSpeed = maxSpeed;
			MaxTurnRate = maxTurnRate;
			MaxForce = maxForce;

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
		public bool RotateHeadingToFacePosition(Vector2 targetHeading)
		{
			Debug.Assert(!float.IsNaN(targetHeading.X));
			Debug.Assert(!float.IsNaN(targetHeading.Y));

			//get the amount to turn towrads the new heading
			float angle = 0.0f;
			if (GetAmountToTurn(targetHeading, ref angle))
			{
				return true;
			}

			angle *= BoidTimer.TimeDelta;

			//update the heading
			RotateHeading(angle);

			return false;
		}

		/// <summary>
		/// Given a target heading, figure out how much to turn towards that heading.
		/// </summary>
		/// <param name="targetHeading"></param>
		/// <param name="angle"></param>
		/// <returns>true if this dude's heading doesnt need to be updated.</returns>
		public bool GetAmountToTurn(Vector2 targetHeading, ref float angle)
		{
			if (targetHeading.LengthSquared() == 0.0f)
			{
				//we are at the target :P
				return true;
			}

			//first determine the angle between the heading vector and the target
			angle = Vector2Ext.AngleBetweenVectors(Heading, targetHeading);
			angle = ClampAngle(angle);

			//return true if the player is facing the target
			if (Math.Abs(angle) < 0.001f)
			{
				return true;
			}

			//clamp the amount to turn to the max turn rate
			angle = MathHelper.Clamp(angle, -MaxTurnRate, MaxTurnRate);
			return false;
		}

		public static float ClampAngle(float fAngle)
		{
			//keep the angle between -180 and 180
			while (-MathHelper.Pi > fAngle)
			{
				fAngle += MathHelper.TwoPi;
			}

			while (MathHelper.Pi < fAngle)
			{
				fAngle -= MathHelper.TwoPi;
			}

			return fAngle;
		}

		/// <summary>
		/// Given an amount to turn, update the heading
		/// </summary>
		/// <param name="fAngle"></param>
		/// <returns></returns>
		public void RotateHeading(float fAngle)
		{
			Debug.Assert(!float.IsNaN(fAngle));

			//The next few lines use a rotation matrix to rotate the player's heading vector accordingly
			Matrix RotationMatrix = MatrixExt.Orientation(fAngle);

			//notice how the direction of rotation has to be determined when creating the rotation matrix
			Heading = RotationMatrix.Multiply(Heading);
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

		/// <summary>
		/// Draw the bounding circle and heading of this boid
		/// </summary>
		/// <param name="prim"></param>
		/// <param name="color"></param>
		public virtual void Render(IPrimitive prim, Color color)
		{
			DrawPhysics(prim, color);
			prim.Line(Position, Position + (Radius * Heading), color);
		}

		/// <summary>
		/// Draw the detection circle and point out all the neighbors
		/// </summary>
		/// <param name="curTime"></param>
		public virtual void DrawNeigbors(IPrimitive prim)
		{
		}

		#endregion //Methods
	}
}