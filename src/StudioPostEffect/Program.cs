using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Globalization;

namespace StudioPostEffect
{
	static class Program
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main()
		{
			Application.CurrentCulture = new CultureInfo("en-US"); // for float formatting/parsing
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			Application.Run(new frmMain());
		}
	}
}
