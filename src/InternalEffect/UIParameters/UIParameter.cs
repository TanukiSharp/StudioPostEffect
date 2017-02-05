using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using Microsoft.DirectX.Direct3D;
using Microsoft.DirectX;
using System.ComponentModel;
using HDRColorPicker;
using PostEffectCore;

namespace InternalEffect
{
	[TypeConverter(typeof(CustomObjectConverter))]
	public class UIParameter : NotEditablePropertyCollection<IParameterUI>, IParameterUI
	{
		protected string m_Name;
		private CustomParameter m_Parameter;
		private Effect m_Effect;
		private EffectHandle m_Handle;
		private ParameterDescription m_Desc;

		public override string DisplayName
		{
			get
			{
				return ("");
			}
		}

		[Browsable(false)]
		public new string Name
		{
			get { return (m_Name); }
		}

		[Browsable(false)]
		string IParameterUI.Name
		{
			get { return (m_Name); }
		}

		[Browsable(false)]
		public ParameterType ParameterType
		{
			get
			{
				return (m_Parameter.Type);
			}
		}

		[Browsable(false)]
		public CustomParameter CustomParameter
		{
			get
			{
				return (m_Parameter);
			}
		}

		public UIParameter(CustomParameter prm)
		{
			if (prm == null)
				return;

			try
			{

				m_Parameter = prm;
				m_Effect = prm.Effect;
				m_Handle = prm.Handle;
				m_Desc = m_Effect.GetParameterDescription(prm.Handle);

				m_Name = prm.Name;

				if ((m_Desc.Class != ParameterClass.Scalar && m_Desc.Class != ParameterClass.Vector) || m_Desc.Rows != 1 ||
					(m_Desc.Type != ParameterType.Float && m_Desc.Type != ParameterType.Integer && m_Desc.Type != ParameterType.Boolean))
				{
					throw new InvalidDataException();
				}

				if (m_Desc.Elements == 0 && m_Desc.Columns == 1)
				{
					// only one element

					string name = m_Desc.Name;

					if (m_Desc.Type == ParameterType.Float)
						AddFloat(this, name);
					else if (m_Desc.Type == ParameterType.Integer)
						AddInt(this, name);
					else if (m_Desc.Type == ParameterType.Boolean)
						AddBool(this, name);
				}
				else
				{
					// several elements

					int countElements = Math.Max(1, m_Desc.Elements);

					for (int arrayElem = 0; arrayElem < countElements; arrayElem++)
					{
						if (m_Desc.Columns == 4)
							AddColor(this, m_Desc.Name);
						/*
						else if (m_Desc.Columns == 2 && m_Desc.Type == ParameterType.Float)
							AddMousePad(this, m_Desc.Name);
						*/
						else
						{
							for (int vectorElem = 0; vectorElem < m_Desc.Columns; vectorElem++)
							{
								string letter = "?";
								if (vectorElem == 0) letter = "X";
								else if (vectorElem == 1) letter = "Y";
								else if (vectorElem == 2) letter = "Z";
								else if (vectorElem == 3) letter = "W";
								string name = letter;
								if (countElements > 1)
									name = string.Format("[{0}].{1}", arrayElem, letter);

								if (m_Desc.Type == ParameterType.Float)
									AddFloat(this, name);
								else if (m_Desc.Type == ParameterType.Integer)
									AddInt(this, name);
								else if (m_Desc.Type == ParameterType.Boolean)
									AddBool(this, name);
							}
						}
					}
				}

				LoadDefault();

			}
			catch
			{
			}
		}

		public void SetParameter(UIParameter param)
		{
			m_Desc = param.m_Desc;
			m_Effect = param.m_Effect;
			m_Handle = param.m_Handle;
			m_Name = param.m_Name;
			m_Parameter = param.m_Parameter;
			base.AddRange(param);
		}

		public IParameterUI Find(Predicate<IParameterUI> match)
		{
			if (match == null)
				return (null);

			for (int i = 0; i < this.Count; i++)
			{
				if (match((IParameterUI)this[i]))
					return ((IParameterUI)this[i]);
			}
			return (null);
		}

		private void AddFloat(UIParameter parent, string name)
		{
			FloatSlider fsl = new FloatSlider(name);
			fsl.ValueChanged += new EventHandler(OnValueChanged);
			parent.Add(fsl);
		}

		private void AddInt(UIParameter parent, string name)
		{
			IntegerSlider isl = new IntegerSlider(name);
			isl.ValueChanged += new EventHandler(OnValueChanged);
			parent.Add(isl);
		}

		private void AddBool(UIParameter parent, string name)
		{
			BooleanSelector bsl = new BooleanSelector(name);
			bsl.ValueChanged += new EventHandler(OnValueChanged);
			parent.Add(bsl);
		}

		private void AddColor(UIParameter parent, string name)
		{
			ColorSelector csl = new ColorSelector(name);
			csl.ValueChanged += new EventHandler(OnValueChanged);
			parent.Add(csl);
		}

		public void LoadDefault()
		{
			try
			{
				int elem = Math.Max(1, m_Desc.Elements);
				int col = m_Desc.Columns;
				int count = elem * col;

				if (elem == 1 && col == 4)
				{
					if (m_Desc.Type == ParameterType.Float)
					{
						float[] fArray = m_Effect.GetValueFloatArray(m_Handle, count);
						((ColorSelector)this[0]).Value = new ColorF(fArray[3], fArray[0], fArray[1], fArray[2]);
					}
					else if (m_Desc.Type == ParameterType.Integer)
					{
						int[] iArray = m_Effect.GetValueIntegerArray(m_Handle, count);
						((ColorSelector)this[0]).Value = new ColorF((float)iArray[3] / 255.0f, (float)iArray[0] / 255.0f, (float)iArray[1] / 255.0f, (float)iArray[2] / 255.0f);
					}
				}
				else
				{
					if (m_Desc.Type == ParameterType.Float)
					{
						float[] v = m_Effect.GetValueFloatArray(m_Handle, count);
						for (int i = 0; i < count; i++)
						{
							FloatSlider fsl = ((FloatSlider)this[i]);
							if (v[i] < fsl.Minimum) fsl.Minimum = (float)Math.Floor(v[i]);
							if (v[i] > fsl.Maximum) fsl.Maximum = (float)Math.Ceiling(v[i]);
							fsl.Value = v[i];
						}
					}
					else if (m_Desc.Type == ParameterType.Integer)
					{
						int[] v = m_Effect.GetValueIntegerArray(m_Handle, count);
						for (int i = 0; i < count; i++)
						{
							IntegerSlider isl = ((IntegerSlider)this[i]);
							if (v[i] < isl.Minimum) isl.Minimum = (int)Math.Floor((decimal)v[i]);
							if (v[i] > isl.Maximum) isl.Maximum = (int)Math.Ceiling((decimal)v[i]);
							isl.Value = v[i];
						}
					}
					else if (m_Desc.Type == ParameterType.Boolean)
					{
						bool[] bArray = m_Effect.GetValueBooleanArray(m_Handle, count);
						for (int i = 0; i < count; i++)
							((BooleanSelector)this[i]).Value = bArray[i];
					}
				}

			}
			catch
			{
			}
		}

		public bool SetValues(string values)
		{
			try
			{

				int elem = Math.Max(1, m_Desc.Elements);
				int col = m_Desc.Columns;
				int count = elem * col;

				string[] values_array = values.Split(';');
				if (values_array.Length != count)
					throw new Exception(string.Format("{0} value expected ({1} received)", count, values.Length));

				if (elem == 1 && col == 4)
				{
					if (m_Desc.Type == ParameterType.Float)
					{
						float[] fArray = m_Effect.GetValueFloatArray(m_Handle, count);
						((ColorSelector)this[0]).Value = new ColorF(fArray[3], fArray[0], fArray[1], fArray[2]);
					}
					else if (m_Desc.Type == ParameterType.Integer)
					{
						int[] iArray = m_Effect.GetValueIntegerArray(m_Handle, count);
						((ColorSelector)this[0]).Value = new ColorF((float)iArray[3] / 255.0f, (float)iArray[0] / 255.0f, (float)iArray[1] / 255.0f, (float)iArray[2] / 255.0f);
					}
				}
				else
				{
					if (m_Desc.Type == ParameterType.Float)
					{
						float[] fArray = StringHelper.ToFloatArray(values_array);
						for (int i = 0; i < count; i++)
							((FloatSlider)this[i]).Value = fArray[i];
					}
					else if (m_Desc.Type == ParameterType.Integer)
					{
						int[] iArray = StringHelper.ToIntegerArray(values_array);
						for (int i = 0; i < count; i++)
							((IntegerSlider)this[i]).Value = iArray[i];
					}
					else if (m_Desc.Type == ParameterType.Boolean)
					{
						bool[] bArray = StringHelper.ToBooleanArray(values_array);
						for (int i = 0; i < count; i++)
							((BooleanSelector)this[i]).Value = bArray[i];
					}
				}

				SetUIValues();
				return (true);

			}
			catch
			{
				return (false);
			}
		}


		public void SetUIValues()
		{
			try
			{
				int elem = Math.Max(1, m_Desc.Elements);
				int col = m_Desc.Columns;
				int count = elem * col;

				if (elem == 1 && col == 4)
				{
					if (m_Desc.Type == ParameterType.Float)
					{
						ColorF c = ((ColorSelector)this[0]).Value;
						m_Effect.SetValue(m_Handle, new float[4] { (float)c.R, (float)c.G, (float)c.B, (float)c.A });
					}
					else if (m_Desc.Type == ParameterType.Integer)
					{
						ColorF c = ((ColorSelector)this[0]).Value;
						m_Effect.SetValue(m_Handle, new int[4] { (int)(c.R * 255.0), (int)(c.G * 255.0), (int)(c.B * 255.0), (int)(c.A * 255.0) });
					}
				}
				else
				{
					if (m_Desc.Type == ParameterType.Float)
					{
						float[] fArray = new float[count];
						for (int i = 0; i < count; i++)
							fArray[i] = ((FloatSlider)this[i]).Value;
						m_Effect.SetValue(m_Handle, fArray);
					}
					else if (m_Desc.Type == ParameterType.Integer)
					{
						int[] iArray = new int[count];
						for (int i = 0; i < count; i++)
							iArray[i] = ((IntegerSlider)this[i]).Value;
						m_Effect.SetValue(m_Handle, iArray);
					}
					else if (m_Desc.Type == ParameterType.Boolean)
					{
						bool[] bArray = new bool[count];
						for (int i = 0; i < count; i++)
							bArray[i] = ((BooleanSelector)this[i]).Value;
						m_Effect.SetValue(m_Handle, bArray);
					}
				}

				foreach (object sub in this)
				{
					if (sub is UIParameter)
						((UIParameter)sub).SetUIValues();
				}
			}
			catch
			{
			}
		}

		public string GetValues()
		{
			List<string> values = new List<string>();

			int elem = Math.Max(1, m_Desc.Elements);
			int col = m_Desc.Columns;
			int count = elem * col;

			if (elem == 1 && col == 4)
			{
				if (m_Desc.Type == ParameterType.Float)
				{
					for (int i = 0; i < count; i += 4)
					{
						ColorF c = ((ColorSelector)this[i / 4]).Value;
						values.Add(string.Format("{0};{1};{2};{3}", (float)c.R, (float)c.G, (float)c.B, (float)c.A));
					}
				}
				else if (m_Desc.Type == ParameterType.Integer)
				{
					for (int i = 0; i < count; i += 4)
					{
						ColorF c = ((ColorSelector)this[i / 4]).Value;
						values.Add(string.Format("{0};{1};{2};{3}", (int)(c.R * 255.0), (int)(c.G * 255.0), (int)(c.B * 255.0), (int)(c.A * 255.0)));
					}
				}
			}
			if (m_Desc.Type == ParameterType.Float)
			{
				for (int i = 0; i < count; i++)
					values.Add(((FloatSlider)this[i]).Value.ToString());
			}
			else if (m_Desc.Type == ParameterType.Integer)
			{
				for (int i = 0; i < count; i++)
					values.Add((((IntegerSlider)this[i]).Value).ToString());
			}
			else if (m_Desc.Type == ParameterType.Boolean)
			{
				for (int i = 0; i < count; i++)
					values.Add(((BooleanSelector)this[i]).Value ? "1" : "0");
			}

			if (values.Count != count)
				throw new Exception(string.Format("{0} value expected ({1} received)", count, values.Count));

			return (string.Join(";", values.ToArray()));
		}


		public string GetMinimums()
		{
			if (m_Desc.Type == ParameterType.Boolean)
				return (null);

			if (m_Desc.Columns == 4) // color
				return (null);

			List<string> values = new List<string>();

			int count = Math.Max(1, m_Desc.Elements) * m_Desc.Columns;
			if (m_Desc.Type == ParameterType.Float)
			{
				for (int i = 0; i < count; i++)
					values.Add(((FloatSlider)this[i]).GetMinimums());
			}
			else if (m_Desc.Type == ParameterType.Integer)
			{
				for (int i = 0; i < count; i++)
					values.Add(((IntegerSlider)this[i]).GetMinimums());
			}

			return (string.Join(";", values.ToArray()));
		}

		public bool SetMinimums(string values)
		{
			if (values == null)
				return (false);

			if (m_Desc.Type == ParameterType.Boolean)
				return (false);

			if (m_Desc.Columns == 4) // color
				return (false);

			string[] values_array = values.Split(';');
			int count = Math.Max(1, m_Desc.Elements) * m_Desc.Columns;

			if (values_array.Length != count)
				throw new Exception(string.Format("{0} value expected ({1} received)", count, values.Length));

			if (m_Desc.Type == ParameterType.Float)
			{
				for (int i = 0; i < count; i++)
					((FloatSlider)this[i]).SetMinimums(values_array[i]);
			}
			else if (m_Desc.Type == ParameterType.Integer)
			{
				for (int i = 0; i < count; i++)
					((IntegerSlider)this[i]).SetMinimums(values_array[i]);
			}
			return (true);
		}

		public string GetMaximums()
		{
			if (m_Desc.Type == ParameterType.Boolean)
				return (null);

			if (m_Desc.Columns == 4) // color
				return (null);

			List<string> values = new List<string>();

			int count = Math.Max(1, m_Desc.Elements) * m_Desc.Columns;
			if (m_Desc.Type == ParameterType.Float)
			{
				for (int i = 0; i < count; i++)
					values.Add(((FloatSlider)this[i]).GetMaximums());
			}
			else if (m_Desc.Type == ParameterType.Integer)
			{
				for (int i = 0; i < count; i++)
					values.Add(((IntegerSlider)this[i]).GetMaximums());
			}

			return (string.Join(";", values.ToArray()));
		}

		public bool SetMaximums(string values)
		{
			if (values == null)
				return (false);

			if (m_Desc.Type == ParameterType.Boolean)
				return (false);

			if (m_Desc.Columns == 4) // color
				return (false);

			string[] values_array = values.Split(';');
			int count = Math.Max(1, m_Desc.Elements) * m_Desc.Columns;

			if (values_array.Length != count)
				throw new Exception(string.Format("{0} value expected ({1} received)", count, values.Length));

			if (m_Desc.Type == ParameterType.Float)
			{
				for (int i = 0; i < count; i++)
					((FloatSlider)this[i]).SetMaximums(values_array[i]);
			}
			else if (m_Desc.Type == ParameterType.Integer)
			{
				for (int i = 0; i < count; i++)
					((IntegerSlider)this[i]).SetMaximums(values_array[i]);
			}
			return (true);
		}


		protected override void OnValueChanged(object component, EventArgs e)
		{
			base.OnValueChanged(component, e);

			SetUIValues();
			ProjectModified();
		}

		private void ProjectModified()
		{
			if (GlobalContainer.Project != null)
				GlobalContainer.Project.IsModified = true;
		}
	}
}
