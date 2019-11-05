using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DT_GUI_Modules.Modules
{
    public partial class SendControl : UserControl
    {
        public event EventHandler SendButtonClick;
        public SendControl()
        {
            InitializeComponent();
        }

        public string GetFromAddress()
        {
            return cmb_from_address.Text;
        }

        public string GetToAddress()
        {
            return txb_to_address.Text;
        }

        public long GetAmount()
        {
            return long.Parse(txb_amount.Text);
        }

        private void btn_send_Click(object sender, EventArgs e)
        {
            SendButtonClick?.Invoke(sender, e);
        }
    }
}
