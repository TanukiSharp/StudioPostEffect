using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using Microsoft.DirectX;
using System.Xml;

namespace ModelScenePlugin
{
	/// <summary>
	/// Primitive transform types.
	/// </summary>
	public enum TransformType
	{
		/// <summary>
		/// Translation transform.
		/// </summary>
		Translation,
		/// <summary>
		/// Rotation transform around the X axis.
		/// </summary>
		RotationX,
		/// <summary>
		/// Rotation transform around the Y axis.
		/// </summary>
		RotationY,
		/// <summary>
		/// Rotation transform around the Z axis.
		/// </summary>
		RotationZ,
		/// <summary>
		/// Scale transform.
		/// </summary>
		Scale
	}

	/// <summary>
	/// Represent a primitive transform.
	/// </summary>
	public class PrimitiveTransform : ListViewItem
	{
		/// <summary>
		/// Transform type that defines the current primitive transform.
		/// </summary>
		private TransformType m_TransformType;
		/// <summary>
		/// Scalar transform, holds a floating point value.
		/// </summary>
		private float m_ScalarTransform;
		/// <summary>
		/// Vector transform, holds a vectorial value.
		/// </summary>
		private Vector3 m_VectorTransform;
		/// <summary>
		/// Stores the transform matrix corresponding to the primitive transform.
		/// </summary>
		private Matrix m_Transform;

		/// <summary>
		/// Instanciate a PrimitiveTransform.
		/// </summary>
		/// <param name="type">Type of primitive transform.</param>
		/// <param name="scalarTransform">Scalar value associated to the transform.</param>
		public PrimitiveTransform(TransformType type, float scalarTransform)
		{
			this.SubItems[0].Text = type.ToString();
			this.SubItems.Add("");

			m_TransformType = type;
			ScalarTransform = scalarTransform;
		}

		/// <summary>
		/// Instanciate a PrimitiveTransform.
		/// </summary>
		/// <param name="type">Type of primitive transform.</param>
		/// <param name="vectorTransform">Vector associated to the transform.</param>
		public PrimitiveTransform(TransformType type, Vector3 vectorTransform)
		{
			this.SubItems[0].Text = type.ToString();
			this.SubItems.Add("");

			m_TransformType = type;
			VectorTransform = vectorTransform;
		}

		/// <summary>
		/// Gets or sets the primitive transform type.
		/// </summary>
		public TransformType TransformType
		{
			get
			{
				return (m_TransformType);
			}
		}

		/// <summary>
		/// Gets of sets the scalar transform value.
		/// </summary>
		public float ScalarTransform
		{
			get
			{
				return (m_ScalarTransform);
			}
			set
			{
				// check transform type, only rotation transforms can be set a scalar value
				if (m_TransformType == TransformType.Translation || m_TransformType == TransformType.Scale)
					throw new Exception("Wrong transform type");

				// updates the ListViewItem (base class) text
				m_ScalarTransform = value;
				if (this.ListView != null)
					this.ListView.SuspendLayout();
				this.SubItems[1].Text = m_ScalarTransform.ToString();
				if (this.ListView != null)
					this.ListView.ResumeLayout();

				// convert human-readable degrees to program-computable radians
				float radScalar = m_ScalarTransform * (float)Math.PI / 180.0f;

				// recreates the transform matrix
				if (m_TransformType == TransformType.RotationX)
					m_Transform = Matrix.RotationX(radScalar);
				else if (m_TransformType == TransformType.RotationY)
					m_Transform = Matrix.RotationY(radScalar);
				else //if (m_TransformType == TransformType.RotationZ)
					m_Transform = Matrix.RotationZ(radScalar);
			}
		}

		/// <summary>
		/// Gets or sets the vectorial transform value.
		/// </summary>
		public Vector3 VectorTransform
		{
			get
			{
				return (m_VectorTransform);
			}
			set
			{
				// check transform type, only translation and scale transforms can be set a vectorial value
				if (m_TransformType != TransformType.Translation && m_TransformType != TransformType.Scale)
					throw new Exception("Wrong transform type");

				// updates the ListViewItem (base class) text
				m_VectorTransform = value;
				if (this.ListView != null)
					this.ListView.SuspendLayout();
				this.SubItems[1].Text = string.Format("{0}; {1}; {2}", m_VectorTransform.X, m_VectorTransform.Y, m_VectorTransform.Z);
				if (this.ListView != null)
					this.ListView.ResumeLayout();

				// recreates the transform matrix
				if (m_TransformType == TransformType.Translation)
					m_Transform = Matrix.Translation(m_VectorTransform);
				else //if (m_TransformType == TransformType.Scale)
					m_Transform = Matrix.Scaling(m_VectorTransform);
			}
		}

		/// <summary>
		/// Gets the primitive transform matrix.
		/// </summary>
		public Matrix Transform
		{
			get
			{
				return (m_Transform);
			}
		}

		/// <summary>
		/// Writes content information to XML.
		/// </summary>
		/// <param name="xw">XmlTextWriter that represents the XML file to write to.</param>
		public void WriteXml(XmlTextWriter xw)
		{
			// write transform type
			xw.WriteStartElement(string.Format("Node.{0}", m_TransformType.ToString()));

			if (m_TransformType == TransformType.RotationX || m_TransformType == TransformType.RotationY || m_TransformType == TransformType.RotationZ)
				// rotation transform
				xw.WriteAttributeString("angle", m_ScalarTransform.ToString());
			else
			{
				// vectorial transform
				xw.WriteAttributeString("x", m_VectorTransform.X.ToString());
				xw.WriteAttributeString("y", m_VectorTransform.Y.ToString());
				xw.WriteAttributeString("z", m_VectorTransform.Z.ToString());
			}
			xw.WriteEndElement();
		}
	}
}
