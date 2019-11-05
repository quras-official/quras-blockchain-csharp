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
using System.Windows.Shapes;
using System.Windows.Threading;

using Pure;
using Pure.Core;

using Quras_gui_wpf.Utils;
using Quras_gui_wpf.Global;
using Quras_gui_wpf.Controls;

namespace Quras_gui_wpf.Dialogs
{
    /// <summary>
    /// Interaction logic for TxOutPutDialog.xaml
    /// </summary>
    public partial class TxOutPutDialog : Window, System.IDisposable
    {
        private LANG iLang => Constant.GetLang();
        private List<TxOutPutItem> outPutItems;
        private AssetState assetState;
        private DispatcherTimer errorMsgTimer = new DispatcherTimer();
        private Fixed8 totalAmount = Fixed8.Zero;

        public TxOutPutDialog()
        {
            InitializeComponent();
            InitInstance();
        }

        public TxOutPutDialog(Window parent, AssetState state)
        {
            Owner = parent;
            InitializeComponent();

            assetState = state;
            InitInstance();
            RefreshLanguage();
        }

        public void InitInstance()
        {
            outPutItems = new List<TxOutPutItem>();
            cmbAssetType.Items.Add(assetState.GetName());
            TxbAvailableAmount.Text = (assetState.Amount - assetState.Available).ToString();

            cmbAssetType.SelectedIndex = 0;
        }

        public void RefreshLanguage()
        {
            TxbTxOutTitle.Text = StringTable.GetInstance().GetString("STR_TOPD_TITLE", iLang);
            fhIssueAssetType.Text = StringTable.GetInstance().GetString("STR_TOPD_ASSET_TYPE", iLang);
            fhAvailableIssue.Text = StringTable.GetInstance().GetString("STR_TOPD_AVAILABLE_ISSUE", iLang);
            TxbAvailableAmount.Tag = StringTable.GetInstance().GetString("STR_TOPD_AVAILABLE_AMOUNT", iLang);
            TxbAddressToAdd.Tag = StringTable.GetInstance().GetString("STR_TOPD_ADDRESS", iLang);
            TxbAmount.Tag = StringTable.GetInstance().GetString("STR_TOPD_AMOUNT", iLang);
            btnYes.Content = StringTable.GetInstance().GetString("STR_BUTTON_YES", iLang);
            btnNo.Content = StringTable.GetInstance().GetString("STR_BUTTON_NO", iLang);
            TxbTotalAmount.Text = String.Format(StringTable.GetInstance().GetString("STR_ISSUE_TOTALS", iLang), totalAmount.ToString(), assetState.GetName());

            foreach(TxOutPutItem item in outPutItems)
            {
                item.RefreshLanguage();
            }
        }
        private void GridMouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                DragMove();
            }
            catch (Exception ex)
            {

            }
        }

        public List<TxOutPutItem> GetOutputList()
        {
            return outPutItems;
        }

        private void btnYes_Click(object sender, RoutedEventArgs e)
        {
            if (outPutItems.Count == 0)
            {
                ShowErrorMessage("STR_ERR_NO_OUTPUTS");
                return;
            }

            if (totalAmount > assetState.Amount - assetState.Available)
            {
                ShowErrorMessage("STR_ERR_ISSUE_AMOUNT_NOT_SUFFICIENT");
                return;
            }
            this.DialogResult = true;
            this.Close();
        }

        private void btnNo_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }

        public void Dispose()
        {
            
        }

        private string checkFields()
        {
            if (TxbAddressToAdd.Text.Length == 0)
            {
                return "STR_ERR_NOT_INPUT_ADDRESS";
            }
            if (TxbAmount.Text.Length == 0)
            {
                return "STR_ERR_NOT_INPUT_AMOUNT";
            }
            try
            {
                Pure.Wallets.Wallet.GetAddressVersion(TxbAddressToAdd.Text);
            }
            catch
            {
                return "STR_ERR_ADDRESS_FORMAT";
            }

            Fixed8 amount;
            if (!Fixed8.TryParse(TxbAmount.Text, out amount))
            {
                return "STR_ERR_AMOUNT_FORMAT";
            }

            return "STR_SUCCESS";
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            string checkResult = checkFields();

            if (checkResult != "STR_SUCCESS")
            {
                ShowErrorMessage(checkResult);
                return;
            }

            Fixed8 amount;
            Fixed8.TryParse(TxbAmount.Text, out amount);

            TxOutPutItem item = new TxOutPutItem(TxbAddressToAdd.Text, amount);

            item.RemoveTxOutPutItemEvent += RemoveTxOutputItemEvent;
            outPutItems.Add(item);

            this.stackOutPuts.Children.Add(item);

            calculateTotalAmount();
        }

        private void RemoveTxOutputItemEvent(object sender, TxOutPutItem item)
        {
            item.RemoveTxOutPutItemEvent -= RemoveTxOutputItemEvent;
            outPutItems.Remove(item);
            this.stackOutPuts.Children.Remove(item);

            calculateTotalAmount();
        }

        private void calculateTotalAmount()
        {
            totalAmount = Fixed8.Zero;

            foreach(TxOutPutItem item in outPutItems)
            {
                totalAmount += item.GetAmount();
            }

            TxbTotalAmount.Text = String.Format(StringTable.GetInstance().GetString("STR_ISSUE_TOTALS", iLang), totalAmount.ToString(), assetState.GetName());
        }

        private void ShowErrorMessage(string ErrMsg)
        {
            TxbError.Text = StringTable.GetInstance().GetString(ErrMsg, iLang);
            TxbError.Visibility = Visibility.Visible;


            errorMsgTimer.Tick -= UpdateTimer;
            errorMsgTimer.Tick += new EventHandler(UpdateTimer);
            errorMsgTimer.Interval = new TimeSpan(0, 0, 0, 5); // 1 hour
            errorMsgTimer.Start();
        }

        private void UpdateTimer(object sender, EventArgs args)
        {
            errorMsgTimer.Stop();
            TxbError.Visibility = Visibility.Hidden;
        }

        public IssueTransaction GetTransaction()
        {
            return Constant.CurrentWallet.MakeTransaction(new IssueTransaction
            {
                Version = 1,
                Outputs = outPutItems.GroupBy(p => p.ScriptHash).Select(g => new TransactionOutput
                {
                    AssetId = (UInt256)assetState.AssetId,
                    Value = g.Sum(p => p.GetAmount()),
                    ScriptHash = g.Key
                }).ToArray()
            }, fee: Fixed8.One);
        }
    }
}
