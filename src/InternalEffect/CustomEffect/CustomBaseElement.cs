using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.DirectX.Direct3D;

namespace InternalEffect
{
	public class CustomBaseElement
	{
		protected Effect m_Effect;
		protected EffectHandle m_Handle;
		protected string m_Name;

		internal CustomBaseElement(Effect effect)
		{
			m_Effect = effect;
		}

		public string Name
		{
			get
			{
				return (m_Name);
			}
		}

		public Effect Effect
		{
			get
			{
				return (m_Effect);
			}
		}

		public EffectHandle Handle
		{
			get
			{
				return (m_Handle);
			}
		}

		public override string ToString()
		{
			return (m_Name);
		}
	}
}
