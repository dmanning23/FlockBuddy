using FilenameBuddy;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Xml;
using Vector2Extensions;

namespace FlockBuddy
{
	/// <summary>
	/// Contains all the data to describe the behavior of a boid
	/// </summary>
	public class BoidTemplate
	{
		#region Members

		/// <summary>
		/// the radius of the boid
		/// </summary>
		public float Radius{ get; private set; }

		/// <summary>
		/// the start direction of the boid
		/// </summary>
		public Vector2 StartDirection { get; private set; }

		/// <summary>
		/// the start speed of the boid
		/// </summary>
		public float StartSpeed { get; private set; }

		/// <summary>
		/// mass o the boid
		/// </summary>
		public float Mass { get; private set; }

		/// <summary>
		/// The max speed of the boid
		/// </summary>
		public float MaxSpeed { get; private set; }

		/// <summary>
		/// max turn rate of this boid
		/// </summary>
		public float MaxTurnRate { get; private set; }

		/// <summary>
		/// the max for ce of teh boid
		/// </summary>
		public float MaxForce { get; private set; }

		/// <summary>
		/// how far out to check for neighbors
		/// </summary>
		public float QueryRadius { get; private set; }

		/// <summary>
		/// A dictionary of behavior types to weights...
		/// Any behaviors in this dictionary are "active"
		/// </summary>
		public Dictionary<EBehaviorType, float> Behaviors { get; private set; }

		/// <summary>
		/// how far the evade behvaior should look out for threats
		/// </summary>
		public float EvadeThreatRange { get; private set; }

		/// <summary>
		/// how far out the flee behvaior should watch before panicking
		/// </summary>
		public float FleePanicDistance { get; private set; }

		/// <summary>
		/// how four the obstacle avoidance behvaior should watch for obastcles
		/// </summary>
		public float ObstacleAvoidanceDetectionDistance { get; private set; }

		/// <summary>
		/// the length of the wall avoidance whiskers
		/// </summary>
		public float WallAvoidanceWhiskerLength { get; private set; }

		#endregion Members

		#region Methods

		/// <summary>
		/// Initializes a new instance of the <see cref="FlockBuddy.Boid"/> class.
		/// </summary>
		public BoidTemplate()
		{
			Behaviors = new Dictionary<EBehaviorType, float>();
		}

		#endregion //Methods

		#region File IO

		/// <summary>
		/// open a file and read the boid template
		/// </summary>
		/// <param name="strFilename"></param>
		/// <param name="rRenderer"></param>
		/// <returns></returns>
		public bool ReadXmlFile(Filename strFilename)
		{
			//Open the file.
			FileStream stream = File.Open(strFilename.File, FileMode.Open, FileAccess.Read);
			XmlDocument xmlDoc = new XmlDocument();
			xmlDoc.Load(stream);
			XmlNode rootNode = xmlDoc.DocumentElement;

			//make sure it is actually an xml node
			if (rootNode.NodeType == XmlNodeType.Element)
			{
				//eat up the name of that xml node
				string strElementName = rootNode.Name;
				if (("XnaContent" != strElementName) || !rootNode.HasChildNodes)
				{
					return false;
				}

				//make sure to read from the the next node
				if (!ReadXmlObject(rootNode.FirstChild))
				{
					return false;
				}
			}
			else
			{
				//should be an xml node!!!
				return false;
			}

			// Close the file.
			stream.Close();
			return true;
		}

		/// <summary>
		/// given an xml node, read in the boid template
		/// </summary>
		/// <param name="rXMLNode"></param>
		/// <returns></returns>
		public bool ReadXmlObject(XmlNode rXMLNode)
		{
			//should have an attribute Type
			XmlNamedNodeMap mapAttributes = rXMLNode.Attributes;
			for (int i = 0; i < mapAttributes.Count; i++)
			{
				//will only have the name attribute
				string strName = mapAttributes.Item(i).Name;
				string strValue = mapAttributes.Item(i).Value;
				if ("Type" == strName)
				{
					if (strValue != "FlockBuddy.BoidTemplateXML")
					{
						Debug.Assert(false);
						return false;
					}
				}
				else
				{
					Debug.Assert(false);
					return false;
				}
			}

			//Read in child nodes
			if (rXMLNode.HasChildNodes)
			{
				for (XmlNode childNode = rXMLNode.FirstChild;
					null != childNode;
					childNode = childNode.NextSibling)
				{
					//what is in this node?
					string strName = childNode.Name;
					string strValue = childNode.InnerText;

					if (strName == "Radius")
					{
						Radius = Convert.ToSingle(strValue);
					}
					else if (strName == "StartDirection")
					{
						StartDirection = strValue.ToVector2();
					}
					else if (strName == "StartSpeed")
					{
						StartSpeed = Convert.ToSingle(strValue);
					}
					else if (strName == "Mass")
					{
						Mass = Convert.ToSingle(strValue);
					}
					else if (strName == "MaxSpeed")
					{
						MaxSpeed = Convert.ToSingle(strValue);
					}
					else if (strName == "MaxTurnRate")
					{
						MaxTurnRate = Convert.ToSingle(strValue);
					}
					else if (strName == "MaxForce")
					{
						MaxForce = Convert.ToSingle(strValue);
					}
					else if (strName == "QueryRadius")
					{
						QueryRadius = Convert.ToSingle(strValue);
					}
					else if (strName == "Behaviors")
					{
						ReadBehaviorXml(childNode);
					}
					else if (strName == "EvadeThreatRange")
					{
						EvadeThreatRange = Convert.ToSingle(strValue);
					}
					else if (strName == "FleePanicDistance")
					{
						FleePanicDistance = Convert.ToSingle(strValue);
					}
					else if (strName == "ObstacleAvoidanceDetectionDistance")
					{
						ObstacleAvoidanceDetectionDistance = Convert.ToSingle(strValue);
					}
					else if (strName == "WallAvoidanceWhiskerLength")
					{
						WallAvoidanceWhiskerLength = Convert.ToSingle(strValue);
					}
					else
					{
						Debug.Assert(false);
						return false;
					}
				}
			}

			return true;
		}

		/// <summary>
		/// Read in a behavior node
		/// </summary>
		/// <param name="rXMLNode"></param>
		/// <returns></returns>
		private bool ReadBehaviorXml(XmlNode rXMLNode)
		{
			//should have an attribute Type
			XmlNamedNodeMap mapAttributes = rXMLNode.Attributes;
			for (int i = 0; i < mapAttributes.Count; i++)
			{
				//will only have the name attribute
				string strAttName = mapAttributes.Item(i).Name;
				string strAttValue = mapAttributes.Item(i).Value;
				if ("Type" == strAttName)
				{
					if (strAttValue != "FlockBuddy.BehaviorXML")
					{
						Debug.Assert(false);
						return false;
					}
				}
				else
				{
					Debug.Assert(false);
					return false;
				}
			}

			//Read in child nodes
			Debug.Assert(rXMLNode.HasChildNodes);
			XmlNode childNode = rXMLNode.FirstChild;

			//get the behavior node
			string strName = childNode.Name;
			string strValue = childNode.InnerText;
			Debug.Assert(strName == "BehaviorType");
			EBehaviorType behavior = (EBehaviorType)Enum.Parse(typeof(EBehaviorType), strValue);

			//get the weight node
			childNode = childNode.NextSibling;
			Debug.Assert(null != childNode);
			strName = childNode.Name;
			strValue = childNode.InnerText;
			Debug.Assert(strName == "BehaviorType");
			float weight =  Convert.ToSingle(strValue);

			//Add the behavior to the list we are going to use
			Behaviors.Add(behavior, weight);

			return true;
		}

		#endregion //File IO
	}
}