using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace InternalEffect
{
	public class CompositionsTreeNode : BaseTreeNode
	{
		public CompositionsTreeNode()
		{
			this.Text = "Compositions";
		}

		public override void Initialize()
		{
			this.ImageIndex = 1;
			this.SelectedImageIndex = 1;

			GenerateContextMenu();
			this.TreeView.AfterSelect += new TreeViewEventHandler(OnAfterSelect);
		}

		private void GenerateContextMenu()
		{
			ContextMenuStrip menu = new ContextMenuStrip();
			menu.ImageList = this.TreeView.ImageList;

			ToolStripMenuItem item = new ToolStripMenuItem("New Composition");
			item.Click += new EventHandler(OnNewCompositionMenuClick);
			item.ImageIndex = 2;

			menu.Items.Add(item);
			this.ContextMenuStrip = menu;
		}

		private void OnNewCompositionMenuClick(object sender, EventArgs e)
		{
			frmInput input = new frmInput("New Composition", "Enter Composition name :");
			if (input.ShowDialog() != DialogResult.OK)
				return;

			foreach (TreeNode tn in this.Nodes)
			{
				if (tn.Text.ToLower() == input.Value.ToLower())
				{
					MessageBox.Show(string.Format("There is already a Composition named '{0}'.\r\nPlease chose a different name.", input.Value), "Name already exists", MessageBoxButtons.OK, MessageBoxIcon.Error);
					return;
				}
			}

			int i;
			for (i = 0; i < this.Nodes.Count; i++)
			{
				if (this.Nodes[i].Text.CompareTo(input.Value) > 0)
					break;
			}

			CompositionTreeNode ctn = new CompositionTreeNode(input.Value);
			this.Nodes.Insert(i, ctn);
			ctn.Initialize();

			if (GlobalContainer.Project != null)
				GlobalContainer.Project.IsModified = true;
		}

		private void OnAfterSelect(object sender, TreeViewEventArgs e)
		{
			if (e.Node is CompositionTreeNode)
			{
				CompositionTreeNode ctn = (CompositionTreeNode)e.Node;
				TabControl tabControl = (TabControl)ctn.TabPage.Parent;
				tabControl.SelectedTab = ctn.TabPage;
			}
		}
	}
}
