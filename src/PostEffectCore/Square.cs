using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.DirectX.Direct3D;
using Microsoft.DirectX;

namespace PostEffectCore
{
	public class Square : IDisposable
	{
		private Device m_Device;
		private VertexBuffer m_VertexBuffer;

		public Square(Device device)
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
			if (m_VertexBuffer != null)
				m_VertexBuffer.Dispose();
		}

		private void CreateVertexBuffer()
		{
			Dispose();

			float w = (float)m_Device.Viewport.Width;
			float h = (float)m_Device.Viewport.Height;

			float z = 0.0f;

			float offset = -0.5f;

			Vector4[] vects = new Vector4[]
			{

		        //top left										// top right
		        new Vector4(offset, offset, z, 1.0f),			new Vector4(w + offset, offset, z, 1.0f),

		        // bottom left									// bottom right
		        new Vector4(offset, h + offset, z, 1.0f),		new Vector4(w + offset, h + offset, z, 1.0f),

		    };

			CustomVertex.TransformedTextured[] vertices = new CustomVertex.TransformedTextured[6];
			vertices[0] = new CustomVertex.TransformedTextured(vects[0], 0.0f, 0.0f);
			vertices[1] = new CustomVertex.TransformedTextured(vects[3], 1.0f, 1.0f);
			vertices[2] = new CustomVertex.TransformedTextured(vects[2], 0.0f, 1.0f);

			vertices[3] = new CustomVertex.TransformedTextured(vects[0], 0.0f, 0.0f);
			vertices[4] = new CustomVertex.TransformedTextured(vects[1], 1.0f, 0.0f);
			vertices[5] = new CustomVertex.TransformedTextured(vects[3], 1.0f, 1.0f);

			m_VertexBuffer = new VertexBuffer(typeof(CustomVertex.TransformedTextured), vertices.Length, m_Device, Usage.WriteOnly, CustomVertex.TransformedTextured.Format, Pool.Managed);

			m_VertexBuffer.SetData(vertices, 0, LockFlags.None);
		}

		public void Draw()
		{
			Matrix proj = m_Device.Transform.Projection;
			Matrix world = m_Device.Transform.World;
			Matrix view = m_Device.Transform.View;

			m_Device.Transform.Projection = Matrix.OrthoOffCenterRH(0.0f, m_Device.Viewport.Width, m_Device.Viewport.Height, 0.0f, 0.0f, 1.0f);
			m_Device.Transform.World = Matrix.Identity;
			m_Device.Transform.View = Matrix.Identity;

			//bool zBufferEnable = m_Device.RenderState.ZBufferEnable;
			//bool zBufferWriteEnable = m_Device.RenderState.ZBufferWriteEnable;

			//m_Device.RenderState.ZBufferEnable = false;
			//m_Device.RenderState.ZBufferWriteEnable = false;

			m_Device.SamplerState[0].MagFilter = TextureFilter.Linear;
			m_Device.SamplerState[0].MinFilter = TextureFilter.Linear;
			m_Device.SamplerState[0].MipFilter = TextureFilter.None;

			m_Device.SamplerState[0].AddressU = TextureAddress.Clamp;
			m_Device.SamplerState[0].AddressV = TextureAddress.Clamp;
			m_Device.SamplerState[0].AddressW = TextureAddress.Clamp;

			m_Device.RenderState.Lighting = false;

			m_Device.SetStreamSource(0, m_VertexBuffer, 0);
			m_Device.VertexFormat = CustomVertex.TransformedTextured.Format;
			m_Device.DrawPrimitives(PrimitiveType.TriangleList, 0, 2);

			//m_Device.RenderState.ZBufferEnable = zBufferEnable;
			//m_Device.RenderState.ZBufferWriteEnable = zBufferWriteEnable;

			m_Device.Transform.View = view;
			m_Device.Transform.World = world;
			m_Device.Transform.Projection = proj;
		}
	}
}
