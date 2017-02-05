using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace ModelScenePlugin
{
	public partial class frmSelectTransform : Form
	{
		private TransformType m_TransformType;

		public frmSelectTransform()
		{
			InitializeComponent();
			string[] names = Enum.GetNames(typeof(TransformType));
			foreach (string name in names)
				cboTransforms.Items.Add(name);

			DialogResult = DialogResult.Cancel;
		}

		private void btnOK_Click(object sender, EventArgs e)
		{
			if (cboTransforms.SelectedIndex == -1)
			{
				MessageBox.Show("You must select a transform.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}

			m_TransformType = (TransformType)cboTransforms.SelectedIndex;
			DialogResult = DialogResult.OK;
			Close();
		}

		private void btnCancel_Click(object sender, EventArgs e)
		{
			Close();
		}

		public TransformType TransformType
		{
			get
			{
				return (m_TransformType);
			}
		}
	}
}
