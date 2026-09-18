using FlockBuddy.Interfaces;
using GameTimer;
using Microsoft.Xna.Framework;
using PrimitiveBuddy;

namespace FlockBuddy
{
	/// <summary>
	/// Base class to define a common interface for all game entities
	/// </summary>
	public class BaseEntity : IBaseEntity
	{
		#region Members

		/// <summary>
		/// The location of this boid
		/// </summary>
		protected Vector2 _Position = Vector2.Zero;
		
		#endregion //Members

		#region Properties

		/// <summary>
		/// its location in the environment
		/// Used by the cell space IMovingEntity thing
		/// </summary>
		public Vector2 Position
		{
			get
			{
				return _Position;
			}
			set
			{
				OldPosition = _Position;
				_Position = value;
			}
		}

		/// <summary>
		/// its location in the environment
		/// Used by the cell space IMovingEntity thing
		/// </summary>
		public Vector2 OldPosition { get; private set; }

		/// <summary>
		/// the length of this object's bounding radius
		/// </summary>
		public virtual float Radius { get; set; }

		#endregion //Properties

		#region Methods

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="pos">teh position of this dude</param>
		/// <param name="r">the radius of this dude</param>
		public BaseEntity(Vector2 position, float radius)
		{
			_Position = position;
			OldPosition = position;
			Radius = radius;
		}

		/// <summary>
		/// Called every frame to update this thing
		/// </summary>
		/// <param name="curTime"></param>
		public virtual void Update(GameClock curTime)
		{
		}

		/// <summary>
		/// Draw the physics info for this entity
		/// </summary>
		/// <param name="prim">basic ptimitive to draw this dude</param>
		/// <param name="color">teh color to draw him</param>
		public virtual void DrawPhysics(IPrimitive prim, Color color)
		{
			prim.Circle(Position, Radius, color);
		}

		#endregion //Methods
	}
}