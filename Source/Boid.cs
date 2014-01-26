using GameTimer;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using AverageBuddy;
using System;
using Vector2Extensions;

namespace FlockBuddy
{
	/// <summary>
	/// Definition of a simple vehicle that uses steering behaviors
	/// </summary>
	public class Boid : Mover
	{
		#region Members

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
		public Averager<Vector2> HeadingSmoother { get; private set; }

		/// <summary>
		/// this vector represents the average of the vehicle's heading
		/// vector smoothed over the last few frames
		/// </summary>
		public Vector2 SmoothedHeading { get; set; }

		/// <summary>
		/// when true, smoothing is active
		/// </summary>
		public bool SmoothingOn { get; set; }

		/// <summary>
		/// keeps a track of the most recent update time. 
		/// (some of the steering behaviors make use of this - see Wander)
		/// </summary>
		public GameClock BoidTimer { get; set; }

		/// <summary>
		/// /buffer for the vehicle shape
		/// </summary>
		List<Vector2> VehicleShape;

		#endregion //Properties

		#region Methods

		/// <summary>
		/// Initializes a new instance of the <see cref="FlockBuddy.Boid"/> class.
		/// </summary>
		public Boid(Vector2 position,
			float radius,
			float rotation,
			Vector2 velocity,
			float mass,
			float max_force,
			float max_speed,
			float max_turn_rate)
				: base(position, 
				radius, 
				velocity, 
				max_speed, 
				new Vector2((float)Math.Sin(rotation), (float)-Math.Cos(rotation)), 
				mass, 
				max_turn_rate, 
				max_force)
		{
			VehicleShape = new List<Vector2>();
			SmoothedHeading = Vector2.Zero;
			SmoothingOn = false;
			InitializeBuffer();
			BoidTimer = new GameClock();
			BoidTimer.Start();

			//set up the steering behavior class
			Behaviors = new SteeringBehaviors(this);

			//set up the smoother
			HeadingSmoother = new Averager<Vector2>(10, Vector2.Zero);
		}

		/// <summary>
		/// fills the vehicle's shape buffer with its vertices
		/// </summary>
		protected virtual void InitializeBuffer()
		{
			VehicleShape.Add(new Vector2(-1.0f, 0.6f));
			VehicleShape.Add(new Vector2(1.0f, 0.0f));
			VehicleShape.Add(new Vector2(-1.0f, -0.6f));
		}

		/// <summary>
		/// Updates the vehicle's position from a series of steering behaviors
		/// </summary>
		/// <param name="time_elapsed"></param>
		public override void Update(GameClock time_elapsed)
		{
			//update the time elapsed
			BoidTimer.Update(time_elapsed);

			//keep a record of its old position so we can update its cell later
			//in this method
			Vector2 OldPos = Position;

			//calculate the combined force from each steering behavior in the vehicle's list
			Vector2 SteeringForce = Behaviors.Calculate();

			//Acceleration = Force/Mass
			Vector2 acceleration = SteeringForce / Mass;

			//update velocity
			_velocity += (acceleration * time_elapsed.TimeDelta);

			//make sure vehicle does not exceed maximum velocity
			_velocity = _velocity.Truncate(MaxSpeed);

			//update the position
			Physics.Translate(Velocity * BoidTimer.TimeDelta);

			//update the heading if the vehicle has a non zero velocity
			if (Velocity.LengthSquared() > 0.00000001)
			{
				_heading.Normalize();
				_side = _heading.Perp();
			}

			//EnforceNonPenetrationConstraint(this, World()->Agents());

			//treat the screen as a toroid
			//WrapAround(m_vPos, m_pWorld->cxClient(), m_pWorld->cyClient());

			////update the vehicle's current cell if space partitioning is turned on
			//if (Steering()->isSpacePartitioningOn())
			//{
			//	World()->CellSpace()->UpdateEntity(this, OldPos);
			//}

			if (SmoothingOn)
			{
				SmoothedHeading = HeadingSmoother.Update(Heading);
			}
		}

		#endregion //Methods
	}
}