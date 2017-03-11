using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Diagnostics;

namespace FlockBuddy
{
	/// <summary>
	/// Group behavior to move boids away from each other
	/// </summary>
	public class Separation : BaseBehavior, IFlockingBehavior
	{
		#region Properties

		/// <summary>
		/// The guys we are trying to align with
		/// </summary>
		public List<IMover> Buddies { private get; set; }

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
				return 0.5f;
			}
		}

		#endregion //Properties

		#region Methods

		/// <summary>
		/// Initializes a new instance of the <see cref="FlockBuddy.Evade"/> class.
		/// </summary>
		public Separation(IBoid dude)
			: base(dude, EBehaviorType.separation, BoidDefaults.SeparationWeight)
		{
		}

		/// <summary>
		/// Called every fram to get the steering direction from this behavior
		/// </summary>
		/// <returns></returns>
		public override Vector2 GetSteering()
		{
			Vector2 steeringForce = Vector2.Zero;

			if (null != Buddies)
			{
				for (int i = 0; i < Buddies.Count; i++)
				{
					//make sure this agent isn't included in the calculations and that the agent being examined is close enough. 
					//***also make sure it doesn't include the evade target ***
					Vector2 toAgent = Owner.Position - Buddies[i].Position;
					float length = toAgent.Length();

					if (length != 0.0f)
					{
						//normalize the vector
						toAgent /= length;

						//scale the force inversely proportional to the agents distance from its neighbor.
						steeringForce += toAgent / length ;
					}
				}

				Debug.Assert(!float.IsNaN(steeringForce.X));
				Debug.Assert(!float.IsNaN(steeringForce.Y));
			}

			return steeringForce * Weight;
		}

		#endregion //Methods
	}
}