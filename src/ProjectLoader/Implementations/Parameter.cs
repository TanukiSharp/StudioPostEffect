using System;
using System.Collections.Generic;
using System.Text;
using StudioPostEffect.ProjectLoader.Interfaces;

namespace StudioPostEffect.ProjectLoader.Implementations
{
	internal class Parameter : IParameter
	{
		#region IParameter Members

		public string Name
		{
			get { throw new Exception("The method or operation is not implemented."); }
		}

		public string[] Values
		{
			get { throw new Exception("The method or operation is not implemented."); }
		}

		public string[] Minimums
		{
			get { throw new Exception("The method or operation is not implemented."); }
		}

		public string[] Maximums
		{
			get { throw new Exception("The method or operation is not implemented."); }
		}

		#endregion

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
