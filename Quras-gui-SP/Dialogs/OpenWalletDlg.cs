using System;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Security.Cryptography;

using Pure.Implementations.Wallets.EntityFramework;

using Quras_gui_SP.Properties;

namespace Quras_gui_SP.Dialogs
{
    public partial class OpenWalletDlg : Form
    {
        public OpenWalletDlg()
        {
            InitializeComponent();
            InitInterface();
        }

        private void InitInterface()
        {
            lbl_warning.Hide();
            txb_wallet_path.Text = Settings.Default.LastWalletPath;
        }

        private void btn_close_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        // Form Move
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [DllImportAttribute("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd,
                         int Msg, int wParam, int lParam);
        [DllImportAttribute("user32.dll")]
        public static extern bool ReleaseCapture();

        private void Form_MouseDown(object sender,
        System.Windows.Forms.MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }

        private void btn_addr_book_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();

            dlg.DefaultExt = "db3";
            dlg.Filter = "Quras Wallet DB Files (*.db3)|*.db3";
            dlg.AddExtension = true;

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                txb_wallet_path.Text = dlg.FileName;
            }
        }

        private bool CheckParameters()
        {
            if (txb_password.Text.Length == 0)
            {
                lbl_warning.Text = "Please Input the password!";
                lbl_warning.Show();
                return false;
            }

            if (txb_wallet_path.Text.Length == 0)
            {
                lbl_warning.Text = "Please Input the wallet path!";
                lbl_warning.Show();
                return false;
            }

            return true;
        }

        private void btn_confirm_Click(object sender, EventArgs e)
        {
            if (!CheckParameters())
                return;

            UserWallet wallet;
            try
            {
                wallet = UserWallet.CheckPassword(txb_wallet_path.Text, txb_password.Text);
            }
            catch (CryptographicException ex)
            {
                lbl_warning.Text = "Password is not correct";
                lbl_warning.Show();
                return;
            }
            catch (FormatException ex)
            {
                lbl_warning.Text = ex.Message;
                lbl_warning.Show();
                return;
            }

            this.DialogResult = DialogResult.OK;
            return;
        }

        public string GetWalletPath()
        {
            return txb_wallet_path.Text;
        }

        public string GetWalletPassword()
        {
            return txb_password.Text;
        }
    }
}
