using System;
using System.Collections.Generic;
using System.Text;

namespace StudioPostEffect.ProjectLoader.Interfaces
{
	public interface IParameter : ILinkable
	{
		string Name { get; }
		string[] Values { get; }
		string[] Minimums { get; }
		string[] Maximums { get; }
	}
}
