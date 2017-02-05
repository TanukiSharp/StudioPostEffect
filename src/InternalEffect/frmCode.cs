using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ICSharpCode.TextEditor.Document;
using System.Collections;
using InternalEffect;
using Microsoft.DirectX.Direct3D;

namespace InternalEffect
{
	public partial class frmCode : Form
	{
		private CustomEffect m_Effect;

		public frmCode(CustomEffect effect)
		{
			InitializeComponent();
			m_Effect = effect;

			m_Effect.OnShaderRebuid += new CustomEffect.ShaderBuildHandler(OnShaderRebuid);

			txtCode.ShowSpaces = false;
			txtCode.ShowTabs = false;
			txtCode.ShowEOLMarkers = false;
			txtCode.EnableFolding = true;

			txtCode.Document.DocumentChanged += new DocumentEventHandler(OnTextChanged);
			txtCode.ActiveTextAreaControl.TextArea.KeyDown += new KeyEventHandler(OnTextKeyDown);
			try
			{
				FileSyntaxModeProvider provider = new FileSyntaxModeProvider(string.Format("{0}\\syntax", Application.StartupPath));
				HighlightingManager manager = HighlightingManager.Manager;
				manager.AddSyntaxModeFileProvider(provider);
				txtCode.Document.HighlightingStrategy = manager.FindHighlighter("HLSL.xshd");
			}
			catch
			{
			}

			txtCode.LoadFile(m_Effect.Filename, true, true);

			m_Changed = false;
			DisplayName();
		}

		private void DisplayName()
		{
			StringBuilder sb = new StringBuilder("Effect Code");
			if (m_Effect != null)
				sb.AppendFormat(" - {0}", m_Effect.Filename);
			if (m_Changed)
				sb.Append(" *");
			this.Text = sb.ToString();
		}

		private void SaveFile()
		{
			if (m_Changed == false)
				return;

			txtCode.SaveFile(m_Effect.Filename);
			m_Effect.MarkToReload();
			m_Changed = false;
			DisplayName();
		}

		private void OnShaderRebuid(string value, bool request)
		{
			if (value != null)
			{
				txtBuildLog.Text = value;
				splitContainer1.Panel2Collapsed = false;
			}
			else
			{
				splitContainer1.Panel2Collapsed = true;
				if (request)
					SaveFile();
			}
		}

		private bool m_Changed = false;

		private void OnTextChanged(object sender, DocumentEventArgs e)
		{
			if (m_Changed == false)
			{
				m_Changed = true;
				DisplayName();
			}
		}

		private void OnTextKeyDown(object sender, KeyEventArgs e)
		{
			if (e.Control == true && e.KeyCode == Keys.S)
				m_Effect.MarkToRebuild(txtCode.Text, true);
		}

		private void frmCode_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (m_Changed)
			{
				DialogResult res;

				res = MessageBox.Show("Some modifications are unsaved and will be lost if you proceed.\r\nDo you still want to close the code editor ?", "Discard modifications ?", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
				if (res == DialogResult.No)
					e.Cancel = true;
			}
		}

		private void btnBuildShader_Click(object sender, EventArgs e)
		{
			m_Effect.MarkToRebuild(txtCode.Text, false);
		}

		private void btnSaveShader_Click(object sender, EventArgs e)
		{
			m_Effect.MarkToRebuild(txtCode.Text, true);
		}
	}
}
