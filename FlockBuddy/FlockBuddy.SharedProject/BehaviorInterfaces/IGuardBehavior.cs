﻿
namespace FlockBuddy
{
	/// <summary>
	/// this is a behavior that tries to protect another entity
	/// </summary>
	public interface IGuardBehavior
	{
		IMover Vip { set; }
	}
}
