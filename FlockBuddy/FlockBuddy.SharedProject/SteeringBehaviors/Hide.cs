using FlockBuddy.Interfaces;
using FlockBuddy.Interfaces.Behaviors;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace FlockBuddy.SteeringBehaviors
{
	/// <summary>
	/// given another agent position to hide from and a list of BaseGameEntitys this
	/// method attempts to put an obstacle between itself and its opponent
	/// </summary>
	public class Hide : BaseBehavior, IPreyBehavior, IObstacleBehavior
	{
		#region Properties

		public List<IBaseEntity> Obstacles { private get; set; }

		public IMover Pursuer { private get; set; }

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
		/// Initializes a new instance of the <see cref="FlockBuddy.Hide"/> class.
		/// </summary>
		public Hide(IBoid dude)
			: base(dude, BehaviorType.Hide, BoidDefaults.HideWeight)
		{
		}

		/// <summary>
		/// Called every fram to get the steering direction from this behavior
		/// </summary>
		/// <returns></returns>
		public override Vector2 GetSteering()
		{
			//TODO:
			return Vector2.Zero * Weight;
		}

		#endregion //Methods
	}
}