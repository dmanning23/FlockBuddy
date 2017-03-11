using Microsoft.Xna.Framework;
using RandomExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using Vector2Extensions;

namespace FlockBuddy
{
	public class FlockManager
	{
		#region Fields

		Random _random = new Random();

		#endregion //Fields

		#region Properties

		private float _boidMass;
		public float BoidMass
		{
			get
			{
				return _boidMass;
			}
			set
			{
				if (BoidMass != value)
				{
					_boidMass = value;
					foreach (var boid in Flock.Boids)
					{
						boid.Mass = BoidMass;
					}
				}
			}
		}

		private float _boidMaxForce;
		public float BoidMaxForce
		{
			get
			{
				return _boidMaxForce;
			}
			set
			{
				if (BoidMaxForce != value)
				{
					_boidMaxForce = value;
					foreach (var boid in Flock.Boids)
					{
						boid.MaxForce = BoidMaxForce;
					}
				}
			}
		}

		private float _boidMinSpeed;
		public float BoidMinSpeed
		{
			get
			{
				return _boidMinSpeed;
			}
			set
			{
				if (BoidMinSpeed != value)
				{
					_boidMinSpeed = value;
					foreach (var boid in Flock.Boids)
					{
						boid.MinSpeed = BoidMinSpeed;
					}
				}
			}
		}

		private float _boidWalkSpeed;
		public float BoidWalkSpeed
		{
			get
			{
				return _boidWalkSpeed;
			}
			set
			{
				if (BoidWalkSpeed != value)
				{
					_boidWalkSpeed = value;
					foreach (var boid in Flock.Boids)
					{
						boid.WalkSpeed = BoidWalkSpeed;
					}
				}
			}
		}

		private float _boidMaxSpeed;
		public float BoidMaxSpeed
		{
			get
			{
				return _boidMaxSpeed;
			}
			set
			{
				if (BoidMaxSpeed != value)
				{
					_boidMaxSpeed = value;
					foreach (var boid in Flock.Boids)
					{
						boid.MaxSpeed = BoidMaxSpeed;
					}
				}
			}
		}

		private float _boidMaxTurnRate;
		public float BoidMaxTurnRate
		{
			get
			{
				return _boidMaxTurnRate;
			}
			set
			{
				if (BoidMaxTurnRate != value)
				{
					_boidMaxTurnRate = value;
					foreach (var boid in Flock.Boids)
					{
						boid.MaxTurnRate = BoidMaxTurnRate;
					}
				}
			}
		}

		private float _boidQueryRadius;
		public float BoidQueryRadius
		{
			get
			{
				return _boidQueryRadius;
			}
			set
			{
				if (BoidQueryRadius != value)
				{
					_boidQueryRadius = value;
					foreach (var mover in Flock.Boids)
					{
						var boid = mover as IBoid;
						boid.QueryRadius = BoidQueryRadius;
					}
				}
			}
		}

		private float _boidRadius;
		public float BoidRadius
		{
			get
			{
				return _boidRadius;
			}
			set
			{
				if (BoidRadius != value)
				{
					_boidRadius = value;
					foreach (var boid in Flock.Boids)
					{
						boid.Radius = BoidRadius;
					}
				}
			}
		}

		private float _boidRetargetTime;
		public float BoidRetargetTime
		{
			get
			{
				return _boidRetargetTime;
			}
			set
			{
				if (BoidRetargetTime != value)
				{
					_boidRetargetTime = value;
					foreach (var mover in Flock.Boids)
					{
						var boid = mover as IBoid;
						boid.RetargetTime = BoidRetargetTime;
					}
				}
			}
		}

		public IFlock Flock { get; set; }

		private ESummingMethod _summingMethod;
		public ESummingMethod SummingMethod
		{
			get
			{
				return _summingMethod;
			}
			set
			{
				if (SummingMethod != value)
				{
					_summingMethod = value;
					foreach (var mover in Flock.Boids)
					{
						var boid = mover as IBoid;
						boid.SummingMethod = SummingMethod;
					}
				}
			}
		}

		public DefaultWalls Walls { get; private set; }

		public Color DebugColor { get; set; }

		private class BehaviorTemplate : IBehavior
		{
			public EBehaviorType BehaviorType { get; set; }

			public float Weight { get; set; }

			public IBoid Owner
			{
				get
				{
					throw new NotImplementedException();
				}

				set
				{
					throw new NotImplementedException();
				}
			}

			public Vector2 GetSteering()
			{
				throw new NotImplementedException();
			}

			public float DirectionChange
			{
				get
				{
					throw new NotImplementedException();
				}
			}

			public float SpeedChange
			{
				get
				{
					throw new NotImplementedException();
				}
			}
		}

		private List<BehaviorTemplate> Behaviors { get; set; }

		#endregion //Properties

		#region Methods

		public FlockManager(IFlock flock)
		{
			Flock = flock;
			Behaviors = new List<BehaviorTemplate>();

			BoidRadius = BoidDefaults.BoidRadius;
			BoidMass = BoidDefaults.BoidMass;
			BoidMinSpeed = BoidDefaults.BoidMinSpeed;
			BoidWalkSpeed = BoidDefaults.BoidWalkSpeed;
			BoidMaxSpeed = BoidDefaults.BoidMaxSpeed;
			BoidMaxTurnRate = BoidDefaults.BoidMaxTurnRate;
			BoidMaxForce = BoidDefaults.BoidMaxForce;
			BoidQueryRadius = BoidDefaults.BoidQueryRadius;
			BoidRetargetTime = BoidDefaults.BoidRetargetTime;
			SummingMethod = BoidDefaults.SummingMethod;
			Walls = BoidDefaults.Walls;
		}

		public IBehavior AddBehavior(EBehaviorType behaviorType)
		{
			float weight = 0f;

			switch (behaviorType)
			{
				case EBehaviorType.wall_avoidance: { weight = BoidDefaults.WallAvoidanceWeight; } break;
				case EBehaviorType.obstacle_avoidance: { weight = BoidDefaults.ObstacleAvoidanceWeight; } break;
				case EBehaviorType.evade: { weight = BoidDefaults.EvadeWeight; } break;
				case EBehaviorType.flee: { weight = BoidDefaults.FleeWeight; } break;
				case EBehaviorType.separation: { weight = BoidDefaults.SeparationWeight; } break;
				case EBehaviorType.alignment: { weight = BoidDefaults.AlignmentWeight; } break;
				case EBehaviorType.cohesion: { weight = BoidDefaults.CohesionWeight; } break;
				case EBehaviorType.seek: { weight = BoidDefaults.SeekWeight; } break;
				case EBehaviorType.arrive: { weight = BoidDefaults.ArriveWeight; } break;
				case EBehaviorType.wander: { weight = BoidDefaults.WanderWeight; } break;
				case EBehaviorType.pursuit: { weight = BoidDefaults.PursuitWeight; } break;
				case EBehaviorType.offset_pursuit: { weight = BoidDefaults.OffsetPursuitWeight; } break;
				case EBehaviorType.interpose: { weight = BoidDefaults.InterposeWeight; } break;
				case EBehaviorType.hide: { weight = BoidDefaults.HideWeight; } break;
				case EBehaviorType.follow_path: { weight = BoidDefaults.FollowPathWeight; } break;
			}

			return AddBehavior(behaviorType, weight);
		}

		public IBehavior AddBehavior(EBehaviorType behaviorType, float weight)
		{
			var behavior = new BehaviorTemplate()
			{
				BehaviorType = behaviorType,
				Weight = weight
			};
			Behaviors.Add(behavior);

			//go through the existing boids and add the behavior
			foreach (var mover in Flock.Boids)
			{
				var boid = mover as IBoid;
				boid.AddBehavior(behaviorType, weight);
			}

			return behavior;
		}

		public void SetBehaviorWeight(EBehaviorType behaviorType, float weight)
		{
			var behavior = Behaviors.Where(x => x.BehaviorType == behaviorType).First();
			behavior.Weight = weight;

			//go through the existing boids and update the weight
			foreach (var mover in Flock.Boids)
			{
				var boid = mover as IBoid;
				boid.AddBehavior(behaviorType, weight);
			}
		}

		public void RemoveBehavior(EBehaviorType behaviorType)
		{
			var behavior = Behaviors.Where(x => x.BehaviorType == behaviorType).First();
			if (behavior != null)
			{
				Behaviors.Remove(behavior);
			}

			foreach (var mover in Flock.Boids)
			{
				var boid = mover as IBoid;
				boid.RemoveBehavior(behavior.BehaviorType);
			}
		}

		public void AddBoid()
		{
			var boid = new Boid(Flock,
					_random.NextVector2(0f, 1280f, 0f, 720f),
					_random.NextVector2(-1f, 1f, -1f, 1f).Normalized(),
					_random.NextFloat(0f, BoidMaxSpeed),
					BoidRadius,
					BoidMass,
					BoidMinSpeed,
					BoidWalkSpeed,
					BoidMaxSpeed,
					BoidMaxTurnRate,
					BoidMaxForce,
					BoidQueryRadius,
					BoidRetargetTime,
					SummingMethod);

			//add all the behaviors
			foreach (var behavior in Behaviors)
			{
				boid.AddBehavior(behavior.BehaviorType, behavior.Weight);
			}
		}

		public void RemoveBoid()
		{
			if (Flock.Boids.Count > 0)
			{
				Flock.RemoveBoid(Flock.Boids[0]);
			}
		}

		public void AddDefaultWalls(DefaultWalls wallsType, Rectangle rect)
		{
			Walls = wallsType;
			Flock.AddDefaultWalls(Walls, rect);
		}

		public bool HasBehavior(EBehaviorType behavior)
		{
			return Behaviors.Exists(x => x.BehaviorType == behavior);
		}

		public IEnumerable<EBehaviorType> GetAllBehaviors()
		{
			return Behaviors.Select(x => x.BehaviorType);
		}

		public float GetBehaviorWeight(EBehaviorType behavior)
		{
			return Behaviors
				.Where(x => x.BehaviorType == behavior)
				.Select(x => x.Weight)
				.First();
		}

		#endregion //Methods
	}
}
