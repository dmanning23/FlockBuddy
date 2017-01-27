using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace FlockBuddy
{
	/// <summary>
	/// Group behavior to move boids in the same direction as each other
	/// </summary>
	public class Alignment : BaseBehavior, IFlockingBehavior
	{
		#region Members

		/// <summary>
		/// The guys we are trying to align with
		/// </summary>
		private List<IMover> Buddies { get; set; }

		#endregion //Members

		#region Methods

		/// <summary>
		/// Initializes a new instance of the <see cref="FlockBuddy.Alignment"/> class.
		/// </summary>
		public Alignment(Boid dude)
			: base(dude, EBehaviorType.alignment, 10f)
		{
		}

		/// <summary>
		/// Called every frame to get the steering direction from this behavior
		/// </summary>
		/// <param name="group">the group of this dude's buddies to align with</param>
		/// <returns></returns>
		public Vector2 GetSteering(List<IMover> group)
		{
			Buddies = group;
			return GetSteering();
		}

		/// <summary>
		/// Called every frame to get the steering direction from this behavior
		/// </summary>
		/// <param name="time"></param>
		/// <returns></returns>
		public override Vector2 GetSteering()
		{
			//used to record the average heading of the neighbors
			Vector2 AverageHeading = Vector2.Zero;

			//used to count the number of vehicles in the neighborhood
			int NeighborCount = 0;

			//iterate through all the tagged vehicles and sum their heading vectors  
			for (int i = 0; i < Buddies.Count; i++)
			{
				//make sure *this* agent isn't included in the calculations 
				//and that the agent being examined  is close enough 
				AverageHeading += Buddies[i].Heading;
				++NeighborCount;
			}

			//if the neighborhood contained one or more vehicles, average their heading vectors.
			if (NeighborCount > 0)
			{
				AverageHeading /= NeighborCount;
				AverageHeading -= Owner.Heading;
			}

			//always multiply the return value by the weight
			return AverageHeading * Weight;
		}

		#endregion //Methods
	}
}