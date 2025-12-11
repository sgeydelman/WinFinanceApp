using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using SLGAutomationLib;
using System.Runtime.InteropServices;

namespace WinFinanceApp
{
    internal static class Program
    {
        // DPI Awareness for .NET Framework
        [DllImport("user32.dll")]
        private static extern bool SetProcessDPIAware();

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // Enable DPI awareness for better scaling on high-DPI displays
            if (Environment.OSVersion.Version.Major >= 6)
            {
                SetProcessDPIAware();
            }

            System.Threading.Mutex mutex = new System.Threading.Mutex(false, "MyMutex");
            //  if (mutex.WaitOne(0, false))
            //   {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Logger.Create(Properties.Settings.Default.SetupPath);
            Application.Run(new FormMain());
            //  }
            // else
            // {
            //     MessageBox.Show("An Instance of WinFinance application is already running!");
            // }
        }
    }
}
