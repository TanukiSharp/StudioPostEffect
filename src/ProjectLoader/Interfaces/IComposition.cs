using System;
using System.Collections.Generic;
using System.Text;

namespace StudioPostEffect.ProjectLoader.Interfaces
{
	public interface IComposition
	{
		IProject ParentProject { get; }
		string Name { get; }
		IWorkflowItem[] WorkflowItems { get; }
		ILink[] Links { get; }
	}
}
