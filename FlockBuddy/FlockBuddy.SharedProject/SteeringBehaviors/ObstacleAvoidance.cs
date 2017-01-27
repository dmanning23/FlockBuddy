using MatrixExtensions;
using Microsoft.Xna.Framework;
using System;

namespace FlockBuddy
{
	/// <summary>
	/// this returns a steering force which will attempt to keep the agent away from any obstacles it may encounter
	/// </summary>
	public class ObstacleAvoidance : BaseBehavior, IObstacleBehavior
	{
		#region Members

		/// <summary>
		/// how far the obstacle avoidance behvaior should watch for obastcles
		/// </summary>
		public float AvoidanceDetectionDistance { get; set; }

		#endregion //Members

		#region Methods

		/// <summary>
		/// Initializes a new instance of the <see cref="FlockBuddy.ObstacleAvoidance"/> class.
		/// </summary>
		public ObstacleAvoidance(Boid dude)
			: base(dude, EBehaviorType.obstacle_avoidance, 30f)
		{
			AvoidanceDetectionDistance = 100.0f;
		}

		public Vector2 GetSteering2()
		{
			return GetSteering();
		}

		/// <summary>
		/// Called every fram to get the steering direction from this behavior
		/// </summary>
		/// <returns></returns>
		public override Vector2 GetSteering()
		{
			//the detection box length is proportional to the agent's velocity
			float boxLength = AvoidanceDetectionDistance + (Owner.Speed / Owner.MaxSpeed) * AvoidanceDetectionDistance;

			//tag all obstacles within range of the box for processing
			var obs = Owner.MyFlock.TagObstacles(Owner, boxLength);

			//this will keep track of the closest intersecting obstacle (CIB)
			IBaseEntity closestIntersectingObstacle = null;

			//this will be used to track the distance to the CIB
			float distToClosestIP = float.MaxValue;

			//this will record the transformed local coordinates of the CIB
			Vector2 localPosOfClosestObstacle = Vector2.Zero;

			for (int i = 0; i < obs.Count; i++)
			{
				var curOb = obs[i];

				//calculate this obstacle's position in local space
				Vector2 localPos = curOb.Position.ToLocalSpace(Owner.Heading, Owner.Side, Owner.Position);

				//if the local position has a negative x value then it must lay
				//behind the agent. (in which case it can be ignored)
				if (localPos.X >= 0)
				{
					//if the distance from the x axis to the object's position is less
					//than its radius + half the width of the detection box then there
					//is a potential intersection.
					double expandedRadius = curOb.BoundingRadius + Owner.BoundingRadius + 1;

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

			//if we have found an intersecting obstacle, calculate a steering force away from it
			Vector2 steeringForce = Vector2.Zero;
			if (null != closestIntersectingObstacle)
			{
				Vector2 toAgent = Owner.Position - closestIntersectingObstacle.Position;
				toAgent.Normalize();

				//get the distance to the edge of the obstacle
				Vector2 dist = (toAgent * closestIntersectingObstacle.BoundingRadius) - localPosOfClosestObstacle;

				//scale the force inversely proportional to the agents distance from the collision point
				float multiplier = 1.0f + (AvoidanceDetectionDistance - dist.X) / AvoidanceDetectionDistance;
				steeringForce = toAgent * multiplier;
			}

			return steeringForce * Weight;
		}

		#endregion //Methods
	}
}