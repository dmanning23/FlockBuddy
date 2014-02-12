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
		/// this is a generic flag. 
		/// </summary>
		public bool Tagged { get; set; }

		/// <summary>
		/// its location in the environment
		/// </summary>
		public Circle Physics { get; protected set; }

		/// <summary>
		/// its location in the environment
		/// Used by the cell space IMovingEntity thing
		/// </summary>
		public Vector2 Position
		{
			get
			{
				return Physics.Pos;
			}
		}

		/// <summary>
		/// its location in the environment
		/// Used by the cell space IMovingEntity thing
		/// </summary>
		public Vector2 OldPosition
		{
			get
			{
				return Physics.OldPos;
			}
		}

		/// <summary>
		/// the length of this object's bounding radius
		/// </summary>
		public float BoundingRadius
		{
			get
			{
				return Physics.Radius;
			}
		}

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
		/// default constructor
		/// </summary>
		public BaseEntity()
		{
			ID = NextValidID();
			Physics = new Circle();
			Tagged = false;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="entity_type"></param>
		/// <param name="pos"></param>
		/// <param name="r"></param>
		public BaseEntity(Vector2 pos, float r)
		{
			ID = NextValidID();
			Physics = new Circle(pos, r);
			Tagged = false;
		}

		/// <summary>
		/// Called every frame to update this thing
		/// </summary>
		/// <param name="time_elapsed"></param>
		public virtual void Update(GameClock curTime)
		{
		}

		/// <summary>
		/// tags any entities contained in a container that are within the radius of the single entity parameter
		/// </summary>
		/// <param name="containerOfEntities"></param>
		/// <param name="radius"></param>
		public void TagNeighbors(List<Boid> dudes, float radius)
		{
			//iterate through all entities checking for range
			for (int i = 0; i < dudes.Count; i++)
			{
				TagObject(dudes[i], radius);
			}
		}

		/// <summary>
		/// check if some non-boid dudes are in range
		/// </summary>
		/// <param name="dudes"></param>
		/// <param name="radius"></param>
		public void TagObjects(List<BaseEntity> dudes, float radius)
		{
			//iterate through all entities checking for range
			for (int i = 0; i < dudes.Count; i++)
			{
				TagObject(dudes[i], radius);
			}
		}

		/// <summary>
		/// check if a dude is in range
		/// </summary>
		/// <param name="dude"></param>
		/// <param name="radius"></param>
		private void TagObject(BaseEntity dude, float radius)
		{
			//first clear any current tag
			dude.Tagged = false;

			Vector2 to = dude.Position - Position;

			//the bounding radius of the other is taken into account by adding it to the range
			double range = BoundingRadius + dude.BoundingRadius;

			//if entity within range, tag for further consideration. 
			//(working in distance-squared space to avoid sqrts)
			if ((to.LengthSquared() < (range * range)) && (dude.ID != ID))
			{
				dude.Tagged = true;
			}
		}

		/// <summary>
		/// Draw the physics info for this entity
		/// </summary>
		/// <param name="curTime"></param>
		public void DrawPhysics(IBasicPrimitive prim, Color color)
		{
			prim.Circle(Physics.Pos, Physics.Radius, color);
		}

		#endregion //Methods
	}
}