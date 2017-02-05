using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.IO;
using Microsoft.DirectX.Direct3D;
using PostEffectCore;

namespace InternalEffect
{
	public class Project : BaseTreeNode
	{
		public const string LatestProjectFileVersion = "0.2";

		public delegate void ProjectModifiedHandler(Project sender);
		public event ProjectModifiedHandler ProjectModified;

		private string m_ProjectFilename;
		private string m_ProjectDir;
		private bool m_IsModified;

		private CompositionsTreeNode m_CompositionsTreeNode;
		private EffectsTreeNode m_EffectsTreeNode;
		private TexturesTreeNode m_TexturesTreeNode;

		private Device m_Device;

		private Project(Device device, string filename)
		{
			ProjectFilename = filename;
			m_Device = device;

			this.Text = "Project";
			this.ImageIndex = 0;
			this.SelectedImageIndex = 0;

			m_CompositionsTreeNode = new CompositionsTreeNode();
			m_EffectsTreeNode = new EffectsTreeNode();
			m_TexturesTreeNode = new TexturesTreeNode();
		}

		public override void Initialize()
		{
			AddNode(m_CompositionsTreeNode);
			AddNode(m_EffectsTreeNode);
			AddNode(m_TexturesTreeNode);

			if (m_New)
			{
				LoadDefaultProjectSettings();
				Save();
			}
			else
			{
				LoadProject();
			}
		}

		//------------------------------------------------------------------------------

		private bool m_New;

		public static Project CreateWithNewFile(Device device, string xmlProjectFilename)
		{
			CheckFile(xmlProjectFilename);
			Project prj = new Project(device, xmlProjectFilename);
			prj.m_New = true;
			return (prj);
		}

		public static Project CreateFromXmlProjectFile(Device device, string xmlProjectFilename)
		{
			CheckFile(xmlProjectFilename);
			Project prj = new Project(device, xmlProjectFilename);
			prj.m_New = false;
			return (prj);
		}

		//------------------------------------------------------------------------------

		private static void CheckFile(string xmlProjectFilename)
		{
			if (xmlProjectFilename == null)
				throw new ArgumentException("Invalid argument", "xmlProjectFilename");

			if (File.Exists(xmlProjectFilename) == false)
				throw new FileNotFoundException(string.Format("Impossible to find file '{0}'.", xmlProjectFilename));
		}

		internal string AttachFileToProject(string fullFilename, string folder)
		{
			if (folder == null)
				folder = "";
			string projectLocationDir = Path.GetFullPath(string.Format("{0}\\{1}", m_ProjectDir, folder));

			int idx = StringHelper.StrCmp(m_ProjectDir, fullFilename, true);

			string resultingFilename;

			if (idx >= m_ProjectDir.Length)
			{
				resultingFilename = fullFilename.Substring(m_ProjectDir.Length).TrimStart('\\');
			}
			else
			{
				string filenameOnly = Path.GetFileName(fullFilename);
				string newFile = Path.GetFullPath(string.Format("{0}\\{1}", projectLocationDir, filenameOnly));
				string tmpRelativeFilename = newFile.Substring(m_ProjectDir.Length).TrimStart('\\');

				DialogResult dlgres = DialogResult.Yes;

				if (File.Exists(newFile))
				{
					string msg = string.Format("The file at location '{0}' is outside the project scope, and so is about to be copied to project folder.\r\nHowever the file '{1}' already exists.\r\n\r\nDo you want to overwite this file ?", fullFilename, newFile);
					dlgres = MessageBox.Show(msg, "Overwrite file ?", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
				}

				if (dlgres == DialogResult.Yes)
				{
					Directory.CreateDirectory(Path.GetDirectoryName(newFile));
					File.Copy(fullFilename, newFile, true);
					resultingFilename = tmpRelativeFilename;
				}
				else if (dlgres == DialogResult.No)
					resultingFilename = tmpRelativeFilename;
				else
					return (null);
			}

			resultingFilename = resultingFilename.Replace('\\', '/');
			return (resultingFilename);
		}


		public CompositionsTreeNode CompositionsTreeNode
		{
			get
			{
				return (m_CompositionsTreeNode);
			}
		}

		public EffectsTreeNode EffectsTreeNode
		{
			get
			{
				return (m_EffectsTreeNode);
			}
		}

		public TexturesTreeNode TexturesTreeNode
		{
			get
			{
				return (m_TexturesTreeNode);
			}
		}


		public void LoadDefaultProjectSettings()
		{
			/*
			m_GridSize = 32;
			m_Zoom = 1;
			m_ShowGrid = true;
			m_ShowElemFrames = true;
			m_ShowFramesNumbers = true;
			m_ShowCollisionsBoxes = true;
			*/
		}

		private string m_LoadedVersion;
		public string LoadedVersion
		{
			get
			{
				return (m_LoadedVersion);
			}
		}

		private void LoadProject()
		{
			XmlDocument doc = new XmlDocument();
			doc.Load(m_ProjectFilename);

			m_LoadedVersion = XmlHelper.GetNodeAttributeValue(doc.DocumentElement, "version");

			//Project

			LoadEffects(doc);
			LoadTextures(doc);
			LoadCompositions(doc);
		}

		private void LoadEffects(XmlDocument doc)
		{
			XmlNodeList effectNodes = doc.SelectNodes("efxprj/effects/effect");
			if (effectNodes == null || effectNodes.Count == 0)
				return;

			List<string> unloadableEffects = new List<string>();

			foreach (XmlNode effectNode in effectNodes)
			{
				string effectFilename = XmlHelper.GetNodeAttributeValue(effectNode, "file");
				string effectFullFilename = string.Format("{0}\\{1}", m_ProjectDir, effectFilename);
				if (File.Exists(effectFullFilename) == false)
					unloadableEffects.Add(effectFilename);

				CustomEffect efx = null;

				try
				{
					efx = new CustomEffect(m_Device, effectFullFilename, effectFilename);
					efx.Reloaded += new EventHandler(OnEffectReloaded);
				}
				catch (Exception ex)
				{
					MessageBox.Show(ex.Message, effectFilename);
					unloadableEffects.Add(effectFilename);
					continue;
				}

				foreach (CustomTechnique tech in efx.Techniques)
				{
					if (tech.Passes.Length == 1)
						m_EffectsTreeNode.AddNode(new PassTreeNode(tech.Passes[0]));
					else
						m_EffectsTreeNode.AddNode(new TechniqueTreeNode(tech));
				}
			}
		}

		internal void OnEffectReloaded(object sender, EventArgs e)
		{
			this.TreeView.SuspendLayout();

			CustomEffect effect = (CustomEffect)sender;

			string filename = effect.Filename.ToLower();
			string curFilename;
			int firstIndex = -1;

			List<BaseElementTreeNode> removeList = new List<BaseElementTreeNode>();
			foreach (BaseElementTreeNode basetn in m_EffectsTreeNode.Nodes)
			{
				curFilename = null;
				if (basetn is PassTreeNode)
					curFilename = ((PassTreeNode)basetn).Pass.ParentTechnique.ParentEffect.Filename.ToLower();
				else if (basetn is TechniqueTreeNode)
					curFilename = ((TechniqueTreeNode)basetn).Technique.ParentEffect.Filename.ToLower();

				if (filename == curFilename)
				{
					if (firstIndex == -1)
						firstIndex = basetn.Index;
					removeList.Add(basetn);
				}
			}

			foreach (BaseElementTreeNode basetn in removeList)
				m_EffectsTreeNode.Nodes.Remove(basetn);

			foreach (CustomTechnique tech in effect.Techniques)
			{
				BaseElementTreeNode basetn;
				if (tech.Passes.Length == 1)
					basetn = new PassTreeNode(tech.Passes[0]);
				else
					basetn = new TechniqueTreeNode(tech);

				m_EffectsTreeNode.Nodes.Insert(firstIndex, basetn);
				firstIndex++;
				basetn.Initialize();
			}

			this.TreeView.ResumeLayout();
		}

		private void LoadTextures(XmlDocument doc)
		{
			XmlNodeList textureNodes = doc.SelectNodes("efxprj/textures/texture");
			if (textureNodes == null || textureNodes.Count == 0)
				return;

			List<string> unloadableTextures = new List<string>();

			foreach (XmlNode textureNode in textureNodes)
			{
				string textureFilename = XmlHelper.GetNodeAttributeValue(textureNode, "file");

				string file = Path.Combine(m_ProjectDir, textureFilename);
				if (File.Exists(file) == false)
					unloadableTextures.Add(file);
				else
				{
					try
					{
						Texture tex = TextureLoader.FromFile(m_Device, file);
						m_TexturesTreeNode.AddNode(new TextureTreeNode(m_ProjectDir, textureFilename));
					}
					catch
					{
						unloadableTextures.Add(file);
					}
				}
			}
		}

		private void LoadCompositions(XmlDocument doc)
		{
			XmlNodeList compositionNodes = doc.SelectNodes("efxprj/compositions/composition");
			if (compositionNodes == null || compositionNodes.Count == 0)
				return;

			List<CompositionTreeNode> compositions = new List<CompositionTreeNode>();
			foreach (XmlNode compositionNode in compositionNodes)
				compositions.Add(new CompositionTreeNode(compositionNode));
			compositions.Sort(new CompositionTreeNodeSorter());

			foreach (CompositionTreeNode compositionTreeNode in compositions)
				m_CompositionsTreeNode.AddNode(compositionTreeNode);
		}

		/*
		private void LoadOptions(XmlDocument doc)
		{
			XmlNode node;
			
			node = XmlHelper.GetNode(doc, "ssp/options/gridsize");
			int gridSize = XmlHelper.GetNodeAttributeValueInt(node, "value");

			node = XmlHelper.GetNode(doc, "ssp/options/zoom");
			int zoom = XmlHelper.GetNodeAttributeValueInt(node, "value");

			node = XmlHelper.GetNode(doc, "ssp/options/showgrid");
			bool showGrid = XmlHelper.GetNodeAttributeValueBoolean(node, "value");

			node = XmlHelper.GetNode(doc, "ssp/options/showelemframes");
			bool showElemFrames = XmlHelper.GetNodeAttributeValueBoolean(node, "value");

			node = XmlHelper.GetNode(doc, "ssp/options/showframesnumbers");
			bool showFramesNumbers = XmlHelper.GetNodeAttributeValueBoolean(node, "value");

			node = XmlHelper.GetNode(doc, "ssp/options/showcollisionbox");
			bool showCollisionBoxes = XmlHelper.GetNodeAttributeValueBoolean(node, "value");

			GridSize = gridSize;
			Zoom = zoom;
			ShowGrid = showGrid;
			ShowElemFrames = showElemFrames;
			ShowFramesNumbers = showFramesNumbers;
			ShowCollisionsBoxes = showCollisionBoxes;
		}
		*/

		/*
		private void LoadTexturesData(XmlDocument doc)
		{
			XmlNodeList textureNodes = doc.SelectNodes("ssp/textures/texture");
			if (textureNodes == null || textureNodes.Count == 0)
				return;

			foreach (XmlNode textureNode in textureNodes)
				Nodes.Add(new TextureTreeNode(m_ProjectDir, textureNode));
		}
		*/

		public string ProjectFilename
		{
			get
			{
				return (m_ProjectFilename);
			}
			internal set
			{
				m_ProjectFilename = value;
				m_ProjectDir = Path.GetDirectoryName(m_ProjectFilename);
			}
		}

		public string ProjectDirectory
		{
			get
			{
				return (m_ProjectDir);
			}
		}

		public Device Device
		{
			get
			{
				return (m_Device);
			}
		}

		public bool IsModified
		{
			get
			{
				return (m_IsModified);
			}
			set
			{
				if (m_IsModified != value)
				{
					m_IsModified = value;
					if (ProjectModified != null)
						ProjectModified(this);
				}
			}
		}

		private void WriteEffects(XmlTextWriter xw)
		{
			xw.WriteStartElement("effects");

			List<string> done = new List<string>();

			foreach (BaseElementTreeNode node in m_EffectsTreeNode.Nodes)
			{
				string file = null;
				if (node is PassTreeNode)
					file = ((PassTreeNode)node).Pass.ParentTechnique.ParentEffect.RelativeEffectFilename;
				else // if (node is TechniqueTreeNode)
					file = ((TechniqueTreeNode)node).Technique.ParentEffect.RelativeEffectFilename;

				if (done.Contains(file) == false)
				{
					done.Add(file);

					xw.WriteStartElement("effect");
					xw.WriteAttributeString("file", file);
					xw.WriteEndElement();
				}
			}

			xw.WriteEndElement(); // effects
		}

		private void WriteTextures(XmlTextWriter xw)
		{
			xw.WriteStartElement("textures");

			foreach (TextureTreeNode node in m_TexturesTreeNode.Nodes)
				node.WriteXml(xw);

			xw.WriteEndElement(); // textures
		}

		private void WriteWorkflows(XmlTextWriter xw)
		{
			xw.WriteStartElement("compositions");

			foreach (CompositionTreeNode compo in m_CompositionsTreeNode.Nodes)
				compo.WriteXml(xw);

			xw.WriteEndElement(); // compositions
		}

		public bool Save()
		{
			XmlTextWriter xw = null;

			try
			{
				xw = new XmlTextWriter(m_ProjectFilename, Encoding.UTF8);
				xw.Formatting = Formatting.Indented;
				xw.Indentation = 1;
				xw.IndentChar = '\t';

				xw.WriteStartDocument();
				xw.WriteStartElement("efxprj");
				xw.WriteAttributeString("version", LatestProjectFileVersion);

				WriteWorkflows(xw);
				WriteEffects(xw);
				WriteTextures(xw);

				xw.WriteEndElement();
				xw.WriteEndDocument();

				xw.Close();

				IsModified = false;
				m_LoadedVersion = LatestProjectFileVersion;

				return (true);
			}
			catch (Exception ex)
			{
				if (xw != null)
					xw.Close();
				MessageBox.Show(string.Format("An error occured while saving project.\r\n\r\n{0}", ex.Message), "Save Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return (false);
			}
		}

		public bool SaveAs(string filename)
		{
			string tmp = m_ProjectFilename;
			ProjectFilename = filename;

			bool saveStatus = Save();
			if (saveStatus == false)
				ProjectFilename = tmp;

			return (saveStatus);
		}
	}
}
