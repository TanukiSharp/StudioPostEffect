using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace StudioPostEffect.ProjectLoader.Interfaces
{
	public enum WorkflowItemType
	{
		Effect,
		Scene,
		PreviousFrame,
		Texture,
		Output,
	}

	public interface IWorkflowItem
	{
		IComposition ParentComposition { get; }
		WorkflowItemType Type { get; }
		Point Position { get; }
		string TextureName { get; }
		string EffectName { get; }
		IParameter[] Inputs { get; }
		ILinkable Output { get; }
	}
}
