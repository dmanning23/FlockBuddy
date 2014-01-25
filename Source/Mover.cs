using Microsoft.Xna.Framework;
using System.Diagnostics;
using System;
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
					Vector2 velocity,
					float max_speed,
					Vector2 heading,
					float mass,
					Vector2 scale,
					float turn_rate,
					float max_force)
			: base(0, position, radius)
		{
			_heading = heading;
			_velocity = velocity;
			Mass = mass;
			_side = _heading.Perp();
			MaxSpeed = max_speed;
			MaxTurnRate = turn_rate;
			MaxForce = max_force;
			_scale = scale;
		}

		//accessors

		bool IsSpeedMaxedOut()
		{
			return MaxSpeed * MaxSpeed >= Velocity.LengthSquared();
		}

		float Speed()
		{
			return Velocity.Length();
		}

		float SpeedSq()
		{
			return Velocity.LengthSquared();
		}



		//------------------------- SetHeading ----------------------------------------
		//
		//  first checks that the given heading is not a vector of zero length. If the
		//  new heading is valid this fumction sets the entity's heading and side 
		//  vectors accordingly
		//-----------------------------------------------------------------------------
		public void SetHeading(Vector2 new_heading)
		{
			Debug.Assert((new_heading.LengthSquared() - 1.0) < 0.00001);

			_heading = new_heading;

			//the side vector must always be perpendicular to the heading
			_side = _heading.Perp();
		}

		//--------------------------- RotateHeadingToFacePosition ---------------------
		//
		//  given a target position, this method rotates the entity's heading and
		//  side vectors by an amount not greater than m_dMaxTurnRate until it
		//  directly faces the target.
		//
		//  returns true when the heading is facing in the desired direction
		//-----------------------------------------------------------------------------
		public bool RotateHeadingToFacePosition(Vector2 target)
		{
			Vector2 toTarget = Vector2.Subtract(target, Position);
			toTarget.Normalize();

			//first determine the angle between the heading vector and the target
			float angle = (float)Math.Acos(Vector2.Dot(_heading, toTarget));

			//return true if the player is facing the target
			if (angle < 0.00001) return true;

			//clamp the amount to turn to the max turn rate
			if (angle > MaxTurnRate) angle = MaxTurnRate;

			//The next few lines use a rotation matrix to rotate the player's heading
			//vector accordingly
			Matrix RotationMatrix;

			//notice how the direction of rotation has to be determined when creating
			//the rotation matrix
			RotationMatrix.Rotate(angle * Heading.Sign(toTarget));
			RotationMatrix.TransformVector2Ds(Heading);
			RotationMatrix.TransformVector2Ds(Velocity);

			//finally recreate m_vSide
			_side = Heading.Perp();

			return false;
		}

		#endregion //Methods
	}
}