using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace InternalEffect
{
	public class Link
	{
		private LinkBound m_Output;
		private LinkBound m_Input;
		private string m_Name;

		public Link(LinkBound output, LinkBound input)
		{
			this.Name = ControlNaming.GetNextName<Link>();

			if (output == null || input == null)
				throw new ArgumentException("Both input and output parameters must not be null");
			if (input == output)
				throw new ArgumentException("Both input and output parameters must be different");

			m_Output = output;
			m_Input = input;
		}

		public string Name
		{
			get
			{
				return (m_Name);
			}
			set
			{
				m_Name = value;
			}
		}

		public LinkBound Output
		{
			get
			{
				return (m_Output);
			}
		}

		public LinkBound Input
		{
			get
			{
				return (m_Input);
			}
		}

		public bool IsEmpty
		{
			get
			{
				return (m_Output == null && m_Input == null);
			}
		}
	}







	public class LinkManager
	{
		public event EventHandler LinksUpdate;

		private List<LinkBound> m_LinkBounds = new List<LinkBound>();
		private List<Link> m_Links = new List<Link>();

		public void AddLinkBound(LinkBound linkBound)
		{
			m_LinkBounds.Add(linkBound);
			linkBound.MouseDown += new MouseEventHandler(OnLinkBoundMouseDown);
			linkBound.MouseUp += new MouseEventHandler(OnLinkBoundMouseUp);
			linkBound.MouseMove += new MouseEventHandler(OnLinkBoundMouseMove);
		}

		private LinkBound m_MouseOn = null;
		private void OnLinkBoundMouseMove(object sender, MouseEventArgs e)
		{
			Control ctrl = GetControlAt(e.X, e.Y);
			if (ctrl is LinkBound)
			{
				if (m_MouseOn == null)
				{
					m_MouseOn = (LinkBound)ctrl;
					m_MouseOn.Select();
				}
				else if (m_MouseOn != ctrl)
				{
					m_MouseOn.Unselect();
					m_MouseOn = (LinkBound)ctrl;
					m_MouseOn.Select();
				}
			}
			else if (m_MouseOn != null && m_MouseOn != ctrl)
			{
				m_MouseOn.Unselect();
				m_MouseOn = null;
			}
		}

		public void RemoveLinkBound(LinkBound linkBound)
		{
			m_LinkBounds.Remove(linkBound);
			linkBound.MouseDown -= new MouseEventHandler(OnLinkBoundMouseDown);
			linkBound.MouseUp -= new MouseEventHandler(OnLinkBoundMouseUp);
			linkBound.MouseMove -= new MouseEventHandler(OnLinkBoundMouseMove);
		}

		public void ClearLinkBounds()
		{
			foreach (LinkBound linkBound in m_LinkBounds)
			{
				linkBound.MouseDown -= new MouseEventHandler(OnLinkBoundMouseDown);
				linkBound.MouseUp -= new MouseEventHandler(OnLinkBoundMouseUp);
				linkBound.MouseMove -= new MouseEventHandler(OnLinkBoundMouseMove);
			}
			m_LinkBounds.Clear();
		}


		private void ProjectModified()
		{
			if (GlobalContainer.Project != null)
				GlobalContainer.Project.IsModified = true;
		}


		private void OnLinkBoundMouseUp(object sender, MouseEventArgs e)
		{
			if (e.Button != MouseButtons.Left)
				return;

			Control ctrl = GetControlAt(e.X, e.Y);

			if (ctrl is LinkBound)
			{
				LinkBound final = (LinkBound)ctrl;
				//------------------------------------------------------------------

				if (m_MouseDownLinkBound == final)
				{
					// same LinkBound -> cancel
					LinkingCanceled();
				}
				else
				{
					if (m_MouseDownLinkBound.Parent == final.Parent)
					{
						LinkingCanceled();
						return;
					}

					if (m_MouseDownLinkBound.IOMode == final.IOMode)
					{
						LinkingCanceled();
						MessageBox.Show("Impossible to link input to input or output to output", "Link Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
						UnselectLinkBounds(final);
					}
					else if (m_MouseDownLinkBound.IOMode == IOMode.Input)
					{
						if (FinalizeLink(m_MouseDownLinkBound, final) == false)
						{
							LinkingCanceled();
							UnselectLinkBounds(final);
						}
					}
					else if (m_MouseDownLinkBound.IOMode == IOMode.Output)
					{
						if (FinalizeLink(final, m_MouseDownLinkBound) == false)
						{
							LinkingCanceled();
							UnselectLinkBounds(final);
						}
					}
				}

				//------------------------------------------------------------------
			}
			else
				LinkingCanceled();
		}

		private LinkBound m_MouseDownLinkBound;
		private void OnLinkBoundMouseDown(object sender, MouseEventArgs e)
		{
			if (e.Button != MouseButtons.Left)
				return;

			m_MouseDownLinkBound = (LinkBound)sender;
			RequestUpdate();
		}


		private Control GetControlAt(int x, int y)
		{
			if (m_MouseDownLinkBound == null)
				return (null);

			Point p = m_MouseDownLinkBound.PointToScreen(new Point(x, y));

			Control ctrl = m_EffectWorkflowManager;
			Control okCtrl = ctrl;

			while (true)
			{
				ctrl = ctrl.GetChildAtPoint(ctrl.PointToClient(p));
				if (ctrl == null)
					return (okCtrl);
				okCtrl = ctrl;
			}
		}


		private void UnselectLinkBounds(LinkBound final)
		{
			if (final != null)
				final.Unselect();
			if (m_MouseDownLinkBound != null)
				m_MouseDownLinkBound.Unselect();
			if (m_MouseOn != null)
				m_MouseOn.Unselect();
		}




		/*
		private void OnLinkBoundMouseDown(object sender, MouseEventArgs e)
		{
			if (e.Button != MouseButtons.Left)
				return;

			LinkBound secondClickedLinkBound = (LinkBound)sender;

			if (m_FirstClickedLinkBound == null)
			{
				m_FirstClickedLinkBound = secondClickedLinkBound;
				RequestUpdate();
			}
			else
			{
				bool firstIsEffect = m_FirstClickedLinkBound.Parent.GetType() == typeof(Panel);
				bool secondIsEffect = secondClickedLinkBound.Parent.GetType() == typeof(Panel);

				if (m_FirstClickedLinkBound == secondClickedLinkBound)
				{
					// same LinkBound -> cancel
					LinkingCanceled();
				}
				else
				{
					if ((firstIsEffect || secondIsEffect) &&
						m_FirstClickedLinkBound.Parent == secondClickedLinkBound.Parent)
					{
						LinkingCanceled();
						return;
					}

					if (m_FirstClickedLinkBound.IOMode == secondClickedLinkBound.IOMode)
					{
						LinkingCanceled();
						MessageBox.Show("Impossible to link input to input or output to output", "Link Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
					}
					else if (m_FirstClickedLinkBound.IOMode == IOMode.Input)
					{
						FinalizeLink(m_FirstClickedLinkBound, secondClickedLinkBound);
					}
					else if (m_FirstClickedLinkBound.IOMode == IOMode.Output)
					{
						FinalizeLink(secondClickedLinkBound, m_FirstClickedLinkBound);
					}
				}
			}
		}
		*/

		internal bool FinalizeLink(LinkBound a, LinkBound b)
		{
			if (LinkExists(a, b) == false)
			{
				LinkBound save1 = b.OtherBound;
				LinkBound save2 = a.OtherBound;

				Link newLink = CreateLink(b, a);

				bool isRecurs = IsRecursivePath(newLink);
				if (isRecurs == false)
				{
					RemoveLink(a);
					ProjectModified();
					m_Links.Add(newLink);
				}
				else
				{
					MessageBox.Show("This link creates a recursion", "Link Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
					b.OtherBound = save1;
					a.OtherBound = save2;
				}

				LinkingCanceled();
				return (!isRecurs);
			}
			return (false);
		}

		public void RequestUpdate()
		{
			if (LinksUpdate != null)
				LinksUpdate(this, null);
		}

		internal List<Link> Links
		{
			get
			{
				return (m_Links);
			}
		}

		internal LinkBound LinkingBound
		{
			get
			{
				return (m_MouseDownLinkBound);
			}
		}

		public void LinkingCanceled()
		{
			m_MouseDownLinkBound = null;
			RequestUpdate();
		}


		public bool IsRecursivePath(Link newLink)
		{
			List<LinkBound> knownElems = new List<LinkBound>();
			return (CheckRecursion(newLink, knownElems));
		}

		private bool CheckRecursion(Link lnk, List<LinkBound> knownElems)
		{
			knownElems.Add(lnk.Output);

			EffectWorkflowItem wfItem = lnk.Input.AttachedObject as EffectWorkflowItem;
			if (wfItem == null)
				return (false);

			foreach (Link l in FindLinksForRecursion(wfItem.EffectOutput, lnk))
			{
				EffectWorkflowItem item = l.Input.AttachedObject as EffectWorkflowItem;
				if (item == null)
					continue;

				if (knownElems.Exists(delegate(LinkBound lb) { return (item.EffectOutput == lb); }))
					return (true);

				if (CheckRecursion(l, knownElems))
					return (true);
			}
			return (false);
		}

		private List<Link> FindLinksForRecursion(LinkBound efxOutput, Link filter)
		{
			List<Link> result = new List<Link>();

			foreach (Link lnk in m_Links)
			{
				if (lnk.Input == filter.Input && lnk.Output == filter.Output)
					continue;
				if (lnk.Output == efxOutput)
					result.Add(lnk);
			}
			return (result);
		}



		public Link CreateLink(LinkBound output, LinkBound input)
		{
			output.OtherBound = input;
			input.OtherBound = output;

			Link lnk = new Link(output, input);
			return (lnk);
		}

		public bool RemoveLink(LinkBound linkBound)
		{
			if (linkBound == null)
				return (false);

			if (linkBound.IOMode == IOMode.Input)
			{
				Link link = m_Links.Find(delegate(Link lnk) { return (lnk.Input == linkBound); });
				if (link == null)
					return (false);

				if (link.Output != null)
					link.Output.OtherBound = null;

				bool res = m_Links.Remove(link);

				if (res)
					ProjectModified();

				return (res);
			}
			else
			{
				List<Link> links = m_Links.FindAll(delegate(Link lnk) { return (lnk.Output == linkBound); });

				bool res = false;
				foreach (Link link in links)
				{
					if (link.Input != null)
						link.Input.OtherBound = null;
					res |= m_Links.Remove(link);
				}

				if (res)
					ProjectModified();

				return (res);
			}
		}

		private bool LinkExists(LinkBound a, LinkBound b)
		{
			return (m_Links.Exists(delegate(Link lnk)
			{
				return ((lnk.Input == a && lnk.Output == b) || (lnk.Input == b && lnk.Output == a));
			}));
		}

		//public LinkBound FindOutput(LinkBound input)
		//{
		//    Link link = m_Links.Find(delegate(Link lnk) { return (lnk.Input == input); });
		//    if (link == null)
		//        return (null);
		//    return (link.Output);
		//}

		//public LinkBound[] FindAllInput(LinkBound output)
		//{
		//    List<LinkBound> inputs = new List<LinkBound>();
		//    m_Links.ForEach(
		//        delegate(Link lnk)
		//        {
		//            if (lnk.Output == output)
		//                inputs.Add(lnk.Input);
		//        });

		//    return (inputs.ToArray());
		//}

		private EffectWorkflowManager m_EffectWorkflowManager;
		internal void SetEffectWorkflowManager(EffectWorkflowManager effectWorkflowManager)
		{
			m_EffectWorkflowManager = effectWorkflowManager;
		}
	}
}
