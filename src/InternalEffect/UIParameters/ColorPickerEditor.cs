using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing.Design;
using System.ComponentModel;
using HDRColorPicker;
using System.Drawing;
using System.Windows.Forms.Design;

namespace InternalEffect.UIParameters
{
	public class ColorPickerEditor : UITypeEditor
	{
		private ITypeDescriptorContext m_Context = null;

		public ColorPickerEditor()
		{
		}

		public override void PaintValue(PaintValueEventArgs e)
		{
			if (e.Value is ColorSelector)
			{
				ColorSelector colorPicker = (ColorSelector)e.Value;
				Color c = Color.FromArgb(
					(int)(colorPicker.Value.A * 255.0f),
					(int)(colorPicker.Value.R * 255.0f),
					(int)(colorPicker.Value.G * 255.0f),
					(int)(colorPicker.Value.B * 255.0f));
				e.Graphics.FillRectangle(new SolidBrush(c), e.Bounds);
			}

			base.PaintValue(e);
		}

		public override bool GetPaintValueSupported(ITypeDescriptorContext context)
		{
			if (context == null)
				return (false);

			if (context.Instance is NotEditablePropertyCollection<IParameterUI>)
				return (true);

			return (base.GetPaintValueSupported(context));
		}

		public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
		{
			return (UITypeEditorEditStyle.DropDown);
		}

		public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
		{
			Type type = value.GetType();

			if (type == typeof(ColorSelector))
			{
				ColorSelector t = (ColorSelector)value;

				IWindowsFormsEditorService editorService = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
				if (editorService != null)
				{
					m_Context = context;
					editorService.DropDownControl(t.InternalControl);
				}
			}

			m_Context = null;
			return (value);
		}
	}
}
