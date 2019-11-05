using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Quras_gui.Controls.Items
{
    public partial class PasswordItem : UserControl
    {
        private bool bPassword;
        public PasswordItem()
        {
            InitializeComponent();
            bPassword = true;
        }

        private void bunifuThinButton21_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();

            dlg.DefaultExt = "db3";
            dlg.Filter = "Data Files (*.db3)|*.db3";
            dlg.AddExtension = true;

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                txb_wallet_path.Text = dlg.FileName;
            }
        }

        public void SetWalletPath(string path)
        {
            txb_wallet_path.Text = path;
        }

        public string GetWalletPath()
        {
            return txb_wallet_path.Text;
        }

        public string GetPassword()
        {
            return txb_password.Text;
        }

        private void btn_show_password_Click(object sender, EventArgs e)
        {
            bPassword = !bPassword;
            
            if (bPassword)
            {
                this.btn_show_password.BackgroundImage = global::Quras_gui.Properties.Resources.show_password;
                this.txb_password.PasswordChar = '*';
            }
            else
            {
                this.btn_show_password.BackgroundImage = global::Quras_gui.Properties.Resources.hidden_password;
                this.txb_password.PasswordChar = '\0';
            }
            this.txb_password.Focus();
        }
    }
}
