using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Microsoft.DirectX.Direct3D;

namespace InternalEffect
{
	public enum IOMode
	{
		//Unset,
		Input,
		Output,
	}

	public partial class LinkBound : Label
	{
		private LinkManager m_LinkManager;

		private IOMode m_IOMode;
		private CustomParameter m_Parameter;

		private LinkBound m_OtherBound;
		private EffectWorkflowItem m_AttachedObject;

		private string m_Name;

		private Color m_NormalColor;
		private Color m_SelectionColor;

		public LinkBound()
		{
			m_Name = ControlNaming.GetNextName<LinkBound>();
			m_IOMode = IOMode.Output;
			m_Parameter = null;
		}

		public LinkBound(IOMode ioMode, CustomParameter customParameter)
		{
			if (customParameter == null)
				m_Name = ControlNaming.GetNextName<LinkBound>();
			else
				m_Name = customParameter.Name;
			m_IOMode = ioMode;
			m_Parameter = customParameter;
		}

		public new string Name
		{
			get
			{
				if (this.Parent == null || this.Parent is EffectWorkflowManager)
					return (string.Format("[{0}]", m_Name));

				return (string.Format("[{0}].[{1}]", this.Parent.Parent.Name, m_Name));
			}
			set
			{
				m_Name = value;
			}
		}

		public string FriendlyName
		{
			get
			{
				if (this.Parent == null || this.Parent is EffectWorkflowManager)
					return (m_Name);

				return (string.Format("{0}.{1}", this.Parent.Parent.Name, m_Name));
			}
			set
			{
				m_Name = value;
			}
		}

		public void SetLinkManager(LinkManager linkManager)
		{
			m_LinkManager = linkManager;
		}

		public void SetBackColor(Color color)
		{
			this.BackColor = color;
			m_NormalColor = color;
			m_SelectionColor = DarkenColor(m_NormalColor);
		}

		public Color SelectionColor
		{
			get
			{
				return (m_SelectionColor);
			}
			set
			{
				m_SelectionColor = value;
			}
		}

		public LinkBound OtherBound
		{
			get
			{
				return (m_OtherBound);
			}
			set
			{
				m_OtherBound = value;
			}
		}

		public EffectWorkflowItem AttachedObject
		{
			get
			{
				return (m_AttachedObject);
			}
			set
			{
				m_AttachedObject = value;
			}
		}

		protected override void OnMouseEnter(EventArgs e)
		{
			base.OnMouseEnter(e);
			Select();
		}

		protected override void OnMouseLeave(EventArgs e)
		{
			base.OnMouseLeave(e);
			Unselect();
		}

		public new void Select()
		{
			this.BackColor = m_SelectionColor;
		}

		public void Unselect()
		{
			this.BackColor = m_NormalColor;
		}

		private Color DarkenColor(Color color)
		{
			return (Color.FromArgb(
				Math.Max(color.R - 50, 0),
				Math.Max(color.G - 50, 0),
				Math.Max(color.B - 50, 0)));
		}

		public IOMode IOMode
		{
			get
			{
				return (m_IOMode);
			}
		}

		public CustomParameter Parameter
		{
			get
			{
				return (m_Parameter);
			}
		}
	}
}
