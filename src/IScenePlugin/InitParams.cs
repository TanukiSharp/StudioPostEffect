using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using Microsoft.DirectX.Direct3D;

namespace IScenePlugin
{
	public interface ICustomRenderTiming : IRenderable
	{
		bool NeedCustomRenderTiming { get; set; }
	}

	public class ScenePluginInitParams
	{
		public ICustomRenderTiming CustomRenderTiming;
		public Device Device;
		public ICamera Camera;
		public IRenderable Grid;
	}
}
