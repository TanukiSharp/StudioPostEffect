using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.DirectX.Direct3D;

namespace InternalEffect
{
	public class CustomPass : CustomBaseElement
	{
		private CustomTechnique m_Parent;
		private int m_PassNumber;

		internal CustomPass(Effect effect, int number, EffectHandle passHandle, CustomTechnique parentTechnique)
			: base(effect)
		{
			m_Parent = parentTechnique;
			m_Handle = passHandle;
			PassDescription desc = m_Effect.GetPassDescription(m_Handle);
			m_Name = desc.Name;
			m_PassNumber = number;
		}

		public CustomTechnique ParentTechnique
		{
			get
			{
				return (m_Parent);
			}
		}

		public int PassNumber
		{
			get
			{
				return (m_PassNumber);
			}
		}

		public override string ToString()
		{
			return (string.Format("{0} - {1}", m_Parent.ToString(), m_Name));
		}
	}
}
