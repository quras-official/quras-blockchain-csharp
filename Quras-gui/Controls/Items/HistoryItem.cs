using System;
using System.Windows.Forms;

using Pure;
using Pure.Core;
using Pure.Wallets;
using Pure.Implementations.Wallets.EntityFramework;

using Quras_gui.Global;

namespace Quras_gui.Controls.Items
{ 
    public partial class HistoryItem : UserControl
    {
        public TransactionInfo _info;

        public string[] MONTH_EN = { "JAN", "FEB", "MAR", "APR", "MAY", "JUN", "JUL", "AUG", "SEP", "OCT", "NOV", "DEC" };
        public string[] MONTH_JP = { "一月", "二月", "三月", "四月", "五月", "六月", "七月", "八月", "九月", "十月", "十一月", "十二月" };
        private int iLang => Constant.GetLang();

        public static string[] STR_FROM = { "From", "入金" };
        public static string[] STR_TO = { "To", "出金" };
        public static string[] STR_ANONYMOUS = { "Anonymous Address", "ニックネームアドレス" };
        public static string[] STR_ANONY_ANONY = { "Anonymous To Anonymous", "匿名の匿名" };
        public static string[] STR_COMPLETE = { "Complete", "完成" };
        public static string[] STR_UNCONFIRM = { "Unconfirmed", "未確認" };

        public event EventHandler MouseHover_Event;
        public event MouseEventHandler MouseDown_Event;

        public HistoryItem()
        {
            InitializeComponent();

            InitInstance();
            InitInterface();
        }

        public HistoryItem(TransactionInfo info)
        {
            InitializeComponent();
            _info = info;

            InitInstance();
            InitInterface();

            RefreshLang();
        }

        public void RefreshLang()
        {
            if (_info != null)
            {
                switch (_info.Transaction.Type)
                {
                    case TransactionType.AnonymousContractTransaction:
                    case TransactionType.ContractTransaction:
                        if (_info.Height == null || _info.Height == 0)
                        {
                            lbl_status.Text = STR_UNCONFIRM[iLang];
                        }
                        else
                        {
                            lbl_status.Text = STR_COMPLETE[iLang];
                        }

                        if (iLang == 0)
                        {
                            lbl_month.Text = MONTH_EN[_info.Time.Month - 1] + " " + _info.Time.Day.ToString();

                            if (lbl_arrow.Text == STR_FROM[1] || lbl_arrow.Text == STR_FROM[0])
                            {
                                lbl_arrow.Text = STR_FROM[0];
                            }
                            else if (lbl_arrow.Text == STR_TO[1] || lbl_arrow.Text == STR_TO[0])
                            {
                                lbl_arrow.Text = STR_TO[0];
                            }
                            else
                            {
                                lbl_arrow.Text = STR_ANONY_ANONY[0];
                            }

                            if (txb_address.Text == STR_ANONYMOUS[1] || txb_address.Text == STR_ANONYMOUS[0])
                                txb_address.Text = STR_ANONYMOUS[0];
                        }
                        else if (iLang == 1)
                        {
                            lbl_month.Text = MONTH_JP[_info.Time.Month - 1] + " " + _info.Time.Day.ToString();

                            if (lbl_arrow.Text == STR_FROM[0] || lbl_arrow.Text == STR_FROM[1])
                            {
                                lbl_arrow.Text = STR_FROM[1];
                            }
                            else if (lbl_arrow.Text == STR_TO[1] || lbl_arrow.Text == STR_TO[0])
                            {
                                lbl_arrow.Text = STR_TO[1];
                            }
                            else
                            {
                                lbl_arrow.Text = STR_ANONY_ANONY[1];
                            }

                            if (txb_address.Text == STR_ANONYMOUS[0] || txb_address.Text == STR_ANONYMOUS[1])
                                txb_address.Text = STR_ANONYMOUS[1];
                        }
                        break;
                    case TransactionType.ClaimTransaction:
                        this.pan_color.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(128)))));
                        this.lbl_arrow.Text = "Claim Transaction";
                        this.txb_address.Text = _info.Transaction.Hash.ToString();
                        break;
                    default:
                        break;
                }
                
            }
        }

        void InitInterface()
        {
            if (_info != null)
            {
                switch (_info.Transaction.Type)
                {
                    case TransactionType.AnonymousContractTransaction:
                    case TransactionType.ContractTransaction:
                        {
                            lbl_year.Text = _info.Time.Year.ToString();

                            if (_info.Height == null || _info.Height == 0)
                            {
                                lbl_status.Text = STR_UNCONFIRM[iLang];
                            }
                            else
                            {
                                lbl_status.Text = STR_COMPLETE[iLang];
                            }

                            if (_info.Transaction.Type == Pure.Core.TransactionType.AnonymousContractTransaction)
                            {
                                DoProcessAnonymousTx();
                            }
                            else
                            {
                                DoProcessTransparentTx();
                            }
                            break;
                        }

                    case TransactionType.ClaimTransaction:
                        this.pan_color.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(128)))));
                        this.lbl_arrow.Text = "Claim Transaction";
                        this.txb_address.Text = _info.Transaction.Hash.ToString();
                        this.lbl_balance.Hide();
                        break;
                    default:
                        break;
                }
                
            }
        }

        void InitInstance()
        {
            if (_info != null)
            {

            }
        }

        void DoProcessAnonymousTx()
        {
            Fixed8 vPubOld = Fixed8.Zero;
            Fixed8 vPubNew = Fixed8.Zero;
            for (int i = 0; i < ((AnonymousContractTransaction)_info.Transaction).byJoinSplit.Count; i ++)
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
                    lbl_arrow.Text = STR_FROM[iLang];
                    txb_address.Text = "";
                    for (int i = 0; i < _info.Transaction.Inputs.Length; i++)
                    {
                        txb_address.Text += Wallet.ToAddress(_info.Transaction.References[_info.Transaction.Inputs[i]].ScriptHash);
                    }

                    Fixed8 balance = new Fixed8(0);
                    for (int i = 0; i < _info.Transaction.Outputs.Length; i++)
                    {
                        if (Constant.CurrentWallet.ContainsAddress(_info.Transaction.Outputs[i].ScriptHash))
                        {
                            balance += _info.Transaction.Outputs[i].Value;
                        }
                    }

                    balance += vPubOld;

                    lbl_balance.Text = balance.ToString();

                    this.pan_color.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(19)))), ((int)(((byte)(166)))), ((int)(((byte)(204)))));
                }
                else
                {
                    lbl_arrow.Text = STR_TO[iLang];

                    txb_address.Text = "Anonymous Address";
                    for (int i = 0; i < _info.Transaction.Outputs.Length; i++)
                    {
                        if (!Constant.CurrentWallet.ContainsAddress(_info.Transaction.Outputs[i].ScriptHash))
                        {
                            txb_address.Text += Wallet.ToAddress(_info.Transaction.Outputs[i].ScriptHash);
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
                txb_address.Text = "Anonymous Address";
                foreach (KeyPair key in Constant.CurrentWallet.GetKeys())
                {
                    if (key.nVersion == KeyType.Anonymous)
                    {
                        lbl_arrow.Text = STR_TO[iLang];
                        this.pan_color.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
                    }
                    else
                    {
                        lbl_arrow.Text = STR_FROM[iLang];
                        this.pan_color.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(19)))), ((int)(((byte)(166)))), ((int)(((byte)(204)))));
                    }
                }

                lbl_balance.Text = vPubNew.ToString();
            }
            else
            { // z_addr to z_addr
                txb_address.Text = "Anonymous Address";
                lbl_arrow.Text = "Anonymous to Anonymous";
                this.pan_color.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(166)))), ((int)(((byte)(204)))));

                lbl_balance.Text = "XXX";
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
                lbl_arrow.Text = STR_FROM[iLang];
                txb_address.Text = "";
                for (int i = 0; i < _info.Transaction.Inputs.Length; i++)
                {
                    txb_address.Text += Wallet.ToAddress(_info.Transaction.References[_info.Transaction.Inputs[i]].ScriptHash);
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
                lbl_arrow.Text = STR_TO[iLang];

                txb_address.Text = "";
                for (int i = 0; i < _info.Transaction.Outputs.Length; i++)
                {
                    if (!Constant.CurrentWallet.ContainsAddress(_info.Transaction.Outputs[i].ScriptHash))
                    {
                        txb_address.Text += Wallet.ToAddress(_info.Transaction.Outputs[i].ScriptHash);
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

        private void HistoryItem_MouseHover(object sender, System.EventArgs e)
        {
            MouseHover_Event?.Invoke(sender, e);
        }

        private void HistoryItem_MouseDown(object sender, MouseEventArgs e)
        {
            MouseDown_Event?.Invoke(_info, e);
        }
    }
}
