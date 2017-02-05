using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace InternalEffect
{
	public partial class frmInput : Form
	{
		private string m_Value;

		public frmInput()
		{
			InitializeComponent();
			DialogResult = DialogResult.Cancel;
		}

		public frmInput(string title)
			: this()
		{
			this.Text = title;
		}

		public frmInput(string title, string prompt)
			: this(title)
		{
			lblPromt.Text = prompt;
		}

		public frmInput(string title, string prompt, string defaultValue)
			: this(title, prompt)
		{
			txtInput.Text = defaultValue;
		}

		private void btnOK_Click(object sender, EventArgs e)
		{
			string value = txtInput.Text.Trim();
			if (value.Length == 0)
			{
				MessageBox.Show("You must enter a value", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}

			m_Value = value;
			DialogResult = DialogResult.OK;
			Close();
		}

		private void btnCancel_Click(object sender, EventArgs e)
		{
			Close();
		}

		public string Value
		{
			get
			{
				return (m_Value);
			}
		}
	}
}
