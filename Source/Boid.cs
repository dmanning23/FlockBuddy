using GameTimer;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using AverageBuddy;
using System;
using Vector2Extensions;
using System.Diagnostics;

namespace FlockBuddy
{
	/// <summary>
	/// Definition of a simple vehicle that uses steering behaviors
	/// </summary>
	public class Boid : Mover
	{
		#region Members

		private Vector2 _smootherHeading = Vector2.UnitX;

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
				return (float)(SmoothingOn ? Math.Atan2(SmoothedHeading.X, SmoothedHeading.Y) : Math.Atan2(Heading.X, Heading.Y));
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
			SmoothingOn = true;

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

			//grab this for later so we can update the cell position
			Vector2 currentPosition = Position;

			//Acceleration = Force/Mass
			Vector2 acceleration = GetSteeringForce() / Mass;
			acceleration.Truncate(MaxForce);//TODO: do need this? prioritixzed shoudl already do it
			//acceleration *= BoidTimer.TimeDelta;

			//get the speed
			float speed = Speed();

			//add the acceleration to the position
			Vector2 desiredPoint = currentPosition + (speed * acceleration);

			//turn towards that point if the vehicle has a non zero velocity
			if (acceleration != Vector2.Zero)
			{
				RotateHeadingToFacePosition(desiredPoint);
				//Heading = desiredPoint - currentPosition;
			}

			_heading.Normalize();

			//Set the velocity
			if (SmoothingOn)
			{
				SmoothedHeading = HeadingSmoother.Update(Heading);
				_velocity = (SmoothedHeading * speed);
			}
			else
			{
				_velocity = (Heading * speed);
			}

			//make sure vehicle does not exceed maximum velocity
			_velocity.Truncate(MaxSpeed);

			//update the position
			currentPosition += (Velocity * BoidTimer.TimeDelta);

			//EnforceNonPenetrationConstraint(this, World()->Agents());

			//treat the screen as a toroid
			MyFlock.WrapWorldPosition(ref currentPosition);

			//Update the position
			Physics.Pos = currentPosition;
			Debug.Assert(!float.IsNaN(Physics.Pos.X));
			Debug.Assert(!float.IsNaN(Physics.Pos.Y));
		}

		/// <summary>
		/// Update all the behaviors and calculate the steering force
		/// </summary>
		/// <returns></returns>
		public Vector2 GetSteeringForce()
		{
			//Update the flock
			List<Boid> neighbors = MyFlock.TagNeighbors(this);

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

		#endregion //Methods
	}
}