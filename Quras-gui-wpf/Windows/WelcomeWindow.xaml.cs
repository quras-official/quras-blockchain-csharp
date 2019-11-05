using System.Windows;
using System.IO;

using Quras_gui_wpf.Global;
using Quras_gui_wpf.Utils;

using Pure.Core;
using Pure.Network;

namespace Quras_gui_wpf.Windows
{
    /// <summary>
    /// Interaction logic for WelcomeWindow.xaml
    /// </summary>
    public partial class WelcomeWindow : Window
    {
        NewWalletWindow NewWalletWnd;
        RestoreWalletWindow RestoreWalletWnd;

        LANG iLang => Constant.GetLang();

        private bool isNext = false;

        public WelcomeWindow()
        {
            InitializeComponent();

            RefreshLanguage();
        }

        public void RefreshLanguage()
        {
            TxbCommentHeader.Text = StringTable.GetInstance().GetString("STR_WP_COMMENT_HEADER", iLang);
            TxbCommentBody.Text = StringTable.GetInstance().GetString("STR_WP_COMMENT_BODY", iLang);

            btnNewWallet.Content = StringTable.GetInstance().GetString("STR_WP_NEW_WALLET", iLang);
            btnRestore.Content = StringTable.GetInstance().GetString("STR_WP_RESTORE", iLang);
        }

        private void NewWalletBtn_Click(object sender, RoutedEventArgs e)
        {
            NewWalletWnd = new NewWalletWindow();
            NewWalletWnd.Show();
            isNext = true;
            this.Close();
        }

        private void btnRestore_Click(object sender, RoutedEventArgs e)
        {
            RestoreWalletWnd = new RestoreWalletWindow();
            RestoreWalletWnd.Show();
            isNext = true;
            this.Close();
        }

        private void Window_Closed(object sender, System.EventArgs e)
        {
            if (isNext == false)
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
                
                Blockchain.Default.Dispose();
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            
        }
    }
}
