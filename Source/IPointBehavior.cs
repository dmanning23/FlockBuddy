using Microsoft.Xna.Framework;

namespace FlockBuddy
{
	/// <summary>
	/// This is the interface for a steering beahvior that takes a target
	/// </summary>
	public interface IPointBehavior
	{
		void SetPoint(Vector2 target);
	}
}