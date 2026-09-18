using Microsoft.Xna.Framework;

namespace FlockBuddy.Interfaces
{
	public interface IBoid : IMover
	{
		Vector2 Side { get; }

		float Mass { get; set; }

		float MinSpeed { get; set; }

		float WalkSpeed { get; set; }

		float Laziness { get; set; }

		float MaxSpeed { get; set; }

		float MaxForce { get; set; }

		float MaxTurnRate { get; set; }

		SummingMethod SummingMethod { get; set; }

		float NeighborsQueryRadius { get; set; }

		float PredatorsQueryRadius { get; set; }

		float PreyQueryRadius { get; set; }

		float VipQueryRadius { get; set; }

		float WallQueryRadius { get; set; }

		float ObstacleQueryRadius { get; set; }

		float WaypointQueryRadius { get; set; }

		float RetargetTime { get; set; }

		IBehavior AddBehavior(BehaviorType behaviorType, float weight);

		void AddBehavior(IBehavior behavior);

		void RemoveBehavior(IBehavior behavior);

		void RemoveBehavior(BehaviorType behaviorType);

		void Initialize(float mass,
			float minSpeed,
			float walkSpeed,
			float laziness,
			float maxSpeed,
			float maxForce,
			float maxTurnRate,
			SummingMethod summingMethod,
			float neighborsQueryRadius,
			float predatorsQueryRadius,
			float preyQueryRadius,
			float vipQueryRadius,
			float wallQueryRadius,
			float obstacleQueryRadius,
			float waypointQueryRadius,
			float retargetTime);
	}
}
