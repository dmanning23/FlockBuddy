using CellSpacePartitionLib;

namespace FlockBuddy.Interfaces
{
	/// <summary>
	/// Base class to define a common interface for all game entities
	/// </summary>
	public interface IBaseEntity : IMovingEntity
	{
		/// <summary>
		/// the length of this object's bounding radius
		/// </summary>
		float Radius { get; set; }
	}
}