namespace InternalEffect
{
	partial class frmShowFrame
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
			this.picFrame = new System.Windows.Forms.PictureBox();
			((System.ComponentModel.ISupportInitialize)(this.picFrame)).BeginInit();
			this.SuspendLayout();
			// 
			// picFrame
			// 
			this.picFrame.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.picFrame.Location = new System.Drawing.Point(12, 12);
			this.picFrame.Name = "picFrame";
			this.picFrame.Size = new System.Drawing.Size(414, 353);
			this.picFrame.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
			this.picFrame.TabIndex = 0;
			this.picFrame.TabStop = false;
			// 
			// frmShowFrame
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.ClientSize = new System.Drawing.Size(438, 377);
			this.Controls.Add(this.picFrame);
			this.MinimizeBox = false;
			this.Name = "frmShowFrame";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Frame";
			((System.ComponentModel.ISupportInitialize)(this.picFrame)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.PictureBox picFrame;
	}
}