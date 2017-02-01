using CellSpacePartitionLib;
using GameTimer;
using Microsoft.Xna.Framework;
using PrimitiveBuddy;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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

		private Random _rand = new Random();

		#endregion Members

		#region Properties

		/// <summary>
		/// The list of behaviors this boid will use
		/// </summary>
		private List<IBehavior> Behaviors { get; set; }

		/// <summary>
		/// How to add up all the steering behaviors
		/// </summary>
		public ESummingMethod SumMethod { private get; set; }

		/// <summary>
		/// the flock that owns this dude
		/// </summary>
		private IFlock MyFlock { get; set; }

		private Vector2 Force
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

		public float RetargetTime { private get; set; }

		private CountdownTimer RetargetTimer { get; set; }

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

			Behaviors = new List<IBehavior>();

			SumMethod = ESummingMethod.weighted_average;

			RetargetTime = 0.05f;
			RetargetTimer = new CountdownTimer();
			RetargetTimer.Start(RetargetTime);
		}

		public void AddBehavior(IBehavior behavior)
		{
			Behaviors.Add(behavior);
			Behaviors.Sort((x, y) => x.BehaviorType.CompareTo(y.BehaviorType));
		}

		/// <summary>
		/// Asynchronous update method
		/// </summary>
		/// <param name="time"></param>
		/// <returns></returns>
		public Task UpdateAsync(GameClock time)
		{
			//run the update method on a different thread
			return Task.Factory.StartNew(() => { Update(time); });
		}

		/// <summary>
		/// Updates the vehicle's position from a series of steering behaviors
		/// </summary>
		/// <param name="time"></param>
		public override void Update(GameClock time)
		{
			base.Update(time);

			//Acceleration = Force/Mass
			_force = GetSteeringForce() / Mass;
			
			//turn towards that point if the vehicle has a non zero velocity
			UpdateHeading(_force);

			//speed up or slow down depending on whether the target is ahead or behind
			UpdateSpeed(_force);

			//update the position
			Vector2 currentPosition = Position + (Velocity * BoidTimer.TimeDelta);

			//EnforceNonPenetrationConstraint(this, World()->Agents());

			//treat the screen as a toroid
			currentPosition = MyFlock.WrapWorldPosition(currentPosition);

			//Update the position
			Position = currentPosition;
		}

		/// <summary>
		/// Update all the behaviors and calculate the steering force
		/// </summary>
		/// <returns></returns>
		private Vector2 GetSteeringForce()
		{
			RetargetTimer.Update(BoidTimer);

			if (!RetargetTimer.HasTimeRemaining())
			{
				//restart the timer
				RetargetTimer.Start(RetargetTime);

				//Update the flock
				var neighbors = MyFlock.FindBoidsInRange(this, QueryRadius);

				//update the enemies
				var predator = MyFlock.FindClosestPredatorInRange(this, QueryRadius);

				//update the target dudes
				var prey = MyFlock.FindClosestPreyInRange(this, QueryRadius);

				//update the obstacles: the detection box length is proportional to the agent's velocity
				float boxLength = QueryRadius + (Speed / MaxSpeed) * QueryRadius;

				//tag all obstacles within range of the box for processing
				var obstacles = MyFlock.FindObstaclesInRange(this, boxLength);

				foreach (var behavior in Behaviors)
				{
					var flockingBehavior = behavior as IFlockingBehavior;
					if (null != flockingBehavior)
					{
						flockingBehavior.Buddies = neighbors;
					}

					var predatorBehavior = behavior as IPredatorBehavior;
					if (null != predatorBehavior)
					{
						predatorBehavior.Prey = prey;
					}

					var preyBehavior = behavior as IPreyBehavior;
					if (null != preyBehavior)
					{
						preyBehavior.Pursuer = predator;
					}

					var obstacleBehavior = behavior as IObstacleBehavior;
					if (null != obstacleBehavior)
					{
						obstacleBehavior.Obstacles = obstacles;
					}
				}
			}

			//calculate the combined force from each steering behavior in the vehicle's list
			return Calculate();
		}

		/// <summary>
		/// calculates and sums the steering forces from any active behaviors
		/// </summary>
		/// <returns></returns>
		private Vector2 Calculate()
		{
			switch (SumMethod)
			{
				case ESummingMethod.weighted_average:
					{
						return CalculateWeightedSum();
					}
				case ESummingMethod.prioritized:
					{
						return CalculatePrioritized();
					}
				default:
					{
						return CalculateDithered();
					}
			}
		}

		/// <summary>
		/// this simply sums up all the active behaviors X their weights and 
		///  truncates the result to the max available steering force before returning
		/// </summary>
		/// <returns></returns>
		private Vector2 CalculateWeightedSum()
		{
			Vector2 steeringForce = Vector2.Zero;

			for (int i = 0; i < Behaviors.Count; i++)
			{
				steeringForce += Behaviors[i].GetSteering();
			}

			return steeringForce;
		}

		/// <summary>
		/// this method calls each active steering behavior in order of priority
		///  and acumulates their forces until the max steering force magnitude
		///  is reached, at which time the function returns the steering force 
		///  accumulated to that  point
		/// </summary>
		/// <returns></returns>
		private Vector2 CalculatePrioritized()
		{
			Vector2 force;
			Vector2 steeringForce = Vector2.Zero;

			for (int i = 0; i < Behaviors.Count; i++)
			{
				force = Behaviors[i].GetSteering();
				if (!AccumulateForce(ref steeringForce, force))
				{
					return steeringForce;
				}
			}

			return steeringForce;
		}

		/// <summary>
		/// this method sums up the active behaviors by assigning a probabilty
		///  of being calculated to each behavior. It then tests the first priority
		///  to see if it should be calcukated this simulation-step. If so, it
		///  calculates the steering force resulting from this behavior. If it is
		///  more than zero it returns the force. If zero, or if the behavior is
		///  skipped it continues onto the next priority, and so on.
		///
		///  NOTE: Not all of the behaviors have been implemented in this method,
		///        just a few, so you get the general idea
		/// </summary>
		/// <returns></returns>
		private Vector2 CalculateDithered()
		{
			//reset the steering force
			Vector2 steeringForce = Vector2.Zero;

			//if (IsActive(EBehaviorType.wall_avoidance) && RandFloat() < Prm.prWallAvoidance)
			//{
			//	steeringForce = WallAvoidance(m_pVehicle->World()->Walls()) *
			//						 m_dWeightWallAvoidance / Prm.prWallAvoidance;

			//	if (!steeringForce.isZero())
			//	{
			//		steeringForce.Truncate(m_pVehicle->MaxForce());

			//		return steeringForce;
			//	}
			//}

			//if (IsActive(EBehaviorType.obstacle_avoidance) && RandFloat() < Prm.prObstacleAvoidance)
			//{
			//	steeringForce += ObstacleAvoidance(m_pVehicle->World()->Obstacles()) *
			//			m_dWeightObstacleAvoidance / Prm.prObstacleAvoidance;

			//	if (!steeringForce.isZero())
			//	{
			//		steeringForce.Truncate(m_pVehicle->MaxForce());

			//		return steeringForce;
			//	}
			//}


			//if (IsActive(EBehaviorType.separation) && RandFloat() < Prm.prSeparation)
			//{
			//	steeringForce += SeparationPlus(m_pVehicle->World()->Agents()) *
			//						m_dWeightSeparation / Prm.prSeparation;

			//	if (!steeringForce.isZero())
			//	{
			//		steeringForce.Truncate(m_pVehicle->MaxForce());

			//		return steeringForce;
			//	}
			//}



			//if (IsActive(EBehaviorType.flee) && RandFloat() < Prm.prFlee)
			//{
			//	steeringForce += Flee(m_pVehicle->World()->Crosshair()) * m_dWeightFlee / Prm.prFlee;

			//	if (!steeringForce.isZero())
			//	{
			//		steeringForce.Truncate(m_pVehicle->MaxForce());

			//		return steeringForce;
			//	}
			//}

			//if (IsActive(EBehaviorType.evade) && RandFloat() < Prm.prEvade)
			//{
			//	assert(m_pTargetAgent1 && "Evade target not assigned");

			//	steeringForce += Evade(m_pTargetAgent1) * m_dWeightEvade / Prm.prEvade;

			//	if (!steeringForce.isZero())
			//	{
			//		steeringForce.Truncate(m_pVehicle->MaxForce());

			//		return steeringForce;
			//	}
			//}



			//if (IsActive(EBehaviorType.allignment) && RandFloat() < Prm.prAlignment)
			//{
			//	steeringForce += AlignmentPlus(m_pVehicle->World()->Agents()) *
			//						m_dWeightAlignment / Prm.prAlignment;

			//	if (!steeringForce.isZero())
			//	{
			//		steeringForce.Truncate(m_pVehicle->MaxForce());

			//		return steeringForce;
			//	}
			//}

			//if (IsActive(EBehaviorType.cohesion) && RandFloat() < Prm.prCohesion)
			//{
			//	steeringForce += CohesionPlus(m_pVehicle->World()->Agents()) *
			//						m_dWeightCohesion / Prm.prCohesion;

			//	if (!steeringForce.isZero())
			//	{
			//		steeringForce.Truncate(m_pVehicle->MaxForce());

			//		return steeringForce;
			//	}
			//}


			//if (IsActive(EBehaviorType.wander) && RandFloat() < Prm.prWander)
			//{
			//	steeringForce += Wander() * m_dWeightWander / Prm.prWander;

			//	if (!steeringForce.isZero())
			//	{
			//		steeringForce.Truncate(m_pVehicle->MaxForce());

			//		return steeringForce;
			//	}
			//}

			//if (IsActive(EBehaviorType.seek) && RandFloat() < Prm.prSeek)
			//{
			//	steeringForce += Seek(m_pVehicle->World()->Crosshair()) * m_dWeightSeek / Prm.prSeek;

			//	if (!steeringForce.isZero())
			//	{
			//		steeringForce.Truncate(m_pVehicle->MaxForce());

			//		return steeringForce;
			//	}
			//}

			//if (IsActive(EBehaviorType.arrive) && RandFloat() < Prm.prArrive)
			//{
			//	steeringForce += Arrive(m_pVehicle->World()->Crosshair(), m_Deceleration) *
			//						m_dWeightArrive / Prm.prArrive;

			//	if (!steeringForce.isZero())
			//	{
			//		steeringForce.Truncate(m_pVehicle->MaxForce());

			//		return steeringForce;
			//	}
			//}

			return steeringForce;
		}

		/// <summary>
		/// This function calculates how much of its max steering force the 
		///  vehicle has left to apply and then applies that amount of the
		///  force to add.
		/// </summary>
		/// <param name="RunningTot"></param>
		/// <param name="ForceToAdd"></param>
		/// <returns></returns>
		private bool AccumulateForce(ref Vector2 runningTot, Vector2 forceToAdd)
		{
			//calculate how much steering force the vehicle has used so far
			float magnitudeSoFar = runningTot.Length();

			//calculate how much steering force remains to be used by this vehicle
			float magnitudeRemaining = MaxForce - magnitudeSoFar;

			//return false if there is no more force left to use
			if (magnitudeRemaining <= 0.0f)
			{
				return false;
			}

			//calculate the magnitude of the force we want to add
			float MagnitudeToAdd = forceToAdd.Length();

			//if the magnitude of the sum of ForceToAdd and the running total
			//does not exceed the maximum force available to this vehicle, just
			//add together. Otherwise add as much of the ForceToAdd vector is
			//possible without going over the max.
			if (MagnitudeToAdd < magnitudeRemaining)
			{
				runningTot += forceToAdd;
			}
			else
			{
				//add it to the steering force
				forceToAdd.Normalize();
				runningTot += (forceToAdd * magnitudeRemaining);
			}

			return true;
		}

		#region Drawing

		/// <summary>
		/// Draw the bounding circle and heading of this boid
		/// </summary>
		/// <param name="prim"></param>
		/// <param name="color"></param>
		public override void Render(IPrimitive prim, Color color)
		{
			base.Render(prim, color);
		}

		/// <summary>
		/// Draw the detection circle and point out all the neighbors
		/// </summary>
		/// <param name="curTime"></param>
		public override void DrawNeigbors(IPrimitive prim)
		{
			////draw the query cells
			//MyFlock.CellSpace.RenderCellIntersections(prim, Position, QueryRadius, Color.Green);

			////get the query rectangle
			//var queryRect = CellSpacePartition<Boid>.CreateQueryBox(Position, QueryRadius);
			//prim.Rectangle(queryRect, Color.White);

			////get the query circle
			//prim.Circle(Position, QueryRadius, Color.White);

			////draw the neighbor dudes
			//List<IMover> neighbors = MyFlock.FindNeighbors(this, QueryRadius);
			//foreach (var neighbor in neighbors)
			//{
			//	prim.Circle(neighbor.Position, neighbor.Radius, Color.Red);
			//}
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
			var behav = Behaviors.Where(x => x is WallAvoidance).First();

			//draw all the whiskers
			var wallAvoidance = behav as IWallBehavior;
			if (null != wallAvoidance)
			{
				foreach (var whisker in wallAvoidance.Feelers)
				{
					prim.Line(Position, whisker, Color.Aqua);
				}
			}
		}

		#endregion //Drawing

		#endregion //Methods
	}
}