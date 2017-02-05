using System;
using System.Collections.Generic;
using System.Text;

namespace StudioPostEffect.ProjectLoader.Interfaces
{
	public interface ILinkable
	{
		IWorkflowItem ParentWorkflowItem { get; }
		ILinkable[] LinkedItems { get; }
		ILinkable FindLinkedItem(string itemName);
	}
}
