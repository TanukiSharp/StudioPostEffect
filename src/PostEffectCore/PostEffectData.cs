using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.DirectX.Direct3D;
using System.Drawing;

namespace PostEffectCore
{
	public class PostEffectData
	{
		private Texture m_SceneFrame;
		private Texture m_PreviousFrame;
		private RenderTexture m_EffectRender;
		private Square m_Square;

		public PostEffectData(Device device, RenderTexture renderTexture)
		{
			m_EffectRender = renderTexture;
			m_Square = new Square(device);
		}

		public RenderTexture EffectRender
		{
			get
			{
				return (m_EffectRender);
			}
		}

		public Square Square
		{
			get
			{
				return (m_Square);
			}
		}

		public Texture SceneFrame
		{
			get
			{
				return (m_SceneFrame);
			}
			set
			{
				m_SceneFrame = value;
			}
		}

		public Texture PreviousFrame
		{
			get
			{
				return (m_PreviousFrame);
			}
			set
			{
				m_PreviousFrame = value;
			}
		}
	}
}
