using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using ViewportDXControl;
using Microsoft.DirectX.Direct3D;
using System.Drawing.Imaging;
using PostEffectCore;

namespace StudioPostEffect
{
	public partial class frmAbout : Form
	{
		public frmAbout()
		{
			InitializeComponent();
			lblVersion.Text = string.Format("version {0}", IOHelper.GetProductVersion());
		}

		private void btnOK_Click(object sender, EventArgs e)
		{
			Close();
		}

		private void lblHome_Click(object sender, EventArgs e)
		{
			Process.Start("http://sites.google.com/site/studioposteffect");
		}
	}
}
