using CollisionBuddy;
using MatrixExtensions;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Diagnostics;
using Vector2Extensions;

namespace FlockBuddy
{
	/// <summary>
	/// this returns a steering force which will keep the agent away from any walls it may encounter
	/// </summary>
	public class WallAvoidance : BaseBehavior, IWallBehavior
	{
		#region Properties

		/// <summary>
		/// The walls to avoid
		/// </summary>
		public List<ILine> Walls { private get; set; }

		/// <summary>
		/// little whiskers that this dude will use to detect walls
		/// </summary>
		public List<Vector2> Feelers { get; set; }

		public override float DirectionChange
		{
			get
			{
				return 1f;
			}
		}

		public override float SpeedChange
		{
			get
			{
				return 1f;
			}
		}

		#endregion //Properties

		#region Methods

		/// <summary>
		/// Initializes a new instance of the <see cref="FlockBuddy.Evade"/> class.
		/// </summary>
		public WallAvoidance(IBoid dude)
			: base(dude, EBehaviorType.wall_avoidance, BoidDefaults.WallAvoidanceWeight)
		{
			Feelers = new List<Vector2>();
		}

		/// <summary>
		/// Called every fram to get the steering direction from this behavior
		/// </summary>
		/// <returns></returns>
		public override Vector2 GetSteering()
		{
			if (null == Walls || 0 >= Walls.Count)
			{
				return Vector2.Zero;
			}

			//the feelers are contained in a std::vector, m_Feelers
			CreateFeelers();

			float distToThisIP = 0.0f;

			Vector2 steeringForce = Vector2.Zero;
			Vector2 point = Vector2.Zero; //used for storing temporary info

			//examine each feeler in turn
			for (int i = 0; i < Feelers.Count; i++)
			{
				//run through each wall checking for any intersection points
				for (int j = 0; j < Walls.Count; j++)
				{
					if (Vector2Ext.LineIntersection2D(Owner.Position,
										   Feelers[i],
										   Walls[j].Start,
										   Walls[j].End,
										   ref distToThisIP,
										   ref point))
					{
						//calculate by what distance the projected position of the agent will overshoot the wall
						var overShoot = Feelers[i] - point;

						//create a force in the direction of the wall normal, with a magnitude of the overshoot
						steeringForce += Walls[j].Normal * overShoot.Length();
					}
				}
			}

			return steeringForce * Weight;
		}

		/// <summary>
		/// Creates the antenna utilized by WallAvoidance
		/// </summary>
		void CreateFeelers()
		{
			//clear out the feelsers
			Feelers.Clear();

			//feeler pointing straight in front
			Feelers.Add(Owner.Position + (Owner.WallQueryRadius * Owner.Heading));

			//feeler to left
			Vector2 temp = Vec2DRotateAroundOrigin(Owner.Heading, MathHelper.PiOver2 * 3.5f);
			Feelers.Add(Owner.Position + ((Owner.WallQueryRadius * 0.5f) * temp));

			//feeler to right
			temp = Vec2DRotateAroundOrigin(Owner.Heading, MathHelper.PiOver2 * 0.5f);
			Feelers.Add(Owner.Position + ((Owner.WallQueryRadius * 0.5f) * temp));
		}

		/// <summary>
		/// rotates a vector ang rads around the origin
		/// </summary>
		/// <param name="v"></param>
		/// <param name="ang"></param>
		/// <returns></returns>
		Vector2 Vec2DRotateAroundOrigin(Vector2 v, float ang)
		{
			//create a rotation matrix
			Matrix mat = MatrixExt.Orientation(ang);

			//now transform the object's vertices
			return mat.Multiply(v);
		}

		#endregion //Methods
	}
}