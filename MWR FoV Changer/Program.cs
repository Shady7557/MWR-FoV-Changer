using CurtLog;
using System;
using System.Text;
using System.Windows.Forms;

namespace MWR_FoV_Changer
{
    internal static class Program
    {
        private static readonly StringBuilder _stringBuilder = new StringBuilder();

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }

        private static void InitLog()
        {
            Log.Settings.CustomLogHeader = _stringBuilder.Clear().Append("===========").Append(Application.ProductName).Append(" Log File===========").ToString();
            Log.Settings.PerformanceOptions = Log.Performance.StandardPerformance;
            Log.Settings.LogHeaderOptions = Log.LogHeader.CustomHeader;

            var logLocation = _stringBuilder.Clear().Append(PathInfos.LogsPath).Append(@"\CFC_").Append(DateTime.Now.ToString("d")).Replace("/", "-").Append(".log").ToString();
            Log.InitializeLogger(logLocation);

        }
    }
}
