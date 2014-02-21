using AverageBuddy;
using BasicPrimitiveBuddy;
using CellSpacePartitionLib;
using GameTimer;
using Microsoft.Xna.Framework;
using RectangleFLib;
using System.Collections.Generic;
using System.Diagnostics;
using Vector2Extensions;

namespace FlockBuddy
{
	/// <summary>
	/// Definition of a simple vehicle that uses steering behaviors
	/// </summary>
	public class Boid : Mover
	{
		#region Members

		private Vector2 _smootherHeading = Vector2.UnitX;

		/// <summary>
		/// how far to do a query to calculate neightbors
		/// </summary>
		private const float QueryRadius = 100.0f;

		private Vector2 _force = Vector2.Zero;

		#endregion Members

		#region Properties

		/// <summary>
		/// The list of behaviors this boid will use
		/// </summary>
		public SteeringBehaviors Behaviors { get; private set; }

		/// <summary>
		/// some steering behaviors give jerky looking movement. 
		/// The following members are used to smooth the vehicle's heading
		/// </summary>
		protected Averager<Vector2> HeadingSmoother { get; private set; }

		/// <summary>
		/// this vector represents the average of the vehicle's heading vector smoothed over the last few frames
		/// </summary>
		public Vector2 SmoothedHeading 
		{
			get
			{
				return _smootherHeading;
			}
			set
			{
				_smootherHeading = value;
				_smootherHeading.Normalize();
			}
		}

		/// <summary>
		/// when true, smoothing is active
		/// </summary>
		public bool SmoothingOn { get; set; }

		/// <summary>
		/// the flock that owns this dude
		/// </summary>
		public Flock MyFlock { get; private set; }

		/// <summary>
		/// Get the direction this dude is facing.
		/// Does some ,ath, so don't go crazy with this.  Use for drawing only!
		/// </summary>
		public float Rotation
		{
			get
			{
				return (SmoothingOn ? SmoothedHeading.Angle() : Heading.Angle());
			}
		}

		public Vector2 Force
		{
			get
			{
				return _force;
			}
		}

		#endregion //Properties

		#region Methods

		/// <summary>
		/// Initializes a new instance of the <see cref="FlockBuddy.Boid"/> class.
		/// </summary>
		public Boid(Flock owner,
			Vector2 position,
			float radius,
			Vector2 heading,
			float speed,
			float mass,
			float max_speed,
			float max_turn_rate,
			float max_force)
				: base(position, 
				radius,
				heading, 
				speed,
				mass,
				max_speed, 
				max_turn_rate, 
				max_force)
		{
			MyFlock = owner;
			SmoothedHeading = Vector2.Zero;
			SmoothingOn = false;

			//set up the steering behavior class
			Behaviors = new SteeringBehaviors(this);

			//set up the smoother
			HeadingSmoother = new Averager<Vector2>(10, Vector2.Zero);
		}

		/// <summary>
		/// Updates the vehicle's position from a series of steering behaviors
		/// </summary>
		/// <param name="time_elapsed"></param>
		public override void Update(GameClock time_elapsed)
		{
			base.Update(time_elapsed);

			//Acceleration = Force/Mass
			_force = GetSteeringForce() / Mass;
			_force = _force.Truncate(MaxForce);//TODO: do need this? prioritixzed shoudl already do it
			//acceleration *= BoidTimer.TimeDelta;

			//turn towards that point if the vehicle has a non zero velocity
			RotateHeadingToFacePosition(_force);

			//Set the velocity
			if (SmoothingOn)
			{
				SmoothedHeading = HeadingSmoother.Update(Heading);
			}

			//make sure vehicle does not exceed maximum velocity
			Speed = MathHelper.Clamp(Speed, 0.0f, MaxSpeed);

			//update the position
			Vector2 currentPosition = Position + (Velocity * BoidTimer.TimeDelta);

			//EnforceNonPenetrationConstraint(this, World()->Agents());

			//treat the screen as a toroid
			MyFlock.WrapWorldPosition(ref currentPosition);

			//Update the position
			Position = currentPosition;
			Debug.Assert(!float.IsNaN(Position.X));
			Debug.Assert(!float.IsNaN(Position.Y));
		}

		/// <summary>
		/// Update all the behaviors and calculate the steering force
		/// </summary>
		/// <returns></returns>
		public Vector2 GetSteeringForce()
		{
			//Update the flock
			List<Boid> neighbors = MyFlock.TagNeighbors(this, QueryRadius);

			//update the enemies
			Boid enemy1;
			Boid enemy2;
			MyFlock.FindEnemies(this, out enemy1, out enemy2);

			//update the target dudes
			Boid target;
			MyFlock.FindTarget(this, out target);

			//Update the steering behaviors
			Behaviors.Neighbors = neighbors;
			Behaviors.Enemy1 = enemy1;
			Behaviors.Enemy2 = enemy2;
			Behaviors.Prey = target;

			//calculate the combined force from each steering behavior in the vehicle's list
			return Behaviors.Calculate(BoidTimer);
		}

		/// <summary>
		/// Draw the bounding circle and heading of this boid
		/// </summary>
		/// <param name="prim"></param>
		/// <param name="color"></param>
		public void Render(IBasicPrimitive prim, Color color)
		{
			DrawPhysics(prim, color);
			prim.Line(Position, Position + (BoundingRadius * Heading), color);
		}

		/// <summary>
		/// Draw the detection circle and point out all the neighbors
		/// </summary>
		/// <param name="curTime"></param>
		public void DrawNeigbors(IBasicPrimitive prim)
		{
			//draw the query cells
			MyFlock.CellSpace.RenderCellIntersections(prim, Position, QueryRadius, Color.Green);

			//get the query rectangle
			RectangleF queryRect = CellSpacePartition<Boid>.CreateQueryBox(Position, QueryRadius);
			prim.Rectangle(queryRect, Color.White);

			//get the query circle
			prim.Circle(Position, QueryRadius, Color.White);

			//draw the neighbor dudes
			List<Boid> neighbors = MyFlock.TagNeighbors(this, QueryRadius);
			foreach (Boid neighbor in neighbors)
			{
				prim.Circle(neighbor.Position, neighbor.BoundingRadius, Color.Red);
			}
		}

		public void DrawVectors(IBasicPrimitive prim)
		{
			//draw the current velocity
			prim.Line(Position, Position + Velocity, Color.Black);

			//draw the force being applied
			prim.Line(Position, Position + Force, Color.Yellow);
		}

		#endregion //Methods
	}
}