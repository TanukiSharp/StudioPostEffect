using System;
using System.Collections.Generic;
using System.Text;

namespace StudioPostEffect.ProjectLoader
{
	public class ProjectLoadingException : Exception
	{
		internal ProjectLoadingException(string message)
			: base(message)
		{
		}
	}
}
