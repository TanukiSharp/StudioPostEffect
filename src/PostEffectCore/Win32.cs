using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace PostEffectCore
{
	public static class Win32
	{
		private const int WM_SETREDRAW = 0x000B;

		public static void SuspendDrawing(Control ctrl)
		{
			SendMessage(ctrl.Handle, WM_SETREDRAW, 0, 0);
		}

		public static void ResumeDrawing(Control ctrl)
		{
			SendMessage(ctrl.Handle, WM_SETREDRAW, 1, 0);
			ctrl.Invalidate();
			ctrl.Refresh();
		}

		[DllImport("user32.dll")]
		private static extern IntPtr SendMessage(IntPtr hWnd, int wMsg, int wParam, int lParam);


		public const int VKEY_TAB = 0x09;
		public const int VKEY_SHIFT = 0x10;
		public const int VKEY_CONTROL = 0x11;
		public const int VKEY_ALT = 0x12;

		public const int WM_MOUSEWHEEL = 0x020A;

		public static bool IsKeyDown(int vKey)
		{
			short keyState = GetAsyncKeyState(vKey);
			// bit 15 is set or unset to indicate pressed or released
			keyState &= (short)~1;
			return (keyState != 0);
		}

		[DllImport("user32.dll")]
		private static extern short GetAsyncKeyState(int vKey);
	}
}
