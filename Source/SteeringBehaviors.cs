using Microsoft.Xna.Framework;
using System.Collections.Generic;
using GameTimer;
using Vector2Extensions;
using RandomExtensions;
using System;

namespace FlockBuddy
{
	/// <summary>
	/// class to encapsulate steering behaviors for a boid
	/// </summary>
	public class SteeringBehaviors
	{
		#region Members

		/// <summary>
		/// The list of behaviors this boid will use
		/// </summary>
		public List<BaseBehavior> Behaviors { get; private set; }

		/// <summary>
		/// How to add up all the steering behaviors
		/// </summary>
		public ESummingMethod SumMethod { get; set; }

		/// <summary>
		/// The boid who owns this dude.
		/// </summary>
		protected Boid Owner { get; private set; }

		private GameClock Timer { get; set; }

		Random _rand = new Random();

		#endregion //Members

		#region Properties

		/// <summary>
		/// A list of all the neighbors to use
		/// </summary>
		public List<Boid> Neighbors { get; set; }

		/// <summary>
		/// these can be used to keep track of pursuers
		/// </summary>
		public Boid Enemy1 { get; set; }

		/// <summary>
		/// these can be used to keep track of pursuers
		/// </summary>
		public Boid Enemy2 { get; set; }

		/// <summary>
		/// these can be used to keep track of target dudes
		/// </summary>
		public Boid Prey { get; set; }

		/// <summary>
		/// the current target point
		/// </summary>
		public Vector2 Target;

		#endregion //Properties

		#region Methods

		/// <summary>
		/// Constructor
		/// </summary>
		public SteeringBehaviors(Boid owner)
		{
			Owner = owner;
			Timer = new GameClock();
			Behaviors = new List<BaseBehavior>();

			//add all the steering behaviors
			Behaviors.Add(new Alignment(owner));
			Behaviors.Add(new Arrive(owner));
			Behaviors.Add(new Cohesion(owner));
			Behaviors.Add(new Evade(owner));
			Behaviors.Add(new Flee(owner));
			Behaviors.Add(new FollowPath(owner));
			Behaviors.Add(new Hide(owner));
			Behaviors.Add(new Interpose(owner));
			Behaviors.Add(new ObstacleAvoidance(owner));
			Behaviors.Add(new OffsetPursuit(owner));
			Behaviors.Add(new Pursuit(owner));
			Behaviors.Add(new Seek(owner));
			Behaviors.Add(new Separation(owner));
			Behaviors.Add(new WallAvoidance(owner));
			Behaviors.Add(new Wander(owner));

			SumMethod = ESummingMethod.weighted_average;
		}

		#region Run Behaviors

		public Vector2 CalcAlignment()
		{
			Alignment behavior = Behaviors[(int)EBehaviorType.alignment] as Alignment;
			return behavior.GetSteering(Neighbors);
		}

		public Vector2 CalcArrive()
		{
			//TODO
			Arrive behavior = Behaviors[(int)EBehaviorType.arrive] as Arrive;
			return Vector2.Zero;
		}

		public Vector2 CalcCohesion()
		{
			Cohesion behavior = Behaviors[(int)EBehaviorType.cohesion] as Cohesion;
			return behavior.GetSteering(Neighbors);
		}

		public Vector2 CalcEvade()
		{
			Evade behavior = Behaviors[(int)EBehaviorType.evade] as Evade;
			return behavior.GetSteering(Enemy1);
		}

		public Vector2 CalcFlee()
		{
			Flee behavior = Behaviors[(int)EBehaviorType.flee] as Flee;
			return behavior.GetSteering(Target);
		}

		public Vector2 CalcFollowPath()
		{
			//TODO
			FollowPath behavior = Behaviors[(int)EBehaviorType.follow_path] as FollowPath;
			return Vector2.Zero;
		}

		public Vector2 CalcHide()
		{
			//TODO
			Hide behavior = Behaviors[(int)EBehaviorType.hide] as Hide;
			return Vector2.Zero;
		}

		public Vector2 CalcInterpose()
		{
			//TODO
			Interpose behavior = Behaviors[(int)EBehaviorType.interpose] as Interpose;
			return Vector2.Zero;
		}

		public Vector2 CalcObstacleAvoidance()
		{
			ObstacleAvoidance behavior = Behaviors[(int)EBehaviorType.obstacle_avoidance] as ObstacleAvoidance;
			return behavior.GetSteering2();
		}

		public Vector2 CalcOffsetPursuit()
		{
			//TODO
			OffsetPursuit behavior = Behaviors[(int)EBehaviorType.offset_pursuit] as OffsetPursuit;
			return Vector2.Zero;
		}

		public Vector2 CalcPursuit()
		{
			//TODO
			Pursuit behavior = Behaviors[(int)EBehaviorType.pursuit] as Pursuit;
			return Vector2.Zero;
		}

		public Vector2 CalcSeek()
		{
			Seek behavior = Behaviors[(int)EBehaviorType.seek] as Seek;
			return behavior.GetSteering(Target);
		}

		public Vector2 CalcSeparation()
		{
			Separation behavior = Behaviors[(int)EBehaviorType.separation] as Separation;
			return behavior.GetSteering(Neighbors);
		}

		public Vector2 CalcWallAvoidance()
		{
			WallAvoidance behavior = Behaviors[(int)EBehaviorType.wall_avoidance] as WallAvoidance;
			return behavior.GetSteering(Owner.MyFlock.Walls);
		}

		public Vector2 CalcWander()
		{
			//TODO
			Wander behavior = Behaviors[(int)EBehaviorType.wander] as Wander;
			return Vector2.Zero;
		}

		#endregion //Run Behaviors

		/// <summary>
		/// Given a list of behaviors, activate only the ones that are specified
		/// This needs to be called once the behavior thing is created.
		/// If you call this more than once, this does NOT deactivate any of the behaviors that are not specified!
		/// </summary>
		/// <param name="behaviors">a list of all the behaviors we want this dude to use</param>
		public void ActivateBehaviors(EBehaviorType[] behaviors)
		{
			foreach (EBehaviorType behavior in behaviors)
			{
				this.Behaviors[(int)behavior].Active = true;
			}
		}

		/// <summary>
		/// Check if a particular steering behavior is active
		/// </summary>
		/// <param name="behavior"></param>
		/// <returns></returns>
		public bool IsActive(EBehaviorType behavior)
		{
			return Behaviors[(int)behavior].Active;
		}

		/// <summary>
		/// calculates and sums the steering forces from any active behaviors
		/// </summary>
		/// <returns></returns>
		public Vector2 Calculate(GameClock time)
		{
			//update the time
			Timer.Update(time);

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

			if (IsActive(EBehaviorType.wall_avoidance))
			{
				steeringForce += CalcWallAvoidance();
			}

			if (IsActive(EBehaviorType.obstacle_avoidance))
			{
				steeringForce += CalcObstacleAvoidance();
			}

			if (IsActive(EBehaviorType.evade))
			{
				steeringForce += CalcEvade();
			}


			//these next three can be combined for flocking behavior
			//(wander is also a good behavior to add into this mix)

			if (IsActive(EBehaviorType.separation))
			{
				steeringForce += CalcSeparation();
			}

			if (IsActive(EBehaviorType.alignment))
			{
				steeringForce += CalcAlignment();
			}

			if (IsActive(EBehaviorType.cohesion))
			{
				steeringForce += CalcCohesion();
			}


			if (IsActive(EBehaviorType.wander))
			{
				steeringForce += CalcWander();
			}

			if (IsActive(EBehaviorType.seek))
			{
				steeringForce += CalcSeek();
			}

			if (IsActive(EBehaviorType.flee))
			{
				steeringForce += CalcFlee();
			}

			if (IsActive(EBehaviorType.arrive))
			{
				steeringForce += CalcArrive();
			}

			if (IsActive(EBehaviorType.pursuit))
			{
				steeringForce += CalcPursuit();
			}

			if (IsActive(EBehaviorType.offset_pursuit))
			{
				steeringForce += CalcOffsetPursuit();
			}

			if (IsActive(EBehaviorType.interpose))
			{
				steeringForce += CalcInterpose();
			}

			if (IsActive(EBehaviorType.hide))
			{
				steeringForce += CalcHide();
			}

			if (IsActive(EBehaviorType.follow_path))
			{
				steeringForce += CalcFollowPath();
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
		Vector2 CalculatePrioritized()
		{
			Vector2 force;
			Vector2 steeringForce = Vector2.Zero;

			if (IsActive(EBehaviorType.wall_avoidance))
			{
				force = CalcWallAvoidance();
				if (!AccumulateForce(ref steeringForce, force)) return steeringForce;
			}

			if (IsActive(EBehaviorType.obstacle_avoidance))
			{
				force = CalcObstacleAvoidance();
				if (!AccumulateForce(ref steeringForce, force)) return steeringForce;
			}

			if (IsActive(EBehaviorType.evade))
			{
				force = CalcEvade();
				if (!AccumulateForce(ref steeringForce, force)) return steeringForce;
			}

			if (IsActive(EBehaviorType.flee))
			{
				force = CalcFlee();
				if (!AccumulateForce(ref steeringForce, force)) return steeringForce;
			}

			//these next three can be combined for flocking behavior
			//(wander is also a good behavior to add into this mix)

			if (IsActive(EBehaviorType.separation))
			{
				force = CalcSeparation();
				if (!AccumulateForce(ref steeringForce, force)) return steeringForce;
			}

			if (IsActive(EBehaviorType.alignment))
			{
				force = CalcAlignment();
				if (!AccumulateForce(ref steeringForce, force)) return steeringForce;
			}

			if (IsActive(EBehaviorType.cohesion))
			{
				force = CalcCohesion();
				if (!AccumulateForce(ref steeringForce, force)) return steeringForce;
			}

			if (IsActive(EBehaviorType.seek))
			{
				force = CalcSeek();
				if (!AccumulateForce(ref steeringForce, force)) return steeringForce;
			}

			if (IsActive(EBehaviorType.arrive))
			{
				force = CalcArrive();
				if (!AccumulateForce(ref steeringForce, force)) return steeringForce;
			}

			if (IsActive(EBehaviorType.wander))
			{
				force = CalcWander();
				if (!AccumulateForce(ref steeringForce, force)) return steeringForce;
			}

			if (IsActive(EBehaviorType.pursuit))
			{
				force = CalcPursuit();
				if (!AccumulateForce(ref steeringForce, force)) return steeringForce;
			}

			if (IsActive(EBehaviorType.offset_pursuit))
			{
				force = CalcOffsetPursuit();
				if (!AccumulateForce(ref steeringForce, force)) return steeringForce;
			}

			if (IsActive(EBehaviorType.interpose))
			{
				force = CalcInterpose();
				if (!AccumulateForce(ref steeringForce, force)) return steeringForce;
			}

			if (IsActive(EBehaviorType.hide))
			{
				force = CalcHide();
				if (!AccumulateForce(ref steeringForce, force)) return steeringForce;
			}

			if (IsActive(EBehaviorType.follow_path))
			{
				force = CalcFollowPath();
				if (!AccumulateForce(ref steeringForce, force)) return steeringForce;
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
		Vector2 CalculateDithered()
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
		bool AccumulateForce(ref Vector2 runningTot, Vector2 forceToAdd)
		{
			//calculate how much steering force the vehicle has used so far
			float magnitudeSoFar = runningTot.Length();

			//calculate how much steering force remains to be used by this vehicle
			float magnitudeRemaining = Owner.MaxForce - magnitudeSoFar;

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
				Vector2.Normalize(forceToAdd);
				runningTot += (forceToAdd * magnitudeRemaining);
			}

			return true;
		}

		#endregion //Methods
	}
}