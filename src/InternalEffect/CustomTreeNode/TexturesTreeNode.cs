using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using Microsoft.DirectX.Direct3D;

namespace InternalEffect
{
	public class TexturesTreeNode : BaseTreeNode
	{
		public TexturesTreeNode()
		{
			this.Text = "Textures";
		}

		public override void Initialize()
		{
			this.ImageIndex = 3;
			this.SelectedImageIndex = 3;

			GenerateContextMenu();
		}

		private void GenerateContextMenu()
		{
			ContextMenuStrip menu = new ContextMenuStrip();
			menu.ImageList = this.TreeView.ImageList;

			ToolStripMenuItem item = new ToolStripMenuItem("Add Texture...");
			menu.Items.Add(item);
			item.Click += new EventHandler(OnAddTextureMenuClick);
			item.ImageIndex = 4;

			this.ContextMenuStrip = menu;
		}

		private void OnAddTextureMenuClick(object sender, EventArgs e)
		{
			OpenFileDialog ofd = new OpenFileDialog();
			ofd.Title = "Select a texture file";
			ofd.Filter = "Bitmap Files (*.bmp)|*.bmp|PNG Files (*.png)|*.png|JPEG Files (*.jpg, *.jpeg)|*.jpg;*.jpeg|GIF Files (*.gif)|*.gif|All Image Files|*.bmp;*.png;*.jpg;*.jpeg;*.gif|All Files (*.*)|*.*";
			ofd.FilterIndex = 5;
			ofd.Multiselect = true;
			ofd.CheckFileExists = true;

			if (ofd.ShowDialog() != DialogResult.OK)
				return;

			Project project = GetProjectNode();

			bool modified = false;
			foreach (string filename in ofd.FileNames)
			{
				string textureRelativeFilename = project.AttachFileToProject(filename, "textures");
				if (textureRelativeFilename != null)
				{
					AddNode(new TextureTreeNode(project.ProjectDirectory, textureRelativeFilename));
					modified = true;
				}
			}

			if (modified)
				Modified();
		}
	}
}
