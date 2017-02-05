using System;
using System.Collections.Generic;
using System.Text;
using InternalEffect;
using System.Windows.Forms;
using System.IO;
using PostEffectCore;

namespace StudioPostEffect
{
	partial class frmMain
	{
		private void CleanProjectControls()
		{
			this.SuspendLayout();

			tabWorkflowManagers.TabPages.Clear();
			trvProject.Nodes.Clear();
			GlobalContainer.Project = null;
			SetWindowTitle();

			GC.Collect();

			this.ResumeLayout();
		}

		public void ApplyOptionsFromProjectToControls()
		{
			Project prj = GetProject();
			if (prj == null)
				return;

		}

		private bool LoadProjectControls(Project project)
		{
			splitContainer1.SuspendLayout();
			Win32.SuspendDrawing(splitContainer1);

			trvProject.Nodes.Add(project);
			project.Initialize();
			project.ExpandAll();

			ApplyOptionsFromProjectToControls();
			SetWindowTitle();

			Win32.ResumeDrawing(splitContainer1);
			splitContainer1.ResumeLayout();

			project.IsModified = false;

			return (true);
		}

		public Project GetProject()
		{
			if (trvProject.Nodes.Count == 0)
				return (null);
			Project prj = trvProject.Nodes[0] as Project;
			return (prj);
		}




		private bool TryCloseProject()
		{
			Project prj = GetProject();

			if (prj == null || prj.IsModified == false)
				return (true);

			DialogResult res = MessageBox.Show("The current project has changed since last time you saved.\r\nDo you want to save it now ?", "Save Project ?", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
			if (res == DialogResult.Yes)
				return (SaveProject());
			if (res == DialogResult.Cancel)
				return (false);

			return (true);
		}

		private bool CloseProject()
		{
			bool closeStatus = TryCloseProject();

			if (closeStatus)
				CleanProjectControls();

			return (closeStatus);
		}

		private bool NewProject()
		{
			if (TryCloseProject() == false)
				return (false);

			frmNewProject newProj = new frmNewProject();
			if (newProj.ShowDialog() != DialogResult.OK)
				return (false);

			CleanProjectControls();

			try
			{
				Project prj = Project.CreateWithNewFile(m_ViewportDX.Device, newProj.ProjectFullFilename);
				GlobalContainer.Project = prj;
				prj.ProjectModified += new Project.ProjectModifiedHandler(OnProjectModified);
				return (LoadProjectControls(prj));
			}
			catch (Exception ex)
			{
				string msg = string.Format("An error occured while loading the project.\r\n\r\n{0}", ex.Message);
				MessageBox.Show(msg, "Load Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return (false);
			}
		}

		private bool OpenProject()
		{
			bool newStatus = TryCloseProject();
			if (newStatus == false)
				return (false);

			OpenFileDialog ofd = new OpenFileDialog();
			ofd.Title = "Open a Studio Post Effect Project file";
			ofd.Multiselect = false;
			ofd.CheckFileExists = true;
			ofd.Filter = "Studio Post Effect Project Files (*.efxprj)|*.efxprj|All Files (*.*)|*.*";
			if (ofd.ShowDialog() != DialogResult.OK)
				return (false);

			CleanProjectControls();

			Project prj = null;
			try
			{
				prj = Project.CreateFromXmlProjectFile(m_ViewportDX.Device, ofd.FileName);
				GlobalContainer.Project = prj;
				prj.ProjectModified += new Project.ProjectModifiedHandler(OnProjectModified);
				return (LoadProjectControls(prj));
			}
			catch (Exception ex)
			{
				CleanProjectControls();
				MessageBox.Show(string.Format("An error occured while trying to open project file.\r\n\r\n{0}", ex.Message), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return (false);
			}
		}

		private void OnProjectModified(Project sender)
		{
			SetWindowTitle();
		}

		private void SetWindowTitle()
		{
			Project prj = GetProject();

			if (prj == null)
				this.Text = "Studio Post Effect";
			else
				this.Text = string.Format("Studio Post Effect - {0}{1}", prj.ProjectFilename, prj.IsModified ? " *" : "");
		}

		private void ErrorNoProjectCreated()
		{
			MessageBox.Show("No Project created!", "Unexpected Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
		}

		private bool SaveProject()
		{
			Project prj = GetProject();

			if (prj == null)
			{
				ErrorNoProjectCreated();
				return (true);
			}

			if (prj.IsModified == false)
				return (true);

			if (prj.ProjectFilename == null)
				return (SaveProjectAs());

			return (prj.Save());
		}

		private bool SaveProjectAs()
		{
			Project prj = GetProject();

			if (prj == null)
			{
				ErrorNoProjectCreated();
				return (true);
			}

			//if (prj.IsModified == false)
			//	return (true);

			SaveFileDialog sfd = new SaveFileDialog();
			sfd.Title = "Save you Studio Post Effect Project";
			sfd.CheckPathExists = true;
			sfd.Filter = "Studio Post Effect Project Files (*.efxprj)|*.efxprj|All Files (*.*)|*.*";
			if (sfd.ShowDialog() != DialogResult.OK)
				return (false);

			return (prj.SaveAs(sfd.FileName));
		}

		//----------------------------------------------------------------------------------------------

		private void mnuFileClose_Click(object sender, EventArgs e)
		{
			Close();
		}

		private void mnuFileNewProject_Click(object sender, EventArgs e)
		{
			NewProject();
		}

		private void mnuFileOpenProject_Click(object sender, EventArgs e)
		{
			OpenProject();
		}

		private void mnuFileSaveProject_Click(object sender, EventArgs e)
		{
			SaveProject();
		}

		private void mnuFileSaveProjectAs_Click(object sender, EventArgs e)
		{
			SaveProjectAs();
		}

		private void mnuCloseProject_Click(object sender, EventArgs e)
		{
			CloseProject();
		}

		private void btnToolbarNewProject_Click(object sender, EventArgs e)
		{
			NewProject();
		}

		private void btnToolbarOpenProject_Click(object sender, EventArgs e)
		{
			OpenProject();
		}

		private void btnToolbarSaveProject_Click(object sender, EventArgs e)
		{
			SaveProject();
		}
	}
}
