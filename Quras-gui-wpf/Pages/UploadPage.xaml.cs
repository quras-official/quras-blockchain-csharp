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
    /// Interaction logic for UploadPage.xaml
    /// </summary>
    public partial class UploadPage : UserControl
    {
        private LANG iLang => Constant.GetLang();
        private InvocationTransaction tx;
        private IssueTransaction issueTx;
        private static readonly Fixed8 net_fee = Fixed8.FromDecimal(0.001m);
        private Fixed8 currentFee = Fixed8.Zero;
        private AssetState assetState;

        public UploadPage()
        {
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
            
        }

        public InvocationTransaction GetTransaction()
        {
            return null;
        }

        public bool MakeAndTestTransaction()
        {
            tx = GetTransaction();


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
            if (!engine.State.HasFlag(VMState.FAULT))
            {
                tx.Gas = engine.GasConsumed - Fixed8.FromDecimal(10);
                if (tx.Gas < Fixed8.Zero) tx.Gas = Fixed8.Zero;
                tx.Gas = tx.Gas.Ceiling();
                Fixed8 fee = tx.Gas.Equals(Fixed8.Zero) ? net_fee : tx.Gas;
                currentFee = fee;
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


        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            InitInterface();
            RefreshLanguage();
        }


        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
