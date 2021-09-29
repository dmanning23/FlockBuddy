
namespace FlockBuddy.Interfaces.Behaviors
{
	/// <summary>
	/// This is a behavior where the bad guys are trying to catch this boid
	/// </summary>
	public interface IPreyBehavior
	{
		IMover Pursuer { set; }
	}
}
