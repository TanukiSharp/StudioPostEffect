using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.DirectX.Direct3D;
using Microsoft.DirectX;

namespace ModelScenePlugin
{
	public class Axis : IDisposable
	{
		private Device m_Device;
		private VertexBuffer m_VertexBufferLight;
		private VertexBuffer m_VertexBufferDark;

		public Axis(Device device)
		{
			m_Device = device;
			m_Device.DeviceReset += new EventHandler(OnDeviceReset);
			OnDeviceReset(m_Device, null);
		}

		public Device Device
		{
			get
			{
				return (m_Device);
			}
		}

		private void OnDeviceReset(object sender, EventArgs e)
		{
			CreateVertexBuffer();
		}

		public void Dispose()
		{
			if (m_VertexBufferLight != null)
				m_VertexBufferLight.Dispose();
			if (m_VertexBufferDark != null)
				m_VertexBufferDark.Dispose();
		}

		private void CreateVertexBuffer()
		{
			Dispose();

			float size = 30.0f;

			Vector3[] vects = new Vector3[]
			{
		        new Vector3(0.0f, 0.0f, 0.0f),

				new Vector3(size, 0.0f, 0.0f),
		        new Vector3(0.0f, size, 0.0f),
		        new Vector3(0.0f, 0.0f, size),
		    };

			uint red_light = 0xFFFF0000;
			uint green_light = 0xFF00FF00;
			uint blue_light = 0xFF0000FF;

			uint red_dark = 0xFF800000;
			uint green_dark = 0xFF008000;
			uint blue_dark = 0xFF000080;

			CustomVertex.PositionColored[] vertices_light = new CustomVertex.PositionColored[6];
			vertices_light[0] = new CustomVertex.PositionColored(vects[0], (int)red_light);
			vertices_light[1] = new CustomVertex.PositionColored(vects[1], (int)red_light);

			vertices_light[2] = new CustomVertex.PositionColored(vects[0], (int)green_light);
			vertices_light[3] = new CustomVertex.PositionColored(vects[2], (int)green_light);

			vertices_light[4] = new CustomVertex.PositionColored(vects[0], (int)blue_light);
			vertices_light[5] = new CustomVertex.PositionColored(vects[3], (int)blue_light);

			CustomVertex.PositionColored[] vertices_dark = new CustomVertex.PositionColored[6];
			vertices_dark[0] = new CustomVertex.PositionColored(vects[0], (int)red_dark);
			vertices_dark[1] = new CustomVertex.PositionColored(vects[1], (int)red_dark);

			vertices_dark[2] = new CustomVertex.PositionColored(vects[0], (int)green_dark);
			vertices_dark[3] = new CustomVertex.PositionColored(vects[2], (int)green_dark);

			vertices_dark[4] = new CustomVertex.PositionColored(vects[0], (int)blue_dark);
			vertices_dark[5] = new CustomVertex.PositionColored(vects[3], (int)blue_dark);

			m_VertexBufferLight = new VertexBuffer(typeof(CustomVertex.PositionColored), vertices_light.Length, m_Device, Usage.WriteOnly, CustomVertex.PositionColored.Format, Pool.Managed);
			m_VertexBufferDark = new VertexBuffer(typeof(CustomVertex.PositionColored), vertices_dark.Length, m_Device, Usage.WriteOnly, CustomVertex.PositionColored.Format, Pool.Managed);

			m_VertexBufferLight.SetData(vertices_light, 0, LockFlags.None);
			m_VertexBufferDark.SetData(vertices_dark, 0, LockFlags.None);
		}

		public void Draw(bool light)
		{
			bool zBufferEnable = m_Device.RenderState.ZBufferEnable;
			bool zBufferWriteEnable = m_Device.RenderState.ZBufferWriteEnable;

			m_Device.RenderState.ZBufferEnable = false;
			m_Device.RenderState.ZBufferWriteEnable = false;

			bool lighting = m_Device.RenderState.Lighting;
			m_Device.RenderState.Lighting = false;

			m_Device.SetTexture(0, null);

			if (light)
				// draw normal red, green and blue axis
				m_Device.SetStreamSource(0, m_VertexBufferLight, 0);
			else
				// draw darker color red, green and blue axis
				m_Device.SetStreamSource(0, m_VertexBufferDark, 0);

			m_Device.VertexFormat = CustomVertex.PositionColored.Format;
			m_Device.DrawPrimitives(PrimitiveType.LineList, 0, 3);

			m_Device.RenderState.Lighting = lighting;

			m_Device.RenderState.ZBufferEnable = zBufferEnable;
			m_Device.RenderState.ZBufferWriteEnable = zBufferWriteEnable;
		}
	}
}
