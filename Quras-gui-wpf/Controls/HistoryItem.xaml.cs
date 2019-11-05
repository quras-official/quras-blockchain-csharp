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
using Pure.Core;
using Pure.Wallets;
using Pure.Wallets.StealthKey;
using Pure.Cryptography;
using Pure.Implementations.Wallets.EntityFramework;

using Quras_gui_wpf.Global;
using Quras_gui_wpf.Utils;

namespace Quras_gui_wpf.Controls
{
    public enum TxStatus
    {
        completed,
        pending,
        unknown
    }

    public enum TxArrow
    {
        From,
        To,
        Anonymous
    }
    /// <summary>
    /// Interaction logic for HistoryItem.xaml
    /// </summary>
    public partial class HistoryItem : UserControl
    {
        public TransactionInfo _info;

        private LANG iLang => Constant.GetLang();

        private TxStatus txStatus;
        private TxArrow txArrow;

        public string[] MONTH_EN = { "JAN", "FEB", "MAR", "APR", "MAY", "JUN", "JUL", "AUG", "SEP", "OCT", "NOV", "DEC" };
        public string[] MONTH_JP = { "一月", "二月", "三月", "四月", "五月", "六月", "七月", "八月", "九月", "十月", "十一月", "十二月" };

        List<byte[]> privKeys;
        IEnumerable<KeyPairBase> keys;
        IEnumerable<UInt160> addresses;

        public HistoryItem(IEnumerable<KeyPairBase> keys, IEnumerable<UInt160> addresses)
        {
            InitializeComponent();
            _info = null;
        }

        public HistoryItem(TransactionInfo info, IEnumerable<KeyPairBase> keys, IEnumerable<UInt160> addresses, List<byte[]> privKeys)
        {
            this.keys = keys;
            this.addresses = addresses;
            this.privKeys = privKeys;

            _info = info;
            InitializeComponent();

            InitInstance();
            InitInterface();

            RefreshLanguage();
        }

        public void RefreshLanguage()
        {
            if (_info != null)
            {
                switch (_info.Transaction.Type)
                {
                    case TransactionType.AnonymousContractTransaction:
                    case TransactionType.ContractTransaction:
                        {
                            TxbStatusPending.Text = StringTable.GetInstance().GetString("STR_HI_PENDING", iLang);
                            TxbStatusCompleted.Text = StringTable.GetInstance().GetString("STR_HI_COMPLETED", iLang);
                            TxbStatusUnknown.Text = StringTable.GetInstance().GetString("STR_HI_UNKNOWN", iLang);

                            TxbFrom.Text = StringTable.GetInstance().GetString("STR_HI_FROM", iLang);
                            TxbTo.Text = StringTable.GetInstance().GetString("STR_HI_TO", iLang);
                            TxbAnonymous.Text = StringTable.GetInstance().GetString("STR_HI_ANONYMOUS", iLang);

                            if (iLang == LANG.JP)
                            {
                                TxbMonthDay.Text = MONTH_JP[_info.Time.Month - 1] + " " + _info.Time.Day.ToString();
                            }
                            else
                            {
                                TxbMonthDay.Text = MONTH_EN[_info.Time.Month - 1] + " " + _info.Time.Day.ToString();
                            }
                            break;
                        }
                    case TransactionType.RingConfidentialTransaction:
                        {
                            TxbStatusPending.Text = StringTable.GetInstance().GetString("STR_HI_PENDING", iLang);
                            TxbStatusCompleted.Text = StringTable.GetInstance().GetString("STR_HI_COMPLETED", iLang);
                            TxbStatusUnknown.Text = StringTable.GetInstance().GetString("STR_HI_UNKNOWN", iLang);

                            TxbFrom.Text = StringTable.GetInstance().GetString("STR_HI_FROM", iLang);
                            TxbTo.Text = StringTable.GetInstance().GetString("STR_HI_TO", iLang);
                            TxbAnonymous.Text = StringTable.GetInstance().GetString("STR_HI_RINGCT", iLang);

                            if (iLang == LANG.JP)
                            {
                                TxbMonthDay.Text = MONTH_JP[_info.Time.Month - 1] + " " + _info.Time.Day.ToString();
                            }
                            else
                            {
                                TxbMonthDay.Text = MONTH_EN[_info.Time.Month - 1] + " " + _info.Time.Day.ToString();
                            }
                            break;
                        }
                    case TransactionType.ClaimTransaction:
                        {
                            this.gridColorPan.Background = new SolidColorBrush(Color.FromArgb(0xFF, 217, 0, 234));
                            this.TxbFrom.Text = StringTable.GetInstance().GetString("STR_TX_TYPE_CLAIM", iLang);
                            this.TxbAddress.Text = _info.Transaction.Hash.ToString();
                            this.TxbAmount.Visibility = Visibility.Hidden;

                            Dictionary<UInt256, Fixed8> fee = new Dictionary<UInt256, Fixed8>();
                            fee[Blockchain.UtilityToken.Hash] = _info.Transaction.SystemFee;

                            this.TxbFee.Text = MakeStrings(fee);

                            TxbYear.Text = _info.Time.Year.ToString();

                            if (iLang == LANG.JP)
                            {
                                TxbMonthDay.Text = MONTH_JP[_info.Time.Month - 1] + " " + _info.Time.Day.ToString();
                            }
                            else
                            {
                                TxbMonthDay.Text = MONTH_EN[_info.Time.Month - 1] + " " + _info.Time.Day.ToString();
                            }
                            break;
                        }
                    case TransactionType.InvocationTransaction:
                        {
                            this.gridColorPan.Background = new SolidColorBrush(Color.FromArgb(0xFF, 185, 0, 181));
                            this.TxbFrom.Text = StringTable.GetInstance().GetString("STR_TX_TYPE_INVOCATION", iLang);
                            this.TxbAmount.Visibility = Visibility.Hidden;
                            this.TxbAddress.Text = _info.Transaction.Hash.ToString();

                            Dictionary<UInt256, Fixed8> fee = new Dictionary<UInt256, Fixed8>();
                            fee[Blockchain.UtilityToken.Hash] = _info.Transaction.SystemFee;

                            this.TxbFee.Text = MakeStrings(fee);

                            TxbYear.Text = _info.Time.Year.ToString();

                            if (iLang == LANG.JP)
                            {
                                TxbMonthDay.Text = MONTH_JP[_info.Time.Month - 1] + " " + _info.Time.Day.ToString();
                            }
                            else
                            {
                                TxbMonthDay.Text = MONTH_EN[_info.Time.Month - 1] + " " + _info.Time.Day.ToString();
                            }
                            break;
                        }
                    case TransactionType.IssueTransaction:
                        {
                            this.gridColorPan.Background = new SolidColorBrush(Color.FromArgb(0xFF, 0, 162, 232));
                            this.TxbFrom.Text = StringTable.GetInstance().GetString("STR_TX_TYPE_ISSUE", iLang);
                            this.TxbAddress.Text = _info.Transaction.Hash.ToString();

                            Dictionary<UInt256, Fixed8> fee = new Dictionary<UInt256, Fixed8>();
                            fee[Blockchain.UtilityToken.Hash] = _info.Transaction.SystemFee + _info.Transaction.NetworkFee;

                            this.TxbFee.Text = MakeStrings(fee);

                            AssetState state = null;
                            if (_info.Transaction.Outputs.Length > 0)
                            {
                                state = Blockchain.Default.GetAssetState(_info.Transaction.Outputs[0].AssetId);
                            }

                            Fixed8 totalAmount = Fixed8.Zero;
                            foreach(TransactionOutput output in _info.Transaction.Outputs)
                            {
                                if (output.AssetId != Blockchain.UtilityToken.Hash)
                                {
                                    totalAmount += output.Value;
                                }
                            }

                            if (state != null)
                            {
                                this.TxbAmount.Text = state.GetName() + " " + totalAmount.ToString();
                            }
                            else
                            {
                                this.TxbAmount.Text = "UNKNOWN";
                            }

                            if (_info.Height == null || _info.Height == 0)
                            {
                                txStatus = TxStatus.pending;
                            }
                            else
                            {
                                txStatus = TxStatus.completed;
                            }

                            TxbYear.Text = _info.Time.Year.ToString();

                            if (iLang == LANG.JP)
                            {
                                TxbMonthDay.Text = MONTH_JP[_info.Time.Month - 1] + " " + _info.Time.Day.ToString();
                            }
                            else
                            {
                                TxbMonthDay.Text = MONTH_EN[_info.Time.Month - 1] + " " + _info.Time.Day.ToString();
                            }
                            break;
                        }
                    case TransactionType.MinerTransaction:
                        {
                            this.gridColorPan.Background = new SolidColorBrush(Color.FromArgb(0xFF, 255, 131, 6));
                            this.TxbFrom.Text = StringTable.GetInstance().GetString("STR_TX_TYPE_MINER", iLang);
                            this.TxbAddress.Text = _info.Transaction.Hash.ToString();
                            this.grdFee.Visibility = Visibility.Collapsed;

                            Dictionary<UInt256, Fixed8> fee = new Dictionary<UInt256, Fixed8>();

                            Fixed8 totalAmount = Fixed8.Zero;
                            foreach (TransactionOutput output in _info.Transaction.Outputs)
                            {
                                if (Constant.CurrentWallet.ContainsAddress(output.ScriptHash))
                                {
                                    if (fee.ContainsKey(output.AssetId))
                                    {
                                        fee[output.AssetId] += output.Value;
                                    }
                                    else
                                    {
                                        fee[output.AssetId] = output.Value;
                                    }
                                }
                            }

                            TxbAmount.Text = MakeStrings(fee);

                            if (_info.Height == null || _info.Height == 0)
                            {
                                txStatus = TxStatus.pending;
                            }
                            else
                            {
                                txStatus = TxStatus.completed;
                            }

                            TxbYear.Text = _info.Time.Year.ToString();

                            if (iLang == LANG.JP)
                            {
                                TxbMonthDay.Text = MONTH_JP[_info.Time.Month - 1] + " " + _info.Time.Day.ToString();
                            }
                            else
                            {
                                TxbMonthDay.Text = MONTH_EN[_info.Time.Month - 1] + " " + _info.Time.Day.ToString();
                            }
                            break;
                        }
                    default:
                        break;
                }
            }
            
        }

        public void InitInstance()
        {
            if (_info != null)
            {

            }
        }

        public void RefreshInterface()
        {
            InitInterface();
        }

        public void InitInterface()
        {
            if (_info != null)
            {
                switch (_info.Transaction.Type)
                {
                    case TransactionType.AnonymousContractTransaction:
                    case TransactionType.ContractTransaction:
                    case TransactionType.RingConfidentialTransaction:
                        {
                            TxbYear.Text = _info.Time.Year.ToString();

                            if (_info.Height == null || _info.Height == 0)
                            {
                                txStatus = TxStatus.pending;
                            }
                            else
                            {
                                txStatus = TxStatus.completed;
                            }

                            ShowTxStatus(txStatus);

                            if (_info.Transaction.Type == Pure.Core.TransactionType.AnonymousContractTransaction)
                            {
                                DoProcessAnonymousTx();
                            }
                            else if(_info.Transaction.Type == Pure.Core.TransactionType.RingConfidentialTransaction)
                            {
                                DoProcessRingConfidentialTx();
                            }
                            else
                            {
                                DoProcessTransparentTx();
                            }
                            break;
                        }

                    case TransactionType.ClaimTransaction:
                        this.gridColorPan.Background = new SolidColorBrush(Color.FromArgb(0xFF, 0xf0, 0xf0, 0xf0));
                        this.TxbFrom.Text = StringTable.GetInstance().GetString("STR_TX_TYPE_CLAIM", iLang);
                        this.TxbAddress.Text = _info.Transaction.Hash.ToString();
                        this.TxbAmount.Visibility = Visibility.Hidden;
                        break;
                    case TransactionType.InvocationTransaction:
                        this.gridColorPan.Background = new SolidColorBrush(Color.FromArgb(0xFF, 185, 0, 181));
                        this.TxbFrom.Text = StringTable.GetInstance().GetString("STR_TX_TYPE_INVOCATION", iLang);
                        break;
                    case TransactionType.MinerTransaction:
                        this.gridColorPan.Background = new SolidColorBrush(Color.FromArgb(0xFF, 255, 131, 6));
                        this.TxbFrom.Text = StringTable.GetInstance().GetString("STR_TX_TYPE_MINER", iLang);
                        break;
                    default:
                        break;
                }
            }
        }

        private void ShowTxStatus(TxStatus status)
        {
            switch (status)
            {
                case TxStatus.completed:
                    TxbStatusPending.Visibility = Visibility.Hidden;
                    TxbStatusCompleted.Visibility = Visibility.Visible;
                    TxbStatusUnknown.Visibility = Visibility.Hidden;
                    break;
                case TxStatus.pending:
                    TxbStatusPending.Visibility = Visibility.Visible;
                    TxbStatusCompleted.Visibility = Visibility.Hidden;
                    TxbStatusUnknown.Visibility = Visibility.Hidden;
                    break;
                default:
                    TxbStatusPending.Visibility = Visibility.Hidden;
                    TxbStatusCompleted.Visibility = Visibility.Hidden;
                    TxbStatusUnknown.Visibility = Visibility.Visible;
                    break;
            }
        }

        private void ShowTxArrows(TxArrow arrow)
        {
            switch (arrow)
            {
                case TxArrow.From:
                    TxbFrom.Visibility = Visibility.Visible;
                    TxbTo.Visibility = Visibility.Hidden;
                    TxbAnonymous.Visibility = Visibility.Hidden;
                    break;
                case TxArrow.To:
                    TxbFrom.Visibility = Visibility.Hidden;
                    TxbTo.Visibility = Visibility.Visible;
                    TxbAnonymous.Visibility = Visibility.Hidden;
                    break;
                case TxArrow.Anonymous:
                    TxbFrom.Visibility = Visibility.Hidden;
                    TxbTo.Visibility = Visibility.Hidden;
                    TxbAnonymous.Visibility = Visibility.Visible;
                    break;
                default:
                    break;
            }
        }

        string MakeStrings(Dictionary<UInt256, Fixed8> balance, bool isFee = false)
        {
            string ret = "";

            Dictionary<UInt256, Fixed8> filter = new Dictionary<UInt256, Fixed8>();

            foreach (var key in balance.Keys)
            {
                if (balance[key] > Fixed8.Zero)
                {
                    filter[key] = balance[key];
                }
            }

            if (filter.Count == 0)
            {
                return isFee? "0" : StringTable.GetInstance().GetString("STR_FEE_FREE", iLang);
            }

            foreach (var key in filter.Keys)
            {
                AssetState asset = Blockchain.Default.GetAssetState(key);
                ret += asset.GetName() + " " + filter[key].ToString();

                if (key != filter.Keys.Last())
                    ret += System.Environment.NewLine;
            }

            return ret;
        }

        void DoProcessAnonymousTx()
        {
            bool isAnonymousKey = false;
            Fixed8 vPubOld = Fixed8.Zero;
            Fixed8 vPubNew = Fixed8.Zero;

            Dictionary<UInt256, Fixed8> vPubOlds = new Dictionary<UInt256, Fixed8>();
            Dictionary<UInt256, Fixed8> vPubNews = new Dictionary<UInt256, Fixed8>();

            foreach(var key in keys)
            {
                if (key.nVersion == KeyType.Anonymous)
                {
                    isAnonymousKey = true;
                }
            }

            for (int i = 0; i < ((AnonymousContractTransaction)_info.Transaction).byJoinSplit.Count; i++)
            {
                vPubOld += ((AnonymousContractTransaction)_info.Transaction).vPub_Old(i);
                vPubNew += ((AnonymousContractTransaction)_info.Transaction).vPub_New(i);

                if (vPubOlds.ContainsKey(((AnonymousContractTransaction)_info.Transaction).Asset_ID(i)))
                {
                    vPubOlds[((AnonymousContractTransaction)_info.Transaction).Asset_ID(i)] += ((AnonymousContractTransaction)_info.Transaction).vPub_Old(i);
                }
                else
                {
                    vPubOlds[((AnonymousContractTransaction)_info.Transaction).Asset_ID(i)] = ((AnonymousContractTransaction)_info.Transaction).vPub_Old(i);
                }

                if (vPubNews.ContainsKey(((AnonymousContractTransaction)_info.Transaction).Asset_ID(i)))
                {
                    vPubNews[((AnonymousContractTransaction)_info.Transaction).Asset_ID(i)] += ((AnonymousContractTransaction)_info.Transaction).vPub_Old(i);
                }
                else
                {
                    vPubNews[((AnonymousContractTransaction)_info.Transaction).Asset_ID(i)] = ((AnonymousContractTransaction)_info.Transaction).vPub_Old(i);
                }
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
                    txArrow = TxArrow.From;
                    TxbAddress.Text = "";

                    List<string> fromAddrs = new List<string>();

                    for (int i = 0; i < _info.Transaction.Inputs.Length; i++)
                    {
                        if (!fromAddrs.Contains(Wallet.ToAddress(_info.Transaction.References[_info.Transaction.Inputs[i]].ScriptHash)))
                        {
                            fromAddrs.Add(Wallet.ToAddress(_info.Transaction.References[_info.Transaction.Inputs[i]].ScriptHash));
                        }
                    }

                    TxbAddress.Text = fromAddrs.First()?.ToString();

                    #region Calculate balance & fee
                    Dictionary<UInt256, Fixed8> balance = new Dictionary<UInt256, Fixed8>();
                    Dictionary<UInt256, Fixed8> fee = new Dictionary<UInt256, Fixed8>();

                    for (int i = 0; i < ((AnonymousContractTransaction)_info.Transaction).byJoinSplit.Count; i++)
                    {
                        if (isAnonymousKey)
                        {
                            foreach (var privKey in privKeys)
                            {
                                List<JSTransactionOutput> outputs = Wallet.DecryptJS((AnonymousContractTransaction)_info.Transaction, i, privKey, ((AnonymousContractTransaction)_info.Transaction).joinSplitPubKey);
                                foreach (JSTransactionOutput output in outputs)
                                {
                                    bool isOwner = false;
                                    foreach (UInt160 scriptHash in addresses.ToArray())
                                    {
                                        VerificationContract contract = Constant.CurrentWallet.GetContract(scriptHash);

                                        if (Wallet.ToAnonymousAddress(output.addr) == contract.Address)
                                        {
                                            isOwner = true;
                                        }
                                    }

                                    if (isOwner == true)
                                    {
                                        if (balance.ContainsKey(output.AssetId))
                                        {
                                            balance[output.AssetId] += output.Value;
                                        }
                                        else
                                        {
                                            balance[output.AssetId] = output.Value;
                                        }
                                    }
                                }
                            }
                        }
                    }

                    fee[Blockchain.UtilityToken.Hash] = ((AnonymousContractTransaction)_info.Transaction).FromTSysFee;
                    #endregion


                    TxbAmount.Text = MakeStrings(balance);
                    TxbFee.Text = MakeStrings(fee);
                    this.gridColorPan.Background = new SolidColorBrush(Color.FromArgb(0xFF, 122, 229, 147));
                }
                else
                {// In case, my address is t-addr, and tx is T->A
                    txArrow = TxArrow.To;

                    TxbAddress.Text = "XXX";
                    for (int i = 0; i < _info.Transaction.Outputs.Length; i++)
                    {
                        if (!Constant.CurrentWallet.ContainsAddress(_info.Transaction.Outputs[i].ScriptHash))
                        {
                            TxbAddress.Text += Wallet.ToAddress(_info.Transaction.Outputs[i].ScriptHash);
                        }
                    }

                    #region Calculate balance & fee
                    Dictionary<UInt256, Fixed8> balance = new Dictionary<UInt256, Fixed8>();
                    Dictionary<UInt256, Fixed8> fee = new Dictionary<UInt256, Fixed8>();

                    // Calculate Input Sum
                    foreach (var key in _info.Transaction.References.Keys)
                    {
                        if (Constant.CurrentWallet.ContainsAddress(_info.Transaction.References[key].ScriptHash))
                        {
                            if (balance.ContainsKey(_info.Transaction.References[key].AssetId))
                            {
                                balance[_info.Transaction.References[key].AssetId] += _info.Transaction.References[key].Value;
                            }
                            else
                            {
                                balance[_info.Transaction.References[key].AssetId] = _info.Transaction.References[key].Value;
                            }
                        }
                    }

                    // Calculate Output Sum
                    foreach (var txOut in _info.Transaction.Outputs)
                    {
                        if (Constant.CurrentWallet.ContainsAddress(txOut.ScriptHash))
                        {
                            if (balance.ContainsKey(txOut.AssetId))
                            {
                                balance[txOut.AssetId] -= txOut.Value;
                            }
                        }
                    }

                    fee[Blockchain.UtilityToken.Hash] = ((AnonymousContractTransaction)_info.Transaction).FromTSysFee;

                    Fixed8 qrgFee = fee.Sum(p => p.Value);
                    fee.Clear();
                    if (qrgFee >= Fixed8.Zero)
                    {
                        fee[Blockchain.UtilityToken.Hash] = qrgFee;
                    }

                    foreach (var key in fee.Keys)
                    {
                        if (balance.ContainsKey(key))
                            balance[key] -= fee[key];
                    }
                    #endregion

                    TxbAmount.Text = MakeStrings(balance);
                    TxbFee.Text = MakeStrings(fee);
                    this.gridColorPan.Background = new SolidColorBrush(Color.FromArgb(0xFF, 229, 122, 132));
                }
            }
            else if (_info.Transaction.Inputs.Length == 0 && _info.Transaction.Outputs.Length > 0)
            { // z_addr to t_addr
                TxbAddress.Text = "XXX";
                foreach (KeyPair key in keys)
                {
                    if (key.nVersion == KeyType.Anonymous)
                    {
                        txArrow = TxArrow.To;
                        this.gridColorPan.Background = new SolidColorBrush(Color.FromArgb(0xFF, 229, 122, 132));

                        foreach(var output in _info.Transaction.Outputs)
                        {
                            TxbAddress.Text = Wallet.ToAddress(output.ScriptHash);
                        }
                    }
                    else
                    {
                        txArrow = TxArrow.From;
                        this.gridColorPan.Background = new SolidColorBrush(Color.FromArgb(0xFF, 122, 229, 147));
                    }
                }

                #region Calculate balance & fee
                Dictionary<UInt256, Fixed8> balance = new Dictionary<UInt256, Fixed8>();
                Dictionary<UInt256, Fixed8> fee = new Dictionary<UInt256, Fixed8>();

                if (txArrow == TxArrow.From)
                {
                    // Calculate balance
                    foreach (var txOut in _info.Transaction.Outputs)
                    {
                        if (Constant.CurrentWallet.ContainsAddress(txOut.ScriptHash))
                        {
                            if (balance.ContainsKey(txOut.AssetId))
                            {
                                balance[txOut.AssetId] += txOut.Value;
                            }
                            else
                            {
                                balance[txOut.AssetId] = txOut.Value;
                            }
                        }
                    }
                    
                    fee[Blockchain.UtilityToken.Hash] = ((AnonymousContractTransaction)_info.Transaction).FromASysFee;
                }
                else if (txArrow == TxArrow.To)
                {
                    IEnumerable<JSCoin> jscoins = Constant.CurrentWallet?.GetJSCoins();
                    for (int i = 0; i < ((AnonymousContractTransaction)_info.Transaction).byJoinSplit.Count; i++)
                    {
                        if (isAnonymousKey)
                        {
                            foreach (var privKey in privKeys)
                            {
                                UInt256[] nullifiers = ((AnonymousContractTransaction)_info.Transaction).Nullifiers(i);
                                foreach (var jscoin in jscoins)
                                {
                                    UInt256 jsNullifier = jscoin.Nullifier(new PureCore.Wallets.AnonymousKey.Key.SpendingKey(new UInt256(privKey)));
                                    if (jsNullifier == nullifiers[0] ||
                                        jsNullifier == nullifiers[1])
                                    {
                                        if (balance.Keys.Contains(jscoin.Output.AssetId))
                                        {
                                            balance[jscoin.Output.AssetId] += jscoin.Output.Value;
                                        }
                                        else
                                        {
                                            balance[jscoin.Output.AssetId] = jscoin.Output.Value;
                                        }
                                    }
                                }
                                List<JSTransactionOutput> outputs = Wallet.DecryptJS((AnonymousContractTransaction)_info.Transaction, i, privKey, ((AnonymousContractTransaction)_info.Transaction).joinSplitPubKey);
                                foreach (JSTransactionOutput output in outputs)
                                {
                                    bool isOwner = false;
                                    foreach (UInt160 scriptHash in addresses.ToArray())
                                    {
                                        VerificationContract contract = Constant.CurrentWallet.GetContract(scriptHash);

                                        if (Wallet.ToAnonymousAddress(output.addr) == contract.Address)
                                        {
                                            isOwner = true;
                                        }
                                    }

                                    if (isOwner == true)
                                    {
                                        AssetState state = Blockchain.Default.GetAssetState(output.AssetId);

                                        if (balance.ContainsKey(output.AssetId))
                                        {
                                            balance[output.AssetId] -= output.Value;
                                        }
                                        else
                                        {
                                            balance[output.AssetId] = -output.Value;
                                        }
                                    }
                                }
                            }
                        }

                        // balance - fee
                        fee[Blockchain.UtilityToken.Hash] = ((AnonymousContractTransaction)_info.Transaction).FromASysFee;

                        foreach (var key in fee.Keys)
                        {
                            if (balance.ContainsKey(key))
                                balance[key] -= fee[key];
                        }
                    }
                }
                #endregion

                TxbAmount.Text = MakeStrings(balance);
                TxbFee.Text = MakeStrings(fee);
            }
            else
            { // z_addr to z_addr
                TxbAddress.Text = "XXX";
                txArrow = TxArrow.Anonymous;
                this.gridColorPan.Background = new SolidColorBrush(Color.FromArgb(0xFF, 160, 166, 204));

                #region Calculate balance & fee
                Dictionary<UInt256, Fixed8> balance = new Dictionary<UInt256, Fixed8>();
                Dictionary<UInt256, Fixed8> fee = new Dictionary<UInt256, Fixed8>();
                IEnumerable<JSCoin> jscoins = Constant.CurrentWallet?.GetJSCoins();
                for (int i = 0; i < ((AnonymousContractTransaction)_info.Transaction).byJoinSplit.Count; i++)
                {
                    if (isAnonymousKey)
                    {
                        foreach (var privKey in privKeys)
                        {
                            UInt256[] nullifiers = ((AnonymousContractTransaction)_info.Transaction).Nullifiers(i);
                            foreach (var jscoin in jscoins)
                            {
                                UInt256 jsNullifier = jscoin.Nullifier(new PureCore.Wallets.AnonymousKey.Key.SpendingKey(new UInt256(privKey)));
                                if (jsNullifier == nullifiers[0] ||
                                    jsNullifier == nullifiers[1])
                                {
                                    if (balance.Keys.Contains(jscoin.Output.AssetId))
                                    {
                                        balance[jscoin.Output.AssetId] += jscoin.Output.Value;
                                    }
                                    else
                                    {
                                        balance[jscoin.Output.AssetId] = jscoin.Output.Value;
                                    }
                                }
                            }
                        }
                    }
                }

                if (balance.Count > 0)  // To
                {
                    this.gridColorPan.Background = new SolidColorBrush(Color.FromArgb(0xFF, 229, 122, 132));

                    for (int i = 0; i < ((AnonymousContractTransaction)_info.Transaction).byJoinSplit.Count; i++)
                    {
                        if (isAnonymousKey)
                        {
                            foreach (var privKey in privKeys)
                            {
                                List<JSTransactionOutput> outputs = Wallet.DecryptJS((AnonymousContractTransaction)_info.Transaction, i, privKey, ((AnonymousContractTransaction)_info.Transaction).joinSplitPubKey);
                                foreach (JSTransactionOutput output in outputs)
                                {
                                    bool isOwner = false;
                                    foreach (UInt160 scriptHash in addresses.ToArray())
                                    {
                                        VerificationContract contract = Constant.CurrentWallet.GetContract(scriptHash);

                                        if (Wallet.ToAnonymousAddress(output.addr) == contract.Address)
                                        {
                                            isOwner = true;
                                        }
                                    }

                                    if (isOwner == true)
                                    {
                                        AssetState state = Blockchain.Default.GetAssetState(output.AssetId);

                                        if (balance.ContainsKey(output.AssetId))
                                        {
                                            balance[output.AssetId] -= output.Value;
                                        }
                                        else
                                        {
                                            balance[output.AssetId] = -output.Value;
                                        }
                                    }
                                }
                            }
                        }

                        // balance - fee
                        fee[Blockchain.UtilityToken.Hash] = ((AnonymousContractTransaction)_info.Transaction).FromASysFee;

                        foreach (var key in fee.Keys)
                        {
                            if (balance.ContainsKey(key))
                                balance[key] -= fee[key];
                        }
                    }
                }
                else  // From
                {
                    this.gridColorPan.Background = new SolidColorBrush(Color.FromArgb(0xFF, 122, 229, 147));

                    for (int i = 0; i < ((AnonymousContractTransaction)_info.Transaction).byJoinSplit.Count; i++)
                    {
                        if (isAnonymousKey)
                        {
                            foreach (var privKey in privKeys)
                            {
                                List<JSTransactionOutput> outputs = Wallet.DecryptJS((AnonymousContractTransaction)_info.Transaction, i, privKey, ((AnonymousContractTransaction)_info.Transaction).joinSplitPubKey);
                                foreach (JSTransactionOutput output in outputs)
                                {
                                    bool isOwner = false;
                                    foreach (UInt160 scriptHash in addresses.ToArray())
                                    {
                                        VerificationContract contract = Constant.CurrentWallet.GetContract(scriptHash);

                                        if (Wallet.ToAnonymousAddress(output.addr) == contract.Address)
                                        {
                                            isOwner = true;
                                        }
                                    }

                                    if (isOwner == true)
                                    {
                                        AssetState state = Blockchain.Default.GetAssetState(output.AssetId);

                                        if (balance.ContainsKey(output.AssetId))
                                        {
                                            balance[output.AssetId] += output.Value;
                                        }
                                        else
                                        {
                                            balance[output.AssetId] = output.Value;
                                        }
                                    }
                                }
                            }
                        }
                        
                        fee[Blockchain.UtilityToken.Hash] = ((AnonymousContractTransaction)_info.Transaction).FromASysFee;
                    }
                }
                #endregion

                TxbAmount.Text = MakeStrings(balance);
                TxbFee.Text = MakeStrings(fee);
            }

            ShowTxArrows(txArrow);
        }

        void DoProcessTransparentTx()
        {
            bool isFrom = true;
            Dictionary<UInt256, Fixed8> balance = new Dictionary<UInt256, Fixed8>();
            Dictionary<UInt256, Fixed8> fee = new Dictionary<UInt256, Fixed8>();

            for (int i = 0; i < _info.Transaction.Inputs.Length; i++) 
            {
                if (Constant.CurrentWallet.ContainsAddress(_info.Transaction.References[_info.Transaction.Inputs[i]].ScriptHash))
                {
                    isFrom = false;

                    if (balance.ContainsKey(_info.Transaction.References[_info.Transaction.Inputs[i]].AssetId))
                    {
                        balance[_info.Transaction.References[_info.Transaction.Inputs[i]].AssetId] += _info.Transaction.References[_info.Transaction.Inputs[i]].Value;
                    }
                    else
                    {
                        balance[_info.Transaction.References[_info.Transaction.Inputs[i]].AssetId] = _info.Transaction.References[_info.Transaction.Inputs[i]].Value;
                    }
                }
            }

            if (isFrom)
            {
                TxbAddress.Text = "";
                for (int i = 0; i < _info.Transaction.Inputs.Length; i++)
                {
                    TxbAddress.Text += Wallet.ToAddress(_info.Transaction.References[_info.Transaction.Inputs[i]].ScriptHash);
                }

                for (int i = 0; i < _info.Transaction.Outputs.Length; i++)
                {
                    AssetState state = Blockchain.Default.GetAssetState(_info.Transaction.Outputs[i].AssetId);
                    if (Constant.CurrentWallet.ContainsAddress(_info.Transaction.Outputs[i].ScriptHash))
                    {
                        if (balance.ContainsKey(_info.Transaction.Outputs[i].AssetId))
                        {
                            balance[_info.Transaction.Outputs[i].AssetId] += _info.Transaction.Outputs[i].Value;
                        }
                        else
                        {
                            balance[_info.Transaction.Outputs[i].AssetId] = _info.Transaction.Outputs[i].Value;
                        }
                    }

                    if (!fee.ContainsKey(_info.Transaction.Outputs[i].AssetId))
                    {
                        fee[_info.Transaction.Outputs[i].AssetId] = _info.Transaction.Outputs[i].Fee;
                    }
                }
                
                Fixed8 qrgFee = fee.Sum(p => p.Value);
                fee.Clear();
                
                if (qrgFee >= Fixed8.Zero)
                {
                    fee[Blockchain.UtilityToken.Hash] = qrgFee;
                }

                TxbAmount.Text = MakeStrings(balance);
                TxbFee.Text = MakeStrings(fee);

                this.gridColorPan.Background = new SolidColorBrush(Color.FromArgb(0xFF, 122, 229, 147));

                txArrow = TxArrow.From;
            }
            else
            {
                TxbAddress.Text = "";
                for (int i = 0; i < _info.Transaction.Outputs.Length; i++)
                {
                    if (!Constant.CurrentWallet.ContainsAddress(_info.Transaction.Outputs[i].ScriptHash))
                    {
                        TxbAddress.Text += Wallet.ToAddress(_info.Transaction.Outputs[i].ScriptHash);
                    }
                }

                #region Calculate balance & fee
                for (int i = 0; i < _info.Transaction.Outputs.Length; i++)
                {
                    AssetState state = Blockchain.Default.GetAssetState(_info.Transaction.Outputs[i].AssetId);
                    if (Constant.CurrentWallet.ContainsAddress(_info.Transaction.Outputs[i].ScriptHash))
                    {
                        if (balance.ContainsKey(_info.Transaction.Outputs[i].AssetId))
                        {
                            balance[_info.Transaction.Outputs[i].AssetId] -= _info.Transaction.Outputs[i].Value;
                        }
                    }

                    if (!fee.ContainsKey(_info.Transaction.Outputs[i].AssetId))
                    {
                        if (_info.Transaction.Outputs[i].Fee != Fixed8.Zero)
                        {
                            fee[_info.Transaction.Outputs[i].AssetId] = _info.Transaction.Outputs[i].Fee;
                        }
                    }
                }

                Fixed8 qrgFee = fee.Sum(p => p.Value);
                fee.Clear();
                if (qrgFee >= Fixed8.Zero)
                {
                    fee[Blockchain.UtilityToken.Hash] = qrgFee;
                }

                foreach (var key in fee.Keys)
                {
                    if (balance.ContainsKey(key))
                        balance[key] -= fee[key];
                }
                #endregion

                TxbAmount.Text = MakeStrings(balance);
                TxbFee.Text = MakeStrings(fee);

                this.gridColorPan.Background = new SolidColorBrush(Color.FromArgb(0xFF, 229, 122, 132));
                txArrow = TxArrow.To;
            }

            ShowTxArrows(txArrow);
        }

        void DoProcessRingConfidentialTx()
        {
            RingConfidentialTransaction transInfo = (RingConfidentialTransaction)_info.Transaction;
            bool isFrom = false;
            Dictionary<UInt256, Fixed8> balance = new Dictionary<UInt256, Fixed8>();
            Dictionary<UInt256, Fixed8> fee = new Dictionary<UInt256, Fixed8>();
            switch (transInfo.GetTxType())
            {
                case RingConfidentialTransactionType.T_S_Transaction:
                    isFrom = true;
                    for (int i = 0; i < _info.Transaction.Inputs.Length; i++)
                    {
                        if (Constant.CurrentWallet.ContainsAddress(_info.Transaction.References[_info.Transaction.Inputs[i]].ScriptHash))
                        {
                            isFrom = false;
                            if (balance.ContainsKey(_info.Transaction.References[_info.Transaction.Inputs[i]].AssetId))
                            {
                                balance[_info.Transaction.References[_info.Transaction.Inputs[i]].AssetId] += _info.Transaction.References[_info.Transaction.Inputs[i]].Value;
                            }
                            else
                            {
                                balance[_info.Transaction.References[_info.Transaction.Inputs[i]].AssetId] = _info.Transaction.References[_info.Transaction.Inputs[i]].Value;
                            }
                        }
                    }

                    if (isFrom == true)
                    {
                        for (int index = 0; index < transInfo.RingCTSig.Count; index++)
                        {
                            foreach (KeyPairBase key in Constant.CurrentWallet.GetKeys())
                            {
                                if (key is StealthKeyPair rctKey)
                                {
                                    for (int j = 0; j < transInfo.RingCTSig[index].outPK.Count; j++)
                                    {
                                        if (transInfo.RingCTSig[index].outPK[j].dest.ToString() == Pure.Cryptography.ECC.ECPoint.DecodePoint(rctKey.GetPaymentPubKeyFromR(transInfo.RHashKey), Pure.Cryptography.ECC.ECCurve.Secp256r1).ToString())
                                        {
                                            byte[] privKey = rctKey.GenOneTimePrivKey(transInfo.RHashKey);
                                            string strPrivKey = privKey.ToHexString();
                                            Fixed8 amount = Fixed8.Zero;
                                            byte[] mask;

                                            isFrom = true;

                                            try
                                            {
                                                amount = Pure.Core.RingCT.Impls.RingCTSignature.DecodeRct(transInfo.RingCTSig[index], privKey, j, out mask);
                                            }
                                            catch (Exception ex)
                                            {
                                                amount = Fixed8.Zero;
                                            }

                                            if (balance.ContainsKey(transInfo.RingCTSig[index].AssetID))
                                            {
                                                balance[transInfo.RingCTSig[index].AssetID] += amount;
                                            }
                                            else
                                            {
                                                balance[transInfo.RingCTSig[index].AssetID] = amount;
                                            }
                                        }
                                    }
                                }
                            }
                        }

                        TxbAddress.Text = "";
                        for (int i = 0; i < _info.Transaction.Inputs.Length; i++)
                        {
                            TxbAddress.Text = Wallet.ToAddress(_info.Transaction.References[_info.Transaction.Inputs[i]].ScriptHash);
                        }

                        fee.Clear();
                        fee[Blockchain.UtilityToken.Hash] = Blockchain.UtilityToken.A_Fee;

                        TxbAmount.Text = MakeStrings(balance);
                        TxbFee.Text = MakeStrings(fee);

                        this.gridColorPan.Background = new SolidColorBrush(Color.FromArgb(0xFF, 122, 229, 147));

                        txArrow = TxArrow.From;
                    }
                    else
                    {
                        TxbAddress.Text = "XXX";

                        #region Calculate balance & fee
                        for (int i = 0; i < _info.Transaction.Outputs.Length; i++)
                        {
                            AssetState state = Blockchain.Default.GetAssetState(_info.Transaction.Outputs[i].AssetId);
                            if (Constant.CurrentWallet.ContainsAddress(_info.Transaction.Outputs[i].ScriptHash))
                            {
                                if (_info.Transaction.Outputs[i].Fee != Fixed8.Zero)
                                {
                                    fee[_info.Transaction.Outputs[i].AssetId] = _info.Transaction.Outputs[i].Fee;
                                }

                                if (balance.ContainsKey(_info.Transaction.Outputs[i].AssetId))
                                {
                                    balance[_info.Transaction.Outputs[i].AssetId] -= _info.Transaction.Outputs[i].Value;
                                }
                                else
                                    balance[_info.Transaction.Outputs[i].AssetId] = _info.Transaction.Outputs[i].Value;

                                if (_info.Transaction.Outputs[i].AssetId == Blockchain.UtilityToken.Hash && i == _info.Transaction.Outputs.Length - 1)
                                    balance[_info.Transaction.Outputs[i].AssetId] -= (_info.Transaction.Outputs[i].AssetId == Blockchain.UtilityToken.Hash? Blockchain.UtilityToken.A_Fee: Blockchain.GoverningToken.A_Fee);
                            }


                            
                        }

                        fee.Clear();
                        fee[Blockchain.UtilityToken.Hash] = Blockchain.UtilityToken.A_Fee;

                        #endregion

                        if (balance.ElementAt(balance.Count - 1).Key == Blockchain.UtilityToken.Hash &&
                             balance.ElementAt(balance.Count - 1).Value == Blockchain.UtilityToken.A_Fee)
                            balance.Remove(Blockchain.UtilityToken.Hash);

                        TxbAmount.Text = MakeStrings(balance);
                        TxbFee.Text = MakeStrings(fee);

                        this.gridColorPan.Background = new SolidColorBrush(Color.FromArgb(0xFF, 229, 122, 132));
                        txArrow = TxArrow.To;
                    }
                    break;
                case RingConfidentialTransactionType.S_T_Transaction:
                    isFrom = true;

                    foreach (KeyPairBase key in Constant.CurrentWallet.GetKeys())
                    {
                        if (key is StealthKeyPair rctKey)
                        {
                            isFrom = false;
                            break;
                        }
                    }

                    if (isFrom == false)
                    {
                        txArrow = TxArrow.To;
                        TxbAddress.Text = "";
                        for (int i = 0; i < _info.Transaction.Outputs.Length; i++)
                        {
                            TxbAddress.Text += Wallet.ToAddress(_info.Transaction.Outputs[i].ScriptHash);
                        }
                        this.gridColorPan.Background = new SolidColorBrush(Color.FromArgb(0xFF, 229, 122, 132));
                    }
                    else
                    {
                        txArrow = TxArrow.From;
                        TxbAddress.Text = "XXX";
                        
                        this.gridColorPan.Background = new SolidColorBrush(Color.FromArgb(0xFF, 122, 229, 147));
                    }

                    for (int i = 0; i < _info.Transaction.Inputs.Length; i++)
                    {
                        if (Constant.CurrentWallet.ContainsAddress(_info.Transaction.References[_info.Transaction.Inputs[i]].ScriptHash))
                        {
                            isFrom = true;
                            if (balance.ContainsKey(_info.Transaction.References[_info.Transaction.Inputs[i]].AssetId))
                            {
                                balance[_info.Transaction.References[_info.Transaction.Inputs[i]].AssetId] += _info.Transaction.References[_info.Transaction.Inputs[i]].Value;
                            }
                            else
                            {
                                balance[_info.Transaction.References[_info.Transaction.Inputs[i]].AssetId] = _info.Transaction.References[_info.Transaction.Inputs[i]].Value;
                            }
                        }
                    }

                    
                    

                    #region Calculate balance & fee
                    for (int i = 0; i < _info.Transaction.Outputs.Length; i++)
                    {
                        AssetState state = Blockchain.Default.GetAssetState(_info.Transaction.Outputs[i].AssetId);

                        if (balance.ContainsKey(_info.Transaction.Outputs[i].AssetId))
                        {
                            balance[_info.Transaction.Outputs[i].AssetId] -= _info.Transaction.Outputs[i].Value;
                        }
                        else
                            balance[_info.Transaction.Outputs[i].AssetId] = _info.Transaction.Outputs[i].Value;


                        if (_info.Transaction.Outputs[i].Fee != Fixed8.Zero)
                        {
                            fee[_info.Transaction.Outputs[i].AssetId] = _info.Transaction.Outputs[i].Fee;
                        }
                    }

                    fee.Clear();
                    fee[Blockchain.UtilityToken.Hash] = Blockchain.UtilityToken.A_Fee;
                    #endregion

                    TxbAmount.Text = MakeStrings(balance);
                    TxbFee.Text = MakeStrings(fee);

                    
                    break;
                case RingConfidentialTransactionType.S_S_Transaction:
                    for (int index = 0; index < transInfo.RingCTSig.Count; index++)
                    {
                        foreach (KeyPairBase key in Constant.CurrentWallet.GetKeys())
                        {
                            if (key is StealthKeyPair rctKey)
                            {
                                for (int j = 0; j < transInfo.RingCTSig[index].outPK.Count; j++)
                                {
                                    if (transInfo.RingCTSig[index].outPK[j].dest.ToString() == Pure.Cryptography.ECC.ECPoint.DecodePoint(rctKey.GetPaymentPubKeyFromR(transInfo.RHashKey), Pure.Cryptography.ECC.ECCurve.Secp256r1).ToString())
                                    {
                                        byte[] privKey = rctKey.GenOneTimePrivKey(transInfo.RHashKey);
                                        string strPrivKey = privKey.ToHexString();
                                        Fixed8 amount = Fixed8.Zero;
                                        byte[] mask;

                                        if (j == 0)
                                            isFrom = true;
                                        else
                                            isFrom = false;

                                        try
                                        {
                                            amount = Pure.Core.RingCT.Impls.RingCTSignature.DecodeRct(transInfo.RingCTSig[index], privKey, 0, out mask);
                                        }
                                        catch (Exception ex)
                                        {
                                            amount = Fixed8.Zero;
                                        }

                                        if (!(index > 0 && index == transInfo.RingCTSig.Count - 1))
                                        {
                                            if (balance.ContainsKey(transInfo.RingCTSig[index].AssetID))
                                            {
                                                balance[transInfo.RingCTSig[index].AssetID] += amount;
                                            }
                                            else
                                                balance[transInfo.RingCTSig[index].AssetID] = amount;
                                        }

                                        
                                        TxbAddress.Text = "XXX";
                                    }
                                }
                            }
                        }
                    }

                    fee.Clear();
                    fee[Blockchain.UtilityToken.Hash] = Blockchain.UtilityToken.A_Fee;

                    TxbAmount.Text = MakeStrings(balance);
                    TxbFee.Text = MakeStrings(fee);

                    if (isFrom == true)
                    {
                        txArrow = TxArrow.From;
                        this.gridColorPan.Background = new SolidColorBrush(Color.FromArgb(0xFF, 122, 229, 147));
                    }
                    else
                    {
                        txArrow = TxArrow.To;
                        this.gridColorPan.Background = new SolidColorBrush(Color.FromArgb(0xFF, 229, 122, 132));
                    }
                    break;
            }

            ShowTxArrows(txArrow);
        }
    }
}
