using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.DirectX.Direct3D;
using System.Windows.Forms;
using System.Drawing;

namespace InternalEffect
{
	public class CustomParameter : CustomBaseElement
	{
		private ParameterClass m_Class;
		private ParameterType m_Type;
		private CustomEffect m_Parent;

		internal CustomParameter(Effect effect, EffectHandle paramHandle, CustomEffect parentEffect)
			: base(effect)
		{
			m_Parent = parentEffect;
			m_Handle = paramHandle;
			ParameterDescription desc = m_Effect.GetParameterDescription(m_Handle);
			m_Name = desc.Name;
			m_Class = desc.Class;
			m_Type = desc.Type;
		}

		public CustomEffect ParentEffect
		{
			get
			{
				return (m_Parent);
			}
		}

		public ParameterClass Class
		{
			get
			{
				return (m_Class);
			}
		}

		public ParameterType Type
		{
			get
			{
				return (m_Type);
			}
		}
	}
}
