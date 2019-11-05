using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Pure;
using Pure.Core;

using Quras_gui_SP.Dialogs;
using Quras_gui_SP.Global;

namespace Quras_gui_SP.MainPans
{
    public partial class SendcoinsPan : UserControl
    {
        private Fixed8 qrs_balance;
        private Fixed8 qrg_balance;

        public event EventHandler AddAddressBookEvent;
        public event EventHandler SendCoinErrorEvent;
        public event EventHandler SendCoinSuccessEvent;
        public SendcoinsPan()
        {
            InitializeComponent();
        }

        public void RefreshInterface(Fixed8 qrs_val, Fixed8 qrg_val)
        {
            // Asset Init
            foreach (UInt256 asset_id in Constant.CurrentWallet.FindUnspentCoins().Select(p => p.Output.AssetId).Distinct())
            {
                AssetState state = Blockchain.Default.GetAssetState(asset_id);
                AssetDescriptor item = new AssetDescriptor(state);

                bool IsAdd = true;
                for (int i = 0; i < cmb_assets.Items.Count; i ++)
                {
                    if (((AssetDescriptor)cmb_assets.Items[i]).AssetId == item.AssetId)
                    {
                        IsAdd = false;
                        break;
                    }
                }
                if (IsAdd)
                {
                    cmb_assets.Items.Add(item);
                }
            }

            qrs_balance = qrs_val;
            qrg_balance = qrg_val;

            if (cmb_assets.SelectedItem == null)
            {
                lbl_balance.Hide();
                lbl_cmt_balance.Hide();
            }
            else
            {
                if (((AssetDescriptor)cmb_assets.SelectedItem).AssetName == "QRS")
                {
                    lbl_balance.Text = qrs_val.ToString() + " QRS";
                }
                else if (((AssetDescriptor)cmb_assets.SelectedItem).AssetName == "QRG")
                {
                    lbl_balance.Text = qrs_val.ToString() + " QRG";
                }
            }
            
            /*
            foreach (string s in Settings.Default.NEP5Watched)
            {
                UInt160 asset_id = UInt160.Parse(s);
                try
                {
                    sendPanControl1.AddAssetCombobox(new AssetDescriptor(asset_id));
                }
                catch (ArgumentException)
                {
                    continue;
                }
            }
            */
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void cmb_assets_SelectedIndexChanged(object sender, EventArgs e)
        {
            lbl_cmt_balance.Show();
            lbl_balance.Show();

            if (((AssetDescriptor)cmb_assets.SelectedItem).AssetName == "QRS")
            {
                lbl_balance.Text = qrs_balance.ToString() + " QRS";
            }
            else if (((AssetDescriptor)cmb_assets.SelectedItem).AssetName == "QRG")
            {
                lbl_balance.Text = qrg_balance.ToString() + " QRG";
            }
        }

        public string GetErrorMessage()
        {
            string ret = "";
            if (txb_from_address.Text.Length == 0)
            {
                ret = "You didn't input the from wallet path!\r\nPlease input the from wallet path.";
                return ret;
            }

            try
            {
                Pure.Wallets.Wallet.GetAddressVersion(txb_from_address.Text);
            }
            catch
            {
                ret = "The from wallet format is not correct!";
                return ret;
            }

            if (txb_recv_addr.Text.Length == 0)
            {
                ret = "You didn't input the wallet path!\r\nPlease input the wallet path.";
                return ret;
            }

            try
            {
                Pure.Wallets.Wallet.GetAddressVersion(txb_recv_addr.Text);
            }
            catch
            {
                ret = "The wallet format is not correct!";
                return ret;
            }

            if (cmb_assets.SelectedItem == null)
            {
                ret = "You didn't select the assets type!\r\nPlease select the asset type.";
                return ret;
            }

            if (txb_amount.Text.Length == 0)
            {
                ret = "You didn't input the amount!\r\nPlease input the amount.";
                return ret;
            }

            if (!Fixed8.TryParse(txb_amount.Text, out Fixed8 amount))
            {
                ret = "Please input the correct amount!\r\nAmount type is not correct.";
                return ret;
            }
            if (amount == Fixed8.Zero)
            {
                ret = "Send amount must be bigger than zero!";
                return ret;
            }

            Fixed8 max_balance = Fixed8.Zero;
            if (((AssetDescriptor)cmb_assets.SelectedItem).AssetName == "QRS")
            {
                max_balance = qrs_balance;
            }
            else if (((AssetDescriptor)cmb_assets.SelectedItem).AssetName == "QRG")
            {
                max_balance = qrg_balance;
            }

            if (amount > max_balance)
            {
                ret = "The amount is unsufficient!";
                return ret;
            }

            return ret;
        }

        private void btn_send_coins_Click(object sender, EventArgs e)
        {
            if (GetErrorMessage().Length > 0)
            {
                SendCoinErrorEvent?.Invoke(sender, e);
            }
            else
            {
                SendCoinSuccessEvent?.Invoke(sender, e);
            }
        }

        public string GetFromAddress()
        {
            return txb_from_address.Text;
        }

        public string GetRecieveAddress()
        {
            return txb_recv_addr.Text;
        }

        public string GetAmount()
        {
            return txb_amount.Text;
        }

        public object GetAsset()
        {
            return cmb_assets.SelectedItem;
        }

        public void ResetSendCoin()
        {
            cmb_assets.Items.Clear();
            txb_amount.Text = "";
            txb_recv_addr.Text = "";
        }

        private void btn_addr_book_Click(object sender, EventArgs e)
        {

        }

        private void btn_add_addr_Click(object sender, EventArgs e)
        {
            using (AddAddrDlg dialog = new AddAddrDlg("", txb_recv_addr.Text))
            {
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    string[] param = new string[2];

                    param[0] = dialog.GetName();
                    param[1] = dialog.GetAddress();

                    AddAddressBookEvent?.Invoke(param, e);
                    return;
                }
            }
        }
    }
}
