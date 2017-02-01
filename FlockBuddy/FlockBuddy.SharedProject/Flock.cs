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
		#region Fields 

		/// <summary>
		/// crappy lock object for list access
		/// </summary>
		private object _listLock = new object();

		private bool _useCellSpace = false;

		#endregion //Fields

		#region Properties

		/// <summary>
		/// a container of all the moving entities this boid is managing
		/// </summary>
		private List<IMover> Boids { get; set; }

		/// <summary>
		/// The game clock to manage this flock
		/// </summary>
		private GameClock FlockTimer { get; set; }

		/// <summary>
		/// break up the game world into cells to make it easier to update the flock
		/// </summary>
		private CellSpacePartition<IMover> CellSpace { get; set; }

		public IFlock Predators { private get; set; }

		public IFlock Prey { private get; set; }

		public IFlock Vips { private get; set; }

		/// <summary>
		/// any obstacles for this flock
		/// </summary>
		public List<IBaseEntity> Obstacles { private get; set; }

		/// <summary>
		/// container containing any walls in the environment
		/// </summary>
		public List<ILine> Walls { private get; set; }

		//any path we may create for the vehicles to follow
		//List<vector2> Path { get; private set; }

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

			Boids = new List<IMover>();
			FlockTimer = new GameClock();
			CellSpace = new CellSpacePartition<IMover>(WorldSize, 20, 20);
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
		public void AddBoid(IBoid boid)
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
		public Vector2 WrapWorldPosition(Vector2 pos)
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

			return pos;
		}

		public IMover FindClosestBoidInRange(IBoid boid, float queryRadius)
		{
			//get all the dudes in range
			var inRange = FindBoidsInRange(boid, queryRadius);

			float closestDistance = 0f;
			IMover closest = null;

			//set the "closest" to the first available
			if (inRange.Count >= 1)
			{
				closest = inRange[0];
				closestDistance = DistanceSquared(boid, inRange[0]);
			}
			
			//loop through the rest and see if there are any closer
			if (inRange.Count >= 2)
			{
				for (int i = 1; i < inRange.Count; i++)
				{
					var distance = DistanceSquared(boid, inRange[i]);
					if (distance < closestDistance)
					{
						closest = inRange[i];
						closestDistance = distance;
					}
				}
			}

			return closest;
		}

		/// <summary>
		/// Tag all the guys inteh flock that are neightbors of a boid.
		/// </summary>
		/// <param name="boid"></param>
		public List<IMover> FindBoidsInRange(IBoid boid, float queryRadius)
		{
			if (UseCellSpace)
			{
				//Update the cell space to find all the boids neighbors
				return CellSpace.CalculateNeighbors(boid.Position, queryRadius);
			}
			else
			{
				//go through all the boids and tag them up
				return FindBoidsInRange(boid, Boids, queryRadius);
			}
		}

		public List<IBaseEntity> FindObstaclesInRange(IBoid boid, float queryRadius)
		{
			return FindObjectsInRange(boid, Obstacles, queryRadius);
		}

		/// <summary>
		/// tags any entities contained in a container that are within the radius of the single entity parameter
		/// </summary>
		/// <param name="boid"></param>
		/// <param name="dudes">teh list of entities to tag as neighbors</param>
		/// <param name="queryRadius">the disdtance to tag neightbors</param>
		private List<IMover> FindBoidsInRange(IBoid boid, List<IMover> dudes, float queryRadius)
		{
			var neighbors = new List<IMover>();

			//iterate through all entities checking for range
			for (int i = 0; i < dudes.Count; i++)
			{
				if (CheckIfObjectInRange(boid, dudes[i], queryRadius))
				{
					neighbors.Add(dudes[i]);
				}
			}

			return neighbors;
		}

		/// <summary>
		/// check if some non-boid dudes are in range
		/// </summary>
		/// <param name="boid"></param>
		/// <param name="dudes"></param>
		/// <param name="queryRadius"></param>
		private List<IBaseEntity> FindObjectsInRange(IBoid boid, List<IBaseEntity> dudes, float queryRadius)
		{
			var neighbors = new List<IBaseEntity>();

			//iterate through all entities checking for range
			for (int i = 0; i < dudes.Count; i++)
			{
				if (CheckIfObjectInRange(boid, dudes[i], queryRadius))
				{
					neighbors.Add(dudes[i]);
				}
			}

			return neighbors;
		}

		/// <summary>
		/// check if a dude is in range
		/// </summary>
		/// <param name="boid"></param>
		/// <param name="dude"></param>
		/// <param name="queryRadius"></param>
		private bool CheckIfObjectInRange(IBoid boid, IBaseEntity dude, float queryRadius)
		{
			if (null == dude)
			{
				return false;
			}
			
			//the bounding radius of the other is taken into account by adding it to the range
			double range = queryRadius + dude.Radius;

			//if entity within range, tag for further consideration. 
			//(working in distance-squared space to avoid sqrts)
			return ((DistanceSquared(boid, dude) < (range * range)) && (dude != boid));
		}

		private float DistanceSquared(IBoid boid, IBaseEntity dude)
		{
			return (dude.Position - boid.Position).LengthSquared();
		}

		/// <summary>
		/// Calculate the enemies of a boid.
		/// Right now just pulls out the first two boids.
		/// </summary>
		/// <param name="boid"></param>
		/// <param name="enemy1"></param>
		/// <param name="enemy2"></param>
		public IMover FindClosestPredatorInRange(IBoid boid, float queryRadius)
		{
			return (null != Predators ? Predators.FindClosestBoidInRange(boid, queryRadius) : null);
		}

		/// <summary>
		/// Find any targets in the list.
		/// Right now just returns the first boid in the list.
		/// </summary>
		/// <param name="boid"></param>
		/// <param name="target"></param>
		public IMover FindClosestPreyInRange(IBoid boid, float queryRadius)
		{
			return (null != Prey ? Prey.FindClosestBoidInRange(boid, queryRadius) : null);
		}

		public IMover FindClosestVipInRange(IBoid boid, float queryRadius)
		{
			return (null != Vips ? Vips.FindClosestBoidInRange(boid, queryRadius) : null);
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
		public List<IBaseEntity> FindObstacles(IBoid boid, float queryRadius)
		{
			return (null != boid ? FindObjectsInRange(boid, Obstacles, queryRadius) : new List<IBaseEntity>());
		}

		/// <summary>
		/// Remove all the boids from this flock.
		/// </summary>
		public virtual void Clear()
		{
			Boids.Clear();
			CellSpace.Clear();
		}

		#region Drawing

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
				var line = wall as Line;
				if (null != line)
				{
					line.Draw(prim, Color.Black);
				}
			}
		}

		#endregion //Drawing

		#endregion //Methods
	}
}