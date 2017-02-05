namespace InternalEffect
{
	partial class SliderControl
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

		#region Component Designer generated code

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.lblValue = new System.Windows.Forms.Label();
			this.trkValue = new System.Windows.Forms.TrackBar();
			this.lblMinBound = new System.Windows.Forms.Label();
			this.lblMaxBound = new System.Windows.Forms.Label();
			((System.ComponentModel.ISupportInitialize)(this.trkValue)).BeginInit();
			this.SuspendLayout();
			// 
			// lblValue
			// 
			this.lblValue.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.lblValue.Location = new System.Drawing.Point(0, 1);
			this.lblValue.Name = "lblValue";
			this.lblValue.Size = new System.Drawing.Size(310, 17);
			this.lblValue.TabIndex = 0;
			this.lblValue.Text = "0";
			// 
			// trkValue
			// 
			this.trkValue.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.trkValue.AutoSize = false;
			this.trkValue.Location = new System.Drawing.Point(-1, 16);
			this.trkValue.Maximum = 1000;
			this.trkValue.Name = "trkValue";
			this.trkValue.Size = new System.Drawing.Size(312, 31);
			this.trkValue.TabIndex = 1;
			this.trkValue.TickStyle = System.Windows.Forms.TickStyle.None;
			this.trkValue.Scroll += new System.EventHandler(this.trkValue_Scroll);
			// 
			// lblMinBound
			// 
			this.lblMinBound.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.lblMinBound.AutoSize = true;
			this.lblMinBound.Location = new System.Drawing.Point(0, 42);
			this.lblMinBound.Name = "lblMinBound";
			this.lblMinBound.Size = new System.Drawing.Size(13, 13);
			this.lblMinBound.TabIndex = 3;
			this.lblMinBound.Text = "0";
			// 
			// lblMaxBound
			// 
			this.lblMaxBound.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.lblMaxBound.Location = new System.Drawing.Point(0, 42);
			this.lblMaxBound.Name = "lblMaxBound";
			this.lblMaxBound.Size = new System.Drawing.Size(312, 13);
			this.lblMaxBound.TabIndex = 4;
			this.lblMaxBound.Text = "1";
			this.lblMaxBound.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// SliderControl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.lblValue);
			this.Controls.Add(this.lblMinBound);
			this.Controls.Add(this.lblMaxBound);
			this.Controls.Add(this.trkValue);
			this.MaximumSize = new System.Drawing.Size(5000, 56);
			this.MinimumSize = new System.Drawing.Size(60, 56);
			this.Name = "SliderControl";
			this.Size = new System.Drawing.Size(310, 56);
			((System.ComponentModel.ISupportInitialize)(this.trkValue)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label lblValue;
		private System.Windows.Forms.TrackBar trkValue;
		private System.Windows.Forms.Label lblMinBound;
		private System.Windows.Forms.Label lblMaxBound;
	}
}
