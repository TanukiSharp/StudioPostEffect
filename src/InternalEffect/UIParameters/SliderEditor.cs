using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing.Design;
using System.ComponentModel;
using System.Windows.Forms.Design;
using System.Windows.Forms;
using System.Drawing;

namespace InternalEffect
{
	public class SliderEditor : UITypeEditor
	{
		private ITypeDescriptorContext m_Context = null;

		public SliderEditor()
		{
		}

		public override void PaintValue(PaintValueEventArgs e)
		{
			if (e.Value is FloatSlider)
			{
				FloatSlider fsl = (FloatSlider)e.Value;
				int c = (int)(fsl.Coeficient * 255.0f);
				e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(c, c, c)), e.Bounds);
			}
			else if (e.Value is IntegerSlider)
			{
				IntegerSlider isl = (IntegerSlider)e.Value;
				float coef = (float)(isl.Value - isl.Minimum) / (float)(isl.Maximum - isl.Minimum);
				coef = Math.Max(0.0f, Math.Min(coef, 1.0f));
				int c = (int)(coef * 255.0f);
				e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(c, c, c)), e.Bounds);
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
			return (UITypeEditorEditStyle.None);
		}

		public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
		{
			Type type = value.GetType();

			if (type == typeof(FloatSlider))
			{
				FloatSlider t = (FloatSlider)value;

				IWindowsFormsEditorService editorService = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
				if (editorService != null)
				{
					m_Context = context;
					editorService.DropDownControl(t.InternalControl);

					return (t.InternalControl.ValueFloat);
				}
			}
			else if (type == typeof(IntegerSlider))
			{
				IntegerSlider t = (IntegerSlider)value;

				IWindowsFormsEditorService editorService = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
				if (editorService != null)
				{
					m_Context = context;
					editorService.DropDownControl(t.InternalControl);
					return (t.InternalControl.ValueInteger);
				}
			}

			m_Context = null;
			return (value);
		}
	}
}
