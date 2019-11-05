using System;
using System.Drawing;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace DT_GUI_Modules.Controls.QurasItems
{
    public partial class DashBoardItem : UserControl
    {
        private string Address;
        private string Amount_QRS;
        private string Amount_QRG;

        public DashBoardItem()
        {
            InitializeComponent();

            this.Address = "";
            this.Amount_QRS = "QRS 0";
            this.Amount_QRG = "QRG 0";

            InitInterface();
            Refresh();
        }

        private void InitInterface()
        {
            lbl_address.Parent = pictureBox1;
            lbl_address.BackColor = Color.Transparent;

            lbl_qrs_amount.Parent = pictureBox1;
            lbl_qrs_amount.BackColor = Color.Transparent;

            lbl_usd_amount.Parent = pictureBox1;
            lbl_usd_amount.BackColor = Color.Transparent;

        }

        public void ShowMask()
        {
            pictureBox1.BackColor = Color.FromArgb(50, 0, 0, 0);
        }

        public void HideMask()
        {
            pictureBox1.BackColor = Color.Gray;
        }

        public DashBoardItem(string addr = "", string qrsAmount = "0", string usdAmount = "0")
        {
            InitializeComponent();

            this.Address = addr;
            this.Amount_QRS = qrsAmount;
            this.Amount_QRG = usdAmount;

            InitInterface();

            Refresh();
        }

        void Refresh()
        {
            this.lbl_address.Text = this.Address;
            this.lbl_qrs_amount.Text = "QRS " + this.Amount_QRS;
            this.lbl_usd_amount.Text = "QRG " + this.Amount_QRG;
        }

        public string GetAddress()
        {
            return lbl_address.Text;
        }

        public void SetAddress(string addr)
        {
            this.Address = addr;
            Refresh();
        }

        public void SetQRS(string amount)
        {
            this.Amount_QRS = amount;
            Refresh();
        }

        public void SetQRG(string amount)
        {
            this.Amount_QRG = amount;
            Refresh();
        }

        private void btn_addr_copy_Click(object sender, EventArgs e)
        {
            try
            {
                Clipboard.SetText(lbl_address.Text);
            }
            catch (ExternalException) { }
        }
    }
}
