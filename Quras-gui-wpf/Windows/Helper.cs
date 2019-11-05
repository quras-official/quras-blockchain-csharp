using Pure.Core;
using Pure.SmartContract;
using System;
using System.Collections.Generic;

using Quras_gui_wpf.Global;
using Quras_gui_wpf.Utils;

namespace Quras_gui_wpf.Windows
{
    internal static class Helper
    {
        /*
        private static Dictionary<Type, Form> tool_forms = new Dictionary<Type, Form>();

        private static void Helper_FormClosing(object sender, FormClosingEventArgs e)
        {
            tool_forms.Remove(sender.GetType());
        }
        */
        internal static int GetListLength(this List<byte[]> value)
        {
            int length = 0;
            for (int i = 0; i < value.Count; i++)
            {
                length += value[i].Length;
            }

            return length;
        }

        /*
        public static void Show<T>() where T : Form, new()
        {
            Type t = typeof(T);
            if (!tool_forms.ContainsKey(t))
            {
                tool_forms.Add(t, new T());
                tool_forms[t].FormClosing += Helper_FormClosing;
            }
            tool_forms[t].Show();
            tool_forms[t].Activate();
        }
        */

        public static void SignAndShowInformation(Transaction tx)
        {
            if (tx == null)
            {
                //MessageBox.Show(Strings.InsufficientFunds);
                StaticUtils.ShowMessageBox(StaticUtils.ErrorBrush, StringTable.GetInstance().GetString("STR_ERR_TX_INSUFFICIENTFUND", Constant.GetLang()));
                return;
            }
            ContractParametersContext context;
            try
            {
                context = new ContractParametersContext(tx);
            }
            catch (InvalidOperationException)
            {
                //MessageBox.Show(Strings.UnsynchronizedBlock);
                StaticUtils.ShowMessageBox(StaticUtils.ErrorBrush, StringTable.GetInstance().GetString("STR_ERR_TX_UNSYNCHRONIZEDBLOCK", Constant.GetLang()));
                return;
            }

            if (tx.Type == TransactionType.AnonymousContractTransaction)
            {
                if (tx.Inputs.Length > 0)
                {
                    Constant.CurrentWallet.Sign(context);

                    if (context.Completed)
                    {
                        context.Verifiable.Scripts = context.GetScripts();
                        Constant.CurrentWallet.SaveTransaction(tx);
                        Constant.LocalNode.Relay(tx);
                        //InformationBox.Show(tx.Hash.ToString(), Strings.SendTxSucceedMessage, Strings.SendTxSucceedTitle);
                        StaticUtils.ShowMessageBox(StaticUtils.GreenBrush, StringTable.GetInstance().GetString("STR_SUC_TX_SUCCESSED", Constant.GetLang()));
                    }
                    else
                    {
                        //InformationBox.Show(context.ToString(), Strings.IncompletedSignatureMessage, Strings.IncompletedSignatureTitle);
                        StaticUtils.ShowMessageBox(StaticUtils.ErrorBrush, StringTable.GetInstance().GetString("STR_ERR_INCOMPLETEDSIGNATUREMESSAGE", Constant.GetLang()));
                    }
                }
                else
                {
                    Constant.CurrentWallet.SaveTransaction(tx);
                    Constant.LocalNode.Relay(tx);
                }
            }
            else
            {
                Constant.CurrentWallet.Sign(context);

                if (context.Completed)
                {
                    context.Verifiable.Scripts = context.GetScripts();
                    Constant.CurrentWallet.SaveTransaction(tx);
                    Constant.LocalNode.Relay(tx);
                    //InformationBox.Show(tx.Hash.ToString(), Strings.SendTxSucceedMessage, Strings.SendTxSucceedTitle);
                    StaticUtils.ShowMessageBox(StaticUtils.GreenBrush, StringTable.GetInstance().GetString("STR_SUC_TX_SUCCESSED", Constant.GetLang()));
                }
                else
                {
                    //InformationBox.Show(context.ToString(), Strings.IncompletedSignatureMessage, Strings.IncompletedSignatureTitle);
                    StaticUtils.ShowMessageBox(StaticUtils.ErrorBrush, StringTable.GetInstance().GetString("STR_ERR_INCOMPLETEDSIGNATUREMESSAGE", Constant.GetLang()));
                }
            }
        }

        public static string FormatNumber(string num, int decimalCount = 2)
        {
            char[] delimiterChars = { '.' };
            string ret = "";

            int comma_count = 0;

            string[] split_num = num.Split(delimiterChars);

            num = split_num[0];

            comma_count = num.Length / 3;
            if (num.Length % 3 == 0) comma_count--;

            int ret_length = num.Length + comma_count;

            if (comma_count > 0)
            {
                int first_block_len = num.Length - 3 * comma_count;

                ret = num.Substring(0, first_block_len);
                ret += ",";

                comma_count--;
                while (comma_count > 0)
                {
                    ret += num.Substring(num.Length - (comma_count + 1) * 3, 3);
                    ret += ",";
                    comma_count--;
                }

                ret += num.Substring(num.Length - 3, 3);
            }
            else
            {
                ret = num;
            }

            if (split_num.Length > 1)
            {
                if (decimalCount > split_num[1].Length) decimalCount = split_num[1].Length;

                ret = ret + "." + split_num[1].Substring(0, decimalCount);
            }

            return ret;
        }
    }
}
