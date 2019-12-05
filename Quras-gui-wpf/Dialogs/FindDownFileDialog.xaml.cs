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
using Pure.Wallets;

namespace Quras_gui_wpf.Dialogs
{
    /// <summary>
    /// Interaction logic for FindDownFileDialog.xaml
    /// </summary>
    public partial class FindDownFileDialog : Window, System.IDisposable
    {
        private LANG iLang => Constant.GetLang();
        private List<TxOutPutItem> outPutItems;
        private AssetState assetState;
        private DispatcherTimer errorMsgTimer = new DispatcherTimer();
        private Fixed8 totalAmount = Fixed8.Zero;

        public FindDownFileDialog()
        {
            InitializeComponent();
            InitInstance();
        }

        public FindDownFileDialog(Window parent, AssetState state)
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
            
        }

        public void RefreshLanguage()
        {
            TxbFindFileTitle.Text = StringTable.GetInstance().GetString("STR_FDFD_TITLE", iLang);
            TxbFileInfoToSearch.Tag = StringTable.GetInstance().GetString("STR_FDFD_TAG_SEARCH_INFO", iLang);
            btnSearch.Content = StringTable.GetInstance().GetString("STR_FDFD_BUTTON_SEARCH", iLang);
            btnOK.Content = StringTable.GetInstance().GetString("STR_FDFD_BUTTON_OK", iLang);
            

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
            this.DialogResult = true;
            this.Close();
        }


        public void Dispose()
        {
            
        }

        private string checkFields()
        {
            /*if (TxbAddressToAdd.Text.Length == 0)
            {
                return "STR_ERR_NOT_INPUT_ADDRESS";
            }
            if (TxbAmount.Text.Length == 0)
            {
                return "STR_ERR_NOT_INPUT_AMOUNT";
            }
            try
            {
                if (Wallet.GetAddressVersion(TxbAddressToAdd.Text) != Wallet.AddressVersion)
                {
                    return "STR_ERR_NOT_INPUT_ADDRESS_VERSION";
                }
            }
            catch
            {
                return "STR_ERR_ADDRESS_FORMAT";
            }

            Fixed8 amount;
            if (!Fixed8.TryParse(TxbAmount.Text, out amount))
            {
                return "STR_ERR_AMOUNT_FORMAT";
            }*/

            return "STR_SUCCESS";
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            /*string checkResult = checkFields();

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

            calculateTotalAmount();*/
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

            
        }

        private void ShowErrorMessage(string ErrMsg)
        {
            errorMsgTimer.Tick -= UpdateTimer;
            errorMsgTimer.Tick += new EventHandler(UpdateTimer);
            errorMsgTimer.Interval = new TimeSpan(0, 0, 0, 5); // 1 hour
            errorMsgTimer.Start();
        }

        private void UpdateTimer(object sender, EventArgs args)
        {
            errorMsgTimer.Stop();
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
