using Pure.Core;
using Pure.SmartContract;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

using Quras_gui_SP.Global;
using Quras_gui_SP.Dialogs;
using Quras_gui_SP.Properties;

namespace Quras_gui_SP.UI
{
    internal static class Helper
    {
        private static Dictionary<Type, Form> tool_forms = new Dictionary<Type, Form>();

        private static void Helper_FormClosing(object sender, FormClosingEventArgs e)
        {
            tool_forms.Remove(sender.GetType());
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
                using (WarningDlg dialog = new WarningDlg("Error", Resources.InsufficientFunds))
                {
                    if (dialog.ShowDialog() != DialogResult.OK)
                    {
                    }
                }
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
                using (WarningDlg dialog = new WarningDlg("Error", Resources.UnsynchronizedBlock))
                {
                    if (dialog.ShowDialog() != DialogResult.OK)
                    {
                    }
                }
                return;
            }
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
}
