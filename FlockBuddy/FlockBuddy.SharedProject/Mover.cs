using GameTimer;
using MatrixExtensions;
using Microsoft.Xna.Framework;
using PrimitiveBuddy;
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
		/// keeps a track of the most recent update time. 
		/// (some of the steering behaviors make use of this - see Wander)
		/// </summary>
		protected GameClock BoidTimer { get; set; }

		#endregion //Members

		#region Properties

		public virtual Vector2 Heading { get; set; }

		/// <summary>
		/// the speed of this dude in pixels/sec
		/// </summary>
		public virtual float Speed { get; set; }

		public Vector2 Velocity
		{
			get
			{
				return Heading * Speed;
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

		#endregion //Properties

		#region Methods

		public Mover(Vector2 position,
					float radius,
					Vector2 heading,
					float speed)
			: base(position, radius)
		{
			Heading = heading;
			Speed = speed;

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
		public virtual void Draw(IPrimitive prim, Color color)
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