using System;
using System.IO;
using System.Windows;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Windows.Controls;

using Quras.Core;
using Quras.Wallets;
using Quras.Network;
using Quras.Implementations.Wallets.EntityFramework;

using Quras_gui_wpf.Utils;
using Quras_gui_wpf.Global;
using Quras_gui_wpf.Properties;
using System.Windows.Threading;
using System.Threading;

namespace Quras_gui_wpf.Windows
{
    /// <summary>
    /// Interaction logic for RestoreWalletWindow.xaml
    /// </summary>
    public partial class RestoreWalletWindow : Window
    {
        #region Members
        private MainWalletWindow MainWalletWnd;

        private UserWallet wallet = null;
        private LANG iLang => Constant.GetLang();
        private bool isNext = false;
        private bool isPrev = false;

        private DispatcherTimer engineTimer;

        private Thread OpenningWalletThread;
        private bool isOpenningWalletFinished = false;

        public class OpenningWallet
        {
            public RestoreWalletWindow parent;

            public string path;
            public string password;
            public void processOpenWallet()
            {
                try
                {
                    parent.wallet = UserWallet.Open(path, password);

                    Settings.Default.LastWalletPath = path;
                    Settings.Default.Save();
                }
                catch (Exception)
                {
                    parent.wallet = null;
                }
                parent.isOpenningWalletFinished = true;
            }
        }
        #endregion

        public RestoreWalletWindow()
        {
            InitializeComponent();

            InitInterface();
            RefreshLanguage();
        }

        public void RefreshLanguage()
        {
            TxbCommentHeader.Text = StringTable.GetInstance().GetString("STR_RW_COMMENT_HEADER", iLang);
            txbWalletPath.Tag = StringTable.GetInstance().GetString("STR_RW_TAG_WALLET_PATH", iLang);
            btnNext.Content = StringTable.GetInstance().GetString("STR_RW_NEXT", iLang);
        }

        private void btnBrowser_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.FileName = "wallet"; // Default file name
            dlg.DefaultExt = "db3";
            dlg.Filter = "Data Files (*.db3)|*.db3";

            // Show save file dialog box
            Nullable<bool> result = dlg.ShowDialog();

            // Process save file dialog box results
            if (result == true)
            {
                // Save document
                txbWalletPath.Text = dlg.FileName;
            }
        }

        /// <summary>
        /// Initialize the interface
        /// </summary>
        private void InitInterface()
        {
            string lastWalletPath = Settings.Default.LastWalletPath;
            txbWalletPath.Text = lastWalletPath;
        }

        /// <summary>
        /// Check all the field of this window.
        /// </summary>
        /// <returns></returns>
        private string CheckStatus()
        {
            if (txbWalletPath.Text.Length == 0)
            {
                return "STR_RW_ERR_WALLET_PATH";
            }

            if (txbPassword.Password.Length == 0)
            {
                return "STR_RW_ERR_PASSWORD_INPUT";
            }

            return "STR_RW_SUCCESS";
        }

        private void btnNext_Click(object sender, RoutedEventArgs e)
        {
            btnNext.IsEnabled = false;
            string checkField = CheckStatus();

            txbStatus.Text = StringTable.GetInstance().GetString(checkField);
            txbStatus.Visibility = Visibility.Visible;

            if (checkField != "STR_RW_SUCCESS")
            {
                btnNext.IsEnabled = true;
                return;
            }

            txbStatus.Text = StringTable.GetInstance().GetString(checkField);

            string walletPath = txbWalletPath.Text;
            string password = txbPassword.Password;

            OpenningWallet openningWallet = new OpenningWallet();
            openningWallet.parent = this;
            openningWallet.path = txbWalletPath.Text;
            openningWallet.password = txbPassword.Password;

            OpenningWalletThread = new Thread(openningWallet.processOpenWallet, 2 * 1024 * 1024);
            OpenningWalletThread.Start();

            if (engineTimer != null)
                engineTimer.Stop();

            engineTimer = new DispatcherTimer();
            engineTimer.Tick += this.dispatcherTimer_Tick;
            engineTimer.Interval = new TimeSpan(0, 0, 0, 0, 500);
            engineTimer.Start();

            btnNext.IsEnabled = false;

            /*Task.Run(() =>
            {
                UserWallet wallet;
                try
                {
                    wallet = UserWallet.Open(walletPath, password);

                    Settings.Default.LastWalletPath = walletPath;
                    Settings.Default.Save();

                    this.Dispatcher.BeginInvoke(new Action(() =>
                    {
                        MainWalletWnd = new MainWalletWindow(wallet);
                        MainWalletWnd.Show();

                        isNext = true;

                        this.Close();
                    }));
                }
                catch (CryptographicException)
                { 
                    
                    this.Dispatcher.BeginInvoke(new Action(() =>
                    {
                        txbStatus.Text = StringTable.GetInstance().GetString("STR_RW_ERR_INCORRECT_PASSWORD");
                        txbStatus.Visibility = Visibility.Visible;
                        btnNext.IsEnabled = true;
                    }));
                    return;
                }
                catch (FormatException ex)
                {
                    this.Dispatcher.BeginInvoke(new Action(() =>
                    {
                        txbStatus.Text = StringTable.GetInstance().GetString("STR_RW_ERR_UNKNOWN");
                        txbStatus.Visibility = Visibility.Visible;
                        LogManager.WriteExceptionLogs(ex);
                        btnNext.IsEnabled = true;
                    }));
                    return;
                }
            });*/
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            if (OpenningWalletThread != null)
            {
                OpenningWalletThread.Interrupt();
            }

            if (engineTimer != null)
            {
                engineTimer.Stop();
            }

            if (isNext == false && isPrev == false)
            {
                if (Constant.NotifyMessageMgr != null)
                {
                    Constant.NotifyMessageMgr.Stop();
                }

                if (Constant.LocalNode != null)
                {
                    using (FileStream fs = new FileStream(Constant.PEER_STATE_PATH, FileMode.Create, FileAccess.Write, FileShare.None))
                    {
                        LocalNode.SaveState(fs);
                    }

                    Constant.LocalNode.Dispose();
                }
                
                if (Blockchain.Default != null)
                {
                    Blockchain.Default.Dispose();
                }
            }
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            WelcomeWindow welcomeWnd = new WelcomeWindow();
            welcomeWnd.Show();
            isPrev = true;
            this.Close();
        }

        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            if (!isOpenningWalletFinished)
            {
                return;
            }

            if (wallet == null)
            {
                txbStatus.Text = StringTable.GetInstance().GetString("STR_NW_ERR_OPEN", iLang);
                txbStatus.Visibility = Visibility.Visible;

                btnNext.IsEnabled = true;
            }
            else
            {
                MainWalletWnd = new MainWalletWindow(wallet);
                MainWalletWnd.Show();

                isNext = true;

                Close();
            }
        }
    }

    public class PasswordBoxMonitor : DependencyObject
    {
        public static bool GetIsMonitoring(DependencyObject obj)
        {
            return (bool)obj.GetValue(IsMonitoringProperty);
        }

        public static void SetIsMonitoring(DependencyObject obj, bool value)
        {
            obj.SetValue(IsMonitoringProperty, value);
        }

        public static readonly DependencyProperty IsMonitoringProperty =
            DependencyProperty.RegisterAttached("IsMonitoring", typeof(bool), typeof(PasswordBoxMonitor), new UIPropertyMetadata(false, OnIsMonitoringChanged));



        public static int GetPasswordLength(DependencyObject obj)
        {
            return (int)obj.GetValue(PasswordLengthProperty);
        }

        public static void SetPasswordLength(DependencyObject obj, int value)
        {
            obj.SetValue(PasswordLengthProperty, value);
        }

        public static readonly DependencyProperty PasswordLengthProperty =
            DependencyProperty.RegisterAttached("PasswordLength", typeof(int), typeof(PasswordBoxMonitor), new UIPropertyMetadata(0));

        private static void OnIsMonitoringChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var pb = d as PasswordBox;
            if (pb == null)
            {
                return;
            }
            if ((bool)e.NewValue)
            {
                pb.PasswordChanged += PasswordChanged;
            }
            else
            {
                pb.PasswordChanged -= PasswordChanged;
            }
        }

        static void PasswordChanged(object sender, RoutedEventArgs e)
        {
            var pb = sender as PasswordBox;
            if (pb == null)
            {
                return;
            }
            SetPasswordLength(pb, pb.Password.Length);
        }
    }
}
