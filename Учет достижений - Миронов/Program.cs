using System;
using System.Windows.Forms;

namespace Учет_достижений___Миронов
{
	static class Program
	{
		[STAThread]
		static void Main()
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			Application.Run(new FormAuthorization());
		}
	}
}
