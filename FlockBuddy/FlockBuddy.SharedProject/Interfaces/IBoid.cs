using Microsoft.Xna.Framework;

namespace FlockBuddy
{
	public interface IBoid : IMover
	{
		Vector2 Side { get; }

		float Mass { get; set; }

		float MinSpeed { get; set; }

		float WalkSpeed { get; set; }

		float MaxSpeed { get; set; }

		float MaxForce { get; set; }

		float MaxTurnRate { get; set; }

		ESummingMethod SummingMethod { set; }

		float NeighborsQueryRadius { get; set; }

		float PredatorsQueryRadius { get; set; }

		float PreyQueryRadius { get; set; }

		float VipQueryRadius { get; set; }

		float WallQueryRadius { get; set; }

		float ObstacleQueryRadius { get; set; }

		float WaypointQueryRadius { get; set; }

		float RetargetTime { set; }

		void AddBehavior(EBehaviorType behaviorType, float weight);

		void AddBehavior(IBehavior behavior);

		void RemoveBehavior(EBehaviorType behaviorType);
	}
}
