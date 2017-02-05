using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Drawing.Design;
using System.Windows.Forms;
using InternalEffect.UIParameters;
using HDRColorPicker;
using System.Drawing;
using PostEffectCore;
using Microsoft.DirectX.Direct3D;

namespace InternalEffect
{
	[TypeConverter(typeof(CustomObjectConverter))]
	[Editor(typeof(SliderEditor), typeof(UITypeEditor))]
	public class FloatSlider : IParameterUI
	{
		public event EventHandler ValueChanged;

		protected string m_Name;
		private SliderControl m_InternalControl;

		internal FloatSlider(string name)
		{
			m_Name = name;
			m_InternalControl = new SliderControl(SliderControl.SliderType.Float);
			m_InternalControl.ValueChanged += new EventHandler(OnInternalControlValueChanged);
		}

		private void OnInternalControlValueChanged(object sender, EventArgs e)
		{
			if (ValueChanged != null)
				ValueChanged(this, new EventArgs());
		}

		[Browsable(false)]
		public ParameterType ParameterType
		{
			get
			{
				return (ParameterType.Float);
			}
		}

		[Browsable(false)]
		public SliderControl InternalControl
		{
			get
			{
				return (m_InternalControl);
			}
		}

		[Browsable(false)]
		public float Value
		{
			get
			{
				return (m_InternalControl.ValueFloat);
			}
			set
			{
				bool changed = false;
				if (m_InternalControl.ValueFloat != value)
					changed = true;
				m_InternalControl.ValueFloat = value;
				if (changed && ValueChanged != null)
					ValueChanged(this, new EventArgs());
			}
		}

		[Browsable(false)]
		public float Coeficient
		{
			get
			{
				return (m_InternalControl.Coeficient);
			}
		}

		[Category("Parameters")]
		public float Minimum
		{
			get
			{
				return (m_InternalControl.MinimumFloat);
			}
			set
			{
				m_InternalControl.MinimumFloat = value;
			}
		}

		[Category("Parameters")]
		public float Maximum
		{
			get
			{
				return (m_InternalControl.MaximumFloat);
			}
			set
			{
				m_InternalControl.MaximumFloat = value;
			}
		}

		public string GetMinimums()
		{
			return (Minimum.ToString());
		}

		public bool SetMinimums(string values)
		{
			float tmp;
			if (float.TryParse(values, out tmp) == false)
				return (false);
			Minimum = tmp;
			return (true);
		}

		public string GetMaximums()
		{
			return (Maximum.ToString());
		}

		public bool SetMaximums(string values)
		{
			float tmp;
			if (float.TryParse(values, out tmp) == false)
				return (false);
			Maximum = tmp;
			return (true);
		}

		[Browsable(false)]
		public string Name
		{
			get
			{
				return (m_Name);
			}
		}

		public string GetValues()
		{
			return (Value.ToString());
		}

		public bool SetValues(string values)
		{
			float tmp;
			if (float.TryParse(values, out tmp) == false)
				return (false);
			Value = tmp;
			return (true);
		}
	}



	[TypeConverter(typeof(CustomObjectConverter))]
	[Editor(typeof(SliderEditor), typeof(UITypeEditor))]
	public class IntegerSlider : IParameterUI
	{
		public event EventHandler ValueChanged;

		protected string m_Name;
		private SliderControl m_InternalControl;

		internal IntegerSlider(string name)
		{
			m_Name = name;
			m_InternalControl = new SliderControl(SliderControl.SliderType.Integer);
			m_InternalControl.ValueChanged += new EventHandler(OnInternalControlValueChanged);
		}

		private void OnInternalControlValueChanged(object sender, EventArgs e)
		{
			if (ValueChanged != null)
				ValueChanged(this, new EventArgs());
		}

		[Browsable(false)]
		public ParameterType ParameterType
		{
			get
			{
				return (ParameterType.Integer);
			}
		}

		[Browsable(false)]
		public SliderControl InternalControl
		{
			get
			{
				return (m_InternalControl);
			}
		}

		[Browsable(false)]
		public int Value
		{
			get
			{
				return (m_InternalControl.ValueInteger);
			}
			set
			{
				bool changed = false;
				if (m_InternalControl.ValueInteger != value)
					changed = true;
				m_InternalControl.ValueInteger = value;
				if (changed && ValueChanged != null)
					ValueChanged(this, new EventArgs());
			}
		}

		[Category("Parameters")]
		public int Minimum
		{
			get
			{
				return (m_InternalControl.MinimumInteger);
			}
			set
			{
				m_InternalControl.MinimumInteger = value;
			}
		}

		[Category("Parameters")]
		public int Maximum
		{
			get
			{
				return (m_InternalControl.MaximumInteger);
			}
			set
			{
				m_InternalControl.MaximumInteger = value;
			}
		}

		public string GetMinimums()
		{
			return (Minimum.ToString());
		}

		public bool SetMinimums(string values)
		{
			int tmp;
			if (int.TryParse(values, out tmp) == false)
				return (false);
			Minimum = tmp;
			return (true);
		}

		public string GetMaximums()
		{
			return (Maximum.ToString());
		}

		public bool SetMaximums(string values)
		{
			int tmp;
			if (int.TryParse(values, out tmp) == false)
				return (false);
			Maximum = tmp;
			return (true);
		}

		[Browsable(false)]
		public string Name
		{
			get
			{
				return (m_Name);
			}
		}

		public string GetValues()
		{
			return (Value.ToString());
		}

		public bool SetValues(string values)
		{
			int tmp;
			if (int.TryParse(values, out tmp) == false)
				return (false);
			Value = tmp;
			return (true);
		}
	}



	[TypeConverter(typeof(BooleanConverter))]
	public class BooleanSelector : IParameterUI
	{
		public event EventHandler ValueChanged;

		protected string m_Name;
		protected bool m_Value;

		internal BooleanSelector(string name)
		{
			m_Name = name;
		}

		[Browsable(false)]
		public ParameterType ParameterType
		{
			get
			{
				return (ParameterType.Boolean);
			}
		}

		[Browsable(false)]
		public bool Value
		{
			get
			{
				return (m_Value);
			}
			set
			{
				bool changed = false;
				if (m_Value != value)
					changed = true;
				m_Value = value;
				if (changed && ValueChanged != null)
					ValueChanged(this, new EventArgs());
			}
		}

		public string GetMinimums()
		{
			return (null);
		}

		public bool SetMinimums(string values)
		{
			return (false);
		}

		public string GetMaximums()
		{
			return (null);
		}

		public bool SetMaximums(string values)
		{
			return (false);
		}

		[Browsable(false)]
		public string Name
		{
			get
			{
				return (m_Name);
			}
		}

		public string GetValues()
		{
			return (Value.ToString().ToLower());
		}

		public bool SetValues(string values)
		{
			bool tmp;
			if (StringHelper.TryParseBool(values, out tmp) == false)
				return (false);
			Value = tmp;
			return (true);
		}
	}


	[TypeConverter(typeof(NonExpandableConverter))]
	[Editor(typeof(ColorPickerEditor), typeof(UITypeEditor))]
	public class ColorSelector : IParameterUI
	{
		public event EventHandler ValueChanged;

		protected string m_Name;
		private UserControlHDRColorPicker m_InternalControl;

		internal ColorSelector(string name)
		{
			m_Name = name;
			m_InternalControl = new UserControlHDRColorPicker();
			m_InternalControl.Size = new Size(196, 260);
			m_InternalControl.ColorChanged += new EventHandler(OnInternalControlColorChanged);
		}

		private void OnInternalControlColorChanged(object sender, EventArgs e)
		{
			Value = this.InternalControl.Color;
			GlobalContainer.ViewportDX.PerformRender();
			if (ValueChanged != null)
				ValueChanged(this, new EventArgs());
		}

		[Browsable(false)]
		public ParameterType ParameterType
		{
			get
			{
				return (ParameterType.Float);
			}
		}

		[Browsable(false)]
		public UserControlHDRColorPicker InternalControl
		{
			get
			{
				return (m_InternalControl);
			}
		}

		[Browsable(false)]
		public ColorF Value
		{
			get
			{
				return (m_InternalControl.Color);
			}
			set
			{
				ColorF newColor = new ColorF(
					(float)Math.Max(0.0, Math.Min(value.A, 1.0)),
					(float)Math.Max(0.0, Math.Min(value.R, 1.0)),
					(float)Math.Max(0.0, Math.Min(value.G, 1.0)),
					(float)Math.Max(0.0, Math.Min(value.B, 1.0)));

				bool changed = false;
				if (m_InternalControl.Color != newColor)
					changed = true;
				m_InternalControl.Color = newColor;
				if (changed && ValueChanged != null)
					ValueChanged(this, new EventArgs());
			}
		}

		[Browsable(false)]
		public string Name
		{
			get
			{
				return (m_Name);
			}
		}

		public string GetValues()
		{
			return (string.Format("{0}; {1}; {2}; {3}",
				(float)m_InternalControl.Color.R,
				(float)m_InternalControl.Color.G,
				(float)m_InternalControl.Color.B,
				(float)m_InternalControl.Color.A));
		}

		public bool SetValues(string values)
		{
			string[] strValues = values.Split(';');
			if (strValues.Length != 4)
				return (false);

			float[] tmp = new float[4];
			for (int i = 0; i < strValues.Length; i++)
			{
				if (float.TryParse(strValues[i], out tmp[i]) == false)
					return (false);
			}
			Value = new ColorF(tmp[3], tmp[0], tmp[1], tmp[2]);
			return (true);
		}

		#region IParameterUI Members


		public string GetMinimums()
		{
			return (null);
		}

		public bool SetMinimums(string values)
		{
			return (true);
		}

		public string GetMaximums()
		{
			return (null);
		}

		public bool SetMaximums(string values)
		{
			return (true);
		}

		#endregion
	}



	/*
	[TypeConverter(typeof(NonExpandableConverter))]
	[Editor(typeof(MousePadEditor), typeof(UITypeEditor))]
	public class MousePadSelector : IParameterUI
	{
		public event EventHandler ValueChanged;

		protected string m_Name;
		private MousePadControl m_InternalControl;

		internal MousePadSelector(string name)
		{
			m_Name = name;
			m_InternalControl = new MousePadControl();
			m_InternalControl.Size = new Size(256, 256);
			m_InternalControl.CoordinatesChanged += new MousePadControl.CoordinateHandler(OnInternalControlCoordinatesChanged);
		}

		private void OnInternalControlCoordinatesChanged(float[] coords)
		{
			if (ValueChanged != null)
				ValueChanged(this, new EventArgs());
		}

		[Browsable(false)]
		public MousePadControl InternalControl
		{
			get
			{
				return (m_InternalControl);
			}
		}

		[Browsable(false)]
		public float[] Value
		{
			get
			{
				return (m_InternalControl.Coordinates);
			}
			set
			{
				m_InternalControl.Coordinates = value;
			}
		}

		public string GetMinimums()
		{
			if (m_InternalControl.CoordinatesType == MousePadControl.CoordType.ZeroToOne)
				return ("0;0");
			else
				return ("-1;-1");
		}

		public bool SetMinimums(string values)
		{
			float[] tmp = new float[2];
			string[] vals = values.Split(';');
			if (vals.Length != 2)
				return (false);
			for (int i = 0; i < 2; i++)
			{
				if (StringHelper.TryParseFloat(vals[i], out tmp[i]) == false)
					return (false);
			}
			if (tmp[0] < 0.0f && tmp[1] < 0.0f)
				m_InternalControl.CoordinatesType = MousePadControl.CoordType.MinusOneToOne;
			else
				m_InternalControl.CoordinatesType = MousePadControl.CoordType.ZeroToOne;

			return (true);
		}

		public string GetMaximums()
		{
			return ("1;1");
		}

		public bool SetMaximums(string values)
		{
			return (true);
		}

		[Browsable(false)]
		public string Name
		{
			get
			{
				return (m_Name);
			}
		}

		public string GetValues()
		{
			return (string.Format("{0};{1}", Value[0], Value[1]));
		}

		public bool SetValues(string values)
		{
			float[] tmp = new float[2];
			string[] vals = values.Split(';');
			if (vals.Length != 2)
				return (false);
			for (int i = 0; i < 2; i++)
			{
				if (StringHelper.TryParseFloat(vals[i], out tmp[i]) == false)
					return (false);
			}
			Value = tmp;
			return (true);
		}
	}
	*/
}
