using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace InternalEffect.UIParameters
{
	public partial class MousePadControl : UserControl
	{
		public delegate void CoordinateHandler(float[] coords);
		public event CoordinateHandler CoordinatesChanged;
		private Font m_TextFont;

		internal enum CoordType
		{
			ZeroToOne,
			MinusOneToOne
		}

		private CoordType m_CoordType = CoordType.ZeroToOne;
		private float[] m_XY = new float[2];

		internal CoordType CoordinatesType
		{
			get
			{
				return (m_CoordType);
			}
			set
			{
				m_CoordType = value;
			}
		}

		public float[] Coordinates
		{
			get
			{
				return (m_XY);
			}
			set
			{
				if (value.Length != 2)
					throw new FormatException();
				m_XY = value;
				this.Invalidate();
			}
		}

		public MousePadControl()
		{
			InitializeComponent();
			m_TextFont = new Font("arial", 7.0f, FontStyle.Regular);
		}

		public float[] GetMousePadValue()
		{
			return (m_XY);
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			int width = this.Width;
			int height = this.Height - pnlToolBar.Height;

			int w2 = width >> 1;
			int h2 = height >> 1;

			Graphics gr = e.Graphics;

			// draw position
			gr.FillEllipse(Brushes.Red, (m_XY[0] * width) - 3.0f, (m_XY[1] * height) - 3.0f, 6.0f, 6.0f);

			gr.DrawLine(Pens.LightGray, w2, 0, w2, height); // vertical centered line
			gr.DrawLine(Pens.LightGray, 0, h2, width, h2); // horizontal centered line

			gr.DrawLine(Pens.Black, w2, 0, w2, 3); // vertical centered small gradation
			gr.DrawLine(Pens.Black, 0, h2, 3, h2); // horizontal centered small gradation

			gr.DrawLine(Pens.Black, width - 1, 0, width - 1, 3); // top right small gradation
			gr.DrawLine(Pens.Black, 0, height - 1, 3, height - 1); // bottom left small gradation

			//base.OnPaint(e);
			if (m_CoordType == CoordType.ZeroToOne)
			{
				gr.DrawString("0", m_TextFont, Brushes.Black, 2.0f, 2.0f);

				gr.DrawString("0.5", m_TextFont, Brushes.LightGray, w2 + 2.0f, 2.0f);
				gr.DrawString("0.5", m_TextFont, Brushes.LightGray, 2.0f, h2 + 2.0f);

				gr.DrawString("1", m_TextFont, Brushes.Black, width - 10.0f, 2.0f);
				gr.DrawString("1", m_TextFont, Brushes.Black, 2.0f, height - 10.0f);
			}
			else
			{
				gr.DrawString("-1", m_TextFont, Brushes.Black, 2.0f, 2.0f);

				gr.DrawString("0", m_TextFont, Brushes.LightGray, w2 + 2.0f, 2.0f);
				gr.DrawString("0", m_TextFont, Brushes.LightGray, 2.0f, h2 + 2.0f);

				gr.DrawString("1", m_TextFont, Brushes.Black, width - 10.0f, 2.0f);
				gr.DrawString("1", m_TextFont, Brushes.Black, 2.0f, height - 10.0f);
			}
		}

		protected override void OnMouseMove(MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
			{
				float x = (float)e.X / (float)this.Width;
				float y = (float)e.Y / (float)(this.Height - pnlToolBar.Height);

				if (m_CoordType == CoordType.ZeroToOne)
				{
					m_XY[0] = x;
					m_XY[1] = y;
				}
				else
				{
					m_XY[0] = (x * 2.0f) - 1.0f;
					m_XY[1] = (y * 2.0f) - 1.0f;
				}

				if (CoordinatesChanged != null)
					CoordinatesChanged(m_XY);

				this.Invalidate();
			}
		}

		private void cboCoordType_SelectedIndexChanged(object sender, EventArgs e)
		{
			m_CoordType = (CoordType)cboCoordType.SelectedIndex;
			this.Invalidate();
		}
	}
}
