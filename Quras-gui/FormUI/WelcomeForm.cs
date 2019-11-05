using System;
using System.Windows.Forms;

using Quras_gui.Global;
using Quras_gui.Properties;

namespace Quras_gui.FormUI
{
    public partial class WelcomeForm : Form
    {
        public WelcomeForm()
        {
            InitializeComponent();
            InitInterface();
        }

        private void InitInterface()
        {
            int iLang = Constant.GetLang();
            lbl_description_bold.Text = StringTable.DATA[iLang, 0];
            lbl_small_info.Text = StringTable.DATA[iLang, 1];
            btn_new_wallet.Text = StringTable.DATA[iLang, 2];
            btn_restore.Text = StringTable.DATA[iLang, 3];
            this.Text = StringTable.DATA[iLang, 16];
        }

        private void btn_new_wallet_Click(object sender, EventArgs e)
        {
            using (NewWalletForm dialog = new NewWalletForm())
            {
                this.Hide();
                FormManager.GetInstance().Push(dialog);
                if (dialog.ShowDialog() != DialogResult.OK) return;
                //ChangeWallet(UserWallet.Create(dialog.WalletPath, dialog.Password));
                //Settings.Default.LastWalletPath = dialog.WalletPath;
                //Settings.Default.Save();
            }
        }

        private void btn_resotre_Click(object sender, EventArgs e)
        {
            using (RestoreWalletForm dialog = new RestoreWalletForm())
            {
                this.Hide();
                FormManager.GetInstance().Push(dialog);
                if (dialog.ShowDialog() != DialogResult.OK) return;
            }
        }

        private void WelcomeForm_Load(object sender, EventArgs e)
        {
            
        }
    }
}
