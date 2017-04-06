using System.Collections.Generic;
using CollisionBuddy;
using GameTimer;
using Microsoft.Xna.Framework;
using PrimitiveBuddy;

namespace FlockBuddy
{
	public interface IFlock
	{
		Color DebugColor { get; }

		string Name { get; set; }

		/// <summary>
		/// All the boids stored in this flock.
		/// </summary>
		List<IMover> Boids { get; }

		/// <summary>
		/// The flock of enemies that will attack boids in this flock
		/// </summary>
		List<IFlock> Predators { get; }

		/// <summary>
		/// The flock of targets that can be attacked
		/// </summary>
		List<IFlock> Prey { get; }

		/// <summary>
		/// A flock of entities that need to be protected
		/// </summary>
		List<IFlock> Vips { get; }

		/// <summary>
		/// The obstacles that have to be avoided
		/// </summary>
		List<IBaseEntity> Obstacles { set; }

		/// <summary>
		/// The walls that have to be avoided
		/// </summary>
		List<ILine> Walls { get; set; }

		void SetWorldSize(Vector2 worldSize, bool useWorldWrap = true, bool useCellSpace = true, int cellsX = 20, int cellsY = 20);

		void AddBoid(IMover boid);

		void RemoveBoid(IMover boid);

		void Update(GameClock time);

		IMover FindClosestBoidInRange(IBoid boid, float queryRadius);

		List<IMover> FindBoidsInRange(IBoid boid, float queryRadius);

		List<IBaseEntity> FindObstaclesInRange(IBoid boid, float queryRadius);

		IMover FindClosestPredatorInRange(IBoid boid, float queryRadius);

		IMover FindClosestPreyInRange(IBoid boid, float queryRadius);

		IMover FindClosestVipInRange(IBoid boid, float queryRadius);

		void RemoveFlock(IFlock flock);

		Vector2 WrapWorldPosition(Vector2 pos);

		void AddDefaultWalls(DefaultWalls wallsType, Rectangle rect);

		void Draw(IPrimitive prim, Color color);
		void DrawWhiskers(IPrimitive prim, Color color);

		void AddFlockToGroup(IFlock flock, FlockGroup group);

		bool IsFlockInGroup(IFlock flock, FlockGroup group);
	}
}