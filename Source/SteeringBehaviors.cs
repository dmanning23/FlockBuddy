using Microsoft.Xna.Framework;
using System.Collections.Generic;

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
		/// the steering force created by the combined effect of all the selected behaviors
		/// </summary>
		private Vector2 SteeringForce { get; set; }

		/// <summary>
		/// How to add up all the steering behaviors
		/// </summary>
		public ESummingMethod SumMethod { get; set; }

		#endregion //Members

		/// <summary>
		/// Constructor
		/// </summary>
		public SteeringBehaviors(Boid owner)
		{
			//add all the steering behaviors
			Behaviors.Add(new Alignment(owner));
			//Behaviors.Add(new Arrive(owner));
			Behaviors.Add(new Cohesion(owner));
			Behaviors.Add(new Evade(owner));
			Behaviors.Add(new Flee(owner));
			//Behaviors.Add(new Flock(owner));
			//Behaviors.Add(new FollowPath(owner));
			//Behaviors.Add(new Hide(owner));
			//Behaviors.Add(new Interpose(owner));
			Behaviors.Add(new ObstacleAvoidance(owner));
			//Behaviors.Add(new OffsetPursuit(owner));
			Behaviors.Add(new Pursuit(owner));
			Behaviors.Add(new Seek(owner));
			Behaviors.Add(new Separation(owner));
			//Behaviors.Add(new WallAvoidance(owner));
			//Behaviors.Add(new Wander(owner));

			SteeringForce = Vector2.Zero;

			SumMethod = ESummingMethod.weighted_average;
		}

		//calculates and sums the steering forces from any active behaviors
		private Vector2 CalculateWeightedSum();
		//private Vector2 CalculatePrioritized();
		//private Vector2 CalculateDithered();

		//calculates and sums the steering forces from any active behaviors
		public Vector2 Calculate()
		{
			//reset the steering force
			SteeringForce = Vector2.Zero;

			//use space partitioning to calculate the neighbours of this vehicle if switched on. 
			//If not, use the standard tagging system
			if (!isSpacePartitioningOn())
			{
				//tag neighbors if any of the following 3 group behaviors are switched on
				if (On(separation) || On(allignment) || On(cohesion))
				{
					m_pVehicle->World()->TagVehiclesWithinViewRange(m_pVehicle, m_dViewDistance);
				}
			}
			//else
			//{
			//	//calculate neighbours in cell-space if any of the following 3 group
			//	//behaviors are switched on
			//	if (On(separation) || On(allignment) || On(cohesion))
			//	{
			//		m_pVehicle->World()->CellSpace()->CalculateNeighbors(m_pVehicle->Pos(), m_dViewDistance);
			//	}
			//}

			switch (m_SummingMethod)
			{
				case weighted_average:

				m_vSteeringForce = CalculateWeightedSum(); break;

				case prioritized:

				m_vSteeringForce = CalculatePrioritized(); break;

				case dithered:

				m_vSteeringForce = CalculateDithered(); break;

				default: m_vSteeringForce = Vector2D(0, 0);

			}//end switch

			return m_vSteeringForce;
		}

		//calculates the component of the steering force that is parallel
		//with the vehicle heading
		public double ForwardComponent()
		{
			return Vector2.Dot(Owner.Heading, SteeringForce);
		}

		//calculates the component of the steering force that is perpendicuar
		//with the vehicle heading
		public float SideComponent()
			{
			return Vector2.Dot(Owner.Side, SteeringForce);
		}

	}




}