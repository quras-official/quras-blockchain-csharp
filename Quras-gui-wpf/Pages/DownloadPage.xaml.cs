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
using System.Globalization;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using Quras;
using Quras.VM;
using Quras.Core;
using Quras.IO.Json;
using Quras.Wallets;
using Quras.SmartContract;
using Quras.Cryptography.ECC;
using Quras_gui_wpf.Global;
using Quras_gui_wpf.Utils;
using Quras_gui_wpf.Dialogs;
using System.Net;
using System.Web.Script.Serialization;
using Quras_gui_wpf.Properties;
using Quras.Implementations.Wallets.EntityFramework;
using Quras_gui_wpf.Controls;
using Newtonsoft.Json;

namespace Quras_gui_wpf.Pages
{
    /// <summary>
    /// Interaction logic for DownloadPage.xaml
    /// </summary>
    public partial class DownloadPage : UserControl
    {
        private LANG iLang => Constant.GetLang();
        List<PendingFileItem> pendingFileList;
        HttpDownFileInformation selectedFileInformation;


        public DownloadPage()
        {
            selectedFileInformation.file_name = "";
            pendingFileList = new List<PendingFileItem>();
            InitializeComponent();
        }

        public void RefreshLanguage()
        {
            /*TxbHeader.Text = StringTable.GetInstance().GetString("STR_AAP_TITLE", iLang);
            TxbComment.Text = StringTable.GetInstance().GetString("STR_AAP_COMMENT", iLang);

            tbMakeTxHeader.Text = StringTable.GetInstance().GetString("STR_AAP_HDR_MAKE_TX", iLang);

            tbTxScriptHashComment.Text = StringTable.GetInstance().GetString("STR_AAP_SCRIPT_HASH_COMMENT", iLang);
            TxbTxScriptHash.Tag = StringTable.GetInstance().GetString("STR_AAP_SCRIPT_HASH_COMMENT", iLang);

            tbTxResult.Text = StringTable.GetInstance().GetString("STR_AAP_TX_RESULT_COMMENT", iLang);
            TxbTxResult.Tag = StringTable.GetInstance().GetString("STR_AAP_TX_RESULT_COMMENT", iLang);*/
        }

        private void InitInterface()
        {

        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            InitInterface();
            RefreshLanguage();
        }
        private void FileSelectedEvent(object sender, HttpDownFileInformation info)
        {
            selectedFileInformation = info;

            TxbChooseFile.Text = selectedFileInformation.file_name;
            TxbDownFileName.Text = selectedFileInformation.file_name;
            TxbFileDescription.Text = selectedFileInformation.file_description;

            BtnReqDownload.IsEnabled = true;
        }
        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            using (FindDownFileDialog dlg = new FindDownFileDialog(Application.Current.MainWindow))
            {
                dlg.FileSelectedEvent += this.FileSelectedEvent;
                if ((bool)dlg.ShowDialog() == true)
                {
                }
            }
        }

        private void BtnRequestDownload_Click(object sender, RoutedEventArgs e)
        {
            DownFileInformation fileInformation;

            fileInformation.txHash = UInt256.Parse(selectedFileInformation.txid);
            fileInformation.FileName = selectedFileInformation.file_name;
            fileInformation.FileDescription = selectedFileInformation.file_description;
            fileInformation.FileURL = selectedFileInformation.file_url;
            fileInformation.PayAmount = Fixed8.Parse(selectedFileInformation.pay_amount.ToString());
            fileInformation.uploadHash = UInt160.Parse(selectedFileInformation.upload_address);

            JArray obj = (JArray)JObject.Parse(selectedFileInformation.file_verifiers);
            fileInformation.FileVerifiers = new List<UInt160>();
            for(int i = 0; i < obj.Count; i ++)
            {
                dynamic value = JsonConvert.DeserializeObject(obj[i].ToString());
                fileInformation.FileVerifiers.Add(UInt160.Parse(value));
            }

            foreach (PendingFileItem pendingFile in pendingFileList)
            {
                if (pendingFile.transInfo.txHash == fileInformation.txHash)
                {
                    StaticUtils.ShowMessageBox(StaticUtils.ErrorBrush, StringTable.GetInstance().GetString("STR_SP_SENDING_DUPLICATED", iLang));
                    return;
                }

            }


            /*if (TxbChooseFile.Text == "" || fileInformation.FileName == "" || fileInformation.FileURL == "" || fileInformation.FileVerifiers.Count == 0 ||
                fileInformation.PayAmount == Fixed8.Zero || fileInformation.txHash == UInt256.Zero || fileInformation.FileDescription == "" || fileInformation.uploadHash == UInt160.Zero)
            {
                StaticUtils.ShowMessageBox(StaticUtils.ErrorBrush, StringTable.GetInstance().GetString("STR_NO_FILE_SELECTED", iLang));
                return;
            }*/

            UInt160 walletAddrHash = UInt160.Zero;
            foreach (UInt160 scriptHash in Constant.CurrentWallet.GetAddresses().ToArray())
            {
                if (Wallet.GetAddressVersion(Wallet.ToAddress(scriptHash)) == Wallet.AddressVersion)
                    walletAddrHash = scriptHash;
            }

            DownloadRequestTransaction finalTx = Constant.CurrentWallet.MakeTransaction(new DownloadRequestTransaction
            {
                Version = 1,
                txHash = fileInformation.txHash,
                FileName = fileInformation.FileName,
                FileDescription = fileInformation.FileDescription,
                FileURL = fileInformation.FileURL,
                PayAmount = fileInformation.PayAmount,
                FileVerifiers = fileInformation.FileVerifiers.ToArray(),
                Attributes = new TransactionAttribute[0],
                uploadHash = fileInformation.uploadHash,
                downloadHash = walletAddrHash,
                Inputs = new CoinReference[0],
                Outputs = new TransactionOutput[0]
            });

            if (finalTx == null)
            {
                StaticUtils.ShowMessageBox(StaticUtils.ErrorBrush, StringTable.GetInstance().GetString("STR_SP_SEDDING_FAILED", iLang));
                return;
            }

            Global.Helper.SignAndShowInformation(finalTx);
            StaticUtils.ShowMessageBox(StaticUtils.GreenBrush, StringTable.GetInstance().GetString("STR_SUC_TX_SUCCESSED", iLang));
        }

        public void AddPendingFileItem(TransactionInfo info)
        {
            if (info.Transaction is DownloadRequestTransaction)
            {
                DownloadRequestTransaction dtx = (DownloadRequestTransaction)info.Transaction;
                PendingFileItem item = new PendingFileItem();

                item.transInfo = dtx;
                item.TxbFileTitle.Text = dtx.FileName;
                item.approved = 0;
                item.approvalTotal = dtx.FileVerifiers.Length;
                item.TxbApproval.Text = string.Format("{0} / {1}", item.approved, item.approvalTotal);
                item.TxbAmount.Text = string.Format("{0} XQG", dtx.PayAmount);
                item.payTxEvent += this.PayTxEvent;

                pendingFileList.Add(item);
                stackFileList.Children.Add(item);
            }
        }

        public void AddApprovalToPending(UInt256 dTXhash)
        {
            foreach (PendingFileItem item in pendingFileList)
            {
                if (item.transInfo.Hash == dTXhash)
                {
                    item.approved++;
                    if (item.approved >= item.approvalTotal)
                    {
                        item.approved = item.approvalTotal;
                    }

                    item.TxbApproval.Text = string.Format("{0} / {1}", item.approved, item.approvalTotal);
                    item.btnPay.Visibility = Visibility.Visible;
                }
            }
        }

        public void SetPayFlag(UInt256 dTXhash)
        {
            foreach (PendingFileItem item in pendingFileList)
            {
                if (item.transInfo.Hash == dTXhash)
                {
                    item.btnPay.Visibility = Visibility.Hidden;
                    item.btnDownload.Visibility = Visibility.Visible;
                    item.progDownPercent.Visibility = Visibility.Visible;
                    break;
                }
            }
        }

        private void PayTxEvent(object sender, PendingFileItem item)
        {
            try
            {
                PayFileTransaction tx = new PayFileTransaction();
                List<TransactionAttribute> attributes = new List<TransactionAttribute>();
                tx.dTXHash = item.transInfo.Hash;
                tx.Attributes = attributes.ToArray();
                TransactionOutput outPut = new TransactionOutput();
                outPut.ScriptHash = item.transInfo.uploadHash;
                outPut.Value = item.transInfo.PayAmount;
                outPut.AssetId = Blockchain.UtilityToken.Hash;
                outPut.Fee = Fixed8.Zero;
                tx.Outputs = new TransactionOutput[1];
                tx.Outputs[0] = outPut;
                if (tx is PayFileTransaction ctx)
                {
                    tx = Constant.CurrentWallet.MakeTransactionFrom(ctx, Wallet.ToAddress(item.transInfo.downloadHash));
                    if (tx == null)
                    {
                        StaticUtils.ShowMessageBox(StaticUtils.ErrorBrush, StringTable.GetInstance().GetString("STR_SP_SEDDING_FAILED", iLang));
                        return;
                    }
                }

                Global.Helper.SignAndShowInformation(tx);
                StaticUtils.ShowMessageBox(StaticUtils.GreenBrush, StringTable.GetInstance().GetString("STR_SUC_TX_SUCCESSED", iLang));
            }
            catch (Exception ex)
            {
            }

        }
    }
}
