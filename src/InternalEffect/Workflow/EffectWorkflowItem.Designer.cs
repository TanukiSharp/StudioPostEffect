namespace InternalEffect
{
	partial class EffectWorkflowItem
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
			this.components = new System.ComponentModel.Container();
			this.lblTitleTopMove = new System.Windows.Forms.Label();
			this.ctxMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.ctxEditCode = new System.Windows.Forms.ToolStripMenuItem();
			this.ctxRemoveWorkflowItem = new System.Windows.Forms.ToolStripMenuItem();
			this.pnlIO = new System.Windows.Forms.Panel();
			this.ctxMenu.SuspendLayout();
			this.SuspendLayout();
			// 
			// lblTitleTopMove
			// 
			this.lblTitleTopMove.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.lblTitleTopMove.AutoEllipsis = true;
			this.lblTitleTopMove.BackColor = System.Drawing.Color.Gray;
			this.lblTitleTopMove.ContextMenuStrip = this.ctxMenu;
			this.lblTitleTopMove.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblTitleTopMove.ForeColor = System.Drawing.Color.White;
			this.lblTitleTopMove.Location = new System.Drawing.Point(1, 1);
			this.lblTitleTopMove.Name = "lblTitleTopMove";
			this.lblTitleTopMove.Size = new System.Drawing.Size(48, 12);
			this.lblTitleTopMove.TabIndex = 0;
			// 
			// ctxMenu
			// 
			this.ctxMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ctxEditCode,
            this.ctxRemoveWorkflowItem});
			this.ctxMenu.Name = "ctxMenu";
			this.ctxMenu.Size = new System.Drawing.Size(133, 48);
			// 
			// ctxEditCode
			// 
			this.ctxEditCode.Name = "ctxEditCode";
			this.ctxEditCode.Size = new System.Drawing.Size(132, 22);
			this.ctxEditCode.Text = "Edit Code...";
			this.ctxEditCode.Click += new System.EventHandler(this.ctxEditCode_Click);
			// 
			// ctxRemoveWorkflowItem
			// 
			this.ctxRemoveWorkflowItem.Name = "ctxRemoveWorkflowItem";
			this.ctxRemoveWorkflowItem.Size = new System.Drawing.Size(132, 22);
			this.ctxRemoveWorkflowItem.Text = "Remove";
			this.ctxRemoveWorkflowItem.Click += new System.EventHandler(this.ctxRemoveWorkflowItem_Click);
			// 
			// pnlIO
			// 
			this.pnlIO.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.pnlIO.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
			this.pnlIO.Location = new System.Drawing.Point(1, 15);
			this.pnlIO.Name = "pnlIO";
			this.pnlIO.Size = new System.Drawing.Size(48, 34);
			this.pnlIO.TabIndex = 2;
			// 
			// EffectWorkflowItem
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.Gray;
			this.Controls.Add(this.pnlIO);
			this.Controls.Add(this.lblTitleTopMove);
			this.MinimumSize = new System.Drawing.Size(50, 50);
			this.Name = "EffectWorkflowItem";
			this.Size = new System.Drawing.Size(50, 50);
			this.ctxMenu.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Label lblTitleTopMove;
		private System.Windows.Forms.Panel pnlIO;
		private System.Windows.Forms.ContextMenuStrip ctxMenu;
		private System.Windows.Forms.ToolStripMenuItem ctxRemoveWorkflowItem;
		private System.Windows.Forms.ToolStripMenuItem ctxEditCode;
	}
}
