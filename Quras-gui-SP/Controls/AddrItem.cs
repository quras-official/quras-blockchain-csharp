using System;
using System.Windows.Forms;
using Quras_gui_SP.Dialogs;

namespace Quras_gui_SP.Controls
{
    public partial class AddrItem : UserControl
    {
        private string name_;
        private string address_;
        private byte version_;

        public event EventHandler AddrDeleteEvent;
        public event EventHandler AddrEditEvent;

        public string Name
        {
            get
            {
                return name_;
            }

            set
            {
                txb_name.Text = Name;
            }
        }

        public string Address
        {
            get
            {
                return address_;
            }

            set
            {
                txb_address.Text = Address;
            }
        }

        public AddrItem()
        {
            InitializeComponent();
        }

        public AddrItem(string contact_name, string address)
        {
            InitializeComponent();

            name_ = contact_name;
            address_ = address;
            InitInterface();
        }

        public void RefreshItem(string contact_name, string address)
        {
            name_ = contact_name;
            address_ = address;
            InitInterface();
        }

        public void InitInterface()
        {
            txb_address.Text = address_;
            txb_name.Text = name_;

            try
            {
                version_ = Pure.Wallets.Wallet.GetAddressVersion(txb_address.Text);
            }
            catch
            {
                version_ = 0;
            }

            if (version_ == Pure.Wallets.Wallet.AddressVersion)
            {
                this.pan_color.BackColor = System.Drawing.Color.Green;
            }
            else if (version_ == Pure.Wallets.Wallet.AnonymouseAddressVersion)
            {
                this.pan_color.BackColor = System.Drawing.Color.DarkRed;
            }
            else
            {
                this.pan_color.BackColor = System.Drawing.Color.Gray;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string[] param = new string[2];
            param[0] = name_;
            param[1] = address_;

            AddrDeleteEvent?.Invoke(param, e);
        }

        private void btn_edit_Click(object sender, EventArgs e)
        {
            using (AddAddrDlg dialog = new AddAddrDlg(name_, address_))
            {
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    string[] param = new string[3];

                    param[0] = address_;
                    param[1] = dialog.GetName();
                    param[2] = dialog.GetAddress();

                    AddrEditEvent?.Invoke(param, e);
                    return;
                }
            }
        }
    }
}
