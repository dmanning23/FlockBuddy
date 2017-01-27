using CellSpacePartitionLib;
using CollisionBuddy;
using GameTimer;
using Microsoft.Xna.Framework;
using PrimitiveBuddy;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace FlockBuddy
{
	/// <summary>
	/// This holds all the data for a single flock...
	/// It has all the boids, which will be updated here.
	/// It has all the enemies, which are updated ELSEWHERE.
	/// It has all the prey items, which are updated ELSEWHERE
	/// It has all the obstacles, walls, and path, which are also all updated somewhere else in the game.
	/// </summary>
	public class Flock : IFlock
	{
		#region Members

		/// <summary>
		/// crappy lock object for list access
		/// </summary>
		private object _listLock = new object();

		/// <summary>
		/// a container of all the moving entities this boid is managing
		/// </summary>
		public List<IMover> Boids { get; private set; }

		/// <summary>
		/// The game clock to manage this flock
		/// </summary>
		public GameClock FlockTimer { get; private set; }

		/// <summary>
		/// break up the game world into cells to make it easier to update the flock
		/// </summary>
		public CellSpacePartition<IMover> CellSpace { get; private set; }

		/// <summary>
		/// any obstacles for this flock
		/// </summary>
		public List<IBaseEntity> Obstacles { get; private set; }

		/// <summary>
		/// container containing any walls in the environment
		/// </summary>
		public List<ILine> Walls { get; private set; }

		/// <summary>
		/// A list of all the enemies of this flock.  We will run away from them!
		/// </summary>
		public List<IMover> Enemies { get; private set; }

		/// <summary>
		/// A list of all the boids we gonna chase
		/// </summary>
		public List<IMover> Targets { get; private set; }

		//any path we may create for the vehicles to follow
		//List<vector2> Path { get; private set; }

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

		public BoidTemplate BoidTemplate { get; set; }

		#endregion //Members

		#region Methods

		/// <summary>
		/// Construct the flock!
		/// </summary>
		public Flock()
		{
			UseWorldWrap = false;

			Boids = new List<IMover>();
			FlockTimer = new GameClock();
			CellSpace = new CellSpacePartition<IMover>(WorldSize, 20, 20);

			//set this stuff here, but will prolly be overridden right away 
			Obstacles = new List<IBaseEntity>();
			Enemies = new List<IMover>();
			Walls = new List<ILine>();
			Targets = new List<IMover>();

			BoidTemplate = new BoidTemplate();
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
			CellSpace = new CellSpacePartition<IMover>(WorldSize, cellsX, cellsY);
		}

		/// <summary>
		/// add a boid to the flock.
		/// This is the only way that boids should be added to ensure they go in the cell space corerctly.
		/// </summary>
		/// <param name="boid"></param>
		internal void AddDude(IBoid boid)
		{
			lock (_listLock)
			{
				Boids.Add(boid);
				if (UseCellSpace)
				{
					CellSpace.Add(boid);
				}
			}
		}

		/// <summary>
		/// update the flock!
		/// This assumes that you have already updated all the external entities (enemies, targets, etc.)
		/// </summary>
		/// <param name="curTime"></param>
		public virtual void Update(GameClock curTime)
		{
			//update the time
			FlockTimer.Update(curTime);

			//create a list of all our tasks
			List<Task> taskList = new List<Task>();

			//Update all the flock boids
			for (int i = 0; i < Boids.Count; i++)
			{
				Boid boid = Boids[i] as Boid;
				Debug.Assert(null != boid);
				taskList.Add(boid.UpdateAsync(FlockTimer));
			}

			//wait for all the updates to finish
			Task.WaitAll(taskList.ToArray());

			//update the vehicle's current cell if space partitioning is turned on
			if (UseCellSpace)
			{
				for (int i = 0; i < Boids.Count; i++)
				{
					CellSpace.Update(Boids[i]);
				}
			}
		}

		/// <summary>
		/// Remove a boid from the list
		/// </summary>
		/// <param name="boid"></param>
		public void RemoveBoid(IBoid boid)
		{
			Boids.Remove(boid);
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
		/// Tag all the guys inteh flock that are neightbors of a boid.
		/// </summary>
		/// <param name="boid"></param>
		public List<IMover> TagNeighbors(IBoid boid, float queryRadius)
		{
			if (UseCellSpace)
			{
				//Update the cell space to find all the boids neighbors
				return CellSpace.CalculateNeighbors(boid.Position, queryRadius);
			}
			else
			{
				//go through all the boids and tag them up
				return boid.TagNeighbors(Boids, queryRadius);
			}
		}

		/// <summary>
		/// Calculate the enemies of a boid.
		/// Right now just pulls out the first two boids.
		/// </summary>
		/// <param name="boid"></param>
		/// <param name="enemy1"></param>
		/// <param name="enemy2"></param>
		public void FindEnemies(IBoid boid, out IMover enemy1, out IMover enemy2)
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
		/// Right now just returns the first boid in the list.
		/// </summary>
		/// <param name="boid"></param>
		/// <param name="target"></param>
		public void FindTarget(IBoid boid, out IMover target)
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
		public void DrawCells(IPrimitive prim)
		{
			CellSpace.RenderCells(prim);
		}

		/// <summary>
		/// draw the vectors of all the boids
		/// </summary>
		/// <param name="prim"></param>
		public void DrawVectors(IPrimitive prim)
		{
			foreach (Boid boid in Boids)
			{
				boid.DrawVectors(prim);
			}
		}

		/// <summary>
		/// draw the wall whiskers of all the boids
		/// </summary>
		/// <param name="prim"></param>
		public void DrawWhiskers(IPrimitive prim)
		{
			foreach (Boid boid in Boids)
			{
				boid.DrawWallFeelers(prim);
			}
		}

		/// <summary>
		/// draw all the walls 
		/// </summary>
		/// <param name="prim"></param>
		public void DrawWalls(IPrimitive prim)
		{
			foreach (var wall in Walls)
			{
				wall.Draw(prim, Color.Black);
			}
		}

		//void NonPenetrationContraint(Boid boid)
		//{
		//	EnforceNonPenetrationConstraint(boid, Boids);
		//}

		/// <summary>
		/// Tag all the obstacles that a boid can see.
		/// </summary>
		/// <param name="boid"></param>
		/// <param name="range"></param>
		public List<IBaseEntity> TagObstacles(IBoid boid, float range)
		{
			return (null == boid ? boid.TagObjects(Obstacles, range) : new List<IBaseEntity>());
		}

		/// <summary>
		/// Remove all the boids from this flock.
		/// </summary>
		public virtual void Clear()
		{
			Boids.Clear();
			CellSpace.Clear();
		}

		#endregion //Methods
	}
}