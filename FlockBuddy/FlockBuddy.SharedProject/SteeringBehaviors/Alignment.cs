using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System;
using System.Diagnostics;

namespace FlockBuddy
{
	/// <summary>
	/// Group behavior to move boids in the same direction as each other
	/// </summary>
	public class Alignment : BaseBehavior, IFlockingBehavior
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
				return 0f;
			}
		}

		#endregion //Properties

		#region Methods

		/// <summary>
		/// Initializes a new instance of the <see cref="FlockBuddy.Alignment"/> class.
		/// </summary>
		public Alignment(IBoid dude)
			: base(dude, EBehaviorType.alignment, BoidDefaults.AlignmentWeight)
		{
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

			//if the neighborhood contained one or more vehicles, average their heading vectors.
			if (null != Buddies && Buddies.Count > 0)
			{
				//iterate through all the tagged vehicles and sum their heading vectors  
				for (int i = 0; i < Buddies.Count; i++)
				{
					//make sure *this* agent isn't included in the calculations 
					//and that the agent being examined  is close enough 
					AverageHeading += Buddies[i].Heading;
				}

				AverageHeading /= Buddies.Count;
				AverageHeading -= Owner.Heading;
			}

			//always multiply the return value by the weight
			return AverageHeading * Weight;
		}

		#endregion //Methods
	}
}