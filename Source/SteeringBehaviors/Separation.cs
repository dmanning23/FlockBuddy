using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace FlockBuddy
{
	/// <summary>
	/// Group behavior to move boids away from each other
	/// </summary>
	public class Separation : BaseBehavior
	{
		#region Members

		/// <summary>
		/// The guys we are trying to align with
		/// </summary>
		private List<Mover> Buddies { get; set; }

		#endregion //Members

		#region Methods

		/// <summary>
		/// Initializes a new instance of the <see cref="FlockBuddy.Evade"/> class.
		/// </summary>
		public Separation(Boid dude)
			: base(dude, EBehaviorType.separation)
		{
			Weight = 120.0f;
		}

		/// <summary>
		/// Called every frame to get the steering direction from this behavior
		/// </summary>
		/// <param name="group">the group of this dude's buddies to align with</param>
		/// <returns></returns>
		public Vector2 GetSteering(List<Mover> group)
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
			Vector2 steeringForce = Vector2.Zero;

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
					steeringForce += toAgent / length;
				}
			}

			return steeringForce * Weight;
		}

		#endregion //Methods
	}
}