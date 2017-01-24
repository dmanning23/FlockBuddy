using CellSpacePartitionLib;

namespace FlockBuddy
{
	/// <summary>
	/// Base class to define a common interface for all game entities
	/// </summary>
	public interface IBaseEntity : IMovingEntity
	{
		/// <summary>
		/// each entity has a unique ID
		/// </summary>
		int ID { get; }

		/// <summary>
		/// the length of this object's bounding radius
		/// </summary>
		float BoundingRadius { get; }
	}
}