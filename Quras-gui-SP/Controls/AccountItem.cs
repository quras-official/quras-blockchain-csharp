using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Quras_gui_SP.Controls
{
    public partial class AccountItem : UserControl
    {
        public AccountItem(string addr = "", string qrs_val = "", string qrg_val = "")
        {
            InitializeComponent();

            InitInterface(addr, qrs_val, qrg_val);
        }

        private void InitInterface(string addr, string qrs_val, string qrg_val)
        {
            txb_addr.Text = addr;
            txb_qrs_val.Text = qrs_val;
            txb_qrg_val.Text = qrg_val;
        }

        public string GetAddress()
        {
            return txb_addr.Text;
        }

        public void SetQRS(string content)
        {
            txb_qrs_val.Text = content;
        }

        public void SetQRG(string content)
        {
            txb_qrg_val.Text = content;
        }
    }
}
