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
    /// Interaction logic for AddAssetsPage.xaml
    /// </summary>
    public partial class AddAssetsPage : UserControl
    {
        private LANG iLang => Constant.GetLang();
        private InvocationTransaction tx;
        private IssueTransaction issueTx;
        private static readonly Fixed8 net_fee = Fixed8.FromDecimal(0.001m);
        private Fixed8 currentFee = Fixed8.Zero;
        private AssetState assetState;

        public AddAssetsPage()
        {
            InitializeComponent();
        }

        public void RefreshLanguage()
        {
            TxbHeader.Text = StringTable.GetInstance().GetString("STR_AAP_TITLE", iLang);
            TxbComment.Text = StringTable.GetInstance().GetString("STR_AAP_COMMENT", iLang);
            TxbRegAssetTitle.Text = StringTable.GetInstance().GetString("STR_AAP_REG_ASSET_TITLE", iLang);
            TxbRegComment1.Text = StringTable.GetInstance().GetString("STR_AAP_REG_COMMENT1", iLang);
            TxbRegComment2.Text = StringTable.GetInstance().GetString("STR_AAP_REG_COMMENT2", iLang);

            fHAssetType.Text = StringTable.GetInstance().GetString("STR_AAP_FH_ASSET_TYPE", iLang);
            fCAssetType.Text = StringTable.GetInstance().GetString("STR_AAP_FC_ASSET_TYPE", iLang);

            fHAssetName.Text = StringTable.GetInstance().GetString("STR_AAP_FH_ASSET_NAME", iLang);
            fCAssetName.Text = StringTable.GetInstance().GetString("STR_AAP_FC_ASSET_NAME", iLang);
            TxbAssetName.Tag = StringTable.GetInstance().GetString("STR_AAP_FH_ASSET_NAME", iLang);

            fHAssetAmount.Text = StringTable.GetInstance().GetString("STR_AAP_FH_ASSET_AMOUNT", iLang);
            fCAssetAmount.Text = StringTable.GetInstance().GetString("STR_AAP_FC_ASSET_AMOUNT", iLang);
            TxbAssetAmount.Tag = StringTable.GetInstance().GetString("STR_AAP_FH_ASSET_AMOUNT", iLang);

            fHAssetPrecision.Text = StringTable.GetInstance().GetString("STR_AAP_FH_ASSET_PRECISION", iLang);
            fCAssetPrecision.Text = StringTable.GetInstance().GetString("STR_AAP_FC_ASSET_PRECISION", iLang);
            TxbAssetPrecision.Tag = StringTable.GetInstance().GetString("STR_AAP_FH_ASSET_PRECISION", iLang);

            fHAssetAFee.Text = StringTable.GetInstance().GetString("STR_AAP_FH_ASSET_AFEE", iLang);
            fCAssetAFee.Text = StringTable.GetInstance().GetString("STR_AAP_FC_ASSET_AFEE", iLang);
            TxbAssetAFee.Tag = StringTable.GetInstance().GetString("STR_AAP_FH_ASSET_AFEE", iLang);

            fHAssetTFeeMin.Text = StringTable.GetInstance().GetString("STR_AAP_FH_ASSET_TFEE_MIN", iLang);
            fCAssetTFeeMin.Text = StringTable.GetInstance().GetString("STR_AAP_FC_ASSET_TFEE_MIN", iLang);
            TxbAssetTFeeMin.Tag = StringTable.GetInstance().GetString("STR_AAP_FH_ASSET_TFEE_MIN", iLang);

            fHAssetTFeeMax.Text = StringTable.GetInstance().GetString("STR_AAP_FH_ASSET_TFEE_MAX", iLang);
            fCAssetTFeeMax.Text = StringTable.GetInstance().GetString("STR_AAP_FC_ASSET_TFEE_MAX", iLang);
            TxbAssetTFeeMax.Tag = StringTable.GetInstance().GetString("STR_AAP_FH_ASSET_TFEE_MAX", iLang);

            fHAssetFeeAddress.Text = StringTable.GetInstance().GetString("STR_AAP_FH_ASSET_FEE_ADDRESS", iLang);
            fCAssetFeeAddress.Text = StringTable.GetInstance().GetString("STR_AAP_FC_ASSET_FEE_ADDRESS", iLang);
            TxbAssetFeeAddress.Tag = StringTable.GetInstance().GetString("STR_AAP_FH_ASSET_FEE_ADDRESS", iLang);

            fHAssetOwner.Text = StringTable.GetInstance().GetString("STR_AAP_FH_ASSET_OWNER", iLang);
            fCAssetOwner.Text = StringTable.GetInstance().GetString("STR_AAP_FC_ASSET_OWNER", iLang);

            fHAssetAdmin.Text = StringTable.GetInstance().GetString("STR_AAP_FH_ASSET_ADMIN", iLang);
            fCAssetAdmin.Text = StringTable.GetInstance().GetString("STR_AAP_FC_ASSET_ADMIN", iLang);

            fHAssetIssuer.Text = StringTable.GetInstance().GetString("STR_AAP_FH_ASSET_ISSUER", iLang);
            fCAssetIssuer.Text = StringTable.GetInstance().GetString("STR_AAP_FC_ASSET_ISSUER", iLang);

            BtnMakeTransaction.Content = StringTable.GetInstance().GetString("STR_AAP_BTN_MAKE_TRANSACTION", iLang);

            tbMakeTxHeader.Text = StringTable.GetInstance().GetString("STR_AAP_HDR_MAKE_TX", iLang);

            tbTxScriptHashComment.Text = StringTable.GetInstance().GetString("STR_AAP_SCRIPT_HASH_COMMENT", iLang);
            TxbTxScriptHash.Tag = StringTable.GetInstance().GetString("STR_AAP_SCRIPT_HASH_COMMENT", iLang);

            tbTxResult.Text = StringTable.GetInstance().GetString("STR_AAP_TX_RESULT_COMMENT", iLang);
            TxbTxResult.Tag = StringTable.GetInstance().GetString("STR_AAP_TX_RESULT_COMMENT", iLang);

            tbAssetID.Text = StringTable.GetInstance().GetString("STR_AAP_ASSET_ID", iLang);
            TxbAssetID.Tag = StringTable.GetInstance().GetString("STR_AAP_ASSET_ID", iLang);

            BtnLaunch.Content = StringTable.GetInstance().GetString("STR_AAP_BTN_LAUNCH", iLang);

            ChkNoLimit.Content = StringTable.GetInstance().GetString("STR_AAP_NO_LIMIT", iLang);
            txbFee.Text = String.Format(StringTable.GetInstance().GetString("STR_SMART_CONTRACT_FEE", iLang), currentFee.ToString());

            TxbIssueAssetTitle.Text = StringTable.GetInstance().GetString("STR_AAP_ISSUE_ASSET_TITLE", iLang);
            TxbIssueAssetComment1.Text = StringTable.GetInstance().GetString("STR_AAP_ISSUE_ASSET_COMMENT", iLang);
            TxbIssueAssetComment2.Text = StringTable.GetInstance().GetString("STR_AAP_ISSUE_ASSET_COMMENT1", iLang);

            fHIssueAssetID.Text = StringTable.GetInstance().GetString("STR_AAP_ISSUE_ASSET_ID", iLang);
            fCIssueAssetID.Text = StringTable.GetInstance().GetString("STR_AAP_ISSUE_ASSET_ID_COMMENT", iLang);
            TxbIssueAssetID.Tag = StringTable.GetInstance().GetString("STR_AAP_ISSUE_ASSET_ID", iLang);

            fhIssueAssetName.Text = StringTable.GetInstance().GetString("STR_AAP_ISSUE_ASSET_NAME", iLang);
            fCIssueAssetName.Text = StringTable.GetInstance().GetString("STR_AAP_ISSUE_ASSET_NAME_COMMENT", iLang);
            TxbIssueAssetName.Tag = StringTable.GetInstance().GetString("STR_AAP_ISSUE_ASSET_NAME", iLang);

            fhIssueAssetIssued.Text = StringTable.GetInstance().GetString("STR_AAP_ISSUE_ASSET_ISSUED", iLang);
            fCIssueAssetIssued.Text = StringTable.GetInstance().GetString("STR_AAP_ISSUE_ASSET_ISSUED_COMMENT", iLang);
            TxbIssueAssetIssued.Tag = StringTable.GetInstance().GetString("STR_AAP_ISSUE_ASSET_ISSUED", iLang);

            fhIssueAssetPrecision.Text = StringTable.GetInstance().GetString("STR_AAP_ISSUE_ASSET_PRECISION", iLang);
            fCIssueAssetPrecision.Text = StringTable.GetInstance().GetString("STR_AAP_ISSUE_ASSET_PRECISION_COMMENT", iLang);
            TxbIssueAssetPrecision.Tag = StringTable.GetInstance().GetString("STR_AAP_ISSUE_ASSET_PRECISION", iLang);

            fhIssueAssetAdmin.Text = StringTable.GetInstance().GetString("STR_AAP_ISSUE_ASSET_ADMIN", iLang);
            fCIssueAssetAdmin.Text = StringTable.GetInstance().GetString("STR_AAP_ISSUE_ASSET_ADMIN_COMMENT", iLang);
            TxbIssueAssetAdmin.Tag = StringTable.GetInstance().GetString("STR_AAP_ISSUE_ASSET_ADMIN", iLang);

            fhIssueAssetOwner.Text = StringTable.GetInstance().GetString("STR_AAP_ISSUE_ASSET_OWNER", iLang);
            fCIssueAssetOwner.Text = StringTable.GetInstance().GetString("STR_AAP_ISSUE_ASSET_OWNER_COMMENT", iLang);
            TxbIssueAssetOwner.Tag = StringTable.GetInstance().GetString("STR_AAP_ISSUE_ASSET_OWNER", iLang);

            BtnMakeOutput.Content = StringTable.GetInstance().GetString("STR_AAP_ISSUE_BTN_MAKE_OUTPUT", iLang);

            tbIssueMakeTxHeader.Text = StringTable.GetInstance().GetString("STR_AAP_ISSUE_MAKE_TX_HEADER", iLang);

            tbIssueTxResult.Text = StringTable.GetInstance().GetString("STR_AAP_ISSUE_TX_RESULT", iLang);
            TxbIssueTxResult.Tag = StringTable.GetInstance().GetString("STR_AAP_ISSUE_TX_RESULT", iLang);
            txbIssueFee.Text = String.Format(StringTable.GetInstance().GetString("STR_SMART_CONTRACT_FEE", iLang), currentFee.ToString());
            BtnIssueLaunch.Content = StringTable.GetInstance().GetString("STR_AAP_BTN_LAUNCH", iLang);
        }

        private void InitInterface()
        {
            cmbAssetType.Items.Clear();
            cmbAssetOwner.Items.Clear();
            cmbAssetAdmin.Items.Clear();
            cmbAssetIssuer.Items.Clear();
            
            cmbAssetType.Items.Add(AssetType.AnonymousToken);
            cmbAssetType.Items.Add(AssetType.TransparentToken);

            if (Constant.CurrentWallet != null)
            {
                foreach (object item in Constant.CurrentWallet.GetContracts().Where(p => p.IsStandard && p.ContractType == KeyType.Transparent).Select(p => ((KeyPair)Constant.CurrentWallet.GetKey(p.PublicKeyHash)).PublicKey).ToArray())
                {
                    cmbAssetOwner.Items.Add(item);
                }

                foreach (object item in Constant.CurrentWallet.GetContracts().Select(p => p.Address).ToArray())
                {
                    cmbAssetAdmin.Items.Add(item);
                    cmbAssetIssuer.Items.Add(item);
                }
            }

            if (cmbAssetType.Items.Count > 0)
            {
                cmbAssetType.SelectedIndex = 0;
            }
            
            if (cmbAssetOwner.Items.Count > 0)
            {
                cmbAssetOwner.SelectedIndex = 0;
            }
            
            if (cmbAssetIssuer.Items.Count > 0)
            {
                cmbAssetIssuer.SelectedIndex = 0;
            }
            
            if (cmbAssetAdmin.Items.Count > 0)
            {
                cmbAssetAdmin.SelectedIndex = 0;
            }
           
        }

        public bool OnlyHexInString(string test)
        {
            // For C-style hex notation (0xFF) you can use @"\A\b(0[xX])?[0-9a-fA-F]+\b\Z"
            return System.Text.RegularExpressions.Regex.IsMatch(test, @"\A\b[0-9a-fA-F]+\b\Z");
        }

        public void InitBorderFields()
        {
            bdAssetOwner.BorderBrush = new SolidColorBrush(Colors.Green);
            bdAssetAdmin.BorderBrush = new SolidColorBrush(Colors.Green);
            bdAssetAmount.BorderBrush = new SolidColorBrush(Colors.Green);
            bdAssetName.BorderBrush = new SolidColorBrush(Colors.Green);
            bdAssetPrecision.BorderBrush = new SolidColorBrush(Colors.Green);
            bdAssetType.BorderBrush = new SolidColorBrush(Colors.Green);
            bdAssetIssuer.BorderBrush = new SolidColorBrush(Colors.Green);

            fHAssetAdmin.Foreground = new SolidColorBrush(Colors.Green);
            fHAssetAmount.Foreground = new SolidColorBrush(Colors.Green);
            fHAssetIssuer.Foreground = new SolidColorBrush(Colors.Green);
            fHAssetName.Foreground = new SolidColorBrush(Colors.Green);
            fHAssetOwner.Foreground = new SolidColorBrush(Colors.Green);
            fHAssetPrecision.Foreground = new SolidColorBrush(Colors.Green);
            fHAssetType.Foreground = new SolidColorBrush(Colors.Green);
        }

        public bool isDuplicatedToken(string assetName)
        {
            var wb = new WebClient();

            var response = wb.DownloadString(SettingsConfig.instance.ApiPrefix + "/v1/assets/all");

            var model = new JavaScriptSerializer().Deserialize<HttpAssetInfo>(response);

            foreach (var asset in model.assets)
            {
                if (asset.name == assetName)
                {
                    return true;
                }
            }
            return false;
        }

        public string CheckFields()
        {
            string ret = "STR_SUCCESS";

            if (TxbAssetName.Text.Length == 0)
            {
                bdAssetName.BorderBrush = new SolidColorBrush(Colors.Red);
                fHAssetName.Foreground = new SolidColorBrush(Colors.Red);
                ret = "STR_ERR_ASSET_NAME_FIELD";
            }

            try
            {
                if (ChkNoLimit.IsChecked == false)
                {
                    if (TxbAssetAmount.Text.Length == 0 || int.Parse(TxbAssetAmount.Text) == 0)
                    {
                        bdAssetAmount.BorderBrush = new SolidColorBrush(Colors.Red);
                        fHAssetAmount.Foreground = new SolidColorBrush(Colors.Red);
                        ret = "STR_ERR_ASSET_AMOUNT_FIELD";
                    }
                }
            }
            catch
            {
                bdAssetAmount.BorderBrush = new SolidColorBrush(Colors.Red);
                fHAssetAmount.Foreground = new SolidColorBrush(Colors.Red);
                ret = "STR_ERR_ASSET_AMOUNT_FIELD";
            }

            try
            {
                if (int.Parse(TxbAssetPrecision.Text) == 0)
                {
                    bdAssetPrecision.BorderBrush = new SolidColorBrush(Colors.Red);
                    fHAssetPrecision.Foreground = new SolidColorBrush(Colors.Red);
                    ret = "STR_ERR_ASSET_PRECISION_FIELD";
                }
            }
            catch (Exception)
            {
                bdAssetPrecision.BorderBrush = new SolidColorBrush(Colors.Red);
                fHAssetPrecision.Foreground = new SolidColorBrush(Colors.Red);
                ret = "STR_ERR_ASSET_PRECISION_FIELD";
            }

            try
            {
                if (Double.Parse(TxbAssetAFee.Text) < 0)
                {
                    bdAssetAFee.BorderBrush = new SolidColorBrush(Colors.Red);
                    fHAssetAFee.Foreground = new SolidColorBrush(Colors.Red);
                    ret = "STR_ERR_ASSET_AFEE_FIELD";
                }
            }
            catch (Exception)
            {
                bdAssetAFee.BorderBrush = new SolidColorBrush(Colors.Red);
                fHAssetAFee.Foreground = new SolidColorBrush(Colors.Red);
                ret = "STR_ERR_ASSET_AFEE_FIELD";
            }

            try
            {
                if (Double.Parse(TxbAssetTFeeMin.Text) < 0)
                {
                    bdAssetTFeeMin.BorderBrush = new SolidColorBrush(Colors.Red);
                    fHAssetTFeeMin.Foreground = new SolidColorBrush(Colors.Red);
                    ret = "STR_ERR_ASSET_TFEE_MIN_FIELD";
                }
            }
            catch (Exception)
            {
                bdAssetTFeeMin.BorderBrush = new SolidColorBrush(Colors.Red);
                fHAssetTFeeMin.Foreground = new SolidColorBrush(Colors.Red);
                ret = "STR_ERR_ASSET_TFEE_MIN_FIELD";
            }

            try
            {
                if (Double.Parse(TxbAssetTFeeMax.Text) < 0)
                {
                    bdAssetTFeeMax.BorderBrush = new SolidColorBrush(Colors.Red);
                    fHAssetTFeeMax.Foreground = new SolidColorBrush(Colors.Red);
                    ret = "STR_ERR_ASSET_TFEE_MAX_FIELD";
                }
            }
            catch (Exception)
            {
                bdAssetTFeeMax.BorderBrush = new SolidColorBrush(Colors.Red);
                fHAssetTFeeMax.Foreground = new SolidColorBrush(Colors.Red);
                ret = "STR_ERR_ASSET_TFEE_MAX_FIELD";
            }

            try
            {
                if (Double.Parse(TxbAssetTFeeMax.Text) < Double.Parse(TxbAssetTFeeMin.Text))
                {
                    bdAssetTFeeMin.BorderBrush = new SolidColorBrush(Colors.Red);
                    fHAssetTFeeMin.Foreground = new SolidColorBrush(Colors.Red);

                    bdAssetTFeeMax.BorderBrush = new SolidColorBrush(Colors.Red);
                    fHAssetTFeeMax.Foreground = new SolidColorBrush(Colors.Red);

                    ret = "STR_ERR_ASSET_TFEE_FIELD";
                }
            }
            catch
            {
                bdAssetTFeeMin.BorderBrush = new SolidColorBrush(Colors.Red);
                fHAssetTFeeMin.Foreground = new SolidColorBrush(Colors.Red);

                bdAssetTFeeMax.BorderBrush = new SolidColorBrush(Colors.Red);
                fHAssetTFeeMax.Foreground = new SolidColorBrush(Colors.Red);

                ret = "STR_ERR_ASSET_TFEE_FIELD";
            }
            

            if (Wallet.GetAddressVersion(TxbAssetFeeAddress.Text) != Wallet.AddressVersion)
            {
                bdAssetFeeAddress.BorderBrush = new SolidColorBrush(Colors.Red);
                fHAssetFeeAddress.Foreground = new SolidColorBrush(Colors.Red);
                ret = "STR_ERR_ASSET_FEE_ADDRESS_FIELD";
            }

            try
            {
                if (cmbAssetAdmin.SelectedItem == null)
                {
                    bdAssetAdmin.BorderBrush = new SolidColorBrush(Colors.Red);
                    fHAssetAdmin.Foreground = new SolidColorBrush(Colors.Red);
                    ret = "STR_ERR_ASSET_ADMIN_FIELD";
                }
            }
            catch (Exception)
            {
                bdAssetAdmin.BorderBrush = new SolidColorBrush(Colors.Red);
                fHAssetAdmin.Foreground = new SolidColorBrush(Colors.Red);
                ret = "STR_ERR_ASSET_ADMIN_FIELD";
            }
            
            if (cmbAssetOwner.SelectedItem == null)
            {
                bdAssetOwner.BorderBrush = new SolidColorBrush(Colors.Red);
                fHAssetOwner.Foreground = new SolidColorBrush(Colors.Red);
                ret = "STR_ERR_ASSET_OWNER_FIELD";  
            }

            if (cmbAssetIssuer.SelectedItem == null)
            {
                bdAssetIssuer.BorderBrush = new SolidColorBrush(Colors.Red);
                fHAssetIssuer.Foreground = new SolidColorBrush(Colors.Red);
                ret = "STR_ERR_ASSET_ISSUER_FIELD";
            }
            
            return ret;
        }

        private void BtnLaunch_Click(object sender, RoutedEventArgs e)
        {
            bool ret = false;
            using (AlertDialog dlg = new AlertDialog(Application.Current.MainWindow, StringTable.GetInstance().GetString("STR_ALERT_DLG_TITLE", iLang), StringTable.GetInstance().GetString("STR_ADD_ASSET_DEPLOY", iLang)))
            {
                ret = (bool)dlg.ShowDialog();
            }

            if (ret == false) return;
            try
            {
                Fixed8 fee = tx.Gas.Equals(Fixed8.Zero) ? net_fee : Fixed8.Zero;
                InvocationTransaction finalTx = Constant.CurrentWallet.MakeTransaction(new InvocationTransaction
                {
                    Version = tx.Version,
                    Script = tx.Script,
                    Gas = tx.Gas,
                    Attributes = tx.Attributes,
                    Inputs = tx.Inputs,
                    Outputs = tx.Outputs
                }, fee: fee);

                if (finalTx == null)
                {
                    StaticUtils.ShowMessageBox(StaticUtils.ErrorBrush, StringTable.GetInstance().GetString("STR_ERR_TX_INSUFFICIENTFUND", iLang));
                    return;
                }

                Global.Helper.SignAndShowInformation(finalTx);
                StaticUtils.ShowMessageBox(StaticUtils.GreenBrush, StringTable.GetInstance().GetString("STR_SUC_TX_SUCCESSED", iLang));
                TxbAssetID.Text = finalTx.Hash.ToString();
            }
            catch (Exception)
            {
                StaticUtils.ShowMessageBox(StaticUtils.ErrorBrush, StringTable.GetInstance().GetString("STR_ERR_INCOMPLETEDSIGNATUREMESSAGE", iLang));
            }
        }

        public InvocationTransaction GetTransaction()
        {
            AssetType asset_type = (AssetType)cmbAssetType.SelectedItem;
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
            }
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
                txbFee.Text = String.Format(StringTable.GetInstance().GetString("STR_SMART_CONTRACT_FEE", iLang), fee.ToString());
                BtnLaunch.IsEnabled = true;

                BtnLaunch.Style = FindResource("QurasAddAssetButtonStyle") as Style;
                StaticUtils.ShowMessageBox(StaticUtils.GreenBrush, StringTable.GetInstance().GetString("STR_SUC_TEST_TRANSACTION", iLang));
                return true;
            }
            else
            {
                BtnLaunch.IsEnabled = false;
                BtnLaunch.Style = FindResource("QurasDisableAddAssetButtonStyle") as Style;
                StaticUtils.ShowMessageBox(StaticUtils.ErrorBrush, StringTable.GetInstance().GetString("STR_ERR_TEST_TRANSACTION", iLang));
                return false;
            }
        }

        private void ChkNoLimit_Click(object sender, RoutedEventArgs e)
        {
            if (ChkNoLimit.IsChecked == true)
            {
                TxbAssetAmount.IsEnabled = false;
            }
            else
            {
                TxbAssetAmount.IsEnabled = true;
            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            InitInterface();
            RefreshLanguage();
        }

        private void BtnMakeTransaction_Click(object sender, RoutedEventArgs e)
        {
            InitBorderFields();
            string ret = CheckFields();

            if (ret != "STR_SUCCESS")
            {
                StaticUtils.ShowMessageBox(StaticUtils.ErrorBrush, StringTable.GetInstance().GetString("STR_ERR_AAP_FIELD", iLang));
                return;
            }

            if (isDuplicatedToken(TxbAssetName.Text))
            {
                StaticUtils.ShowMessageBox(StaticUtils.ErrorBrush, StringTable.GetInstance().GetString("STR_ERR_DUPLICATED_TOKEN", iLang));
                return;
            }

            MakeAndTestTransaction();
        }

        private void TxbIssueAssetID_TextChanged(object sender, TextChangedEventArgs e)
        {
            AssetState state = null;
            if (UInt256.TryParse(TxbIssueAssetID.Text, out UInt256 asset_id))
            {
                state = Blockchain.Default.GetAssetState(asset_id);
            }
            
            if (state == null)
            {
                state = null;
                TxbIssueAssetName.Text = "";
                TxbIssueAssetOwner.Text = "";
                TxbIssueAssetAdmin.Text = "";
                TxbIssueAssetAmount.Text = "";
                TxbIssueAssetIssued.Text = "";
                TxbIssueAssetPrecision.Text = "";
                StaticUtils.ShowMessageBox(StaticUtils.ErrorBrush, StringTable.GetInstance().GetString("STR_ERR_NOT_FOUND_ASSET", iLang));
                BtnMakeOutput.IsEnabled = false;
                BtnIssueLaunch.IsEnabled = false;

                BtnMakeOutput.Style = FindResource("QurasDisableAddAssetButtonStyle") as Style;
                BtnIssueLaunch.Style = FindResource("QurasDisableAddAssetButtonStyle") as Style;
                return;
            }


            TxbIssueAssetName.Text = state.GetName();
            TxbIssueAssetOwner.Text = state.Owner.ToString();
            TxbIssueAssetAdmin.Text = Wallet.ToAddress(state.Admin);
            TxbIssueAssetAmount.Text = state.Amount.ToString();
            TxbIssueAssetIssued.Text = state.Available.ToString();
            TxbIssueAssetPrecision.Text = state.Precision.ToString();

            BtnMakeOutput.IsEnabled = true;
            BtnMakeOutput.Style = FindResource("QurasAddAssetButtonStyle") as Style;
            assetState = state;
            StaticUtils.ShowMessageBox(StaticUtils.GreenBrush, StringTable.GetInstance().GetString("STR_FOUND_ASSET", iLang));
        }

        private void BtnMakeOutput_Click(object sender, RoutedEventArgs e)
        {
            using (TxOutPutDialog dlg = new TxOutPutDialog(Application.Current.MainWindow, assetState))
            {
                if ((bool)dlg.ShowDialog() == true)
                {
                    issueTx = dlg.GetTransaction();
                    TxbIssueTxResult.Text = issueTx.Hash.ToString();
                    txbIssueFee.Text = String.Format(StringTable.GetInstance().GetString("STR_SMART_CONTRACT_FEE", iLang), (issueTx.NetworkFee + issueTx.SystemFee).ToString());

                    BtnIssueLaunch.IsEnabled = true;
                    BtnIssueLaunch.Style = FindResource("QurasAddAssetButtonStyle") as Style;
                }
                else
                {
                    TxbIssueTxResult.Text = "";
                    txbIssueFee.Text = String.Format(StringTable.GetInstance().GetString("STR_SMART_CONTRACT_FEE", iLang), "0");

                    BtnIssueLaunch.IsEnabled = false;
                    BtnIssueLaunch.Style = FindResource("QurasDisableAddAssetButtonStyle") as Style;
                }
            }
        }

        private void BtnIssueLaunch_Click(object sender, RoutedEventArgs e)
        {
            bool ret = false;
            using (AlertDialog dlg = new AlertDialog(Application.Current.MainWindow, StringTable.GetInstance().GetString("STR_ALERT_DLG_TITLE", iLang), StringTable.GetInstance().GetString("STR_ISSUE_ASSET_QUESTION", iLang)))
            {
                ret = (bool)dlg.ShowDialog();
            }

            if (ret == false) return;

            try
            {
                Global.Helper.SignAndShowInformation(issueTx);
                StaticUtils.ShowMessageBox(StaticUtils.GreenBrush, StringTable.GetInstance().GetString("STR_SUC_TX_SUCCESSED", iLang));
            }
            catch (Exception)
            {
                StaticUtils.ShowMessageBox(StaticUtils.ErrorBrush, StringTable.GetInstance().GetString("STR_ERR_INCOMPLETEDSIGNATUREMESSAGE", iLang));
            }

            TxbIssueTxResult.Text = "";
            txbIssueFee.Text = String.Format(StringTable.GetInstance().GetString("STR_SMART_CONTRACT_FEE", iLang), "0");

            BtnIssueLaunch.IsEnabled = false;
            BtnIssueLaunch.Style = FindResource("QurasDisableAddAssetButtonStyle") as Style;
        }
    }
}
