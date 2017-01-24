
namespace FlockBuddy
{
	/// <summary>
	/// stupid class to create unique IDs using round robin
	/// </summary>
	public class RoundRobinID
	{
		#region Members

		/// <summary>
		/// Counter used to round-robin entity ids
		/// </summary>
		private static int NextID;

		#endregion //Members

		#region Properties

		/// <summary>
		/// each entity has a unique ID
		/// </summary>
		public int ID { get; private set; }

		#endregion //Properties

		#region Methods

		/// <summary>
		/// used by the constructor to give each entity a unique ID
		/// </summary>
		/// <returns>an id to use for an entity</returns>
		private static int NextValidID() { return NextID++; }

		/// <summary>
		/// Static constructor
		/// </summary>
		static RoundRobinID()
		{
			NextID = 0;
		}

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="pos">teh position of this dude</param>
		/// <param name="r">the radius of this dude</param>
		public RoundRobinID()
		{
			ID = NextValidID();
		}

		#endregion //Methods
	}
}