using System;
using System.IO;
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

using Pure;
using Pure.IO.Json;
using Pure.Core;
using Pure.VM;
using Pure.SmartContract;

using Quras_gui_wpf.Utils;
using Quras_gui_wpf.Global;

namespace Quras_gui_wpf.Pages
{
    /// <summary>
    /// Interaction logic for DeploySmartContractPage.xaml
    /// </summary>
    public partial class DeploySmartContractPage : UserControl
    {
        InvocationTransaction tx;
        private static readonly Fixed8 net_fee = Fixed8.FromDecimal(0.001m);
        private Fixed8 currentFee = Fixed8.Zero;

        private LANG iLang => Constant.GetLang();

        public DeploySmartContractPage()
        {
            InitializeComponent();
            RefreshLanguage();
        }

        public void RefreshLanguage()
        {
            TxbHeader.Text = StringTable.GetInstance().GetString("STR_SMARTCONTRACT_DEPLOY_BUTTON", iLang);
            TxbComment.Text = StringTable.GetInstance().GetString("STR_SMARTCONTRACT_DEPLOY_COMMENT", iLang);
            TxbSmartContractInfo.Text = StringTable.GetInstance().GetString("STR_DSCP_SMART_CONTRACT_INFO", iLang);
            TxbContractName.Tag = StringTable.GetInstance().GetString("STR_DSCP_SMART_CONTRACT_NAME", iLang);
            TxbContractVersion.Tag = StringTable.GetInstance().GetString("STR_DSCP_SMART_CONTRACT_VERSION", iLang);
            TxbContractAuthor.Tag = StringTable.GetInstance().GetString("STR_DSCP_SMART_CONTRACT_AUTHOR", iLang);
            TxbContractEmail.Tag = StringTable.GetInstance().GetString("STR_DSCP_SMART_CONTRACT_EMAIL", iLang);
            TxbContractDescription.Tag = StringTable.GetInstance().GetString("STR_DSCP_SMART_CONTRACT_DESCRIPTION", iLang);
            TxbParameters.Text = StringTable.GetInstance().GetString("STR_DSCP_PARAMETERS_TITLE", iLang);
            TxbContractParameters.Tag = StringTable.GetInstance().GetString("STR_DSCP_SMART_CONTRACT_PARAMETERS", iLang);
            TxbContractReturnTypes.Tag = StringTable.GetInstance().GetString("STR_DSCP_SMART_CONTRACT_RETURN_TYPE", iLang);
            TxbSmartContractCode.Text = StringTable.GetInstance().GetString("STR_DSCP_SMART_CONTRACT_CODE", iLang);
            TxbSmartContractCodes.Tag = StringTable.GetInstance().GetString("STR_DSCP_SMART_CONTRACT_CODES", iLang);
            btnLoad.Content = StringTable.GetInstance().GetString("STR_DSCP_BUTTON_LOAD", iLang);
            TxbInvocationTxScriptHash.Tag = StringTable.GetInstance().GetString("STR_DSCP_INVOCATION_TX_SCRIPT_HASH", iLang);
            TxbSmartContractScriptHashComment.Text = StringTable.GetInstance().GetString("STR_DSCP_SMART_CONTRACT_SCRIPT_HASH_COMMENT", iLang);
            TxbSmartContractScriptHash.Tag = StringTable.GetInstance().GetString("STR_DSCP_SMART_CONTRACT_SCRIPT_HASH", iLang);
            TxbSmartContractResultComment.Text = StringTable.GetInstance().GetString("STR_DSCP_SMART_CONTRACT_RESULT_COMMENT", iLang);
            TxbSmartContractResult.Tag = StringTable.GetInstance().GetString("STR_DSCP_SMART_CONTRACT_RESULT", iLang);
            TxbSmartContractFee.Text = String.Format(StringTable.GetInstance().GetString("STR_SMART_CONTRACT_FEE", iLang), currentFee.ToString());
            ChkNeedStorage.Content = StringTable.GetInstance().GetString("STR_DSCP_NEED_STORAGE", iLang);
            ChkNeedVM.Content = StringTable.GetInstance().GetString("STR_DSCP_NEED_VM", iLang);
            btnTest.Content = StringTable.GetInstance().GetString("STR_DSCP_BUTTON_TEST", iLang);
            btnDeploy.Content = StringTable.GetInstance().GetString("STR_DSCP_BUTTON_DEPLOY", iLang);
        }

        public InvocationTransaction GetTransaction()
        {
            byte[] script = TxbSmartContractCodes.Text.HexToBytes();
            byte[] parameter_list = TxbContractParameters.Text.HexToBytes();
            ContractParameterType return_type = TxbContractReturnTypes.Text.HexToBytes().Select(p => (ContractParameterType?)p).FirstOrDefault() ?? ContractParameterType.Void;
            bool need_storage = (bool)ChkNeedStorage.IsChecked;
            string name = TxbContractName.Text;
            string version = TxbContractVersion.Text;
            string author = TxbContractAuthor.Text;
            string email = TxbContractEmail.Text;
            string description = TxbContractDescription.Text;
            using (ScriptBuilder sb = new ScriptBuilder())
            {
                sb.EmitSysCall("Pure.Contract.Create", script, parameter_list, return_type, need_storage, name, version, author, email, description);
                return new InvocationTransaction
                {
                    Script = sb.ToArray()
                };
            }
        }

        public InvocationTransaction GetFinalTransaction()
        {
            Fixed8 fee = tx.Gas.Equals(Fixed8.Zero) ? net_fee : Fixed8.Zero;
            return Constant.CurrentWallet.MakeTransaction(new InvocationTransaction
            {
                Version = tx.Version,
                Script = tx.Script,
                Gas = tx.Gas,
                Attributes = tx.Attributes,
                Inputs = tx.Inputs,
                Outputs = tx.Outputs
            }, fee: fee);
        }

        private void btnLoad_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.FileName = "SmartContract"; // Default file name
            dlg.DefaultExt = "qsb";
            dlg.Filter = "Quras Smart Contract binary file (*.qsb)|*.qsb";

            // Show save file dialog box
            Nullable<bool> result = dlg.ShowDialog();

            // Process save file dialog box results
            if (result == true)
            {
                // Save document
                TxbSmartContractCodes.Text = File.ReadAllBytes(dlg.FileName).ToHexString();
                btnDeploy.IsEnabled = false;
            }
        }

        private void btnDeploy_Click(object sender, RoutedEventArgs e)
        {
            
            InvocationTransaction invTx = GetFinalTransaction();

            Quras_gui_wpf.Windows.Helper.SignAndShowInformation(invTx);
        }

        private void SCTextBoxs_TextChanged(object sender, TextChangedEventArgs e)
        {
            btnTest.IsEnabled = TxbInvocationTxScriptHash.Text.Length > 0;
            btnLoad.IsEnabled = TxbContractName.Text.Length > 0
                && TxbContractAuthor.Text.Length > 0
                && TxbContractEmail.Text.Length > 0
                && TxbContractVersion.Text.Length > 0
                && TxbContractDescription.Text.Length > 0;

            if (btnTest.IsEnabled)
            {
                btnTest.Style = FindResource("QurasAddAssetButtonStyle") as Style;
            }
            else
            {
                btnTest.Style = FindResource("QurasDisableAddAssetButtonStyle") as Style;
            }

            if (btnLoad.IsEnabled)
            {
                btnLoad.Style = FindResource("QurasAddAssetButtonStyle") as Style;
            }
            else
            {
                btnLoad.Style = FindResource("QurasDisableAddAssetButtonStyle") as Style;
            }

            try
            {
                TxbSmartContractScriptHash.Text = TxbSmartContractCodes.Text.HexToBytes().ToScriptHash().ToString();

                tx = GetTransaction();
                TxbInvocationTxScriptHash.Text = tx.Script.ToHexString();
            }
            catch (FormatException)
            {
                TxbSmartContractScriptHash.Text = "";
            }
        }

        private void btnTest_Click(object sender, RoutedEventArgs e)
        {
            if (tx == null) tx = new InvocationTransaction();
            tx.Version = 1;
            tx.Script = TxbInvocationTxScriptHash.Text.HexToBytes();
            if (tx.Attributes == null) tx.Attributes = new TransactionAttribute[0];
            if (tx.Inputs == null) tx.Inputs = new CoinReference[0];
            if (tx.Outputs == null) tx.Outputs = new TransactionOutput[0];
            if (tx.Scripts == null) tx.Scripts = new Witness[0];
            ApplicationEngine engine = ApplicationEngine.Run(tx.Script, tx);
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"VM State: {engine.State}");
            sb.AppendLine($"Gas Consumed: {engine.GasConsumed}");
            sb.AppendLine($"Evaluation Stack: {new JArray(engine.EvaluationStack.Select(p => p.ToParameter().ToJson()))}");
            TxbSmartContractResult.Text = sb.ToString();
            if (!engine.State.HasFlag(VMState.FAULT))
            {
                tx.Gas = engine.GasConsumed - Fixed8.FromDecimal(10);
                if (tx.Gas < Fixed8.Zero) tx.Gas = Fixed8.Zero;
                tx.Gas = tx.Gas.Ceiling();
                Fixed8 fee = tx.Gas.Equals(Fixed8.Zero) ? net_fee : tx.Gas;
                currentFee = fee;
                TxbSmartContractFee.Text = fee + " XQG";

                if (TxbContractName.Text.Length > 0
                && TxbContractAuthor.Text.Length > 0
                && TxbContractEmail.Text.Length > 0
                && TxbContractVersion.Text.Length > 0
                && TxbContractDescription.Text.Length > 0
                && TxbSmartContractCodes.Text.Length > 0)
                {
                    btnDeploy.IsEnabled = true;
                    btnDeploy.Style = FindResource("QurasAddAssetButtonStyle") as Style;
                }
            }
            else
            {
                btnDeploy.IsEnabled = false;
                btnDeploy.Style = FindResource("QurasDisableAddAssetButtonStyle") as Style;
            }
        }
    }
}
