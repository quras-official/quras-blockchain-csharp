using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Pure;
using Pure.Core;
using Pure.Implementations.Wallets.EntityFramework;

using MaterialSkin;
using MaterialSkin.Controls;

using Quras_gui.Global;

namespace Quras_gui.Dialogs
{
    public partial class TransactionInfoDialog : MaterialForm
    {
        private TransactionInfo _txInfo;
        private int iLang => Constant.GetLang();

        // Strings
        private string[] STR_ANONYMOUS_ADDR = { "Anonymous Address", "ニックネームアドレス" };
        private string[] STR_ERROR = { "Error", "" };

        private string[] STR_COMMENT = { "Bellow, you can see the transaction information.", "下記でトランザクションの情報を確認することができます。" };
        private string[] STR_BLOCKNUMBER = { "Block Number", "ブロックナンバー" };
        private string[] STR_TIME = { "Time", "タイム" };
        private string[] STR_TX_HASH = { "Tx Hash", "Txハッシュ" };
        private string[] STR_TX_TYPE = { "Tx Type", "Txタイプ" };
        private string[] STR_FROM = { "From", "送金元" };
        private string[] STR_TO = { "To", "送金先" };
        private string[] STR_AMOUNT = { "Amount", "金額" };
        private string[] STR_TX_VERSION = { "Tx Version", "Txヴァージョン" };
        private string[] STR_FEE = { "Fee", "手数料" };

        public TransactionInfoDialog()
        {
            InitializeComponent();

            // Create a material theme manager and add the form to manage (this)
            MaterialSkinManager materialSkinManager = MaterialSkinManager.Instance;
            materialSkinManager.AddFormToManage(this);
            materialSkinManager.Theme = MaterialSkinManager.Themes.LIGHT;

            // Configure color schema
            materialSkinManager.ColorScheme = new ColorScheme(
                Primary.Blue700, Primary.Blue700,
                Primary.Blue700, Accent.LightBlue200,
                TextShade.WHITE
            );

            _txInfo = null;

            InitInterface();
        }

        public TransactionInfoDialog(TransactionInfo txInfo )
        {
            InitializeComponent();

            // Create a material theme manager and add the form to manage (this)
            MaterialSkinManager materialSkinManager = MaterialSkinManager.Instance;
            materialSkinManager.AddFormToManage(this);
            materialSkinManager.Theme = MaterialSkinManager.Themes.LIGHT;

            // Configure color schema
            materialSkinManager.ColorScheme = new ColorScheme(
                Primary.Blue700, Primary.Blue700,
                Primary.Blue700, Accent.LightBlue200,
                TextShade.WHITE
            );

            _txInfo = txInfo;

            InitInterface();
        }

        public void InitInterface()
        {
            lbl_comment.Text = STR_COMMENT[iLang];
            lbl_blocknumber.Text = STR_BLOCKNUMBER[iLang];
            lbl_timestamp.Text = STR_TIME[iLang];
            lbl_txhash.Text = STR_TX_HASH[iLang];
            lbl_txtype.Text = STR_TX_TYPE[iLang];
            lbl_from.Text = STR_FROM[iLang];
            lbl_to.Text = STR_TO[iLang];
            lbl_amount.Text = STR_AMOUNT[iLang];
            lbl_txversion.Text = STR_TX_VERSION[iLang];
            lbl_fee.Text = STR_FEE[iLang];

            if (_txInfo != null)
            {
                txb_blocknumber.Text = _txInfo.Height.ToString();
                txb_time.Text = _txInfo.Time.ToLocalTime().ToString();
                txb_txhash.Text = _txInfo.Transaction.Hash.ToString();
                txb_txtype.Text = _txInfo.Transaction.Type.ToString();

                if (_txInfo.Transaction.Type == Pure.Core.TransactionType.AnonymousContractTransaction)
                {
                    Fixed8 amount = Fixed8.Zero;

                    if (_txInfo.Transaction.Inputs.Length > 0)
                    {
                        txb_from.Text = Pure.Wallets.Wallet.ToAddress(_txInfo.Transaction.References[_txInfo.Transaction.Inputs[0]].ScriptHash);
                    }
                    else
                    {
                        txb_from.Text = STR_ANONYMOUS_ADDR[iLang];
                    }

                    if (_txInfo.Transaction.Outputs.Length > 0)
                    {
                        if (_txInfo.Transaction.Inputs.Length > 0)
                        {
                            if (_txInfo.Transaction.Outputs.Where(p => p.ScriptHash != _txInfo.Transaction.References[_txInfo.Transaction.Inputs[0]].ScriptHash).ToList().Count > 0)
                            {
                                txb_to.Text = Pure.Wallets.Wallet.ToAddress(_txInfo.Transaction.Outputs.Where(p => p.ScriptHash != _txInfo.Transaction.References[_txInfo.Transaction.Inputs[0]].ScriptHash).ToList()[0].ScriptHash);
                            }
                            else
                            {
                                txb_to.Text = STR_ANONYMOUS_ADDR[iLang];
                            }
                            amount = _txInfo.Transaction.Outputs.Sum(p => p.Value);
                        }
                        else
                        {
                            txb_to.Text = Pure.Wallets.Wallet.ToAddress(_txInfo.Transaction.Outputs.ToList()[0].ScriptHash);
                            amount = _txInfo.Transaction.Outputs.Sum(p => p.Value);
                        }
                        
                    }
                    else
                    {
                        txb_to.Text = STR_ANONYMOUS_ADDR[iLang];
                    }

                    txb_amount.Text = amount.ToString();
                }
                else if (_txInfo.Transaction.Type == Pure.Core.TransactionType.ContractTransaction)
                {
                    try
                    {
                        txb_from.Text = Pure.Wallets.Wallet.ToAddress(_txInfo.Transaction.References[_txInfo.Transaction.Inputs[0]].ScriptHash);
                        txb_to.Text = Pure.Wallets.Wallet.ToAddress(_txInfo.Transaction.Outputs.Where(p => p.ScriptHash != _txInfo.Transaction.References[_txInfo.Transaction.Inputs[0]].ScriptHash).ToList()[0].ScriptHash);
                        txb_amount.Text = _txInfo.Transaction.Outputs.Where(p => p.ScriptHash != _txInfo.Transaction.References[_txInfo.Transaction.Inputs[0]].ScriptHash).Sum(p => p.Value).ToString();
                    }
                    catch
                    {
                        txb_from.Text = STR_ERROR[iLang];
                        txb_to.Text = STR_ERROR[iLang];
                        txb_amount.Text = STR_ERROR[iLang];
                    }
                }

                txb_txversion.Text = _txInfo.Transaction.Version.ToString();
                txb_fee.Text = (_txInfo.Transaction.NetworkFee + _txInfo.Transaction.SystemFee).ToString();
            }
        }
    }
}
