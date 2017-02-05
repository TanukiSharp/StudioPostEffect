namespace InternalEffect
{
	partial class frmCode
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmCode));
			this.txtCode = new ICSharpCode.TextEditor.TextEditorControl();
			this.splitContainer1 = new System.Windows.Forms.SplitContainer();
			this.txtBuildLog = new System.Windows.Forms.TextBox();
			this.toolStrip1 = new System.Windows.Forms.ToolStrip();
			this.btnBuildShader = new System.Windows.Forms.ToolStripButton();
			this.btnSaveShader = new System.Windows.Forms.ToolStripButton();
			this.splitContainer1.Panel1.SuspendLayout();
			this.splitContainer1.Panel2.SuspendLayout();
			this.splitContainer1.SuspendLayout();
			this.toolStrip1.SuspendLayout();
			this.SuspendLayout();
			// 
			// txtCode
			// 
			this.txtCode.Dock = System.Windows.Forms.DockStyle.Fill;
			this.txtCode.HideMouseCursor = true;
			this.txtCode.Location = new System.Drawing.Point(0, 0);
			this.txtCode.Name = "txtCode";
			this.txtCode.ShowEOLMarkers = true;
			this.txtCode.ShowInvalidLines = false;
			this.txtCode.ShowSpaces = true;
			this.txtCode.ShowTabs = true;
			this.txtCode.ShowVRuler = true;
			this.txtCode.Size = new System.Drawing.Size(763, 609);
			this.txtCode.TabIndex = 0;
			// 
			// splitContainer1
			// 
			this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.splitContainer1.IsSplitterFixed = true;
			this.splitContainer1.Location = new System.Drawing.Point(0, 28);
			this.splitContainer1.Name = "splitContainer1";
			this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
			// 
			// splitContainer1.Panel1
			// 
			this.splitContainer1.Panel1.Controls.Add(this.txtCode);
			// 
			// splitContainer1.Panel2
			// 
			this.splitContainer1.Panel2.Controls.Add(this.txtBuildLog);
			this.splitContainer1.Panel2Collapsed = true;
			this.splitContainer1.Size = new System.Drawing.Size(763, 609);
			this.splitContainer1.SplitterDistance = 459;
			this.splitContainer1.TabIndex = 2;
			// 
			// txtBuildLog
			// 
			this.txtBuildLog.Dock = System.Windows.Forms.DockStyle.Fill;
			this.txtBuildLog.Location = new System.Drawing.Point(0, 0);
			this.txtBuildLog.Multiline = true;
			this.txtBuildLog.Name = "txtBuildLog";
			this.txtBuildLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.txtBuildLog.Size = new System.Drawing.Size(150, 46);
			this.txtBuildLog.TabIndex = 0;
			// 
			// toolStrip1
			// 
			this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnBuildShader,
            this.btnSaveShader});
			this.toolStrip1.Location = new System.Drawing.Point(0, 0);
			this.toolStrip1.Name = "toolStrip1";
			this.toolStrip1.Size = new System.Drawing.Size(763, 25);
			this.toolStrip1.TabIndex = 3;
			this.toolStrip1.Text = "toolStrip1";
			// 
			// btnBuildShader
			// 
			this.btnBuildShader.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.btnBuildShader.Image = ((System.Drawing.Image)(resources.GetObject("btnBuildShader.Image")));
			this.btnBuildShader.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.btnBuildShader.Name = "btnBuildShader";
			this.btnBuildShader.Size = new System.Drawing.Size(23, 22);
			this.btnBuildShader.Text = "Build";
			this.btnBuildShader.Click += new System.EventHandler(this.btnBuildShader_Click);
			// 
			// btnSaveShader
			// 
			this.btnSaveShader.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.btnSaveShader.Image = ((System.Drawing.Image)(resources.GetObject("btnSaveShader.Image")));
			this.btnSaveShader.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.btnSaveShader.Name = "btnSaveShader";
			this.btnSaveShader.Size = new System.Drawing.Size(23, 22);
			this.btnSaveShader.Text = "&Save";
			this.btnSaveShader.Click += new System.EventHandler(this.btnSaveShader_Click);
			// 
			// frmCode
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.ClientSize = new System.Drawing.Size(763, 638);
			this.Controls.Add(this.toolStrip1);
			this.Controls.Add(this.splitContainer1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
			this.Name = "frmCode";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Effect Code";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmCode_FormClosing);
			this.splitContainer1.Panel1.ResumeLayout(false);
			this.splitContainer1.Panel2.ResumeLayout(false);
			this.splitContainer1.Panel2.PerformLayout();
			this.splitContainer1.ResumeLayout(false);
			this.toolStrip1.ResumeLayout(false);
			this.toolStrip1.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private ICSharpCode.TextEditor.TextEditorControl txtCode;
		private System.Windows.Forms.SplitContainer splitContainer1;
		private System.Windows.Forms.TextBox txtBuildLog;
		private System.Windows.Forms.ToolStrip toolStrip1;
		private System.Windows.Forms.ToolStripButton btnSaveShader;
		private System.Windows.Forms.ToolStripButton btnBuildShader;
	}
}