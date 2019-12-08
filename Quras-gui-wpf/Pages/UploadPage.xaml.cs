using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System;
using System.IO;
using System.Web;

using Pure;
using Pure.VM;
using Pure.Core;
using Pure.IO.Json;
using Pure.SmartContract;
using Quras_gui_wpf.Global;
using Quras_gui_wpf.Utils;
using System.Windows.Forms;
using System.Net;
using Newtonsoft.Json;
using Quras_gui_wpf.Controls;
using System.Collections.Generic;
using Pure.Wallets;

namespace Quras_gui_wpf.Pages
{
    /// <summary>
    /// Interaction logic for UploadPage.xaml
    /// </summary>
    public partial class UploadPage : System.Windows.Controls.UserControl
    {
        private LANG iLang => Constant.GetLang();
        private static readonly Fixed8 net_fee = Fixed8.FromDecimal(0.001m);
        private Fixed8 currentFee = Fixed8.Zero;
        public List<FileVerifierItem> verifierList;

        private string uploadURL;

        public UploadPage()
        {
            verifierList = new List<FileVerifierItem>();
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

        private void BtnLaunch_Click(object sender, RoutedEventArgs e)
        {
            if (TxbChooseFile.Text == "")
            {
                StaticUtils.ShowMessageBox(StaticUtils.ErrorBrush, StringTable.GetInstance().GetString("STR_NO_FILE_SELECTED", iLang));
                return;
            }

            if (TxbFileDescription.Text == "")
            {
                StaticUtils.ShowMessageBox(StaticUtils.ErrorBrush, StringTable.GetInstance().GetString("STR_NO_FILE_DESCRIPTION", iLang));
                return;
            }
            if (stackOutPuts.Children.Count == 0)
            {
                StaticUtils.ShowMessageBox(StaticUtils.ErrorBrush, StringTable.GetInstance().GetString("STR_NO_VERIFIERS", iLang));
                return;
            }

            List<UInt160> verifiers = new List<UInt160>();

            for (int i = 0 ; i < verifierList.Count ; i ++)
            {
                verifiers.Add(Wallet.ToScriptHash(verifierList[i].TxbAddress.Text));
            }

            UInt160 walletAddrHash = UInt160.Zero;
            foreach (UInt160 scriptHash in Constant.CurrentWallet.GetAddresses().ToArray())
            {
                if (Wallet.GetAddressVersion(Wallet.ToAddress(scriptHash)) == Wallet.AddressVersion)
                    walletAddrHash = scriptHash;
            }

            UploadRequestTransaction finalTx = Constant.CurrentWallet.MakeTransaction(new UploadRequestTransaction
            {
                Version = 1,
                FileName = TxbChooseFile.Text,
                FileDescription = TxbFileDescription.Text,
                FileURL = uploadURL,
                PayAmount = Fixed8.Parse(TxbPayAmount.Text),
                FileVerifiers = verifiers.ToArray(),
                Attributes = new TransactionAttribute[0],
                uploadHash = walletAddrHash,
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

        public InvocationTransaction GetTransaction()
        {
            return null;
        }

        


        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            InitInterface();
            RefreshLanguage();
        }

        private bool UploadFile(string filePath)
        {
            try
            {
                var wc = new WebClient();
                byte[] response_binary = wc.UploadFile("http://13.112.100.149/fUpload/upload.php", "POST", filePath);
                string response = System.Text.Encoding.UTF8.GetString(response_binary);
                dynamic stuff = JsonConvert.DeserializeObject(response);
                string url = stuff[0];
                string compare_url = "http://13.112.100.149/fUpload";
                uploadURL = url;
                return url.Substring(0, compare_url.Length) == compare_url;
            }
            catch (Exception e)
            {
                return false;
            }
        }
        private void btnUpload_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();

            dlg.DefaultExt = "*";
            dlg.Filter = "Choose Files to upload (*.*)|*.*";
            dlg.AddExtension = true;

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                if (UploadFile(dlg.FileName) == true)
                {
                    TxbChooseFile.Text = dlg.SafeFileName;
                    StaticUtils.ShowMessageBox(StaticUtils.GreenBrush, StringTable.GetInstance().GetString("STR_UPLOAD_SUCCEED", iLang));
                }
                else
                {
                    StaticUtils.ShowMessageBox(StaticUtils.ErrorBrush, StringTable.GetInstance().GetString("STR_UPLOAD_FAILED", iLang));
                }
            }
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (TxbAddressToAdd.Text == "")
            {
                return;
            }
            FileVerifierItem item = new FileVerifierItem();
            item.TxbAddress.Text = TxbAddressToAdd.Text;
            verifierList.Add(item);
            stackOutPuts.Children.Add(item);
        }
    }
}
