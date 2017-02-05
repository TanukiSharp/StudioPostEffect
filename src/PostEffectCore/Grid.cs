using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.DirectX.Direct3D;
using Microsoft.DirectX;
using System.Drawing;
using IScenePlugin;

namespace PostEffectCore
{
	public class Grid : IRenderable
	{
		private Device m_Device;
		private VertexBuffer m_VertexBuffer;
		private Size m_Size;
		private int m_Color;
		private CustomVertex.PositionColored[] m_Vertices;

		public Grid(Device device, Size size, Color color)
		{
			m_Device = device;
			m_Size = size;
			m_Color = color.ToArgb();
			CreateVertices();
			m_Device.DeviceReset += new EventHandler(OnDeviceReset);
			OnDeviceReset(m_Device, null);
		}

		private void CreateVertices()
		{
			List<CustomVertex.PositionColored> vertices = new List<CustomVertex.PositionColored>();

			float y = 0.0f;

			int size = 400;
			int step = 10;

			int hsize = size / 2;
			int i;
			for (i = -hsize; i < hsize; i += step)
			{
				vertices.Add(new CustomVertex.PositionColored((float)i, y, (float)-hsize, m_Color));
				vertices.Add(new CustomVertex.PositionColored((float)i, y, (float)hsize, m_Color));
				vertices.Add(new CustomVertex.PositionColored((float)-hsize, y, (float)i, m_Color));
				vertices.Add(new CustomVertex.PositionColored((float)hsize, y, (float)i, m_Color));
			}
			vertices.Add(new CustomVertex.PositionColored((float)i, y, (float)-hsize, m_Color));
			vertices.Add(new CustomVertex.PositionColored((float)i, y, (float)hsize, m_Color));
			vertices.Add(new CustomVertex.PositionColored((float)-hsize, y, (float)i, m_Color));
			vertices.Add(new CustomVertex.PositionColored((float)hsize, y, (float)i, m_Color));

			m_Vertices = vertices.ToArray();
		}

		private void OnDeviceReset(object sender, EventArgs e)
		{
			if (m_VertexBuffer != null)
				m_VertexBuffer.Dispose();

			m_VertexBuffer = new VertexBuffer(typeof(CustomVertex.PositionColored), m_Vertices.Length, m_Device, Usage.Dynamic | Usage.WriteOnly, CustomVertex.PositionColored.Format, Pool.Default);
			m_VertexBuffer.SetData(m_Vertices, 0, LockFlags.None);
		}

		public void Render()
		{
			Matrix mtx = m_Device.Transform.World;
			m_Device.Transform.World = Matrix.Identity;

			m_Device.RenderState.Lighting = false;

			m_Device.SetTexture(0, null);
			m_Device.SetStreamSource(0, m_VertexBuffer, 0);
			m_Device.VertexFormat = CustomVertex.PositionColored.Format;
			m_Device.DrawPrimitives(PrimitiveType.LineList, 0, m_Vertices.Length / 2);

			m_Device.Transform.World = mtx;
		}
	}
}
