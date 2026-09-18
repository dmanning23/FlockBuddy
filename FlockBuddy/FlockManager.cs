using FlockBuddy.Interfaces;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FlockBuddy
{
	public class FlockManager : IFlockManager
	{
		#region Fields

		private static int _debugColorIndex = 0;

		#endregion //Fields

		#region Properties

		/// <summary>
		/// Used for database persistance
		/// </summary>
		public virtual int? Id
		{
			get
			{
				return Flock.Id;
			}
			set
			{
				Flock.Id = value;
			}
		}

		public virtual string Name { get; set; }

		public Color DebugColor { get; private set; }

		private float _boidMass = BoidDefaults.BoidMass;
		public virtual float BoidMass
		{
			get
			{
				return _boidMass;
			}
			set
			{
				_boidMass = value;
				foreach (var mover in Flock.Boids)
				{
					if (mover is Boid boid)
					{
						boid.Mass = BoidMass;
					}
				}
			}
		}

		private float _boidMaxForce = BoidDefaults.BoidMaxForce;
		public virtual float BoidMaxForce
		{
			get
			{
				return _boidMaxForce;
			}
			set
			{
				_boidMaxForce = value;
				foreach (var mover in Flock.Boids)
				{
					if (mover is Boid boid)
					{
						boid.MaxForce = BoidMaxForce;
					}
				}
			}
		}

		private float _boidMinSpeed = BoidDefaults.BoidMinSpeed;
		public virtual float BoidMinSpeed
		{
			get
			{
				return _boidMinSpeed;
			}
			set
			{
				_boidMinSpeed = value;
				foreach (var mover in Flock.Boids)
				{
					if (mover is Boid boid)
					{
						boid.MinSpeed = BoidMinSpeed;
					}
				}
			}
		}

		private float _boidWalkSpeed = BoidDefaults.BoidWalkSpeed;
		public virtual float BoidWalkSpeed
		{
			get
			{
				return _boidWalkSpeed;
			}
			set
			{
				_boidWalkSpeed = value;
				foreach (var mover in Flock.Boids)
				{
					if (mover is Boid boid)
					{
						boid.WalkSpeed = BoidWalkSpeed;
					}
				}
			}
		}

		private float _boidLaziness = BoidDefaults.BoidLaziness;
		public virtual float BoidLaziness
		{
			get
			{
				return _boidLaziness;
			}
			set
			{
				_boidLaziness = value;
				foreach (var mover in Flock.Boids)
				{
					if (mover is Boid boid)
					{
						boid.Laziness = BoidLaziness;
					}
				}
			}
		}

		private float _boidMaxSpeed = BoidDefaults.BoidMaxSpeed;
		public virtual float BoidMaxSpeed
		{
			get
			{
				return _boidMaxSpeed;
			}
			set
			{
				_boidMaxSpeed = value;
				foreach (var mover in Flock.Boids)
				{
					if (mover is Boid boid)
					{
						boid.MaxSpeed = BoidMaxSpeed;
					}
				}
			}
		}

		private float _boidMaxTurnRate = BoidDefaults.BoidMaxTurnRate;
		public virtual float BoidMaxTurnRate
		{
			get
			{
				return _boidMaxTurnRate;
			}
			set
			{
				_boidMaxTurnRate = value;
				foreach (var mover in Flock.Boids)
				{
					if (mover is Boid boid)
					{
						boid.MaxTurnRate = BoidMaxTurnRate;
					}
				}
			}
		}

		private float _boidNeighborQueryRadius = BoidDefaults.BoidQueryRadius;
		public virtual float BoidNeighborQueryRadius
		{
			get
			{
				return _boidNeighborQueryRadius;
			}
			set
			{
				_boidNeighborQueryRadius = value;
				foreach (var mover in Flock.Boids)
				{
					if (mover is Boid boid)
					{
						boid.NeighborsQueryRadius = BoidNeighborQueryRadius;
					}
				}
			}
		}

		private float _boidPredatorQueryRadius = BoidDefaults.BoidQueryRadius;
		public virtual float BoidPredatorQueryRadius
		{
			get
			{
				return _boidPredatorQueryRadius;
			}
			set
			{
				_boidPredatorQueryRadius = value;
				foreach (var mover in Flock.Boids)
				{
					if (mover is Boid boid)
					{
						boid.PredatorsQueryRadius = BoidPredatorQueryRadius;
					}
				}
			}
		}

		private float _boidPreyQueryRadius = BoidDefaults.BoidQueryRadius;
		public virtual float BoidPreyQueryRadius
		{
			get
			{
				return _boidPreyQueryRadius;
			}
			set
			{
				_boidPreyQueryRadius = value;
				foreach (var mover in Flock.Boids)
				{
					if (mover is Boid boid)
					{
						boid.PreyQueryRadius = BoidPreyQueryRadius;
					}
				}
			}
		}

		private float _boidVipQueryRadius = BoidDefaults.BoidQueryRadius;
		public virtual float BoidVipQueryRadius
		{
			get
			{
				return _boidVipQueryRadius;
			}
			set
			{
				_boidVipQueryRadius = value;
				foreach (var mover in Flock.Boids)
				{
					if (mover is Boid boid)
					{
						boid.VipQueryRadius = BoidVipQueryRadius;
					}
				}
			}
		}

		private float _boidWallQueryRadius = BoidDefaults.BoidQueryRadius;
		public virtual float BoidWallQueryRadius
		{
			get
			{
				return _boidWallQueryRadius;
			}
			set
			{
				_boidWallQueryRadius = value;
				foreach (var mover in Flock.Boids)
				{
					if (mover is Boid boid)
					{
						boid.WallQueryRadius = BoidWallQueryRadius;
					}
				}
			}
		}

		private float _boidObstacleQueryRadius = BoidDefaults.BoidQueryRadius;
		public virtual float BoidObstacleQueryRadius
		{
			get
			{
				return _boidObstacleQueryRadius;
			}
			set
			{
				_boidObstacleQueryRadius = value;
				foreach (var mover in Flock.Boids)
				{
					if (mover is Boid boid)
					{
						boid.ObstacleQueryRadius = BoidObstacleQueryRadius;
					}
				}
			}
		}

		private float _boidWaypointQueryRadius = BoidDefaults.BoidQueryRadius;
		public virtual float BoidWaypointQueryRadius
		{
			get
			{
				return _boidWaypointQueryRadius;
			}
			set
			{
				_boidWaypointQueryRadius = value;
				foreach (var mover in Flock.Boids)
				{
					if (mover is Boid boid)
					{
						boid.WaypointQueryRadius = BoidWaypointQueryRadius;
					}
				}
			}
		}

		private float _boidRadius = BoidDefaults.BoidRadius;
		public virtual float BoidRadius
		{
			get
			{
				return _boidRadius;
			}
			set
			{
				_boidRadius = value;
				foreach (var boid in Flock.Boids)
				{
					boid.Radius = BoidRadius;
				}
			}
		}

		private float _boidRetargetTime = BoidDefaults.BoidRetargetTime;
		public virtual float BoidRetargetTime
		{
			get
			{
				return _boidRetargetTime;
			}
			set
			{
				_boidRetargetTime = value;
				foreach (var mover in Flock.Boids)
				{
					if (mover is Boid boid)
					{
						boid.RetargetTime = BoidRetargetTime;
					}
				}
			}
		}

		public virtual IFlock Flock { get; set; }

		private SummingMethod _summingMethod = BoidDefaults.DefaultSummingMethod;
		public virtual SummingMethod SummingMethod
		{
			get
			{
				return _summingMethod;
			}
			set
			{
				_summingMethod = value;
				foreach (var mover in Flock.Boids)
				{
					if (mover is Boid boid)
					{
						boid.SummingMethod = SummingMethod;
					}
				}
			}
		}

		public virtual DefaultWalls Walls { get; set; } = BoidDefaults.Walls;

		public List<BehaviorTemplate> Behaviors { get; private set; }

		#endregion //Properties

		#region Methods

		static FlockManager()
		{
			_debugColorIndex = 0;
		}

		protected FlockManager()
		{
			SetDebugColor();
			Name = DebugColor.ToString();
		}

		public FlockManager(IFlock flock) : this()
		{
			Behaviors = new List<BehaviorTemplate>();
			Flock = flock;
		}

		public FlockManager(IFlockManager flockManager) : this()
		{
			Flock = new Flock();
			Behaviors = new List<BehaviorTemplate>();

			Id = flockManager.Id;
			Name = flockManager.Name;
			BoidRadius = flockManager.BoidRadius;
			BoidMass = flockManager.BoidMass;
			BoidMinSpeed = flockManager.BoidMinSpeed;
			BoidWalkSpeed = flockManager.BoidWalkSpeed;
			BoidLaziness = flockManager.BoidLaziness;
			BoidMaxSpeed = flockManager.BoidMaxSpeed;
			BoidMaxTurnRate = flockManager.BoidMaxTurnRate;
			BoidMaxForce = flockManager.BoidMaxForce;
			BoidNeighborQueryRadius = flockManager.BoidNeighborQueryRadius;
			BoidPredatorQueryRadius = flockManager.BoidPredatorQueryRadius;
			BoidPreyQueryRadius = flockManager.BoidPreyQueryRadius;
			BoidVipQueryRadius = flockManager.BoidVipQueryRadius;
			BoidObstacleQueryRadius = flockManager.BoidObstacleQueryRadius;
			BoidWallQueryRadius = flockManager.BoidWallQueryRadius;
			BoidWaypointQueryRadius = flockManager.BoidWaypointQueryRadius;
			BoidRetargetTime = flockManager.BoidRetargetTime;
			SummingMethod = flockManager.SummingMethod;
			Walls = flockManager.Walls;
		}

		private void SetDebugColor()
		{
			switch (_debugColorIndex++)
			{
				case 0: { DebugColor = Color.Red; } break;
				case 1: { DebugColor = Color.Orange; } break;
				case 2: { DebugColor = Color.Yellow; } break;
				case 3: { DebugColor = Color.Green; } break;
				case 4: { DebugColor = Color.Blue; } break;
				case 5: { DebugColor = Color.Purple; } break;
				case 6: { DebugColor = Color.Pink; } break;
				case 7: { DebugColor = Color.Brown; } break;
				case 8: { DebugColor = Color.White; } break;
				default:
					{
						DebugColor = Color.Black;
						_debugColorIndex = 0;
					}
					break;
			}
		}

		public IBehavior AddBehavior(BehaviorType behaviorType)
		{
			float weight = 0f;

			switch (behaviorType)
			{
				case BehaviorType.WallAvoidance: { weight = BoidDefaults.WallAvoidanceWeight; } break;
				case BehaviorType.ObstacleAvoidance: { weight = BoidDefaults.ObstacleAvoidanceWeight; } break;
				case BehaviorType.Evade: { weight = BoidDefaults.EvadeWeight; } break;
				case BehaviorType.Flee: { weight = BoidDefaults.FleeWeight; } break;
				case BehaviorType.Separation: { weight = BoidDefaults.SeparationWeight; } break;
				case BehaviorType.Alignment: { weight = BoidDefaults.AlignmentWeight; } break;
				case BehaviorType.Cohesion: { weight = BoidDefaults.CohesionWeight; } break;
				case BehaviorType.Seek: { weight = BoidDefaults.SeekWeight; } break;
				case BehaviorType.Arrive: { weight = BoidDefaults.ArriveWeight; } break;
				case BehaviorType.Wander: { weight = BoidDefaults.WanderWeight; } break;
				case BehaviorType.Pursuit: { weight = BoidDefaults.PursuitWeight; } break;
				case BehaviorType.OffsetPursuit: { weight = BoidDefaults.OffsetPursuitWeight; } break;
				case BehaviorType.Interpose: { weight = BoidDefaults.InterposeWeight; } break;
				case BehaviorType.Hide: { weight = BoidDefaults.HideWeight; } break;
				case BehaviorType.FollowPath: { weight = BoidDefaults.FollowPathWeight; } break;
				case BehaviorType.GuardAlignment: { weight = BoidDefaults.AlignmentWeight; } break;
				case BehaviorType.GuardCohesion: { weight = BoidDefaults.CohesionWeight; } break;
				case BehaviorType.GuardSeparation: { weight = BoidDefaults.SeparationWeight; } break;
				default: { throw new NotImplementedException(string.Format("Unhandled BehaviorType: {0}", behaviorType)); }
			}

			return AddBehavior(behaviorType, weight);
		}

		public IBehavior AddBehavior(BehaviorType behaviorType, float weight)
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

		public void SetBehaviorWeight(BehaviorType behaviorType, float weight)
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

		public void RemoveBehavior(BehaviorType behaviorType)
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

		public virtual IBoid AddBoid(Vector2 position, Vector2 heading)
		{
			var boid = BoidFactory(position, heading);
			InitializeBoidBehaviors(boid);
			return boid;
		}

		public virtual IBoid BoidFactory(Vector2 position, Vector2 heading)
		{
			var boid = new Boid(Flock,
					position, //_random.NextVector2(0f, 1280f, 0f, 720f),
					heading, //_random.NextVector2(-1f, 1f, -1f, 1f).Normalized(),
					0,
					BoidRadius);
			InitializeBoid(boid);
			return boid;
		}

		protected void InitializeBoid(IBoid boid)
		{
			boid.Initialize(this.BoidMass,
				this.BoidMinSpeed,
				this.BoidWalkSpeed,
				this.BoidLaziness,
				this.BoidMaxSpeed,
				this.BoidMaxForce,
				this.BoidMaxTurnRate,
				this.SummingMethod,
				this.BoidNeighborQueryRadius,
				this.BoidPredatorQueryRadius,
				this.BoidPreyQueryRadius,
				this.BoidVipQueryRadius,
				this.BoidWallQueryRadius,
				this.BoidObstacleQueryRadius,
				this.BoidWaypointQueryRadius,
				this.BoidRetargetTime);
		}

		protected void InitializeBoidBehaviors(IBoid boid)
		{
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

		public bool HasBehavior(BehaviorType behavior)
		{
			return Behaviors.Exists(x => x.BehaviorType == behavior);
		}

		public IEnumerable<BehaviorType> GetAllBehaviors()
		{
			return Behaviors.Select(x => x.BehaviorType);
		}

		public float GetBehaviorWeight(BehaviorType behavior)
		{
			return Behaviors
				.Where(x => x.BehaviorType == behavior)
				.Select(x => x.Weight)
				.First();
		}

		#endregion //Methods
	}
}
