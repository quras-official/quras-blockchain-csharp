using System;
using System.Windows.Forms;
using System.Security.Cryptography;

using Pure.Implementations.Wallets.EntityFramework;
using Quras_gui.Properties;
using Quras_gui.Global;

namespace Quras_gui.FormUI
{
    public partial class RestoreWalletForm : Form
    {
        public MainWalletForm mainWalletForm;

        private int iLang => Constant.GetLang();
        public RestoreWalletForm()
        {
            InitializeComponent();
            InitInterface();
        }

        private void InitInterface()
        {
            lbl_warning.Hide();
            passwordItem1.SetWalletPath(Settings.Default.LastWalletPath);
            lbl_comment.Text = StringTable.DATA[iLang, 18];
            btn_continue.Text = StringTable.DATA[iLang, 19];
            this.Text = StringTable.DATA[iLang, 20];
        }

        private void btn_continue_Click(object sender, EventArgs e)
        {
            if (!CheckStatus())
                return;

            UserWallet wallet;
            try
            {
                wallet = UserWallet.Open(passwordItem1.GetWalletPath(), passwordItem1.GetPassword());
            }
            catch (CryptographicException ex)
            {
                lbl_warning.Text = StringTable.DATA[iLang, 21];
                lbl_warning.Show();
                return;
            }
            catch (FormatException ex)
            {
                lbl_warning.Text = ex.Message;
                lbl_warning.Show();
                return;
            }

            Settings.Default.LastWalletPath = passwordItem1.GetWalletPath();
            Settings.Default.Save();

            this.Hide();
            mainWalletForm = new MainWalletForm(wallet);
            if (mainWalletForm.ShowDialog() == DialogResult.OK)
            {

            }
        }
        
        private bool CheckStatus()
        {
            if (passwordItem1.GetWalletPath().Length == 0)
            {
                lbl_warning.Text = StringTable.DATA[iLang, 22];
                lbl_warning.Show();
                return false;
            }

            if (passwordItem1.GetPassword().Length == 0)
            {
                lbl_warning.Text = StringTable.DATA[iLang, 23];
                lbl_warning.Show();
                return false;
            }

            return true;
        }

        private void btn_browser_Click(object sender, EventArgs e)
        {
            
        }

        private void RestoreWalletForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }
    }
}
