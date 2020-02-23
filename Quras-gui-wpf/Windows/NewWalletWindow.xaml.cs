using System;
using System.Windows;
using System.IO;

using Quras;
using Quras.Core;
using Quras.Network;
using Quras.Wallets;
using Quras.Implementations.Wallets.EntityFramework;
using QurasCore.Wallets.AnonymousKey.Key;
using QurasCore.Wallets.AnonymousKey.Note;

using Quras_gui_wpf.Global;
using Quras_gui_wpf.Utils;
using Quras_gui_wpf.Properties;
using System.Threading;
using System.Windows.Threading;
using System.Windows.Media;

namespace Quras_gui_wpf.Windows
{
    /// <summary>
    /// Interaction logic for NewWalletWindow.xaml
    /// </summary>
    enum PASSWORD_LEVEL
    {
        WEAK,
        MEDIUM,
        STRONG,
        VERY_STRONG
    };
    public partial class NewWalletWindow : Window
    {
        #region Members
        private MainWalletWindow MainWalletWnd;

        private UserWallet wallet = null;

        private LANG iLang => Constant.GetLang();
        private bool isNext = false;
        private bool isPrev = false;

        private DispatcherTimer engineTimer;

        private Thread CreatingWalletThread;
        private bool isCreatingWalletFinished = false;

        public class CreatingWallet
        {
            public NewWalletWindow parent;

            public KeyType keyType;
            public string path;
            public string password;
            public void processNewWallet()
            {
                try
                {
                    parent.wallet = UserWallet.Create(path, password, keyType);

                    Settings.Default.LastWalletPath = path;
                    Settings.Default.Save();
                }
                catch (Exception ex)
                {
                    LogManager.WriteExceptionLogs(ex);
                    parent.wallet = null;
                }
                parent.isCreatingWalletFinished = true;
            }
        }
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
            txbStatus.Text = StringTable.GetInstance().GetString("STR_NW_NOTE_PASSWORD", iLang);
            btnBack.Content = StringTable.GetInstance().GetString("STR_NW_BACK", iLang);
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

            txbStatus.Text = StringTable.GetInstance().GetString(checkField, iLang);
            txbStatus.Foreground = new SolidColorBrush(Color.FromRgb(255,69,0));
            txbStatus.Visibility = Visibility.Visible;

            if (checkField != "STR_NW_SUC_WALLET")
            {
                return;
            }

            txbStatus.Text = StringTable.GetInstance().GetString(checkField, iLang);

            CreatingWallet creatingWallet = new CreatingWallet();
            creatingWallet.parent = this;
            creatingWallet.path = txbWalletPath.Text;
            creatingWallet.password = txbPassword.Password;

            if (rdbAnonymous.IsChecked == true)
            {
                creatingWallet.keyType = KeyType.Anonymous;
            }
            else if (rdbTransparent.IsChecked == true)
            {
                creatingWallet.keyType = KeyType.Transparent;
            }
            else if (rdbStealth.IsChecked == true)
            {
                creatingWallet.keyType = KeyType.Stealth;
            }

            CreatingWalletThread = new Thread(creatingWallet.processNewWallet, 2 * 1024 * 1024);
            CreatingWalletThread.Start();

            engineTimer = new DispatcherTimer();
            engineTimer.Tick += this.dispatcherTimer_Tick;
            engineTimer.Interval = new TimeSpan(0, 0, 0, 0, 500);
            engineTimer.Start();

            btnNext.IsEnabled = false;
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            if (CreatingWalletThread != null)
            {
                CreatingWalletThread.Interrupt();
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

        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            if (!isCreatingWalletFinished)
            {
                return;
            }

            if (wallet == null)
            {
                txbStatus.Text = StringTable.GetInstance().GetString("STR_NW_ERR_UNKNOWN", iLang);
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

        private void TxbPassword_GotFocus(object sender, RoutedEventArgs e)
        {
            txbStatus.Visibility = Visibility.Visible;
        }

        private PASSWORD_LEVEL Password_Level(string password)
        {
            int nPoint = 0;
            bool bNumber = false, bLowerAlpha = false, bUpperAlpha = false, bSpecialCharacter = false;

            if (password.Length > 8)
                nPoint++;
            for (int i = 0; i < password.Length; i ++)
            {
                if (password[i] >= '0' && password[i] <= '9')
                    bNumber = true;
                else if (password[i] >= 'a' && password[i] <= 'z')
                    bLowerAlpha = true;
                else if (password[i] >= 'A' && password[i] <= 'Z')
                    bUpperAlpha = true;
                else
                    bSpecialCharacter = true;
            }

            if (bNumber)
                nPoint++;
            if (bLowerAlpha)
                nPoint++;
            if (bUpperAlpha)
                nPoint++;
            if (bSpecialCharacter)
                nPoint++;
            if (password.Length < 8 && nPoint < 2)
                return PASSWORD_LEVEL.WEAK;
            else if (nPoint < 3)
                return PASSWORD_LEVEL.MEDIUM;
            else if (nPoint < 5)
                return PASSWORD_LEVEL.STRONG;
            else
                return PASSWORD_LEVEL.VERY_STRONG;
        }

        private void TxbPassword_PasswordChanged(object sender, RoutedEventArgs e)
        {
            PASSWORD_LEVEL level = Password_Level(txbPassword.Password);
            if (level == PASSWORD_LEVEL.WEAK)
            {
                txbPwdLevel.Text = StringTable.GetInstance().GetString("STR_NW_PASSWORD_WEAK", iLang);
                txbPwdLevel.Foreground = new SolidColorBrush(Color.FromRgb(255, 125, 0));
                txbPwdLevel.Visibility = Visibility.Visible;
            }
            else if (level == PASSWORD_LEVEL.MEDIUM)
            {
                txbPwdLevel.Text = StringTable.GetInstance().GetString("STR_NW_PASSWORD_MEDIUM", iLang);
                txbPwdLevel.Foreground = new SolidColorBrush(Color.FromRgb(243, 243, 0));
                txbPwdLevel.Visibility = Visibility.Visible;
            }
            else if (level == PASSWORD_LEVEL.STRONG)
            {
                txbPwdLevel.Text = StringTable.GetInstance().GetString("STR_NW_PASSWORD_STRONG", iLang);
                txbPwdLevel.Foreground = new SolidColorBrush(Color.FromRgb(0, 243, 211));
                txbPwdLevel.Visibility = Visibility.Visible;
            }
            else if (level == PASSWORD_LEVEL.VERY_STRONG)
            {
                txbPwdLevel.Text = StringTable.GetInstance().GetString("STR_NW_PASSWORD_VSTRONG", iLang);
                txbPwdLevel.Foreground = new SolidColorBrush(Color.FromRgb(126, 252, 0));
                txbPwdLevel.Visibility = Visibility.Visible;
            }
        }
    }
}
