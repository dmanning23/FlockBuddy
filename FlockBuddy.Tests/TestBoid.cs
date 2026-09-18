using FlockBuddy.Interfaces;
using GameTimer;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace FlockBuddy.Tests
{
	public class TestBoid : Boid
	{
		public new float Speed
		{
			get
			{
				return base.Speed;
			}
			set
			{
				base.Speed = value;
			}
		}

		public GameClock Timer
		{
			get
			{
				return BoidTimer;
			}
		}

		public TestBoid(IFlock flock,
			Vector2 position,
			float radius,
			Vector2 heading,
			float speed,
			float mass = -1f,
			float minSpeed = -1f,
			float walkSpeed = -1f,
			float maxSpeed = -1f,
			float maxTurnRate = -1f,
			float maxForce = -1f) :
				base(flock,
					position,
					heading,
					speed,
					radius)
		{
			BoidTimer.TimeDelta = 1f;
			Mass = mass;
			MinSpeed = minSpeed;
			WalkSpeed = walkSpeed;
			MaxSpeed = maxSpeed;
			MaxTurnRate = maxTurnRate;
			MaxForce = maxForce;
			Laziness = 0f;
		}

		public new void RotateHeading(float angle)
		{
			base.RotateHeading(angle);
		}

		public new void UpdateSpeed(Vector2 targetHeading)
		{
			base.UpdateSpeed(targetHeading);
		}

		public new bool GetAmountToTurn(Vector2 targetHeading, ref float angle)
		{
			return base.GetAmountToTurn(targetHeading, ref angle);
		}

		public new float GetSpeedChange(Vector2 targetHeading)
		{
			return base.GetSpeedChange(targetHeading);
		}

		public List<IBehavior> GetBehaviors()
		{
			return Behaviors;
		}
	}
}
