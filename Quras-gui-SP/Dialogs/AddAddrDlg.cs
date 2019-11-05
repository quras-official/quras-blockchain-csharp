using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace Quras_gui_SP.Dialogs
{
    public partial class AddAddrDlg : Form
    {
        public AddAddrDlg()
        {
            InitializeComponent();
            InitInterface();
        }

        public AddAddrDlg(string name, string address)
        {
            InitializeComponent();
            InitInterface();

            txb_contact_name.Text = name;
            txb_address.Text = address;
        }

        public void InitInterface()
        {
            lbl_warning.Hide();
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

        public string GetErrorMessage()
        {
            string ret = "";
            if (txb_contact_name.Text.Length == 0)
            {
                ret = "You didn't input the name.!\r\nPlease input the name.";
                return ret;
            }

            try
            {
                Pure.Wallets.Wallet.GetAddressVersion(txb_address.Text);
            }
            catch
            {
                ret = "The address format is not correct!";
                return ret;
            }

            return ret;
        }

        public string GetName()
        {
            return txb_contact_name.Text;
        }

        public string GetAddress()
        {
            return txb_address.Text;
        }

        private void btn_confirm_Click(object sender, EventArgs e)
        {
            if (GetErrorMessage().Length > 0)
            {
                lbl_warning.Text = GetErrorMessage();
                lbl_warning.Show();
                return;
            }

            this.DialogResult = DialogResult.OK;
        }
    }
}
