
namespace FlockBuddy
{
	/// <summary>
	/// This is the interface for a steering beahvior that tries to catch another boid
	/// </summary>
	public interface IPredatorBehavior
	{
		void SetTarget(Boid target1);
	}
}