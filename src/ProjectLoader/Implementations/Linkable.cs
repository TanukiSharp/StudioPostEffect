using System;
using System.Collections.Generic;
using System.Text;
using StudioPostEffect.ProjectLoader.Interfaces;

namespace StudioPostEffect.ProjectLoader.Implementations
{
	internal class Linkable : ILinkable
	{
		#region ILinkable Members

		public IWorkflowItem ParentWorkflowItem
		{
			get { throw new Exception("The method or operation is not implemented."); }
		}

		public ILinkable[] LinkedItems
		{
			get { throw new Exception("The method or operation is not implemented."); }
		}

		public ILinkable FindLinkedItem(string itemName)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		#endregion
	}
}
