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

using Pure;
using Pure.VM;
using Pure.Core;
using Pure.IO.Json;
using Pure.Wallets;
using Pure.SmartContract;
using Pure.Cryptography.ECC;
using Quras_gui_wpf.Global;
using Quras_gui_wpf.Utils;
using Quras_gui_wpf.Dialogs;
using System.Net;
using System.Web.Script.Serialization;
using Quras_gui_wpf.Properties;
using Pure.Implementations.Wallets.EntityFramework;
using Quras_gui_wpf.Controls;

namespace Quras_gui_wpf.Pages
{
    /// <summary>
    /// Interaction logic for DownloadPage.xaml
    /// </summary>
    public partial class DownloadPage : UserControl
    {
        private LANG iLang => Constant.GetLang();
        List<PendingFileItem> pendingFileList;


        public DownloadPage()
        {
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

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
        }

        private void BtnRequestDownload_Click(object sender, RoutedEventArgs e)
        {
            DownFileInformation fileInformation;

            fileInformation.txHash = UInt256.Parse("0x65264a270f004b828761057d41040f4eaa4aa3148e4e5f72007723a8afa3a26d");
            fileInformation.FileName = "123.txt";
            fileInformation.FileDescription = "National Security Report";
            fileInformation.FileURL = "http://13.112.100.149/fUpload/uploads/1575620651/123.txt";
            fileInformation.PayAmount = Fixed8.Parse("10");
            fileInformation.uploadHash = UInt160.Parse("0x1154e640e82afa5d6af784f2876bdf180f412291");
            fileInformation.FileVerifiers = new List<UInt160>();
            fileInformation.FileVerifiers.Add(UInt160.Parse("0x85d169f42cdf0659aac109f4e3e87a79ad360481"));


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

                pendingFileList.Add(item);
                stackFileList.Children.Add(item);
            }
        }

        public void AddApprovalToPending(UInt256 dTXhash)
        {
            //foreach()
        }
    }
}
