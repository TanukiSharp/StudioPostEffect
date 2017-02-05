using System;
using System.Collections.Generic;
using System.Text;
using StudioPostEffect.ProjectLoader.Interfaces;
using StudioPostEffect.ProjectLoader.Implementations;

namespace StudioPostEffect.ProjectLoader
{
	public static class Loader
	{
		public static IProject LoadProject(string xmlFilename)
		{
			Project project = new Project(xmlFilename);
			return (project);
		}
	}
}
