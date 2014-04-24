using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace FlockBuddy
{
	public class BehaviodXML
	{
		public string BehaviorType = "";
		public float Weight = 1.0f;
	}

	/// <summary>
	/// This is all the data needed to persist a particle emitter out to XML
	/// </summary>
	public class BoidTemplateXML
	{
		public float Radius = 0.0f;
		public Vector2 StartDirection = Vector2.Zero;
		public float StartSpeed = 0.0f;
		public float Mass = 0.0f;
		public float MaxSpeed = 0.0f;
		public float MaxTurnRate = 0.0f;
		public float MaxForce = 0.0f;
		public float QueryRadius = 0.0f;
		public Dictionary<EBehaviorType, float> Behaviors = new Dictionary<EBehaviorType, float>();
		public float EvadeThreatRange = 0.0f;
		public float FleePanicDistance = 0.0f;
		public float ObstacleAvoidanceDetectionDistance = 0.0f;
		public float WallAvoidanceWhiskerLength = 0.0f;
	}
}