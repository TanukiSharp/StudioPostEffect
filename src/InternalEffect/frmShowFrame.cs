using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace InternalEffect
{
	public partial class frmShowFrame : Form
	{
		public frmShowFrame(Bitmap bmp)
		{
			InitializeComponent();

			this.ClientSize = new Size(bmp.Width + 24, bmp.Height + 24);
			picFrame.Image = bmp;

			this.Text = string.Format("Frame - [{0}, {1}]", bmp.Width, bmp.Height);
		}

		protected override void OnClosed(EventArgs e)
		{
			base.OnClosed(e);
			picFrame.Image.Dispose();
		}
	}
}
