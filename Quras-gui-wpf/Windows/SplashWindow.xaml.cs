using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.IO;
using System.Xml.Linq;
using System.Reflection;
using System.Windows.Threading;

using Pure.Core;
using Pure.Network;
using Pure.Core.Anonoymous;
using Pure.Implementations.Blockchains.LevelDB;

using Quras_gui_wpf.Utils;
using Quras_gui_wpf.Global;
using Quras_gui_wpf.Properties;

namespace Quras_gui_wpf.Windows
{
    /// <summary>
    /// Interaction logic for SplashWindow.xaml
    /// </summary>
    public partial class SplashWindow : Window
    {
        private DispatcherTimer SplashTimer = new DispatcherTimer();
        private WelcomeWindow WelcomeWnd;
        private bool isLoadedBlockchain = false;
        private LANG iLang => Constant.GetLang();

        static Mutex _m;

        public SplashWindow()
        {
            InitializeComponent();
            InitInterface();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

            try
            {
                Mutex.OpenExisting("QURAS WALLET MUTEX");
                MessageBox.Show(StringTable.GetInstance().GetString("STR_WALLET_DUPPLICATION_ERROR", iLang), "QURAS");
                this.Close();
                return;
            }
            catch
            {
                _m = new Mutex(true, "QURAS WALLET MUTEX");
            }

            

            SplashTimer.Tick += new EventHandler(NextWindow);
            SplashTimer.Interval = new TimeSpan(0, 0, 3);
            SplashTimer.Start();

            Version localVersion = Assembly.GetExecutingAssembly().GetName().Version;
            Version serverVersion = GetNewestWalletVersionFromServer();

            if (localVersion < serverVersion)
            {
                // Show the Update Window;
            }
            else
            { 
                // Check the Key file Exception.

            }

            Task.Run(() =>
            {
                try
                {
                    string vkKeyPath = SettingsConfig.Default.VkKeyPath;
                    string pkKeyPath = SettingsConfig.Default.PkKeyPath;

                    vkKeyPath = System.IO.Path.GetFullPath(vkKeyPath);
                    pkKeyPath = System.IO.Path.GetFullPath(pkKeyPath);

                    if (Constant.isLoadedVK == false )
                    {
                        int ret = SnarkDllApi.Snark_DllInit(1, vkKeyPath.ToArray(), pkKeyPath.ToArray());

                        if (ret <= 0)
                        {
                            MessageBox.Show("Verify key cannot load.", "Error", MessageBoxButton.OK);
                            this.Close();
                            return;
                        }

                        Constant.isLoadedVK = true;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Verify key cannot load." + ex.ToString(), "Error", MessageBoxButton.OK);
                    this.Dispatcher.BeginInvoke(new Action(() =>
                    {
                        this.Close();
                    }));
                }

                try
                {
                    Blockchain.RegisterBlockchain(new LevelDBBlockchain("chain"));
                    this.Dispatcher.BeginInvoke(new Action(() =>
                    {
                        isLoadedBlockchain = true;
                    }));
                }
                catch (Exception ex)
                {
                    this.Dispatcher.BeginInvoke(new Action(() =>
                    {
                        MessageBox.Show("Cannot load blockchain db." + System.Environment.NewLine + ex.ToString(), "Error", MessageBoxButton.OK);
                        this.Close();
                    }));
                }
            });
        }

        /// <summary>
        /// Initialize the Interface of Splash Window.
        /// </summary>
        private void InitInterface()
        {
            Version version = Assembly.GetExecutingAssembly().GetName().Version;
            TxbVersion.Text = StringTable.GetInstance().GetString("STR_SPLASH_VERSION", iLang) + " " + version.ToString();
        }

        /// <summary>
        /// Process the Unhandled Exceptions and save errors as File.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            using (FileStream fs = new FileStream("error.log", FileMode.Create, FileAccess.Write, FileShare.None))
            using (StreamWriter w = new StreamWriter(fs))
            {
                LogManager.PrintErrorLogs(w, (Exception)e.ExceptionObject);
            }
        }

        private Version GetNewestWalletVersionFromServer()
        {
            Version minimum = new Version("1.0.0.1");
            XDocument xdoc = null;
            try
            {
                xdoc = XDocument.Load("http://13.230.62.42/quras/update/update.xml");
            }
            catch { }
            if (xdoc != null)
            {
                minimum = Version.Parse(xdoc.Element("update").Attribute("minimum").Value);
            }
            return minimum;
        }

        private void GridMouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                DragMove();
            }
            catch (Exception)
            {

            }
        }

        private void NextWindow(object sender, EventArgs args)
        {
            if (isLoadedBlockchain)
            {
                WelcomeWnd = new WelcomeWindow();
                WelcomeWnd.Show();

                SplashTimer.Stop();
                this.Close();
            }
        }
    }
}
