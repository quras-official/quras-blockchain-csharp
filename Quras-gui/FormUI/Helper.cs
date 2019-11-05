using Pure.Core;
using Pure.SmartContract;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

using Quras_gui.Global;

namespace Quras_gui.FormUI
{
    internal static class Helper
    {
        private static Dictionary<Type, Form> tool_forms = new Dictionary<Type, Form>();

        private static void Helper_FormClosing(object sender, FormClosingEventArgs e)
        {
            tool_forms.Remove(sender.GetType());
        }

        internal static int GetListLength(this List<byte[]> value)
        {
            int length = 0;
            for (int i = 0; i < value.Count; i++)
            {
                length += value[i].Length;
            }

            return length;
        }

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

        public static void SignAndShowInformation(Transaction tx)
        {
            if (tx == null)
            {
                //MessageBox.Show(Strings.InsufficientFunds);
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
                    }
                    else
                    {
                        //InformationBox.Show(context.ToString(), Strings.IncompletedSignatureMessage, Strings.IncompletedSignatureTitle);
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
                }
                else
                {
                    //InformationBox.Show(context.ToString(), Strings.IncompletedSignatureMessage, Strings.IncompletedSignatureTitle);
                }
            }
        }

        public static string FormatNumber(string num)
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
                ret = ret + "." + split_num[1];
            }

            return ret;
        }
    }
}
