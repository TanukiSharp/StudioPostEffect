namespace SkyboxScenePlugin
{
	partial class frmConfig
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
			this.lblSkyBoxes = new System.Windows.Forms.Label();
			this.cboSkyboxes = new System.Windows.Forms.ComboBox();
			this.btnLoadSkyBox = new System.Windows.Forms.Button();
			this.btnClose = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// lblSkyBoxes
			// 
			this.lblSkyBoxes.AutoSize = true;
			this.lblSkyBoxes.Location = new System.Drawing.Point(12, 8);
			this.lblSkyBoxes.Name = "lblSkyBoxes";
			this.lblSkyBoxes.Size = new System.Drawing.Size(60, 12);
			this.lblSkyBoxes.TabIndex = 0;
			this.lblSkyBoxes.Text = "Skyboxes :";
			// 
			// cboSkyboxes
			// 
			this.cboSkyboxes.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.cboSkyboxes.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboSkyboxes.FormattingEnabled = true;
			this.cboSkyboxes.Location = new System.Drawing.Point(12, 23);
			this.cboSkyboxes.Name = "cboSkyboxes";
			this.cboSkyboxes.Size = new System.Drawing.Size(178, 20);
			this.cboSkyboxes.TabIndex = 1;
			// 
			// btnLoadSkyBox
			// 
			this.btnLoadSkyBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnLoadSkyBox.Location = new System.Drawing.Point(196, 21);
			this.btnLoadSkyBox.Name = "btnLoadSkyBox";
			this.btnLoadSkyBox.Size = new System.Drawing.Size(75, 23);
			this.btnLoadSkyBox.TabIndex = 2;
			this.btnLoadSkyBox.Text = "Load";
			this.btnLoadSkyBox.UseVisualStyleBackColor = true;
			this.btnLoadSkyBox.Click += new System.EventHandler(this.btnLoadSkyBox_Click);
			// 
			// btnClose
			// 
			this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.btnClose.Location = new System.Drawing.Point(12, 64);
			this.btnClose.Name = "btnClose";
			this.btnClose.Size = new System.Drawing.Size(75, 23);
			this.btnClose.TabIndex = 3;
			this.btnClose.Text = "Close";
			this.btnClose.UseVisualStyleBackColor = true;
			this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
			// 
			// frmConfig
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.ClientSize = new System.Drawing.Size(283, 99);
			this.ControlBox = false;
			this.Controls.Add(this.btnClose);
			this.Controls.Add(this.btnLoadSkyBox);
			this.Controls.Add(this.cboSkyboxes);
			this.Controls.Add(this.lblSkyBoxes);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "frmConfig";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "SkyBox Plugin - Configuration";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label lblSkyBoxes;
		private System.Windows.Forms.ComboBox cboSkyboxes;
		private System.Windows.Forms.Button btnLoadSkyBox;
		private System.Windows.Forms.Button btnClose;
	}
}