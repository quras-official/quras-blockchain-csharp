using System;
using System.Windows;
using System.IO;

using Pure;
using Pure.Core;
using Pure.Network;
using Pure.Wallets;
using Pure.Implementations.Wallets.EntityFramework;
using PureCore.Wallets.AnonymousKey.Key;
using PureCore.Wallets.AnonymousKey.Note;

using Quras_gui_wpf.Global;
using Quras_gui_wpf.Utils;
using Quras_gui_wpf.Properties;

namespace Quras_gui_wpf.Windows
{
    /// <summary>
    /// Interaction logic for NewWalletWindow.xaml
    /// </summary>
    public partial class NewWalletWindow : Window
    {
        #region Members
        private MainWalletWindow MainWalletWnd;
        private LANG iLang => Constant.GetLang();
        private bool isNext = false;
        private bool isPrev = false;
        #endregion

        public NewWalletWindow()
        {
            InitializeComponent();
            RefreshLanguage();
            InitInstance();
        }

        public void RefreshLanguage()
        {
            TxbCommentHeader.Text = StringTable.GetInstance().GetString("STR_NW_COMMENT_HEADER", iLang);
            txbWalletPath.Tag = StringTable.GetInstance().GetString("STR_NW_WALLET_PATH", iLang);
            rdbAnonymous.Content = StringTable.GetInstance().GetString("STR_NW_ANONYMOUS", iLang);
            rdbTransparent.Content = StringTable.GetInstance().GetString("STR_NW_TRANSPARENT", iLang);
            rdbStealth.Content = StringTable.GetInstance().GetString("STR_NW_STEALTH", iLang);
            btnNext.Content = StringTable.GetInstance().GetString("STR_NW_NEXT", iLang);
        }

        public void InitInstance()
        {
            rdbTransparent.IsChecked = true;
        }
        private void btnBrowser_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
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

        private string CheckParameter()
        {
            if (txbWalletPath.Text.Length == 0)
            {
                return "STR_NW_ERR_WALLET_PATH";
            }

            if (!Directory.Exists(System.IO.Path.GetDirectoryName(txbWalletPath.Text)))
            {
                return "STR_NW_ERR_PATH";
            }

            if (txbPassword.Password.Length == 0)
            {
                return "STR_NW_ERR_PASSWORD";
            }

            if (txbPassword.Password != txbConfirmPassword.Password)
            {
                return "STR_NW_ERR_CONFIRM_PASSWORD";
            }

            return "STR_NW_SUC_WALLET";
        }

        private void btnNext_Click(object sender, RoutedEventArgs e)
        {
            string checkField = CheckParameter();

            if (checkField != "STR_NW_SUC_WALLET")
            {
                txbStatus.Text = StringTable.GetInstance().GetString(checkField, iLang);
                txbStatus.Visibility = Visibility.Visible;
                return;
            }

            txbStatus.Text = StringTable.GetInstance().GetString(checkField, iLang);

            UserWallet wallet;

            try
            {
                if (rdbAnonymous.IsChecked == true)
                {
                    wallet = UserWallet.Create(txbWalletPath.Text, txbPassword.Password, KeyType.Anonymous);
                }
                else if (rdbTransparent.IsChecked == true)
                {
                    wallet = UserWallet.Create(txbWalletPath.Text, txbPassword.Password, KeyType.Transparent);
                }
                else if (rdbStealth.IsChecked == true)
                {
                    wallet = UserWallet.Create(txbWalletPath.Text, txbPassword.Password, KeyType.Stealth);
                }
                else
                {
                    txbStatus.Text = StringTable.GetInstance().GetString("STR_NW_ERR_UNKNOWN", iLang);
                    txbStatus.Visibility = Visibility.Visible;
                    return;
                }

                Settings.Default.LastWalletPath = txbWalletPath.Text;
                Settings.Default.Save();
            }
            catch (Exception ex)
            {
                txbStatus.Text = StringTable.GetInstance().GetString("STR_NW_ERR_UNKNOWN", iLang);
                txbStatus.Visibility = Visibility.Visible;
                LogManager.WriteExceptionLogs(ex);
                return;
            }

            MainWalletWnd = new MainWalletWindow(wallet);
            MainWalletWnd.Show();

            isNext = true;

            this.Close();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            if (isNext == false && isPrev == false)
            {
                if (Constant.NotifyMessageMgr != null)
                {
                    Constant.NotifyMessageMgr.Stop();
                }
                if (Constant.LocalNode != null)
                {
                    Constant.LocalNode.Dispose();
                }
                
                Blockchain.Default.Dispose();

                using (FileStream fs = new FileStream(Constant.PEER_STATE_PATH, FileMode.Create, FileAccess.Write, FileShare.None))
                {
                    LocalNode.SaveState(fs);
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
    }
}
