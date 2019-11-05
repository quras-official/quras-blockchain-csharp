using System;
using System.IO;
using System.Xml.Linq;
using System.Reflection;
using System.Windows.Forms;
using System.Diagnostics;
using System.Runtime.InteropServices;

using Pure.Core;
using Pure.Network;
using Pure.Implementations.Blockchains.LevelDB;

using Quras_gui_SP.UI;
using Quras_gui_SP.Global;

namespace Quras_gui_SP
{
    static class Program
    {
        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            using (FileStream fs = new FileStream("error.log", FileMode.Create, FileAccess.Write, FileShare.None))
            using (StreamWriter w = new StreamWriter(fs))
            {
                LogManager.PrintErrorLogs(w, (Exception)e.ExceptionObject);
            }
        }

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            if (Environment.OSVersion.Version.Major >= 6)
                SetProcessDPIAware();

            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

#if DEBUG
            Debug.WriteLine("Wallet starting ...");
            //AllocConsole();
#endif

            XDocument xdoc = null;

            try
            {
                xdoc = XDocument.Load("http://localhost/pure/update/update.xml");
            }
            catch { }
            if (xdoc != null)
            {
                Version version = Assembly.GetExecutingAssembly().GetName().Version;
                Version minimum = Version.Parse(xdoc.Element("update").Attribute("minimum").Value);
                if (version < minimum)
                {
                    using (UpdateWindow dialog = new UpdateWindow(xdoc))
                    {
                        dialog.ShowDialog();
                    }
                    return;
                }
            }

            Form startForm;
            startForm = new MainForm();

            if (File.Exists(Constant.PEER_STATE_PATH))
                using (FileStream fs = new FileStream(Constant.PEER_STATE_PATH, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    LocalNode.LoadState(fs);
                }
            using (Blockchain.RegisterBlockchain(new LevelDBBlockchain("chain")))
            using (Constant.LocalNode = new LocalNode())
            {
                Constant.LocalNode.UpnpEnabled = true;
                Application.Run(startForm);
            }
            using (FileStream fs = new FileStream(Constant.PEER_STATE_PATH, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                LocalNode.SaveState(fs);
            }
        }

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern bool SetProcessDPIAware();

        [System.Runtime.InteropServices.DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool AllocConsole();
    }
}
