
namespace FlockBuddy
{
	/// <summary>
	/// This is the interface for a steering beahvior that trie to avoid predators
	/// </summary>
	public interface IPreyBehavior
	{
		void SetBadGuys(Boid badGuy1, Boid badGuy2);
	}
}