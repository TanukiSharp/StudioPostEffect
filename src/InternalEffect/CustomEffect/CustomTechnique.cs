using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.DirectX.Direct3D;

namespace InternalEffect
{
	public class CustomTechnique : CustomBaseElement
	{
		private CustomEffect m_Parent;
		private CustomPass[] m_Passes;

		internal CustomTechnique(Effect effect, EffectHandle techHandle, CustomEffect parentEffect)
			: base(effect)
		{
			// parent CustomEffect
			m_Parent = parentEffect;
			// technique handle
			m_Handle = techHandle;
			// technique description
			TechniqueDescription desc = m_Effect.GetTechniqueDescription(m_Handle);
			// technique name
			m_Name = desc.Name;

			// passes
			List<CustomPass> list = new List<CustomPass>();
			for (int i = 0; i < desc.Passes; i++)
				list.Add(new CustomPass(m_Effect, i, m_Effect.GetPass(m_Handle, i), this));
			// build pass array now because it will not change and so it will be faster when calling Passes property
			m_Passes = list.ToArray();
		}

		/// <summary>
		/// Get parent element (CustomEffect)
		/// </summary>
		public CustomEffect ParentEffect
		{
			get
			{
				return (m_Parent);
			}
		}

		/// <summary>
		/// Get Passes of the current Technique
		/// </summary>
		public CustomPass[] Passes
		{
			get
			{
				return (m_Passes);
			}
		}

		public CustomPass FindPass(string name)
		{
			return (CustomEffect.FindElement(m_Passes, name) as CustomPass);
		}

		public override string ToString()
		{
			return (string.Format("{0} - {1}", m_Parent.ToString(), m_Name));
		}
	}
}
