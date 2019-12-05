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

namespace Quras_gui_wpf.Pages
{
    /// <summary>
    /// Interaction logic for DownloadPage.xaml
    /// </summary>
    public partial class DownloadPage : UserControl
    {
        private LANG iLang => Constant.GetLang();
        private InvocationTransaction tx;
        private IssueTransaction issueTx;
        private static readonly Fixed8 net_fee = Fixed8.FromDecimal(0.001m);
        private Fixed8 currentFee = Fixed8.Zero;
        private AssetState assetState;

        public DownloadPage()
        {
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

        public bool OnlyHexInString(string test)
        {
            // For C-style hex notation (0xFF) you can use @"\A\b(0[xX])?[0-9a-fA-F]+\b\Z"
            return System.Text.RegularExpressions.Regex.IsMatch(test, @"\A\b[0-9a-fA-F]+\b\Z");
        }

        public void InitBorderFields()
        {
            bdAssetType.BorderBrush = new SolidColorBrush(Colors.Green);
        }

        

        public string CheckFields()
        {
            string ret = "STR_SUCCESS";

            
            return ret;
        }

        

        public InvocationTransaction GetTransaction()
        {
            return null;
            /*AssetType asset_type = (AssetType)cmbAssetType.SelectedItem;
            string name = string.IsNullOrWhiteSpace(TxbAssetName.Text) ? string.Empty : $"[{{\"lang\":\"{CultureInfo.CurrentCulture.Name}\",\"name\":\"{TxbAssetName.Text}\"}}]";
            Fixed8 amount = (bool)ChkNoLimit.IsChecked ? -Fixed8.Satoshi : Fixed8.Parse(TxbAssetAmount.Text);
            byte precision = (byte)int.Parse(TxbAssetPrecision.Text);
            ECPoint owner = (ECPoint)cmbAssetOwner.SelectedItem;
            UInt160 admin = Wallet.ToScriptHash(cmbAssetAdmin.Text);
            UInt160 issuer = Wallet.ToScriptHash(cmbAssetIssuer.Text);
            Fixed8 AFee = Fixed8.Parse(TxbAssetAFee.Text);
            Fixed8 TFee = Fixed8.Zero;
            Fixed8 TFeeMin = Fixed8.Parse(TxbAssetTFeeMin.Text);
            Fixed8 TFeeMax = Fixed8.Parse(TxbAssetTFeeMax.Text);
            UInt160 feeAddress = Wallet.ToScriptHash(TxbAssetFeeAddress.Text);
            using (ScriptBuilder sb = new ScriptBuilder())
            {
                sb.EmitSysCall("Pure.Asset.Create", asset_type, name, amount, precision, owner, admin, issuer, AFee, TFee, TFeeMin, TFeeMax, feeAddress);
                return new InvocationTransaction
                {
                    Attributes = new[]
                    {
                        new TransactionAttribute
                        {
                            Usage = TransactionAttributeUsage.Script,
                            Data = Contract.CreateSignatureRedeemScript(owner).ToScriptHash().ToArray()
                        }
                    },
                    Script = sb.ToArray()
                };
            }*/
        }

        public bool MakeAndTestTransaction()
        {
            tx = GetTransaction();

            TxbTxScriptHash.Text = tx.Script.ToHexString();

            tx.Version = 1;
            if (tx.Attributes == null) tx.Attributes = new TransactionAttribute[0];
            if (tx.Inputs == null) tx.Inputs = new CoinReference[0];
            if (tx.Outputs == null) tx.Outputs = new TransactionOutput[0];
            if (tx.Scripts == null) tx.Scripts = new Witness[0];
            ApplicationEngine engine = ApplicationEngine.Run(tx.Script, tx);
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"VM State: {engine.State}");
            sb.AppendLine($"Gas Consumed: {engine.GasConsumed}");
            sb.AppendLine($"Evaluation Stack: {new JArray(engine.EvaluationStack.Select(p => p.ToParameter().ToJson()))}");
            TxbTxResult.Text = sb.ToString();
            if (!engine.State.HasFlag(VMState.FAULT))
            {
                tx.Gas = engine.GasConsumed - Fixed8.FromDecimal(10);
                if (tx.Gas < Fixed8.Zero) tx.Gas = Fixed8.Zero;
                tx.Gas = tx.Gas.Ceiling();
                Fixed8 fee = tx.Gas.Equals(Fixed8.Zero) ? net_fee : tx.Gas;
                currentFee = fee;
                StaticUtils.ShowMessageBox(StaticUtils.GreenBrush, StringTable.GetInstance().GetString("STR_SUC_TEST_TRANSACTION", iLang));
                return true;
            }
            else
            {
                StaticUtils.ShowMessageBox(StaticUtils.ErrorBrush, StringTable.GetInstance().GetString("STR_ERR_TEST_TRANSACTION", iLang));
                return false;
            }
        }


        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            InitInterface();
            RefreshLanguage();
        }


        


        

        

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            using (FindDownFileDialog dlg = new FindDownFileDialog(Application.Current.MainWindow, assetState))
            {
                if ((bool)dlg.ShowDialog() == true)
                {
                }
            }
        }

        private void BtnRequestDownload_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
