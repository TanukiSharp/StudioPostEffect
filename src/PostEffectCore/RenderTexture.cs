using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.DirectX.Direct3D;
using System.Drawing;

namespace PostEffectCore
{
	public class RenderTexture : IDisposable
	{
		private Device m_Device;
		private Size m_CurrentSize;
		private Size m_OriginalSize;
		private SizeMode m_SizeMode;
		private Texture[] m_Texture;
		private Surface[] m_Surface;
		private int m_CurrentBufferIndex;
		private Surface m_PreviousRenderTarget = null;
		private Surface m_PreviousDepthSurface = null;
		private Texture m_AACopyTexture = null;
		private Surface m_AACopySurface = null;

		private bool m_MultiSampleAvailable = false;
		private bool m_AntiAliasing = false;
		private bool m_AntiAliasingChangeRequest = false;
		private int m_MultiSampleQuality = 0;
		private MultiSampleType m_MultiSampleType = MultiSampleType.None;

		/// <summary>
		/// Back buffer sizing definition.
		/// </summary>
		public enum SizeMode
		{
			Normal,
			Half,
			Quart,
		}

		/// <summary>
		/// Instanciate a 'render to texture' capable object.
		/// It automatically manages the antialising (off by default).
		/// </summary>
		/// <param name="device">DirectX device instance.</param>
		/// <param name="sizeMode">Size mode, for half or quarter buffers (not yet implemented).</param>
		public RenderTexture(Device device, SizeMode sizeMode)
			: this(device, sizeMode, false)
		{
		}

		/// <summary>
		/// Instanciate a 'render to texture' capable object.
		/// It automatically manages the antialising.
		/// </summary>
		/// <param name="device">DirectX device instance.</param>
		/// <param name="sizeMode">Size mode, for half or quarter buffers (not yet implemented).</param>
		/// <param name="antiAliasing">Flag that indicates the antialising usage.</param>
		public RenderTexture(Device device, SizeMode sizeMode, bool antiAliasing)
		{
			m_Device = device;
			m_SizeMode = sizeMode;

			// check for multisampling capability
			m_MultiSampleAvailable = DirectXHelper.CheckForMultiSampling(m_Device, out m_MultiSampleType, out m_MultiSampleQuality);
			// antialiasing is off by default, set to true only
			// if hardware is capable and it user requested it
			if (m_MultiSampleAvailable && antiAliasing)
				m_AntiAliasing = true;

			m_Device.DeviceReset += new EventHandler(OnDeviceReset);
			OnDeviceReset(m_Device, null);
		}

		/// <summary>
		/// Gets the highest multi-sampling (antialiasing) quality available on the current hardware.
		/// </summary>
		public int MultiSampleQuality
		{
			get
			{
				return (m_MultiSampleQuality);
			}
		}

		/// <summary>
		/// Gets the highest multi-sampling (antialiasing) type (none, 2x, 4x, etc...) available on the current hardware.
		/// </summary>
		public MultiSampleType MultiSampleType
		{
			get
			{
				return (m_MultiSampleType);
			}
		}

		/// <summary>
		/// Gets flag indicating if the current hardware can manage multi-sampling (antialiasing).
		/// </summary>
		public bool MultiSampleAvailable
		{
			get
			{
				return (m_MultiSampleAvailable);
			}
		}

		/// <summary>
		/// Gets or sets the antialising usage.
		/// If the current hardware is not capable of antialiasing, this call is just ignored.
		/// (Use MultiSampleAvailable property to check capability).
		/// </summary>
		public bool AntiAliasing
		{
			get
			{
				return (m_AntiAliasing);
			}
			set
			{
				// skip if antialiasing is not available
				if (m_MultiSampleAvailable)
				{
					// value changed so let's indicate to swap antialiasing flag
					if (value != m_AntiAliasing)
						m_AntiAliasingChangeRequest = true;
				}
			}
		}

		private void OnDeviceReset(object sender, EventArgs e)
		{
			// dispose resources before re-allocating some again
			Dispose();

			// gather information about current the back buffer
			int width = m_Device.PresentationParameters.BackBufferWidth;
			int height = m_Device.PresentationParameters.BackBufferHeight;
			Format backBufferFormat = m_Device.PresentationParameters.BackBufferFormat;
			DepthFormat depthFormat = m_Device.PresentationParameters.AutoDepthStencilFormat;

			// set original size according to back buffer data
			m_OriginalSize = new Size(width, height);
			// for the moment sizing is not yet implemented
			m_CurrentSize = new Size(width, height);

			// instanciate 2 surfaces since they will be used in
			// either modes (antialiasing or not)
			m_Surface = new Surface[2];

			if (m_AntiAliasing)
			{
				// if antialiasing is activated

				// create multisample surface for render (back buffer and depth buffer)
				m_Surface[0] = m_Device.CreateRenderTarget(m_CurrentSize.Width, m_CurrentSize.Height, backBufferFormat, m_MultiSampleType, m_MultiSampleQuality, false);
				m_Surface[1] = m_Device.CreateDepthStencilSurface(m_CurrentSize.Width, m_CurrentSize.Height, depthFormat, m_MultiSampleType, m_MultiSampleQuality, false);

				// prepare render target texture (non-antialiasing) for post-processing purpose
				m_AACopyTexture = new Texture(m_Device, m_CurrentSize.Width, m_CurrentSize.Height, 1, Usage.RenderTarget, backBufferFormat, Pool.Default);
				// retrieve texture's surface for offscreen render
				m_AACopySurface = m_AACopyTexture.GetSurfaceLevel(0);
			}
			else
			{
				// if antialiasing is desactivated

				// instanciate 2 texture for back buffer purpose (2 because of swapping mecanism)
				m_Texture = new Texture[2];

				for (int i = 0; i < 2; i++)
				{
					// create the render target purpose texture using current back buffer information
					m_Texture[i] = new Texture(m_Device, m_CurrentSize.Width, m_CurrentSize.Height, 1, Usage.RenderTarget, backBufferFormat, Pool.Default);
					// retrieve the render surface of the texture
					m_Surface[i] = m_Texture[i].GetSurfaceLevel(0);
				}
			}
		}

		/// <summary>
		/// Redirects all the future renders to self offscreen surface.
		/// </summary>
		public void BeginRender()
		{
			// check for antialiasing setting change
			if (m_AntiAliasingChangeRequest)
			{
				// change antialiasing setting
				m_AntiAliasing = !m_AntiAliasing;
				// recreate render targets with new antialiasing settings
				OnDeviceReset(m_Device, null);
				// unset change flag
				m_AntiAliasingChangeRequest = false;
			}

			// consistency state check, ensure BeginRender was not called twice
			if (m_PreviousRenderTarget != null)
				throw new Exception("Already in render target mode");

			// save the current back buffer and depth buffer to restore them later
			m_PreviousRenderTarget = m_Device.GetRenderTarget(0);
			m_PreviousDepthSurface = m_Device.DepthStencilSurface;

			if (m_AntiAliasing)
			{
				// if antialiasing is activated, set proper antialiasing purpose surfaces
				// (back buffer and depth buffer)
				m_Device.SetRenderTarget(0, m_Surface[0]);
				m_Device.DepthStencilSurface = m_Surface[1];
			}
			else
			{
				// if antialiasing is not activated, set the swappable, render target, non-multisample render surface
				m_Device.SetRenderTarget(0, m_Surface[m_CurrentBufferIndex]);
			}
		}

		/// <summary>
		/// Terminates renders redirection and provides a texture containing the renders.
		/// </summary>
		/// <returns>Returns a texture containing all the renders between BeginRender and this call.</returns>
		public Texture EndRender()
		{
			// consistency state check, ensure EndRender was not called twice
			if (m_PreviousRenderTarget == null)
				throw new Exception("Not in render target mode");

			// restore the previously saved default back buffer
			// (also called 'implicit swap chain back buffer')
			m_Device.SetRenderTarget(0, m_PreviousRenderTarget);
			// dispose is called just to decremente the COM reference count
			m_PreviousRenderTarget.Dispose();
			// clean the instance to notify that normal state was properly restored
			m_PreviousRenderTarget = null;


			//if (m_PreviousDepthSurface != null)
			//{
				m_Device.DepthStencilSurface = m_PreviousDepthSurface;
				m_PreviousDepthSurface.Dispose();
				m_PreviousDepthSurface = null;
			//}


			if (m_AntiAliasing)
			{
				Rectangle rect = new Rectangle(new Point(0, 0), m_CurrentSize);
				m_Device.StretchRectangle(m_Surface[0], rect, m_AACopySurface, rect, TextureFilter.Linear);
				return (m_AACopyTexture);
			}
			else
			{
				Texture result = m_Texture[m_CurrentBufferIndex];
				m_CurrentBufferIndex ^= 1;
				return (result);
			}
		}

		/// <summary>
		/// Gets original back buffers size.
		/// </summary>
		public Size OriginalSize
		{
			get
			{
				return (m_OriginalSize);
			}
		}

		/// <summary>
		/// Gets current working back buffers size.
		/// </summary>
		public Size CurrentSize
		{
			get
			{
				return (m_CurrentSize);
			}
		}

		//private void ResetViewport()
		//{
		//    Viewport vp = new Viewport();
		//    vp.MinZ = m_Device.Viewport.MinZ;
		//    vp.MaxZ = m_Device.Viewport.MaxZ;
		//    vp.X = m_Device.Viewport.X;
		//    vp.Y = m_Device.Viewport.Y;

		//    vp.Width = m_OriginalSize.Width;
		//    vp.Height = m_OriginalSize.Height;

		//    m_Device.Viewport = vp;
		//}

		//private void SetViewportToSizeMode(out int width, out int height)
		//{
		//    width = m_OriginalSize.Width;
		//    height = m_OriginalSize.Height;

		//    if (m_SizeMode == SizeMode.Half)
		//    {
		//        width /= 2;
		//        height /= 2;
		//    }
		//    else if (m_SizeMode == SizeMode.Quart)
		//    {
		//        width /= 4;
		//        height /= 4;
		//    }

		//    Viewport vp = m_Device.Viewport;
		//    Viewport newVP = new Viewport();

		//    newVP.MinZ = vp.MinZ;
		//    newVP.MaxZ = vp.MaxZ;
		//    newVP.X = vp.X;
		//    newVP.Y = vp.Y;
		//    newVP.Width = width;
		//    newVP.Height = height;

		//    m_Device.Viewport = newVP;
		//}


		/// <summary>
		/// Dispose all allocated data.
		/// </summary>
		public void Dispose()
		{
			if (m_PreviousRenderTarget != null)
			{
				m_Device.SetRenderTarget(0, m_PreviousRenderTarget);
				m_PreviousRenderTarget.Dispose();
				m_PreviousRenderTarget = null;
			}

			if (m_Surface != null)
			{
				for (int i = 0; i < 2; i++)
				{
					if (m_Surface[i] != null)
						m_Surface[i].Dispose();
				}
			}

			if (m_AACopyTexture != null)
				m_AACopyTexture.Dispose();
			if (m_AACopySurface != null)
				m_AACopySurface.Dispose();

			if (m_Texture != null)
			{
				for (int i = 0; i < 2; i++)
				{
					if (m_Texture[i] != null)
						m_Texture[i].Dispose();
				}
			}
		}
	}
}
