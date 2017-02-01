using System.Collections.Generic;
using CollisionBuddy;
using GameTimer;
using Microsoft.Xna.Framework;

namespace FlockBuddy
{
	public interface IFlock
	{
		/// <summary>
		/// The flock of enemies that will attack boids in this flock
		/// </summary>
		IFlock Predators { set; }

		/// <summary>
		/// The flock of targets that can be attacked
		/// </summary>
		IFlock Prey { set; }

		/// <summary>
		/// A flock of entities that need to be protected
		/// </summary>
		IFlock Vips { set; }

		/// <summary>
		/// The obstacles that have to be avoided
		/// </summary>
		List<IBaseEntity> Obstacles { set; }

		/// <summary>
		/// The walls that have to be avoided
		/// </summary>
		List<ILine> Walls { set; }

		void AddBoid(IBoid boid);

		void RemoveBoid(IBoid boid);

		void Update(GameClock time);

		IMover FindClosestBoidInRange(IBoid boid, float queryRadius);

		List<IMover> FindBoidsInRange(IBoid boid, float queryRadius);

		List<IBaseEntity> FindObstaclesInRange(IBoid boid, float queryRadius);

		IMover FindClosestPredatorInRange(IBoid boid, float queryRadius);

		IMover FindClosestPreyInRange(IBoid boid, float queryRadius);

		IMover FindClosestVipInRange(IBoid boid, float queryRadius);

		Vector2 WrapWorldPosition(Vector2 pos);
	}
}