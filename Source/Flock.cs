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
		public List<Boid> Dudes { get; private set; }

		/// <summary>
		/// The game clock to manage this flock
		/// </summary>
		public GameClock FlockTimer { get; private set; }

		/// <summary>
		/// break up the game world into cells to make it easier to update the flock
		/// </summary>
		public CellSpacePartition<Boid> CellSpace { get; private set; }

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

		/// <summary>
		/// whether or not to use the cell space partitioning
		/// </summary>
		public bool UseCellSpace { get; set; }

		/// <summary>
		/// how far to do a query to calculate neightbors
		/// </summary>
		const float QueryRadius = 50.0f;

		//stuff for th cell space
		const int NumCells = 20;
		Vector2 WorldSize = new Vector2(1024.0f, 768.0f);

		#endregion //Members

		/// <summary>
		/// Construct the flock!
		/// </summary>
		public Flock()
		{
			Dudes = new List<Boid>();
			FlockTimer = new GameClock();
			CellSpace = new CellSpacePartition<Boid>(WorldSize, NumCells, NumCells);
		}

		/// <summary>
		/// update the flock!
		/// This assumes that you have already updated all the external entities (enemies, targets, etc.)
		/// </summary>
		/// <param name="curTime"></param>
		public void Update(GameTime curTime)
		{
			//update the time
			FlockTimer.Update(curTime);

			//Update all the flock dudes
			for (int i = 0; i < Dudes.Count; i++)
			{
				Dudes[i].Update(FlockTimer);
			}
		}

		/// <summary>
		/// Tag all the guys inteh flock that are neightbors of a dude.
		/// </summary>
		/// <param name="dude"></param>
		public List<Boid> TagNeighbors(Boid dude)
		{
			if (UseCellSpace)
			{
				//Update the cell space to find all the dudes neighbors
				CellSpace.CalculateNeighbors(dude.Position, QueryRadius);

				//tag & return all the dudes it found
				for (int i = 0; i < CellSpace.Neighbors.Count; i++)
				{
					CellSpace.Neighbors[i].Tagged = true;
				}
				return CellSpace.Neighbors;
			}
			else
			{
				//go through all the dudes and tag them up
				dude.TagNeighbors(Dudes, QueryRadius);
				return Dudes;
			}
		}

		/// <summary>
		/// Calculate the enemies of a dude.
		/// Right now just pulls out the first two dudes.
		/// </summary>
		/// <param name="dude"></param>
		/// <param name="enemy1"></param>
		/// <param name="enemy2"></param>
		public void FindEnemies(Boid dude, out Boid enemy1, out Boid enemy2)
		{
			//are there any enemies in the list?
			if (Enemies.Count >= 2)
			{
				enemy2 = Enemies[1];
			}
			else
			{
				enemy2 = null;
			}

			if (Enemies.Count >= 1)
			{
				enemy1 = Enemies[0];
			}
			else
			{
				enemy1 = null;
			}
		}

		/// <summary>
		/// Find any targets in the list.
		/// Right now just returns the first dude in the list.
		/// </summary>
		/// <param name="dude"></param>
		/// <param name="target"></param>
		public void FindTarget(Boid dude, out Boid target)
		{
			//are there any targets in the list?
			if (Targets.Count >= 1)
			{
				target = Targets[0];
			}
			else
			{
				target = null;
			}
		}

		void Render()
		{
		}

		//void NonPenetrationContraint(Boid dude)
		//{
		//	EnforceNonPenetrationConstraint(dude, Dudes);
		//}

		//void TagVehiclesWithinViewRange(Boid dude, double range)
		//{
		//	dude.TagNeighbors(Dudes, range);
		//}

		/// <summary>
		/// Tag all the obstacles that a dude can see.
		/// </summary>
		/// <param name="dude"></param>
		/// <param name="range"></param>
		public void TagObstacles(Boid dude, float range)
		{
			dude.TagObjects(Obstacles, range);
		}
	}
}