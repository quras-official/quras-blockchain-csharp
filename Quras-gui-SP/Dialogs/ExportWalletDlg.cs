using System;
using System.Windows.Forms;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Security.Cryptography;

using Pure;
using Pure.Wallets;

using Quras_gui_SP.Global;

namespace Quras_gui_SP.Dialogs
{
    public partial class ExportWalletDlg : Form
    {
        public ExportWalletDlg()
        {
            InitializeComponent();
            InitInstance();
            InitInterface();
        }

        private void InitInterface()
        {
            lbl_warning.Hide();
        }

        private void InitInstance()
        {
            // cmb_address initialize
            foreach (UInt160 scriptHash in Constant.CurrentWallet.GetAddresses().ToArray())
            {
                KeyPair key = (KeyPair)Constant.CurrentWallet.GetKeyByScriptHash(scriptHash);
                VerificationContract contract = Constant.CurrentWallet.GetContract(scriptHash);

                if (contract == null)
                {
                    //AddAddressToListView(scriptHash);
                }
                else
                {
                    AddContractToCombobox(contract);
                }

                cmb_address.SelectedIndex = 0;
            }

        }

        private void AddContractToCombobox(VerificationContract contract)
        {
            cmb_address.Items.Add(contract.Address);
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

        private void btn_close_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btn_addr_book_Click(object sender, EventArgs e)
        {
            SaveFileDialog dlg = new SaveFileDialog();

            dlg.DefaultExt = "json";
            dlg.Filter = "Quras Wallet Json Files (*.json)|*.json";
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

            if (txb_confirm_password.Text.Length == 0)
            {
                lbl_warning.Text = "Please Input the confirm password!";
                lbl_warning.Show();
                return false;
            }

            if (txb_wallet_path.Text.Length == 0)
            {
                lbl_warning.Text = "Please Input the wallet path!";
                lbl_warning.Show();
                return false;
            }

            if (txb_password.Text != txb_confirm_password.Text)
            {
                lbl_warning.Text = "Confirm password is not correct!";
                lbl_warning.Show();
                return false;
            }

            return true;
        }

        private void btn_confirm_Click(object sender, EventArgs e)
        {
            if (!CheckParameters())
                return;

            KeyPair key = null;
            VerificationContract contract;
            foreach (UInt160 scriptHash in Constant.CurrentWallet.GetAddresses().ToArray())
            {
                key = (KeyPair)Constant.CurrentWallet.GetKeyByScriptHash(scriptHash);
                contract = Constant.CurrentWallet.GetContract(scriptHash);

                if (contract.Address == cmb_address.SelectedItem)
                {
                    break;
                }
            }

            if (key != null)
                Wallet.ExportJsonPrivateKeyFile(txb_wallet_path.Text, key.PrivateKey, txb_password.Text, key.nVersion);

            this.DialogResult = DialogResult.OK;
        }
    }
}
