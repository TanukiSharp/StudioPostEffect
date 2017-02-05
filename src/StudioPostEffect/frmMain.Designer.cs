namespace StudioPostEffect
{
	partial class frmMain
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
			this.mnuMain = new System.Windows.Forms.MenuStrip();
			this.mnuFile = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuFileNewProject = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuFileOpenProject = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuFileSaveProject = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuFileSaveProjectAs = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuCloseProject = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
			this.mnuFileClose = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuPlugins = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuHelp = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuHelpHome = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
			this.mnuHelpAbout = new System.Windows.Forms.ToolStripMenuItem();
			this.tlbToolBar = new System.Windows.Forms.ToolStrip();
			this.btnToolbarNewProject = new System.Windows.Forms.ToolStripButton();
			this.btnToolbarOpenProject = new System.Windows.Forms.ToolStripButton();
			this.btnToolbarSaveProject = new System.Windows.Forms.ToolStripButton();
			this.lblToolBarFPS = new System.Windows.Forms.ToolStripLabel();
			this.viewportDX = new ViewportDXControl.ViewportDX();
			this.trvProject = new System.Windows.Forms.TreeView();
			this.imglstProjectTreeView = new System.Windows.Forms.ImageList(this.components);
			this.splitContainer1 = new System.Windows.Forms.SplitContainer();
			this.splitContainer2 = new System.Windows.Forms.SplitContainer();
			this.splitContainer3 = new System.Windows.Forms.SplitContainer();
			this.tabWorkflowManagers = new System.Windows.Forms.TabControl();
			this.pnlParameterSlider = new System.Windows.Forms.Panel();
			this.grdUIParameters = new System.Windows.Forms.PropertyGrid();
			this.mnuMain.SuspendLayout();
			this.tlbToolBar.SuspendLayout();
			this.splitContainer1.Panel1.SuspendLayout();
			this.splitContainer1.Panel2.SuspendLayout();
			this.splitContainer1.SuspendLayout();
			this.splitContainer2.Panel1.SuspendLayout();
			this.splitContainer2.Panel2.SuspendLayout();
			this.splitContainer2.SuspendLayout();
			this.splitContainer3.Panel1.SuspendLayout();
			this.splitContainer3.Panel2.SuspendLayout();
			this.splitContainer3.SuspendLayout();
			this.SuspendLayout();
			// 
			// mnuMain
			// 
			this.mnuMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuFile,
            this.mnuPlugins,
            this.mnuHelp});
			this.mnuMain.Location = new System.Drawing.Point(0, 0);
			this.mnuMain.Name = "mnuMain";
			this.mnuMain.Size = new System.Drawing.Size(1098, 24);
			this.mnuMain.TabIndex = 4;
			// 
			// mnuFile
			// 
			this.mnuFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuFileNewProject,
            this.mnuFileOpenProject,
            this.mnuFileSaveProject,
            this.mnuFileSaveProjectAs,
            this.mnuCloseProject,
            this.toolStripMenuItem1,
            this.mnuFileClose});
			this.mnuFile.Name = "mnuFile";
			this.mnuFile.Size = new System.Drawing.Size(36, 20);
			this.mnuFile.Text = "File";
			// 
			// mnuFileNewProject
			// 
			this.mnuFileNewProject.Image = ((System.Drawing.Image)(resources.GetObject("mnuFileNewProject.Image")));
			this.mnuFileNewProject.Name = "mnuFileNewProject";
			this.mnuFileNewProject.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.N)));
			this.mnuFileNewProject.Size = new System.Drawing.Size(180, 22);
			this.mnuFileNewProject.Text = "New Projecst";
			this.mnuFileNewProject.Click += new System.EventHandler(this.mnuFileNewProject_Click);
			// 
			// mnuFileOpenProject
			// 
			this.mnuFileOpenProject.Image = ((System.Drawing.Image)(resources.GetObject("mnuFileOpenProject.Image")));
			this.mnuFileOpenProject.Name = "mnuFileOpenProject";
			this.mnuFileOpenProject.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
			this.mnuFileOpenProject.Size = new System.Drawing.Size(180, 22);
			this.mnuFileOpenProject.Text = "Open Project...";
			this.mnuFileOpenProject.Click += new System.EventHandler(this.mnuFileOpenProject_Click);
			// 
			// mnuFileSaveProject
			// 
			this.mnuFileSaveProject.Image = ((System.Drawing.Image)(resources.GetObject("mnuFileSaveProject.Image")));
			this.mnuFileSaveProject.Name = "mnuFileSaveProject";
			this.mnuFileSaveProject.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
			this.mnuFileSaveProject.Size = new System.Drawing.Size(180, 22);
			this.mnuFileSaveProject.Text = "Save Project";
			this.mnuFileSaveProject.Click += new System.EventHandler(this.mnuFileSaveProject_Click);
			// 
			// mnuFileSaveProjectAs
			// 
			this.mnuFileSaveProjectAs.Name = "mnuFileSaveProjectAs";
			this.mnuFileSaveProjectAs.Size = new System.Drawing.Size(180, 22);
			this.mnuFileSaveProjectAs.Text = "Save Project As...";
			this.mnuFileSaveProjectAs.Click += new System.EventHandler(this.mnuFileSaveProjectAs_Click);
			// 
			// mnuCloseProject
			// 
			this.mnuCloseProject.Name = "mnuCloseProject";
			this.mnuCloseProject.Size = new System.Drawing.Size(180, 22);
			this.mnuCloseProject.Text = "Close Project";
			this.mnuCloseProject.Click += new System.EventHandler(this.mnuCloseProject_Click);
			// 
			// toolStripMenuItem1
			// 
			this.toolStripMenuItem1.Name = "toolStripMenuItem1";
			this.toolStripMenuItem1.Size = new System.Drawing.Size(177, 6);
			// 
			// mnuFileClose
			// 
			this.mnuFileClose.Name = "mnuFileClose";
			this.mnuFileClose.Size = new System.Drawing.Size(180, 22);
			this.mnuFileClose.Text = "Close";
			this.mnuFileClose.Click += new System.EventHandler(this.mnuFileClose_Click);
			// 
			// mnuPlugins
			// 
			this.mnuPlugins.Name = "mnuPlugins";
			this.mnuPlugins.Size = new System.Drawing.Size(54, 20);
			this.mnuPlugins.Text = "Plugins";
			// 
			// mnuHelp
			// 
			this.mnuHelp.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuHelpHome,
            this.toolStripMenuItem2,
            this.mnuHelpAbout});
			this.mnuHelp.Name = "mnuHelp";
			this.mnuHelp.Size = new System.Drawing.Size(22, 20);
			this.mnuHelp.Text = "?";
			// 
			// mnuHelpHome
			// 
			this.mnuHelpHome.Image = ((System.Drawing.Image)(resources.GetObject("mnuHelpHome.Image")));
			this.mnuHelpHome.Name = "mnuHelpHome";
			this.mnuHelpHome.Size = new System.Drawing.Size(106, 22);
			this.mnuHelpHome.Text = "Home...";
			this.mnuHelpHome.ToolTipText = "http://sites.google.com/site/studioposteffect/";
			this.mnuHelpHome.Click += new System.EventHandler(this.mnuHelpHome_Click);
			// 
			// toolStripMenuItem2
			// 
			this.toolStripMenuItem2.Name = "toolStripMenuItem2";
			this.toolStripMenuItem2.Size = new System.Drawing.Size(103, 6);
			// 
			// mnuHelpAbout
			// 
			this.mnuHelpAbout.Name = "mnuHelpAbout";
			this.mnuHelpAbout.Size = new System.Drawing.Size(106, 22);
			this.mnuHelpAbout.Text = "About...";
			this.mnuHelpAbout.Click += new System.EventHandler(this.mnuHelpAbout_Click);
			// 
			// tlbToolBar
			// 
			this.tlbToolBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.tlbToolBar.AutoSize = false;
			this.tlbToolBar.Dock = System.Windows.Forms.DockStyle.None;
			this.tlbToolBar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnToolbarNewProject,
            this.btnToolbarOpenProject,
            this.btnToolbarSaveProject,
            this.lblToolBarFPS});
			this.tlbToolBar.Location = new System.Drawing.Point(0, 24);
			this.tlbToolBar.Name = "tlbToolBar";
			this.tlbToolBar.Size = new System.Drawing.Size(1098, 25);
			this.tlbToolBar.TabIndex = 5;
			// 
			// btnToolbarNewProject
			// 
			this.btnToolbarNewProject.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.btnToolbarNewProject.Image = ((System.Drawing.Image)(resources.GetObject("btnToolbarNewProject.Image")));
			this.btnToolbarNewProject.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.btnToolbarNewProject.Name = "btnToolbarNewProject";
			this.btnToolbarNewProject.Size = new System.Drawing.Size(23, 22);
			this.btnToolbarNewProject.Text = "&New";
			this.btnToolbarNewProject.Click += new System.EventHandler(this.btnToolbarNewProject_Click);
			// 
			// btnToolbarOpenProject
			// 
			this.btnToolbarOpenProject.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.btnToolbarOpenProject.Image = ((System.Drawing.Image)(resources.GetObject("btnToolbarOpenProject.Image")));
			this.btnToolbarOpenProject.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.btnToolbarOpenProject.Name = "btnToolbarOpenProject";
			this.btnToolbarOpenProject.Size = new System.Drawing.Size(23, 22);
			this.btnToolbarOpenProject.Text = "&Open";
			this.btnToolbarOpenProject.Click += new System.EventHandler(this.btnToolbarOpenProject_Click);
			// 
			// btnToolbarSaveProject
			// 
			this.btnToolbarSaveProject.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.btnToolbarSaveProject.Image = ((System.Drawing.Image)(resources.GetObject("btnToolbarSaveProject.Image")));
			this.btnToolbarSaveProject.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.btnToolbarSaveProject.Name = "btnToolbarSaveProject";
			this.btnToolbarSaveProject.Size = new System.Drawing.Size(23, 22);
			this.btnToolbarSaveProject.Text = "&Save";
			this.btnToolbarSaveProject.Click += new System.EventHandler(this.btnToolbarSaveProject_Click);
			// 
			// lblToolBarFPS
			// 
			this.lblToolBarFPS.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
			this.lblToolBarFPS.Name = "lblToolBarFPS";
			this.lblToolBarFPS.Size = new System.Drawing.Size(36, 22);
			this.lblToolBarFPS.Text = "0 FPS";
			// 
			// viewportDX
			// 
			this.viewportDX.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(60)))));
			this.viewportDX.Dock = System.Windows.Forms.DockStyle.Fill;
			this.viewportDX.Location = new System.Drawing.Point(0, 0);
			this.viewportDX.Name = "viewportDX";
			this.viewportDX.Size = new System.Drawing.Size(897, 498);
			this.viewportDX.TabIndex = 7;
			// 
			// trvProject
			// 
			this.trvProject.Dock = System.Windows.Forms.DockStyle.Fill;
			this.trvProject.HideSelection = false;
			this.trvProject.ImageIndex = 0;
			this.trvProject.ImageList = this.imglstProjectTreeView;
			this.trvProject.LabelEdit = true;
			this.trvProject.Location = new System.Drawing.Point(0, 0);
			this.trvProject.Name = "trvProject";
			this.trvProject.SelectedImageIndex = 0;
			this.trvProject.Size = new System.Drawing.Size(173, 745);
			this.trvProject.TabIndex = 8;
			this.trvProject.AfterLabelEdit += new System.Windows.Forms.NodeLabelEditEventHandler(this.trvProject_AfterLabelEdit);
			this.trvProject.BeforeLabelEdit += new System.Windows.Forms.NodeLabelEditEventHandler(this.trvProject_BeforeLabelEdit);
			// 
			// imglstProjectTreeView
			// 
			this.imglstProjectTreeView.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imglstProjectTreeView.ImageStream")));
			this.imglstProjectTreeView.TransparentColor = System.Drawing.Color.Transparent;
			this.imglstProjectTreeView.Images.SetKeyName(0, "project.png");
			this.imglstProjectTreeView.Images.SetKeyName(1, "compositions.png");
			this.imglstProjectTreeView.Images.SetKeyName(2, "composition.png");
			this.imglstProjectTreeView.Images.SetKeyName(3, "textures.png");
			this.imglstProjectTreeView.Images.SetKeyName(4, "texture.png");
			this.imglstProjectTreeView.Images.SetKeyName(5, "effects.png");
			this.imglstProjectTreeView.Images.SetKeyName(6, "technique.png");
			this.imglstProjectTreeView.Images.SetKeyName(7, "pass.png");
			this.imglstProjectTreeView.Images.SetKeyName(8, "delete.png");
			this.imglstProjectTreeView.Images.SetKeyName(9, "rename.png");
			this.imglstProjectTreeView.Images.SetKeyName(10, "new_effect.png");
			this.imglstProjectTreeView.Images.SetKeyName(11, "edit_source.png");
			// 
			// splitContainer1
			// 
			this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.splitContainer1.Location = new System.Drawing.Point(12, 52);
			this.splitContainer1.Name = "splitContainer1";
			// 
			// splitContainer1.Panel1
			// 
			this.splitContainer1.Panel1.Controls.Add(this.trvProject);
			// 
			// splitContainer1.Panel2
			// 
			this.splitContainer1.Panel2.Controls.Add(this.splitContainer2);
			this.splitContainer1.Size = new System.Drawing.Size(1074, 745);
			this.splitContainer1.SplitterDistance = 173;
			this.splitContainer1.TabIndex = 9;
			this.splitContainer1.SplitterMoved += new System.Windows.Forms.SplitterEventHandler(this.splitContainer_SplitterMoved);
			// 
			// splitContainer2
			// 
			this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitContainer2.Location = new System.Drawing.Point(0, 0);
			this.splitContainer2.Name = "splitContainer2";
			this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
			// 
			// splitContainer2.Panel1
			// 
			this.splitContainer2.Panel1.Controls.Add(this.viewportDX);
			// 
			// splitContainer2.Panel2
			// 
			this.splitContainer2.Panel2.Controls.Add(this.splitContainer3);
			this.splitContainer2.Size = new System.Drawing.Size(897, 745);
			this.splitContainer2.SplitterDistance = 498;
			this.splitContainer2.TabIndex = 0;
			this.splitContainer2.SplitterMoved += new System.Windows.Forms.SplitterEventHandler(this.splitContainer_SplitterMoved);
			// 
			// splitContainer3
			// 
			this.splitContainer3.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitContainer3.Location = new System.Drawing.Point(0, 0);
			this.splitContainer3.Name = "splitContainer3";
			// 
			// splitContainer3.Panel1
			// 
			this.splitContainer3.Panel1.Controls.Add(this.tabWorkflowManagers);
			// 
			// splitContainer3.Panel2
			// 
			this.splitContainer3.Panel2.Controls.Add(this.pnlParameterSlider);
			this.splitContainer3.Panel2.Controls.Add(this.grdUIParameters);
			this.splitContainer3.Panel2Collapsed = true;
			this.splitContainer3.Size = new System.Drawing.Size(897, 243);
			this.splitContainer3.SplitterDistance = 649;
			this.splitContainer3.TabIndex = 0;
			this.splitContainer3.SplitterMoved += new System.Windows.Forms.SplitterEventHandler(this.splitContainer_SplitterMoved);
			// 
			// tabWorkflowManagers
			// 
			this.tabWorkflowManagers.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tabWorkflowManagers.Location = new System.Drawing.Point(0, 0);
			this.tabWorkflowManagers.Name = "tabWorkflowManagers";
			this.tabWorkflowManagers.SelectedIndex = 0;
			this.tabWorkflowManagers.Size = new System.Drawing.Size(897, 243);
			this.tabWorkflowManagers.TabIndex = 4;
			this.tabWorkflowManagers.Selected += new System.Windows.Forms.TabControlEventHandler(this.tabWorkflowManagers_Selected);
			this.tabWorkflowManagers.ControlAdded += new System.Windows.Forms.ControlEventHandler(this.tabWorkflowManagers_ControlAdded);
			// 
			// pnlParameterSlider
			// 
			this.pnlParameterSlider.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.pnlParameterSlider.Location = new System.Drawing.Point(22, 150);
			this.pnlParameterSlider.Name = "pnlParameterSlider";
			this.pnlParameterSlider.Size = new System.Drawing.Size(179, 56);
			this.pnlParameterSlider.TabIndex = 1;
			// 
			// grdUIParameters
			// 
			this.grdUIParameters.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.grdUIParameters.HelpVisible = false;
			this.grdUIParameters.Location = new System.Drawing.Point(22, 18);
			this.grdUIParameters.Name = "grdUIParameters";
			this.grdUIParameters.PropertySort = System.Windows.Forms.PropertySort.NoSort;
			this.grdUIParameters.Size = new System.Drawing.Size(149, 100);
			this.grdUIParameters.TabIndex = 0;
			this.grdUIParameters.ToolbarVisible = false;
			this.grdUIParameters.SelectedGridItemChanged += new System.Windows.Forms.SelectedGridItemChangedEventHandler(this.grdUIParameters_SelectedGridItemChanged);
			// 
			// frmMain
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.ClientSize = new System.Drawing.Size(1098, 809);
			this.Controls.Add(this.splitContainer1);
			this.Controls.Add(this.tlbToolBar);
			this.Controls.Add(this.mnuMain);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MainMenuStrip = this.mnuMain;
			this.Name = "frmMain";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Studio Post-Effect";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmMain_FormClosing);
			this.mnuMain.ResumeLayout(false);
			this.mnuMain.PerformLayout();
			this.tlbToolBar.ResumeLayout(false);
			this.tlbToolBar.PerformLayout();
			this.splitContainer1.Panel1.ResumeLayout(false);
			this.splitContainer1.Panel2.ResumeLayout(false);
			this.splitContainer1.ResumeLayout(false);
			this.splitContainer2.Panel1.ResumeLayout(false);
			this.splitContainer2.Panel2.ResumeLayout(false);
			this.splitContainer2.ResumeLayout(false);
			this.splitContainer3.Panel1.ResumeLayout(false);
			this.splitContainer3.Panel2.ResumeLayout(false);
			this.splitContainer3.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.MenuStrip mnuMain;
		private System.Windows.Forms.ToolStripMenuItem mnuFile;
		private System.Windows.Forms.ToolStripMenuItem mnuFileOpenProject;
		private System.Windows.Forms.ToolStripMenuItem mnuFileSaveProject;
		private System.Windows.Forms.ToolStripMenuItem mnuFileSaveProjectAs;
		private System.Windows.Forms.ToolStripMenuItem mnuCloseProject;
		private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
		private System.Windows.Forms.ToolStripMenuItem mnuFileClose;
		private System.Windows.Forms.ToolStripMenuItem mnuPlugins;
		private System.Windows.Forms.ToolStrip tlbToolBar;
		private System.Windows.Forms.ToolStripButton btnToolbarOpenProject;
		private System.Windows.Forms.ToolStripButton btnToolbarSaveProject;
		private System.Windows.Forms.ToolStripLabel lblToolBarFPS;
		private System.Windows.Forms.ToolStripButton btnToolbarNewProject;
		private ViewportDXControl.ViewportDX viewportDX;
		private System.Windows.Forms.TreeView trvProject;
		private System.Windows.Forms.SplitContainer splitContainer1;
		private System.Windows.Forms.SplitContainer splitContainer2;
		private System.Windows.Forms.ToolStripMenuItem mnuFileNewProject;
		private System.Windows.Forms.TabControl tabWorkflowManagers;
		private System.Windows.Forms.ToolStripMenuItem mnuHelp;
		private System.Windows.Forms.ToolStripMenuItem mnuHelpAbout;
		private System.Windows.Forms.SplitContainer splitContainer3;
		private System.Windows.Forms.ImageList imglstProjectTreeView;
		private System.Windows.Forms.ToolStripMenuItem mnuHelpHome;
		private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
		private System.Windows.Forms.PropertyGrid grdUIParameters;
		private System.Windows.Forms.Panel pnlParameterSlider;
	}
}

