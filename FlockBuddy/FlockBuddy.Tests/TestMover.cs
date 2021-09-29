using GameTimer;
using Microsoft.Xna.Framework;

namespace FlockBuddy.Tests
{
	public class TestMover : Mover
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

		public TestMover() : base(Vector2.Zero, 1f, Vector2.UnitX, 0f)
		{
		}

		public TestMover(Vector2 position, float radius, Vector2 heading, float speed, float mass, float minSpeed, float walkSpeed, float maxSpeed, float maxTurnRate, float maxForce) : base(position, radius, heading, speed)
		{
			BoidTimer.TimeDelta = 1f;
		}

		public new void RotateHeading(float angle)
		{
			base.RotateHeading(angle);
		}
	}
}
