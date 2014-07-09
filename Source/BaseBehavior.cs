using Microsoft.Xna.Framework;

namespace FlockBuddy
{
	/// <summary>
	/// This is the base class for all behaviors
	/// </summary>
	public abstract class BaseBehavior
	{
		#region Members

		/// <summary>
		/// The type of this beahvior
		/// </summary>
		public EBehaviorType BehaviorType { get; private set; }

		/// <summary>
		/// a pointer to the owner of this instance
		/// </summary>
		public Boid Owner { get; set; }

		/// <summary>
		/// is cell space partitioning to be used or not?
		/// </summary>
		public bool CellSpaceOn { get; set; }

		public BoidTemplate BoidTemplate { get; private set; }

		/// <summary>
		/// The behavior template for this dude
		/// </summary>
		public BehaviorTemplate BehaviorTemplate { get; private set; }

		public float Weight
		{
			get
			{
				return BehaviorTemplate.Weight;
			}
			set
			{
				BehaviorTemplate.Weight = value;
			}
		}

		public bool Active
		{
			get
			{
				return BehaviorTemplate.Enabled;
			}
			set
			{
				BehaviorTemplate.Enabled = value;
			}
		}

		#endregion //Members

		#region Methods

		/// <summary>
		/// Initializes a new instance of the <see cref="FlockBuddy.BaseBehavior"/> class.
		/// </summary>
		public BaseBehavior(Boid dude, EBehaviorType behaviorType, BoidTemplate template)
		{
			BehaviorType = behaviorType;
			Owner = dude;
			BoidTemplate = template;
			BehaviorTemplate = BoidTemplate.Behaviors[(int)BehaviorType];
		}

		/// <summary>
		/// Called every fram to get the steering direction from this behavior
		/// Dont call this for inactive behaviors
		/// </summary>
		/// <returns></returns>
		protected abstract Vector2 GetSteering();

		#endregion //Methods
	}
}