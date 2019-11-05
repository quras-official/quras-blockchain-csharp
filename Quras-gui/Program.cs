using System;
using System.IO;
using System.Xml.Linq;
using System.Reflection;
using System.Windows.Forms;
using System.Security.Cryptography;

using Pure.Core;
using Pure.Network;
using Pure.Implementations.Blockchains.LevelDB;

using Quras_gui.FormUI;
using Quras_gui.Global;
using Quras_gui.Properties;

namespace Quras_gui
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            using (FileStream fs = new FileStream("error.log", FileMode.Create, FileAccess.Write, FileShare.None))
            using (StreamWriter w = new StreamWriter(fs))
            {
                LogManager.PrintErrorLogs(w, (Exception)e.ExceptionObject);
            }
        }

        [STAThread]
        static void Main()
        {
            if (Environment.OSVersion.Version.Major >= 6)
                SetProcessDPIAware();

            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            XDocument xdoc = null;
            try
            {
                xdoc = XDocument.Load("http://13.230.62.42/quras/update/update.xml");
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
                        if (dialog.DialogResult != DialogResult.Cancel)
                        {
                            return;
                        }
                    }
                }
            }
            SplashForm splash = new SplashForm();
            splash.Show();
            Application.DoEvents();
            int zkSnarksKeyStatus = Global.Utils.CheckZKSnarksKeyStatus();
            splash.Hide();
            if (zkSnarksKeyStatus != 0)
            {
                using (DownloadKeyForm dialog = new DownloadKeyForm(zkSnarksKeyStatus))
                {
                    dialog.ShowDialog();
                    if (dialog.DialogResult == DialogResult.Cancel)
                    {
                        return;
                    }
                }
            }

            Form startForm;
            startForm = new WelcomeForm();
            /*
            if (Settings.Default.LastWalletPath.Length == 0)
            {
                startForm = new WelcomeForm();
            }
            else
            {
                startForm = new RestoreWalletForm();
            }
            */
            FormManager.GetInstance().Push(startForm);

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
    }
}
