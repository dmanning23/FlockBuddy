using Microsoft.Xna.Framework;

namespace FlockBuddy
{
	/// <summary>
	/// class to create and render 2D walls. 
	/// Defined as the two vectors A - B with a perpendicular normal. 
	/// </summary>
	public class Wall2
	{
		#region Members

		protected Vector2 _start;
		protected Vector2 _end;
		protected Vector2 _normal;

		#endregion //Members

		#region Properties

		Vector2 Start
		{
			get
			{
				return _start;
			}
			set { _start = value; CalculateNormal(); }
		}

		Vector2 End
		{
			get
			{
				return _end;
			}
			set { _end = value; CalculateNormal(); }
		}
		Vector2 Normal
		{
			get
			{
				return _normal;
			}
			set { _normal = value; }
		}

		#endregion //Properties

		#region Methods

		public Wall2() 
		{
		}

		public Wall2(Vector2 start, Vector2 end)
		{
			_start = start;
			_end = end;
			CalculateNormal();
		}

		public Wall2(Vector2 start, Vector2 end, Vector2 normal)
		{
			_start = start;
			_end = end;
			_normal = normal;
		}

		void CalculateNormal()
		{
			Vector2 temp = new Vector2(_end - _start);
			temp.Normalize();

			_normal.x = -temp.y;
			_normal.y = temp.x;
		}

		public Vector2 Center() 
		{
			return (m_vA + m_vB) / 2.0f; 
		}

		//virtual void Render(bool RenderNormals = false)const
		//{
		//  gdi->Line(m_vA, m_vB);

		//  //render the normals if rqd
		//  if (RenderNormals)
		//  {
		//	int MidX = (int)((m_vA.x+m_vB.x)/2);
		//	int MidY = (int)((m_vA.y+m_vB.y)/2);

		//	gdi->Line(MidX, MidY, (int)(MidX+(m_vN.x * 5)), (int)(MidY+(m_vN.y * 5)));
		//  }
		//}

		#endregion //Methods
	}
}