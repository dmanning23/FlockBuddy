using FlockBuddy.Interfaces;
using Microsoft.Xna.Framework;
using System;

namespace FlockBuddy
{
	public class BehaviorTemplate : IBehavior
	{
		public BehaviorType BehaviorType { get; set; }

		public float Weight { get; set; }

		public IBoid Owner
		{
			get
			{
				throw new NotImplementedException();
			}

			set
			{
				throw new NotImplementedException();
			}
		}

		public Vector2 GetSteering()
		{
			throw new NotImplementedException();
		}

		public float DirectionChange
		{
			get
			{
				throw new NotImplementedException();
			}
		}

		public float SpeedChange
		{
			get
			{
				throw new NotImplementedException();
			}
		}
	}
}
