using MatrixExtensions;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace FlockBuddy
{
	/// <summary>
	/// this returns a steering force which will attempt to keep the agent away from any obstacles it may encounter
	/// </summary>
	public class ObstacleAvoidance : BaseBehavior, IObstacleBehavior
	{
		#region Properties

		public List<IBaseEntity> Obstacles { private get; set; }

		public override float DirectionChange
		{
			get
			{
				return 1f;
			}
		}

		public override float SpeedChange
		{
			get
			{
				return 1f;
			}
		}

		#endregion //Properties

		#region Methods

		/// <summary>
		/// Initializes a new instance of the <see cref="FlockBuddy.ObstacleAvoidance"/> class.
		/// </summary>
		public ObstacleAvoidance(IBoid dude)
			: base(dude, EBehaviorType.obstacle_avoidance, BoidDefaults.ObstacleAvoidanceWeight)
		{
		}

		/// <summary>
		/// Called every fram to get the steering direction from this behavior
		/// </summary>
		/// <returns></returns>
		public override Vector2 GetSteering()
		{
			//this will keep track of the closest intersecting obstacle (CIB)
			IBaseEntity closestIntersectingObstacle = null;

			//this will be used to track the distance to the CIB
			float distToClosestIP = float.MaxValue;

			//this will record the transformed local coordinates of the CIB
			Vector2 localPosOfClosestObstacle = Vector2.Zero;

			if (null != Obstacles)
			{
				for (int i = 0; i < Obstacles.Count; i++)
				{
					var curOb = Obstacles[i];

					//calculate this obstacle's position in local space
					Vector2 localPos = curOb.Position.ToLocalSpace(Owner.Heading, Owner.Side, Owner.Position);

					//if the local position has a negative x value then it must lay
					//behind the agent. (in which case it can be ignored)
					if (localPos.X >= 0)
					{
						//if the distance from the x axis to the object's position is less
						//than its radius + half the width of the detection box then there
						//is a potential intersection.
						double expandedRadius = curOb.Radius + Owner.Radius + 1;

						if (Math.Abs(localPos.Y) < expandedRadius)
						{
							//now to do a line/circle intersection test. The center of the 
							//circle is represented by (cX, cY). The intersection points are 
							//given by the formula x = cX +/-sqrt(r^2-cY^2) for y=0. 
							//We only need to look at the smallest positive value of x because
							//that will be the closest point of intersection.
							float cX = localPos.X;
							float cY = localPos.Y;

							//we only need to calculate the sqrt part of the above equation once
							float sqrtPart = (float)Math.Sqrt(expandedRadius * expandedRadius - cY * cY);

							float ip = cX - sqrtPart;

							if (ip <= 0.0)
							{
								ip = cX + sqrtPart;
							}

							//test to see if this is the closest so far. If it is keep a
							//record of the obstacle and its local coordinates
							if (ip < distToClosestIP)
							{
								distToClosestIP = ip;
								closestIntersectingObstacle = curOb;
								localPosOfClosestObstacle = localPos;
							}
						}
					}
				}
			}

			//if we have found an intersecting obstacle, calculate a steering force away from it
			Vector2 steeringForce = Vector2.Zero;
			if (null != closestIntersectingObstacle)
			{
				Vector2 toAgent = Owner.Position - closestIntersectingObstacle.Position;
				toAgent.Normalize();

				//get the distance to the edge of the obstacle
				Vector2 dist = (toAgent * closestIntersectingObstacle.Radius) - localPosOfClosestObstacle;

				//scale the force inversely proportional to the agents distance from the collision point
				float multiplier = 1.0f + (Owner.ObstacleQueryRadius - dist.X) / Owner.ObstacleQueryRadius;
				steeringForce = toAgent * multiplier;
			}

			return steeringForce * Weight;
		}

		#endregion //Methods
	}
}