using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using Microsoft.DirectX.Direct3D;
using System.IO;
using PostEffectCore;

namespace InternalEffect
{
	public partial class EffectWorkflowManager : UserControl
	{
		public delegate void WorkflowItemHandler(EffectWorkflowItem workflowItem);
		public event WorkflowItemHandler WorkflowItemSelected;
		public event WorkflowItemHandler WorkflowItemRemoved;

		public delegate void CustomEffectHandler(CustomEffect customEffect);
		public event CustomEffectHandler EffectEdit;

		private LinkManager m_LinkManager = new LinkManager();
		private LinkBound m_Result;

		private Pen m_LinkPen;

		public EffectWorkflowManager()
		{
			InitializeComponent();

			m_LinkPen = new Pen(Color.Gray, 2.0f);

			this.Resize += new EventHandler(OnWorkflowManagerResize);
			this.MouseUp += new MouseEventHandler(OnPicRenderMouseUp);
			this.MouseDown += new MouseEventHandler(OnPicRenderMouseDown);
			this.MouseMove += new MouseEventHandler(OnPicRenderMouseMove);

			this.MouseClick += new MouseEventHandler(OnPicRenderMouseClick);

			m_LinkManager.SetEffectWorkflowManager(this);
			m_LinkManager.LinksUpdate += new EventHandler(OnLinksUpdate);
		}

		public void Initialize()
		{
			AddResult(250, 150);
		}

		internal LinkManager LinkManager
		{
			get
			{
				return (m_LinkManager);
			}
		}

		/*
		public EffectWorkflowItem SelectedWorkflowItem
		{
			get
			{
				return (m_SelectedWorkflowItem);
			}
			set
			{
				SelectWorkflowItem(value);
			}
		}
		*/


		public enum WorkflowItemSelectionMode
		{
			AddToSelection,
			SetAsSelection,
		}

		private bool m_SelectionNotifierProtected;
		internal void WorkflowItemSelectionChangedNotification(EffectWorkflowItem item)
		{
			if (m_SelectionNotifierProtected)
				return;

			int selectedCount = 0;
			EffectWorkflowItem selectedItem = null;
			foreach (EffectWorkflowItem elem in this.Controls)
			{
				if (elem.IsSelected)
				{
					selectedItem = elem;
					selectedCount++;
				}
				if (selectedCount > 1)
					break;
			}

			if (selectedCount > 1)
				GlobalContainer.SplitContainerWorkflowParams.Panel2Collapsed = true;

			if (selectedCount == 1 && WorkflowItemSelected != null)
				WorkflowItemSelected(selectedItem);
		}

		public void UnselectAllWorkflowItems()
		{
			m_SelectionNotifierProtected = true;
			foreach (EffectWorkflowItem item in this.Controls)
				item.Unselected();
			GlobalContainer.SplitContainerWorkflowParams.Panel2Collapsed = true;
			m_SelectionNotifierProtected = false;
		}

		public bool IsZeroSelected()
		{
			foreach (EffectWorkflowItem item in this.Controls)
			{
				if (item.IsSelected)
					return (false);
			}
			return (true);
		}

		public IEnumerable<EffectWorkflowItem> SelectedWorkflowItems
		{
			get
			{
				foreach (EffectWorkflowItem item in this.Controls)
				{
					if (item.IsSelected)
						yield return item;
				}
			}
		}




		internal void FireWorkflowItemRemovedEvent(EffectWorkflowItem workflowItem)
		{
			if (WorkflowItemRemoved != null)
				WorkflowItemRemoved(workflowItem);
		}

		public void OnUpdateScene()
		{
			this.FindForm().Invoke((MethodInvoker)delegate()
			{
				m_CurrentMousePosition = this.PointToClient(Control.MousePosition);
				this.Invalidate();
			});
		}

		public LinkBound Result
		{
			get
			{
				return (m_Result);
			}
		}

		private void OnLinksUpdate(object sender, EventArgs e)
		{
			if (m_LinkManager.LinkingBound != null)
			{
				if (m_LinkManager.LinkingBound.Parent != this)
					m_CurrentMousePosition = this.PointToClient(m_LinkManager.LinkingBound.PointToScreen(new Point()));
				else
				{
					m_CurrentMousePosition = new Point(m_LinkManager.LinkingBound.Left + m_LinkManager.LinkingBound.Width / 2,
						m_LinkManager.LinkingBound.Top + m_LinkManager.LinkingBound.Height / 2);
				}
			}
		}

		private void OnPicRenderMouseUp(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
			{
				m_MouseDown = false;
			}
		}

		private bool m_MouseDown;
		private Point m_MouseDownPosition;
		private void OnPicRenderMouseDown(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
			{
				m_MouseDown = true;
				m_MouseDownPosition = new Point(e.X, e.Y);
				UnselectAllWorkflowItems();
			}

			m_LinkManager.LinkingCanceled();
		}

		private Point m_CurrentMousePosition;
		private void OnPicRenderMouseMove(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
			{
				if (m_MouseDown == false)
					OnPicRenderMouseDown(sender, e);

				int dx = e.X - m_MouseDownPosition.X;
				int dy = e.Y - m_MouseDownPosition.Y;
				foreach (Control ctrl in this.Controls)
				{
					ctrl.Left += dx;
					ctrl.Top += dy;
					m_MouseDownPosition = new Point(e.X, e.Y);
				}
				ProjectModified();
			}
		}

		private void ProjectModified()
		{
			if (GlobalContainer.Project != null)
				GlobalContainer.Project.IsModified = true;
		}

		private Point m_MenuLocation;
		private void OnPicRenderMouseClick(object sender, MouseEventArgs e)
		{
			m_LinkManager.LinkingCanceled();

			if (e.Button == MouseButtons.Right)
			{
				m_MenuLocation = new Point(e.X, e.Y);
				ctxMenu.Show(this, e.X, e.Y);
			}
		}

		private void OnWorkflowManagerResize(object sender, EventArgs e)
		{
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);

			e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

			foreach (Link link in m_LinkManager.Links)
				RenderPoints(e.Graphics, link.Input, link.Output, link);

			if (m_LinkManager.LinkingBound != null)
			{
				if (m_LinkManager.LinkingBound.IOMode == IOMode.Input)
					RenderPoints(e.Graphics, m_LinkManager.LinkingBound, null, null);
				else
					RenderPoints(e.Graphics, null, m_LinkManager.LinkingBound, null);
			}
		}

		private Point outLoc;
		private Point outLocCtrl;
		private Point inLocCtrl;
		private Point inLoc;

		private void RenderPoints(Graphics gr, LinkBound input, LinkBound output, Link link)
		{
			ComputeBezierPoints(input, output, out outLoc, out outLocCtrl, out inLocCtrl, out inLoc);
			gr.DrawBezier(m_LinkPen, outLoc, outLocCtrl, inLocCtrl, inLoc);
		}


		private void ComputeBezierPoints(LinkBound input, LinkBound output, out Point outLoc, out Point outLocCtrl, out Point inLocCtrl, out Point inLoc)
		{
			if (input != null)
			{
				inLoc = input.Location;
				if (input.Parent != this)
					inLoc = this.PointToClient(input.PointToScreen(new Point()));
				inLoc.X += 1;
				inLoc.Y += (input.Height / 2);
			}
			else
				inLoc = m_CurrentMousePosition;

			if (output != null)
			{
				outLoc = output.Location;
				if (output.Parent != this)
					outLoc = this.PointToClient(output.PointToScreen(new Point()));
				outLoc.X += output.Width - 1;
				outLoc.Y += (output.Height / 2);
			}
			else
				outLoc = m_CurrentMousePosition;


			outLocCtrl = new Point(outLoc.X + 50, outLoc.Y);
			inLocCtrl = new Point(inLoc.X - 50, inLoc.Y);
		}




		protected override void OnDragEnter(DragEventArgs drgevent)
		{
			base.OnDragEnter(drgevent);
			
			drgevent.Effect = DragDropEffects.None;

			bool passtn = drgevent.Data.GetDataPresent(typeof(PassTreeNode));
			bool textn = drgevent.Data.GetDataPresent(typeof(TextureTreeNode));
			if (passtn || textn)
				drgevent.Effect = DragDropEffects.Copy;
		}

		protected override void OnDragDrop(DragEventArgs drgevent)
		{
			base.OnDragDrop(drgevent);

			Point pos = this.PointToClient(new Point(drgevent.X, drgevent.Y));
			PassTreeNode passtn = (PassTreeNode)drgevent.Data.GetData(typeof(PassTreeNode));
			TextureTreeNode textn = (TextureTreeNode)drgevent.Data.GetData(typeof(TextureTreeNode));

			if (passtn != null)
			{
				AddEffect(pos.X, pos.Y, passtn);
				ProjectModified();
			}
			else if (textn != null)
			{
				AddTexture(textn.TextureRelativeFilename, pos.X, pos.Y, textn.Texture);
				ProjectModified();
			}
		}

		private void OnEffectEdit(CustomEffect customEffect)
		{
			if (EffectEdit != null)
				EffectEdit(customEffect);
		}


		private void ctxNewScene_Click(object sender, EventArgs e)
		{
			AddScene(m_MenuLocation.X, m_MenuLocation.Y);
		}

		private void ctxNewPreviousFrame_Click(object sender, EventArgs e)
		{
			AddPreviousFrame(m_MenuLocation.X, m_MenuLocation.Y);
		}

		internal EffectWorkflowItem AddScene(int x, int y)
		{
			LinkBound dummy;
			return (AddInputOutput("Scene", PrimitiveItemType.Scene, x, y, Color.FromArgb(240, 255, 240), Color.FromArgb(128, 255, 128), IOMode.Output, out dummy));
		}

		internal EffectWorkflowItem AddPreviousFrame(int x, int y)
		{
			LinkBound dummy;
			return (AddInputOutput("Previous Frame", PrimitiveItemType.PreviousFrame, x, y, Color.FromArgb(255, 255, 240), Color.FromArgb(255, 255, 128), IOMode.Output, out dummy));
		}

		internal EffectWorkflowItem AddResult(int x, int y)
		{
			m_MenuLocation = new Point(this.Width - 70, this.Height - 50);
			return (AddInputOutput("Result", PrimitiveItemType.Output, x, y, Color.FromArgb(255, 240, 240), Color.FromArgb(255, 128, 128), IOMode.Input, out m_Result));
		}

		private EffectWorkflowItem AddInputOutput(string text, PrimitiveItemType type, int x, int y, Color backColor, Color selectionColor, IOMode ioMode, out LinkBound linkBound)
		{
			Win32.SuspendDrawing(this);

			LinkBound input = new LinkBound(ioMode, null);
			input.SetLinkManager(m_LinkManager);
			input.Text = text;
			input.SetBackColor(backColor);
			input.SelectionColor = selectionColor;

			EffectWorkflowItem item = new EffectWorkflowItem(input, type);
			item.SetLinkManager(m_LinkManager);
			item.SetEffectWorkflowManager(this);
			this.Controls.Add(item);
			item.RecreateParameters();

			item.Left = x;
			item.Top = y;

			Win32.ResumeDrawing(this);

			linkBound = input;
			return (item);
		}

		internal EffectWorkflowItem AddEffect(int x, int y, PassTreeNode passtn)
		{
			Win32.SuspendDrawing(this);

			EffectWorkflowItem item = new EffectWorkflowItem(passtn.Pass);
			item.EffectEdit += new CustomEffectHandler(OnEffectEdit);

			item.SetLinkManager(m_LinkManager);
			item.SetEffectWorkflowManager(this);
			this.Controls.Add(item);
			item.RecreateParameters();

			item.Left = x;
			item.Top = y;

			UnselectAllWorkflowItems();
			item.Selected();

			Win32.ResumeDrawing(this);

			return (item);
		}

		internal EffectWorkflowItem AddTexture(string filename, int x, int y, Texture tex)
		{
			Win32.SuspendDrawing(this);

			LinkBound input = new LinkBound(IOMode.Output, null);
			input.SetLinkManager(m_LinkManager);
			string text = Path.GetFileNameWithoutExtension(filename);
			input.Text = text;

			input.SetBackColor(Color.FromArgb(240, 240, 255));
			input.SelectionColor = Color.FromArgb(128, 128, 255);

			EffectWorkflowItem item = new EffectWorkflowItem(input, filename, tex);
			item.SetLinkManager(m_LinkManager);
			item.SetEffectWorkflowManager(this);
			this.Controls.Add(item);
			item.RecreateParameters();

			item.Left = x;
			item.Top = y;

			Win32.ResumeDrawing(this);

			return (item);
		}

		public IEnumerable<EffectWorkflowItem> EffectWorkflowItems
		{
			get
			{
				foreach (Control ctrl in this.Controls)
				{
					if (ctrl is EffectWorkflowItem)
						yield return ((EffectWorkflowItem)ctrl);
				}
			}
		}
	}
}
