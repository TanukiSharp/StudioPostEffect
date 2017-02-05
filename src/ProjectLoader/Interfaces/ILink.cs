using System;
using System.Collections.Generic;
using System.Text;

namespace StudioPostEffect.ProjectLoader.Interfaces
{
	public interface ILink
	{
		ILinkable Output { get; }
		ILinkable Input { get; }
	}
}
