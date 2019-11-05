using System.Windows.Forms;

using Pure;
using Pure.Core;
using Pure.Wallets;
using Pure.Implementations.Wallets.EntityFramework;
using Quras_gui_SP.Global;

namespace Quras_gui_SP.Controls
{
    public partial class SideTxItem : UserControl
    {
        public TransactionInfo _info;

        public string[] MONTH = { "JAN", "FEB", "MAR", "APR", "MAY", "JUN", "JUL", "AUG", "SEP", "OCT", "NOV", "DEC" };

        public SideTxItem()
        {
            InitializeComponent();

            InitInstance();
            InitInterface();
        }

        public SideTxItem(TransactionInfo info)
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
                lbl_month.Text = MONTH[_info.Time.Month];
                lbl_day.Text = _info.Time.Day.ToString();

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
                    Fixed8 balance = new Fixed8(0);
                    for (int i = 0; i < _info.Transaction.Outputs.Length; i++)
                    {
                        if (Constant.CurrentWallet.ContainsAddress(_info.Transaction.Outputs[i].ScriptHash))
                        {
                            balance += _info.Transaction.Outputs[i].Value;
                        }
                    }

                    lbl_balance.Text = balance.ToString();

                    this.lbl_balance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(19)))), ((int)(((byte)(166)))), ((int)(((byte)(204)))));
                }
                else
                {
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

                    this.lbl_balance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
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
                Fixed8 balance = new Fixed8(0);
                for (int i = 0; i < _info.Transaction.Outputs.Length; i++)
                {
                    if (Constant.CurrentWallet.ContainsAddress(_info.Transaction.Outputs[i].ScriptHash))
                    {
                        balance += _info.Transaction.Outputs[i].Value;
                    }
                }

                lbl_balance.Text = balance.ToString();

                this.lbl_balance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(19)))), ((int)(((byte)(166)))), ((int)(((byte)(204)))));
            }
            else
            {
                Fixed8 balance = new Fixed8(0);
                for (int i = 0; i < _info.Transaction.Outputs.Length; i++)
                {
                    if (!Constant.CurrentWallet.ContainsAddress(_info.Transaction.Outputs[i].ScriptHash))
                    {
                        balance += _info.Transaction.Outputs[i].Value;
                    }
                }

                lbl_balance.Text = balance.ToString();

                this.lbl_balance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
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
