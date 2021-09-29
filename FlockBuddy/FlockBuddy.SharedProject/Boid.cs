using CellSpacePartitionLib;
using FlockBuddy.Interfaces;
using FlockBuddy.Interfaces.Behaviors;
using FlockBuddy.SteeringBehaviors;
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

		/// <summary>
		/// a vector perpendicular to the heading vector
		/// </summary>
		private Vector2 _side;

		#endregion Members

		#region Properties

		/// <summary>
		/// The list of behaviors this boid will use
		/// </summary>
		protected List<IBehavior> Behaviors { get; set; }

		/// <summary>
		/// How to add up all the steering behaviors
		/// </summary>
		public SummingMethod SummingMethod { get; set; }

		/// <summary>
		/// the flock that owns this dude
		/// </summary>
		private IFlock MyFlock { get; set; }

		public float RetargetTime { get; set; }

		public CountdownTimer RetargetTimer { get; set; }

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

		public float Mass { get; set; }

		public float MinSpeed { get; set; }

		public float WalkSpeed { get; set; }

		/// <summary>
		/// If the total force is less than laziness, this boid will stop moving.
		/// </summary>
		public float Laziness { get; set; }

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

		public override Vector2 Heading
		{
			get
			{
				return base.Heading;
			}
			set
			{
				// If the new heading is valid this fumction sets the entity's heading and side vectors accordingly
				base.Heading = value;

				//the side vector must always be perpendicular to the heading
				_side = Heading.Perp();
			}
		}

		public Vector2 Side
		{
			get
			{
				return _side;
			}
		}

		#endregion //Properties

		#region Methods

		/// <summary>
		/// Initializes a new instance of the <see cref="FlockBuddy.Boid"/> class.
		/// </summary>
		public Boid(IFlock owner,
			Vector2 position,
			Vector2 heading,
			float speed,
			float radius = BoidDefaults.BoidRadius)
				: base(position,
				radius,
				heading,
				speed)
		{
			MyFlock = owner;
			MyFlock.AddBoid(this);

			Behaviors = new List<IBehavior>();
			RetargetTimer = new CountdownTimer();

			Initialize();
		}

		public void Initialize(float mass = BoidDefaults.BoidMass,
			float minSpeed = BoidDefaults.BoidMinSpeed,
			float walkSpeed = BoidDefaults.BoidWalkSpeed,
			float laziness = BoidDefaults.BoidLaziness,
			float maxSpeed = BoidDefaults.BoidMaxSpeed,
			float maxForce = BoidDefaults.BoidMaxForce,
			float maxTurnRate = BoidDefaults.BoidMaxTurnRate,
			SummingMethod summingMethod = BoidDefaults.DefaultSummingMethod,
			float neighborsQueryRadius = BoidDefaults.BoidQueryRadius,
			float predatorsQueryRadius = BoidDefaults.BoidQueryRadius,
			float preyQueryRadius = BoidDefaults.BoidQueryRadius,
			float vipQueryRadius = BoidDefaults.BoidQueryRadius,
			float wallQueryRadius = BoidDefaults.BoidQueryRadius,
			float obstacleQueryRadius = BoidDefaults.BoidQueryRadius,
			float waypointQueryRadius = BoidDefaults.BoidQueryRadius,
			float retargetTime = BoidDefaults.BoidRetargetTime)
		{
			Mass = mass;
			MinSpeed = minSpeed;
			WalkSpeed = walkSpeed;
			Laziness = laziness;
			MaxSpeed = maxSpeed;
			MaxForce = maxForce;
			MaxTurnRate = maxTurnRate;
			SummingMethod = summingMethod;
			NeighborsQueryRadius = neighborsQueryRadius;
			PredatorsQueryRadius = predatorsQueryRadius;
			PreyQueryRadius = preyQueryRadius;
			VipQueryRadius = vipQueryRadius;
			WallQueryRadius = wallQueryRadius;
			ObstacleQueryRadius = obstacleQueryRadius;
			WaypointQueryRadius = waypointQueryRadius;
			RetargetTime = retargetTime;

			RetargetTimer.Start(RetargetTime);
		}

		public IBehavior AddBehavior(BehaviorType behaviorType, float weight)
		{
			IBehavior behavior;

			//first check if we alreadu have that behavior
			if (!Behaviors.Exists(x => x.BehaviorType == behaviorType))
			{
				behavior = BaseBehavior.BehaviorFactory(behaviorType, this);
				behavior.Weight = weight;
				AddBehavior(behavior);
			}
			else
			{
				//we already have that behavior, just update the weight
				behavior = Behaviors.Where(x => x.BehaviorType == behaviorType).First();
				behavior.Weight = weight;
			}

			return behavior;
		}

		public void AddBehavior(IBehavior behavior)
		{
			if (!Behaviors.Exists(x => x.BehaviorType == behavior.BehaviorType))
			{
				Behaviors.Add(behavior);
				Behaviors.Sort((x, y) => x.BehaviorType.CompareTo(y.BehaviorType));
			}
			else
			{
				//we already have that behavior, just update the weight
				var currentbehavior = Behaviors.Where(x => x.BehaviorType == behavior.BehaviorType).First();
				currentbehavior.Weight = behavior.Weight;
			}
		}

		public void RemoveBehavior(IBehavior behavior)
		{
			Behaviors.Remove(behavior);
		}

		public void RemoveBehavior(BehaviorType behaviorType)
		{
			var behavior = Behaviors.Where(x => x.BehaviorType == behaviorType).FirstOrDefault();
			if (behavior != null)
			{
				Behaviors.Remove(behavior);
			}
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
		/// Given a target direction, either speed up or slow down the guy to get to it
		/// </summary>
		/// <param name="targetHeading"></param>
		protected void UpdateSpeed(Vector2 targetHeading)
		{
			//update the speed but make sure vehicle does not exceed maximum velocity
			Speed = MathHelper.Clamp(Speed + GetSpeedChange(targetHeading), MinSpeed, MaxSpeed);
		}

		protected float GetSpeedChange(Vector2 targetHeading)
		{
			//get the dot product of the current heading and the target
			var dotHeading = Vector2.Dot(Heading, targetHeading);

			//get the amount of force that can be applied pre timedelta
			var maxForceDelta = MaxForce * BoidTimer.TimeDelta;

			if (0f == dotHeading)
			{
				//if the dotproduct is exactly zero, we want to hit the target speed
				if (Speed < WalkSpeed)
				{
					//if the total force is less than laziness, why bother speeding up?
					if (_totalForce.LengthSquared() < (Laziness * Laziness))
					{
						return maxForceDelta *= -1f;
					}

					//we are going too slow, speed up!
					return maxForceDelta;
				}
				else
				{
					//we are going too fast, slow down!
					return maxForceDelta *= -1f;
				}
			}
			else if ((0 < dotHeading) && (_totalForce.LengthSquared() >= (Laziness * Laziness)))
			{
				//if the dot product is greater than zero, we want to got the current direction
				return maxForceDelta;
			}
			else if ((0 > dotHeading) && (_totalForce.LengthSquared() >= (Laziness * Laziness)))
			{
				//if the dot product is less than zero, we want to got the other direction
				return maxForceDelta *= -1f;
			}
			else
			{
				//Don't go anywhere!
				return 0f;
			}
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

		/// <summary>
		/// Update all the behaviors and calculate the steering force
		/// </summary>
		/// <returns></returns>
		private void GetForces()
		{
			RetargetTimer.Update(BoidTimer);

			if (!RetargetTimer.HasTimeRemaining)
			{
				//restart the timer
				RetargetTimer.Start(RetargetTime);
				var neighbors = FindNeighbors();
				var predator = FindPredator();
				var prey = FindPrey();
				var vip = FindVip();

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

					var pathBehavior = behavior as IPathBehavior;
					if (null != pathBehavior)
					{
						pathBehavior.Path = MyFlock.Waypoints;
					}
				}
			}

			//calculate the combined force from each steering behavior in the vehicle's list
			Calculate();
		}

		protected virtual IMover FindVip()
		{
			return MyFlock.FindClosestVipInRange(this, VipQueryRadius);
		}

		protected virtual IMover FindPrey()
		{
			return MyFlock.FindClosestPreyInRange(this, PreyQueryRadius);
		}

		protected virtual IMover FindPredator()
		{
			return MyFlock.FindClosestPredatorInRange(this, PredatorsQueryRadius);
		}

		protected virtual List<IMover> FindNeighbors()
		{
			return MyFlock.FindBoidsInRange(this, NeighborsQueryRadius);
		}

		/// <summary>
		/// calculates and sums the steering forces from any active behaviors
		/// </summary>
		/// <returns></returns>
		private void Calculate()
		{
			switch (SummingMethod)
			{
				case SummingMethod.WeightedAverage: { CalculateWeightedSum(); } break;
				case SummingMethod.Prioritized: { CalculatePrioritized(); } break;
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

				if (Behaviors[i].BehaviorType == BehaviorType.Direction)
				{
					Debug.WriteLine(steeringForce.ToString());
				}
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

			//if (IsActive(BehaviorType.wall_avoidance) && RandFloat() < Prm.prWallAvoidance)
			//{
			//	steeringForce = WallAvoidance(m_pVehicle->World()->Walls()) *
			//						 m_dWeightWallAvoidance / Prm.prWallAvoidance;

			//	if (!steeringForce.isZero())
			//	{
			//		steeringForce.Truncate(m_pVehicle->MaxForce());

			//		return steeringForce;
			//	}
			//}

			//if (IsActive(BehaviorType.obstacle_avoidance) && RandFloat() < Prm.prObstacleAvoidance)
			//{
			//	steeringForce += ObstacleAvoidance(m_pVehicle->World()->Obstacles()) *
			//			m_dWeightObstacleAvoidance / Prm.prObstacleAvoidance;

			//	if (!steeringForce.isZero())
			//	{
			//		steeringForce.Truncate(m_pVehicle->MaxForce());

			//		return steeringForce;
			//	}
			//}


			//if (IsActive(BehaviorType.separation) && RandFloat() < Prm.prSeparation)
			//{
			//	steeringForce += SeparationPlus(m_pVehicle->World()->Agents()) *
			//						m_dWeightSeparation / Prm.prSeparation;

			//	if (!steeringForce.isZero())
			//	{
			//		steeringForce.Truncate(m_pVehicle->MaxForce());

			//		return steeringForce;
			//	}
			//}



			//if (IsActive(BehaviorType.flee) && RandFloat() < Prm.prFlee)
			//{
			//	steeringForce += Flee(m_pVehicle->World()->Crosshair()) * m_dWeightFlee / Prm.prFlee;

			//	if (!steeringForce.isZero())
			//	{
			//		steeringForce.Truncate(m_pVehicle->MaxForce());

			//		return steeringForce;
			//	}
			//}

			//if (IsActive(BehaviorType.evade) && RandFloat() < Prm.prEvade)
			//{
			//	assert(m_pTargetAgent1 && "Evade target not assigned");

			//	steeringForce += Evade(m_pTargetAgent1) * m_dWeightEvade / Prm.prEvade;

			//	if (!steeringForce.isZero())
			//	{
			//		steeringForce.Truncate(m_pVehicle->MaxForce());

			//		return steeringForce;
			//	}
			//}



			//if (IsActive(BehaviorType.allignment) && RandFloat() < Prm.prAlignment)
			//{
			//	steeringForce += AlignmentPlus(m_pVehicle->World()->Agents()) *
			//						m_dWeightAlignment / Prm.prAlignment;

			//	if (!steeringForce.isZero())
			//	{
			//		steeringForce.Truncate(m_pVehicle->MaxForce());

			//		return steeringForce;
			//	}
			//}

			//if (IsActive(BehaviorType.cohesion) && RandFloat() < Prm.prCohesion)
			//{
			//	steeringForce += CohesionPlus(m_pVehicle->World()->Agents()) *
			//						m_dWeightCohesion / Prm.prCohesion;

			//	if (!steeringForce.isZero())
			//	{
			//		steeringForce.Truncate(m_pVehicle->MaxForce());

			//		return steeringForce;
			//	}
			//}


			//if (IsActive(BehaviorType.Wander) && RandFloat() < Prm.prWander)
			//{
			//	steeringForce += Wander() * m_dWeightWander / Prm.prWander;

			//	if (!steeringForce.isZero())
			//	{
			//		steeringForce.Truncate(m_pVehicle->MaxForce());

			//		return steeringForce;
			//	}
			//}

			//if (IsActive(BehaviorType.seek) && RandFloat() < Prm.prSeek)
			//{
			//	steeringForce += Seek(m_pVehicle->World()->Crosshair()) * m_dWeightSeek / Prm.prSeek;

			//	if (!steeringForce.isZero())
			//	{
			//		steeringForce.Truncate(m_pVehicle->MaxForce());

			//		return steeringForce;
			//	}
			//}

			//if (IsActive(BehaviorType.arrive) && RandFloat() < Prm.prArrive)
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
		public override void DrawNeigborQuery(IPrimitive prim, Color color)
		{
			////draw the query cells
			//MyFlock.CellSpace.RenderCellIntersections(prim, Position, QueryRadius, Color.Green);

			////get the query rectangle
			//var queryRect = CellSpacePartition<Boid>.CreateQueryBox(Position, QueryRadius);
			//prim.Rectangle(queryRect, Color.White);

			//get the query circle
			prim.Circle(Position, NeighborsQueryRadius, color);

			////draw the neighbor dudes
			//List<IMover> neighbors = MyFlock.FindNeighbors(this, QueryRadius);
			//foreach (var neighbor in neighbors)
			//{
			//	prim.Circle(neighbor.Position, neighbor.Radius, Color.Red);
			//}
		}

		public override void DrawPursuitQuery(IPrimitive prim)
		{
			var pursuit = Behaviors.FirstOrDefault(x => x.BehaviorType == BehaviorType.Pursuit) as Pursuit;

			if (null != pursuit && pursuit.Prey != null)
			{
				prim.Circle(Position, PreyQueryRadius, Color.Red);
			}
			else
			{
				prim.Circle(Position, PreyQueryRadius, Color.White);
			}
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