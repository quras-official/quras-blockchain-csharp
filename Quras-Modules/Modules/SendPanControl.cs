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
    public partial class SendPanControl : UserControl
    {
        public event EventHandler SendButtonClick;
        public event EventHandler AssetTypeChangedEvent;
        public SendPanControl()
        {
            InitializeComponent();
        }
        public string GetAssetType()
        {
            return cmb_assets.Text;
        }

        public string GetToAddress()
        {
            return txb_to_address.Text;
        }

        public string GetAmount()
        {
            return txb_amount.Text;
        }

        private void btn_send_Click(object sender, EventArgs e)
        {
            SendButtonClick?.Invoke(sender, e);
        }

        public void AddAssetCombobox(object item)
        {
            cmb_assets.Items.Add(item);
        }

        public object GetSelectedAssetObj()
        {
            return cmb_assets.SelectedItem;
        }

        public void SetTotalBalance(string balace)
        {
            txb_balance.Text = balace;
        }

        private void cmb_assets_SelectedIndexChanged(object sender, EventArgs e)
        {
            AssetTypeChangedEvent?.Invoke(sender, e);
        }
    }
}
