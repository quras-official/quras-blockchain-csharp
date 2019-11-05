using System;
using System.IO;
using System.IO.Compression;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.Windows.Forms;
using System.Threading;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

using Quras_gui.Global;
using Quras_gui.Properties;

using Pure;
using Pure.IO;
using Pure.Core;
using Pure.Wallets;
using Pure.Core.Anonoymous;
using Pure.Implementations.Wallets.EntityFramework;
using Pure.Implementations.Blockchains.LevelDB;
using DT_GUI_Modules.Modules;

using Quras_gui.Dialogs;
using Quras_gui.Controls.Items;

namespace Quras_gui.FormUI
{
    public partial class MainWalletForm : Form
    {
        private bool balance_changed = false;
        private bool check_nep5_balance = false;
        private DateTime persistence_time = DateTime.MinValue;

        //Interface
        private Panel pan_transparent;
        private Panel panSendReceive;
        private SendControl SendCoinControl;

        //Interface History
        public List<HistoryItem> txHistoryItems;

        private int sendPanel_height;
        private bool sendPanel_hidden;

        private int iLang => Constant.GetLang();

        public MainWalletForm(UserWallet userWallet = null)
        {
            InitializeComponent();
            InitInstance();
            InitInterface();

            ChangeWallet(userWallet);
        }

        private void InitInstance()
        {
            //History
            txHistoryItems = new List<HistoryItem>();
            vsb_history.Hide();

            pan_history.MouseWheel += (s, e) => {
                HandleScroll(s, e);
            };

            //Assets
            AssetsImp.getInstance().Reset();

            dashboardPan1.QRCodeBtnClicked += Event_QRCodeBtnClicked;
        }

        private void InitInterface()
        {
            pan_transparent = new DT_GUI_Modules.Controls.Panels.TransparentPanel();

            pan_transparent.Dock = DockStyle.None;
            pan_transparent.BackColor = System.Drawing.Color.Black;
            pan_transparent.Location = new System.Drawing.Point(0, 106);
            pan_transparent.Name = "tt";
            pan_transparent.Size = new System.Drawing.Size(1210, 627);
            pan_transparent.TabIndex = 3;
            pan_transparent.Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom | AnchorStyles.Top;

            this.Controls.Add(pan_transparent);
            pan_transparent.Click += TransparentPanel_Clicked;


            //Panel Send & Receive
            panSendReceive = new Panel();
            SendCoinControl = new SendControl();
            SendCoinControl.SendButtonClick += MakeSendTransaction;

            SendCoinControl.Location = new System.Drawing.Point(0, 0);
            SendCoinControl.Size = new System.Drawing.Size(542, 240);

            panSendReceive.Controls.Add(SendCoinControl);
            panSendReceive.BackColor = System.Drawing.Color.Blue;
            panSendReceive.Dock = DockStyle.None;
            panSendReceive.Location = new System.Drawing.Point(0, 0);
            panSendReceive.Name = "pan_send_receive";
            panSendReceive.Size = new System.Drawing.Size(542, 240);
            panSendReceive.TabIndex = 0;
            this.Controls.Add(panSendReceive);

            // Interface History
            lbl_no_history.Show();

            // Language

            lbl_obj_about.Text = StringTable.DATA[iLang, 77];
            RefreshLang();
        }

        private void RefreshLang()
        {
            if (iLang == 0)
                this.btn_lang.Image = global::Quras_gui.Properties.Resources.usa;
            else if (iLang == 1)
                this.btn_lang.Image = global::Quras_gui.Properties.Resources.japan2;

            this.Text = StringTable.DATA[iLang, 29];
            btn_send.Text = StringTable.DATA[iLang, 30];
            btn_receive.Text = StringTable.DATA[iLang, 31];
            btn_copy_address.Text = StringTable.DATA[iLang, 32];
            if (Constant.CurrentWallet == null)
            {
                lbl_height.Text = $"{StringTable.DATA[iLang, 33]} : {0}/{Blockchain.Default.Height}/{Blockchain.Default.HeaderHeight}";
            }
            else
            {
                lbl_height.Text = $"{StringTable.DATA[iLang, 33]} : {Constant.CurrentWallet.WalletHeight - 1}/{Blockchain.Default.Height}/{Blockchain.Default.HeaderHeight}";
            }

            lbl_connected.Text = $"{StringTable.DATA[iLang, 34]} : " + Constant.LocalNode.RemoteNodeCount.ToString();
            lbl_transaction_history.Text = StringTable.DATA[iLang, 35];
            lbl_no_history.Text = StringTable.DATA[iLang, 36];

            if (iLang == 0)
            {
                for (int i = 0; i < StringTable.DATA.Length / 2; i++)
                {
                    if (lbl_status.Text == StringTable.DATA[1, i])
                    {
                        lbl_status.Text = StringTable.DATA[0, i];
                    }
                    if (lbl_obj_about.Text == StringTable.DATA[1, i])
                    {
                        lbl_obj_about.Text = StringTable.DATA[0, i];
                    }
                }
            }
            else if (iLang == 1)
            {
                for (int i = 0; i < StringTable.DATA.Length / 2; i++)
                {
                    if (lbl_status.Text == StringTable.DATA[0, i])
                    {
                        lbl_status.Text = StringTable.DATA[1, i];
                    }
                    if (lbl_obj_about.Text == StringTable.DATA[0, i])
                    {
                        lbl_obj_about.Text = StringTable.DATA[1, i];
                    }
                }
            }
            
            for (int i = 0; i < txHistoryItems.Count; i ++)
            {
                txHistoryItems[i].RefreshLang();
            }
        }

        private void ShowMask()
        {
            pan_transparent.BringToFront();
        }

        private void HideMask()
        {
            pan_transparent.SendToBack();
        }

        private void ChangeWallet(UserWallet wallet)
        {
            if (Constant.CurrentWallet != null)
            {
                Constant.CurrentWallet.BalanceChanged -= CurrentWallet_BalanceChanged;
                Constant.CurrentWallet.TransactionsChanged -= CurrentWallet_TransactionsChanged;
                Constant.CurrentWallet.Dispose();
            }
            Constant.CurrentWallet = wallet;

            if (Constant.CurrentWallet != null)
            {
                CurrentWallet_TransactionsChanged(null, Constant.CurrentWallet.LoadTransactions());
                Constant.CurrentWallet.BalanceChanged += CurrentWallet_BalanceChanged;
                Constant.CurrentWallet.TransactionsChanged += CurrentWallet_TransactionsChanged;
            }

            if (Constant.CurrentWallet != null)
            {
                foreach (UInt160 scriptHash in Constant.CurrentWallet.GetAddresses().ToArray())
                {
                    KeyPair key = (KeyPair)Constant.CurrentWallet.GetKeyByScriptHash(scriptHash);
                    VerificationContract contract = Constant.CurrentWallet.GetContract(scriptHash);

                    if (contract == null)
                        AddAddressToListView(scriptHash);
                    else
                        AddContractToListView(contract);
                }
            }
            balance_changed = true;
            check_nep5_balance = true;
        }

        private void AddAddressToListView(UInt160 scriptHash, bool selected = false)
        {
            /*string address = Wallet.ToAddress(scriptHash);
            ListViewItem item = listView1.Items[address];
            if (item == null)
            {
                ListViewGroup group = listView1.Groups["watchOnlyGroup"];
                item = listView1.Items.Add(new ListViewItem(new[]
                {
                    new ListViewItem.ListViewSubItem
                    {
                        Name = "address",
                        Text = address
                    },
                    new ListViewItem.ListViewSubItem
                    {
                        Name = "ans"
                    },
                    new ListViewItem.ListViewSubItem
                    {
                        Name = "anc"
                    }
                }, -1, group)
                {
                    Name = address,
                    Tag = scriptHash
                });
            }
            item.Selected = selected;*/
        }

        private void AddContractToListView(VerificationContract contract, bool selected = false)
        {
            DT_GUI_Modules.Modules.AddrStruct item;
            item.Address = contract.Address;
            item.AmountQRS = "0";
            item.AmountUSD = "0";
            //dashboardControl1.AddAddress(item);
        }

        private void CurrentWallet_BalanceChanged(object sender, EventArgs e)
        {
            balance_changed = true;
        }

        private void CurrentWallet_TransactionsChanged(object sender, IEnumerable<TransactionInfo> transactions)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new Action<object, IEnumerable<TransactionInfo>>(CurrentWallet_TransactionsChanged), sender, transactions);
            }
            else
            {
                foreach (TransactionInfo info in transactions)
                {
                    string txid = info.Transaction.Hash.ToString();

                    AddTransaction(info);
                }
            }
        }

        private void MainWalletForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            //Blockchain.PersistCompleted -= Blockchain_PersistCompleted;
            //ChangeWallet(null);
            Application.Exit();
        }

        private void MainWalletForm_Load(object sender, EventArgs e)
        {
            string vkKeyPath = Settings.Default.VkKeyPath;
            string pkKeyPath = Settings.Default.PkKeyPath;

            vkKeyPath = Path.GetFullPath(vkKeyPath);
            pkKeyPath = Path.GetFullPath(pkKeyPath);

            if (Utils.CheckZKSnarksKeyStatus() == 0)
            {
                int ret;
                try
                {
                    ret = SnarkDllApi.Snark_DllInit(1, vkKeyPath.ToArray(), pkKeyPath.ToArray());
                }
                catch (Exception)
                {
                    ret = -1;
                }
                if (ret > 0)
                {
                    Invoke(new Action(() =>
                    {
                        lbl_status.Text = StringTable.DATA[iLang, 37];
                    }));

                    Task.Run(() =>
                    {
                        Invoke(new Action(() =>
                        {
                            lbl_status.Text = StringTable.DATA[iLang, 38];
                        }));

                        vkKeyPath = Settings.Default.VkKeyPath;
                        pkKeyPath = Settings.Default.PkKeyPath;

                        vkKeyPath = Path.GetFullPath(vkKeyPath);
                        pkKeyPath = Path.GetFullPath(pkKeyPath);

                        ret = SnarkDllApi.Snark_DllInit(2, vkKeyPath.ToArray(), pkKeyPath.ToArray());
                        if (ret > 0)
                        {
                            Invoke(new Action(() =>
                            {
                                lbl_status.Text = StringTable.DATA[iLang, 39];
                                Constant.bSnarksParamLoaded = true;
                            }));
                        }
                        else
                        {
                            Invoke(new Action(() =>
                            {
                                lbl_status.Text = StringTable.DATA[iLang, 40] + ret.ToString();
                            }));
                        }
                    });
                }
                else
                {
                    Invoke(new Action(() =>
                    {
                        lbl_status.Text = StringTable.DATA[iLang, 41] + ret.ToString();
                    }));
                }
            }
            else
            {
                Invoke(new Action(() =>
                {
                    lbl_status.Text = StringTable.DATA[iLang, 41];
                }));
            }

            Task.Run(() =>
            {
                const string acc_path = "chain.acc";
                const string acc_zip_path = acc_path + ".zip";
                if (File.Exists(acc_path))
                {
                    using (FileStream fs = new FileStream(acc_path, FileMode.Open, FileAccess.Read, FileShare.None))
                    {
                        ImportBlocks(fs);
                    }
                    File.Delete(acc_path);
                }
                else if (File.Exists(acc_zip_path))
                {
                    using (FileStream fs = new FileStream(acc_zip_path, FileMode.Open, FileAccess.Read, FileShare.None))
                    using (ZipArchive zip = new ZipArchive(fs, ZipArchiveMode.Read))
                    using (Stream zs = zip.GetEntry(acc_path).Open())
                    {
                        ImportBlocks(zs);
                    }
                    File.Delete(acc_zip_path);
                }
                Blockchain.PersistCompleted += Blockchain_PersistCompleted;
                Constant.LocalNode.Start(Settings.Default.NodePort, Settings.Default.WsPort);
            });
        }

        private void LoadAssets()
        {
            Task.Run(() =>
            {
                for (int i = 0; i < AssetsImp.getInstance().GetList().Count; i++)
                {
                    Invoke(new Action(() =>
                    {
                        //settingsPan1.AddAsset(AssetsImp.getInstance().GetList()[i]);
                    }));
                }
            });
        }

        private void Blockchain_PersistCompleted(object sender, Block block)
        {
            persistence_time = DateTime.UtcNow;
            if (Constant.CurrentWallet != null)
            {
                check_nep5_balance = true;
                if (Constant.CurrentWallet.GetCoins().Any(p => !p.State.HasFlag(CoinState.Spent) && p.Output.AssetId.Equals(Blockchain.GoverningToken.Hash)) == true)
                    balance_changed = true;
            }
            CurrentWallet_TransactionsChanged(null, Enumerable.Empty<TransactionInfo>());
        }

        private void ImportBlocks(Stream stream)
        {
            LevelDBBlockchain blockchain = (LevelDBBlockchain)Blockchain.Default;
            blockchain.VerifyBlocks = false;
            using (BinaryReader r = new BinaryReader(stream))
            {
                uint count = r.ReadUInt32();
                for (int height = 0; height < count; height++)
                {
                    byte[] array = r.ReadBytes(r.ReadInt32());
                    if (height > Blockchain.Default.Height)
                    {
                        Block block = array.AsSerializable<Block>();
                        Blockchain.Default.AddBlock(block);
                    }
                }
            }
            blockchain.VerifyBlocks = true;
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            using (SendDialog dialog = new SendDialog())
            {
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    string fromAddress = dialog.GetFromAddress();
                    string toAddress = dialog.GetToAddress();
                    string strAmount = dialog.GetAmount();
                    object AssetID = dialog.GetAssetID();

                    SendCoinThread scthread = new SendCoinThread();
                    scthread.Amount = strAmount;
                    scthread.FromAddr = fromAddress;
                    scthread.ToAddr = toAddress;
                    scthread.Asset = AssetID;

                    ThreadStart starter = new ThreadStart(scthread.DoWork);
                    starter += () => {
                        // Do what you want in the callback
                        Invoke(new Action(() =>
                        {
                            lbl_status.Text = lbl_status.Text.Substring(0, lbl_status.Text.Length - 12);
                        }));
                    };

                    Invoke(new Action(() =>
                    {
                        lbl_status.Text = lbl_status.Text + ";Sending Tx.";
                    }));

                    Thread thread = new Thread(starter) { IsBackground = true };
                    thread.Start();

                    return;
                }
            }
        }

        private void InitializeSendPanel()
        {
            // Asset Init
            foreach (UInt256 asset_id in Constant.CurrentWallet.FindUnspentCoins().Select(p => p.Output.AssetId).Distinct())
            {
                AssetState state = Blockchain.Default.GetAssetState(asset_id);
                //sendPanControl1.AddAssetCombobox(new AssetDescriptor(state));
            }
            /*
            foreach (string s in Settings.Default.NEP5Watched)
            {
                UInt160 asset_id = UInt160.Parse(s);
                try
                {
                    sendPanControl1.AddAssetCombobox(new AssetDescriptor(asset_id));
                }
                catch (ArgumentException)
                {
                    continue;
                }
            }
            */

            // Total Balance Init
        }

        private void btn_receive_Click(object sender, EventArgs e)
        {
            using (ReceiveDialog dialog = new ReceiveDialog())
            {
                if (dialog.ShowDialog() != DialogResult.OK)
                {
                    return;
                }
            }
        }

        private void TransparentPanel_Clicked(object sender, EventArgs e)
        {
            HideMask();
            Utils.Animate(panSendReceive, Utils.Effect.Slide, 150, 270);
        }

        private void SendAssetTypeChangedEvent(object sender, EventArgs e)
        {

        }

        private void MakeSendTransaction(object sender, EventArgs e)
        {

        }

        public void Perform_JoinSplit(AsyncJoinSplitInfo info, UInt256 joinSplitPubKey_)
        {
            UInt256 esk = new UInt256();
            QrsJoinSplit qrsJSparam = new QrsJoinSplit();
            JSDescription jsdesc = JSDescription.Randomized(
                    qrsJSparam,
                    joinSplitPubKey_,
                    new UInt256(),
                    info.vjsin,
                    info.vjsout,
                    info.vpub_old,
                    info.vpub_new,
                    true,
                    esk);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            lbl_height.Text = $"{StringTable.DATA[iLang, 33]} : {Constant.CurrentWallet.WalletHeight - 1}/{Blockchain.Default.Height}/{Blockchain.Default.HeaderHeight}";
            lbl_connected.Text = $"{StringTable.DATA[iLang, 34]} : " + Constant.LocalNode.RemoteNodeCount.ToString();

            if (Constant.CurrentWallet != null)
            {
                if (Constant.CurrentWallet.WalletHeight <= Blockchain.Default.Height + 1)
                {
                    if (balance_changed)
                    {
                        IEnumerable<Coin> coins = Constant.CurrentWallet?.GetCoins().Where(p => !p.State.HasFlag(CoinState.Spent)) ?? Enumerable.Empty<Coin>();
                        IEnumerable<JSCoin> jscoins = Constant.CurrentWallet?.GetJSCoins().Where(p => !p.State.HasFlag(CoinState.Spent)) ?? Enumerable.Empty<JSCoin>();

                        Fixed8 bonus_available = Blockchain.CalculateBonus(Constant.CurrentWallet.GetUnclaimedCoins().Select(p => p.Reference));
                        Fixed8 bonus_unavailable = Blockchain.CalculateBonus(coins.Where(p => p.State.HasFlag(CoinState.Confirmed) && p.Output.AssetId.Equals(Blockchain.GoverningToken.Hash)).Select(p => p.Reference), Blockchain.Default.Height + 1);
                        Fixed8 bonus = bonus_available + bonus_unavailable;
                        var assets = coins.GroupBy(p => p.Output.AssetId, (k, g) => new
                        {
                            Asset = Blockchain.Default.GetAssetState(k),
                            Value = g.Sum(p => p.Output.Value),
                            Claim = k.Equals(Blockchain.UtilityToken.Hash) ? bonus : Fixed8.Zero
                        }).ToDictionary(p => p.Asset.AssetId);
                        if (bonus != Fixed8.Zero && !assets.ContainsKey(Blockchain.UtilityToken.Hash))
                        {
                            assets[Blockchain.UtilityToken.Hash] = new
                            {
                                Asset = Blockchain.Default.GetAssetState(Blockchain.UtilityToken.Hash),
                                Value = Fixed8.Zero,
                                Claim = bonus
                            };
                        }
                        var balance_qrs = coins.Where(p => p.Output.AssetId.Equals(Blockchain.GoverningToken.Hash)).GroupBy(p => p.Output.ScriptHash).ToDictionary(p => p.Key, p => p.Sum(i => i.Output.Value));
                        var balance_js_qrs = jscoins.Where(p => p.Output.AssetId.Equals(Blockchain.GoverningToken.Hash)).GroupBy(p => p.Output.ScriptHash).ToDictionary(p => p.Key, p => p.Sum(i => i.Output.Value));
                        var balance_qrg = coins.Where(p => p.Output.AssetId.Equals(Blockchain.UtilityToken.Hash)).GroupBy(p => p.Output.ScriptHash).ToDictionary(p => p.Key, p => p.Sum(i => i.Output.Value));
                        var balance_js_qrg = jscoins.Where(p => p.Output.AssetId.Equals(Blockchain.UtilityToken.Hash)).GroupBy(p => p.Output.ScriptHash).ToDictionary(p => p.Key, p => p.Sum(i => i.Output.Value));
                        Fixed8 qrs_total = Fixed8.Zero;
                        Fixed8 qrg_total = Fixed8.Zero;
                        foreach (UInt160 scriptHash in Constant.CurrentWallet.GetAddresses().ToArray())
                        {
                            VerificationContract contract = Constant.CurrentWallet.GetContract(scriptHash);

                            Fixed8 qrs = balance_qrs.ContainsKey(scriptHash) ? balance_qrs[scriptHash] : Fixed8.Zero;
                            Fixed8 js_qrs = balance_js_qrs.ContainsKey(scriptHash) ? balance_js_qrs[scriptHash] : Fixed8.Zero;
                            Fixed8 qrg = balance_qrg.ContainsKey(scriptHash) ? balance_qrg[scriptHash] : Fixed8.Zero;
                            Fixed8 js_qrg = balance_js_qrg.ContainsKey(scriptHash) ? balance_js_qrg[scriptHash] : Fixed8.Zero;

                            AddressAssetsImp.getInstance().RefrshAssets(contract.Address, Blockchain.GoverningToken.Hash, (qrs + js_qrs).ToString());
                            AddressAssetsImp.getInstance().RefrshAssets(contract.Address, Blockchain.UtilityToken.Hash, (qrg + js_qrg).ToString());

                            qrs_total += qrs;
                            qrs_total += js_qrs;
                            qrg_total += qrg;
                            qrg_total += js_qrg;
                        }

                        dashboardPan1.SetQRSTotalBalance(Helper.FormatNumber(qrs_total.ToString()));
                        dashboardPan1.SetQRGTotalBalance(Helper.FormatNumber(qrg_total.ToString()));

                        balance_changed = false;
                    }
                }
            }
        }

        private void pan_timer_Tick(object sender, EventArgs e)
        {

        }

        private void AddTransaction(TransactionInfo info)
        {
            bool isAdd = true;
            for (int i = 0; i < txHistoryItems.Count; i++)
            {
                if (txHistoryItems[i]._info.Transaction.Hash == info.Transaction.Hash)
                {
                    isAdd = false;
                    break;
                }
            }

            if (isAdd)
            {
                HistoryItem item = new HistoryItem(info);


                item.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                    | System.Windows.Forms.AnchorStyles.Right)));
                item.BackColor = System.Drawing.Color.White;
                item.Location = new System.Drawing.Point(15, 16 + (txHistoryItems.Count) * (82 + 10));
                item.Name = "historyItem1";
                item.Size = new System.Drawing.Size(641, 82);
                item.TabIndex = 0;

                item.MouseDown_Event += HistoryItemMouseDown_Event;
                item.MouseHover_Event += HistoryItemMouseHover_Event;

                this.pan_history.Controls.Add(item);

                txHistoryItems.Add(item);

                if (txHistoryItems.Count * 92 + 16 > pan_history.Height)
                {
                    vsb_history.Maximum = txHistoryItems.Count * 92 + 16 - pan_history.Height + 16;
                    vsb_history.Show();
                }
            }

            if (txHistoryItems.Count > 0)
            {
                lbl_no_history.Hide();
            }
        }

        private void HistoryItemMouseHover_Event(object param, EventArgs e)
        {

        }

        private void HistoryItemMouseDown_Event(object param, MouseEventArgs e)
        {
            TransactionInfo txInfo = (TransactionInfo)param;
            using (TransactionInfoDialog dlg = new TransactionInfoDialog(txInfo))
            {
                if (dlg.ShowDialog() == DialogResult.OK)
                {

                }
            }
        }

        private void vsb_history_ValueChanged(object sender, EventArgs e)
        {
            for (int i = 0; i < txHistoryItems.Count; i++)
            {
                txHistoryItems[i].Top = 16 + 92 * i - vsb_history.Value;
            }
        }

        private void HandleScroll(object sender, EventArgs e)
        {
            MouseEventArgs param = (MouseEventArgs)e;

            if (param.Delta > 0)
            {
                if (vsb_history.Value > 10)
                {
                    vsb_history.Value = vsb_history.Value - 10;
                }
                else
                {
                    vsb_history.Value = 0;
                }
            }
            else
            {
                if (vsb_history.Value < vsb_history.Maximum - 10)
                {
                    vsb_history.Value = vsb_history.Value + 10;
                }
                else
                {
                    vsb_history.Value = vsb_history.Maximum;
                }
            }

        }

        private void btn_copy_address_Click(object sender, EventArgs e)
        {
            Point parentPt = new Point();
            parentPt.X = this.Location.X + (this.Width - 466) / 2;
            parentPt.Y = this.Location.Y + this.Height - 80;
            using (AlertDialog altDlg = new AlertDialog(
                                            AlertLevel.Show,
                                            parentPt,
                                            this.Location.Y + this.Height,
                                            this.Location.Y + this.Height - 76 - 40,
                                            StringTable.DATA[iLang, 42],
                                            StringTable.DATA[iLang, 43]))
            {
                if (altDlg.ShowDialog() == DialogResult.OK)
                {
                    string copied_msg = "";
                    foreach (UInt160 scriptHash in Constant.CurrentWallet.GetAddresses().ToArray())
                    {
                        VerificationContract contract = Constant.CurrentWallet.GetContract(scriptHash);

                        copied_msg += contract.Address;

                        if (scriptHash != Constant.CurrentWallet.GetAddresses().Last())
                        {
                            copied_msg += ", ";
                        }
                    }

                    try
                    {
                        Clipboard.SetText(copied_msg);
                    }
                    catch (ExternalException) { }
                }
            }
        }

        private void MainWalletForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            timer1.Enabled = false;
            Blockchain.PersistCompleted -= Blockchain_PersistCompleted;
            ChangeWallet(null);
        }

        private void Event_QRCodeBtnClicked(object sender, EventArgs e)
        {
            string address = "";
            foreach (UInt160 scriptHash in Constant.CurrentWallet.GetAddresses().ToArray())
            {
                VerificationContract contract = Constant.CurrentWallet.GetContract(scriptHash);

                address = contract.Address;
            }

            using (QRCodeDialog dlg = new QRCodeDialog(address))
            {
                if (dlg.ShowDialog() == DialogResult.OK)
                {

                }
            }
        }

        private void btn_send_MouseHover(object sender, EventArgs e)
        {
            lbl_obj_about.Text = StringTable.DATA[iLang, 44];
        }

        private void btn_receive_MouseHover(object sender, EventArgs e)
        {
            lbl_obj_about.Text = StringTable.DATA[iLang, 45];
        }

        private void btn_copy_address_MouseHover(object sender, EventArgs e)
        {
            lbl_obj_about.Text = StringTable.DATA[iLang, 46];
        }

        private void dashboardPan1_MouseHover(object sender, EventArgs e)
        {
            lbl_obj_about.Text = StringTable.DATA[iLang, 47];
        }

        private void japaneseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Settings.Default.Language = "JP";
            Settings.Default.Save();
            RefreshLang();
        }

        private void englishToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Settings.Default.Language = "EN";
            Settings.Default.Save();
            RefreshLang();
        }

        private void btn_claim_qrg_Click(object sender, EventArgs e)
        {
            using (ClaimDialog dlg = new ClaimDialog())
            {
                if (dlg.ShowDialog() == DialogResult.OK)
                {

                }
            }
        }

        private void btn_lang_Click(object sender, EventArgs e)
        {

        }
    }
}
