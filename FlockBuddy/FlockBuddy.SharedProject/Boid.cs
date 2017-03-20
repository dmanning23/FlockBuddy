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

		private Vector2 _totalForce = Vector2.Zero;

		private Vector2 _directionForce = Vector2.Zero;

		private Vector2 _speedForce = Vector2.Zero;

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
		public ESummingMethod SummingMethod { private get; set; }

		/// <summary>
		/// the flock that owns this dude
		/// </summary>
		private IFlock MyFlock { get; set; }

		public float RetargetTime { private get; set; }

		private CountdownTimer RetargetTimer { get; set; }

		/// <summary>
		/// how far out to check for neighbors
		/// </summary>
		public float NeighborsQueryRadius { get; set; }

		/// <summary>
		/// how far out to watch for predators
		/// </summary>
		public float PredatorsQueryRadius { get; set; }

		/// <summary>
		/// how far to watch out for prey
		/// </summary>
		public float PreyQueryRadius { get; set; }

		/// <summary>
		/// how far to watch out or vips to guard
		/// </summary>
		public float VipQueryRadius { get; set; }

		public float ObstacleQueryRadius { get; set; }

		public float WallQueryRadius { get; set; }

		public float WaypointQueryRadius { get; set; }

		#endregion //Properties

		#region Methods

		/// <summary>
		/// Initializes a new instance of the <see cref="FlockBuddy.Boid"/> class.
		/// </summary>
		public Boid(IFlock owner,
			Vector2 position,
			Vector2 heading,
			float speed,
			float radius = BoidDefaults.BoidRadius,
			float mass = BoidDefaults.BoidMass,
			float minSpeed = BoidDefaults.BoidMinSpeed,
			float walkSpeed = BoidDefaults.BoidWalkSpeed,
			float maxSpeed = BoidDefaults.BoidMaxSpeed,
			float maxTurnRate = BoidDefaults.BoidMaxTurnRate,
			float maxForce = BoidDefaults.BoidMaxForce,
			float retargetTime = BoidDefaults.BoidRetargetTime,
			ESummingMethod summingMethod = BoidDefaults.SummingMethod)
				: base(position,
				radius,
				heading,
				speed,
				mass,
				minSpeed,
				walkSpeed,
				maxSpeed,
				maxTurnRate,
				maxForce)
		{
			MyFlock = owner;
			MyFlock.AddBoid(this);

			NeighborsQueryRadius = BoidDefaults.BoidQueryRadius;
			PredatorsQueryRadius = BoidDefaults.BoidQueryRadius;
			PreyQueryRadius = BoidDefaults.BoidQueryRadius;
			VipQueryRadius = BoidDefaults.BoidQueryRadius;
			ObstacleQueryRadius = BoidDefaults.BoidQueryRadius;
			WallQueryRadius = BoidDefaults.BoidQueryRadius;
			WaypointQueryRadius = BoidDefaults.BoidQueryRadius;

			Behaviors = new List<IBehavior>();

			SummingMethod = summingMethod;

			RetargetTime = retargetTime;
			RetargetTimer = new CountdownTimer();
			RetargetTimer.Start(RetargetTime);
		}

		public void AddBehavior(EBehaviorType behaviorType, float weight)
		{
			//first check if we alreadu have that behavior
			if (!Behaviors.Exists(x => x.BehaviorType == behaviorType))
			{
				IBehavior behavior;
				switch (behaviorType)
				{
					case EBehaviorType.wall_avoidance: { behavior = new WallAvoidance(this); } break;
					case EBehaviorType.obstacle_avoidance: { behavior = new ObstacleAvoidance(this); } break;
					case EBehaviorType.evade: { behavior = new Evade(this); } break;
					case EBehaviorType.flee: { behavior = new Flee(this); } break;
					case EBehaviorType.separation: { behavior = new Separation(this); } break;
					case EBehaviorType.alignment: { behavior = new Alignment(this); } break;
					case EBehaviorType.cohesion: { behavior = new Cohesion(this); } break;
					case EBehaviorType.seek: { behavior = new Seek(this); } break;
					case EBehaviorType.arrive: { behavior = new Arrive(this); } break;
					case EBehaviorType.wander: { behavior = new Wander(this); } break;
					case EBehaviorType.pursuit: { behavior = new Pursuit(this); } break;
					case EBehaviorType.offset_pursuit: { behavior = new OffsetPursuit(this); } break;
					case EBehaviorType.interpose: { behavior = new Interpose(this); } break;
					case EBehaviorType.hide: { behavior = new Hide(this); } break;
					case EBehaviorType.follow_path: { behavior = new FollowPath(this); } break;
					case EBehaviorType.guard_alignment: { behavior = new GuardAlignment(this); } break;
					case EBehaviorType.guard_cohesion: { behavior = new GuardCohesion(this); } break;
					case EBehaviorType.guard_separation: { behavior = new GuardSeparation(this); } break;
					default: { throw new NotImplementedException(string.Format("Unhandled EBehaviorType: {0}", behaviorType)); }
				}

				behavior.Weight = weight;
				AddBehavior(behavior);
			}
			else
			{
				//we already have that behavior, just update the weight
				var behavior = Behaviors.Where(x => x.BehaviorType == behaviorType).First();
				behavior.Weight = weight;
			}
		}

		public void AddBehavior(IBehavior behavior)
		{
			Behaviors.Add(behavior);
			Behaviors.Sort((x, y) => x.BehaviorType.CompareTo(y.BehaviorType));
		}

		public void RemoveBehavior(EBehaviorType behaviorType)
		{
			var behavior = Behaviors.Where(x => x.BehaviorType == behaviorType).FirstOrDefault();
			if (behavior != null)
			{
				Behaviors.Remove(behavior);
			}
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
			GetForces();

			//speed up or slow down depending on whether the target is ahead or behind
			UpdateSpeed(_speedForce);

			//turn towards that point if the vehicle has a non zero velocity
			UpdateHeading(_directionForce);

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
		private void GetForces()
		{
			RetargetTimer.Update(BoidTimer);

			if (!RetargetTimer.HasTimeRemaining())
			{
				//restart the timer
				RetargetTimer.Start(RetargetTime);

				//Update the flock
				var neighbors = MyFlock.FindBoidsInRange(this, NeighborsQueryRadius);

				//update the enemies
				var predator = MyFlock.FindClosestPredatorInRange(this, PredatorsQueryRadius);

				//update the target dudes
				var prey = MyFlock.FindClosestPreyInRange(this, PreyQueryRadius);

				var vip = MyFlock.FindClosestVipInRange(this, VipQueryRadius);

				//update the obstacles: the detection box length is proportional to the agent's velocity
				float boxLength = ObstacleQueryRadius + (Speed / MaxSpeed) * ObstacleQueryRadius;

				//tag all obstacles within range of the box for processing
				var obstacles = MyFlock.FindObstaclesInRange(this, boxLength);

				//add all the walls
				var walls = MyFlock.Walls;

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

					var guardBehavior = behavior as IGuardBehavior;
					if (null != guardBehavior)
					{
						guardBehavior.Vip = vip;
					}

					var obstacleBehavior = behavior as IObstacleBehavior;
					if (null != obstacleBehavior)
					{
						obstacleBehavior.Obstacles = obstacles;
					}

					var wallBehavior = behavior as IWallBehavior;
					if (null != wallBehavior)
					{
						wallBehavior.Walls = walls;
					}
				}
			}

			//calculate the combined force from each steering behavior in the vehicle's list
			Calculate();
		}

		/// <summary>
		/// calculates and sums the steering forces from any active behaviors
		/// </summary>
		/// <returns></returns>
		private void Calculate()
		{
			switch (SummingMethod)
			{
				case ESummingMethod.weighted_average: { CalculateWeightedSum(); } break;
				case ESummingMethod.prioritized: { CalculatePrioritized(); } break;
				default: { CalculateDithered(); } break;
			}
		}

		/// <summary>
		/// this simply sums up all the active behaviors X their weights and 
		///  truncates the result to the max available steering force before returning
		/// </summary>
		/// <returns></returns>
		private void CalculateWeightedSum()
		{
			_totalForce = Vector2.Zero;
			_directionForce = Vector2.Zero;
			_speedForce = Vector2.Zero;

			for (int i = 0; i < Behaviors.Count; i++)
			{
				var steeringForce = Behaviors[i].GetSteering();

				_totalForce += steeringForce;
				_directionForce += steeringForce * Behaviors[i].DirectionChange;
				_speedForce += steeringForce * Behaviors[i].SpeedChange;
			}

			//divide all forces by mass
			_totalForce /= Mass;
			_directionForce /= Mass;
			_speedForce /= Mass;
		}

		/// <summary>
		/// this method calls each active steering behavior in order of priority
		///  and acumulates their forces until the max steering force magnitude
		///  is reached, at which time the function returns the steering force 
		///  accumulated to that  point
		/// </summary>
		/// <returns></returns>
		private void CalculatePrioritized()
		{
			_totalForce = Vector2.Zero;
			_directionForce = Vector2.Zero;
			_speedForce = Vector2.Zero;

			Vector2 steeringForce = Vector2.Zero;
			Vector2 appliedForce;

			for (int i = 0; i < Behaviors.Count; i++)
			{
				//get the steering forace from the behavior
				steeringForce = Behaviors[i].GetSteering();

				//apply as much of the steering force as is available
				var result = AccumulateForce(ref _totalForce, out appliedForce, steeringForce);

				//update the direction and speed vectors
				_directionForce += appliedForce * Behaviors[i].DirectionChange;
				_speedForce += appliedForce * Behaviors[i].SpeedChange;

				//if we used up the available steering force, we are done collecting steering forces.
				if (result)
				{
					break;
				}
			}

			//divide all forces by mass
			_totalForce /= Mass;
			_directionForce /= Mass;
			_speedForce /= Mass;
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
		private void CalculateDithered()
		{
			//reset the steering force
			_totalForce = Vector2.Zero;
			_directionForce = Vector2.Zero;
			_speedForce = Vector2.Zero;

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
		}

		/// <summary>
		/// This function calculates how much of its max steering force the 
		///  vehicle has left to apply and then applies that amount of the
		///  force to add.
		/// </summary>
		/// <param name="RunningTot"></param>
		/// <param name="ForceToAdd"></param>
		/// <returns></returns>
		private bool AccumulateForce(ref Vector2 runningTot, out Vector2 appliedForce, Vector2 forceToAdd)
		{
			//calculate how much steering force the vehicle has used so far
			float magnitudeSoFar = runningTot.Length();

			//calculate how much steering force remains to be used by this vehicle
			float magnitudeRemaining = MaxForce - magnitudeSoFar;

			//return false if there is no more force left to use
			if (magnitudeRemaining <= 0.0f)
			{
				appliedForce = Vector2.Zero;
				return false;
			}

			//calculate the magnitude of the force we want to add
			float magnitudeToAdd = forceToAdd.Length();

			//if the magnitude of the sum of ForceToAdd and the running total
			//does not exceed the maximum force available to this vehicle, just
			//add together. Otherwise add as much of the ForceToAdd vector is
			//possible without going over the max.
			if (magnitudeToAdd < magnitudeRemaining)
			{
				appliedForce = forceToAdd;
				runningTot += forceToAdd;
				return true;
			}
			else
			{
				//add it to the steering force
				appliedForce = forceToAdd.Normalized();
				appliedForce *= magnitudeRemaining;
				runningTot += appliedForce;
				return false;
			}
		}

		#endregion //Methods

		#region Drawing

		/// <summary>
		/// Draw the bounding circle and heading of this boid
		/// </summary>
		/// <param name="prim"></param>
		/// <param name="color"></param>
		public override void Draw(IPrimitive prim, Color color)
		{
			base.Draw(prim, color);
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

		public void DrawTotalForce(IPrimitive prim, Color color)
		{
			//draw the force being applied
			prim.Line(Position, Position + _totalForce, color);
		}

		public void DrawWallFeelers(IPrimitive prim, Color color)
		{
			//get the wall avoidance steering behavior
			var behav = Behaviors.Where(x => x is WallAvoidance).FirstOrDefault();

			//draw all the whiskers
			var wallAvoidance = behav as IWallBehavior;
			if (null != wallAvoidance)
			{
				foreach (var whisker in wallAvoidance.Feelers)
				{
					prim.Line(Position, whisker, color);
				}
			}
		}

		/// <summary>
		/// draw the current velocity
		/// </summary>
		/// <param name="prim"></param>
		/// <param name="color"></param>
		public void DrawVelocity(IPrimitive prim, Color color)
		{
			prim.Line(Position, Position + Velocity, color);
		}

		public void DrawSpeedForce(IPrimitive prim, Color color)
		{
			//draw a circle at the MaxForce line
			//prim.Circle(Position, MaxForce, color);

			//draw the speed force being applied
			prim.Line(Position, Position + _speedForce, color);
		}

		#endregion //Drawing
	}
}