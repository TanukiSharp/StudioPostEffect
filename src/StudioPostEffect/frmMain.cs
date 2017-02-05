using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Microsoft.DirectX.Direct3D;
using ViewportDXControl;
using Microsoft.DirectX;
using IScenePlugin;
using PostEffectCore;
using InternalEffect;
using System.IO;
using System.Reflection;
using System.Diagnostics;

namespace StudioPostEffect
{
	public partial class frmMain : Form
	{
		private ViewportDX m_ViewportDX;
		private Scene m_Scene = null;
		private ScenePluginInitParams m_ScenePluginInitParams = null;

		private Camera m_Camera;
		private Grid m_Grid;

		private EffectWorkflowManager m_WorkflowManager;

		public frmMain()
		{
			InitializeComponent();

			GlobalContainer.MainForm = this;
			GlobalContainer.CompositionsTabControl = tabWorkflowManagers;
			GlobalContainer.UIParametersPropertyGrid = grdUIParameters;
			GlobalContainer.SplitContainerWorkflowParams = splitContainer3;
			GlobalContainer.ProjectImageList = imglstProjectTreeView;

			GlobalContainer.SliderPanel = pnlParameterSlider;

			InitMainForm();
			InitViewportDX();

			LoadPlugins();

			RunPluginByTypeName("DefaultScene");
		}

		private bool RunPluginByTypeName(string pluginTypeName)
		{
			if (m_ScenePluginsMenus.Count == 0)
				return (false);

			ToolStripMenuItem pluginMenu = m_ScenePluginsMenus.Find(delegate(ToolStripMenuItem m) { return (m.Tag.GetType().Name == pluginTypeName); });
			if (pluginTypeName == null)
				return (false);

			OnScenePluginMenuClick(pluginMenu, null);
			return (true);
		}

		private void InitMainForm()
		{
			this.Text = string.Format("Studio Post-Effect {0}", IOHelper.GetProductVersion());
			tlbToolBar.BringToFront();

			trvProject.MouseDown += new MouseEventHandler(OnProjectTreeViewMouseDown);
		}

		private Point m_MenuPosition;
		private void OnProjectTreeViewMouseDown(object sender, MouseEventArgs e)
		{
			m_MenuPosition = e.Location;

			BaseTreeNode tn = trvProject.GetNodeAt(e.Location) as BaseTreeNode;
			if (tn == null)
				return;

			trvProject.SelectedNode = tn;

			if (e.Button == MouseButtons.Left)
			{
				if (tn is PassTreeNode || tn is TextureTreeNode)
					DoDragDrop(tn, DragDropEffects.Copy);
			}
		}

		private void InitViewportDX()
		{
			m_ViewportDX = viewportDX;

			m_ViewportDX.DeviceInitialized += new ViewportDX.VDXEventHandler(OnDeviceInitialized);
			m_ViewportDX.DeviceInitializing += new ViewportDX.VDXEventHandler<DeviceCreationArgs>(OnDeviceInitializing);
			m_ViewportDX.DeviceReset += new ViewportDX.VDXEventHandler(OnDeviceReset);

			m_ViewportDX.UpdateScene += new ViewportDX.VDXEventHandler(OnUpdateScene);
			m_ViewportDX.RenderScene += new ViewportDX.VDXEventHandler(OnRenderScene);

			m_ViewportDX.MouseDown += new MouseEventHandler(OnViewportDXMouseDown);
			m_ViewportDX.MouseUp += new MouseEventHandler(OnViewportDXMouseUp);
			m_ViewportDX.MouseWheel += new MouseEventHandler(OnViewportDXMouseWheel);
			m_ViewportDX.MouseMove += new MouseEventHandler(OnViewportDXMouseMove);

			m_ViewportDX.DragEnter += new DragEventHandler(OnViewportDXDragEnter);
			m_ViewportDX.DragDrop += new DragEventHandler(OnViewportDXDragDrop);
			m_ViewportDX.AllowDrop = true;

			m_ViewportDX.Init();
			GlobalContainer.ViewportDX = m_ViewportDX;
		}

		private void OnViewportDXDragEnter(object sender, DragEventArgs e)
		{
			e.Effect = DragDropEffects.None;
			if (e.Data.GetDataPresent(DataFormats.FileDrop))
				e.Effect = DragDropEffects.Copy;
		}

		private void OnViewportDXDragDrop(object sender, DragEventArgs e)
		{
			if (m_Scene != null)
			{
				string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
				m_Scene.OnFilesDropped(e.X, e.Y, files);
			}
		}

		private void OnDeviceInitializing(ViewportDX vp, DeviceCreationArgs args)
		{
			args.Parameters.PresentationInterval = PresentInterval.Default;
			args.Parameters.BackBufferFormat = Format.A8R8G8B8;
		}

		private void OnDeviceReset(ViewportDX vp)
		{
			vp.Device.RenderState.Ambient = Color.Gray;

			if (m_Scene != null)
				m_Scene.OnDeviceReset(vp.Device);
		}

		private void OnViewportDXMouseDown(object sender, MouseEventArgs e)
		{
			if (m_Scene != null)
			{
				m_Camera.SetMouseDownLocation(e.X, e.Y);
				m_Scene.OnMouseDown(e);
			}
		}

		private void OnViewportDXMouseUp(object sender, MouseEventArgs e)
		{
			if (m_Scene != null)
				m_Scene.OnMouseUp(e);
		}

		private void OnViewportDXMouseWheel(object sender, MouseEventArgs e)
		{
			if (m_Scene != null)
				m_Scene.OnMouseWheel(e);
		}

		private void OnViewportDXMouseMove(object sender, MouseEventArgs e)
		{
			if (m_Scene != null)
				m_Scene.OnMouseMove(e);
		}

		private void OnDeviceInitialized(ViewportDX vp)
		{
			m_Square = new Square(vp.Device);
			m_InputFrame = new RenderTexture(vp.Device, RenderTexture.SizeMode.Normal, true);
			m_EffectFrame = new RenderTexture(vp.Device, RenderTexture.SizeMode.Normal);
			m_PreviousFrame = new RenderTexture(vp.Device, RenderTexture.SizeMode.Normal);

			m_Grid = new Grid(vp.Device, new Size(150, 150), Color.Gray);
			m_Camera = new Camera(vp.Device, new Vector3(0.0f, 50.0f, 35.0f), new Vector3(), new Vector3(0.0f, 1.0f, 0.0f));

			m_PostEffectData = new PostEffectData(vp.Device, new RenderTexture(vp.Device, RenderTexture.SizeMode.Normal));
		}

		private void OnUpdateScene(ViewportDX vp)
		{
			lblToolBarFPS.Text = string.Format("{0} FPS", vp.FPS);

			Project project = GetProject();
			if (project != null)
			{
				foreach (BaseElementTreeNode basetn in project.EffectsTreeNode.Nodes)
				{
					CustomEffect customEfx = null;
					if (basetn is PassTreeNode)
						customEfx = ((PassTreeNode)basetn).Pass.ParentTechnique.ParentEffect;
					else if (basetn is TechniqueTreeNode)
						customEfx = ((TechniqueTreeNode)basetn).Technique.ParentEffect;

					if (customEfx != null && customEfx.CheckEffect())
					{
						if (m_WorkflowManager != null)
						{
							foreach (EffectWorkflowItem item in m_WorkflowManager.EffectWorkflowItems)
								item.CheckUnderlyingEffect();
						}
					}
				}
			}

			if (m_WorkflowManager != null)
				m_WorkflowManager.OnUpdateScene();

			if (m_Scene != null)
				m_Scene.UpdateScene(vp.ElapsedTime);

			//UpdateEffects();
		}

		private RenderTexture m_InputFrame;
		private RenderTexture m_EffectFrame;
		private RenderTexture m_PreviousFrame = null;
		private Square m_Square;

		private Texture m_PreviousTexture = null;

		private PostEffectData m_PostEffectData;

		private void OnRenderScene(ViewportDX vp)
		{
			if (m_Scene == null)
				return;

			m_InputFrame.BeginRender();
			// render plugin scene
			m_Scene.RenderScene(vp.Device);
			Texture inputTexture = m_InputFrame.EndRender();

			m_PostEffectData.SceneFrame = inputTexture;
			m_PostEffectData.PreviousFrame = m_PreviousTexture;

			//Debug.WorkflowSequence.Clear();

			Texture output = ProcessEffect(m_PostEffectData);
			if (output != null)
				inputTexture = output;

			//textBox1.Text = string.Join("\r\n", Debug.WorkflowSequence.ToArray());

			m_PreviousFrame.BeginRender();
			vp.Device.SetTexture(0, inputTexture);
			m_Square.Draw();
			m_PreviousTexture = m_PreviousFrame.EndRender();

			vp.Device.SetTexture(0, m_PreviousTexture);
			m_Square.Draw();
		}

		public Texture ProcessEffect(PostEffectData data)
		{
			if (m_WorkflowManager == null)
				return (data.SceneFrame);

			LinkBound result = m_WorkflowManager.Result;
			if (result == null)
				return (data.SceneFrame);

			if (result.OtherBound == null)
				return (data.SceneFrame);

			PrimitiveItemType type = result.OtherBound.AttachedObject.Type;
			if (type != PrimitiveItemType.Effect)
			{
				if (type == PrimitiveItemType.Scene)
					return (data.SceneFrame);
				else if (type == PrimitiveItemType.PreviousFrame)
					return (data.PreviousFrame);
			}

			Texture resultTexture = ((EffectWorkflowItem)result.OtherBound.AttachedObject).ProcessEffect(data);

			//Debug.WorkflowSequence.Add(string.Format("{0} -> {1}", result.OtherBound.Name, result.Name));

			return (resultTexture);
		}


		private void splitContainer_SplitterMoved(object sender, SplitterEventArgs e)
		{
			trvProject.Focus();
		}

		private void tabWorkflowManagers_Selected(object sender, TabControlEventArgs e)
		{
			if (m_WorkflowManager != null)
				m_WorkflowManager.UnselectAllWorkflowItems();

			if (e.TabPage == null)
			{
				m_WorkflowManager = null;
				return;
			}

			m_WorkflowManager = (EffectWorkflowManager)e.TabPage.Controls["effectWorkflowManager"];
			trvProject.SelectedNode = (CompositionTreeNode)e.TabPage.Tag;
		}

		private void tabWorkflowManagers_ControlAdded(object sender, ControlEventArgs e)
		{
			TabPage tab = (TabPage)e.Control;

			if (tab == null)
			{
				m_WorkflowManager = null;
				return;
			}
			m_WorkflowManager = (EffectWorkflowManager)tab.Controls["effectWorkflowManager"];
			trvProject.SelectedNode = (CompositionTreeNode)tab.Tag;
		}

		private void mnuHelpAbout_Click(object sender, EventArgs e)
		{
			frmAbout about = new frmAbout();
			about.Show(this);
		}

		private void mnuHelpHome_Click(object sender, EventArgs e)
		{
			Process.Start("http://sites.google.com/site/studioposteffect");
		}

		private void trvProject_AfterLabelEdit(object sender, NodeLabelEditEventArgs e)
		{
			if (e.Node == null || e.Label == null)
				return;

			if (e.Node is CompositionTreeNode)
			{
				CompositionTreeNode ctn = (CompositionTreeNode)e.Node;

				if (e.Label.ToLower() == ctn.Text.ToLower())
					return;

				foreach (TreeNode tn in ctn.Parent.Nodes)
				{
					if (tn.Text.ToLower() == e.Label.ToLower())
					{
						MessageBox.Show(string.Format("There is already a Composition named '{0}'.\r\nPlease chose a different name.", e.Label), "Name already exists", MessageBoxButtons.OK, MessageBoxIcon.Error);
						e.CancelEdit = true;
						return;
					}
				}
				Project project = GetProject();
				if (project != null)
					project.IsModified = true;
				ctn.Name = e.Label;
			}
		}

		private void trvProject_BeforeLabelEdit(object sender, NodeLabelEditEventArgs e)
		{
			if (!(e.Node is CompositionTreeNode))
				e.CancelEdit = true;
		}

		private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (CloseProject() == false)
				e.Cancel = true;
		}

		private void grdUIParameters_SelectedGridItemChanged(object sender, SelectedGridItemChangedEventArgs e)
		{
			Panel pnlSliders = GlobalContainer.SliderPanel;

			pnlSliders.SuspendLayout();

			pnlSliders.Controls.Clear();
			if (e.NewSelection.Value is FloatSlider)
			{
				FloatSlider fsl = (FloatSlider)e.NewSelection.Value;
				pnlSliders.Controls.Add(fsl.InternalControl);
				fsl.InternalControl.Dock = DockStyle.Fill;
			}
			else if (e.NewSelection.Value is IntegerSlider)
			{
				IntegerSlider isl = (IntegerSlider)e.NewSelection.Value;
				pnlSliders.Controls.Add(isl.InternalControl);
				isl.InternalControl.Dock = DockStyle.Fill;
			}

			pnlSliders.ResumeLayout();
		}
	}
}
