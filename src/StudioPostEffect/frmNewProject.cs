using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Xml;
using PostEffectCore;

namespace StudioPostEffect
{
	public partial class frmNewProject : Form
	{
		private string m_ProjectFullFilename;

		public frmNewProject()
		{
			InitializeComponent();
			DialogResult = DialogResult.Cancel;
		}

		public string ProjectFullFilename
		{
			get
			{
				return (m_ProjectFullFilename);
			}
		}

		private void btnBrowse_Click(object sender, EventArgs e)
		{
			FolderBrowserDialog fbd = new FolderBrowserDialog();
			fbd.Description = "Select a directory to store your project";
			fbd.ShowNewFolderButton = true;
			if (fbd.ShowDialog() != DialogResult.OK)
				return;

			txtLocation.Text = fbd.SelectedPath;
		}

		private void btnOK_Click(object sender, EventArgs e)
		{
			string name = txtName.Text.Trim();
			string location = txtLocation.Text.Trim();

			if (name.Length == 0)
			{
				MessageBox.Show("You must enter a name for your project.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}

			if (location.Length == 0)
			{
				MessageBox.Show("You must specify a location for your project.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}

			if (Directory.Exists(location) == false)
			{
				DialogResult res = MessageBox.Show(string.Format("The directory '{0}' does not exist.\r\nDo you want to create it now ?", location), "Create Directory", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
				if (res == DialogResult.No)
					return;
			}

			if (chkCreateProjectDirectory.Checked)
			{
				location = string.Format("{0}\\{1}", location, name);
				if (Directory.Exists(location))
				{
					DialogResult res = MessageBox.Show(string.Format("The directory '{0}' already exist.\r\nDo you want to proceed ?", location), "Use Directory", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
					if (res == DialogResult.No)
						return;
				}
			}

			if (IOHelper.TryCreateDirectory(location) == false)
			{
				MessageBox.Show(string.Format("Impossible to create directory '{0}'.", location), "Path Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}

			string fullFilename = string.Format("{0}\\{1}.efxprj", location, name);

			if (File.Exists(fullFilename))
			{
				DialogResult res = MessageBox.Show(string.Format("The project file '{0}' already exist. Do you want to overwrite it ?", fullFilename), "Overwrite File ?", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
				if (res == DialogResult.No)
					return;
				File.Delete(fullFilename);
			}

			XmlTextWriter xw = null;
			try
			{
				xw = new XmlTextWriter(fullFilename, Encoding.UTF8);
				xw.Formatting = Formatting.Indented;
				xw.IndentChar = '\t';
				xw.Indentation = 1;

				xw.WriteStartDocument();
				xw.WriteStartElement("efxprj");
				xw.WriteEndElement();
				xw.WriteEndDocument();

				xw.Close();

				m_ProjectFullFilename = fullFilename;
				DialogResult = DialogResult.OK;
				Close();
			}
			catch
			{
				if (xw != null)
				{
					xw.Close();
					File.Delete(fullFilename);
				}
				MessageBox.Show(string.Format("Impossible to create file '{0}'.", fullFilename), "File Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}
		}

		private void btnCancel_Click(object sender, EventArgs e)
		{
			Close();
		}
	}
}
