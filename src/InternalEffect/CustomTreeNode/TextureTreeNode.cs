using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.DirectX.Direct3D;
using System.Xml;
using System.IO;
using System.Windows.Forms;
using System.ComponentModel;
using PostEffectCore;

namespace InternalEffect
{
	public class TextureTreeNode : BaseTreeNode
	{
		private Device m_Device;
		private string m_ProjectDirectory;
		private string m_TextureRelativeFilename;

		private Texture m_Texture;

		public TextureTreeNode(string projectDirectory, string textureRelativeFilename)
		{
			m_ProjectDirectory = projectDirectory;
			m_TextureRelativeFilename = textureRelativeFilename;
		}

		public TextureTreeNode(string projectDirectory, XmlNode node)
		{
			m_ProjectDirectory = projectDirectory;
			m_TextureRelativeFilename = XmlHelper.GetNodeAttributeValue(node, "file");
			m_TextureRelativeFilename.Replace('\\', '/');
		}

		public override void Initialize()
		{
			this.ImageIndex = 4;
			this.SelectedImageIndex = 4;

			m_Device = GetProjectNode().Device;
			string textureFullFilename = Path.Combine(m_ProjectDirectory, m_TextureRelativeFilename);
			m_Texture = TextureLoader.FromFile(m_Device, textureFullFilename);
			this.Text = Path.ChangeExtension(m_TextureRelativeFilename, null).Substring(9); // 9 == "textures/".Length

			GenerateContextMenu();
		}

		private ToolStripMenuItem m_RemoveTextureMenuItem;

		private void GenerateContextMenu()
		{
			ContextMenuStrip menu = new ContextMenuStrip();
			this.ContextMenuStrip = menu;
			menu.Opening += new CancelEventHandler(OnRemoveTextureContextMenuClick);
			menu.ImageList = this.TreeView.ImageList;

			m_RemoveTextureMenuItem = new ToolStripMenuItem("Remove Texture");
			menu.Items.Add(m_RemoveTextureMenuItem);
			m_RemoveTextureMenuItem.Click += new EventHandler(OnRemoveTextureMenuClick);
			m_RemoveTextureMenuItem.ImageIndex = 8;
		}

		private void OnRemoveTextureContextMenuClick(object sender, CancelEventArgs e)
		{
			Project project = GetProjectNode();
			if (project != null)
			{
				foreach (CompositionTreeNode ctn in project.CompositionsTreeNode.Nodes)
				{
					foreach (EffectWorkflowItem item in ctn.WorkflowManager.EffectWorkflowItems)
					{
						if (item.Type == PrimitiveItemType.Texture && item.TextureFilename == this.TextureRelativeFilename)
						{
							m_RemoveTextureMenuItem.Text = "Remove Texture [in use]";
							m_RemoveTextureMenuItem.Enabled = false;
							return;
						}
					}
				}
			}

			m_RemoveTextureMenuItem.Text = "Remove Texture";
			m_RemoveTextureMenuItem.Enabled = true;
		}

		private void OnRemoveTextureMenuClick(object sender, EventArgs e)
		{
			Project project = GetProjectNode();
			if (project == null)
				return;

			this.Texture.Dispose();
			this.Parent.Nodes.Remove(this);

			if (GlobalContainer.Project != null)
				GlobalContainer.Project.IsModified = true;
		}



		public void WriteXml(XmlTextWriter xw)
		{
			xw.WriteStartElement("texture");
			xw.WriteAttributeString("file", m_TextureRelativeFilename);
			xw.WriteEndElement();
		}

		public Texture Texture
		{
			get
			{
				return (m_Texture);
			}
		}

		public string TextureRelativeFilename
		{
			get
			{
				return (m_TextureRelativeFilename);
			}
		}
	}
}
