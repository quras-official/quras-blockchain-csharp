using System;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Drawing;
using System.Windows;
using System.Threading;
using System.Windows.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Animation;

using QRCoder;

using Pure;
using Pure.IO;
using Pure.Core;
using Pure.Wallets;
using Pure.Network;
using Pure.Core.Anonoymous;
using Pure.Implementations.Wallets.EntityFramework;
using Pure.Implementations.Blockchains.LevelDB;

using WpfPageTransitions;

using Quras_gui_wpf.Controls;
using Quras_gui_wpf.Pages;
using Quras_gui_wpf.Utils;
using Quras_gui_wpf.Global;
using Quras_gui_wpf.Dialogs;
using Quras_gui_wpf.Properties;

namespace Quras_gui_wpf.Windows
{
    public enum PageStatus
    {
        HistoryPage,
        SettingPage,
        SmartContractPage
    }

    public enum SendOrReceivePageStatus
    {
        None,
        SendPage,
        ReceivePage
    }

    public enum MenuPageStatus
    {
        AssetPage,
        TaskPage
    }
    /// <summary>
    /// Interaction logic for MainWalletWindow.xaml
    /// </summary>
    public partial class MainWalletWindow : Window
    {
        private DispatcherTimer engineTimer;

        private bool balance_changed = false;
        private bool check_nep5_balance = false;
        private DateTime persistence_time = DateTime.MinValue;

        private PageStatus currentPageStatus;
        private SendOrReceivePageStatus currentSendOrReceivePageStatus;
        private MenuPageStatus currentMenuPageStatus;

        private List<VerificationContract> currentVerificationContractList;
        private bool isMenuOpen = false;
        private bool isRepair = false;
        private bool isLogOut = false;

        List<byte[]> privKeys;
        IEnumerable<KeyPairBase> keys; 
        IEnumerable<UInt160> addresses;

        #region Pages
        private SendPages sendPage;
        private ReceivePage receivePage;
        private HistoryPage historyPage;
        private SettingsPage settingsPage;
        private SmartContractPage smartContractPage;
        #endregion

        #region MenuPages
        private AssetInfoPage assetInfoPage;
        private TaskInfoPage taskInfoPage;
        #endregion

        #region customize Items
        public List<HistoryItem> txHistoryItems;
        public List<AssetItem> assetItems;
        #endregion

        private LANG iLang => Constant.GetLang();

        public MainWalletWindow(UserWallet userWallet = null)
        {
            InitializeComponent();
            InitInstance();

            RefreshLanguage();
            ChangeWallet(userWallet);
        }

        public void RefreshLanguage()
        {
            if (iLang == LANG.JP)
            {
                btnSend.Visibility = Visibility.Hidden;
                btnSendJP.Visibility = Visibility.Visible;

                btnReceive.Visibility = Visibility.Hidden;
                btnReceiveJP.Visibility = Visibility.Visible;

                btnCopyAddress.Visibility = Visibility.Hidden;
                btnCopyAddressJP.Visibility = Visibility.Visible;

                btnSmartContract.Visibility = Visibility.Hidden;
                btnSmartContractJP.Visibility = Visibility.Visible;

                btnSettings.Visibility = Visibility.Hidden;
                btnSettingsJP.Visibility = Visibility.Visible;

                btnHistory.Visibility = Visibility.Hidden;
                btnHistoryJP.Visibility = Visibility.Visible;
            }
            else
            {
                btnSend.Visibility = Visibility.Visible;
                btnSendJP.Visibility = Visibility.Hidden;

                btnReceive.Visibility = Visibility.Visible;
                btnReceiveJP.Visibility = Visibility.Hidden;

                btnCopyAddress.Visibility = Visibility.Visible;
                btnCopyAddressJP.Visibility = Visibility.Hidden;

                btnSmartContract.Visibility = Visibility.Visible;
                btnSmartContractJP.Visibility = Visibility.Hidden;

                btnSettings.Visibility = Visibility.Visible;
                btnSettingsJP.Visibility = Visibility.Hidden;

                btnHistory.Visibility = Visibility.Visible;
                btnHistoryJP.Visibility = Visibility.Hidden;
            }

            sendPage.RefreshLanguage();
            receivePage.RefreshLanguage();
            historyPage.RefreshLanguage();
            smartContractPage.RefreshLanguage();
        }

        private void InitInstance()
        {
            engineTimer = new DispatcherTimer();
            engineTimer.Tick += this.dispatcherTimer_Tick;
            engineTimer.Interval = new TimeSpan(0, 0, 0, 0, 500);
            engineTimer.Start();

            currentVerificationContractList = new List<VerificationContract>();

            #region Pages Initializer
            sendPage = new SendPages();
            receivePage = new ReceivePage();
            historyPage = new HistoryPage();
            settingsPage = new SettingsPage();
            smartContractPage = new SmartContractPage();

            assetInfoPage = new AssetInfoPage();
            taskInfoPage = new TaskInfoPage();
            #endregion

            #region Pages Event Handlers
            sendPage.TaskChangedEventHandler += TaskChangedEvent;

            settingsPage.ChangeLanguageEvent += ChangeLanguageEvent;
            settingsPage.UpdateDownloadedFinished += UpdateDownloadedFinishedEvent;
            settingsPage.ResetHistoryEvent += ResetHistoryEvent;
            settingsPage.TaskChangedEvent += TaskChangedEvent;
            #endregion

            txHistoryItems = new List<HistoryItem>();
            assetItems = new List<AssetItem>();

            #region Load Pages
            currentPageStatus = PageStatus.HistoryPage;
            pageMainTransitionControl.ShowPage(historyPage);
            
            currentSendOrReceivePageStatus = SendOrReceivePageStatus.None;
            currentMenuPageStatus = MenuPageStatus.AssetPage;
            ShowCurrentMenu();

            #endregion

            if (File.Exists(Constant.PEER_STATE_PATH))
                using (FileStream fs = new FileStream(Constant.PEER_STATE_PATH, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    LocalNode.LoadState(fs);
                }

            try
            {
                if (Constant.LocalNode == null)
                {
                    Constant.LocalNode = new LocalNode();
                    Constant.LocalNode.UpnpEnabled = true;
                }
            }
            catch
            {

            }
            finally
            {

            }
        }

        private void DrawQRCode()
        {
            // Get Address
            string receiveAddress = "";
            foreach (UInt160 scriptHash in Constant.CurrentWallet.GetAddresses().ToArray())
            {
                VerificationContract contract = Constant.CurrentWallet.GetContract(scriptHash);
                receiveAddress = contract.Address;
            }

            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(receiveAddress, QRCodeGenerator.ECCLevel.Q);
            QRCode qrCode = new QRCode(qrCodeData);
            Bitmap qrCodeImage = qrCode.GetGraphic(20);

            BitmapImage qrBtmImg = new BitmapImage();

            qrBtmImg.BeginInit();

            MemoryStream ms = new MemoryStream();

            // Save to a memory stream...
            qrCodeImage.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);

            // Rewind the stream...
            ms.Seek(0, SeekOrigin.Begin);

            // Tell the WPF image to use this stream...
            qrBtmImg.StreamSource = ms;

            qrBtmImg.EndInit();

            ImgQRCode.Source = qrBtmImg;

            /*
            Bitmap result = new Bitmap(ImgQRCode.Width, pic_qr_code.Height);
            using (Graphics g = Graphics.FromImage(result))
                g.DrawImage(qrCodeImage, 0, 0, pic_qr_code.Width, pic_qr_code.Height);

            pic_qr_code.Image = result;
            */
        }

        private void CurrentWallet_ErrorsOccured(object sender, string err)
        {
            if (Thread.CurrentThread != this.Dispatcher.Thread)
            {
                this.Dispatcher.BeginInvoke(new Action<object, string>(CurrentWallet_ErrorsOccured), sender, err);
            }
            else
            {
                StaticUtils.ShowMessageBox(StaticUtils.ErrorBrush, StringTable.GetInstance().GetString("STR_WALLET_REPAIR", iLang));
            }
            
        }

        private void ChangeWallet(UserWallet wallet)
        {
            if (Constant.CurrentWallet != null)
            {
                Constant.CurrentWallet.BalanceChanged -= this.CurrentWallet_BalanceChanged;
                Constant.CurrentWallet.TransactionsChanged -= this.CurrentWallet_TransactionsChanged;
                Constant.CurrentWallet.ErrorsOccured -= this.CurrentWallet_ErrorsOccured;
                Constant.CurrentWallet.Dispose();
            }
            Constant.CurrentWallet = wallet;

            if (Constant.CurrentWallet != null)
            {
                keys = Constant.CurrentWallet.GetKeys();
                addresses = Constant.CurrentWallet.GetAddresses();

                if (privKeys == null)
                {
                    privKeys = new List<byte[]>();
                }
                else
                {
                    privKeys.Clear();
                }

                foreach (var key in keys)
                {
                    if (key.nVersion == KeyType.Stealth)
                    {

                    }
                    else
                    {
                        using (((KeyPair)key).Decrypt())
                        {
                            byte[] privKey = new byte[((KeyPair)key).PrivateKey.Length];

                            for (int i = 0; i < ((KeyPair)key).PrivateKey.Length; i++)
                            {
                                privKey[i] = ((KeyPair)key).PrivateKey[i];
                            }

                            privKeys.Add(privKey);
                        }
                    }
                    
                }
                
                CurrentWallet_TransactionsChanged(null, Constant.CurrentWallet.LoadTransactions());
                Constant.CurrentWallet.BalanceChanged += this.CurrentWallet_BalanceChanged;
                Constant.CurrentWallet.TransactionsChanged += this.CurrentWallet_TransactionsChanged;
                Constant.CurrentWallet.ErrorsOccured += this.CurrentWallet_ErrorsOccured;
            }

            if (Constant.CurrentWallet != null)
            {
                currentVerificationContractList.Clear();
                foreach (UInt160 scriptHash in Constant.CurrentWallet.GetAddresses().ToArray())
                {
                    KeyPairBase key = Constant.CurrentWallet.GetKeyByScriptHash(scriptHash);
                    VerificationContract contract = Constant.CurrentWallet.GetContract(scriptHash);
                    
                    if (contract == null)
                    {
                        //AddAddressToListView(scriptHash);
                    }
                    else
                    {
                        currentVerificationContractList.Add(contract);
                        AddContractToListView(contract);
                    }
                }
            }
            balance_changed = true;
            check_nep5_balance = true;

            if (Constant.CurrentWallet != null)
            {
                string fromAddr = "";

                foreach (UInt160 scriptHash in Constant.CurrentWallet.GetAddresses().ToArray())
                {
                    VerificationContract contract = Constant.CurrentWallet.GetContract(scriptHash);
                    fromAddr = contract.Address;
                }

                if (Wallet.GetAddressVersion(fromAddr) != Wallet.AddressVersion)
                {
                    smartContractPage.hideAddAssetBtn();
                }
            }

            DrawQRCode();
        }

        private void ResetBalanceFields()
        {
            TxbQRSBalance.Text = "0";
            TxbQrsUsdBalance.Text = "$0";
            TxbQRGBalance.Text = "0";
            TxbQrgUsdBalance.Text = "$0";
        }

        private void AddContractToListView(VerificationContract contract, bool selected = false)
        {
            ResetBalanceFields();
        }

        private void CurrentWallet_BalanceChanged(object sender, EventArgs e)
        {
            balance_changed = true;
        }

        private void CurrentWallet_TransactionsChanged(object sender, IEnumerable<TransactionInfo> transactions)
        {
            if (Thread.CurrentThread != this.Dispatcher.Thread)
            {
                this.Dispatcher.BeginInvoke(new Action<object, IEnumerable<TransactionInfo>>(CurrentWallet_TransactionsChanged), sender, transactions);
            }
            else
            {
                if (Constant.CurrentWallet.WalletHeight - 1 <= Blockchain.Default.Height)
                {
                    foreach (TransactionInfo info in transactions)
                    {
                        string txid = info.Transaction.Hash.ToString();

                        AddTransaction(info);
                    }
                }
            }
        }

        private void AddTransaction(TransactionInfo info)
        {
            bool isAdd = true;
            for (int i = 0; i < txHistoryItems.Count; i++)
            {
                if (txHistoryItems[i]._info.Transaction.Hash == info.Transaction.Hash)
                {
                    isAdd = false;
                    txHistoryItems[i]._info = info;
                    txHistoryItems[i].RefreshInterface();
                }
            }

            if (isAdd)
            {
                HistoryItem item = new HistoryItem(info, keys, addresses, privKeys);
                item.Margin = new Thickness(20, 6, 20, 7);
                item.Height = 60;

                txHistoryItems.Add(item);
                historyPage.Add(item);
            }
            else
            {
                historyPage.Refresh(txHistoryItems);
            }
        }

        private void AddAssets(AssetState asset, Fixed8 value, Fixed8 claim)
        {
            bool isAdd = true;
            foreach (AssetItem item in assetItems)
            {
                if (item.Asset.AssetId == asset.AssetId)
                {
                    isAdd = false;
                    item.Asset = asset;
                    item.Value = value;
                    item.Claim = claim;
                    item.RefreshInterface();
                }
            }

            if (isAdd)
            {
                AssetItem item = new AssetItem(asset, value, claim);
                assetItems.Add(item);
            }

            assetInfoPage.Refresh(assetItems);

            if (AssetsManager.GetInstance().AddAssets(asset.AssetId, asset.GetName()))
            {
                sendPage.AddAsset(asset.AssetId, asset.GetName(), value);
                receivePage.AddAsset(asset.AssetId, asset.GetName(), value);
            }
            else
            {
                sendPage.RefreshAsset(asset.AssetId, value);
            }
        }

        private void ResetAssetInfoPan()
        {
            assetItems.Clear();
            assetInfoPage.Refresh(assetItems);
        }

        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            if (Constant.CurrentWallet.WalletHeight == 0)
            {
                TxbHeight.Text = $"{StringTable.GetInstance().GetString("STR_MW_HEIGHT", iLang)} : {Constant.CurrentWallet.WalletHeight}/{Blockchain.Default.Height}/{Blockchain.Default.HeaderHeight}";
            }
            else
            {
                TxbHeight.Text = $"{StringTable.GetInstance().GetString("STR_MW_HEIGHT", iLang)} : {Constant.CurrentWallet.WalletHeight - 1}/{Blockchain.Default.Height}/{Blockchain.Default.HeaderHeight}";
            }
            TxbConnected.Text = $"{StringTable.GetInstance().GetString("STR_MW_CONNECTED", iLang)} : " + Constant.LocalNode.RemoteNodeCount.ToString();

            if (Constant.LocalNode.RemoteNodeCount > 0)
            {
                TxbConnected.Foreground = new SolidColorBrush(Colors.YellowGreen);
            }
            else
            {
                TxbConnected.Foreground = new SolidColorBrush(Colors.Orange);
            }

            if (Constant.CurrentWallet.WalletHeight - 1 < Blockchain.Default.HeaderHeight - 1 || Blockchain.Default.Height < Blockchain.Default.HeaderHeight - 1)
            {
                TxbHeight.Foreground = new SolidColorBrush(Colors.Orange);
            }
            else
            {
                TxbHeight.Foreground = new SolidColorBrush(Colors.YellowGreen);
            }

            if (Constant.CurrentWallet != null)
            {
                if (Constant.CurrentWallet.WalletHeight <= Blockchain.Default.Height + 1)
                {
                    if (balance_changed)
                    {
                        IEnumerable<Coin> coins = Constant.CurrentWallet?.GetCoins().Where(p => !p.State.HasFlag(CoinState.Spent)) ?? Enumerable.Empty<Coin>();
                        IEnumerable<JSCoin> jscoins = Constant.CurrentWallet?.GetJSCoins().Where(p => !p.State.HasFlag(CoinState.Spent)) ?? Enumerable.Empty<JSCoin>();
                        IEnumerable<RCTCoin> rctcoins = Constant.CurrentWallet?.GetRCTCoins().Where(p => !p.State.HasFlag(CoinState.Spent)) ?? Enumerable.Empty<RCTCoin>();
                        List<RCTCoin> rctCoinCache = Constant.CurrentWallet?.GetRCTCoinCache();

                        Fixed8 bonus_available = Blockchain.CalculateBonus(Constant.CurrentWallet.GetUnclaimedCoins().Select(p => p.Reference));
                        Fixed8 bonus_unavailable = Blockchain.CalculateBonus(coins.Where(p => p.State.HasFlag(CoinState.Confirmed) && p.Output.AssetId.Equals(Blockchain.GoverningToken.Hash)).Select(p => p.Reference), Blockchain.Default.Height + 1);
                        Fixed8 bonus = bonus_available + bonus_unavailable;
                        var assets = coins.GroupBy(p => p.Output.AssetId, (k, g) => new
                        {
                            Asset = Blockchain.Default.GetAssetState(k),
                            Value = g.Sum(p => p.Output.Value),
                            Claim = k.Equals(Blockchain.UtilityToken.Hash) ? bonus : Fixed8.Zero
                        }).ToDictionary(p => p.Asset.AssetId);

                        var jsAssets = jscoins.GroupBy(p => p.Output.AssetId, (k, g) => new
                        {
                            Asset = Blockchain.Default.GetAssetState(k),
                            Value = g.Sum(p => p.Output.Value)
                        }).ToDictionary(p => p.Asset.AssetId);

                        var rctAssets = rctcoins.GroupBy(p => p.Output.AssetId, (k, g) => new
                        {
                            Asset = Blockchain.Default.GetAssetState(k),
                            Value = g.Sum(p => p.Output.Value)
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

                        if (assets.Count + jsAssets.Count + rctAssets.Count < assetItems.Count)
                        {
                            ResetAssetInfoPan();
                            AssetsManager.GetInstance().Reset();
                            sendPage.Reset();
                        }

                        foreach (var asset in assets.Values)
                        {
                            AddAssets(asset.Asset, asset.Value, asset.Claim);
                        }

                        foreach(var jsAsset in jsAssets.Values)
                        {
                            AddAssets(jsAsset.Asset, jsAsset.Value, Fixed8.Zero);
                        }

                        foreach(var rctAsset in rctAssets.Values)
                        {
                            AddAssets(rctAsset.Asset, rctAsset.Value, Fixed8.Zero);
                        }

                        var balance_qrs = coins.Where(p => p.Output.AssetId.Equals(Blockchain.GoverningToken.Hash)).GroupBy(p => p.Output.ScriptHash).ToDictionary(p => p.Key, p => p.Sum(i => i.Output.Value));
                        var balance_rct_qrs = rctcoins.Where(p => p.Output.AssetId.Equals(Blockchain.GoverningToken.Hash)).GroupBy(p => p.Output.ScriptHash).ToDictionary(p => p.Key, p => p.Sum(i => i.Output.Value));
                        var balance_js_qrs = jscoins.Where(p => p.Output.AssetId.Equals(Blockchain.GoverningToken.Hash)).GroupBy(p => p.Output.ScriptHash).ToDictionary(p => p.Key, p => p.Sum(i => i.Output.Value));
                        var balance_qrg = coins.Where(p => p.Output.AssetId.Equals(Blockchain.UtilityToken.Hash)).GroupBy(p => p.Output.ScriptHash).ToDictionary(p => p.Key, p => p.Sum(i => i.Output.Value));
                        var balance_rct_qrg = rctcoins.Where(p => p.Output.AssetId.Equals(Blockchain.UtilityToken.Hash)).GroupBy(p => p.Output.ScriptHash).ToDictionary(p => p.Key, p => p.Sum(i => i.Output.Value));
                        var balance_js_qrg = jscoins.Where(p => p.Output.AssetId.Equals(Blockchain.UtilityToken.Hash)).GroupBy(p => p.Output.ScriptHash).ToDictionary(p => p.Key, p => p.Sum(i => i.Output.Value));
                        Fixed8 qrs_total = Fixed8.Zero;
                        Fixed8 qrg_total = Fixed8.Zero;
                        foreach (UInt160 scriptHash in Constant.CurrentWallet.GetAddresses().ToArray())
                        {
                            VerificationContract contract = Constant.CurrentWallet.GetContract(scriptHash);

                            Fixed8 qrs = balance_qrs.ContainsKey(scriptHash) ? balance_qrs[scriptHash] : Fixed8.Zero;
                            Fixed8 js_qrs = balance_js_qrs.ContainsKey(scriptHash) ? balance_js_qrs[scriptHash] : Fixed8.Zero;
                            Fixed8 rct_qrs = balance_rct_qrs.ContainsKey(scriptHash) ? balance_rct_qrs[scriptHash] : Fixed8.Zero;
                            Fixed8 qrg = balance_qrg.ContainsKey(scriptHash) ? balance_qrg[scriptHash] : Fixed8.Zero;
                            Fixed8 js_qrg = balance_js_qrg.ContainsKey(scriptHash) ? balance_js_qrg[scriptHash] : Fixed8.Zero;
                            Fixed8 rct_qrg = balance_rct_qrg.ContainsKey(scriptHash) ? balance_rct_qrg[scriptHash] : Fixed8.Zero;

                            AddressAssetsImp.getInstance().RefrshAssets(contract.Address, Blockchain.GoverningToken.Hash, (qrs + js_qrs).ToString());
                            AddressAssetsImp.getInstance().RefrshAssets(contract.Address, Blockchain.UtilityToken.Hash, (qrg + js_qrg).ToString());

                            qrs_total += qrs;
                            qrs_total += js_qrs;
                            qrs_total += rct_qrs;
                            qrg_total += qrg;
                            qrg_total += js_qrg;
                            qrg_total += rct_qrg;
                        }

                        foreach(RCTCoin coin in rctCoinCache)
                        {
                            if (coin.Output.AssetId == Blockchain.GoverningToken.Hash)
                                qrs_total += coin.Output.Value;
                            else if (coin.Output.AssetId == Blockchain.UtilityToken.Hash)
                                qrg_total += coin.Output.Value;
                               
                        }

                        TxbQRSBalance.Text = Helper.FormatNumber(qrs_total.ToString());
                        TxbQRGBalance.Text = Helper.FormatNumber(qrg_total.ToString());

                        TxbQrsUsdBalance.Text = $"${Helper.FormatNumber((qrs_total * Fixed8.Satoshi * 8000000).ToString())}";
                        TxbQrgUsdBalance.Text = $"${Helper.FormatNumber((qrg_total * Fixed8.Satoshi * 2000000).ToString())}";

                        balance_changed = false;
                    }
                }
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

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow = this;

            Constant.NotifyMessageMgr = new Dialogs.NotifyMessage.NotifyMessageManager
                (
                    this.Left,
                    this.Top,
                    Width,
                    Height,
                    400,
                    30
                );
            Constant.NotifyMessageMgr.Start();

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
                Constant.LocalNode.Start(SettingsConfig.Default.NodePort, SettingsConfig.Default.WsPort);
            });
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (isLogOut)
            {
                Constant.CurrentWallet.Dispose();
                Constant.NotifyMessageMgr.Stop();
            }
            else
            {
                Constant.CurrentWallet.Dispose();
                Constant.NotifyMessageMgr.Stop();

                using (FileStream fs = new FileStream(Constant.PEER_STATE_PATH, FileMode.Create, FileAccess.Write, FileShare.None))
                {
                    LocalNode.SaveState(fs);
                }

                Constant.LocalNode.Dispose();
                Blockchain.Default.Dispose();
            }
        }

        private void btnSend_Click(object sender, RoutedEventArgs e)
        {
            if (currentSendOrReceivePageStatus == SendOrReceivePageStatus.SendPage) return; 

            currentSendOrReceivePageStatus = SendOrReceivePageStatus.SendPage;
            pageTransitionControl.ShowPage(sendPage);
        }

        private void btnReceive_Click(object sender, RoutedEventArgs e)
        {
            if (currentSendOrReceivePageStatus == SendOrReceivePageStatus.ReceivePage) return;

            currentSendOrReceivePageStatus = SendOrReceivePageStatus.ReceivePage;
            pageTransitionControl.ShowPage(receivePage);
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (Constant.NotifyMessageMgr != null)
            {
                if (Application.Current.MainWindow.WindowState == WindowState.Maximized)
                {
                    Constant.NotifyMessageMgr.ResetNotifyMessageLocations(0, 0, this.ActualWidth, this.ActualHeight, 400, 30);
                }
                else
                {
                    Constant.NotifyMessageMgr.ResetNotifyMessageLocations(this.Left, this.Top, this.ActualWidth, this.ActualHeight, 400, 30);
                }
            }
        }

        private void Window_LocationChanged(object sender, EventArgs e)
        {
            if (Constant.NotifyMessageMgr != null)
            {
                Constant.NotifyMessageMgr.ResetNotifyMessageLocations(this.Left, this.Top, this.ActualWidth, this.ActualHeight, 400, 30);
            }
        }

        private void Window_Activated(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// Copy the current wallet's address
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCopyAddress_Click(object sender, RoutedEventArgs e)
        {
            string copiedAddress = "";
            foreach (UInt160 scriptHash in Constant.CurrentWallet.GetAddresses().ToArray())
            {
                VerificationContract contract = Constant.CurrentWallet.GetContract(scriptHash);

                copiedAddress += contract.Address;

                copiedAddress += ",";
            }

            // Remove ',' from copiedAddress
            copiedAddress = copiedAddress.Substring(0, copiedAddress.Length - 1);

            try
            {
                Clipboard.SetText(copiedAddress);
            }
            catch (ArgumentNullException ex)
            {
                StaticUtils.ShowMessageBox(StaticUtils.ErrorBrush, StringTable.GetInstance().GetString("STR_ERR_ADDR_COPY", iLang));
                return;
            }
            catch (Exception ex)
            {

            }
            StaticUtils.ShowMessageBox(StaticUtils.GreenBrush, StringTable.GetInstance().GetString("STR_MW_ADDRESS_COPIED_SUCCESS", iLang));
        }

        private void UpdateDownloadedFinishedEvent(object send, string downloadPath)
        {
            DirectoryInfo di = new DirectoryInfo("update");
            if (di.Exists) di.Delete(true);
            di.Create();
            ZipFile.ExtractToDirectory(downloadPath, di.Name);
            FileSystemInfo[] fs = di.GetFileSystemInfos();
            if (fs.Length == 1 && fs[0] is DirectoryInfo)
            {
                ((DirectoryInfo)fs[0]).MoveTo("update2");
                di.Delete();
                Directory.Move("update2", di.Name);
            }
            File.WriteAllBytes("update.bat", Quras_gui_wpf.Properties.Resources.update);
            Close();
            Process.Start("update.bat");
        }

        private void ChangeLanguageEvent(object sender, EventArgs e)
        {
            RefreshLanguage();
        }

        private void ResetHistoryEvent(object sender, EventArgs e)
        {
            ResetBalanceFields();
            historyPage.Reset();
            txHistoryItems.Clear();
            ResetAssetInfoPan();

            sendPage.Reset();
        }
        
        private void TaskChangedEvent(object sender, TaskMessage task)
        {
            taskInfoPage.AddTask(task);
        }

        private void btnSettings_Click(object sender, RoutedEventArgs e)
        {
            if (currentPageStatus == PageStatus.SettingPage) return;

            currentPageStatus = PageStatus.SettingPage;
            pageMainTransitionControl.ShowPage(settingsPage);
        }

        private void btnHistory_Click(object sender, RoutedEventArgs e)
        {
            if (currentPageStatus == PageStatus.HistoryPage) return;

            currentPageStatus = PageStatus.HistoryPage;
            pageMainTransitionControl.ShowPage(historyPage);
        }

        private void btnSmartContract_Click(object sender, RoutedEventArgs e)
        {
            if (currentPageStatus == PageStatus.SmartContractPage) return;

            currentPageStatus = PageStatus.SmartContractPage;
            pageMainTransitionControl.ShowPage(smartContractPage);
        }

        private void btnMenu_Click(object sender, RoutedEventArgs e)
        {
            isMenuOpen = !isMenuOpen;
            if (isMenuOpen)
            {
                Storyboard sb = this.FindResource("OpenMenu") as Storyboard;
                sb.Begin();
            }
            else
            {
                Storyboard sb = this.FindResource("CloseMenu") as Storyboard;
                sb.Begin();
            }
        }

        private void ShowCurrentMenu()
        {
            switch(currentMenuPageStatus)
            {
                case MenuPageStatus.AssetPage:
                    {
                        btnAsset.IsChecked = true;
                        btnTasks.IsChecked = false;
                        pageMenu.ShowPage(assetInfoPage);
                        break;
                    }
                case MenuPageStatus.TaskPage:
                    {
                        btnAsset.IsChecked = false;
                        btnTasks.IsChecked = true;
                        pageMenu.ShowPage(taskInfoPage);
                        break;
                    }
                default:
                    {
                        break;
                    }
            }
        }

        private void btnAsset_Click(object sender, RoutedEventArgs e)
        {
            btnAsset.IsChecked = true;
            if (currentMenuPageStatus == MenuPageStatus.AssetPage)
            {
                return;
            }

            currentMenuPageStatus = MenuPageStatus.AssetPage;
            ShowCurrentMenu();
        }

        private void btnTasks_Click(object sender, RoutedEventArgs e)
        {
            btnTasks.IsChecked = true;
            if (currentMenuPageStatus == MenuPageStatus.TaskPage)
            {
                return;
            }

            currentMenuPageStatus = MenuPageStatus.TaskPage;
            ShowCurrentMenu();
        }

        private void btnLogOut_Click(object sender, RoutedEventArgs e)
        {
            WelcomeWindow WelcomeWnd = new WelcomeWindow();
            WelcomeWnd.Show();
            isLogOut = true;
            AddressAssetsImp.getInstance().Reset();
            AssetsManager.GetInstance().Reset();
            this.Close();
        }
    }
}
