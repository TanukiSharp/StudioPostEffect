using System;
using System.Collections.Generic;
using System.Text;
using StudioPostEffect.ProjectLoader.Interfaces;

namespace StudioPostEffect.ProjectLoader
{
	internal class Composition : IComposition
	{
		#region IComposition Members

		public IProject ParentProject
		{
			get { throw new Exception("The method or operation is not implemented."); }
		}

		public string Name
		{
			get { throw new Exception("The method or operation is not implemented."); }
		}

		public IWorkflowItem[] WorkflowItems
		{
			get { throw new Exception("The method or operation is not implemented."); }
		}

		public ILink[] Links
		{
			get { throw new Exception("The method or operation is not implemented."); }
		}

		#endregion
	}
}
