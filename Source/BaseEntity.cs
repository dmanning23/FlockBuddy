using Microsoft.Xna.Framework;
using GameTimer;

namespace FlockBuddy
{
	/// <summary>
	/// Base class to define a common interface for all game entities
	/// </summary>
	public class BaseEntity
	{
		#region Members

		/// <summary>
		/// its location in the environment
		/// </summary>
		protected Vector2 _position;

		/// <summary>
		/// the size of the thing
		/// </summary>
		protected Vector2 _scale;

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
		/// every entity has a type associated with it (health, troll, ammo etc)
		/// </summary>
		public int EntityType { get; set; }

		/// <summary>
		/// this is a generic flag. 
		/// </summary>
		public bool Tagged { get; set; }

		/// <summary>
		/// its location in the environment
		/// </summary>
		public Vector2 Position
		{
			get
			{
				return _position;
			}
			set
			{
				_position = value;
			}
		}

		/// <summary>
		/// the size of the thing
		/// </summary>
		public Vector2 Scale
		{
			get
			{
				return _scale;
			}
		}

		/// <summary>
		/// the length of this object's bounding radius
		/// </summary>
		public float BoundingRadius { get; set; }

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
			BoundingRadius = 0.0f;
			_position = Vector2.Zero;
			_scale = new Vector2(1.0f, 1.0f);
			EntityType = -1;
			Tagged = false;
		}

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="entity_type">the entity type to use</param>
		public BaseEntity(int entity_type)
		{
			ID = NextValidID();
			BoundingRadius = 0.0f;
			_position = Vector2.Zero;
			_scale = new Vector2(1.0f, 1.0f);
			EntityType = entity_type;
			Tagged = false;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="entity_type"></param>
		/// <param name="pos"></param>
		/// <param name="r"></param>
		public BaseEntity(int entity_type, Vector2 pos, float r)
		{
			ID = NextValidID();
			BoundingRadius = r;
			_position = pos;
			_scale = new Vector2(1.0f, 1.0f);
			EntityType = entity_type;
			Tagged = false;
		}

		//this can be used to create an entity with a 'forced' ID. It can be used
		//when a previously created entity has been removed and deleted from the
		//game for some reason. For example, The Raven map editor uses this ctor 
		//in its undo/redo operations. 
		//USE WITH CAUTION!
		public BaseEntity(int entity_type, int ForcedID)
		{
			ID = ForcedID;
			BoundingRadius = 0.0f;
			_position = Vector2.Zero;
			_scale = new Vector2(1.0f, 1.0f);
			EntityType = entity_type;
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
		/// called every frame to draw this thing
		/// </summary>
		/// <param name="curTime"></param>
		public virtual void Render(GameClock curTime)
		{
		}

		/// <summary>
		/// Set the scale via a vector
		/// </summary>
		/// <param name="val"></param>
		void SetScale(Vector2 val)
		{
			BoundingRadius *= MathHelper.Max(val.X, val.Y) / MathHelper.Max(Scale.X, Scale.Y);
			_scale = val;
		}

		/// <summary>
		/// Set teh scale via a scalar value
		/// </summary>
		/// <param name="val"></param>
		void SetScale(float val)
		{
			BoundingRadius = (val / MathHelper.Max(Scale.X, Scale.Y));
			_scale = new Vector2(val, val);
		}

		#endregion //Methods
	}
}