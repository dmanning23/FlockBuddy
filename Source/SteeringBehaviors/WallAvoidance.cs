using Microsoft.Xna.Framework;
using System.Collections.Generic;
using CollisionBuddy;
using MatrixExtensions;
using Vector2Extensions;

namespace FlockBuddy
{
	/// <summary>
	/// this returns a steering force which will keep the agent away from any walls it may encounter
	/// </summary>
	public class WallAvoidance : BaseBehavior
	{
		#region Members

		/// <summary>
		/// The walls to avoid
		/// </summary>
		List<Line> Walls { get; set; }

		/// <summary>
		/// little whiskers that this dude will use to detect walls
		/// </summary>
		List<Vector2> Feelers { get; set; }

		const float WhiskerLength = 20.0f;

		#endregion //Members

		#region Methods

		/// <summary>
		/// Initializes a new instance of the <see cref="FlockBuddy.Evade"/> class.
		/// </summary>
		public WallAvoidance(Boid dude)
			: base(dude, EBehaviorType.wall_avoidance)
		{
			Feelers = new List<Vector2>();
		}

		/// <summary>
		/// This returns a steering force that will keep the agent away from any walls it may encounter
		/// </summary>
		/// <param name="time"></param>
		/// <returns></returns>
		public Vector2 GetSteering(List<Line> walls)
		{
			Walls = walls;
			return GetSteering();
		}

		/// <summary>
		/// Called every fram to get the steering direction from this behavior
		/// </summary>
		/// <param name="time"></param>
		/// <returns></returns>
		protected override Vector2 GetSteering()
		{
			//the feelers are contained in a std::vector, m_Feelers
			CreateFeelers();

			float DistToThisIP = 0.0f;
			float DistToClosestIP = float.MaxValue;

			//this will hold an index into the vector of walls
			int ClosestWall = -1;

			Vector2 SteeringForce = Vector2.Zero;
			Vector2 point = Vector2.Zero; //used for storing temporary info
			Vector2 ClosestPoint = Vector2.Zero;//holds the closest intersection point

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
										   ref DistToThisIP,
										   ref point))
					{
						//is this the closest found so far? If so keep a record
						if (DistToThisIP < DistToClosestIP)
						{
							DistToClosestIP = DistToThisIP;
							ClosestWall = j;
							ClosestPoint = point;
						}
					}
				}

				//if an intersection point has been detected, calculate a force that will direct the agent away
				if (ClosestWall >= 0)
				{
					//calculate by what distance the projected position of the agent will overshoot the wall
					Vector2 OverShoot = Feelers[i] - ClosestPoint;

					//create a force in the direction of the wall normal, with a magnitude of the overshoot
					SteeringForce = Walls[ClosestWall].Normal * OverShoot.Length();
				}
			}

			return SteeringForce * Weight;
		}

		/// <summary>
		/// Creates the antenna utilized by WallAvoidance
		/// </summary>
		void CreateFeelers()
		{
			//clear out the feelsers
			Feelers.Clear();

			//feeler pointing straight in front
			Feelers.Add(Owner.Position + (WhiskerLength * Owner.Heading));

			//feeler to left
			Vector2 temp = Vec2DRotateAroundOrigin(Owner.Heading, MathHelper.PiOver2 * 3.5f);
			Feelers.Add(Owner.Position + ((WhiskerLength * 0.5f) * temp));

			//feeler to right
			temp = Vec2DRotateAroundOrigin(Owner.Heading, MathHelper.PiOver2 * 0.5f);
			Feelers.Add(Owner.Position + ((WhiskerLength * 0.5f) * temp));
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