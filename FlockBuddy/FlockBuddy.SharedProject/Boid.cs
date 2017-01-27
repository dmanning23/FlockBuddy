using CellSpacePartitionLib;
using GameTimer;
using Microsoft.Xna.Framework;
using PrimitiveBuddy;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Vector2Extensions;

namespace FlockBuddy
{
	/// <summary>
	/// Definition of a simple vehicle that uses steering behaviors
	/// </summary>
	public class Boid : Mover, IBoid
	{
		#region Members

		private Vector2 _force = Vector2.Zero;

		#endregion Members

		#region Properties

		/// <summary>
		/// The list of behaviors this boid will use
		/// </summary>
		public SteeringBehaviors Behaviors { get; private set; }

		/// <summary>
		/// the flock that owns this dude
		/// </summary>
		public IFlock MyFlock { get; private set; }

		public Vector2 Force
		{
			get
			{
				return _force;
			}
		}

		/// <summary>
		/// how far out to check for neighbors
		/// </summary>
		public float QueryRadius { get; set; }

		#endregion //Properties

		#region Methods

		/// <summary>
		/// Initializes a new instance of the <see cref="FlockBuddy.Boid"/> class.
		/// </summary>
		public Boid(IFlock owner,
			Vector2 position,
			Vector2 heading,
			float speed,
			float radius = 10f,
			float mass = 1f,
			float maxSpeed = 500f,
			float maxTurnRate = 10f,
			float maxForce = 100f)
				: base(position, 
				radius,
				heading, 
				speed,
				mass,
				maxSpeed,
				maxTurnRate,
				maxForce)
		{
			MyFlock = owner;
			MyFlock.AddBoid(this);

			QueryRadius = 100.0f;

			//set up the steering behavior class
			Behaviors = new SteeringBehaviors(this);
		}

		/// <summary>
		/// Asynchronous update method
		/// </summary>
		/// <param name="time_elapsed"></param>
		/// <returns></returns>
		public Task UpdateAsync(GameClock time_elapsed)
		{
			//run the update method on a different thread
			return Task.Factory.StartNew(() => { Update(time_elapsed); });
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

			Debug.Assert(!float.IsNaN(Mass));
			Debug.Assert(!float.IsNaN(_force.X));
			Debug.Assert(!float.IsNaN(_force.Y));

			_force = _force.Truncate(MaxForce);//TODO: do need this? prioritixzed shoudl already do it
			//acceleration *= BoidTimer.TimeDelta;

			//turn towards that point if the vehicle has a non zero velocity
			RotateHeadingToFacePosition(_force);

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
			Behaviors.Neighbors = MyFlock.TagNeighbors(this, MyFlock.BoidTemplate.QueryRadius);

			//update the enemies
			IMover enemy1;
			IMover enemy2;
			MyFlock.FindEnemies(this, out enemy1, out enemy2);

			//update the target dudes
			IMover target;
			MyFlock.FindTarget(this, out target);

			//Update the steering behaviors
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
		public override void Render(IPrimitive prim, Color color)
		{
			base.Render(prim, color);
			
			//draw the target
			if (null != Behaviors.Prey)
			{
				Behaviors.Prey.Render(prim, Color.Yellow);
			}
		}

		/// <summary>
		/// Draw the detection circle and point out all the neighbors
		/// </summary>
		/// <param name="curTime"></param>
		public override void DrawNeigbors(IPrimitive prim)
		{
			//draw the query cells
			MyFlock.CellSpace.RenderCellIntersections(prim, Position, MyFlock.BoidTemplate.QueryRadius, Color.Green);

			//get the query rectangle
			var queryRect = CellSpacePartition<Boid>.CreateQueryBox(Position, MyFlock.BoidTemplate.QueryRadius);
			prim.Rectangle(queryRect, Color.White);

			//get the query circle
			prim.Circle(Position, MyFlock.BoidTemplate.QueryRadius, Color.White);

			//draw the neighbor dudes
			List<IMover> neighbors = MyFlock.TagNeighbors(this, MyFlock.BoidTemplate.QueryRadius);
			foreach (var neighbor in neighbors)
			{
				prim.Circle(neighbor.Position, neighbor.Radius, Color.Red);
			}
		}

		public void DrawVectors(IPrimitive prim)
		{
			//draw the current velocity
			prim.Line(Position, Position + Velocity, Color.Black);

			//draw the force being applied
			prim.Line(Position, Position + Force, Color.Yellow);
		}

		public void DrawWallFeelers(IPrimitive prim)
		{
			//get the wall avoidance steering behavior
			WallAvoidance behav = Behaviors.Behaviors[(int)EBehaviorType.wall_avoidance] as WallAvoidance;

			//draw all the whiskers
			foreach (var whisker in behav.Feelers)
			{
				prim.Line(Position, whisker, Color.Aqua);
			}
		}

		#endregion //Methods
	}
}