using System;

namespace FlockBuddy
{
	public static class BoidDefaults
	{
		public const float BoidRadius = 10f;
		public const float BoidMass = 1f;
		public const float BoidMinSpeed = 140f;
		public const float BoidWalkSpeed = 200f;
		public const float BoidLaziness = 0.5f;
		public const float BoidMaxSpeed = 275f;
		public const float BoidMaxTurnRate = (float)Math.PI;
		public const float BoidMaxForce = 200f;
		public const float BoidQueryRadius = 100f;
		public const float BoidRetargetTime = 0.1f;
		public const ESummingMethod SummingMethod = ESummingMethod.weighted_average;
		public const DefaultWalls Walls = DefaultWalls.None;

		public const float AlignmentWeight = 10f;
		public const float ArriveWeight = 1f;
		public const float CohesionWeight = 1f;
		public const float EvadeWeight = 1f;
		public const float DirectionWeight = 1f;
		public const float FleeWeight = 1f;
		public const float FollowPathWeight = 1f;
		public const float HideWeight = 1f;
		public const float InterposeWeight = 10f;
		public const float ObstacleAvoidanceWeight = 30f;
		public const float OffsetPursuitWeight = 1f;
		public const float PursuitWeight = 0.1f;
		public const float SeekWeight = 1f;
		public const float SeparationWeight = 60f;
		public const float WallAvoidanceWeight = 50f;
		public const float WanderWeight = 1f;
	}
}
