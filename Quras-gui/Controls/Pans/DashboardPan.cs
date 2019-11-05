using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Quras_gui.Controls.Pans
{
    public partial class DashboardPan : UserControl
    {
        public event EventHandler QRCodeBtnClicked;

        public DashboardPan()
        {
            InitializeComponent();
        }
        
        public void SetQRSTotalBalance(string balance)
        {
            txb_qrs_balance.Text = balance;
        }

        public void SetQRGTotalBalance(string balance)
        {
            txb_qrg_balance.Text = balance;
        }

        private void btn_qrcode_Click(object sender, EventArgs e)
        {
            QRCodeBtnClicked?.Invoke(sender, e);
        }
    }
}
