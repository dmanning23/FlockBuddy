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

		/// <summary>
		/// The boid who owns this dude.
		/// </summary>
		protected Boid Owner { get; private set; }

		#endregion //Members

		#region Properties

		/// <summary>
		/// these can be used to keep track of friends, pursuers, or prey
		/// </summary>
		Boid TargetAgent1 { get; set; }

		/// <summary>
		/// these can be used to keep track of friends, pursuers, or prey
		/// </summary>
		Boid TargetAgent2 { get; set; }

		/// <summary>
		/// the current target
		/// </summary>
		Vector2 Target;

		#endregion //Properties

		#region Methods

		/// <summary>
		/// Constructor
		/// </summary>
		public SteeringBehaviors(Boid owner)
		{
			Owner = owner;

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

			SteeringForce = Vector2.Zero;

			SumMethod = ESummingMethod.weighted_average;
		}

		

		//calculates and sums the steering forces from any active behaviors
		public Vector2 Calculate()
		{
			////reset the steering force
			//SteeringForce = Vector2.Zero;

			////use space partitioning to calculate the neighbours of this vehicle if switched on. 
			////If not, use the standard tagging system
			//if (!isSpacePartitioningOn())
			//{
			//	//tag neighbors if any of the following 3 group behaviors are switched on
			//	if (On(separation) || On(allignment) || On(cohesion))
			//	{
			//		m_pVehicle->World()->TagVehiclesWithinViewRange(m_pVehicle, m_dViewDistance);
			//	}
			//}
			////else
			////{
			////	//calculate neighbours in cell-space if any of the following 3 group
			////	//behaviors are switched on
			////	if (On(separation) || On(allignment) || On(cohesion))
			////	{
			////		m_pVehicle->World()->CellSpace()->CalculateNeighbors(m_pVehicle->Pos(), m_dViewDistance);
			////	}
			////}

			//switch (m_SummingMethod)
			//{
			//	case weighted_average:

			//	m_vSteeringForce = CalculateWeightedSum(); break;

			//	case prioritized:

			//	m_vSteeringForce = CalculatePrioritized(); break;

			//	case dithered:

			//	m_vSteeringForce = CalculateDithered(); break;

			//	default: m_vSteeringForce = Vector2D(0, 0);

			//}//end switch

			return SteeringForce;
		}





//---------------------- CalculatePrioritized ----------------------------
//
//  this method calls each active steering behavior in order of priority
//  and acumulates their forces until the max steering force magnitude
//  is reached, at which time the function returns the steering force 
//  accumulated to that  point
//------------------------------------------------------------------------
Vector2 CalculatePrioritized()
{       
  Vector2 force;
  
   if (On(wall_avoidance))
  {
    force = WallAvoidance(m_pVehicle->World()->Walls()) *
            m_dWeightWallAvoidance;

    if (!AccumulateForce(m_vSteeringForce, force)) return m_vSteeringForce;
  }
   
  if (On(obstacle_avoidance))
  {
    force = ObstacleAvoidance(m_pVehicle->World()->Obstacles()) * 
            m_dWeightObstacleAvoidance;

    if (!AccumulateForce(m_vSteeringForce, force)) return m_vSteeringForce;
  }

  if (On(evade))
  {
    assert(m_pTargetAgent1 && "Evade target not assigned");
    
    force = Evade(m_pTargetAgent1) * m_dWeightEvade;

    if (!AccumulateForce(m_vSteeringForce, force)) return m_vSteeringForce;
  }

  
  if (On(flee))
  {
    force = Flee(m_pVehicle->World()->Crosshair()) * m_dWeightFlee;

    if (!AccumulateForce(m_vSteeringForce, force)) return m_vSteeringForce;
  }


 
  //these next three can be combined for flocking behavior (wander is
  //also a good behavior to add into this mix)
  if (!isSpacePartitioningOn())
  {
    if (On(separation))
    {
      force = Separation(m_pVehicle->World()->Agents()) * m_dWeightSeparation;

      if (!AccumulateForce(m_vSteeringForce, force)) return m_vSteeringForce;
    }

    if (On(allignment))
    {
      force = Alignment(m_pVehicle->World()->Agents()) * m_dWeightAlignment;

      if (!AccumulateForce(m_vSteeringForce, force)) return m_vSteeringForce;
    }

    if (On(cohesion))
    {
      force = Cohesion(m_pVehicle->World()->Agents()) * m_dWeightCohesion;

      if (!AccumulateForce(m_vSteeringForce, force)) return m_vSteeringForce;
    }
  }

  else
  {

    if (On(separation))
    {
      force = SeparationPlus(m_pVehicle->World()->Agents()) * m_dWeightSeparation;

      if (!AccumulateForce(m_vSteeringForce, force)) return m_vSteeringForce;
    }

    if (On(allignment))
    {
      force = AlignmentPlus(m_pVehicle->World()->Agents()) * m_dWeightAlignment;

      if (!AccumulateForce(m_vSteeringForce, force)) return m_vSteeringForce;
    }

    if (On(cohesion))
    {
      force = CohesionPlus(m_pVehicle->World()->Agents()) * m_dWeightCohesion;

      if (!AccumulateForce(m_vSteeringForce, force)) return m_vSteeringForce;
    }
  }

  if (On(seek))
  {
    force = Seek(m_pVehicle->World()->Crosshair()) * m_dWeightSeek;

    if (!AccumulateForce(m_vSteeringForce, force)) return m_vSteeringForce;
  }


  if (On(arrive))
  {
    force = Arrive(m_pVehicle->World()->Crosshair(), m_Deceleration) * m_dWeightArrive;

    if (!AccumulateForce(m_vSteeringForce, force)) return m_vSteeringForce;
  }

  if (On(wander))
  {
    force = Wander() * m_dWeightWander;

    if (!AccumulateForce(m_vSteeringForce, force)) return m_vSteeringForce;
  }

  if (On(pursuit))
  {
    assert(m_pTargetAgent1 && "pursuit target not assigned");

    force = Pursuit(m_pTargetAgent1) * m_dWeightPursuit;

    if (!AccumulateForce(m_vSteeringForce, force)) return m_vSteeringForce;
  }

  if (On(offset_pursuit))
  {
    assert (m_pTargetAgent1 && "pursuit target not assigned");
    assert (!m_vOffset.isZero() && "No offset assigned");

    force = OffsetPursuit(m_pTargetAgent1, m_vOffset);

    if (!AccumulateForce(m_vSteeringForce, force)) return m_vSteeringForce;
  }

  if (On(interpose))
  {
    assert (m_pTargetAgent1 && m_pTargetAgent2 && "Interpose agents not assigned");

    force = Interpose(m_pTargetAgent1, m_pTargetAgent2) * m_dWeightInterpose;

    if (!AccumulateForce(m_vSteeringForce, force)) return m_vSteeringForce;
  }

  if (On(hide))
  {
    assert(m_pTargetAgent1 && "Hide target not assigned");

    force = Hide(m_pTargetAgent1, m_pVehicle->World()->Obstacles()) * m_dWeightHide;

    if (!AccumulateForce(m_vSteeringForce, force)) return m_vSteeringForce;
  }


  if (On(follow_path))
  {
    force = FollowPath() * m_dWeightFollowPath;

    if (!AccumulateForce(m_vSteeringForce, force)) return m_vSteeringForce;
  }

  return m_vSteeringForce;
}


//---------------------- CalculateWeightedSum ----------------------------
//
//  this simply sums up all the active behaviors X their weights and 
//  truncates the result to the max available steering force before 
//  returning
//------------------------------------------------------------------------
Vector2D SteeringBehavior::CalculateWeightedSum()
{        
  if (On(wall_avoidance))
  {
    m_vSteeringForce += WallAvoidance(m_pVehicle->World()->Walls()) *
                         m_dWeightWallAvoidance;
  }
   
  if (On(obstacle_avoidance))
  {
    m_vSteeringForce += ObstacleAvoidance(m_pVehicle->World()->Obstacles()) * 
            m_dWeightObstacleAvoidance;
  }

  if (On(evade))
  {
    assert(m_pTargetAgent1 && "Evade target not assigned");
    
    m_vSteeringForce += Evade(m_pTargetAgent1) * m_dWeightEvade;
  }


  //these next three can be combined for flocking behavior (wander is
  //also a good behavior to add into this mix)
  if (!isSpacePartitioningOn())
  {
    if (On(separation))
    {
      m_vSteeringForce += Separation(m_pVehicle->World()->Agents()) * m_dWeightSeparation;
    }

    if (On(allignment))
    {
      m_vSteeringForce += Alignment(m_pVehicle->World()->Agents()) * m_dWeightAlignment;
    }

    if (On(cohesion))
    {
      m_vSteeringForce += Cohesion(m_pVehicle->World()->Agents()) * m_dWeightCohesion;
    }
  }
  else
  {
    if (On(separation))
    {
      m_vSteeringForce += SeparationPlus(m_pVehicle->World()->Agents()) * m_dWeightSeparation;
    }

    if (On(allignment))
    {
      m_vSteeringForce += AlignmentPlus(m_pVehicle->World()->Agents()) * m_dWeightAlignment;
    }

    if (On(cohesion))
    {
      m_vSteeringForce += CohesionPlus(m_pVehicle->World()->Agents()) * m_dWeightCohesion;
    }
  }


  if (On(wander))
  {
    m_vSteeringForce += Wander() * m_dWeightWander;
  }

  if (On(seek))
  {
    m_vSteeringForce += Seek(m_pVehicle->World()->Crosshair()) * m_dWeightSeek;
  }

  if (On(flee))
  {
    m_vSteeringForce += Flee(m_pVehicle->World()->Crosshair()) * m_dWeightFlee;
  }

  if (On(arrive))
  {
    m_vSteeringForce += Arrive(m_pVehicle->World()->Crosshair(), m_Deceleration) * m_dWeightArrive;
  }

  if (On(pursuit))
  {
    assert(m_pTargetAgent1 && "pursuit target not assigned");

    m_vSteeringForce += Pursuit(m_pTargetAgent1) * m_dWeightPursuit;
  }

  if (On(offset_pursuit))
  {
    assert (m_pTargetAgent1 && "pursuit target not assigned");
    assert (!m_vOffset.isZero() && "No offset assigned");

    m_vSteeringForce += OffsetPursuit(m_pTargetAgent1, m_vOffset) * m_dWeightOffsetPursuit;
  }

  if (On(interpose))
  {
    assert (m_pTargetAgent1 && m_pTargetAgent2 && "Interpose agents not assigned");

    m_vSteeringForce += Interpose(m_pTargetAgent1, m_pTargetAgent2) * m_dWeightInterpose;
  }

  if (On(hide))
  {
    assert(m_pTargetAgent1 && "Hide target not assigned");

    m_vSteeringForce += Hide(m_pTargetAgent1, m_pVehicle->World()->Obstacles()) * m_dWeightHide;
  }

  if (On(follow_path))
  {
    m_vSteeringForce += FollowPath() * m_dWeightFollowPath;
  }

  m_vSteeringForce.Truncate(m_pVehicle->MaxForce());
 
  return m_vSteeringForce;
}


//---------------------- CalculateDithered ----------------------------
//
//  this method sums up the active behaviors by assigning a probabilty
//  of being calculated to each behavior. It then tests the first priority
//  to see if it should be calcukated this simulation-step. If so, it
//  calculates the steering force resulting from this behavior. If it is
//  more than zero it returns the force. If zero, or if the behavior is
//  skipped it continues onto the next priority, and so on.
//
//  NOTE: Not all of the behaviors have been implemented in this method,
//        just a few, so you get the general idea
//------------------------------------------------------------------------
Vector2D SteeringBehavior::CalculateDithered()
{  
  //reset the steering force
   m_vSteeringForce.Zero();

  if (On(wall_avoidance) && RandFloat() < Prm.prWallAvoidance)
  {
    m_vSteeringForce = WallAvoidance(m_pVehicle->World()->Walls()) *
                         m_dWeightWallAvoidance / Prm.prWallAvoidance;

    if (!m_vSteeringForce.isZero())
    {
      m_vSteeringForce.Truncate(m_pVehicle->MaxForce()); 
      
      return m_vSteeringForce;
    }
  }
   
  if (On(obstacle_avoidance) && RandFloat() < Prm.prObstacleAvoidance)
  {
    m_vSteeringForce += ObstacleAvoidance(m_pVehicle->World()->Obstacles()) * 
            m_dWeightObstacleAvoidance / Prm.prObstacleAvoidance;

    if (!m_vSteeringForce.isZero())
    {
      m_vSteeringForce.Truncate(m_pVehicle->MaxForce()); 
      
      return m_vSteeringForce;
    }
  }

  if (!isSpacePartitioningOn())
  {
    if (On(separation) && RandFloat() < Prm.prSeparation)
    {
      m_vSteeringForce += Separation(m_pVehicle->World()->Agents()) * 
                          m_dWeightSeparation / Prm.prSeparation;

      if (!m_vSteeringForce.isZero())
      {
        m_vSteeringForce.Truncate(m_pVehicle->MaxForce()); 
      
        return m_vSteeringForce;
      }
    }
  }

  else
  {
    if (On(separation) && RandFloat() < Prm.prSeparation)
    {
      m_vSteeringForce += SeparationPlus(m_pVehicle->World()->Agents()) * 
                          m_dWeightSeparation / Prm.prSeparation;

      if (!m_vSteeringForce.isZero())
      {
        m_vSteeringForce.Truncate(m_pVehicle->MaxForce()); 
      
        return m_vSteeringForce;
      }
    }
  }


  if (On(flee) && RandFloat() < Prm.prFlee)
  {
    m_vSteeringForce += Flee(m_pVehicle->World()->Crosshair()) * m_dWeightFlee / Prm.prFlee;

    if (!m_vSteeringForce.isZero())
    {
      m_vSteeringForce.Truncate(m_pVehicle->MaxForce()); 
      
      return m_vSteeringForce;
    }
  }

  if (On(evade) && RandFloat() < Prm.prEvade)
  {
    assert(m_pTargetAgent1 && "Evade target not assigned");
    
    m_vSteeringForce += Evade(m_pTargetAgent1) * m_dWeightEvade / Prm.prEvade;

    if (!m_vSteeringForce.isZero())
    {
      m_vSteeringForce.Truncate(m_pVehicle->MaxForce()); 
      
      return m_vSteeringForce;
    }
  }


  if (!isSpacePartitioningOn())
  {
    if (On(allignment) && RandFloat() < Prm.prAlignment)
    {
      m_vSteeringForce += Alignment(m_pVehicle->World()->Agents()) *
                          m_dWeightAlignment / Prm.prAlignment;

      if (!m_vSteeringForce.isZero())
      {
        m_vSteeringForce.Truncate(m_pVehicle->MaxForce()); 
      
        return m_vSteeringForce;
      }
    }

    if (On(cohesion) && RandFloat() < Prm.prCohesion)
    {
      m_vSteeringForce += Cohesion(m_pVehicle->World()->Agents()) * 
                          m_dWeightCohesion / Prm.prCohesion;

      if (!m_vSteeringForce.isZero())
      {
        m_vSteeringForce.Truncate(m_pVehicle->MaxForce()); 
      
        return m_vSteeringForce;
      }
    }
  }
  else
  {
    if (On(allignment) && RandFloat() < Prm.prAlignment)
    {
      m_vSteeringForce += AlignmentPlus(m_pVehicle->World()->Agents()) *
                          m_dWeightAlignment / Prm.prAlignment;

      if (!m_vSteeringForce.isZero())
      {
        m_vSteeringForce.Truncate(m_pVehicle->MaxForce()); 
      
        return m_vSteeringForce;
      }
    }

    if (On(cohesion) && RandFloat() < Prm.prCohesion)
    {
      m_vSteeringForce += CohesionPlus(m_pVehicle->World()->Agents()) *
                          m_dWeightCohesion / Prm.prCohesion;

      if (!m_vSteeringForce.isZero())
      {
        m_vSteeringForce.Truncate(m_pVehicle->MaxForce()); 
      
        return m_vSteeringForce;
      }
    }
  }

  if (On(wander) && RandFloat() < Prm.prWander)
  {
    m_vSteeringForce += Wander() * m_dWeightWander / Prm.prWander;

    if (!m_vSteeringForce.isZero())
    {
      m_vSteeringForce.Truncate(m_pVehicle->MaxForce()); 
      
      return m_vSteeringForce;
    }
  }

  if (On(seek) && RandFloat() < Prm.prSeek)
  {
    m_vSteeringForce += Seek(m_pVehicle->World()->Crosshair()) * m_dWeightSeek / Prm.prSeek;

    if (!m_vSteeringForce.isZero())
    {
      m_vSteeringForce.Truncate(m_pVehicle->MaxForce()); 
      
      return m_vSteeringForce;
    }
  }

  if (On(arrive) && RandFloat() < Prm.prArrive)
  {
    m_vSteeringForce += Arrive(m_pVehicle->World()->Crosshair(), m_Deceleration) * 
                        m_dWeightArrive / Prm.prArrive;

    if (!m_vSteeringForce.isZero())
    {
      m_vSteeringForce.Truncate(m_pVehicle->MaxForce()); 
      
      return m_vSteeringForce;
    }
  }
 
  return m_vSteeringForce;
}

				//--------------------- AccumulateForce ----------------------------------
//
//  This function calculates how much of its max steering force the 
//  vehicle has left to apply and then applies that amount of the
//  force to add.
//------------------------------------------------------------------------
bool AccumulateForce(Vector2 RunningTot,
                                       Vector2 ForceToAdd)
{
  
  //calculate how much steering force the vehicle has used so far
  double MagnitudeSoFar = RunningTot.Length();

  //calculate how much steering force remains to be used by this vehicle
  double MagnitudeRemaining = m_pVehicle->MaxForce() - MagnitudeSoFar;

  //return false if there is no more force left to use
  if (MagnitudeRemaining <= 0.0) return false;

  //calculate the magnitude of the force we want to add
  double MagnitudeToAdd = ForceToAdd.Length();
  
  //if the magnitude of the sum of ForceToAdd and the running total
  //does not exceed the maximum force available to this vehicle, just
  //add together. Otherwise add as much of the ForceToAdd vector is
  //possible without going over the max.
  if (MagnitudeToAdd < MagnitudeRemaining)
  {
    RunningTot += ForceToAdd;
  }

  else
  {
    //add it to the steering force
    RunningTot += (Vec2DNormalize(ForceToAdd) * MagnitudeRemaining); 
  }

  return true;
}

		#endregion //Methods
	}
}