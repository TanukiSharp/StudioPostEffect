using System;
using System.Collections.Generic;
using System.Text;
using StudioPostEffect.ProjectLoader.Interfaces;

namespace StudioPostEffect.ProjectLoader.Implementations
{
	internal class Link : ILink
	{
		#region ILink Members

		public ILinkable Output
		{
			get { throw new Exception("The method or operation is not implemented."); }
		}

		public ILinkable Input
		{
			get { throw new Exception("The method or operation is not implemented."); }
		}

		#endregion
	}
}
