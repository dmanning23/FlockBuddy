using CellSpacePartitionLib;
using CollisionBuddy;
using FlockBuddy.Interfaces;
using GameTimer;
using Microsoft.Xna.Framework;
using PrimitiveBuddy;
using System;
using System.Collections.Generic;

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
		protected object _listLock = new object();
		
		#endregion //Fields

		#region Properties

		public Color DebugColor { get; private set; }

		public string Name { get; set; }

		/// <summary>
		/// Used for database persistance
		/// </summary>
		public int? Id { get; set; }

		/// <summary>
		/// a container of all the moving entities this boid is managing
		/// </summary>
		public List<IMover> Boids { get; set; }

		/// <summary>
		/// The game clock to manage this flock
		/// </summary>
		private GameClock FlockTimer { get; set; }

		/// <summary>
		/// break up the game world into cells to make it easier to update the flock
		/// </summary>
		public CellSpacePartition<IMover> CellSpace { get; set; }

		public List<IFlock> Predators { get; private set; }

		public List<IFlock> Prey { get; private set; }

		public List<IFlock> Vips { get; private set; }

		/// <summary>
		/// any obstacles for this flock
		/// </summary>
		public List<IBaseEntity> Obstacles { private get; set; }

		/// <summary>
		/// container containing any walls in the environment
		/// </summary>
		public List<ILine> Walls { get; set; }

		public List<Vector2> Waypoints { get; set; }

		//any path we may create for the vehicles to follow
		//List<vector2> Path { get; private set; }

		/// <summary>
		/// whether or not to use the cell space partitioning
		/// </summary>
		public bool UseCellSpace => null != CellSpace;

		/// <summary>
		/// Whether or not to constrian  boids to within toroid world
		/// </summary>
		public bool UseWorldWrap { get; set; }

		/// <summary>
		/// The size of the world!
		/// </summary>
		public Vector2 WorldSize { get; private set; } = new Vector2(1024.0f, 768.0f);

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

			Predators = new List<IFlock>();
			Prey = new List<IFlock>();
			Vips = new List<IFlock>();
			Waypoints = new List<Vector2>();
		}

		/// <summary>
		/// add a boid to the flock.
		/// This is the only way that boids should be added to ensure they go in the cell space corerctly.
		/// </summary>
		/// <param name="boid"></param>
		public void AddBoid(IMover boid)
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

		public void AddBoids(IEnumerable<IMover> boids)
		{
			lock (_listLock)
			{
				Boids.AddRange(boids);
				if (UseCellSpace)
				{
					foreach (var boid in boids)
					{
						CellSpace.Add(boid);
					}
				}
			}
		}

		/// <summary>
		/// Remove a boid from the list
		/// </summary>
		/// <param name="boid"></param>
		public void RemoveBoid(IMover boid)
		{
			Boids.Remove(boid);

			if (UseCellSpace)
			{
				CellSpace.Remove(boid);
			}
		}

		/// <summary>
		/// Remove all the boids from this flock.
		/// </summary>
		public virtual void Clear()
		{
			Boids.Clear();

			if (UseCellSpace)
			{
				CellSpace.Clear();
			}
		}

		public void AddDefaultWalls(DefaultWalls wallsType, Rectangle rect)
		{
			//only keep the walls we want
			switch (wallsType)
			{
				case DefaultWalls.All:
					{
						Walls = Line.ExtendedInsideRect(rect, 128f);
					}
					break;
				case DefaultWalls.None:
					{
						//empty list!
						Walls = new List<ILine>();
					}
					break;
				case DefaultWalls.TopBottom:
					{
						var walls = Line.ExtendedInsideRect(rect, 128f);
						Walls = new List<ILine>()
						{
							walls[0],
							walls[2]
						};
					}
					break;
				case DefaultWalls.LeftRight:
					{
						var walls = Line.ExtendedInsideRect(rect, 128f);
						Walls = new List<ILine>()
						{
							walls[1],
							walls[3]
						};
					}
					break;
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

			//Update all the flock boids
			for (int i = 0; i < Boids.Count; i++)
			{
				if (Boids[i] is Boid boid)
				{
					boid.Update(FlockTimer);
				}
			}

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

		public void RemoveFlock(IFlock flock)
		{
			Predators.Remove(flock);
			Prey.Remove(flock);
			Vips.Remove(flock);
		}

		public void AddFlockToGroup(IFlock flock, FlockGroup group)
		{
			//remove the flock from all groups
			RemoveFlock(flock);

			//add the flock to the correct group
			switch (group)
			{
				case FlockGroup.Predator:
					{
						Predators.Add(flock);
					}
					break;
				case FlockGroup.Prey:
					{
						Prey.Add(flock);
					}
					break;
				case FlockGroup.Vip:
					{
						Vips.Add(flock);
					}
					break;
			}
		}

		public bool IsFlockInGroup(IFlock flock, FlockGroup group)
		{
			switch (group)
			{
				case FlockGroup.None:
					{
						return !IsFlockInGroup(flock, FlockGroup.Predator) &&
							!IsFlockInGroup(flock, FlockGroup.Prey) &&
							!IsFlockInGroup(flock, FlockGroup.Vip);
					}
				case FlockGroup.Predator:
					{
						return Predators.Contains(flock);
					}
				case FlockGroup.Prey:
					{
						return Prey.Contains(flock);
					}
				case FlockGroup.Vip:
					{
						return Vips.Contains(flock);
					}
				default:
					{
						throw new Exception("did you add a new flock group?");
					}
			}
		}

		#region Find Methods

		public IMover FindClosestBoidInRange(IBoid boid, float queryRadius)
		{
			//get all the dudes in range
			var inRange = FindBoidsInRange(boid, queryRadius);
			return FindClosestFromList(boid, inRange);
		}

		public IMover FindBoidAtPosition(Vector2 position, float boidRadius)
		{
			if (UseCellSpace)
			{
				return CellSpace.NearestNeighbor(position, boidRadius);
			}
			else
			{
				//get all the dudes in range
				var inRange = FindBoidsAtPosition(position, boidRadius);

				//find the closest out of all of those
				return FindClosestFromList(position, inRange);
			}
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
				return CellSpace.CalculateNeighbors(boid.Position, queryRadius, true);
			}
			else
			{
				//go through all the boids and tag them up
				return FindBoidsInRange(boid, Boids, queryRadius);
			}
		}

		public List<IMover> FindBoidsAtPosition(Vector2 position, float boidRadius)
		{
			if (UseCellSpace)
			{
				//Update the cell space to find all the boids neighbors
				return CellSpace.CalculateNeighbors(position, boidRadius);
			}
			else
			{
				//go through all the boids and tag them up
				return FindBoidsAtPosition(position, Boids);
			}
		}

		public List<IBaseEntity> FindObstaclesInRange(IBoid boid, float queryRadius)
		{
			return FindObjectsInRange(boid, Obstacles, queryRadius);
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
			return FindClosestFromFlockList(boid, queryRadius, Predators);
		}

		/// <summary>
		/// Find any targets in the list.
		/// Right now just returns the first boid in the list.
		/// </summary>
		/// <param name="boid"></param>
		/// <param name="target"></param>
		public IMover FindClosestPreyInRange(IBoid boid, float queryRadius)
		{
			return FindClosestFromFlockList(boid, queryRadius, Prey);
		}

		public IMover FindClosestVipInRange(IBoid boid, float queryRadius)
		{
			return FindClosestFromFlockList(boid, queryRadius, Vips);
		}

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

		private List<IMover> FindBoidsAtPosition(Vector2 position, List<IMover> dudes)
		{
			var neighbors = new List<IMover>();

			//iterate through all entities checking for range
			for (int i = 0; i < dudes.Count; i++)
			{
				if (CheckIfObjectAtPosition(position, dudes[i]))
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
			if (null != dudes)
			{
				for (int i = 0; i < dudes.Count; i++)
				{
					if (CheckIfObjectInRange(boid, dudes[i], queryRadius))
					{
						neighbors.Add(dudes[i]);
					}
				}
			}

			return neighbors;
		}

		private IMover FindClosestFromFlockList(IBoid boid, float queryRadius, List<IFlock> flocks)
		{
			var close = new List<IMover>();
			foreach (var flock in flocks)
			{
				var closest = flock.FindClosestBoidInRange(boid, queryRadius);
				if (null != closest)
				{
					close.Add(closest);
				}
			}

			return FindClosestFromList(boid, close);
		}

		private IMover FindClosestFromList(IBoid boid, List<IMover> inRange)
		{
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

		private IMover FindClosestFromList(Vector2 position, List<IMover> inRange)
		{
			float closestDistance = 0f;
			IMover closest = null;

			//set the "closest" to the first available
			if (inRange.Count >= 1)
			{
				closest = inRange[0];
				closestDistance = DistanceSquared(position, inRange[0]);
			}

			//loop through the rest and see if there are any closer
			if (inRange.Count >= 2)
			{
				for (int i = 1; i < inRange.Count; i++)
				{
					var distance = DistanceSquared(position, inRange[i]);
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

		private bool CheckIfObjectAtPosition(Vector2 position, IBaseEntity dude)
		{
			if (null == dude)
			{
				return false;
			}

			//the bounding radius of the other is taken into account by adding it to the range
			double range = dude.Radius;

			//if entity within range, tag for further consideration. 
			//(working in distance-squared space to avoid sqrts)
			return ((DistanceSquared(position, dude) < (range * range)));
		}

		private float DistanceSquared(IBoid boid, IBaseEntity dude)
		{
			return (dude.Position - boid.Position).LengthSquared();
		}

		private float DistanceSquared(Vector2 position, IBaseEntity dude)
		{
			return (dude.Position - position).LengthSquared();
		}

		#endregion //Find Methods

		#region Drawing

		public void Draw(IPrimitive prim, Color color)
		{
			foreach (Boid boid in Boids)
			{
				boid.Draw(prim, color);
				boid.DrawSpeedForce(prim, Color.White);
			}
		}

		/// <summary>
		/// draw a bunch of debug info
		/// </summary>
		/// <param name="prim"></param>
		public void DrawCells(IPrimitive prim)
		{
			if (UseCellSpace)
			{
				CellSpace.RenderCells(prim);
			}
		}

		/// <summary>
		/// draw the vectors of all the boids
		/// </summary>
		/// <param name="prim"></param>
		public void DrawTotalForce(IPrimitive prim, Color color)
		{
			foreach (Boid boid in Boids)
			{
				boid.DrawTotalForce(prim, color);
			}
		}

		/// <summary>
		/// draw the wall whiskers of all the boids
		/// </summary>
		/// <param name="prim"></param>
		public void DrawWhiskers(IPrimitive prim, Color color)
		{
			foreach (Boid boid in Boids)
			{
				boid.DrawWallFeelers(prim, color);
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