using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing.Design;
using System.ComponentModel;
using System.Windows.Forms.Design;

namespace InternalEffect.UIParameters
{
	/*
	public class MousePadEditor : UITypeEditor
	{
		private ITypeDescriptorContext m_Context = null;

		public MousePadEditor()
		{
		}

		public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
		{
			return (UITypeEditorEditStyle.DropDown);
		}

		public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
		{
			Type type = value.GetType();

			if (type == typeof(MousePadSelector))
			{
				MousePadSelector t = (MousePadSelector)value;

				IWindowsFormsEditorService editorService = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
				if (editorService != null)
				{
					m_Context = context;
					editorService.DropDownControl(t.InternalControl);

					return (t.InternalControl.Coordinates);
				}
			}

			m_Context = null;
			return (value);
		}
	}
	*/
}
