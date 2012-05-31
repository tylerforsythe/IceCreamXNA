using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Globalization;

namespace Milkshake
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            string _projToLoad="";
            if (args.Length == 1)
            {
                _projToLoad = args[0];
            }
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.CurrentCulture = CultureInfo.InvariantCulture;
            Application.Run(new MilkshakeForm(_projToLoad));
        }
    }
}