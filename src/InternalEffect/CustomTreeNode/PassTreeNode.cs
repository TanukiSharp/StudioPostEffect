using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel;
using System.IO;

namespace InternalEffect
{
	public class PassTreeNode : BaseElementTreeNode
	{
		public PassTreeNode(CustomPass pass)
			: base(pass)
		{
		}

		public CustomPass Pass
		{
			get
			{
				return (m_Element as CustomPass);
			}
		}

		public override void Initialize()
		{
			this.ImageIndex = 7;
			this.SelectedImageIndex = 7;

			GenerateContextMenu();
		}

		private ToolStripMenuItem m_RemoveEffectMenuItem;

		private void GenerateContextMenu()
		{
			ContextMenuStrip menu = new ContextMenuStrip();
			this.ContextMenuStrip = menu;
			menu.Opening += new CancelEventHandler(OnPassContextMenuClick);
			menu.ImageList = this.TreeView.ImageList;

			m_RemoveEffectMenuItem = new ToolStripMenuItem("Remove Effect");
			menu.Items.Add(m_RemoveEffectMenuItem);
			m_RemoveEffectMenuItem.Click += new EventHandler(OnRemoveEffectMenuClick);
			m_RemoveEffectMenuItem.ImageIndex = 8;
		}

		private void OnPassContextMenuClick(object sender, CancelEventArgs e)
		{
			CustomPass pass = (CustomPass)m_Element;

			Project project = GetProjectNode();
			if (project != null)
			{
				foreach (CompositionTreeNode ctn in project.CompositionsTreeNode.Nodes)
				{
					foreach (EffectWorkflowItem item in ctn.WorkflowManager.EffectWorkflowItems)
					{
						if (item.Effect != null && item.Effect == pass.ParentTechnique.ParentEffect)
						{
							m_RemoveEffectMenuItem.Text = "Remove Effect [in use]";
							m_RemoveEffectMenuItem.Enabled = false;
							return;
						}
					}
				}
			}

			m_RemoveEffectMenuItem.Text = "Remove Effect";
			m_RemoveEffectMenuItem.Enabled = true;
		}

		private void OnRemoveEffectMenuClick(object sender, EventArgs e)
		{
			if (GlobalContainer.Project == null)
				return;

			CustomPass pass = (CustomPass)m_Element;

			string msg = string.Format("Do you want to delete the related shader file ?\r\n{0}", pass.ParentTechnique.ParentEffect.Filename);
			DialogResult dlgres = MessageBox.Show(msg, "Delete shader file ?", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);

			if (dlgres == DialogResult.Cancel)
				return;

			if (dlgres == DialogResult.Yes)
				File.Delete(pass.ParentTechnique.ParentEffect.Filename);

			BaseTreeNode parent = this.Parent;
			foreach (BaseElementTreeNode basetn in GlobalContainer.Project.EffectsTreeNode.Nodes)
			{
				if (basetn.Element is CustomTechnique)
				{
					CustomTechnique tech = (CustomTechnique)basetn.Element;
					if (tech.ParentEffect == pass.ParentTechnique.ParentEffect)
						parent.Nodes.Remove(basetn);
				}
				else if (basetn.Element is CustomPass)
				{
					CustomPass p = (CustomPass)basetn.Element;
					if (p.ParentTechnique.ParentEffect == pass.ParentTechnique.ParentEffect)
						parent.Nodes.Remove(basetn);
				}
			}

			GlobalContainer.Project.IsModified = true;
		}
	}
}
