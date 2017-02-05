using System;
using System.Collections.Generic;
using System.Text;
using IScenePlugin;
using Microsoft.DirectX.Direct3D;
using System.Windows.Forms;
using System.Drawing;
using System.Xml;
using System.IO;
using System.Reflection;
using Microsoft.DirectX;

namespace ModelScenePlugin
{
	/// <summary>
	/// Core class of the plugin.
	/// This is class is the one instanciated and run by Studio Post-Effect application.
	/// </summary>
	public class ModelScene : Scene
	{
		/// <summary>
		/// DirectX device instance.
		/// </summary>
		private Device m_Device;
		/// <summary>
		/// Scene controllable orbital camera.
		/// </summary>
		private ICamera m_Camera;
		/// <summary>
		/// Scene grid.
		/// </summary>
		private IRenderable m_Grid;
		/// <summary>
		/// Render clear color.
		/// </summary>
		private Color m_ClearColor;
		/// <summary>
		/// Root scene node.
		/// </summary>
		private SceneNode m_RootSceneNode;
		/// <summary>
		/// Main setting window.
		/// </summary>
		private frmSceneGraph m_SceneGraphForm;
		/// <summary>
		/// Absolute path of the default scene files.
		/// </summary>
		private string m_ScenesPath;
		/// <summary>
		/// Coordinate system axis model.
		/// </summary>
		internal static Axis m_Axis;
		/// <summary>
		/// Local coordinate system matrix.
		/// </summary>
		internal static Matrix m_SelectedNodeMatrix;
		/// <summary>
		/// Current coordinate system matrix.
		/// </summary>
		internal static Matrix m_SelectedNodeCurrentMatrix;

		/// <summary>
		/// Rotation angle of the first light.
		/// </summary>
		internal float m_LightRotationAngle;

		/// <summary>
		/// Gets the plugin name.
		/// </summary>
		public override string Name
		{
			get
			{
				return ("Models Scene [Studio Post-Effect]");
			}
		}

		/// <summary>
		/// Initializes the plugin.
		/// </summary>
		/// <param name="prms">Initialization parameters provided by Studio Post-Effect application.</param>
		public override void Initialize(ScenePluginInitParams prms)
		{
			// stores DirectX device
			m_Device = prms.Device;
			// stores camera
			m_Camera = prms.Camera;
			// stores grid
			m_Grid = prms.Grid;
			m_ClearColor = Color.FromArgb(77, 77, 69);

			// initializes the ResourceManager for model caching
			ResourceManager.Initialize(m_Device);

			if (m_Axis == null)
				// instanciate the m_Axis static member
				m_Axis = new Axis(m_Device);

			// retrieve the default scenes path
			string allPluginsPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
			m_ScenesPath = Path.Combine(allPluginsPath, "ModelScenePlugin.scenes");

			// instanciate the SceneGraph window only if it hasn't been instanciated before or if it has been disposed
			if (m_SceneGraphForm == null || m_SceneGraphForm.IsDisposed)
				m_SceneGraphForm = new frmSceneGraph(m_Device, this, m_ScenesPath);

			// if it's first init, displays the SceneGraph window
			if (m_RootSceneNode == null)
			{
				m_SceneGraphForm.TopMost = true;
				m_SceneGraphForm.Show();
			}
		}

		/// <summary>
		/// Terminates the plugin, called when the user switches from this plugin to another one.
		/// </summary>
		public override void Terminate()
		{
			// dispose all the cache content
			ResourceManager.Clean();

			// close SceneGraph window
			m_SceneGraphForm.ForceClose();
			// dispose SceneGraph window
			m_SceneGraphForm.Dispose();
			// unset local elements
			m_SceneGraphForm = null;
			m_RootSceneNode = null;

			// disabled used lights
			m_Device.Lights[0].Enabled = false;
			m_Device.Lights[1].Enabled = false;
			m_Device.Lights[2].Enabled = false;
		}

		/// <summary>
		/// Allow external components to set the root scene node.
		/// </summary>
		/// <param name="root">Root scene node.</param>
		internal void SetRootNode(SceneNode root)
		{
			m_RootSceneNode = root;
		}

		public override void OnDeviceReset(Device device)
		{
			// fucking Vista trick to make lighting to work fine...
			// if lights are not desactivated before being set, the lighting does not
			// work on some PC running Vista operating system...
			// I let you guess how long time I spent to discover it... and let you guess how strange and useless it is...
			device.Lights[0].Enabled = false;
			device.Lights[1].Enabled = false;
			device.Lights[2].Enabled = false;

			// set required render states
			device.RenderState.CullMode = Cull.None;
			device.RenderState.ZBufferEnable = true;
			device.RenderState.ZBufferWriteEnable = true;
			device.RenderState.NormalizeNormals = true;

			// set first light (spinning)
			device.Lights[0].Type = LightType.Directional;
			device.Lights[0].Ambient = Color.FromArgb(32, 32, 32);
			device.Lights[0].Diffuse = Color.FromArgb(128, 128, 128);
			device.Lights[0].Specular = Color.White;
			device.Lights[0].Direction = new Vector3(-1.0f, -1.0f, -1.0f);
			device.Lights[0].Enabled = true;

			// set second light (fixed)
			device.Lights[1].Type = LightType.Directional;
			device.Lights[1].Ambient = Color.FromArgb(64, 64, 64);
			device.Lights[1].Diffuse = Color.FromArgb(92, 92, 92);
			device.Lights[1].Specular = Color.White;
			device.Lights[1].Direction = new Vector3(1.0f, 1.0f, 1.0f);
			device.Lights[1].Enabled = true;

			// set third light (fixed)
			device.Lights[2].Type = LightType.Directional;
			device.Lights[2].Ambient = Color.FromArgb(64, 64, 64);
			device.Lights[2].Diffuse = Color.FromArgb(92, 92, 92);
			device.Lights[2].Specular = Color.White;
			device.Lights[2].Direction = new Vector3(-1.0f, -1.0f, -1.0f);
			device.Lights[2].Enabled = true;
		}

		/// <summary>
		/// Opens the SceneGraph window.
		/// </summary>
		public override void ConfigPanelOpen()
		{
			// instanciate the SceneGraph window only if it hasn't been instanciated before or if it has been disposed
			if (m_SceneGraphForm == null || m_SceneGraphForm.IsDisposed)
				m_SceneGraphForm = new frmSceneGraph(m_Device, this, m_ScenesPath);

			// set TopMost and show the window
			m_SceneGraphForm.TopMost = true;
			m_SceneGraphForm.Show();
		}

		public override void OnMouseMove(MouseEventArgs e)
		{
			// updates the camera according to the mouse events
			m_Camera.Update(e);
		}

		/// <summary>
		/// Updates the scene elements.
		/// </summary>
		/// <param name="elapsedTime">Time elapsed between previous frame, in milliseconds.</param>
		public override void UpdateScene(double elapsedTime)
		{
			if (m_SceneGraphForm == null)
				return;

			// check light rotation setting
			if (m_SceneGraphForm.chkSpinningLight.Checked)
			{
				// light rotation is set to automatic, so let's update the lighting vector
				m_LightRotationAngle += (float)elapsedTime * 2.0f; // according to the time
				if (m_LightRotationAngle >= 6.2831854f)
					m_LightRotationAngle = 0.0f; // reset rotation angle to 0 if it reaches 360 degrees
			}

			// computes new directional lighting vector according to light rotation angle
			float nz = (float)(-Math.Cos(m_LightRotationAngle) + Math.Sin(m_LightRotationAngle));
			float nx = (float)(-Math.Sin(m_LightRotationAngle) - Math.Cos(m_LightRotationAngle));
			float ny = -1.0f; // the rotation is along the Y axis and lit downward
			// update light settings
			m_Device.Lights[0].Direction = new Vector3(nx, ny, nz);
			m_Device.Lights[0].Update();

			// updates the camera
			m_Camera.Update();
		}

		/// <summary>
		/// Render the scene elements.
		/// </summary>
		/// <param name="device">DirectX device instance.</param>
		public override void RenderScene(Device device)
		{
			if (m_SceneGraphForm == null)
				return;

			// clears the render
			device.Clear(ClearFlags.Target | ClearFlags.ZBuffer, m_ClearColor, 1.0f, 0);

			// draw the grid or not according to the options settings
			if (m_SceneGraphForm.chkShowGrid.Checked)
				m_Grid.Render();

			// ensure lighting is on
			device.RenderState.Lighting = true;

			// render the scene root node, it will itself render its subnodes, and so on...
			if (m_RootSceneNode != null)
				m_RootSceneNode.Render();

			// stores the current world matrix
			Matrix mtx = m_Device.Transform.World;
			
			Matrix axisScale;
			// check the options for rendering current coordinate system axis
			if (m_SceneGraphForm.chkShowCurrentTransformAxis.Checked)
			{
				// extract scale from the transform matrix to apply back its inverse
				// in order to keep the transform the same but to cancel the scaling
				axisScale = Matrix.Scaling(ExtractScaleInverse(m_SelectedNodeCurrentMatrix));
				m_Device.Transform.World = axisScale * m_SelectedNodeCurrentMatrix;
				// draw axis
				m_Axis.Draw(false);
			}

			// check the options for rendering local coordinate system axis
			if (m_SceneGraphForm.chkShowParentTransformAxis.Checked)
			{
				// extract scale from the transform matrix to apply back its inverse
				// in order to keep the transform the same but to cancel the scaling
				axisScale = Matrix.Scaling(ExtractScaleInverse(m_SelectedNodeMatrix));
				m_Device.Transform.World = axisScale * m_SelectedNodeMatrix;
				// draw axis
				m_Axis.Draw(true);
			}

			// restores the world matrix
			m_Device.Transform.World = mtx;
		}

		/// <summary>
		/// This function retrieves the current scaling applied to the given Matrix,
		/// and return a Vector3 containing the inverse scaling value.
		/// </summary>
		/// <param name="mtx">Matrix representing a 3D transform.</param>
		/// <returns>Returns a Vector3 containing the inverse scaling values of the Matrix scaling.</returns>
		private Vector3 ExtractScaleInverse(Matrix mtx)
		{
			// extract Matrix scaling values
			float scaleX = new Vector3(mtx.M11, mtx.M12, mtx.M13).Length();
			float scaleY = new Vector3(mtx.M21, mtx.M22, mtx.M23).Length();
			float scaleZ = new Vector3(mtx.M31, mtx.M32, mtx.M33).Length();

			// values smaller or equal this this threshold value are considered as 0
			const float threshold = 0.000001f;

			// ensure the scaleX, scaleY or scaleZ values can be used as a divider
			if (scaleX >= -threshold && scaleX <= threshold)
				scaleX = 1.0f;
			else
				scaleX = 1.0f / scaleX;

			if (scaleY >= -threshold && scaleY <= threshold)
				scaleY = 1.0f;
			else
				scaleY = 1.0f / scaleY;
			if (scaleZ >= -threshold && scaleZ <= threshold)
				scaleZ = 1.0f;
			else
				scaleZ = 1.0f / scaleZ;

			// inverse scale will be applied as a scaling Matrix

			return (new Vector3(scaleX, scaleY, scaleZ));
		}
	}
}
