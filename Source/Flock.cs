using System.Collections.Generic;
using System.Diagnostics;
using CollisionBuddy;
using CellSpacePartitionLib;
using Microsoft.Xna.Framework;
using GameTimer;
using BasicPrimitiveBuddy;
using System.Threading.Tasks;

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
		/// crappy lock object for list access
		/// </summary>
		private object _listLock = new object();

		/// <summary>
		/// a container of all the moving entities this dude is managing
		/// </summary>
		public List<Mover> Dudes { get; private set; }

		/// <summary>
		/// The game clock to manage this flock
		/// </summary>
		public GameClock FlockTimer { get; private set; }

		/// <summary>
		/// break up the game world into cells to make it easier to update the flock
		/// </summary>
		public CellSpacePartition<Mover> CellSpace { get; private set; }

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
		public List<Mover> Enemies { get; set; }

		/// <summary>
		/// A list of all the dudes we gonna chase
		/// </summary>
		public List<Mover> Targets { get; set; }

		//any path we may create for the vehicles to follow
		//Path*                         m_pPath;

		private bool _useCellSpace = false;

		/// <summary>
		/// whether or not to use the cell space partitioning
		/// </summary>
		public bool UseCellSpace 
		{
			get
			{
				return _useCellSpace;
			}
			set
			{
				_useCellSpace = value;

				//Clear out the cell space if we arent using it
				if (false == _useCellSpace)
				{
					CellSpace.Clear();
				}
			}
		}

		/// <summary>
		/// Whether or not to constrian  boids to within toroid world
		/// </summary>
		private bool UseWorldWrap { get; set; }

		/// <summary>
		/// The size of the world!
		/// </summary>
		private Vector2 WorldSize = new Vector2(1024.0f, 768.0f);

		#endregion //Members

		#region Methods

		/// <summary>
		/// Construct the flock!
		/// </summary>
		public Flock()
		{
			UseWorldWrap = false;

			Dudes = new List<Mover>();
			FlockTimer = new GameClock();
			CellSpace = new CellSpacePartition<Mover>(WorldSize, 20, 20);

			//set this stuff here, but will prolly be overridden right away 
			Obstacles = new List<BaseEntity>();
			Enemies = new List<Mover>();
			Walls = new List<Line>();
			Targets = new List<Mover>();
		}

		/// <summary>
		/// Setup the world for this flock
		/// Do this BEFORE adding any boids, or you will fuck it up!!!
		/// </summary>
		/// <param name="worldSize"></param>
		/// <param name="useWorldWrap"></param>
		/// <param name="useCellSpace"></param>
		/// <param name="cellsX"></param>
		/// <param name="cellsY"></param>
		public void SetWorldSize(Vector2 worldSize, bool useWorldWrap, bool useCellSpace, int cellsX, int cellsY)
		{
			this.WorldSize = worldSize;
			this.UseWorldWrap = useWorldWrap;
			this.UseCellSpace = useCellSpace;
			CellSpace = new CellSpacePartition<Mover>(WorldSize, cellsX, cellsY);
		}

		/// <summary>
		/// add a dude to the flock.
		/// This is the only way that dudes should be added to ensure they go in the cell space corerctly.
		/// </summary>
		/// <param name="dude"></param>
		internal void AddDude(Boid dude)
		{
			lock (_listLock)
			{
				Dudes.Add(dude);
				if (UseCellSpace)
				{
					CellSpace.Add(dude);
				}
			}
		}

		/// <summary>
		/// update the flock!
		/// This assumes that you have already updated all the external entities (enemies, targets, etc.)
		/// </summary>
		/// <param name="curTime"></param>
		public virtual void Update(GameTime curTime)
		{
			//update the time
			FlockTimer.Update(curTime);

			//create a list of all our tasks
			List<Task> taskList = new List<Task>();

			//Update all the flock dudes
			for (int i = 0; i < Dudes.Count; i++)
			{
				Boid dude = Dudes[i] as Boid;
				Debug.Assert(null != dude);
				taskList.Add(dude.UpdateAsync(FlockTimer));
			}

			//wait for all the updates to finish
			Task.WaitAll(taskList.ToArray());

			//update the vehicle's current cell if space partitioning is turned on
			if (UseCellSpace)
			{
				for (int i = 0; i < Dudes.Count; i++)
				{
					CellSpace.Update(Dudes[i]);
				}
			}
		}

		/// <summary>
		/// Remove a boid from the list
		/// </summary>
		/// <param name="dude"></param>
		public void RemoveBoid(Boid dude)
		{
			Dudes.Remove(dude);
		}

		/// <summary>
		/// given a position, wrap them around the screen
		/// </summary>
		public void WrapWorldPosition(ref Vector2 pos)
		{
			if (UseWorldWrap)
			{
				//wrap aroud the edge
				if (pos.X > WorldSize.X)
				{
					pos.X = 0.0f;
				}
				else if (pos.X < 0.0f)
				{
					pos.X = WorldSize.X;
				}

				//wrap around the floor/ceiling
				if (pos.Y > WorldSize.Y)
				{
					pos.Y = 0.0f;
				}
				else if (pos.Y < 0.0f)
				{
					pos.Y = WorldSize.Y;
				}
			}
		}

		/// <summary>
		/// Tag all the guys inteh flock that are neightbors of a dude.
		/// </summary>
		/// <param name="dude"></param>
		public List<Mover> TagNeighbors(Boid dude, float queryRadius)
		{
			if (UseCellSpace)
			{
				//Update the cell space to find all the dudes neighbors
				CellSpace.CalculateNeighbors(dude.Position, queryRadius);

				//tag & return all the dudes it found
				return CellSpace.Neighbors;
			}
			else
			{
				//go through all the dudes and tag them up
				return dude.TagNeighbors(Dudes, queryRadius);
			}
		}

		/// <summary>
		/// Calculate the enemies of a dude.
		/// Right now just pulls out the first two dudes.
		/// </summary>
		/// <param name="dude"></param>
		/// <param name="enemy1"></param>
		/// <param name="enemy2"></param>
		public void FindEnemies(Boid dude, out Mover enemy1, out Mover enemy2)
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
		public void FindTarget(Boid dude, out Mover target)
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

		/// <summary>
		/// draw a bunch of debug info
		/// </summary>
		/// <param name="prim"></param>
		public void DrawCells(IBasicPrimitive prim)
		{
			CellSpace.RenderCells(prim);
		}

		/// <summary>
		/// draw the vectors of all the dudes
		/// </summary>
		/// <param name="prim"></param>
		public void DrawVectors(IBasicPrimitive prim)
		{
			foreach (Boid dude in Dudes)
			{
				dude.DrawVectors(prim);
			}
		}

		/// <summary>
		/// draw the wall whiskers of all the dudes
		/// </summary>
		/// <param name="prim"></param>
		public void DrawWhiskers(IBasicPrimitive prim)
		{
			foreach (Boid dude in Dudes)
			{
				dude.DrawWallFeelers(prim);
			}
		}

		/// <summary>
		/// draw all the walls 
		/// </summary>
		/// <param name="prim"></param>
		public void DrawWalls(IBasicPrimitive prim)
		{
			foreach (var wall in Walls)
			{
				wall.Draw(prim, Color.Black);
			}
		}

		//void NonPenetrationContraint(Boid dude)
		//{
		//	EnforceNonPenetrationConstraint(dude, Dudes);
		//}

		/// <summary>
		/// Tag all the obstacles that a dude can see.
		/// </summary>
		/// <param name="dude"></param>
		/// <param name="range"></param>
		public List<BaseEntity> TagObstacles(Boid dude, float range)
		{
			return dude.TagObjects(Obstacles, range);
		}

		/// <summary>
		/// Remove all the boids from this flock.
		/// </summary>
		public void Clear()
		{
			Dudes.Clear();
			CellSpace.Clear();
		}

		#endregion //Methods
	}
}