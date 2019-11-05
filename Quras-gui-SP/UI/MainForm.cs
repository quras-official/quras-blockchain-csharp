using System;
using System.IO;
using System.IO.Compression;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;

using Pure;
using Pure.IO;
using Pure.Core;
using Pure.Wallets;
using Pure.Core.Anonoymous;
using Pure.Implementations.Wallets.EntityFramework;
using Pure.Implementations.Blockchains.LevelDB;

using Quras_gui_SP.Dialogs;
using Quras_gui_SP.Global;
using Quras_gui_SP.Properties;
using Quras_gui_SP.Global.Addressbook;

using PureCore.Wallets.AnonymousKey.Key;

namespace Quras_gui_SP.UI
{
    public partial class MainForm : Form
    {
        private bool balance_changed = false;
        private bool check_nep5_balance = false;
        private DateTime persistence_time = DateTime.MinValue;

        AddrbookManager addrbookManager;

        public MainForm()
        {
            InitializeComponent();
            InitInterface();
            InitInstance();
        }

        private void InitInterface()
        {
            overviewPan1.BringToFront();
            overviewPan1.Top = 0;
            overviewPan1.Left = 0;
        }

        private void InitInstance()
        {
            overviewPan1.WalletOpenedEvent += Event_WalletOpened;
            
            //SendCoinPan
            sendcoinsPan1.AddAddressBookEvent += Event_AddAddress;
            sendcoinsPan1.SendCoinErrorEvent += Event_SendCoinError;
            sendcoinsPan1.SendCoinSuccessEvent += Event_SendCoinSuccess;

            // AddressBook
            addrBookPan1.AddAddressBookEvent += Event_AddAddress;
            addrBookPan1.EditAddressBookEvent += Event_EditAddress;
            addrBookPan1.DeleteAddressBookEvent += Event_DeleteAddress;

            AssetsImp.getInstance().Reset();
        }

        private void btn_close_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void btn_overview_Click(object sender, EventArgs e)
        {
            pan_tab_select.Top = btn_overview.Top;

            overviewPan1.BringToFront();
            overviewPan1.Top = 0;
            overviewPan1.Left = 0;
        }

        private void btn_tab_send_coins_Click(object sender, EventArgs e)
        {
            pan_tab_select.Top = btn_tab_send_coins.Top;

            sendcoinsPan1.BringToFront();
            sendcoinsPan1.Top = 0;
            sendcoinsPan1.Left = 0;
        }

        private void btn_tab_receive_coins_Click(object sender, EventArgs e)
        {
            pan_tab_select.Top = btn_tab_receive_coins.Top;

            receivecoinsPan1.BringToFront();
            receivecoinsPan1.Top = 0;
            receivecoinsPan1.Left = 0;
        }

        private void btn_tab_transactions_Click(object sender, EventArgs e)
        {
            pan_tab_select.Top = btn_tab_transactions.Top;

            transactionsPan1.BringToFront();
            transactionsPan1.Top = 0;
            transactionsPan1.Left = 0;
        }

        private void btn_tab_addr_book_Click(object sender, EventArgs e)
        {
            pan_tab_select.Top = btn_tab_addr_book.Top;

            addrBookPan1.BringToFront();
            addrBookPan1.Top = 0;
            addrBookPan1.Left = 0;
        }

        private void btn_tab_settings_Click(object sender, EventArgs e)
        {
            pan_tab_select.Top = btn_tab_settings.Top;

            settingsPan1.BringToFront();
            settingsPan1.Top = 0;
            settingsPan1.Left = 0;
        }

        private void btn_minimize_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        // Form Move
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [DllImportAttribute("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd,
                         int Msg, int wParam, int lParam);
        [DllImportAttribute("user32.dll")]
        public static extern bool ReleaseCapture();

        private void MainForm_MouseDown(object sender,
        System.Windows.Forms.MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
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

                    transactionsPan1.AddTransaction(info);
                    transactionRightSidePan1.AddTransaction(info);
                }

                transactionRightSidePan1.LayoutItems();
            }
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

        private void MainForm_Load(object sender, EventArgs e)
        {
            string vkKeyPath = Settings.Default.VkKeyPath;
            string pkKeyPath = Settings.Default.PkKeyPath;

            vkKeyPath = Path.GetFullPath(vkKeyPath);
            pkKeyPath = Path.GetFullPath(pkKeyPath);
            int ret = SnarkDllApi.Snark_DllInit(1, vkKeyPath.ToArray(), pkKeyPath.ToArray());
            if (ret > 0)
            {
                Invoke(new Action(() =>
                {
                    lbl_status.Text = "Verify key was loaded.";
                }));

                Task.Run(() =>
                {
                    Invoke(new Action(() =>
                    {
                        lbl_status.Text = "Loading PK...";
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
                            lbl_status.Text = "Loading PK Successed!";
                        }));
                    }
                    else
                    {
                        Invoke(new Action(() =>
                        {
                            lbl_status.Text = "Loading PK Failed!" + ret.ToString();
                        }));
                    }
                });
            }
            else
            {
                Invoke(new Action(() =>
                {
                    lbl_status.Text = "Loading verify key was failed" + ret.ToString();
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

            LoadAddrbook();
            LoadAssets();
        }

        private void LoadAssets()
        {
            Task.Run(() =>
            {
                for (int i = 0; i < AssetsImp.getInstance().GetList().Count; i ++)
                {
                    Invoke(new Action(() =>
                    {
                        settingsPan1.AddAsset(AssetsImp.getInstance().GetList()[i]);
                    }));
                }
            });
        }

        private void LoadAddrbook()
        {
            Task.Run(() =>
            {
                string addrbookPath = Settings.Default.AddrbookPath;
                addrbookPath = Path.GetFullPath(addrbookPath);
                
                if (File.Exists(addrbookPath))
                {
                    addrbookManager = new AddrbookManager(addrbookPath, false);
                }
                else
                {
                    string addrbookDirPath = Path.GetDirectoryName(addrbookPath);
                    if (!Directory.Exists(addrbookPath))
                    {
                        Directory.CreateDirectory(addrbookDirPath);
                    }

                    addrbookManager = new AddrbookManager(addrbookPath, true);
                }

                var keys = addrbookManager.AddrBooks.Keys;
                foreach(string key in keys)
                {
                    Invoke(new Action(() =>
                    {
                        addrBookPan1.AddContact(addrbookManager.AddrBooks[key].ContactName, key);
                    }));
                }
            });
        }

        private void CurrentWallet_BalanceChanged(object sender, EventArgs e)
        {
            balance_changed = true;
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
            AddrStruct item;
            item.Address = contract.Address;
            item.AmountQRS = "0";
            item.AmountQRG = "0";
            overviewPan1.AddAddress(item);
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

        private void Event_SendCoinSuccess(object sender, EventArgs e)
        {
            Global.AssetDescriptor asset = sendcoinsPan1.GetAsset() as Global.AssetDescriptor;
            string fromAddress = sendcoinsPan1.GetFromAddress();
            string toAddress = sendcoinsPan1.GetRecieveAddress();
            string strAmount = sendcoinsPan1.GetAmount();
            byte toAddrVersion;
            byte fromAddrVersion;

            if (asset == null)
            {
                return;
            }

            if (!Fixed8.TryParse(strAmount, out Fixed8 amount))
            {
                return;
            }
            if (amount == Fixed8.Zero)
            {
                return;
            }
            if (amount.GetData() % (long)Math.Pow(10, 8 - (sendcoinsPan1.GetAsset() as Global.AssetDescriptor).Precision) != 0)
            {
                return;
            }

            try
            {
                fromAddrVersion = Wallet.GetAddressVersion(fromAddress);
                toAddrVersion = Wallet.GetAddressVersion(toAddress);
            }
            catch
            {
                return;
            }

            Transaction tx;

            if (toAddrVersion == Wallet.AddressVersion && fromAddrVersion == Wallet.AddressVersion)
            { 
                List<TransactionAttribute> attributes = new List<TransactionAttribute>();

                tx = new ContractTransaction();

                //if (!string.IsNullOrEmpty(remark))
                //    attributes.Add(new TransactionAttribute
                //    {
                //        Usage = TransactionAttributeUsage.Remark,
                //        Data = Encoding.UTF8.GetBytes(remark)
                //    });

                tx.Attributes = attributes.ToArray();
                TransactionOutput outPut = new TransactionOutput();
                outPut.ScriptHash = Wallet.ToScriptHash(toAddress);
                outPut.Value = amount;
                outPut.AssetId = (UInt256)asset.AssetId;
                tx.Outputs = new TransactionOutput[1];
                tx.Outputs[0] = outPut;
                if (tx is ContractTransaction ctx)
                    tx = Constant.CurrentWallet.MakeTransactionFrom(ctx, fromAddress);

                /*
                if (tx is InvocationTransaction itx)
                {
                    using (InvokeContractDialog dialog = new InvokeContractDialog(itx))
                    {
                        if (dialog.ShowDialog() != DialogResult.OK) return;
                        tx = dialog.GetTransaction();
                    }
                }
                */
                Helper.SignAndShowInformation(tx);
            }
            else if (toAddrVersion == Wallet.AnonymouseAddressVersion)
            {
                UInt256 joinSplitPubKey_;
                byte[] joinSplitPrivKey_;

                List<TransactionAttribute> attributes = new List<TransactionAttribute>();

                tx = new AnonymousContractTransaction();

                tx.Attributes = attributes.ToArray();

                Sodium.KeyPair keyPair;
                keyPair = Sodium.PublicKeyAuth.GenerateKeyPair();

                joinSplitPubKey_ = new UInt256(keyPair.PublicKey);
                joinSplitPrivKey_ = keyPair.PrivateKey;

                ((AnonymousContractTransaction)tx).joinSplitPubKey = joinSplitPubKey_;

                AsyncJoinSplitInfo info = new AsyncJoinSplitInfo();
                info.vpub_old = new Fixed8(0);
                info.vpub_new = new Fixed8(0);

                JSOutput jsOut = new JSOutput(Wallet.ToPaymentAddress(toAddress), amount, (UInt256)asset.AssetId);

                info.vjsout.Add(jsOut);
                info.vpub_old += amount;

                if (tx is AnonymousContractTransaction ctx)
                {
                    tx = Constant.CurrentWallet.MakeTandATransaction(ctx, fromAddress, info);                  
                    if (tx is AnonymousContractTransaction ctx_)
                    {
                        IntPtr w = SnarkDllApi.Witnesses_Create();
                        UInt256 anchor = new UInt256();
                        tx = Constant.CurrentWallet.Perform_JoinSplit(ctx_, info, joinSplitPubKey_, joinSplitPrivKey_, (UInt256)asset.AssetId, w, anchor);
                    }
                }

                Helper.SignAndShowInformation(tx);
            }
        }

        private void Event_SendCoinError(object sender, EventArgs e)
        {
            using (WarningDlg dialog = new WarningDlg("Error", sendcoinsPan1.GetErrorMessage()))
            {
                if (dialog.ShowDialog() != DialogResult.OK)
                {
                }
            }
        }

        private void Event_WalletOpened(object sender, EventArgs e)
        {
            UserWallet wallet;
            try
            {
                wallet = UserWallet.Open(overviewPan1.GetWalletPath(), overviewPan1.GetWalletPassword());
            }
            catch (CryptographicException ex)
            {
                //lbl_warning.Text = "Password is not correct";
                //lbl_warning.Show();
                return;
            }
            catch (FormatException ex)
            {
                //lbl_warning.Text = ex.Message;
                //lbl_warning.Show();
                return;
            }

            overviewPan1.ResetWallet();
            sendcoinsPan1.ResetSendCoin();
            transactionRightSidePan1.Reset();
            transactionsPan1.Reset();

            ChangeWallet(wallet);
            
        }

        private void Event_AddAddress(object sender, EventArgs e)
        {
            string[] param = new string[2];
            param = (string[])sender;

            addrbookManager.SaveStoredData(param[0], param[1]);

            addrBookPan1.AddContact(param[0], param[1]);
        }

        private void Event_EditAddress(object sender, EventArgs e)
        {
            string[] param = new string[3];
            param = (string[])sender;

            addrbookManager.DeleteAddress(param[0]);
            addrbookManager.SaveStoredData(param[1], param[2]);

            addrBookPan1.EditContact(param[0], param[1], param[2]);
        }

        private void Event_DeleteAddress(object sender, EventArgs e)
        {
            using (YesNoDlg dialog = new YesNoDlg("Are you sure?", "You are deleting the contact"))
            {
                if (dialog.ShowDialog() != DialogResult.OK)
                {
                    return;
                }
            }

            string[] param = new string[2];
            param = (string[])sender;

            addrbookManager.DeleteAddress(param[1]);

            addrBookPan1.RemoveContact(param[0], param[1]);
        }

        private void timer_blockchain_Tick(object sender, EventArgs e)
        {
            overviewPan1.SetBlockchainHeight($"Height : {Blockchain.Default.Height}/{Blockchain.Default.HeaderHeight}");
            overviewPan1.SetPeersConnect("Connected : " + Constant.LocalNode.RemoteNodeCount.ToString());

            if (Constant.CurrentWallet != null)
            {
                if (Constant.CurrentWallet.WalletHeight <= Blockchain.Default.Height + 1)
                {
                    if (balance_changed)
                    {
                        IEnumerable<Coin> coins = Constant.CurrentWallet?.GetCoins().Where(p => !p.State.HasFlag(CoinState.Spent)) ?? Enumerable.Empty<Coin>();
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
                        var balance_qrg = coins.Where(p => p.Output.AssetId.Equals(Blockchain.UtilityToken.Hash)).GroupBy(p => p.Output.ScriptHash).ToDictionary(p => p.Key, p => p.Sum(i => i.Output.Value));

                        Fixed8 qrs_total = Fixed8.Zero;
                        Fixed8 qrg_total = Fixed8.Zero;
                        foreach (Controls.AccountItem item in overviewPan1.addrItem)
                        {
                            UInt160 script_hash = Wallet.ToScriptHash(item.GetAddress());
                            Fixed8 qrs = balance_qrs.ContainsKey(script_hash) ? balance_qrs[script_hash] : Fixed8.Zero;
                            Fixed8 qrg = balance_qrg.ContainsKey(script_hash) ? balance_qrg[script_hash] : Fixed8.Zero;

                            qrs_total += qrs;
                            qrg_total += qrg;

                            item.SetQRS(qrs.ToString());
                            item.SetQRG(qrg.ToString());
                        }

                        overviewPan1.SetQrsTotalBalance(qrs_total.ToString());
                        overviewPan1.SetQrgTotalBalance(qrg_total.ToString());

                        sendcoinsPan1.RefreshInterface(qrs_total, qrg_total);

                        balance_changed = false;
                    }
                }
            }
        }
    }
}
