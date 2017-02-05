using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace InternalEffect
{
	public class BaseElementTreeNode : BaseTreeNode
	{
		protected CustomBaseElement m_Element;

		public BaseElementTreeNode(CustomBaseElement element)
		{
			m_Element = element;
			this.Text = m_Element.Name;
		}

		public override void Initialize()
		{
		}

		public CustomBaseElement Element
		{
			get
			{
				return (m_Element);
			}
		}
	}
}
