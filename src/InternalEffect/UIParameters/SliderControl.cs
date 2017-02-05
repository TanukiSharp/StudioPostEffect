using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace InternalEffect
{
	public partial class SliderControl : UserControl
	{
		public event EventHandler ValueChanged;

		public enum SliderType
		{
			Float,
			Integer
		}

		private SliderType m_Type;
		private float m_Value;
		private float m_Coef;
		private float m_MaxBound;
		private float m_MinBound;

		public SliderControl(SliderType type)
		{
			InitializeComponent();

			trkValue.ValueChanged += new EventHandler(trkValue_ValueChanged);
			trkValue.Scroll += new EventHandler(trkValue_Scroll);

			BackColor = Color.FromKnownColor(KnownColor.Control);
			m_Type = type;

			if (m_Type == SliderType.Float)
			{
				SetBounds(0.0f, 1.0f);
			}
			else // if (m_Type == SliderType.Integer)
			{
				SetBounds(0, 255);
			}
		}

		private void trkValue_ValueChanged(object sender, EventArgs e)
		{
			if (ValueChanged != null)
				ValueChanged(this, e);
		}

		private void trkValue_Scroll(object sender, EventArgs e)
		{
			if (m_Type == SliderType.Float)
			{
				m_Coef = ((float)trkValue.Value / (float)trkValue.Maximum);
				m_Value = m_MinBound + m_Coef * (m_MaxBound - m_MinBound);
				lblValue.Text = m_Value.ToString();
			}
			else // if (m_Type == SliderType.Integer)
			{
				lblValue.Text = trkValue.Value.ToString();
			}
		}

		public float ValueFloat
		{
			get
			{
				if (m_Type == SliderType.Float)
					return (m_Value);
				else
				{
					ThrowTypeExcepetion();
					return (0.0f);
				}
			}
			set
			{
				if (m_Type == SliderType.Float)
				{
					m_Value = Math.Max(m_MinBound, Math.Min((float)value, m_MaxBound));
					lblValue.Text = m_Value.ToString();
					m_Coef = (m_Value - m_MinBound) / (m_MaxBound - m_MinBound);
					m_Coef = Math.Max(0.0f, Math.Min(m_Coef, 1.0f));
					trkValue.Value = (int)(m_Coef * (float)trkValue.Maximum);
				}
				else
					ThrowTypeExcepetion();
			}
		}

		public float Coeficient
		{
			get
			{
				if (m_Type == SliderType.Float)
					return (m_Coef);
				else
				{
					ThrowTypeExcepetion();
					return (0.0f);
				}
			}
		}

		public int ValueInteger
		{
			get
			{
				if (m_Type == SliderType.Integer)
					return (trkValue.Value);
				else
				{
					ThrowTypeExcepetion();
					return (0);
				}
			}
			set
			{
				if (m_Type == SliderType.Integer)
					trkValue.Value = Math.Max(trkValue.Minimum, Math.Min((int)value, trkValue.Maximum));
				else
					ThrowTypeExcepetion();
			}
		}

		public float MinimumFloat
		{
			get
			{
				if (m_Type == SliderType.Float)
					return (m_MinBound);
				else
				{
					ThrowTypeExcepetion();
					return (0.0f);
				}
			}
			set
			{
				if (m_Type == SliderType.Float)
					SetBounds(value, m_MaxBound);
				else
					ThrowTypeExcepetion();
			}
		}

		public float MaximumFloat
		{
			get
			{
				if (m_Type == SliderType.Float)
					return (m_MaxBound);
				else
				{
					ThrowTypeExcepetion();
					return (0.0f);
				}
			}
			set
			{
				if (m_Type == SliderType.Float)
					SetBounds(m_MinBound, value);
				else
					ThrowTypeExcepetion();
			}
		}

		public int MinimumInteger
		{
			get
			{
				if (m_Type == SliderType.Integer)
					return (trkValue.Minimum);
				else
				{
					ThrowTypeExcepetion();
					return (0);
				}
			}
			set
			{
				if (m_Type == SliderType.Integer)
					SetBounds(value, trkValue.Maximum);
				else
					ThrowTypeExcepetion();
			}
		}

		public int MaximumInteger
		{
			get
			{
				if (m_Type == SliderType.Integer)
					return (trkValue.Maximum);
				else
				{
					ThrowTypeExcepetion();
					return (0);
				}
			}
			set
			{
				if (m_Type == SliderType.Integer)
					SetBounds(trkValue.Minimum, value);
				else
					ThrowTypeExcepetion();
			}
		}

		private void SetBounds(int min, int max)
		{
			int tmp = Math.Max(min, Math.Min(trkValue.Value, max));
			trkValue.Value = tmp;
			lblValue.Text = trkValue.Value.ToString();

			trkValue.Minimum = min;
			trkValue.Maximum = max;

			lblMinBound.Text = min.ToString();
			lblMaxBound.Text = max.ToString();
		}

		internal void SetBounds(float min, float max)
		{
			float tmp = Math.Max(min, Math.Min(m_Value, max));
			m_Value = tmp;

			trkValue.Minimum = 0;
			trkValue.Maximum = 1000;
			m_MinBound = min;
			m_MaxBound = max;

			float coef = (m_Value - m_MinBound) / (m_MaxBound - m_MinBound);
			coef = Math.Max(0.0f, Math.Min(coef, 1.0f));
			trkValue.Value = (int)(coef * (float)trkValue.Maximum);
			lblValue.Text = m_Value.ToString();

			lblMinBound.Text = min.ToString();
			lblMaxBound.Text = max.ToString();
		}

		private void ThrowTypeExcepetion()
		{
			throw new Exception(string.Format("Wrong type (this type is {0})", m_Type.ToString().ToLower()));
		}
	}
}
