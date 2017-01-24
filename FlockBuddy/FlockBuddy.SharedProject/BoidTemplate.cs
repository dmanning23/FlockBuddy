using System;
using System.Collections.Generic;

namespace FlockBuddy
{
	/// <summary>
	/// properties required to run a behavior
	/// </summary>
	public class BehaviorTemplate
	{
		#region Properties

		/// <summary>
		/// binary flag to indicate whether or not a behavior should be active
		/// </summary>
		public bool Enabled { get; set; }

		/// <summary>
		/// How much weight to apply to this beahvior
		/// </summary>
		public float Weight { get; set; }

		#endregion //Properties

		#region Methods

		/// <summary>
		/// construuctor
		/// </summary>
		public BehaviorTemplate()
		{
			Enabled = false;
			Weight = 1.0f;
		}

		#endregion //Methods
	}

	/// <summary>
	/// Contains all the data to describe the behavior of a boid
	/// </summary>
	public class BoidTemplate
	{
		#region Members

		/// <summary>
		/// the radius of the boid
		/// </summary>
		public float Radius { get; set; }

		/// <summary>
		/// mass o the boid
		/// </summary>
		public float Mass { get; set; }

		/// <summary>
		/// The max speed of the boid
		/// </summary>
		public float MaxSpeed { get; set; }

		/// <summary>
		/// max turn rate of this boid
		/// </summary>
		public float MaxTurnRate { get; set; }

		/// <summary>
		/// the max for ce of teh boid
		/// </summary>
		public float MaxForce { get; set; }

		/// <summary>
		/// how far out to check for neighbors
		/// </summary>
		public float QueryRadius { get; set; }

		/// <summary>
		/// A dictionary of behavior types to weights...
		/// Any behaviors in this dictionary are "active"
		/// </summary>
		public List<BehaviorTemplate> Behaviors { get; private set; }

		/// <summary>
		/// how far the evade behvaior should look out for threats
		/// </summary>
		public float EvadeThreatRange { get; set; }

		/// <summary>
		/// how far out the flee behvaior should watch before panicking
		/// </summary>
		public float FleePanicDistance { get; set; }

		/// <summary>
		/// how four the obstacle avoidance behvaior should watch for obastcles
		/// </summary>
		public float ObstacleAvoidanceDetectionDistance { get; set; }

		/// <summary>
		/// the length of the wall avoidance whiskers
		/// </summary>
		public float WallAvoidanceWhiskerLength { get; set; }

		/// <summary>
		/// If this is set to true, predators will head straight at prey instead of looking ahead
		/// </summary>
		public bool ViciousPursuit { get; set; }

		#endregion Members

		#region Methods

		/// <summary>
		/// Initializes a new instance of the <see cref="FlockBuddy.Boid"/> class.
		/// </summary>
		public BoidTemplate()
		{
			Radius = 10.0f;
			Mass = 1.0f;
			MaxSpeed = 500.0f;
			MaxTurnRate = 10.0f;
			MaxForce = 100.0f;
			QueryRadius = 100.0f;

			int numBehaviors = (int)Enum.GetNames(typeof(EBehaviorType)).Length;
			Behaviors = new List<BehaviorTemplate>();
			for (int i = 0; i < numBehaviors; i++)
			{
				Behaviors.Add(new BehaviorTemplate());
			}

			SetBehavior(EBehaviorType.alignment, true, 10.0f);
			SetBehavior(EBehaviorType.cohesion, true, 0.5f);
			SetBehavior(EBehaviorType.separation, true, 120.0f);
			SetBehavior(EBehaviorType.obstacle_avoidance, true, 30.0f);
			SetBehavior(EBehaviorType.wall_avoidance, true, 50.0f);
			SetBehavior(EBehaviorType.pursuit, true, 0.1f);
			SetBehavior(EBehaviorType.evade, true, 1.0f);

			EvadeThreatRange = 80.0f;
			FleePanicDistance = 100.0f;
			ObstacleAvoidanceDetectionDistance = 100.0f;
			WallAvoidanceWhiskerLength = 60.0f;
			ViciousPursuit = false;
		}

		public void SetBehavior(EBehaviorType behavior, bool enable, float weight)
		{
			Behaviors[(int)behavior].Enabled = enable;
			Behaviors[(int)behavior].Weight = weight;
		}

		#endregion //Methods
	}
}