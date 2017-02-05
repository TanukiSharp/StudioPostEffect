using System;
using System.Collections.Generic;
using System.Text;
using IScenePlugin;
using System.Windows.Forms;

namespace StudioPostEffect
{
	internal class CustomRender : ICustomRenderTiming
	{
		private bool m_NeedCustomRenderTiming;
		private MethodInvoker m_RenderMethod;

		internal CustomRender(MethodInvoker renderMethod)
		{
			m_RenderMethod = renderMethod;
		}

		public bool NeedCustomRenderTiming
		{
			get
			{
				return (m_NeedCustomRenderTiming);
			}
			set
			{
				m_NeedCustomRenderTiming = value;
			}
		}

		public void Render()
		{
			m_RenderMethod();
		}
	}
}
