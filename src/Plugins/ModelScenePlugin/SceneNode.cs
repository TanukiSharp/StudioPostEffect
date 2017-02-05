using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
using System.Windows.Forms;
using System.IO;

namespace ModelScenePlugin
{
	/// <summary>
	/// Represent a node in the scene graph. It can contains one or several models to render,
	/// but it can also be empty, and thus just be used as a transform group.
	/// </summary>
	public class SceneNode : TreeNode
	{
		/// <summary>
		/// DirectX device instance.
		/// </summary>
		private Device m_Device;
		/// <summary>
		/// Absolute path of the XML scene file (used as reference path to load model textures).
		/// </summary>
		private string m_Path;
		/// <summary>
		/// Stores the models according to their relative filename.
		/// </summary>
		private Dictionary<string, Model> m_Models = new Dictionary<string, Model>();
		/// <summary>
		/// Stores all the primitive transforms applied to this node.
		/// </summary>
		private List<PrimitiveTransform> m_PrimitiveTransforms = new List<PrimitiveTransform>();
		/// <summary>
		/// Stores the final transformation of the node.
		/// </summary>
		private Matrix m_Transform = Matrix.Identity;

		/// <summary>
		/// Node of the scene graph.
		/// </summary>
		/// <param name="device">DirectX device instance.</param>
		/// <param name="path">Absolute path of the XML scene file.</param>
		public SceneNode(Device device, string path)
		{
			m_Device = device;
			m_Path = path;
			this.Text = "root";
			this.Name = "_root_";
		}

		/// <summary>
		/// Node of the scene graph.
		/// </summary>
		/// <param name="device">DirectX device instance.</param>
		/// <param name="node">XML node from which to get the node description information.</param>
		/// <param name="path">Absolute path of the XML scene file.</param>
		public SceneNode(Device device, XmlNode node)
		{
			m_Device = device;

			// extract the xml file path from the base URI of the parent document element
			string path = Uri.UnescapeDataString(node.OwnerDocument.BaseURI);
			if (path.StartsWith("file:///"))
				path = path.Substring(8);
			path = path.Replace('/', '\\');
			m_Path = Path.GetDirectoryName(path);

			this.Text = "node";
			// loads the node information
			LoadNode(node);
		}

		/// <summary>
		/// Node of the scene graph.
		/// </summary>
		/// <param name="device">DirectX device instance.</param>
		/// <param name="model">Model to display (unique element of the node).</param>
		public SceneNode(Device device, Model model)
		{
			m_Device = device;
			this.Text = "model";
			// add model to the render model collection
			m_Models.Add("model", model);
		}

		/// <summary>
		/// Loads the node information.
		/// </summary>
		/// <param name="node">XmlNode that holds the information of the current node.</param>
		private void LoadNode(XmlNode node)
		{
			XmlAttribute attr;
			attr = node.Attributes["name"]; // check node name
			if (attr != null)
				this.Text = attr.Value.Trim();

			// check node elements
			foreach (XmlNode n in node.ChildNodes)
			{
				string nodename = n.Name;
				if (nodename.StartsWith("Node.")) // transform data
				{
					if (nodename == "Node.Translation") // primitive translation transform
						m_PrimitiveTransforms.Add(new PrimitiveTransform(TransformType.Translation, ReadNodeTransform(n, 0.0f)));
					else if (nodename == "Node.RotationX") // primitive X axis rotation transform
						m_PrimitiveTransforms.Add(new PrimitiveTransform(TransformType.RotationX, ReadNodeRotateTransform(n)));
					else if (nodename == "Node.RotationY") // primitive Y axis rotation transform
						m_PrimitiveTransforms.Add(new PrimitiveTransform(TransformType.RotationY, ReadNodeRotateTransform(n)));
					else if (nodename == "Node.RotationZ") // primitive Z axis rotation transform
						m_PrimitiveTransforms.Add(new PrimitiveTransform(TransformType.RotationZ, ReadNodeRotateTransform(n)));
					else if (nodename == "Node.Scale") // primitive scale transform
						m_PrimitiveTransforms.Add(new PrimitiveTransform(TransformType.Scale, ReadNodeTransform(n, 1.0f)));
				}
				else if (nodename == "Model") // model data
				{
					// get model relative filename
					attr = n.Attributes["filename"];
					if (attr != null)
					{
						// remove eventual useless spaces and ensure filename is not empty
						string modelFilename = attr.Value.Trim();
						if (modelFilename.Length > 0)
						{
							// combine absolute path of the xml scene file with the relative model filename
							string filename = Path.Combine(m_Path, modelFilename);
							// get model from the ResourceManager and add it to the model render collection
							m_Models.Add(modelFilename, ResourceManager.GetModel(filename));
						}
					}
				}
				else if (nodename == "Node") // subnode data
				{
					// create and add subnode to the current node, and so on...
					SceneNode subnode = new SceneNode(m_Device, n);
					this.Nodes.Add(subnode);
				}
			}

			// creates the node transform matrix according to all the current node primitive transforms
			UpdateTransform();
		}

		/// <summary>
		/// Reads primitive rotation transform (scalar) information from an XmlNode.
		/// </summary>
		/// <param name="node">XmlNode instance from which to read the rotation information.</param>
		/// <returns>Returns the angle value read from the node.</returns>
		private float ReadNodeRotateTransform(XmlNode node)
		{
			// look for angle attribute
			XmlAttribute attr = node.Attributes["angle"];

			float a;
			// try to get the floating point angle value
			if (attr == null || float.TryParse(attr.Value.Trim(), out a) == false)
				a = 0.0f; // if not possible, 0 is set as default value

			return (a);
		}

		/// <summary>
		/// Reads primitive vector transform information from an XmlNode.
		/// </summary>
		/// <param name="node">XmlNode instance from which to read the vector information.</param>
		/// <param name="defaultValue">Default value to return in case of expected data could not be retrieved from XmlNode instance.</param>
		/// <returns>Returns a vector containing X, Y and Z transform values.</returns>
		private Vector3 ReadNodeTransform(XmlNode node, float defaultValue)
		{
			XmlAttribute xAttr = node.Attributes["x"];
			XmlAttribute yAttr = node.Attributes["y"];
			XmlAttribute zAttr = node.Attributes["z"];

			float x, y, z;

			// try to get floating point X transform value
			if (xAttr == null || float.TryParse(xAttr.Value.Trim(), out x) == false)
				x = defaultValue;

			// try to get floating point Y transform value
			if (yAttr == null || float.TryParse(yAttr.Value.Trim(), out y) == false)
				y = defaultValue;

			// try to get floating point Z transform value
			if (zAttr == null || float.TryParse(zAttr.Value.Trim(), out z) == false)
				z = defaultValue;

			return (new Vector3(x, y, z));
		}

		/// <summary>
		/// Render the current scene node, and subnodes.
		/// </summary>
		public virtual void Render()
		{
			// saves the current world matrix
			Matrix mtx = m_Device.Transform.World;
			// applies the node transform to the current world
			m_Device.Transform.World = m_Transform * m_Device.Transform.World;

			// renders all models of the node
			foreach (Model model in m_Models.Values)
				model.Render();

			// if this node has been selected by the user from the UI
			// set the transform matrices to render the different axis
			if (this.IsSelected)
			{
				Matrix axisMatrix1 = mtx;
				Matrix axisMatrix2 = mtx;

				// look for the index of the primitive transform currently
				// selected by the user from the UI, in the current node
				int n = -1;
				for (int k = 0; k < this.PrimitiveTransforms.Count; k++)
				{
					if (this.PrimitiveTransforms[k].Selected)
					{
						// selected item found, stores the index and exit the loop
						n = k;
						break;
					}
				}

				if (n > -1) // check if there were element selected or not
				{
					// creates the local axis transform matrix applying all the primitive transform until the selected one
					axisMatrix1 = UpdateTransform(n, axisMatrix1);
					// creates the current axis transform matrix applying all the primitive transform until the one just after the selected one
					axisMatrix2 = UpdateTransform(n + 1, axisMatrix2);
				}

				// set member matrices of the ModelScene class for further render purpose
				// (not so clean design!)
				ModelScene.m_SelectedNodeMatrix = axisMatrix1;
				ModelScene.m_SelectedNodeCurrentMatrix = axisMatrix2;
			}

			// render all subnodes (recursively)
			foreach (SceneNode subnode in this.Nodes)
				subnode.Render();

			// restores the world matrix
			m_Device.Transform.World = mtx;
		}

		/// <summary>
		/// Gets all the primitive transform that compose the current node transform.
		/// </summary>
		public List<PrimitiveTransform> PrimitiveTransforms
		{
			get
			{
				return (m_PrimitiveTransforms);
			}
		}

		/// <summary>
		/// Creates the node local transform matrix of the current node, applying all the primitive transforms.
		/// </summary>
		public void UpdateTransform()
		{
			m_Transform = UpdateTransform(m_PrimitiveTransforms.Count, Matrix.Identity);
		}

		/// <summary>
		/// Creates and returns a node transform matrix, applying the n first primitive transforms, based on a pre-existing transform.
		/// </summary>
		/// <param name="n">The n first primitive transforms of the current node to apply.</param>
		/// <param name="baseMatrix">Base transform on which to cumultate the current node primitive transforms.</param>
		/// <returns>Returns a transform matrix.</returns>
		public Matrix UpdateTransform(int n, Matrix baseMatrix)
		{
			for (int i = 0; i < n; i++)
				baseMatrix = m_PrimitiveTransforms[i].Transform * baseMatrix;
			return (baseMatrix);
		}

		/// <summary>
		/// Gets the local node transform matrix.
		/// </summary>
		public Matrix Transform
		{
			get
			{
				return (m_Transform);
			}
		}

		/// <summary>
		/// Returns the node name (Text property of the TreeNode base class).
		/// </summary>
		/// <returns>Returns the node name (Text property of the TreeNode base class).</returns>
		public override string ToString()
		{
			return (this.Text);
		}

		/// <summary>
		/// Writes the content information of the node as XML.
		/// </summary>
		/// <param name="xw">XmlTextWriter instance representing the XML file to write to.</param>
		public void WriteXml(XmlTextWriter xw)
		{
			xw.WriteStartElement("Node");
			xw.WriteAttributeString("name", this.Text); // node name
			{
				// write all the primitive transforms first
				foreach (PrimitiveTransform transform in m_PrimitiveTransforms)
					transform.WriteXml(xw);

				// then write the models
				foreach (string filename in m_Models.Keys)
				{
					xw.WriteStartElement("Model");
					xw.WriteAttributeString("filename", filename);
					xw.WriteEndElement();
				}

				// then writes the subnodes
				foreach (SceneNode subnode in this.Nodes)
					subnode.WriteXml(xw);
			}
			xw.WriteEndElement();
		}
	}
}
