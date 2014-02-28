using Microsoft.Xna.Framework;
using GameTimer;
using CollisionBuddy;
using System.Collections.Generic;
using CellSpacePartitionLib;
using BasicPrimitiveBuddy;

namespace FlockBuddy
{
	/// <summary>
	/// Base class to define a common interface for all game entities
	/// </summary>
	public class BaseEntity : IMovingEntity
	{
		#region Members

		/// <summary>
		/// The location of this boid
		/// </summary>
		protected Vector2 _Position = Vector2.Zero;

		#endregion //Members

		#region Properties

		/// <summary>
		/// Counter used to round-robin entity ids
		/// </summary>
		private static int NextID;

		/// <summary>
		/// each entity has a unique ID
		/// </summary>
		public int ID { get; set; }

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
		public float BoundingRadius { get; private set; }

		#endregion //Properties

		#region Methods

		/// <summary>
		/// used by the constructor to give each entity a unique ID
		/// </summary>
		/// <returns>an id to use for an entity</returns>
		static int NextValidID() { return NextID++; }

		/// <summary>
		/// Static constructor
		/// </summary>
		static BaseEntity()
		{
			NextID = 0;
		}

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="pos">teh position of this dude</param>
		/// <param name="r">the radius of this dude</param>
		public BaseEntity(Vector2 pos, float r)
		{
			ID = NextValidID();

			_Position = pos;
			OldPosition = pos;
			BoundingRadius = r;
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
		public List<Mover> TagNeighbors(List<Mover> dudes, float radius)
		{
			var neighbors = new List<Mover>();

			//iterate through all entities checking for range
			for (int i = 0; i < dudes.Count; i++)
			{
				if (TagObject(dudes[i], radius))
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
		/// <param name="radius"></param>
		public List<BaseEntity> TagObjects(List<BaseEntity> dudes, float radius)
		{
			var neighbors = new List<BaseEntity>();

			//iterate through all entities checking for range
			for (int i = 0; i < dudes.Count; i++)
			{
				if (TagObject(dudes[i], radius))
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
		/// <param name="radius"></param>
		private bool TagObject(BaseEntity dude, float radius)
		{
			Vector2 to = dude.Position - Position;

			//the bounding radius of the other is taken into account by adding it to the range
			double range = radius + dude.BoundingRadius;

			//if entity within range, tag for further consideration. 
			//(working in distance-squared space to avoid sqrts)
			return ((to.LengthSquared() < (range * range)) && (dude.ID != ID));
		}

		/// <summary>
		/// Draw the physics info for this entity
		/// </summary>
		/// <param name="prim">basic ptimitive to draw this dude</param>
		/// <param name="color">teh color to draw him</param>
		public void DrawPhysics(IBasicPrimitive prim, Color color)
		{
			prim.Circle(Position, BoundingRadius, color);
		}

		#endregion //Methods
	}
}