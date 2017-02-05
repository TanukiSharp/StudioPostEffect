using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using StudioPostEffect.ProjectLoader.Interfaces;

namespace StudioPostEffect.ProjectLoader.Implementations
{
	internal class Project : IProject
	{
		private string m_Filename;
		private Version m_Version;

		internal Project(string xmlFilename)
		{
			m_Filename = xmlFilename;

			XmlDocument doc = new XmlDocument();

			doc.Load(m_Filename);
			XmlNode root = doc.DocumentElement;
			LoadXml(root);
		}

		private void LoadXml(XmlNode root)
		{
			if (root.Name != "efxprj")
				throw new ProjectLoadingException("Not a valid Studio Post-Effect project file.");

			XmlAttribute attr = root.Attributes["version"];
			if (attr == null)
				throw new ProjectLoadingException("This project file does not have version number.");

			m_Version = new Version(attr.Value);
		}


		public Version FileVersion
		{
			get { throw new Exception("The method or operation is not implemented."); }
		}

		public string[] CompositionNames
		{
			get { throw new Exception("The method or operation is not implemented."); }
		}

		public IComposition[] Compositions
		{
			get { throw new Exception("The method or operation is not implemented."); }
		}

		public IComposition FindComposition(string compositionName)
		{
			throw new Exception("The method or operation is not implemented.");
		}
	}
}
