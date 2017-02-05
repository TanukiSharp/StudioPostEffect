namespace StudioPostEffect
{
	partial class frmAbout
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmAbout));
			this.picLogo = new System.Windows.Forms.PictureBox();
			this.btnOK = new System.Windows.Forms.Button();
			this.lblVersion = new System.Windows.Forms.Label();
			this.lblDescription = new System.Windows.Forms.Label();
			this.lblHome = new System.Windows.Forms.Label();
			((System.ComponentModel.ISupportInitialize)(this.picLogo)).BeginInit();
			this.SuspendLayout();
			// 
			// picLogo
			// 
			this.picLogo.Image = ((System.Drawing.Image)(resources.GetObject("picLogo.Image")));
			this.picLogo.Location = new System.Drawing.Point(0, 0);
			this.picLogo.Name = "picLogo";
			this.picLogo.Size = new System.Drawing.Size(671, 92);
			this.picLogo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
			this.picLogo.TabIndex = 0;
			this.picLogo.TabStop = false;
			// 
			// btnOK
			// 
			this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnOK.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnOK.Location = new System.Drawing.Point(584, 273);
			this.btnOK.Name = "btnOK";
			this.btnOK.Size = new System.Drawing.Size(75, 23);
			this.btnOK.TabIndex = 1;
			this.btnOK.Text = "OK";
			this.btnOK.UseVisualStyleBackColor = true;
			this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
			// 
			// lblVersion
			// 
			this.lblVersion.AutoSize = true;
			this.lblVersion.Font = new System.Drawing.Font("Arial", 12F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblVersion.Location = new System.Drawing.Point(415, 95);
			this.lblVersion.Name = "lblVersion";
			this.lblVersion.Size = new System.Drawing.Size(0, 18);
			this.lblVersion.TabIndex = 2;
			// 
			// lblDescription
			// 
			this.lblDescription.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblDescription.Location = new System.Drawing.Point(32, 119);
			this.lblDescription.Name = "lblDescription";
			this.lblDescription.Size = new System.Drawing.Size(356, 104);
			this.lblDescription.TabIndex = 3;
			this.lblDescription.Text = "Studio Post-Effect\r\nCopyright (c) Sebastien SAIGO 2008-2009\r\n\r\nWritten in Microso" +
				"ft .NET C# using Managed DirectX.\r\nStudio Post-Effect is a free software, still " +
				"in developpment.\r\n";
			// 
			// lblHome
			// 
			this.lblHome.AutoSize = true;
			this.lblHome.Cursor = System.Windows.Forms.Cursors.Hand;
			this.lblHome.Font = new System.Drawing.Font("Arial", 9F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblHome.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(119)))), ((int)(((byte)(170)))));
			this.lblHome.Location = new System.Drawing.Point(32, 266);
			this.lblHome.Name = "lblHome";
			this.lblHome.Size = new System.Drawing.Size(58, 15);
			this.lblHome.TabIndex = 4;
			this.lblHome.Text = "Web Site";
			this.lblHome.Click += new System.EventHandler(this.lblHome_Click);
			// 
			// frmAbout
			// 
			this.AcceptButton = this.btnOK;
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.BackColor = System.Drawing.Color.White;
			this.CancelButton = this.btnOK;
			this.ClientSize = new System.Drawing.Size(671, 308);
			this.ControlBox = false;
			this.Controls.Add(this.lblHome);
			this.Controls.Add(this.lblDescription);
			this.Controls.Add(this.lblVersion);
			this.Controls.Add(this.btnOK);
			this.Controls.Add(this.picLogo);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "frmAbout";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "About Studio Post-Effect";
			((System.ComponentModel.ISupportInitialize)(this.picLogo)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.PictureBox picLogo;
		private System.Windows.Forms.Button btnOK;
		private System.Windows.Forms.Label lblVersion;
		private System.Windows.Forms.Label lblDescription;
		private System.Windows.Forms.Label lblHome;

	}
}