using System.Collections.Generic;
using CollisionBuddy;
using CellSpacePartitionLib;
using Microsoft.Xna.Framework;
using GameTimer;

namespace FlockBuddy
{
	/// <summary>
	/// This holds all the data for a single flock...
	/// It has all the dudes, which will be updated here.
	/// It has all the enemies, which are updated ELSEWHERE.
	/// It has all the prey items, which are updated ELSEWHERE
	/// It has all the obstacles, walls, and path, which are also all updated somewhere else in the game.
	/// </summary>
	public class Flock
	{
		#region Members

		/// <summary>
		/// a container of all the moving entities this dude is managing
		/// </summary>
		public List<Boid> Flock { get; private set; }

		/// <summary>
		/// The game clock to manage this flock
		/// </summary>
		public GameClock FlockTimer { get; private set; }

		/// <summary>
		/// break up the game world into cells to make it easier to update the flock
		/// </summary>
		public CellSpacePartition CellSpace { get; private set; }

		/// <summary>
		/// any obstacles for this flock
		/// </summary>
		public List<BaseEntity> Obstacles { get; set; }

		/// <summary>
		/// container containing any walls in the environment
		/// </summary>
		public List<Line> Walls { get; set; }

		/// <summary>
		/// A list of all the enemies of this flock.  We will run away from them!
		/// </summary>
		public List<Boid> Enemies { get; set; }

		/// <summary>
		/// A list of all the dudes we gonna chase
		/// </summary>
		public List<Boid> Targets { get; set; }

		//any path we may create for the vehicles to follow
		//Path*                         m_pPath;

		#endregion //Members

		/// <summary>
		/// Construct the flock!
		/// </summary>
		Flock()
		{
			Flock = new List<Boid>();
			FlockTimer = new GameClock();
		}

		/// <summary>
		/// update the flock!
		/// This assumes that you have already updated all the external entities (enemies, targets, etc.)
		/// </summary>
		/// <param name="curTime"></param>
		void Update(GameTime curTime)
		{
		}

		void Render();


		void NonPenetrationContraint(Vehicle* v) { EnforceNonPenetrationConstraint(v, m_Vehicles); }

		void TagVehiclesWithinViewRange(BaseGameEntity* pVehicle, double range)
		{
			TagNeighbors(pVehicle, m_Vehicles, range);
		}

		void TagObstaclesWithinViewRange(BaseGameEntity* pVehicle, double range)
		{
			TagNeighbors(pVehicle, m_Obstacles, range);
		}
	}
}