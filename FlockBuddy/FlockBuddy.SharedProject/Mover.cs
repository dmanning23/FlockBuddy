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
			protected set
			{
				// If the new heading is valid this fumction sets the entity's heading and side vectors accordingly
				_heading = value;

				//the side vector must always be perpendicular to the heading
				_side = _heading.Perp();
			}
		}

		/// <summary>
		/// the speed of this dude in pixels/sec
		/// </summary>
		public virtual float Speed { get; protected set; }

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
		protected float Rotation
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
		/// Given a target direction, either speed up or slow down the guy to get to it
		/// </summary>
		/// <param name="targetHeading"></param>
		protected void UpdateSpeed(Vector2 targetHeading)
		{
			//update the speed but make sure vehicle does not exceed maximum velocity
			Speed = MathHelper.Clamp(Speed + GetSpeedChange(targetHeading), 0.0f, MaxSpeed);
		}

		protected float GetSpeedChange(Vector2 targetHeading)
		{
			//get the dot product of the current heading and the target
			var dotHeading = Vector2.Dot(Heading, targetHeading);

			//get the amount of force that can be applied pre timedelta
			var maxForceDelta = MaxForce * BoidTimer.TimeDelta;

			//if the dot product is less than zero, we want to got the other direction
			if (0 > dotHeading)
			{
				maxForceDelta *= -1f;
			}

			//update the speed but make sure vehicle does not exceed maximum velocity
			return maxForceDelta;
		}

		/// <summary>
		/// given a target position, this method rotates the entity's heading and
		/// side vectors by an amount not greater than m_dMaxTurnRate until it
		/// directly faces the target.
		/// </summary>
		/// <param name="target"></param>
		/// <returns>returns true when the heading is facing in the desired direction</returns>
		protected bool UpdateHeading(Vector2 targetHeading)
		{
			//get the amount to turn towrads the new heading
			float angle = 0.0f;
			if (GetAmountToTurn(targetHeading, ref angle))
			{
				return true;
			}

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
		protected bool GetAmountToTurn(Vector2 targetHeading, ref float angle)
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

			//clamp the amount to turn between the maxturnrate of the timedelta
			var maxTurnRateDelta = MaxTurnRate * BoidTimer.TimeDelta;

			//clamp the amount to turn to the max turn rate
			angle = MathHelper.Clamp(angle, -maxTurnRateDelta, maxTurnRateDelta);
			return false;
		}

		protected static float ClampAngle(float angle)
		{
			//keep the angle between -180 and 180
			while (-MathHelper.Pi > angle)
			{
				angle += MathHelper.TwoPi;
			}

			while (MathHelper.Pi < angle)
			{
				angle -= MathHelper.TwoPi;
			}

			return angle;
		}

		/// <summary>
		/// Given an amount to turn, update the heading
		/// </summary>
		/// <param name="angle"></param>
		/// <returns></returns>
		protected void RotateHeading(float angle)
		{
			//The next few lines use a rotation matrix to rotate the player's heading vector accordingly
			Matrix rotationMatrix = MatrixExt.Orientation(angle);

			//notice how the direction of rotation has to be determined when creating the rotation matrix
			Heading = rotationMatrix.Multiply(Heading);
		}

		#region drawing

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

		#endregion //drawing

		#endregion //Methods
	}
}