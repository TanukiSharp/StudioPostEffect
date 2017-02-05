using System;
using System.Collections.Generic;
using System.Text;
using IScenePlugin;
using Microsoft.DirectX.Direct3D;
using System.Windows.Forms;
using System.Drawing;
using Microsoft.DirectX;

namespace DefaultScenePlugin
{
	public class DefaultScene : Scene
	{
		private Device m_Device;
		private IntPtr m_DeviceUnmanaged;

		private Color m_ClearColor;

		private Mesh m_MeshSphere;
		private Mesh m_MeshCube;
		private Mesh m_MeshCone;
		private Material m_Material;

		private ICamera m_Camera;
		private IRenderable m_Grid;

		// 3D objects rotation
		private float m_Rotation = 0.0f;

		public override string Name
		{
			get
			{
				return ("Default Scene [Studio Post-Effect]");
			}
		}

		// init the plugin
		public override void Initialize(ScenePluginInitParams prms)
		{
			// store main plugin element for further use
			m_Device = prms.Device;
			m_DeviceUnmanaged = m_Device.GetObjectByValue(-759872593);

			m_Camera = prms.Camera;
			m_Grid = prms.Grid;

			// clear color
			m_ClearColor = Color.FromArgb(77, 77, 69);

			//int result = ActivateDetour(m_DeviceUnmanaged);
		}

		public override void Terminate()
		{
			// turn off the used lights when the plugin is not used anymore
			m_Device.Lights[0].Enabled = false;
			m_Device.Lights[1].Enabled = false;

			//int result = DesactivateDetour();
		}

		public override void OnDeviceReset(Device device)
		{
			// fucking Vista trick to make lighting to work fine...
			device.Lights[0].Enabled = false;
			device.Lights[1].Enabled = false;

			// set the necessary render states
			device.RenderState.CullMode = Cull.None;
			device.RenderState.ZBufferEnable = true;
			device.RenderState.ZBufferWriteEnable = true;
			device.RenderState.NormalizeNormals = true;

			device.RenderState.Ambient = Color.Gray;

			device.RenderState.Lighting = true;

			// use two directional lights
			device.Lights[0].Type = LightType.Directional;
			device.Lights[0].Diffuse = Color.Gray;
			device.Lights[0].Ambient = Color.Gray;
			device.Lights[0].Direction = new Vector3(-1.0f, -1.0f, -1.0f);
			device.Lights[0].Enabled = true;

			device.Lights[1].Type = LightType.Directional;
			device.Lights[1].Diffuse = Color.FromArgb(64, 64, 64);
			device.Lights[1].Ambient = Color.Black;
			device.Lights[1].Direction = new Vector3(1.0f, 1.0f, 1.0f);
			device.Lights[1].Enabled = true;

			// release mesh resources before reallocating them
			if (m_MeshCube != null)
				m_MeshCube.Dispose();
			if (m_MeshSphere != null)
				m_MeshSphere.Dispose();
			if (m_MeshCone != null)
				m_MeshCone.Dispose();

			// create mesh objects
			m_MeshCube = Mesh.Box(device, 25.0f, 25.0f, 25.0f);
			m_MeshCube.ComputeNormals();
			m_MeshSphere = Mesh.Sphere(device, 15.0f, 30, 30);
			m_MeshSphere.ComputeNormals();
			m_MeshCone = Mesh.Cylinder(device, 15.0f, 0.0f, 25.0f, 30, 10);
			m_MeshCone.ComputeNormals();

			// set objects material
			Material material = new Material();
			material.Ambient = Color.FromArgb(0, 119, 170);
			material.Diffuse = Color.FromArgb(128, 220, 255);
			m_Material = material;
		}

		public override void OnMouseMove(MouseEventArgs e)
		{
			// update the camera according to the mouse events
			m_Camera.Update(e);
		}

		public override void UpdateScene(double elapsedTime)
		{
			// update the objects rotation angle based on time
			m_Rotation += (float)elapsedTime;
			m_Camera.Update();
		}

		public override void RenderScene(Device device)
		{
			//Test(m_DeviceUnmanaged);
			//return;

			// clear render
			device.Clear(ClearFlags.Target | ClearFlags.ZBuffer, m_ClearColor, 1.0f, 0);

			// draw grid
			m_Grid.Render();

			device.RenderState.Lighting = true;

			// unset texture
			device.SetTexture(0, null);
			// set objects material
			device.Material = m_Material;

			// draw objects
			device.Transform.World = Matrix.Translation(10.0f, 0.0f, 0.0f) * Matrix.RotationY(m_Rotation * 3.0f);
			m_MeshSphere.DrawSubset(0);
			device.Transform.World = Matrix.RotationY(m_Rotation) * Matrix.Translation(50.0f, 0.0f, 0.0f);
			m_MeshCube.DrawSubset(0);
			device.Transform.World = Matrix.RotationX(-(float)Math.PI / 2.0f) * Matrix.RotationZ(m_Rotation) * Matrix.Translation(-50.0f, 0.0f, 0.0f);
			m_MeshCone.DrawSubset(0);

			// restore default world transform
			device.Transform.World = Matrix.Identity;
		}

		/*
		[System.Runtime.InteropServices.DllImport("DetourDX.dll")]
		private static extern void Test(IntPtr device);
		[System.Runtime.InteropServices.DllImport("DetourDX.dll")]
		private static extern int ActivateDetour(IntPtr device);
		[System.Runtime.InteropServices.DllImport("DetourDX.dll")]
		private static extern int DesactivateDetour();
		*/
	}
}
