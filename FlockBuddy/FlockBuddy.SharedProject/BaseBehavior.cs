using FlockBuddy.Interfaces;
using FlockBuddy.SteeringBehaviors;
using Microsoft.Xna.Framework;
using System;

namespace FlockBuddy
{
	/// <summary>
	/// This is the base class for all behaviors
	/// </summary>
	public abstract class BaseBehavior : IBehavior
	{
		#region Properties

		/// <summary>
		/// The type of this beahvior
		/// </summary>
		public BehaviorType BehaviorType { get; private set; }

		/// <summary>
		/// a pointer to the owner of this instance
		/// </summary>
		public IBoid Owner { get; set; }

		/// <summary>
		/// How much weight to apply to this beahvior
		/// </summary>
		public float Weight { get; set; }

		public abstract float DirectionChange { get; }

		public abstract float SpeedChange { get; }

		#endregion //Properties

		#region Methods

		/// <summary>
		/// Initializes a new instance of the <see cref="FlockBuddy.BaseBehavior"/> class.
		/// </summary>
		public BaseBehavior(IBoid owner, BehaviorType behaviorType, float weight)
		{
			BehaviorType = behaviorType;
			Owner = owner;
			Weight = weight;
		}

		/// <summary>
		/// Called every frame to get the steering direction from this behavior
		/// Dont call this for inactive behaviors
		/// </summary>
		/// <returns></returns>
		public abstract Vector2 GetSteering();

		public static IBehavior BehaviorFactory(BehaviorType behaviorType, IBoid boid)
		{
				switch (behaviorType)
				{
					case BehaviorType.WallAvoidance: { return new WallAvoidance(boid); }
					case BehaviorType.ObstacleAvoidance: { return new ObstacleAvoidance(boid); }
					case BehaviorType.Evade: { return new Evade(boid); }
					case BehaviorType.Flee: { return new Flee(boid); }
					case BehaviorType.Direction: { return new Direction(boid); }
					case BehaviorType.Separation: { return new Separation(boid); }
					case BehaviorType.Alignment: { return new Alignment(boid); }
					case BehaviorType.Cohesion: { return new Cohesion(boid); }
					case BehaviorType.Seek: { return new Seek(boid); }
					case BehaviorType.Arrive: { return new Arrive(boid); }
					case BehaviorType.Wander: { return new Wander(boid); }
					case BehaviorType.Pursuit: { return new Pursuit(boid); }
					case BehaviorType.OffsetPursuit: { return new OffsetPursuit(boid); }
					case BehaviorType.Interpose: { return new Interpose(boid); }
					case BehaviorType.Hide: { return new Hide(boid); }
					case BehaviorType.FollowPath: { return new FollowPath(boid); }
					case BehaviorType.GuardAlignment: { return new GuardAlignment(boid); }
					case BehaviorType.GuardCohesion: { return new GuardCohesion(boid); }
					case BehaviorType.GuardSeparation: { return new GuardSeparation(boid); }
					default: { throw new NotImplementedException(string.Format("Unhandled BehaviorType: {0}", behaviorType)); }
				}
			}

		#endregion //Methods
	}
}