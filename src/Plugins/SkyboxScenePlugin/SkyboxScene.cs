using System;
using System.Collections.Generic;
using System.Text;
using IScenePlugin;
using Microsoft.DirectX.Direct3D;
using System.Windows.Forms;
using System.Drawing;
using Microsoft.DirectX;
using System.IO;
using System.Reflection;

namespace SkyboxScenePlugin
{
	public class SkyboxScene : Scene
	{
		private Device m_Device;
		private Color m_ClearColor;
		private Skybox m_Skybox;

		private string m_PluginPath;
		private string[] m_SkyboxDefinitionFiles;
		private int m_SkyboxIndex;

		public override void Initialize(ScenePluginInitParams prms)
		{
			m_Device = prms.Device;
			m_ClearColor = Color.FromArgb(128, 128, 128);
			m_PluginPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

			string skyboxesPath = string.Format("{0}\\SkyboxScenePlugin.skyboxes", m_PluginPath);
			m_SkyboxDefinitionFiles = Directory.GetFiles(skyboxesPath, "*.xml", SearchOption.AllDirectories);
			Array.Sort(m_SkyboxDefinitionFiles);

			m_Device.Transform.Projection = Matrix.PerspectiveFovRH(m_FieldOfView * (float)Math.PI / 180.0f, 4.0f / 3.0f, 0.00001f, 10000.0f);
			m_Device.Transform.World = Matrix.RotationY(m_RotateX) * Matrix.RotationX(m_RotateY);

			ResetSkybox();
		}

		public override string Name
		{
			get
			{
				return ("SkyBox Scene [Studio Post-Effect]");
			}
		}

		internal void SetSkybox(int index)
		{
			m_SkyboxIndex = index;
			if (m_Device != null)
				ResetSkybox();
		}

		internal int GetSkybox()
		{
			return (m_SkyboxIndex);
		}

		public override void OnDeviceReset(Device device)
		{
			device.RenderState.CullMode = Cull.None;
			device.RenderState.ZBufferEnable = true;
			device.RenderState.Lighting = false;

			device.SamplerState[0].MagFilter = TextureFilter.Linear;
			device.SamplerState[0].MinFilter = TextureFilter.Linear;

			m_Skybox.OnDeviceReset(m_Device, null);
		}

		private void ResetSkybox()
		{
			try
			{
				Skybox tmp = new Skybox(m_Device, m_SkyboxDefinitionFiles[m_SkyboxIndex]);
				if (m_Skybox != null)
					m_Skybox.Dispose();
				m_Skybox = tmp;
				m_Skybox.OnDeviceReset(m_Device, null);
			}
			catch (Exception ex)
			{
				string msg = string.Format("Impossible to create skybox '{0}'\r\n\r\n{1}", Path.GetFileNameWithoutExtension(m_SkyboxDefinitionFiles[m_SkyboxIndex]), ex.Message);
				MessageBox.Show(msg, "Skybox Creation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		public override void OnFilesDropped(int x, int y, string[] files)
		{
			try
			{
				Skybox skybox = null;
				FileInfo fi = new FileInfo(files[0]);

				if ((fi.Attributes & FileAttributes.Directory) == FileAttributes.Directory)
				{
					files = Directory.GetFiles(files[0], "*.xml", SearchOption.TopDirectoryOnly);
					if (files.Length > 0)
						skybox = new Skybox(m_Device, files[0]);
				}
				else
					skybox = new Skybox(m_Device, files[0]);

				if (skybox != null)
					m_Skybox = skybox;
			}
			catch
			{
			}
		}

		public override void OnMouseDown(MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
			{
				m_RotateMouseDownX = m_RotateX;
				m_RotateMouseDownY = m_RotateY;

				m_MouseDownX = e.X;
				m_MouseDownY = e.Y;
			}
		}

		public override void OnMouseWheel(MouseEventArgs e)
		{
			m_FieldOfView = Math.Max(10.0f, Math.Min(m_FieldOfView - (e.Delta / 120.0f), 170.0f));
		}

		public override void OnMouseMove(MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
			{
				m_RotateX = m_RotateMouseDownX + ((m_MouseDownX - e.X) / 100.0f);
				m_RotateY = m_RotateMouseDownY + ((m_MouseDownY - e.Y) / 100.0f);

				m_Device.Transform.View = Matrix.Identity;
				m_Device.Transform.World = Matrix.RotationY(m_RotateX) * Matrix.RotationX(m_RotateY);
			}
		}

		public override void ConfigPanelOpen()
		{
			frmConfig frm = new frmConfig(this, m_SkyboxDefinitionFiles);
			frm.TopMost = true;
			frm.Show();
		}

		private int m_MouseDownX;
		private int m_MouseDownY;
		private float m_RotateMouseDownX;
		private float m_RotateMouseDownY;

		private float m_RotateX = 0.001f;
		private float m_RotateY = 0.0f;

		private float m_FieldOfView = 105.0f;

		public override void UpdateScene(double elapsedTime)
		{
			m_Device.Transform.Projection = Matrix.PerspectiveFovRH(m_FieldOfView * (float)Math.PI / 180.0f, 4.0f / 3.0f, 0.00001f, 10000.0f);
		}

		public override void RenderScene(Device device)
		{
			device.Clear(ClearFlags.Target | ClearFlags.ZBuffer, m_ClearColor, 1.0f, 0);
			if (m_Skybox != null)
				m_Skybox.Draw();
		}
	}
}
