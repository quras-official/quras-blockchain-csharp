using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Pure.Implementations.Wallets.EntityFramework;

using Quras_gui_SP.Global;
using Quras_gui_SP.Dialogs;
using Quras_gui_SP.Properties;
using Quras_gui_SP.Controls;

namespace Quras_gui_SP.MainPans
{
    public partial class OverviewPan : UserControl
    {
        private string wallet_path_;
        private string wallet_pwd_;

        public List<AccountItem> addrItem;
        public event EventHandler WalletOpenedEvent;

        public OverviewPan()
        {
            InitializeComponent();
            SetWalletStatus(WalletStatus.Empty);
            InitInstance();
            InitInterface();
        }

        public OverviewPan(WalletStatus status = WalletStatus.Empty)
        {
            InitializeComponent();
            SetWalletStatus(status);
            InitInstance();
            InitInterface();
        }

        private void InitInterface()
        {
            vScrollBar1.Hide();
        }

        private void InitInstance()
        {
            wallet_path_ = "";
            wallet_pwd_ = "";

            addrItem = new List<AccountItem>();
        }

        private void vScrollBar1_ValueChanged(object sender, EventArgs e)
        {
            for (int i = 0; i < addrItem.Count; i ++)
            {
                addrItem[i].Top = 3 + 71 * i - vScrollBar1.Value;
            }
        }

        public void SetQrsTotalBalance(string balance)
        {
            lbl_qrs_balance.Text = balance;
        }

        public void SetQrgTotalBalance(string balance)
        {
            lbl_qrg_balance.Text = balance;
        }

        public void SetBlockchainHeight(string content)
        {
            lbl_block_height.Text = content;
        }

        public void SetPeersConnect(string content)
        {
            lbl_connect.Text = content;
        }

        public void SetWalletStatus(WalletStatus status)
        {
            switch(status)
            {
                case WalletStatus.Empty:
                    lbl_wallet_status.Text = "Please open your wallet.";
                    btn_export_wallet.Enabled = false;
                    btn_add_addr.Enabled = false;
                    btn_refresh.Enabled = false;
                    break;
                case WalletStatus.Opened:
                    lbl_wallet_status.Text = "Here is a summary of your wallet.";
                    btn_export_wallet.Enabled = true;
                    btn_add_addr.Enabled = true;
                    btn_refresh.Enabled = true;
                    break;
                default:
                    lbl_wallet_status.Text = "There is some errors. please ask on Quras dev team.";
                    btn_export_wallet.Enabled = false;
                    btn_add_addr.Enabled = false;
                    btn_refresh.Enabled = false;
                    break;
            }
        }

        private void btn_open_wallet_Click(object sender, EventArgs e)
        {
            using (OpenWalletDlg dialog = new OpenWalletDlg())
            {
                if (dialog.ShowDialog() != DialogResult.OK)
                {
                    wallet_path_ = "";
                    wallet_pwd_ = "";
                    return;
                }

                wallet_path_ = dialog.GetWalletPath();
                wallet_pwd_ = dialog.GetWalletPassword();

                Settings.Default.LastWalletPath = wallet_path_;
                Settings.Default.Save();

                WalletOpenedEvent?.Invoke(sender, e);

                SetWalletStatus(WalletStatus.Opened);
            }
        }

        public string GetWalletPath()
        {
            return wallet_path_;
        }

        public string GetWalletPassword()
        {
            return wallet_pwd_;
        }

        public void AddAddress(AddrStruct addrInfo)
        {
            AccountItem item = new AccountItem(addrInfo.Address, addrInfo.AmountQRS, addrInfo.AmountQRG);

            item.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(36)))), ((int)(((byte)(45)))), ((int)(((byte)(55)))));
            item.Location = new System.Drawing.Point(0, 3 + 71 * addrItem.Count);
            item.Name = "accountItem" + addrItem.Count();
            item.Size = new System.Drawing.Size(555, 71);
            item.TabIndex = 0;

            this.pan_addr_main.Controls.Add(item);

            addrItem.Add(item);

            if (addrItem.Count * 71 + 3 > pan_addr_main.Height)
            {
                vScrollBar1.Maximum = addrItem.Count * 71 + 3 - pan_addr_main.Height + 16;
                vScrollBar1.Show();
            }
        }

        public void ResetWallet()
        {
            for (int i = 0; i < addrItem.Count; i++)
            {
                pan_addr_main.Controls.Remove(addrItem[i]);
            }

            addrItem.Clear();
        }

        private void btn_new_wallet_Click(object sender, EventArgs e)
        {
            using (NewWalletDlg dialog = new NewWalletDlg())
            {
                if (dialog.ShowDialog() != DialogResult.OK)
                {
                    wallet_path_ = "";
                    wallet_pwd_ = "";
                    return;
                }

                wallet_path_ = dialog.GetWalletPath();
                wallet_pwd_ = dialog.GetWalletPassword();

                Settings.Default.LastWalletPath = wallet_path_;
                Settings.Default.Save();

                WalletOpenedEvent?.Invoke(sender, e);

                SetWalletStatus(WalletStatus.Opened);
            }
        }

        private void btn_export_wallet_Click(object sender, EventArgs e)
        {
            using (ExportWalletDlg dialog = new ExportWalletDlg())
            {
                if (dialog.ShowDialog() != DialogResult.OK)
                {
                    return;
                }
            }
        }

        private void btn_refresh_Click(object sender, EventArgs e)
        {
            using (YesNoDlg dialog = new YesNoDlg("Are you sure?", "Your wallet db will be initialized."))
            {
                if (dialog.ShowDialog() != DialogResult.OK)
                {
                    return;
                }
            }

            Constant.CurrentWallet.Rebuild();

            WalletOpenedEvent?.Invoke(sender, e);

            SetWalletStatus(WalletStatus.Opened);
        }
    }
}
