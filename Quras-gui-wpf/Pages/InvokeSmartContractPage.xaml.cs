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

using Pure;
using Pure.VM;
using Pure.Core;
using Pure.IO.Json;
using Pure.SmartContract;

using Quras_gui_wpf.Dialogs;
using Quras_gui_wpf.Global;
using Quras_gui_wpf.Utils;

namespace Quras_gui_wpf.Pages
{
    /// <summary>
    /// Interaction logic for InvokeSmartContractPage.xaml
    /// </summary>
    public partial class InvokeSmartContractPage : UserControl
    {
        private InvocationTransaction tx;
        private UInt160 script_hash;
        private ContractParameter[] parameters;
        private Fixed8 currentFee = Fixed8.Zero;

        private LANG iLang => Constant.GetLang();

        private static readonly Fixed8 net_fee = Fixed8.FromDecimal(0.001m);

        public InvokeSmartContractPage()
        {
            InitializeComponent();
            RefreshLanguage();
        }

        public void RefreshLanguage()
        {
            TxbHeader.Text = StringTable.GetInstance().GetString("STR_SMARTCONTRACT_INVOKE_BUTTON", iLang);
            TxbComment.Text = StringTable.GetInstance().GetString("STR_SMARTCONTRACT_INVOKE_COMMENT", iLang);

            TxbSmartContractInfo.Text = StringTable.GetInstance().GetString("STR_DSCP_SMART_CONTRACT_INFO", iLang);
            TxbContractScriptHash.Tag = StringTable.GetInstance().GetString("STR_DSCP_SMART_CONTRACT_SCRIPT_HASH", iLang);
            BtnSearch.Content = StringTable.GetInstance().GetString("STR_ISCP_BUTTON_SEARCH", iLang);
            TxbContractName.Tag = StringTable.GetInstance().GetString("STR_DSCP_SMART_CONTRACT_NAME", iLang);
            TxbContractVersion.Tag = StringTable.GetInstance().GetString("STR_DSCP_SMART_CONTRACT_VERSION", iLang);
            TxbContractAuthor.Tag = StringTable.GetInstance().GetString("STR_DSCP_SMART_CONTRACT_AUTHOR", iLang);
            TxbContractParameters.Tag = StringTable.GetInstance().GetString("STR_DSCP_SMART_CONTRACT_PARAMETERS", iLang);
            BtnAdd.Content = StringTable.GetInstance().GetString("STR_ISCP_BUTTON_ADD", iLang);
            TxbScriptHashTitle.Text = StringTable.GetInstance().GetString("STR_ISCP_SCRIPT", iLang);
            TxbScript.Tag = StringTable.GetInstance().GetString("STR_ISCP_SCRIPT", iLang);
            TxbResults.Text = StringTable.GetInstance().GetString("STR_ISCP_RESULTS", iLang);
            TxbContractResults.Tag = StringTable.GetInstance().GetString("STR_ISCP_CONTRACT_RESULT", iLang);
            TxbFee.Text = String.Format(StringTable.GetInstance().GetString("STR_SMART_CONTRACT_FEE", iLang), currentFee.ToString());
            btnTest.Content = StringTable.GetInstance().GetString("STR_DSCP_BUTTON_TEST", iLang);
            btnRun.Content = StringTable.GetInstance().GetString("STR_ISCP_BUTTON_RUN", iLang);
        }

        private void BtnSearch_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                script_hash = UInt160.Parse(TxbContractScriptHash.Text);
            }
            catch
            {
                StaticUtils.ShowMessageBox(StaticUtils.ErrorBrush, StringTable.GetInstance().GetString("STR_ERR_SM_SCRIPT_HASH_FORMAT", iLang));
                return;
            }
            ContractState contract = Blockchain.Default.GetContract(script_hash);
            if (contract == null)
            {
                StaticUtils.ShowMessageBox(StaticUtils.ErrorBrush, StringTable.GetInstance().GetString("STR_INV_NOT_FOUND_CONTRACT", iLang));
                return;
            }
            parameters = contract.ParameterList.Select(p => new ContractParameter(p)).ToArray();
            TxbContractName.Text = contract.Name;
            TxbContractVersion.Text = contract.CodeVersion;
            TxbContractAuthor.Text = contract.Author;
            TxbContractParameters.Text = string.Join(", ", contract.ParameterList);
            BtnAdd.IsEnabled = parameters.Length > 0;

            if (BtnAdd.IsEnabled)
            {
                BtnAdd.Style = FindResource("QurasAddAssetButtonStyle") as Style;
            }
            else
            {
                BtnAdd.Style = FindResource("QurasDisableAddAssetButtonStyle") as Style;
            }

            UpdateScript();
        }

        private void UpdateScript()
        {
            if (parameters.Any(p => p.Value == null)) return;
            using (ScriptBuilder sb = new ScriptBuilder())
            {
                sb.EmitAppCall(script_hash, parameters);
                TxbScript.Text = sb.ToArray().ToHexString();
            }
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            bool ret = false;
            using (SmartContractParamsDialog dlg = new SmartContractParamsDialog(Application.Current.MainWindow, parameters))
            {
                ret = (bool)dlg.ShowDialog();

                if (ret == true)
                {
                    UpdateScript();
                }
            }
        }

        private void TxbScript_TextChanged(object sender, TextChangedEventArgs e)
        {
            btnTest.IsEnabled = TxbScript.Text.Length > 0;

            if (btnTest.IsEnabled)
            {
                btnTest.Style = FindResource("QurasAddAssetButtonStyle") as Style;
            }
            else
            {
                btnTest.Style = FindResource("QurasDisableAddAssetButtonStyle") as Style;
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

        private void btnTest_Click(object sender, RoutedEventArgs e)
        {
            if (tx == null) tx = new InvocationTransaction();
            tx.Version = 1;
            tx.Script = TxbScript.Text.HexToBytes();
            if (tx.Attributes == null) tx.Attributes = new TransactionAttribute[0];
            if (tx.Inputs == null) tx.Inputs = new CoinReference[0];
            if (tx.Outputs == null) tx.Outputs = new TransactionOutput[0];
            if (tx.Scripts == null) tx.Scripts = new Witness[0];
            ApplicationEngine engine = ApplicationEngine.Run(tx.Script, tx);
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"VM State: {engine.State}");
            sb.AppendLine($"Gas Consumed: {engine.GasConsumed}");
            sb.AppendLine($"Evaluation Stack: {new JArray(engine.EvaluationStack.Select(p => p.ToParameter().ToJson()))}");
            TxbContractResults.Text = sb.ToString();
            if (!engine.State.HasFlag(VMState.FAULT))
            {
                tx.Gas = engine.GasConsumed - Fixed8.FromDecimal(10);
                if (tx.Gas < Fixed8.Zero) tx.Gas = Fixed8.Zero;
                tx.Gas = tx.Gas.Ceiling();
                Fixed8 fee = tx.Gas.Equals(Fixed8.Zero) ? net_fee : tx.Gas;
                currentFee = fee;
                TxbFee.Text = fee + " XQG";
                btnRun.IsEnabled = true;
                btnRun.Style = FindResource("QurasAddAssetButtonStyle") as Style;
            }
            else
            {
                StaticUtils.ShowMessageBox(StaticUtils.ErrorBrush, StringTable.GetInstance().GetString("STR_ERR_INVOKE_TEST", iLang));
                btnRun.IsEnabled = false;
                btnRun.Style = FindResource("QurasDisableAddAssetButtonStyle") as Style;
            }
        }

        private void btnRun_Click(object sender, RoutedEventArgs e)
        {
            InvocationTransaction invTx = GetFinalTransaction();

            Quras_gui_wpf.Windows.Helper.SignAndShowInformation(invTx);
        }
    }
}
