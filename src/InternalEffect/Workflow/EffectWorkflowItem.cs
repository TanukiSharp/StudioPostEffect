using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Microsoft.DirectX.Direct3D;
using System.Drawing.Drawing2D;
using PostEffectCore;

namespace InternalEffect
{
	public enum PrimitiveItemType
	{
		Effect,
		Scene,
		PreviousFrame,
		Texture,
		Output,
	}

	public partial class EffectWorkflowItem : UserControl
	{
		public event EffectWorkflowManager.CustomEffectHandler EffectEdit;

		private Random m_Random;
		private float m_RandomSeed;

		private LinkManager m_LinkManager;
		private EffectWorkflowManager m_WorkflowManager;

		private LinkBound m_LinkBound;
		private string m_TechniqueName;
		private string m_PassName;
		private CustomEffect m_CustomEffect;
		private CustomTechnique m_CustomTechnique;
		private CustomPass m_CustomPass;
		private Texture m_Texture;
		private string m_TextureFilename;
		private UIParameter m_UIParamaters = new UIParameter(null);

		private System.Drawing.Font m_Font;
		private PrimitiveItemType m_PrimitiveItemType;

		private bool m_Selected = false;

		public EffectWorkflowItem(CustomPass pass)
		{
			InitializeComponent();
			InternalInitialize();

			m_CustomPass = pass;
			m_CustomTechnique = m_CustomPass.ParentTechnique;
			m_CustomEffect = m_CustomTechnique.ParentEffect;

			m_PassName = m_CustomPass.Name;
			m_TechniqueName = m_CustomPass.ParentTechnique.Name;

			m_PrimitiveItemType = PrimitiveItemType.Effect;
		}

		public EffectWorkflowItem(LinkBound linkBound, string texFilename, Texture tex)
		{
			InitializeComponent();
			InternalInitialize();

			ctxEditCode.Visible = false;

			m_Texture = tex;
			m_TextureFilename = texFilename;
			m_LinkBound = linkBound;
			m_PrimitiveItemType = PrimitiveItemType.Texture;
		}

		public EffectWorkflowItem(LinkBound linkBound, PrimitiveItemType type)
		{
			InitializeComponent();
			InternalInitialize();

			if (linkBound.IOMode == IOMode.Output)
				ctxEditCode.Visible = false;
			else
				lblTitleTopMove.ContextMenuStrip = null;

			m_LinkBound = linkBound;
			m_PrimitiveItemType = type;
		}

		private void InternalInitialize()
		{
			m_Font = new System.Drawing.Font("arial", 7.0f, FontStyle.Regular);
			
			m_Random = new Random(Environment.TickCount);
			m_RandomSeed = (float)m_Random.NextDouble();

			CreateLinkBoundContextMenu();

			ctxEditCode.Image = GlobalContainer.ProjectImageList.Images[11];
			ctxRemoveWorkflowItem.Image = GlobalContainer.ProjectImageList.Images[8];

			lblTitleTopMove.MouseUp += new MouseEventHandler(OnTopMoveMouseUp);
			lblTitleTopMove.MouseDown += new MouseEventHandler(OnTopMoveMouseDown);
			lblTitleTopMove.MouseMove += new MouseEventHandler(OnTopMoveMouseMove);
			lblTitleTopMove.MouseDoubleClick += new MouseEventHandler(OnTopMoveMouseDoubleClick);

			pnlIO.MouseDown += new MouseEventHandler(OnPanelIOMouseDown);

			this.PerformLayout();
		}

		public PrimitiveItemType Type
		{
			get
			{
				return (m_PrimitiveItemType);
			}
		}

		public CustomEffect Effect
		{
			get
			{
				return (m_CustomEffect);
			}
		}

		internal void Selected()
		{
			lblTitleTopMove.BackColor = Color.Gray;
			m_Selected = true;
			m_WorkflowManager.WorkflowItemSelectionChangedNotification(this);
		}

		internal void Unselected()
		{
			lblTitleTopMove.BackColor = Color.Silver;
			m_Selected = false;
			m_WorkflowManager.WorkflowItemSelectionChangedNotification(this);
		}

		public bool IsSelected
		{
			get
			{
				return (m_Selected);
			}
		}




		private void OnPanelIOMouseDown(object sender, MouseEventArgs e)
		{
			m_WorkflowManager.UnselectAllWorkflowItems();
			this.Selected();
		}


		#region Mouse management

		private void OnTopMoveMouseUp(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
			{
				if (m_IsDragging == false)
				{
					if (Win32.IsKeyDown(Win32.VKEY_CONTROL))
					{
						if (m_Selected)
							Unselected();
						else
						{
							Win32.SuspendDrawing(GlobalContainer.SplitContainerWorkflowParams);
							Selected();
							Win32.ResumeDrawing(GlobalContainer.SplitContainerWorkflowParams);
						}
					}
					else
					{
						Win32.SuspendDrawing(GlobalContainer.SplitContainerWorkflowParams);
						m_WorkflowManager.UnselectAllWorkflowItems();
						Selected();
						Win32.ResumeDrawing(GlobalContainer.SplitContainerWorkflowParams);
					}
				}
				m_IsDragging = false;
			}
		}

		private bool m_IsDragging = false;
		private Point m_MoveMouseDown;
		private void OnTopMoveMouseDown(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
			{
				if (!m_Selected && !Win32.IsKeyDown(Win32.VKEY_CONTROL))
				{
					Win32.SuspendDrawing(GlobalContainer.SplitContainerWorkflowParams);
					m_WorkflowManager.UnselectAllWorkflowItems();
					Selected();
					Win32.ResumeDrawing(GlobalContainer.SplitContainerWorkflowParams);
				}

				m_IsDragging = false;
				m_MoveMouseDown = new Point(e.X, e.Y);
				this.BringToFront();
			}
			else if (e.Button == MouseButtons.Middle)
				RemoveMyself();
		}

		private void OnTopMoveMouseMove(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
			{
				if (m_IsDragging)
				{
					if (m_Selected == false)
						Selected();
					foreach (EffectWorkflowItem selectedItem in m_WorkflowManager.SelectedWorkflowItems)
					{
						selectedItem.Left += e.X - m_MoveMouseDown.X;
						selectedItem.Top += e.Y - m_MoveMouseDown.Y;
					}
					ProjectModified();
					m_LinkManager.RequestUpdate();
				}
				else
				{
					float dx = e.X - m_MoveMouseDown.X;
					float dy = e.Y - m_MoveMouseDown.Y;
					if (Math.Sqrt(dx * dx + dy * dy) > 3.0)
						m_IsDragging = true;
				}
			}
		}

		private void OnTopMoveMouseDoubleClick(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
			{
				if (EffectEdit != null)
					EffectEdit(m_CustomEffect);
			}
		}

		#endregion


		private void ProjectModified()
		{
			if (GlobalContainer.Project != null)
				GlobalContainer.Project.IsModified = true;
		}

		private void OnBottomMenuMouseDown(object sender, MouseEventArgs e)
		{
			m_WorkflowManager.UnselectAllWorkflowItems();
			this.Selected();
		}

		public void SetLinkManager(LinkManager linkManager)
		{
			m_LinkManager = linkManager;
		}

		public void SetEffectWorkflowManager(EffectWorkflowManager workflowManager)
		{
			m_WorkflowManager = workflowManager;
		}


		private LinkBound m_EffectOutput;
		private LinkBound[] m_EffectInputs;

		public LinkBound EffectOutput
		{
			get
			{
				return (m_EffectOutput);
			}
		}

		public LinkBound[] EffectInputs
		{
			get
			{
				return (m_EffectInputs);
			}
		}

		public UIParameter UIComponents
		{
			get
			{
				return (m_UIParamaters);
			}
		}

		public string TextureFilename
		{
			get
			{
				return (m_TextureFilename);
			}
		}

		public CustomPass CustomPass
		{
			get
			{
				return (m_CustomPass);
			}
		}

		internal int GetIndexOfLinkBound(LinkBound lb)
		{
			ControlCollection ctrls = pnlIO.Controls;
			for (int i = 0; i < ctrls.Count; i++)
			{
				if (ctrls[i] == lb)
					return (i);
			}
			return (-1);
		}

		internal void RecreateParameters()
		{
			if (m_PrimitiveItemType == PrimitiveItemType.Effect)
				RecreateParametersCustomPass();
			else if (m_PrimitiveItemType == PrimitiveItemType.Output)
				RecreateParametersInputLinkBound();
			else /*if (m_PrimitiveItemType == PrimitiveItemType.Scene || m_PrimitiveItemType == PrimitiveItemType.PreviousFrame || m_PrimitiveItemType == PrimitiveItemType.Texture)*/
				RecreateParametersOutputLinkBound();
		}


		private ContextMenuStrip m_LinkBoundContextMenu;
		private ToolStripMenuItem m_ViewFrameMenu;
		private void CreateLinkBoundContextMenu()
		{
			m_LinkBoundContextMenu = new ContextMenuStrip();
			m_ViewFrameMenu = new ToolStripMenuItem();
			m_ViewFrameMenu.Click += new EventHandler(OnViewFrameMenuClick);
			m_LinkBoundContextMenu.Items.Add(m_ViewFrameMenu);
		}

		private void OnViewFrameMenuClick(object sender, EventArgs e)
		{
			ToolStripMenuItem menu = (ToolStripMenuItem)sender;
			m_DebugViewFrame = (LinkBound)menu.Tag;
		}

		private void OnLinkBoundMouseClick(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Right)
			{
				LinkBound linkBound = (LinkBound)sender;

				if (linkBound.AttachedObject.Type != PrimitiveItemType.Effect)
					return;

				if (linkBound.IOMode == IOMode.Input)
					m_ViewFrameMenu.Text = "View input frame...";
				else
					m_ViewFrameMenu.Text = "View output frame...";
				m_ViewFrameMenu.Tag = linkBound;
				m_LinkBoundContextMenu.Show(this, e.X, e.Y);
			}
		}




		private void RecreateParametersInputLinkBound()
		{
			// for 'Output'

			if (m_LinkManager == null)
				return;

			int height = 0;
			int maxwidth = 0;

			RemoveLinks();

			m_UIParamaters.Clear();
			pnlIO.Controls.Clear();

			string name = m_LinkBound.Name;
			this.Name = name;

			int tmpWidth = lblTitleTopMove.Width;
			int tmpHeight = lblTitleTopMove.Height;

			lblTitleTopMove.AutoSize = true;
			lblTitleTopMove.Text = "";
			maxwidth = Math.Max(maxwidth, lblTitleTopMove.Width);

			lblTitleTopMove.AutoSize = false;
			lblTitleTopMove.Width = tmpWidth;
			lblTitleTopMove.Height = tmpHeight + 1;
			lblTitleTopMove.Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right;

			m_LinkBound.AttachedObject = this;

			m_LinkBound.AutoSize = true;
			m_LinkBound.Font = m_Font;
			m_LinkBound.MouseClick += new MouseEventHandler(OnLinkBoundMouseClick);
			pnlIO.Controls.Add(m_LinkBound);

			m_LinkBound.SetLinkManager(m_LinkManager);

			m_LinkBound.Left = 0;
			m_LinkBound.Top = height;
			m_LinkManager.AddLinkBound(m_LinkBound);

			m_LinkBound.MouseDown += new MouseEventHandler(OnPanelIOMouseDown);

			height += m_LinkBound.Height;
			maxwidth = Math.Max(maxwidth, m_LinkBound.Width);

			m_EffectInputs = new LinkBound[] { m_LinkBound };

			int widthDiff = this.Width - pnlIO.ClientSize.Width;
			int heightDiff = this.Height - pnlIO.ClientSize.Height;

			Size size = new Size(maxwidth + widthDiff, height + heightDiff);

			this.MinimumSize = size;
			this.MaximumSize = size;
			this.Size = size;

			this.Invalidate();
			this.Update();
		}



		private void RecreateParametersOutputLinkBound()
		{
			// for 'Scene' and 'Previous Frame'

			if (m_LinkManager == null)
				return;

			int height = 0;
			int maxwidth = 0;

			RemoveLinks();

			m_UIParamaters.Clear();
			pnlIO.Controls.Clear();

			this.Name = m_LinkBound.Name;

			int tmpWidth = lblTitleTopMove.Width;
			int tmpHeight = lblTitleTopMove.Height;

			lblTitleTopMove.AutoSize = true;
			lblTitleTopMove.Text = "";
			maxwidth = Math.Max(maxwidth, lblTitleTopMove.Width);

			lblTitleTopMove.AutoSize = false;
			lblTitleTopMove.Width = tmpWidth;
			lblTitleTopMove.Height = tmpHeight + 1;
			lblTitleTopMove.Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right;

			//m_EffectInputs = new LinkBound[0];
			m_EffectInputs = null;

			m_LinkBound.AttachedObject = this;
			m_LinkBound.AutoSize = true;
			m_LinkBound.Font = m_Font;
			m_LinkBound.MouseClick += new MouseEventHandler(OnLinkBoundMouseClick);
			//m_LinkBound.Name = m_LinkBound.Name;
			//m_LinkBound.Text = m_LinkBound.Name;
			pnlIO.Controls.Add(m_LinkBound);

			m_LinkBound.SetLinkManager(m_LinkManager);
			m_LinkBound.Left = pnlIO.ClientSize.Width - m_LinkBound.Width;
			m_LinkBound.Top = pnlIO.ClientSize.Height - m_LinkBound.Height;
			m_LinkBound.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;

			m_EffectOutput = m_LinkBound;
			m_LinkManager.AddLinkBound(m_LinkBound);

			height += m_LinkBound.Height;
			maxwidth = Math.Max(maxwidth, m_LinkBound.Width);

			int widthDiff = this.Width - pnlIO.ClientSize.Width;
			int heightDiff = this.Height - pnlIO.ClientSize.Height;

			Size size = new Size(maxwidth + widthDiff, height + heightDiff);

			this.MinimumSize = size;
			this.MaximumSize = size;
			this.Size = size;

			this.Invalidate();
			this.Update();
		}






		private void RecreateParametersCustomPass()
		{
			if (m_LinkManager == null)
				return;

			LinkBound linkBound;

			int height = 0;
			int maxwidth = 0;

			RemoveLinks();

			List<LinkBound> inputs = new List<LinkBound>();

			m_UIParamaters.Clear();
			pnlIO.Controls.Clear();

			string name = string.Format("{0}.{1}", m_TechniqueName, m_PassName);

			string text;
			if (m_CustomTechnique.Passes.Length == 1)
				text = m_PassName;
			else
				text = string.Format("{0}.{1}", m_TechniqueName, m_PassName);
			this.Name = name;

			int tmpWidth = lblTitleTopMove.Width;
			int tmpHeight = lblTitleTopMove.Height;

			lblTitleTopMove.AutoSize = true;
			lblTitleTopMove.Text = text;
			maxwidth = Math.Max(maxwidth, lblTitleTopMove.Width);

			lblTitleTopMove.AutoSize = false;
			lblTitleTopMove.Width = tmpWidth;
			lblTitleTopMove.Height = tmpHeight + 1;
			lblTitleTopMove.Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right;

			foreach (CustomParameter prm in m_CustomEffect.Parameters)
			{
				ParameterType type = prm.Type;
				ParameterClass cls = prm.Class;

				if (prm.Name == "PIXEL_SIZE" || prm.Name == "RENDER_TARGET_SIZE" || prm.Name == "RANDOMSEED")
					continue;

				UIParameter uip = null;

				try
				{
					uip = new UIParameter(prm);
					if (uip.Count == 1)
						m_UIParamaters.SetParameter(uip);
					else if (uip.Count > 1)
						m_UIParamaters.Add(uip);

					m_UIParamaters.SetUIValues();
				}
				catch (InvalidDataException)
				{
					continue;
				}

				if (!(cls == ParameterClass.Object &&
					(type == ParameterType.Texture ||
					type == ParameterType.Texture1D ||
					type == ParameterType.Texture2D ||
					type == ParameterType.Texture3D ||
					type == ParameterType.TextureCube)))
					continue;

				linkBound = new LinkBound(IOMode.Input, prm);

				linkBound.AttachedObject = this;

				linkBound.ForeColor = Color.Blue;
				linkBound.SetBackColor(linkBound.BackColor);
				linkBound.AutoSize = true;
				linkBound.Font = m_Font;
				linkBound.MouseClick += new MouseEventHandler(OnLinkBoundMouseClick);
				linkBound.Text = prm.Name;
				pnlIO.Controls.Add(linkBound);
			
				linkBound.SetLinkManager(m_LinkManager);

				linkBound.Left = 0;
				linkBound.Top = height;
				m_LinkManager.AddLinkBound(linkBound);

				//linkBound.MouseDown += new MouseEventHandler(OnPanelIOMouseDown);
				inputs.Add(linkBound);

				height += linkBound.Height;
				maxwidth = Math.Max(maxwidth, linkBound.Width);
			}
			m_EffectInputs = inputs.ToArray();

			linkBound = new LinkBound(); // output, no parameter
			linkBound.AttachedObject = this;
			linkBound.AutoSize = true;
			linkBound.Font = m_Font;
			linkBound.MouseClick += new MouseEventHandler(OnLinkBoundMouseClick);
			linkBound.Name = "Output";
			linkBound.Text = "Output";
			pnlIO.Controls.Add(linkBound);

			linkBound.SetLinkManager(m_LinkManager);
			linkBound.ForeColor = Color.Red;
			linkBound.SetBackColor(linkBound.BackColor);
			linkBound.Left = pnlIO.ClientSize.Width - linkBound.Width;
			linkBound.Top = pnlIO.ClientSize.Height - linkBound.Height;
			linkBound.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;

			m_EffectOutput = linkBound;
			m_LinkManager.AddLinkBound(linkBound);

			height += linkBound.Height;
			maxwidth = Math.Max(maxwidth, linkBound.Width);

			int widthDiff = this.Width - pnlIO.ClientSize.Width;
			int heightDiff = this.Height - pnlIO.ClientSize.Height;

			Size size = new Size(maxwidth + widthDiff, height + heightDiff);

			this.MinimumSize = size;
			this.MaximumSize = size;
			this.Size = size;

			this.Invalidate();
			this.Update();
		}

		private void RecreateUIParameters()
		{
			UIParameter newRootUIParameter = new UIParameter(null);

			foreach (CustomParameter prm in m_CustomEffect.Parameters)
			{
				IParameterUI found = m_UIParamaters.Find(delegate(IParameterUI p) { return (p.Name == prm.Name); });

				string values = null;
				string minimums = null;
				string maximums = null;
				if (found != null)
				{
					minimums = found.GetMinimums();
					maximums = found.GetMaximums();
					values = found.GetValues();
					//m_UIParamaters.Remove(found);
				}

				ParameterType type = prm.Type;
				ParameterClass cls = prm.Class;

				if (prm.Name == "PIXEL_SIZE" || prm.Name == "RENDER_TARGET_SIZE" || prm.Name == "RANDOMSEED")
					continue;

				UIParameter uip = null;

				try
				{
					uip = new UIParameter(prm);
					if (uip.Count == 1)
						newRootUIParameter.SetParameter(uip);
					else if (uip.Count > 1)
						newRootUIParameter.Add(uip);

					uip.SetMinimums(minimums);
					uip.SetMaximums(maximums);

					if (values == null)
						uip.LoadDefault();
					else
						uip.SetValues(values);

					uip.SetUIValues();
				}
				catch (InvalidDataException)
				{
					continue;
				}
			}

			m_UIParamaters.Clear();
			m_UIParamaters = newRootUIParameter;
			PropertyGrid grdUIParameters = GlobalContainer.UIParametersPropertyGrid;
			Win32.SuspendDrawing(grdUIParameters);
			grdUIParameters.SelectedObject = m_UIParamaters;
			Win32.ResumeDrawing(grdUIParameters);
		}

		private void RemoveLinks()
		{
			foreach (LinkBound linkBound in pnlIO.Controls)
				m_LinkManager.RemoveLink(linkBound);
		}

		private void ctxRemoveWorkflowItem_Click(object sender, EventArgs e)
		{
			RemoveMyself();
		}

		private void ctxEditCode_Click(object sender, EventArgs e)
		{
			if (EffectEdit != null)
				EffectEdit(m_CustomEffect);
		}

		private void RemoveMyself()
		{
			if (m_PrimitiveItemType == PrimitiveItemType.Output)
				return;

			RemoveLinks();

			// remove all LinkBound
			foreach (LinkBound linkBound in pnlIO.Controls)
				m_LinkManager.RemoveLinkBound(linkBound);

			m_WorkflowManager.Controls.Remove(this);
			m_WorkflowManager.FireWorkflowItemRemovedEvent(this);

			ProjectModified();
		}

		public void CheckUnderlyingEffect()
		{
			if (m_CustomPass == null)
				return;

			if (m_CustomEffect.CheckEffect())
			{
				CustomTechnique techFound = m_CustomEffect.FindTechnique(m_TechniqueName);
				if (techFound == null)
				{
					RemoveMyself();
					return;
				}
				CustomPass passFound = m_CustomEffect.FindPass(techFound, m_PassName);
				if (passFound == null)
				{
					RemoveMyself();
					return;
				}

				m_CustomTechnique = techFound;
				m_CustomPass = passFound;
				RecreateUIParameters();
				m_FaileCount = 15;
			}
		}

		private LinkBound m_DebugViewFrame = null;

		private int m_FaileCount = 15;

		public Texture ProcessEffect(PostEffectData data)
		{
			if (m_CustomPass == null && m_Texture != null)
				return (m_Texture);

			Dictionary<string, Texture> textures = new Dictionary<string, Texture>();

			foreach (LinkBound input in pnlIO.Controls)
			{
				if (input.IOMode != IOMode.Input)
					continue;

				LinkBound output = input.OtherBound;
				if (output == null)
					continue;

				Texture curTex = null;

				PrimitiveItemType type = output.AttachedObject.Type;
				if (type != PrimitiveItemType.Effect)
				{
					if (type == PrimitiveItemType.Scene)
						curTex = data.SceneFrame;
					else if (type == PrimitiveItemType.PreviousFrame)
						curTex = data.PreviousFrame;
					else
						curTex = output.AttachedObject.m_Texture;
				}
				else if (output.AttachedObject is EffectWorkflowItem)
				{
					EffectWorkflowItem seqWorker = (EffectWorkflowItem)output.AttachedObject;
					curTex = seqWorker.ProcessEffect(data);
				}

				if (curTex != null)
				{
					if (m_DebugViewFrame == input)
					{
						frmShowFrame frame = new frmShowFrame(TextureToBitmap(curTex));
						frame.Show(GlobalContainer.MainForm);
						m_DebugViewFrame = null;
					}

					textures.Add(input.Name, curTex);
				}

				//Debug.WorkflowSequence.Add(string.Format("{0} -> {1}", output.Name, input.Name));
			}

			/*
			if (m_EffectInputs.Length == 1)
				Debug.WorkflowSequence.Add(string.Format("{0} -> {1}", m_EffectInputs[0].Name, m_EffectOutput.Name));
			else
			{
				List<string> tmp = new List<string>();
				foreach (LinkBound lbtmp in m_EffectInputs)
					tmp.Add(lbtmp.Name);
				Debug.WorkflowSequence.Add(string.Format("({0}) -> {1}", string.Join(" + ", tmp.ToArray()), m_EffectOutput.Name));
			}
			*/

			Effect efx = m_CustomEffect.Effect;

			try
			{
				// set technique
				efx.Technique = m_CustomTechnique.Handle;
			}
			catch
			{
				m_FaileCount--;
				if (m_FaileCount <= 0)
					RemoveMyself();

				CustomTechnique techFound = m_CustomEffect.FindTechnique(m_TechniqueName);
				if (techFound == null)
					return (data.SceneFrame);

				m_CustomTechnique = techFound;

				CustomPass passFound = m_CustomEffect.FindPass(techFound, m_PassName);
				if (passFound == null)
					return (data.SceneFrame);

				m_CustomPass = passFound;
			}

			data.EffectRender.BeginRender();

			// begin effect
			efx.Begin(FX.None);

			// set values
			foreach (CustomParameter prm in m_CustomEffect.Parameters)
			{
				Texture tex;
				if (textures.TryGetValue(string.Format("[{0}].[{1}]", this.Name, prm.Name), out tex))
					efx.SetValue(prm.Handle, tex);
			}

			// update shader values
			m_UIParamaters.SetUIValues();

			if (m_CustomEffect.PixelSize != null)
				efx.SetValue(m_CustomEffect.PixelSize.Handle, new float[] { 1.0f / (float)GlobalContainer.ViewportDX.Width, 1.0f / (float)GlobalContainer.ViewportDX.Height });

			if (m_CustomEffect.RenderTargetSize != null)
				efx.SetValue(m_CustomEffect.RenderTargetSize.Handle, new float[] { (float)GlobalContainer.ViewportDX.Width, (float)GlobalContainer.ViewportDX.Height });

			if (m_CustomEffect.RandomSeed != null)
				efx.SetValue(m_CustomEffect.RandomSeed.Handle, m_RandomSeed);

			// set pass
			efx.BeginPass(m_CustomPass.PassNumber);
			data.Square.Draw();
			efx.EndPass();

			// end effect
			efx.End();

			Texture outputTexture = data.EffectRender.EndRender();

			if (m_DebugViewFrame == m_EffectOutput)
			{
				frmShowFrame frame = new frmShowFrame(TextureToBitmap(outputTexture));
				frame.Show(GlobalContainer.MainForm);
				m_DebugViewFrame = null;
			}

			// display image for a future step-to-step render chain option

			return (outputTexture);
		}


		private Bitmap TextureToBitmap(Texture texture)
		{
			Surface surface = texture.GetSurfaceLevel(0);
			int width = surface.Description.Width;
			int height = surface.Description.Height;

			Microsoft.DirectX.GraphicsStream gs = TextureLoader.SaveToStream(ImageFileFormat.Bmp, texture);
			Bitmap bmp = new Bitmap(width, height, System.Drawing.Imaging.PixelFormat.Format32bppRgb);
			System.Drawing.Imaging.BitmapData bd = bmp.LockBits(new Rectangle(0, 0, width, height), System.Drawing.Imaging.ImageLockMode.WriteOnly, System.Drawing.Imaging.PixelFormat.Format32bppRgb);

			byte[] argb = new byte[gs.Length - 54];
			System.Runtime.InteropServices.Marshal.Copy(new IntPtr(gs.InternalData.ToInt32() + 54), argb, 0, argb.Length);

			int len = (int)argb.Length / 4;
			for (int i = 0; i < len; i++)
			{
				System.Runtime.InteropServices.Marshal.Copy(argb, (i * 4), new IntPtr(bd.Scan0.ToInt32() + (i * 4)), 4);
			}

			bmp.UnlockBits(bd);
			bmp.RotateFlip(RotateFlipType.RotateNoneFlipY);
			gs.Close();

			return (bmp);
		}
	}
}
