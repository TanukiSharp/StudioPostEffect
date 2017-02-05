using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel;
using System.IO;

namespace InternalEffect
{
	public class TechniqueTreeNode : BaseElementTreeNode
	{
		private CustomTechnique m_CustomTech;

		public TechniqueTreeNode(CustomTechnique technique)
			: base(technique)
		{
			m_CustomTech = technique;
		}

		public override void Initialize()
		{
			foreach (CustomPass pass in m_CustomTech.Passes)
				this.AddNode(new PassTreeNode(pass));

			this.ImageIndex = 6;
			this.SelectedImageIndex = 6;

			GenerateContextMenu();
		}


		private ToolStripMenuItem m_RemoveEffectMenuItem;

		private void GenerateContextMenu()
		{
			ContextMenuStrip menu = new ContextMenuStrip();
			this.ContextMenuStrip = menu;
			menu.Opening += new CancelEventHandler(OnTechniqueContextMenuClick);
			menu.ImageList = this.TreeView.ImageList;

			m_RemoveEffectMenuItem = new ToolStripMenuItem("Remove Effect");
			menu.Items.Add(m_RemoveEffectMenuItem);
			m_RemoveEffectMenuItem.Click += new EventHandler(OnRemoveEffectMenuClick);
			m_RemoveEffectMenuItem.ImageIndex = 8;
		}

		private void OnTechniqueContextMenuClick(object sender, CancelEventArgs e)
		{
			CustomTechnique technique = (CustomTechnique)m_Element;

			Project project = GetProjectNode();
			if (project != null)
			{
				foreach (CompositionTreeNode ctn in project.CompositionsTreeNode.Nodes)
				{
					foreach (EffectWorkflowItem item in ctn.WorkflowManager.EffectWorkflowItems)
					{
						if (item.Effect != null && item.Effect == technique.ParentEffect)
						{
							m_RemoveEffectMenuItem.Text = "Remove Effect [Effect in use]";
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

			CustomTechnique technique = (CustomTechnique)m_Element;

			string msg = string.Format("Do you want to delete the related shader file ?\r\n{0}", technique.ParentEffect.Filename);
			DialogResult dlgres = MessageBox.Show(msg, "Delete shader file ?", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);

			if (dlgres == DialogResult.Cancel)
				return;

			if (dlgres == DialogResult.Yes)
				File.Delete(technique.ParentEffect.Filename);

			BaseTreeNode parent = this.Parent;
			foreach (BaseElementTreeNode basetn in GlobalContainer.Project.EffectsTreeNode.Nodes)
			{
				if (basetn.Element is CustomTechnique)
				{
					CustomTechnique tech = (CustomTechnique)basetn.Element;
					if (tech.ParentEffect == technique.ParentEffect)
						parent.Nodes.Remove(basetn);
				}
				else if (basetn.Element is CustomPass)
				{
					CustomPass pass = (CustomPass)basetn.Element;
					if (pass.ParentTechnique.ParentEffect == technique.ParentEffect)
						parent.Nodes.Remove(basetn);
				}
			}

			GlobalContainer.Project.IsModified = true;
		}

		public CustomTechnique Technique
		{
			get
			{
				return (m_Element as CustomTechnique);
			}
		}
	}
}
