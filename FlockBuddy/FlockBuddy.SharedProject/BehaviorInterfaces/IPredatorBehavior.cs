
namespace FlockBuddy
{
	/// <summary>
	/// A behavior where this boid tries to get another one
	/// </summary>
	public interface IPredatorBehavior
	{
		IMover Prey { set; }
	}
}
