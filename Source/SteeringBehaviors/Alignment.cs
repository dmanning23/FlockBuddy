using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace FlockBuddy
{
	/// <summary>
	/// Group behavior to move boids in the same direction as each other
	/// </summary>
	public class Alignment : BaseBehavior, IGroupBehavior, IPreyBehavior
	{
		#region Members

		/// <summary>
		/// The guys we are trying to align with
		/// </summary>
		private List<Boid> Buddies { get; set; }

		/// <summary>
		/// a bad guy, don't align with him
		/// </summary>
		private Boid Enemy1 { get; set; }

		/// <summary>
		/// a bad guy, don't align with him
		/// </summary>
		private Boid Enemy2 { get; set; }

		#endregion //Members

		#region Methods

		/// <summary>
		/// Initializes a new instance of the <see cref="FlockBuddy.Alignment"/> class.
		/// </summary>
		public Alignment(Boid dude)
			: base(dude, EBehaviorType.alignment)
		{
		}

		public void SetGroup(List<Boid> group)
		{
			Buddies = group;
		}

		public void SetBadGuys(Boid badGuy1, Boid badGuy2)
		{
			Enemy1 = badGuy1;
			Enemy2 = badGuy2;
		}

		/// <summary>
		/// Called every fram to get the steering direction from this behavior
		/// </summary>
		/// <param name="time"></param>
		/// <returns></returns>
		public override Vector2 GetSteering(GameTime time)
		{
			//used to record the average heading of the neighbors
			Vector2 AverageHeading = Vector2.Zero;

			//used to count the number of vehicles in the neighborhood
			int NeighborCount = 0;

			//iterate through all the tagged vehicles and sum their heading vectors  
			for (int i = 0; i < Buddies.Count; i++)
			{
				//make sure *this* agent isn't included in the calculations and that
				//the agent being examined  is close enough 
				//***also make sure it doesn't include any evade target ***
				if ((Buddies[i].ID != Owner.ID) && 
					Buddies[i].Tag && 
					(Buddies[i].ID != Enemy1.ID) &&
					(Buddies[i].ID != Enemy2.ID))
				{
					AverageHeading += Buddies[i]->Heading;
					++NeighborCount;
				}
			}

			//if the neighborhood contained one or more vehicles, average their heading vectors.
			if (NeighborCount > 0)
			{
				AverageHeading /= NeighborCount;
				AverageHeading -= Owner.Heading;
			}

			return AverageHeading;
		}

		#endregion //Methods
	}
}