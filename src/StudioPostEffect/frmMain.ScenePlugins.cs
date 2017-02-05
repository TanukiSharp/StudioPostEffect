using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using IScenePlugin;
using System.IO;
using System.Reflection;
using Microsoft.DirectX;

namespace StudioPostEffect
{
	partial class frmMain
	{
		private List<ToolStripMenuItem> m_ScenePluginsMenus = new List<ToolStripMenuItem>();
		private ToolStripMenuItem m_ScenePluginConfigurationMenu;

		private void LoadPlugins()
		{
			string pluginPath = string.Format("{0}\\plugins", Application.StartupPath);

			string[] dllFiles = Directory.GetFiles(pluginPath, "*.dll", SearchOption.AllDirectories);
			foreach (string dllFile in dllFiles)
			{
				string newDllFile = Path.ChangeExtension(dllFile, ".loaded");
				try
				{
					File.Copy(dllFile, newDllFile, true);
					Assembly ass = Assembly.LoadFile(newDllFile);
					Type[] types = ass.GetTypes();

					if (types.Length == 0)
						File.Delete(newDllFile);

					foreach (Type type in types)
					{
						if (type.IsSubclassOf(typeof(Scene)))
						{
							Scene plugin = (Scene)Activator.CreateInstance(type);
							string name = plugin.Name;

							if (m_ScenePluginsMenus.Exists(delegate(ToolStripMenuItem m) { return (((Scene)m.Tag).Name.ToLower() == name.ToLower()); }) == false)
							{
								ToolStripMenuItem menu = new ToolStripMenuItem(name);
								menu.Click += new EventHandler(OnScenePluginMenuClick);
								menu.Tag = plugin;

								mnuPlugins.DropDownItems.Add(menu);
								m_ScenePluginsMenus.Add(menu);
							}
						}
					}
				}
				catch
				{
					if (File.Exists(newDllFile))
						File.Delete(newDllFile);
				}
			}

			if (m_ScenePluginsMenus.Count == 0)
			{
				mnuPlugins.Enabled = false;
				return;
			}

			mnuPlugins.DropDownItems.Add(new ToolStripSeparator());

			m_ScenePluginConfigurationMenu = new ToolStripMenuItem("Configuration...");
			m_ScenePluginConfigurationMenu.Click += new EventHandler(OnScenePluginConfigurationMenuClick);
			mnuPlugins.DropDownItems.Add(m_ScenePluginConfigurationMenu);

			mnuPlugins.DropDownOpening += new EventHandler(OnPluginsMenuClick);
		}

		private void OnPluginsMenuClick(object sender, EventArgs e)
		{
			m_ScenePluginConfigurationMenu.Enabled = false;
			foreach (ToolStripMenuItem menu in m_ScenePluginsMenus)
			{
				if (menu.Checked)
				{
					m_ScenePluginConfigurationMenu.Enabled = true;
					break;
				}
			}
		}

		private void OnScenePluginMenuClick(object sender, EventArgs e)
		{
			if (sender == null)
				return;

			ToolStripMenuItem menu = (ToolStripMenuItem)sender;

			if (SetScenePlugin((Scene)menu.Tag))
			{
				foreach (ToolStripMenuItem submenu in m_ScenePluginsMenus)
					submenu.Checked = false;

				m_ScenePluginConfigurationMenu.Tag = menu.Tag;
				menu.Checked = true;
			}
		}

		private void OnScenePluginConfigurationMenuClick(object sender, EventArgs e)
		{
			ToolStripMenuItem menu = (ToolStripMenuItem)sender;
			Scene plugin = (Scene)menu.Tag;
			plugin.ConfigPanelOpen();
		}

		private bool SetScenePlugin(Scene scene)
		{
			if (scene == null)
				return (false);

			if (m_Scene != scene)
			{
				try
				{
					if (m_Scene != null)
						m_Scene.Terminate();

					m_ScenePluginInitParams = new ScenePluginInitParams();
					SetDefaultScenePluginInitParams(m_ScenePluginInitParams);

					m_ViewportDX.Device.Transform.World = Matrix.Identity;
					m_ViewportDX.Device.Transform.View = Matrix.Identity;

					scene.Initialize(m_ScenePluginInitParams);
					scene.OnDeviceReset(m_ViewportDX.Device);

					m_Scene = scene;

					m_ViewportDX.SetAutoRender(true);
					if (m_ScenePluginInitParams.CustomRenderTiming.NeedCustomRenderTiming)
						m_ViewportDX.SetAutoRender(false);

					return (true);
				}
				catch (Exception ex)
				{
					SetDefaultScenePluginInitParams(m_ScenePluginInitParams);

					string msg = string.Format("The scene plugin '{0}' has generated the following exception:\r\n\r\n{1}", scene.Name, ex.Message);
					MessageBox.Show(msg, "Scene Plugin Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				}
			}
			return (false);
		}

		private void SetDefaultScenePluginInitParams(ScenePluginInitParams prms)
		{
			prms.Device = m_ViewportDX.Device;
			prms.Camera = m_Camera;
			prms.CustomRenderTiming = new CustomRender(OnScenePluginRequestRender);
			prms.CustomRenderTiming.NeedCustomRenderTiming = false;
			prms.Grid = m_Grid;
		}

		private void OnScenePluginRequestRender()
		{
			if (m_ScenePluginInitParams.CustomRenderTiming.NeedCustomRenderTiming)
				m_ViewportDX.PerformRender();
		}
	}
}
