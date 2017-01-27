using GameTimer;
using Microsoft.Xna.Framework;
using PrimitiveBuddy;
using System.Collections.Generic;

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

		private RoundRobinID _id;

		#endregion //Members

		#region Properties

		/// <summary>
		/// each entity has a unique ID
		/// </summary>
		public int ID
		{
			get
			{
				return _id.ID;
			}
		}

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
		public virtual float Radius { get; protected set; }

		#endregion //Properties

		#region Methods

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="pos">teh position of this dude</param>
		/// <param name="r">the radius of this dude</param>
		public BaseEntity(Vector2 position, float radius)
		{
			_id = new RoundRobinID();

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
		/// tags any entities contained in a container that are within the radius of the single entity parameter
		/// </summary>
		/// <param name="dudes">teh list of entities to tag as neighbors</param>
		/// <param name="radius">the disdtance to tag neightbors</param>
		public List<IMover> TagNeighbors(List<IMover> dudes, float distance)
		{
			var neighbors = new List<IMover>();

			//iterate through all entities checking for range
			for (int i = 0; i < dudes.Count; i++)
			{
				if (TagObject(dudes[i], distance))
				{
					neighbors.Add(dudes[i]);
				}
			}

			return neighbors;
		}

		/// <summary>
		/// check if some non-boid dudes are in range
		/// </summary>
		/// <param name="dudes"></param>
		/// <param name="distance"></param>
		public List<IBaseEntity> TagObjects(List<IBaseEntity> dudes, float distance)
		{
			var neighbors = new List<IBaseEntity>();

			//iterate through all entities checking for range
			for (int i = 0; i < dudes.Count; i++)
			{
				if (TagObject(dudes[i], distance))
				{
					neighbors.Add(dudes[i]);
				}
			}

			return neighbors;
		}

		/// <summary>
		/// check if a dude is in range
		/// </summary>
		/// <param name="dude"></param>
		/// <param name="distance"></param>
		private bool TagObject(IBaseEntity dude, float distance)
		{
			if (null == dude)
			{
				return false;
			}

			Vector2 to = dude.Position - Position;

			//the bounding radius of the other is taken into account by adding it to the range
			double range = distance + dude.Radius;

			//if entity within range, tag for further consideration. 
			//(working in distance-squared space to avoid sqrts)
			return ((to.LengthSquared() < (range * range)) && (dude.ID != ID));
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