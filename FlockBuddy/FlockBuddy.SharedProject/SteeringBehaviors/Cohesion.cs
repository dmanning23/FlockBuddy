using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Diagnostics;

namespace FlockBuddy
{
	/// <summary>
	/// Group behavior to move boids together
	/// </summary>
	public class Cohesion : BaseBehavior, IFlockingBehavior
	{
		#region Properties

		/// <summary>
		/// The guys we are trying to align with
		/// </summary>
		public List<IMover> Buddies { private get; set; }

		private Seek SeekBehavior { get; set; }

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
				return 0f;
			}
		}

		#endregion //Properties

		#region Methods

		/// <summary>
		/// Initializes a new instance of the <see cref="FlockBuddy.Evade"/> class.
		/// </summary>
		public Cohesion(IBoid dude)
			: base(dude, EBehaviorType.cohesion, 0.5f)
		{
			Buddies = new List<IMover>();
			SeekBehavior = new Seek(dude);
		}

		/// <summary>
		/// Called every fram to get the steering direction from this behavior
		/// </summary>
		/// <returns></returns>
		public override Vector2 GetSteering()
		{
			//first find the center of mass of all the agents
			Vector2 centerOfMass = Vector2.Zero;
			Vector2 steeringForce = Vector2.Zero;

			if (null != Buddies && Buddies.Count > 0)
			{
				//iterate through the neighbors and sum up all the position vectors
				for (int i = 0; i < Buddies.Count; i++)
				{
					//make sure *this* agent isn't included in the calculations and that
					//the agent being examined is close enough ***also make sure it doesn't
					//include the evade target ***
					centerOfMass += Buddies[i].Position;
				}

				//the center of mass is the average of the sum of positions
				centerOfMass /= Buddies.Count;

				//now seek towards that position
				SeekBehavior.TargetPosition = centerOfMass;
				steeringForce = SeekBehavior.GetSteering();
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