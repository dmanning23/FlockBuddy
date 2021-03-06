﻿using Microsoft.Xna.Framework;
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

		private float _boidMass;
		public virtual float BoidMass
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
					foreach (var mover in Flock.Boids)
					{
						var boid = mover as IBoid;
						if (null != boid)
						{
							boid.Mass = BoidMass;
						}
					}
				}
			}
		}

		private float _boidMaxForce;
		public virtual float BoidMaxForce
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
					foreach (var mover in Flock.Boids)
					{
						var boid = mover as IBoid;
						if (null != boid)
						{
							boid.MaxForce = BoidMaxForce;
						}
					}
				}
			}
		}

		private float _boidMinSpeed;
		public virtual float BoidMinSpeed
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
					foreach (var mover in Flock.Boids)
					{
						var boid = mover as IBoid;
						if (null != boid)
						{
							boid.MinSpeed = BoidMinSpeed;
						}
					}
				}
			}
		}

		private float _boidWalkSpeed;
		public virtual float BoidWalkSpeed
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
					foreach (var mover in Flock.Boids)
					{
						var boid = mover as IBoid;
						if (null != boid)
						{
							boid.WalkSpeed = BoidWalkSpeed;
						}
					}
				}
			}
		}

		private float _boidLaziness;
		public virtual float BoidLaziness
		{
			get
			{
				return _boidLaziness;
			}
			set
			{
				if (BoidLaziness != value)
				{
					_boidLaziness = value;
					foreach (var mover in Flock.Boids)
					{
						var boid = mover as IBoid;
						if (null != boid)
						{
							boid.Laziness = BoidLaziness;
						}
					}
				}
			}
		}

		private float _boidMaxSpeed;
		public virtual float BoidMaxSpeed
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
					foreach (var mover in Flock.Boids)
					{
						var boid = mover as IBoid;
						if (null != boid)
						{
							boid.MaxSpeed = BoidMaxSpeed;
						}
					}
				}
			}
		}

		private float _boidMaxTurnRate;
		public virtual float BoidMaxTurnRate
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
					foreach (var mover in Flock.Boids)
					{
						var boid = mover as IBoid;
						if (null != boid)
						{
							boid.MaxTurnRate = BoidMaxTurnRate;
						}
					}
				}
			}
		}

		private float _boidNeighborQueryRadius;
		public virtual float BoidNeighborQueryRadius
		{
			get
			{
				return _boidNeighborQueryRadius;
			}
			set
			{
				if (BoidNeighborQueryRadius != value)
				{
					_boidNeighborQueryRadius = value;
					foreach (var mover in Flock.Boids)
					{
						var boid = mover as IBoid;
						if (null != boid)
						{
							boid.NeighborsQueryRadius = BoidNeighborQueryRadius;
						}
					}
				}
			}
		}

		private float _boidPredatorQueryRadius;
		public virtual float BoidPredatorQueryRadius
		{
			get
			{
				return _boidPredatorQueryRadius;
			}
			set
			{
				if (BoidPredatorQueryRadius != value)
				{
					_boidPredatorQueryRadius = value;
					foreach (var mover in Flock.Boids)
					{
						var boid = mover as IBoid;
						if (null != boid)
						{
							boid.PredatorsQueryRadius = BoidPredatorQueryRadius;
						}
					}
				}
			}
		}

		private float _boidPreyQueryRadius;
		public virtual float BoidPreyQueryRadius
		{
			get
			{
				return _boidPreyQueryRadius;
			}
			set
			{
				if (BoidPreyQueryRadius != value)
				{
					_boidPreyQueryRadius = value;
					foreach (var mover in Flock.Boids)
					{
						var boid = mover as IBoid;
						if (null != boid)
						{
							boid.PreyQueryRadius = BoidPreyQueryRadius;
						}
					}
				}
			}
		}

		private float _boidVipQueryRadius;
		public virtual float BoidVipQueryRadius
		{
			get
			{
				return _boidVipQueryRadius;
			}
			set
			{
				if (BoidVipQueryRadius != value)
				{
					_boidVipQueryRadius = value;
					foreach (var mover in Flock.Boids)
					{
						var boid = mover as IBoid;
						if (null != boid)
						{
							boid.VipQueryRadius = BoidVipQueryRadius;
						}
					}
				}
			}
		}

		private float _boidWallQueryRadius;
		public virtual float BoidWallQueryRadius
		{
			get
			{
				return _boidWallQueryRadius;
			}
			set
			{
				if (BoidWallQueryRadius != value)
				{
					_boidWallQueryRadius = value;
					foreach (var mover in Flock.Boids)
					{
						var boid = mover as IBoid;
						if (null != boid)
						{
							boid.WallQueryRadius = BoidWallQueryRadius;
						}
					}
				}
			}
		}

		private float _boidObstacleQueryRadius;
		public virtual float BoidObstacleQueryRadius
		{
			get
			{
				return _boidObstacleQueryRadius;
			}
			set
			{
				if (BoidObstacleQueryRadius != value)
				{
					_boidObstacleQueryRadius = value;
					foreach (var mover in Flock.Boids)
					{
						var boid = mover as IBoid;
						if (null != boid)
						{
							boid.ObstacleQueryRadius = BoidObstacleQueryRadius;
						}
					}
				}
			}
		}

		private float _boidWaypointQueryRadius;
		public virtual float BoidWaypointQueryRadius
		{
			get
			{
				return _boidWaypointQueryRadius;
			}
			set
			{
				if (BoidWaypointQueryRadius != value)
				{
					_boidWaypointQueryRadius = value;
					foreach (var mover in Flock.Boids)
					{
						var boid = mover as IBoid;
						if (null != boid)
						{
							boid.WaypointQueryRadius = BoidWaypointQueryRadius;
						}
					}
				}
			}
		}

		private float _boidRadius;
		public virtual float BoidRadius
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
		public virtual float BoidRetargetTime
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
						if (null != boid)
						{
							boid.RetargetTime = BoidRetargetTime;
						}
					}
				}
			}
		}

		public virtual IFlock Flock { get; set; }

		private ESummingMethod _summingMethod;
		public virtual ESummingMethod SummingMethod
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
						if (null != boid)
						{
							boid.SummingMethod = SummingMethod;
						}
					}
				}
			}
		}

		public virtual DefaultWalls Walls { get; set; }

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
			Flock = flock;
			Behaviors = new List<BehaviorTemplate>();

			BoidRadius = BoidDefaults.BoidRadius;
			BoidMass = BoidDefaults.BoidMass;
			BoidMinSpeed = BoidDefaults.BoidMinSpeed;
			BoidWalkSpeed = BoidDefaults.BoidWalkSpeed;
			BoidLaziness = BoidDefaults.BoidLaziness;
			BoidMaxSpeed = BoidDefaults.BoidMaxSpeed;
			BoidMaxTurnRate = BoidDefaults.BoidMaxTurnRate;
			BoidMaxForce = BoidDefaults.BoidMaxForce;
			BoidNeighborQueryRadius = BoidDefaults.BoidQueryRadius;
			BoidPredatorQueryRadius = BoidDefaults.BoidQueryRadius;
			BoidPreyQueryRadius = BoidDefaults.BoidQueryRadius;
			BoidVipQueryRadius = BoidDefaults.BoidQueryRadius;
			BoidRetargetTime = BoidDefaults.BoidRetargetTime;
			SummingMethod = BoidDefaults.SummingMethod;
			Walls = BoidDefaults.Walls;
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
				case EBehaviorType.guard_alignment: { weight = BoidDefaults.AlignmentWeight; } break;
				case EBehaviorType.guard_cohesion: { weight = BoidDefaults.CohesionWeight; } break;
				case EBehaviorType.guard_separation: { weight = BoidDefaults.SeparationWeight; } break;
				default: { throw new NotImplementedException(string.Format("Unhandled EBehaviorType: {0}", behaviorType)); }
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

		public virtual IBoid AddBoid(Vector2 position, Vector2 heading, float walkSpeed)
		{
			var boid = BoidFactory(position, heading, walkSpeed);
			InitializeBoidBehaviors(boid);
			return boid;
		}

		public virtual IBoid BoidFactory(Vector2 position, Vector2 heading, float walkSpeed)
		{
			var boid = new Boid(Flock,
					position, //_random.NextVector2(0f, 1280f, 0f, 720f),
					heading, //_random.NextVector2(-1f, 1f, -1f, 1f).Normalized(),
					walkSpeed, //_random.NextFloat(BoidWalkSpeed, BoidMaxSpeed),
					BoidRadius);
			InitializeBoid(boid);
			return boid;
		}

		protected void InitializeBoid(IBoid boid)
		{
			boid.Mass = this.BoidMass;
			boid.MinSpeed = this.BoidMinSpeed;
			boid.WalkSpeed = this.BoidWalkSpeed;
			boid.Laziness = this.BoidLaziness;
			boid.MaxSpeed = this.BoidMaxSpeed;
			boid.MaxTurnRate = this.BoidMaxTurnRate;
			boid.MaxForce = this.BoidMaxForce;
			boid.RetargetTime = this.BoidRetargetTime;
			boid.SummingMethod = this.SummingMethod;
			boid.NeighborsQueryRadius = this.BoidNeighborQueryRadius;
			boid.ObstacleQueryRadius = this.BoidObstacleQueryRadius;
			boid.PredatorsQueryRadius = this.BoidPredatorQueryRadius;
			boid.PreyQueryRadius = this.BoidPreyQueryRadius;
			boid.VipQueryRadius = this.BoidVipQueryRadius;
			boid.WallQueryRadius = this.BoidWallQueryRadius;
			boid.WaypointQueryRadius = this.BoidWaypointQueryRadius;
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
