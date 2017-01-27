using System.Collections.Generic;
using CollisionBuddy;

namespace FlockBuddy
{
	public interface IFlock
	{
		List<IMover> Boids { get; }
		List<IMover> Enemies { get; }
		List<IBaseEntity> Obstacles { get; }
		List<IMover> Targets { get; }
		List<ILine> Walls { get; }

		void AddBoid(IBoid boid);
	}
}