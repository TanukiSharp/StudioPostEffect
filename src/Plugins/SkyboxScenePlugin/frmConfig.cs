using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace SkyboxScenePlugin
{
	public partial class frmConfig : Form
	{
		private SkyboxScene m_Plugin;
		private string[] m_SkyboxDefinitionFiles;

		public frmConfig(SkyboxScene plugin, string[] xmlFiles)
		{
			InitializeComponent();
			m_Plugin = plugin;
			m_SkyboxDefinitionFiles = xmlFiles;
			foreach (string xmlFile in xmlFiles)
				cboSkyboxes.Items.Add(Path.GetFileNameWithoutExtension(xmlFile));
			cboSkyboxes.SelectedIndex = plugin.GetSkybox();
		}

		private void btnLoadSkyBox_Click(object sender, EventArgs e)
		{
			m_Plugin.SetSkybox(cboSkyboxes.SelectedIndex);
		}

		private void btnClose_Click(object sender, EventArgs e)
		{
			Close();
		}
	}
}
