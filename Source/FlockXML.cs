using Microsoft.Xna.Framework;

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
		public Vector2 StartDirection = Vector2.Zero;
		public float StartSpeed { get; private set; }
		public float Mass { get; private set; }
		public float MaxSpeed { get; private set; }
		public float MaxTurnRate { get; private set; }
		public float MaxForce { get; private set; }
		public float QueryRadius { get; private set; }
		public Dictionary<EBehaviorType, float> Behaviors { get; private set; }
		public float EvadeThreatRange { get; private set; }
		public float FleePanicDistance { get; private set; }
		public float ObstacleAvoidanceDetectionDistance { get; private set; }
		public float WallAvoidanceWhiskerLength { get; private set; }
	}
}