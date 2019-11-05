using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Threading;

using Pure;
using Pure.Core;
using Pure.Wallets;

using Quras_gui_wpf.Dialogs;
using Quras_gui_wpf.Utils;
using Quras_gui_wpf.Global;
using Quras_gui_wpf.Dialogs.NotifyMessage;

namespace Quras_gui_wpf.Pages
{
    public class AssetTypeItem
    {
        public UInt256 AssetID;
        public string Name;
        public Fixed8 Value;

        public AssetTypeItem(UInt256 assetID, string name, Fixed8 value)
        {
            this.AssetID = assetID;
            this.Name = name;
            this.Value = value;
        }
    }
    /// <summary>
    /// Interaction logic for SendPages.xaml
    /// </summary>
    public partial class SendPages : UserControl
    {
        private LANG iLang => Constant.GetLang();
        private string fromAddress;
        public event EventHandler<TaskMessage> TaskChangedEventHandler;
        private bool isExpandedPan = true;
        private bool isSendCoin = true;

        public SendPages()
        {
            InitializeComponent();

            InitInstance();
            ShowExpandedPan();
            RefreshLanguage();
        }

        private void InitInstance()
        {

        }

        public void Reset()
        {
            cmbAssetType.Items.Clear();
        }

        public void AddAsset(UInt256 assetId, string assetName, Fixed8 value)
        {
            ComboBoxItem item = new ComboBoxItem();

            AssetTypeItem tagItem = new AssetTypeItem(assetId, assetName, value);

            item.Tag = tagItem;
            item.Content = assetName;

            cmbAssetType.Items.Add(item);

            if (cmbAssetType.SelectedItem == null)
            {
                cmbAssetType.SelectedIndex = 0;
            }
        }

        public void RefreshAsset(UInt256 assetId, Fixed8 value)
        {
            foreach(ComboBoxItem item in cmbAssetType.Items)
            {
                if (((AssetTypeItem)item.Tag).AssetID == assetId)
                {
                    ((AssetTypeItem)item.Tag).Value = value;
                    break;
                }
            }

            if (cmbAssetType.SelectedItem == null)
            {
                TxbSpendable.Text = String.Format(StringTable.GetInstance().GetString("STR_SP_SPENDABLE", iLang), 0, "");
                return; 
            }

            AssetTypeItem tag = ((ComboBoxItem)cmbAssetType.SelectedItem).Tag as AssetTypeItem;

            if (tag != null)
            {
                TxbSpendable.Text = String.Format(StringTable.GetInstance().GetString("STR_SP_SPENDABLE", iLang), tag.Value.ToString(), tag.Name);
            }
        }

        public void RefreshLanguage()
        {
            txbReceiveAddress.Tag = StringTable.GetInstance().GetString("STR_SP_RECEIVE_ADDRESS", iLang);
            txbAmount.Tag = StringTable.GetInstance().GetString("STR_SP_AMOUNT", iLang);
            btnSend.Content = StringTable.GetInstance().GetString("STR_SP_SEND", iLang);
        }

        private string CheckFieldFormat()
        {
            try
            {
                Pure.Wallets.Wallet.GetAddressVersion(txbReceiveAddress.Text);
            }
            catch
            {
                return "STR_SP_ERR_INCORRECT_RECEIVE_ADDRESS";
            }

            // Get the amount being able to send.

            UInt256 assetId = ((AssetTypeItem)((ComboBoxItem)cmbAssetType.SelectedItem).Tag).AssetID;

            AssetState assetState = Blockchain.Default.GetAssetState(assetId);
            if (assetState.AssetType == AssetType.TransparentToken)
            {
                if (Wallet.GetAddressVersion(fromAddress) == Wallet.AnonymouseAddressVersion ||
                Wallet.GetAddressVersion(txbReceiveAddress.Text) == Wallet.AnonymouseAddressVersion)
                {
                    return "STR_SP_ERR_TRANSPARENT_TOKEN";
                }
            }

            if (assetState.AssetType == AssetType.AnonymousToken)
            {
                if (Wallet.GetAddressVersion(fromAddress) == Wallet.StealthAddressVersion ||
                Wallet.GetAddressVersion(txbReceiveAddress.Text) == Wallet.StealthAddressVersion)
                {
                    return "STR_SP_ERR_ANONYMOUSE_TOKEN";
                }
            }

            Fixed8 balance = Fixed8.Zero;
            foreach (UInt160 scriptHash in Constant.CurrentWallet.GetAddresses().ToArray())
            {
                VerificationContract contract = Constant.CurrentWallet.GetContract(scriptHash);
                balance += ((AssetTypeItem)((ComboBoxItem)cmbAssetType.SelectedItem).Tag).Value;
                fromAddress = contract.Address;
            }

            if (fromAddress == txbReceiveAddress.Text)
            {
                return "STR_SP_ERR_SELF_TRANSFER";
            }
            
            try
            {
                if (double.Parse(balance.ToString()) < double.Parse(txbAmount.Text))
                {
                    return "STR_SP_ERR_INCORRECT_AMOUNT";
                }
            }
            catch (Exception)
            {
                return "STR_SP_ERR_INCORRECT_AMOUNT";
            }

            double amount = 0;

            try
            {
                if (cmbAssetType.Text == "XQG")
                {
                    amount = double.Parse(txbAmount.Text);
                }
                else if (cmbAssetType.Text == "XQC")
                {
                    amount = double.Parse(txbAmount.Text);
                }
                else
                {
                    amount = double.Parse(txbAmount.Text);
                }
            }
            catch
            {
                return "STR_SP_ERR_AMOUNT_FORMAT";
            }

            if (amount <= 0)
            {
                return "STR_SP_ERR_INPUT_AMOUNT";
            }

            if (Wallet.GetAddressVersion(fromAddress) == Wallet.AnonymouseAddressVersion ||
                Wallet.GetAddressVersion(txbReceiveAddress.Text) == Wallet.AnonymouseAddressVersion)
            {
                if (Constant.bSnarksParamLoaded == false)
                {
                    return "STR_SP_ERR_NOT_LOADED_ZK_SNARKS_KEY";
                }
            }

            if (Wallet.GetAddressVersion(fromAddress) == Wallet.AnonymouseAddressVersion && Wallet.GetAddressVersion(txbReceiveAddress.Text) == Wallet.StealthAddressVersion ||
                Wallet.GetAddressVersion(fromAddress) == Wallet.StealthAddressVersion && Wallet.GetAddressVersion(txbReceiveAddress.Text) == Wallet.AnonymouseAddressVersion)
            {
                return "STR_ERR_ANONYMOUSE_STEALTH";
            }

            return "STR_SP_SUCCESS";
        }

        private void TaskChangedEvent(object sender, TaskMessage task)
        {
            TaskChangedEventHandler?.Invoke(sender, task);
        }

        private void btnSend_Click(object sender, RoutedEventArgs e)
        {
            #region Check Fields
            string strCheckRet = CheckFieldFormat();

            if (strCheckRet != "STR_SP_SUCCESS")
            {
                StaticUtils.ShowMessageBox(StaticUtils.ErrorBrush, StringTable.GetInstance().GetString(strCheckRet, iLang));
                return;
            }

            if (!isSendCoin)
            {
                StaticUtils.ShowMessageBox(StaticUtils.ErrorBrush, StringTable.GetInstance().GetString("STR_SEND_COIN_ERROR_WAITING", iLang));
                return;
            }

            bool ret = false;
            using (Quras_gui_wpf.Dialogs.AlertDialog dlg = new Quras_gui_wpf.Dialogs.AlertDialog(Application.Current.MainWindow, StringTable.GetInstance().GetString("STR_ALERT_DLG_TITLE", iLang), StringTable.GetInstance().GetString("STR_ALERT_SEND_TX", iLang)))
            {
                ret = (bool)dlg.ShowDialog();
            }

            if (ret == false) return;
            #endregion
            
            #region Sending Coin
            SendCoinThread scthread = new SendCoinThread();
            scthread.Amount = txbAmount.Text;
            scthread.Fee = txbFeeAmount.Text;
            scthread.FromAddr = fromAddress;
            scthread.ToAddr = txbReceiveAddress.Text;
            scthread.TaskChangedEvent += TaskChangedEvent;

            AssetState state = Blockchain.Default.GetAssetState(AssetsManager.GetInstance().GetAssetID(cmbAssetType.Text));
            Global.AssetDescriptor asset = new Global.AssetDescriptor(state);

            scthread.Asset = asset;
            
            ThreadStart starter = new ThreadStart(scthread.DoWork);
            starter += () => {
                // Do what you want in the callback
                this.Dispatcher.Invoke(new Action(() =>
                {
                    // Show the status
                    StaticUtils.ShowMessageBox(StaticUtils.GreenBrush, StringTable.GetInstance().GetString("STR_SP_SENDING_SUCCESSFULLY", iLang));

                    SendTaskMessage taskMessage = new SendTaskMessage(fromAddress, txbReceiveAddress.Text, txbAmount.Text, cmbAssetType.Text, DateTime.Now, StringTable.GetInstance().GetString("STR_TASK_MESSAGE_SEND_TX_FINISHED", iLang));
                    Constant.TaskMessages.Add(taskMessage);
                    TaskChangedEventHandler?.Invoke(sender, taskMessage);
                    isSendCoin = true;
                }));
            };

            this.Dispatcher.Invoke(new Action(() =>
            {
                // show status
                StaticUtils.ShowMessageBox(StaticUtils.BlueBrush, StringTable.GetInstance().GetString("STR_SP_SENDING_TX", iLang));

                SendTaskMessage taskMessage = new SendTaskMessage(fromAddress, txbReceiveAddress.Text, txbAmount.Text, cmbAssetType.Text, DateTime.Now, StringTable.GetInstance().GetString("STR_TASK_MESSAGE_SEND_TX_START", iLang));
                Constant.TaskMessages.Add(taskMessage);
                TaskChangedEventHandler?.Invoke(sender, taskMessage);
                isSendCoin = false;
            }));
            
            Thread thread = new Thread(() => SafeExecute(() => starter())) { IsBackground = true };

            try
            {
                thread.Start();
            }
            catch (Exception)
            {
                this.Dispatcher.Invoke(new Action(() =>
                {
                    // show status
                    StaticUtils.ShowMessageBox(StaticUtils.ErrorBrush, StringTable.GetInstance().GetString("STR_SP_SEDDING_FAILED", iLang));

                    SendTaskMessage taskMessage = new SendTaskMessage(fromAddress, txbReceiveAddress.Text, txbAmount.Text, cmbAssetType.Text, DateTime.Now, StringTable.GetInstance().GetString("STR_TASK_MESSAGE_SEND_TX_FAILED", iLang), TaskColor.Red);
                    Constant.TaskMessages.Add(taskMessage);
                    TaskChangedEventHandler?.Invoke(sender, taskMessage);
                    isSendCoin = true;
                }));
            }
            #endregion

            #region Initialize fields
            txbReceiveAddress.Text = "";
            txbAmount.Text = "";
            #endregion
        }

        private void SafeExecute(Action test)
        {
            try
            {
                test.Invoke();
            }
            catch (Exception)
            {
                this.Dispatcher.Invoke(new Action(() =>
                {
                    // show status
                    StaticUtils.ShowMessageBox(StaticUtils.ErrorBrush, StringTable.GetInstance().GetString("STR_SP_SEDDING_FAILED", iLang));

                    SendTaskMessage taskMessage = new SendTaskMessage(fromAddress, txbReceiveAddress.Text, txbAmount.Text, cmbAssetType.Text, DateTime.Now, StringTable.GetInstance().GetString("STR_TASK_MESSAGE_SEND_TX_FAILED", iLang), TaskColor.Red);
                    Constant.TaskMessages.Add(taskMessage);
                    TaskChangedEventHandler?.Invoke(this, taskMessage);
                    isSendCoin = true;
                }));
            }
        }

        private void btnQRCode_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.FileName = "wallet"; // Default file name
            dlg.DefaultExt = "db3";
            dlg.Filter = "Data Files (*.db3)|*.db3";

            // Show save file dialog box
            Nullable<bool> result = dlg.ShowDialog();

            // Process save file dialog box results
            if (result == true)
            {
                // Save document
                
            }
        }

        private void ShowExpandedPan()
        {
            if (isExpandedPan == true)
            {
                btnPlus.Content = "-";
                panSpendable.Visibility = Visibility.Visible;
                panFee.Visibility = Visibility.Visible;
            }
            else
            {
                btnPlus.Content = "+";
                panSpendable.Visibility = Visibility.Collapsed;
                panFee.Visibility = Visibility.Collapsed;
            }
        }

        private void btnPlus_Click(object sender, RoutedEventArgs e)
        {
            isExpandedPan = !isExpandedPan;

            ShowExpandedPan();
        }

        private void cmbAssetType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                AssetTypeItem tag = ((ComboBoxItem)cmbAssetType.SelectedItem).Tag as AssetTypeItem;
                AssetState asset = Blockchain.Default.GetAssetState(tag.AssetID);

                if (fromAddress == null)
                {
                    foreach (UInt160 scriptHash in Constant.CurrentWallet.GetAddresses().ToArray())
                    {
                        VerificationContract contract = Constant.CurrentWallet.GetContract(scriptHash);
                        fromAddress = contract.Address;
                    }
                }

                if (Wallet.GetAddressVersion(fromAddress) == Wallet.AnonymouseAddressVersion || Wallet.GetAddressVersion(fromAddress) == Wallet.StealthAddressVersion)
                {
                    TxbFee.Text = String.Format(StringTable.GetInstance().GetString("STR_SP_AFEE", iLang), Blockchain.UtilityToken.A_Fee.ToString(), "XQG");
                    txbFeeAmount.IsEnabled = false;
                    txbFeeAmount.Text = Blockchain.UtilityToken.A_Fee.ToString();
                }
                else
                {
                    if (Wallet.GetAddressVersion(txbReceiveAddress.Text) == Wallet.AnonymouseAddressVersion || Wallet.GetAddressVersion(txbReceiveAddress.Text) == Wallet.StealthAddressVersion)
                    {
                        TxbFee.Text = String.Format(StringTable.GetInstance().GetString("STR_SP_AFEE", iLang), Blockchain.UtilityToken.A_Fee.ToString(), "XQG");
                        txbFeeAmount.IsEnabled = false;
                        txbFeeAmount.Text = Blockchain.UtilityToken.A_Fee.ToString();
                    }
                    else
                    {
                        TxbFee.Text = String.Format(StringTable.GetInstance().GetString("STR_SP_FEE", iLang), (asset.FeeMin).ToString(), "XQG", (asset.FeeMax).ToString(), "XQG");
                        txbFeeAmount.IsEnabled = true;
                    }
                }

                if (tag != null)
                {
                    TxbSpendable.Text = String.Format(StringTable.GetInstance().GetString("STR_SP_SPENDABLE", iLang), tag.Value.ToString(), tag.Name);
                }
                else
                {
                    TxbSpendable.Text = String.Format(StringTable.GetInstance().GetString("STR_SP_SPENDABLE", iLang), 0, "");
                }
            }
            catch (Exception)
            {
                TxbSpendable.Text = String.Format(StringTable.GetInstance().GetString("STR_SP_SPENDABLE", iLang), 0, "");
            }
        }

        private void TxbFeeAmount_LostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                AssetTypeItem tag = ((ComboBoxItem)cmbAssetType.SelectedItem).Tag as AssetTypeItem;
                AssetState asset = Blockchain.Default.GetAssetState(tag.AssetID);

                if (txbFeeAmount.Text == "") {
                    return;
                }

                Fixed8 fee = Fixed8.Satoshi * Convert.ToInt64(100000000 * Convert.ToDouble(txbFeeAmount.Text));

                if (fromAddress == null)
                {
                    foreach (UInt160 scriptHash in Constant.CurrentWallet.GetAddresses().ToArray())
                    {
                        VerificationContract contract = Constant.CurrentWallet.GetContract(scriptHash);
                        fromAddress = contract.Address;
                    }
                }

                if (Wallet.GetAddressVersion(fromAddress) == Wallet.AnonymouseAddressVersion || Wallet.GetAddressVersion(fromAddress) == Wallet.StealthAddressVersion)
                {
                    if (asset.AssetType == AssetType.GoverningToken)
                    {
                        if (asset.AFee < fee)
                        {
                            throw new Exception();
                        }
                    }
                    else
                    {
                        if (Fixed8.Satoshi * 10000000 + asset.AFee < fee)
                        {
                            throw new Exception();
                        }
                    }
                }
                else
                {
                    if (asset.FeeMin > fee || asset.FeeMax < fee)
                    {
                        throw new Exception();
                    }
                }
            }
            catch (Exception)
            {
                txbFeeAmount.Text = "";
                StaticUtils.ShowMessageBox(StaticUtils.ErrorBrush, StringTable.GetInstance().GetString("STR_RP_ERR_INPUT_FEE_IN_LIMIT", iLang));
            }
        }

        private void TxbReceiveAddress_LostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                AssetTypeItem tag = ((ComboBoxItem)cmbAssetType.SelectedItem).Tag as AssetTypeItem;
                AssetState asset = Blockchain.Default.GetAssetState(tag.AssetID);

                if (fromAddress == null)
                {
                    foreach (UInt160 scriptHash in Constant.CurrentWallet.GetAddresses().ToArray())
                    {
                        VerificationContract contract = Constant.CurrentWallet.GetContract(scriptHash);
                        fromAddress = contract.Address;
                    }
                }

                if (Wallet.GetAddressVersion(fromAddress) == Wallet.AnonymouseAddressVersion || Wallet.GetAddressVersion(fromAddress) == Wallet.StealthAddressVersion)
                {
                    TxbFee.Text = String.Format(StringTable.GetInstance().GetString("STR_SP_AFEE", iLang), Blockchain.UtilityToken.A_Fee.ToString(), "XQG");
                    txbFeeAmount.IsEnabled = false;
                    txbFeeAmount.Text = Blockchain.UtilityToken.A_Fee.ToString();
                }
                else
                {
                    if (Wallet.GetAddressVersion(txbReceiveAddress.Text) == Wallet.AnonymouseAddressVersion || Wallet.GetAddressVersion(txbReceiveAddress.Text) == Wallet.StealthAddressVersion)
                    {
                        TxbFee.Text = String.Format(StringTable.GetInstance().GetString("STR_SP_AFEE", iLang), asset.AFee.ToString(), "XQG");
                        txbFeeAmount.IsEnabled = false;
                        txbFeeAmount.Text = asset.AFee.ToString();

                    }
                    else
                    {
                        TxbFee.Text = String.Format(StringTable.GetInstance().GetString("STR_SP_FEE", iLang), (asset.FeeMin).ToString(), "XQG", (asset.FeeMax).ToString(), "XQG");
                        txbFeeAmount.IsEnabled = true;
                    }
                }

                if (tag != null)
                {
                    TxbSpendable.Text = String.Format(StringTable.GetInstance().GetString("STR_SP_SPENDABLE", iLang), tag.Value.ToString(), tag.Name);
                }
                else
                {
                    TxbSpendable.Text = String.Format(StringTable.GetInstance().GetString("STR_SP_SPENDABLE", iLang), 0, "");
                }
            }
            catch (Exception)
            {
                TxbSpendable.Text = String.Format(StringTable.GetInstance().GetString("STR_SP_SPENDABLE", iLang), 0, "");
            }
        }
    }
}
