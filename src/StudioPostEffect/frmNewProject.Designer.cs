namespace StudioPostEffect
{
	partial class frmNewProject
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
			this.lblName = new System.Windows.Forms.Label();
			this.lblLocation = new System.Windows.Forms.Label();
			this.chkCreateProjectDirectory = new System.Windows.Forms.CheckBox();
			this.txtName = new System.Windows.Forms.TextBox();
			this.txtLocation = new System.Windows.Forms.TextBox();
			this.btnBrowse = new System.Windows.Forms.Button();
			this.btnOK = new System.Windows.Forms.Button();
			this.btnCancel = new System.Windows.Forms.Button();
			this.lblLineDark = new System.Windows.Forms.Label();
			this.lblLineClear = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// lblName
			// 
			this.lblName.AutoSize = true;
			this.lblName.Location = new System.Drawing.Point(12, 15);
			this.lblName.Name = "lblName";
			this.lblName.Size = new System.Drawing.Size(38, 13);
			this.lblName.TabIndex = 0;
			this.lblName.Text = "Name:";
			// 
			// lblLocation
			// 
			this.lblLocation.AutoSize = true;
			this.lblLocation.Location = new System.Drawing.Point(12, 41);
			this.lblLocation.Name = "lblLocation";
			this.lblLocation.Size = new System.Drawing.Size(51, 13);
			this.lblLocation.TabIndex = 1;
			this.lblLocation.Text = "Location:";
			// 
			// chkCreateProjectDirectory
			// 
			this.chkCreateProjectDirectory.AutoSize = true;
			this.chkCreateProjectDirectory.Checked = true;
			this.chkCreateProjectDirectory.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkCreateProjectDirectory.Location = new System.Drawing.Point(305, 64);
			this.chkCreateProjectDirectory.Name = "chkCreateProjectDirectory";
			this.chkCreateProjectDirectory.Size = new System.Drawing.Size(150, 17);
			this.chkCreateProjectDirectory.TabIndex = 3;
			this.chkCreateProjectDirectory.Text = "Create directory for project";
			this.chkCreateProjectDirectory.UseVisualStyleBackColor = true;
			// 
			// txtName
			// 
			this.txtName.Location = new System.Drawing.Point(72, 12);
			this.txtName.Name = "txtName";
			this.txtName.Size = new System.Drawing.Size(383, 20);
			this.txtName.TabIndex = 0;
			// 
			// txtLocation
			// 
			this.txtLocation.Location = new System.Drawing.Point(72, 38);
			this.txtLocation.Name = "txtLocation";
			this.txtLocation.Size = new System.Drawing.Size(383, 20);
			this.txtLocation.TabIndex = 1;
			// 
			// btnBrowse
			// 
			this.btnBrowse.Location = new System.Drawing.Point(461, 36);
			this.btnBrowse.Name = "btnBrowse";
			this.btnBrowse.Size = new System.Drawing.Size(75, 23);
			this.btnBrowse.TabIndex = 2;
			this.btnBrowse.Text = "Browse...";
			this.btnBrowse.UseVisualStyleBackColor = true;
			this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
			// 
			// btnOK
			// 
			this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnOK.Location = new System.Drawing.Point(382, 104);
			this.btnOK.Name = "btnOK";
			this.btnOK.Size = new System.Drawing.Size(75, 23);
			this.btnOK.TabIndex = 4;
			this.btnOK.Text = "OK";
			this.btnOK.UseVisualStyleBackColor = true;
			this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
			// 
			// btnCancel
			// 
			this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Location = new System.Drawing.Point(463, 104);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(75, 23);
			this.btnCancel.TabIndex = 5;
			this.btnCancel.Text = "Cancel";
			this.btnCancel.UseVisualStyleBackColor = true;
			this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
			// 
			// lblLineDark
			// 
			this.lblLineDark.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.lblLineDark.BackColor = System.Drawing.Color.DarkGray;
			this.lblLineDark.Location = new System.Drawing.Point(12, 93);
			this.lblLineDark.Name = "lblLineDark";
			this.lblLineDark.Size = new System.Drawing.Size(526, 1);
			this.lblLineDark.TabIndex = 8;
			// 
			// lblLineClear
			// 
			this.lblLineClear.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.lblLineClear.BackColor = System.Drawing.Color.White;
			this.lblLineClear.Location = new System.Drawing.Point(12, 93);
			this.lblLineClear.Name = "lblLineClear";
			this.lblLineClear.Size = new System.Drawing.Size(527, 2);
			this.lblLineClear.TabIndex = 9;
			// 
			// frmNewProject
			// 
			this.AcceptButton = this.btnOK;
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.BackColor = System.Drawing.SystemColors.Control;
			this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
			this.CancelButton = this.btnCancel;
			this.ClientSize = new System.Drawing.Size(550, 139);
			this.Controls.Add(this.lblLineDark);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.btnOK);
			this.Controls.Add(this.btnBrowse);
			this.Controls.Add(this.txtLocation);
			this.Controls.Add(this.txtName);
			this.Controls.Add(this.chkCreateProjectDirectory);
			this.Controls.Add(this.lblLocation);
			this.Controls.Add(this.lblName);
			this.Controls.Add(this.lblLineClear);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "frmNewProject";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "New Project";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label lblName;
		private System.Windows.Forms.Label lblLocation;
		private System.Windows.Forms.CheckBox chkCreateProjectDirectory;
		private System.Windows.Forms.TextBox txtName;
		private System.Windows.Forms.TextBox txtLocation;
		private System.Windows.Forms.Button btnBrowse;
		private System.Windows.Forms.Button btnOK;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.Label lblLineDark;
		private System.Windows.Forms.Label lblLineClear;
	}
}