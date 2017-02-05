using System;
using System.Collections.Generic;
using System.Text;

namespace StudioPostEffect.ProjectLoader.Interfaces
{
	public interface IProject
	{
		Version FileVersion { get; }
		string[] CompositionNames { get; }
		IComposition[] Compositions { get; }
		IComposition FindComposition(string compositionName);
	}
}
