using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Pure;
using Pure.Core;
using Pure.Wallets;
using Pure.Implementations.Wallets.EntityFramework;
using Quras_gui_SP.Global;

namespace Quras_gui_SP.Controls
{
    public partial class TxItem : UserControl
    {
        public TransactionInfo _info;

        public string[] MONTH = { "JAN", "FEB", "MAR", "APR", "MAY", "JUN", "JUL", "AUG", "SEP", "OCT", "NOV", "DEC" };

        public TxItem()
        {
            InitializeComponent();

            InitInstance();
            InitInterface();
        }

        public TxItem(TransactionInfo info)
        {
            InitializeComponent();
            _info = info;

            InitInstance();
            InitInterface();
        }

        void InitInterface()
        {
            if (_info != null)
            {
                lbl_date.Text = MONTH[_info.Time.Month] + " " + _info.Time.Day.ToString();
                lbl_date_year.Text = _info.Time.Year.ToString();

                if (_info.Transaction.Type == Pure.Core.TransactionType.AnonymousContractTransaction)
                {
                    DoProcessAnonymousTx();
                }
                else
                {
                    DoProcessTransparentTx();
                }
            }
        }

        void DoProcessAnonymousTx()
        {
            Fixed8 vPubOld = Fixed8.Zero;
            Fixed8 vPubNew = Fixed8.Zero;
            for (int i = 0; i < ((AnonymousContractTransaction)_info.Transaction).byJoinSplit.Count; i++)
            {
                vPubOld += ((AnonymousContractTransaction)_info.Transaction).vPub_Old(i);
                vPubNew += ((AnonymousContractTransaction)_info.Transaction).vPub_New(i);
            }

            if (_info.Transaction.Inputs.Length > 0 && vPubOld > Fixed8.Zero)
            { // t_addr to z_addr
                bool isFrom = false;

                for (int i = 0; i < _info.Transaction.Inputs.Length; i++)
                {
                    if (!Constant.CurrentWallet.ContainsAddress(_info.Transaction.References[_info.Transaction.Inputs[i]].ScriptHash))
                    {
                        isFrom = true;
                        break;
                    }
                }

                if (isFrom)
                {
                    lbl_cmt_from.Text = "From";
                    txb_from.Text = "";
                    for (int i = 0; i < _info.Transaction.Inputs.Length; i++)
                    {
                        txb_from.Text += Wallet.ToAddress(_info.Transaction.References[_info.Transaction.Inputs[i]].ScriptHash);
                    }

                    Fixed8 balance = new Fixed8(0);
                    for (int i = 0; i < _info.Transaction.Outputs.Length; i++)
                    {
                        if (Constant.CurrentWallet.ContainsAddress(_info.Transaction.Outputs[i].ScriptHash))
                        {
                            balance += _info.Transaction.Outputs[i].Value;
                        }
                    }

                    lbl_balance.Text = balance.ToString();

                    this.pan_color.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(19)))), ((int)(((byte)(166)))), ((int)(((byte)(204)))));
                }
                else
                {
                    lbl_cmt_from.Text = "To";

                    txb_from.Text = "Anonymous Address";
                    for (int i = 0; i < _info.Transaction.Outputs.Length; i++)
                    {
                        if (!Constant.CurrentWallet.ContainsAddress(_info.Transaction.Outputs[i].ScriptHash))
                        {
                            txb_from.Text += Wallet.ToAddress(_info.Transaction.Outputs[i].ScriptHash);
                        }
                    }

                    Fixed8 balance = new Fixed8(0);
                    for (int i = 0; i < _info.Transaction.Outputs.Length; i++)
                    {
                        if (!Constant.CurrentWallet.ContainsAddress(_info.Transaction.Outputs[i].ScriptHash))
                        {
                            balance += _info.Transaction.Outputs[i].Value;
                        }
                    }

                    balance += vPubOld;

                    lbl_balance.Text = balance.ToString();

                    this.pan_color.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
                }
            }
            else if (_info.Transaction.Inputs.Length == 0 && vPubNew > Fixed8.Zero)
            { // z_addr to t_addr

            }
            else
            { // z_addr to z_addr

            }
        }

        void DoProcessTransparentTx()
        {
            bool isFrom = false;

            for (int i = 0; i < _info.Transaction.Inputs.Length; i++)
            {
                if (!Constant.CurrentWallet.ContainsAddress(_info.Transaction.References[_info.Transaction.Inputs[i]].ScriptHash))
                {
                    isFrom = true;
                    break;
                }
            }

            if (isFrom)
            {
                lbl_cmt_from.Text = "From";
                txb_from.Text = "";
                for (int i = 0; i < _info.Transaction.Inputs.Length; i++)
                {
                    txb_from.Text += Wallet.ToAddress(_info.Transaction.References[_info.Transaction.Inputs[i]].ScriptHash);
                }

                Fixed8 balance = new Fixed8(0);
                for (int i = 0; i < _info.Transaction.Outputs.Length; i++)
                {
                    if (Constant.CurrentWallet.ContainsAddress(_info.Transaction.Outputs[i].ScriptHash))
                    {
                        balance += _info.Transaction.Outputs[i].Value;
                    }
                }

                lbl_balance.Text = balance.ToString();

                this.pan_color.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(19)))), ((int)(((byte)(166)))), ((int)(((byte)(204)))));
            }
            else
            {
                lbl_cmt_from.Text = "To";

                txb_from.Text = "";
                for (int i = 0; i < _info.Transaction.Outputs.Length; i++)
                {
                    if (!Constant.CurrentWallet.ContainsAddress(_info.Transaction.Outputs[i].ScriptHash))
                    {
                        txb_from.Text += Wallet.ToAddress(_info.Transaction.Outputs[i].ScriptHash);
                    }
                }

                Fixed8 balance = new Fixed8(0);
                for (int i = 0; i < _info.Transaction.Outputs.Length; i++)
                {
                    if (!Constant.CurrentWallet.ContainsAddress(_info.Transaction.Outputs[i].ScriptHash))
                    {
                        balance += _info.Transaction.Outputs[i].Value;
                    }
                }

                lbl_balance.Text = balance.ToString();

                this.pan_color.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            }
        }

        void InitInstance()
        {
            if (_info != null)
            {

            }
        }
    }
}
