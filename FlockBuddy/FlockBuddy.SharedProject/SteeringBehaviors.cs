using GameTimer;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

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
		private List<IBehavior> Behaviors { get; set; }

		/// <summary>
		/// How to add up all the steering behaviors
		/// </summary>
		public ESummingMethod SumMethod { get; set; }

		/// <summary>
		/// The boid who owns this dude.
		/// </summary>
		protected IBoid Owner { get; private set; }

		private GameClock Timer { get; set; }

		private Random _rand = new Random();

		#endregion //Members

		#region Properties

		/// <summary>
		/// A list of all the neighbors to use
		/// </summary>
		public List<IMover> Neighbors { get; set; }

		/// <summary>
		/// these can be used to keep track of pursuers
		/// </summary>
		public IMover Enemy1 { get; set; }

		/// <summary>
		/// these can be used to keep track of pursuers
		/// </summary>
		public IMover Enemy2 { get; set; }

		/// <summary>
		/// these can be used to keep track of target dudes
		/// </summary>
		public IMover Prey { get; set; }

		/// <summary>
		/// the current target point
		/// </summary>
		public Vector2 Target;

		#endregion //Properties

		#region Methods

		/// <summary>
		/// Constructor
		/// </summary>
		public SteeringBehaviors(IBoid owner)
		{
			Owner = owner;
			Timer = new GameClock();
			Behaviors = new List<IBehavior>();

			SumMethod = ESummingMethod.weighted_average;
		}

		public void AddBehavior(IBehavior behavior)
		{
			Behaviors.Add(behavior);
			Behaviors.Sort((x, y) => x.BehaviorType.CompareTo(y.BehaviorType));
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
				forceToAdd.Normalize();
				runningTot += (forceToAdd * magnitudeRemaining);
			}

			return true;
		}

		#endregion //Methods
	}
}