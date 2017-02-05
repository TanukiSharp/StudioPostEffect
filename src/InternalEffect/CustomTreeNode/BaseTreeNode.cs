using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace InternalEffect
{
	public abstract class BaseTreeNode : TreeNode
	{
		public BaseTreeNode()
		{
		}

		public new BaseTreeNode Parent
		{
			get
			{
				return (base.Parent as BaseTreeNode);
			}
		}

		public void AddNode(BaseTreeNode node)
		{
			this.Nodes.Add(node);
			node.Initialize();
		}

		protected void Modified()
		{
			BaseTreeNode parent = this.Parent;

			if (parent != null)
			{
				if (parent is Project)
				{
					((Project)parent).IsModified = true;
					return;
				}
				parent.Modified();
			}
		}

		public abstract void Initialize();

		public Project GetProjectNode()
		{
			if (this is Project)
				return (this as Project);

			if (this.Parent is Project)
				return (this.Parent as Project);
			else
				return (this.Parent.GetProjectNode());
		}


		/*
		public TextureTreeNode GetTextureNode()
		{
			if (this is Project)
				return (null);

			if (this is TextureTreeNode)
				return (this as TextureTreeNode);

			if (this.Parent is TextureTreeNode)
				return (this.Parent as TextureTreeNode);
			else
				return (this.Parent.GetTextureNode());
		}
		*/
	}
}
