using System;
using System.Collections.Generic;
using System.Text;

namespace FlockBuddy
{
	public interface IFlockManager
	{
		int? Id { get; set; }

		string Name { get; set; }

		float BoidMass { get; set; }
		float BoidMaxForce { get; set; }
		float BoidMinSpeed { get; set; }
		float BoidWalkSpeed { get; set; }
		float BoidMaxSpeed { get; set; }
		float BoidMaxTurnRate { get; set; }
		float BoidNeighborQueryRadius { get; set; }
		float BoidPredatorQueryRadius { get; set; }
		float BoidPreyQueryRadius { get; set; }
		float BoidVipQueryRadius { get; set; }
		float BoidWallQueryRadius { get; set; }
		float BoidObstacleQueryRadius { get; set; }
		float BoidWaypointQueryRadius { get; set; }
		float BoidRadius { get; set; }
		float BoidRetargetTime { get; set; }

		ESummingMethod SummingMethod { get; set; }

		DefaultWalls Walls { get; set; }
	}
}
