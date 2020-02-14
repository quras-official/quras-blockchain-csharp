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

namespace Quras_gui_wpf.Pages
{
    /// <summary>
    /// Interaction logic for ApprovePage.xaml
    /// </summary>
    public partial class ApprovePage : UserControl
    {
        List<ApprovalItem> approvalItemList;
        private LANG iLang => Constant.GetLang();
        public ApprovePage()
        {
            approvalItemList = new List<ApprovalItem>();
            InitializeComponent();
        }

        public void RefreshLanguage()
        {
            /*TxbHeader.Text = StringTable.GetInstance().GetString("STR_AAP_TITLE", iLang);
            TxbComment.Text = StringTable.GetInstance().GetString("STR_AAP_COMMENT", iLang);
            TxbRegAssetTitle.Text = StringTable.GetInstance().GetString("STR_AAP_REG_ASSET_TITLE", iLang);
            TxbRegComment1.Text = StringTable.GetInstance().GetString("STR_AAP_REG_COMMENT1", iLang);*/

        }

        private void InitInterface()
        {


        }

        public bool OnlyHexInString(string test)
        {
            // For C-style hex notation (0xFF) you can use @"\A\b(0[xX])?[0-9a-fA-F]+\b\Z"
            return System.Text.RegularExpressions.Regex.IsMatch(test, @"\A\b[0-9a-fA-F]+\b\Z");
        }

        public void InitBorderFields()
        {
        }

        public string CheckFields()
        {
            string ret = "STR_SUCCESS";
            return ret;
        }


        public InvocationTransaction GetTransaction()
        {
            return null;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            InitInterface();
            RefreshLanguage();
        }
        private void ApproveTxEvent(object sender, ApprovalItem item)
        {
            UInt160 walletAddrHash = UInt160.Zero;
            foreach (UInt160 scriptHash in Constant.CurrentWallet.GetAddresses().ToArray())
            {
                if (Wallet.GetAddressVersion(Wallet.ToAddress(scriptHash)) == Wallet.AddressVersion)
                    walletAddrHash = scriptHash;
            }

            ApproveDownloadTransaction finalTx = Constant.CurrentWallet.MakeTransaction(new ApproveDownloadTransaction
            {
                dTXHash = item.dTXhash,
                approveHash = walletAddrHash,
                downloadHash = Wallet.ToScriptHash(item.TxbDownAddress.Text),
                approveState = true
            });

            if (finalTx == null)
            {
                StaticUtils.ShowMessageBox(StaticUtils.ErrorBrush, StringTable.GetInstance().GetString("STR_SP_SEDDING_FAILED", iLang));
                return;
            }

            Global.Helper.SignAndShowInformation(finalTx);
            StaticUtils.ShowMessageBox(StaticUtils.GreenBrush, StringTable.GetInstance().GetString("STR_SUC_TX_SUCCESSED", iLang));
            item.BtnApprove.IsEnabled = false;
            item.BtnDisput.IsEnabled = false;
        }

        private void DisputTxEvent(object sender, ApprovalItem item)
        {
            UInt160 walletAddrHash = UInt160.Zero;
            foreach (UInt160 scriptHash in Constant.CurrentWallet.GetAddresses().ToArray())
            {
                if (Wallet.GetAddressVersion(Wallet.ToAddress(scriptHash)) == Wallet.AddressVersion)
                    walletAddrHash = scriptHash;
            }

            ApproveDownloadTransaction finalTx = Constant.CurrentWallet.MakeTransaction(new ApproveDownloadTransaction
            {
                dTXHash = item.dTXhash,
                approveHash = walletAddrHash,
                downloadHash = UInt160.Parse(item.TxbDownAddress.Text),
                approveState = true
            });

            if (finalTx == null)
            {
                StaticUtils.ShowMessageBox(StaticUtils.ErrorBrush, StringTable.GetInstance().GetString("STR_SP_SEDDING_FAILED", iLang));
                return;
            }

            Global.Helper.SignAndShowInformation(finalTx);
            StaticUtils.ShowMessageBox(StaticUtils.GreenBrush, StringTable.GetInstance().GetString("STR_SUC_TX_SUCCESSED", iLang));
            item.BtnApprove.IsEnabled = false;
            item.BtnDisput.IsEnabled = false;
        }

        public void AddApprovalItem(TransactionInfo info)
        {
            if (info.Transaction is DownloadRequestTransaction)
            {
                DownloadRequestTransaction dtx = (DownloadRequestTransaction)info.Transaction;
                ApprovalItem item = new ApprovalItem();

                item.dTXhash = dtx.Hash;
                item.TxbFileName.Text = dtx.FileName;
                item.TxbDescription.Text = dtx.FileDescription;
                item.TxbUpAddress.Text = Wallet.ToAddress(dtx.uploadHash);
                item.TxbDownAddress.Text = Wallet.ToAddress(dtx.downloadHash);
                item.ApproveTxEvent += this.ApproveTxEvent;
                item.DisputTxEvent += this.DisputTxEvent;

                approvalItemList.Add(item);
                stackApproveItemList.Children.Add(item);
            }
            
        }

        public void RemoveApprovalItem(UInt256 dTXhash)
        {
            foreach (ApprovalItem item in approvalItemList)
            {
                if (item.dTXhash == dTXhash)
                {
                    approvalItemList.Remove(item);
                    stackApproveItemList.Children.Remove(item);
                    break;
                }
            }
        }

    }
}
