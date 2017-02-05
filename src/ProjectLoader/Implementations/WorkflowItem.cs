using System;
using System.Collections.Generic;
using System.Text;
using StudioPostEffect.ProjectLoader.Interfaces;

namespace StudioPostEffect.ProjectLoader.Implementations
{
	internal class WorkflowItem : IWorkflowItem
	{
		#region IWorkflowItem Members

		public IComposition ParentComposition
		{
			get { throw new Exception("The method or operation is not implemented."); }
		}

		public WorkflowItemType Type
		{
			get { throw new Exception("The method or operation is not implemented."); }
		}

		public System.Drawing.Point Position
		{
			get { throw new Exception("The method or operation is not implemented."); }
		}

		public string TextureName
		{
			get { throw new Exception("The method or operation is not implemented."); }
		}

		public string EffectName
		{
			get { throw new Exception("The method or operation is not implemented."); }
		}

		public IParameter[] Inputs
		{
			get { throw new Exception("The method or operation is not implemented."); }
		}

		public ILinkable Output
		{
			get { throw new Exception("The method or operation is not implemented."); }
		}

		#endregion
	}
}
