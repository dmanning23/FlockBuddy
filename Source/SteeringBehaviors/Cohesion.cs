using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Xna.Framework;

namespace FlockBuddy
{
	/// <summary>
	/// Group behavior to move boids together
	/// </summary>
	public class Cohesion : BaseBehavior
	{
		#region Members

		/// <summary>
		/// The guys we are trying to align with
		/// </summary>
		private List<Boid> Buddies { get; set; }

		private Seek SeekBehavior { get; set; }

		#endregion //Members

		#region Methods

		/// <summary>
		/// Initializes a new instance of the <see cref="FlockBuddy.Evade"/> class.
		/// </summary>
		public Cohesion(Boid dude)
			: base(dude, EBehaviorType.cohesion)
		{
			SeekBehavior = new Seek(dude);
		}

		/// <summary>
		/// Called every frame to get the steering direction from this behavior
		/// </summary>
		/// <param name="group">the group of this dude's buddies to align with</param>
		/// <returns></returns>
		public Vector2 GetSteering(List<Boid> group)
		{
			Buddies = group;
			return GetSteering();
		}

		/// <summary>
		/// Called every fram to get the steering direction from this behavior
		/// </summary>
		/// <returns></returns>
		protected override Vector2 GetSteering()
		{
			//first find the center of mass of all the agents
			Vector2 centerOfMass = Vector2.Zero;
			Vector2 steeringForce = Vector2.Zero;

			int neighborCount = 0;

			//iterate through the neighbors and sum up all the position vectors
			for (int i = 0; i < Buddies.Count; i++)
			{
				//make sure *this* agent isn't included in the calculations and that
				//the agent being examined is close enough ***also make sure it doesn't
				//include the evade target ***
				if (Buddies[i].Tagged && (Buddies[i].ID != Owner.ID))
				{
					centerOfMass += Buddies[i].Position;
					neighborCount++;
				}
			}

			if (neighborCount > 0)
			{
				//the center of mass is the average of the sum of positions
				centerOfMass /= neighborCount;

				//now seek towards that position
				steeringForce = SeekBehavior.GetSteering(centerOfMass);
				Debug.Assert(!float.IsNaN(steeringForce.X));
				Debug.Assert(!float.IsNaN(steeringForce.Y));
			}

			//the magnitude of cohesion is usually much larger than separation or
			//allignment so it usually helps to normalize it.
			if (steeringForce.LengthSquared() > 0.0f)
			{
				steeringForce.Normalize();
			}
			return steeringForce * Weight;
		}

		#endregion //Methods
	}
}