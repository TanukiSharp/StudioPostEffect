namespace InternalEffect.UIParameters
{
	partial class MousePadControl
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
			this.pnlToolBar = new System.Windows.Forms.Panel();
			this.cboCoordType = new System.Windows.Forms.ComboBox();
			this.pnlToolBar.SuspendLayout();
			this.SuspendLayout();
			// 
			// pnlToolBar
			// 
			this.pnlToolBar.BackColor = System.Drawing.Color.Silver;
			this.pnlToolBar.Controls.Add(this.cboCoordType);
			this.pnlToolBar.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.pnlToolBar.Location = new System.Drawing.Point(0, 483);
			this.pnlToolBar.Name = "pnlToolBar";
			this.pnlToolBar.Size = new System.Drawing.Size(599, 57);
			this.pnlToolBar.TabIndex = 0;
			// 
			// cboCoordType
			// 
			this.cboCoordType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboCoordType.FormattingEnabled = true;
			this.cboCoordType.Items.AddRange(new object[] {
            "0 -> 1",
            "-1 -> 1"});
			this.cboCoordType.Location = new System.Drawing.Point(3, 34);
			this.cboCoordType.Name = "cboCoordType";
			this.cboCoordType.Size = new System.Drawing.Size(121, 20);
			this.cboCoordType.TabIndex = 0;
			this.cboCoordType.SelectedIndexChanged += new System.EventHandler(this.cboCoordType_SelectedIndexChanged);
			// 
			// MousePadControl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.pnlToolBar);
			this.Name = "MousePadControl";
			this.Size = new System.Drawing.Size(599, 540);
			this.pnlToolBar.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Panel pnlToolBar;
		private System.Windows.Forms.ComboBox cboCoordType;
	}
}
