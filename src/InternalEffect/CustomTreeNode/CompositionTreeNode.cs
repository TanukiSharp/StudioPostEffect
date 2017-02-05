using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Drawing;
using Microsoft.DirectX.Direct3D;
using System.IO;
using PostEffectCore;

namespace InternalEffect
{
	public class CompositionTreeNode : BaseTreeNode
	{
		private string m_Name;
		private XmlNode m_CompositionNode;
		private TabPage m_TabPage;
		private EffectWorkflowManager m_WorkflowManager;

		public CompositionTreeNode(string name)
		{
			this.Name = name;
		}

		public CompositionTreeNode(XmlNode node)
		{
			this.Name = XmlHelper.GetNodeAttributeValue(node, "name");
			m_CompositionNode = node;
		}

		public override void Initialize()
		{
			this.ImageIndex = 2;
			this.SelectedImageIndex = 2;

			TabControl tabControl = GlobalContainer.CompositionsTabControl;

			tabControl.SuspendLayout();

			m_TabPage = new TabPage(this.Name);
			tabControl.TabPages.Insert(this.Index, m_TabPage);
			m_TabPage.Tag = this;

			m_WorkflowManager = new EffectWorkflowManager();
			m_TabPage.Controls.Add(m_WorkflowManager);
			m_WorkflowManager.Name = "effectWorkflowManager";
			m_WorkflowManager.BackColor = Color.FromKnownColor(KnownColor.Window);
			m_WorkflowManager.Dock = DockStyle.Fill;
			m_WorkflowManager.AllowDrop = true;

			m_WorkflowManager.WorkflowItemSelected += new EffectWorkflowManager.WorkflowItemHandler(OnWorkflowItemSelected);
			m_WorkflowManager.EffectEdit += new EffectWorkflowManager.CustomEffectHandler(OnEffectEdit);

			if (m_CompositionNode != null)
				InitFromXml();
			else
				m_WorkflowManager.Initialize();

			// force tab selection
			tabControl.SelectedTab = null;
			tabControl.SelectedTab = m_TabPage;

			tabControl.ResumeLayout();

			GenerateContextMenu();
		}

		private void GenerateContextMenu()
		{
			ContextMenuStrip menu = new ContextMenuStrip();
			menu.ImageList = this.TreeView.ImageList;

			ToolStripMenuItem renameCompositionMenuItem = new ToolStripMenuItem("Rename Composition");
			menu.Items.Add(renameCompositionMenuItem);
			renameCompositionMenuItem.Click += new EventHandler(OnRenameCompositionMenuClick);
			renameCompositionMenuItem.ImageIndex = 9;

			ToolStripMenuItem removeCompositionMenuItem = new ToolStripMenuItem("Remove Composition");
			menu.Items.Add(removeCompositionMenuItem);
			removeCompositionMenuItem.Click += new EventHandler(OnRemoveCompositionMenuClick);
			removeCompositionMenuItem.ImageIndex = 8;

			this.ContextMenuStrip = menu;
		}

		private void OnRenameCompositionMenuClick(object sender, EventArgs e)
		{
			this.TreeView.SelectedNode = this;
			this.BeginEdit();
		}

		private void OnRemoveCompositionMenuClick(object sender, EventArgs e)
		{
			DialogResult res = MessageBox.Show("All the underlying data will be lost, are you sure to want to proceed ?", "Remove Composition", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
			if (res == DialogResult.No)
				return;

			TabControl tabControl = (TabControl)m_TabPage.Parent;
			tabControl.TabPages.Remove(m_TabPage);

			this.Parent.Nodes.Remove(this);

			if (GlobalContainer.Project != null)
				GlobalContainer.Project.IsModified = true;
		}

		private void InitFromXml()
		{
			Dictionary<int, EffectWorkflowItem> workflow = new Dictionary<int, EffectWorkflowItem>();
			InitEffects(workflow);
			InitLinks(workflow);
		}

		private void InitEffects(Dictionary<int, EffectWorkflowItem> workflow)
		{
			XmlNodeList itemNodes = m_CompositionNode.SelectNodes("items/item");
			foreach (XmlNode itemNode in itemNodes)
			{
				int id = XmlHelper.GetNodeAttributeValueInt(itemNode, "id");
				EffectWorkflowItem item = null;

				string primitiveStr = null;
				string textureStr = null;
				string passStr = null;

				int count = 0;
				if (itemNode.Attributes["primitive"] != null)
				{
					primitiveStr = itemNode.Attributes["primitive"].Value.ToLower();
					count++;
				}
				if (itemNode.Attributes["pass"] != null)
				{
					passStr = itemNode.Attributes["pass"].Value.ToLower();
					count++;
				}
				if (itemNode.Attributes["texture"] != null)
				{
					textureStr = itemNode.Attributes["texture"].Value.ToLower();
					count++;
				}

				if (count > 1)
					throw new FormatException(string.Format("A node '{0}' can contain only one 'primitive', 'pass' or 'texture' attribute", XmlHelper.GetNodeFullPath(m_CompositionNode)));

				int x = XmlHelper.GetNodeAttributeValueInt(itemNode, "x");
				int y = XmlHelper.GetNodeAttributeValueInt(itemNode, "y");

				if (primitiveStr != null)
				{
					if (primitiveStr == "result")
					{
						item = m_WorkflowManager.AddResult(x, y);
						workflow.Add(id, item);
					}
					else if (primitiveStr == "scene")
					{
						item = m_WorkflowManager.AddScene(x, y);
						workflow.Add(id, item);
					}
					else if (primitiveStr == "prevframe")
					{
						item = m_WorkflowManager.AddPreviousFrame(x, y);
						workflow.Add(id, item);
					}
					else
						throw new FormatException(string.Format("Unknown primitive element '{0}' in compoistion node '{1}' id {2}", primitiveStr, this.Name, id));
				}
				else if (textureStr != null)
				{
					foreach (TextureTreeNode textn in GetProjectNode().TexturesTreeNode.Nodes)
					{
						if (textn.TextureRelativeFilename.ToLower() == textureStr)
						{
							item = m_WorkflowManager.AddTexture(textn.TextureRelativeFilename, x, y, textn.Texture);
							workflow.Add(id, item);
							break;
						}
					}
				}
				else if (passStr != null)
				{
					try
					{
						item = InitPass(itemNode, passStr, x, y);
						workflow.Add(id, item);
					}
					catch (Exception ex)
					{
						string msg = string.Format("An error occured while loading effect.\r\n{0}", ex.Message);
						MessageBox.Show(msg, passStr, MessageBoxButtons.OK, MessageBoxIcon.Error);
						continue;
					}
				}

				// common code for item (EffectWorkflowItem)
				item.Unselected();
			}
			m_WorkflowManager.UnselectAllWorkflowItems();
		}

		private EffectWorkflowItem InitPass(XmlNode itemNode, string passStr, int x, int y)
		{
			EffectWorkflowItem item = null;

			foreach (BaseElementTreeNode basetn in GetProjectNode().EffectsTreeNode.Nodes)
			{
				if (basetn is TechniqueTreeNode)
				{
					foreach (PassTreeNode ptn in ((TechniqueTreeNode)basetn).Nodes)
					{
						if (string.Format("{0}.{1}", ptn.Pass.ParentTechnique.Name, ptn.Pass.Name).ToLower() == passStr)
						{
							item = m_WorkflowManager.AddEffect(x, y, ptn);
							break;
						}
					}
				}
				else if (basetn is PassTreeNode)
				{
					PassTreeNode ptn = (PassTreeNode)basetn;
					if (string.Format("{0}.{1}", ptn.Pass.ParentTechnique.Name, ptn.Pass.Name).ToLower() == passStr)
					{
						item = m_WorkflowManager.AddEffect(x, y, ptn);
						break;
					}
				}
			}

			if (item == null)
				throw new FormatException(string.Format("Unknown effect '{0}'", passStr));

			XmlNodeList paramNodes = itemNode.SelectNodes("param");
			foreach (XmlNode paramNode in paramNodes)
			{
				string paramName = XmlHelper.GetNodeAttributeValue(paramNode, "name");
				string paramValue = XmlHelper.GetNodeAttributeValue(paramNode, "value");

				string min = null;
				string max = null;
				if (paramNode.Attributes["min"] != null)
					min = paramNode.Attributes["min"].Value;
				if (paramNode.Attributes["max"] != null)
					max = paramNode.Attributes["max"].Value;

				foreach (IParameterUI iuip in item.UIComponents)
				{
					if (iuip.Name == paramName)
					{
						if (min != null)
							iuip.SetMinimums(min);
						if (max != null)
							iuip.SetMaximums(max);
						iuip.SetValues(paramValue);
					}
				}
			}

			return (item);
		}

		private void InitLinks(Dictionary<int, EffectWorkflowItem> workflow)
		{
			XmlNodeList linkNodes = m_CompositionNode.SelectNodes("links/link");

			int linkError = 0;
			Project project = GetProjectNode();

			foreach (XmlNode linkNode in linkNodes)
			{
				int outIdx = XmlHelper.GetNodeAttributeValueInt(linkNode, "out");
				int inIdx = XmlHelper.GetNodeAttributeValueInt(linkNode, "in");

				if (workflow.ContainsKey(outIdx) == false || workflow.ContainsKey(inIdx) == false)
				{
					linkError++;
					continue;
				}

				EffectWorkflowItem outItem = workflow[outIdx];
				EffectWorkflowItem inItem = workflow[inIdx];

				int paramIdx = 0;

				if (linkNode.Attributes["param"] != null)
				{
					// loads part specific to latest file format
					if (project.LoadedVersion == Project.LatestProjectFileVersion)
					{
						XmlAttribute attr = linkNode.Attributes["param"];
						if (attr == null)
							throw new FormatException("Parameter name is missing in the links definition");
						string paramName = attr.Value;

						paramIdx = Array.FindIndex<LinkBound>(inItem.EffectInputs, delegate(LinkBound cur) { return (cur.FriendlyName == paramName); });
						if (paramIdx < 0)
							throw new FormatException(string.Format("Wrong param name '{0}'", paramName));
					}
					// loads part specific to version 0.1
					else if (project.LoadedVersion == "0.1")
					{
						paramIdx = int.Parse(linkNode.Attributes["param"].Value);
						if (paramIdx < 0)
							throw new FormatException(string.Format("Wrong param index '{0}'", paramIdx));
					}
				}

				m_WorkflowManager.LinkManager.FinalizeLink(inItem.EffectInputs[paramIdx], outItem.EffectOutput);
				m_WorkflowManager.LinkManager.RequestUpdate();
			}

			if (linkError == 1)
				MessageBox.Show("There is 1 link error.\r\nPlease check your workflow(s).", "Link Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			else if (linkError > 1)
				MessageBox.Show(string.Format("There are {0} link errors.\r\nPlease check your workflow(s).", linkError), "Link Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
		}

		public TabPage TabPage
		{
			get
			{
				return (m_TabPage);
			}
		}

		public EffectWorkflowManager WorkflowManager
		{
			get
			{
				return (m_WorkflowManager);
			}
		}

		private void OnEffectEdit(CustomEffect customEffect)
		{
			frmCode editCode = new frmCode(customEffect);
			editCode.Show(GlobalContainer.MainForm);
		}

		private void OnWorkflowItemSelected(EffectWorkflowItem item)
		{
			PropertyGrid grdUIParameters = GlobalContainer.UIParametersPropertyGrid;
			Panel pnlParameterSlider = GlobalContainer.SliderPanel;

			grdUIParameters.SuspendLayout();

			if (item != null)
			{
				grdUIParameters.SelectedObject = item.UIComponents;

				bool nai = (item.UIComponents.Count == 0);
				GlobalContainer.SplitContainerWorkflowParams.Panel2Collapsed = nai;

				if (!nai)
				{
					GlobalContainer.SplitContainerWorkflowParams.SuspendLayout();

					pnlParameterSlider.Left = 0;
					pnlParameterSlider.Top = GlobalContainer.SplitContainerWorkflowParams.Panel2.Height - pnlParameterSlider.Height;
					pnlParameterSlider.Width = GlobalContainer.SplitContainerWorkflowParams.Panel2.Width;
					pnlParameterSlider.Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom;

					grdUIParameters.Location = new Point(0, 0);
					grdUIParameters.Width = GlobalContainer.SplitContainerWorkflowParams.Panel2.Width;
					grdUIParameters.Height = GlobalContainer.SplitContainerWorkflowParams.Panel2.Height - pnlParameterSlider.Height - 6;
					grdUIParameters.Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right | AnchorStyles.Bottom;

					GlobalContainer.SplitContainerWorkflowParams.ResumeLayout();
				}
			}
			else
				GlobalContainer.SplitContainerWorkflowParams.Panel2Collapsed = true;

			grdUIParameters.ResumeLayout();
		}

		public new string Name
		{
			get
			{
				return (m_Name);
			}
			set
			{
				if (value != null)
				{
					// control unicity of name

					m_Name = value;
					this.Text = m_Name;
					if (m_TabPage != null)
						m_TabPage.Text = m_Name;
				}
			}
		}

		public void WriteXml(XmlTextWriter xw)
		{
			EffectWorkflowManager ewm = (EffectWorkflowManager)m_TabPage.Controls["effectWorkflowManager"];

			xw.WriteStartElement("composition");
			xw.WriteAttributeString("name", m_Name);
			{
				WriteWorkflowItems(ewm, xw);
				WriteLinks(ewm, xw);
			}
			xw.WriteEndElement(); // composition
		}

		private void WriteWorkflowItems(EffectWorkflowManager ewm, XmlTextWriter xw)
		{
			xw.WriteStartElement("items");
			{
				int id = 0;
				foreach (EffectWorkflowItem item in ewm.Controls)
				{
					xw.WriteStartElement("item");
					xw.WriteAttributeString("id", id.ToString());

					if (item.Type == PrimitiveItemType.Effect)
						xw.WriteAttributeString("pass", string.Format("{0}.{1}", item.CustomPass.ParentTechnique.Name, item.CustomPass.Name));
					else if (item.Type == PrimitiveItemType.Scene)
						xw.WriteAttributeString("primitive", "scene");
					else if (item.Type == PrimitiveItemType.PreviousFrame)
						xw.WriteAttributeString("primitive", "prevframe");
					else if (item.Type == PrimitiveItemType.Output)
						xw.WriteAttributeString("primitive", "result");
					else if (item.Type == PrimitiveItemType.Texture)
						xw.WriteAttributeString("texture", item.TextureFilename);

					xw.WriteAttributeString("x", item.Left.ToString());
					xw.WriteAttributeString("y", item.Top.ToString());

					if (item.Type == PrimitiveItemType.Effect)
					{
						foreach (IParameterUI param in item.UIComponents)
						{
							xw.WriteStartElement("param");
							xw.WriteAttributeString("name", param.Name);
							xw.WriteAttributeString("value", param.GetValues());
							xw.WriteAttributeString("min", param.GetMinimums());
							xw.WriteAttributeString("max", param.GetMaximums());
							xw.WriteEndElement();
						}
					}

					xw.WriteEndElement(); // item

					id++;
				}
			}
			xw.WriteEndElement(); // items
		}

		private void WriteLinks(EffectWorkflowManager ewm, XmlTextWriter xw)
		{
			xw.WriteStartElement("links");
			{
				int id = 0;
				foreach (Link link in ewm.LinkManager.Links)
				{
					xw.WriteStartElement("link");
					{
						int outItemIndex = GetIndexOfEffectWorkflowItem(link.Output, ewm);
						int inItemIndex = GetIndexOfEffectWorkflowItem(link.Input, ewm);

						xw.WriteAttributeString("out", outItemIndex.ToString());
						xw.WriteAttributeString("in", inItemIndex.ToString());

						string test = link.Input.FriendlyName;
						if (test.StartsWith("[Result]"))
						{
							test = link.Input.FriendlyName;
						}

						if (((EffectWorkflowItem)ewm.Controls[inItemIndex]).Type == PrimitiveItemType.Effect)
							xw.WriteAttributeString("param", link.Input.FriendlyName);
					}
					xw.WriteEndElement(); // link

					id++;
				}
			}
			xw.WriteEndElement(); // links
		}

		private int GetIndexOfEffectWorkflowItem(LinkBound lb, EffectWorkflowManager ewm)
		{
			int i = 0;
			foreach (EffectWorkflowItem item in ewm.Controls)
			{
				if (item.GetIndexOfLinkBound(lb) >= 0)
					return (i);
				i++;
			}
			return (-1);
		}
	}


	public class CompositionTreeNodeSorter : IComparer<CompositionTreeNode>
	{
		public int Compare(CompositionTreeNode x, CompositionTreeNode y)
		{
			return (x.Text.CompareTo(y.Text));
		}
	}
}
