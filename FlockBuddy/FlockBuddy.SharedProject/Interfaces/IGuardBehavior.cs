using System;
using System.Collections.Generic;
using System.Text;

namespace FlockBuddy
{
	/// <summary>
	/// this is a behavior that tries to protect another entity
	/// </summary>
    public interface IGuardBehavior
    {
		IBaseEntity Vip { get; }
    }
}
