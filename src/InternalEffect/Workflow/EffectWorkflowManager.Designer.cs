namespace InternalEffect
{
	partial class EffectWorkflowManager
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
			this.ctxMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.ctxNewSequence = new System.Windows.Forms.ToolStripMenuItem();
			this.ctxNewScene = new System.Windows.Forms.ToolStripMenuItem();
			this.ctxNewPreviousFrame = new System.Windows.Forms.ToolStripMenuItem();
			this.ctxMenu.SuspendLayout();
			this.SuspendLayout();
			// 
			// ctxMenu
			// 
			this.ctxMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ctxNewSequence});
			this.ctxMenu.Name = "ctxMenu";
			this.ctxMenu.Size = new System.Drawing.Size(153, 48);
			// 
			// ctxNewSequence
			// 
			this.ctxNewSequence.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ctxNewScene,
            this.ctxNewPreviousFrame});
			this.ctxNewSequence.Name = "ctxNewSequence";
			this.ctxNewSequence.Size = new System.Drawing.Size(152, 22);
			this.ctxNewSequence.Text = "New";
			// 
			// ctxNewScene
			// 
			this.ctxNewScene.Name = "ctxNewScene";
			this.ctxNewScene.Size = new System.Drawing.Size(152, 22);
			this.ctxNewScene.Text = "Scene";
			this.ctxNewScene.Click += new System.EventHandler(this.ctxNewScene_Click);
			// 
			// ctxNewPreviousFrame
			// 
			this.ctxNewPreviousFrame.Name = "ctxNewPreviousFrame";
			this.ctxNewPreviousFrame.Size = new System.Drawing.Size(152, 22);
			this.ctxNewPreviousFrame.Text = "Previous Frame";
			this.ctxNewPreviousFrame.Click += new System.EventHandler(this.ctxNewPreviousFrame_Click);
			// 
			// EffectWorkflowManager
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.DoubleBuffered = true;
			this.Name = "EffectWorkflowManager";
			this.Size = new System.Drawing.Size(342, 285);
			this.ctxMenu.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.ContextMenuStrip ctxMenu;
		private System.Windows.Forms.ToolStripMenuItem ctxNewSequence;
		private System.Windows.Forms.ToolStripMenuItem ctxNewScene;
		private System.Windows.Forms.ToolStripMenuItem ctxNewPreviousFrame;
	}
}
