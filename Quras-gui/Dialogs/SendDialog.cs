using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Runtime.InteropServices;

using MaterialSkin;
using MaterialSkin.Controls;

using Pure;
using Pure.Core;
using Pure.Core.Anonoymous;
using Pure.Cryptography;
using Pure.Wallets;
using Quras_gui.Global;
using Pure.IO;
using Quras_gui.FormUI;

namespace Quras_gui.Dialogs
{
    public partial class SendDialog : MaterialForm
    {
        private int iLang => Constant.GetLang();

        public SendDialog()
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

            InitInterface();
            InitInstance();
        }

        private void InitInterface()
        {
            lbl_warning.Hide();

            this.Text = StringTable.DATA[iLang, 49];
            lbl_from_addr.Text = StringTable.DATA[iLang, 50];
            lbl_reciepent_addr.Text = StringTable.DATA[iLang, 51];
            lbl_addr_comment.Text = StringTable.DATA[iLang, 52];
            lbl_amount.Text = StringTable.DATA[iLang, 53];
            lbl_cmt_asset.Text = StringTable.DATA[iLang, 54];
            lbl_max.Text = StringTable.DATA[iLang, 55];
            lbl_cmt_amount.Text = StringTable.DATA[iLang, 56];
            btn_send.Text = StringTable.DATA[iLang, 57];
        }

        private void InitInstance()
        {
            // From Combobox initialize
            foreach (UInt160 scriptHash in Constant.CurrentWallet.GetAddresses().ToArray())
            {
                VerificationContract contract = Constant.CurrentWallet.GetContract(scriptHash);
                cmb_from.Items.Add(contract.Address);
            }

            if (cmb_from.Items.Count > 0)
            {
                cmb_from.SelectedIndex = 0;
            }

            // Load Assets
            LoadAssets();

            // Load Max Amount
            LoadMaxAmount();
        }

        private void LoadMaxAmount()
        {
            string FromAddr = cmb_from.Text;
            Global.AssetDescriptor asset = cmb_assets.SelectedItem as Global.AssetDescriptor;

            if (asset == null)
            {
                return;
            }

            UInt256 AssetID = new UInt256(asset.AssetId.ToArray());

            if (AddressAssetsImp.getInstance().GetList().ContainsKey(FromAddr))
            {
                if (AddressAssetsImp.getInstance().GetList()[FromAddr].Assets.ContainsKey(AssetID))
                {
                    txb_max_amount.Text = AddressAssetsImp.getInstance().GetList()[FromAddr].Assets[AssetID]  + " " + cmb_assets.SelectedItem;
                    return;
                }
            }

            txb_max_amount.Text = "0 " + cmb_assets.SelectedItem;
        }

        private void LoadAssets()
        {
            for (int i = 0; i < AssetsImp.getInstance().GetList().Count; i++)
            {
                AssetState state = Blockchain.Default.GetAssetState(AssetsImp.getInstance().GetList()[i].Asset_ID);
                Global.AssetDescriptor item = new Global.AssetDescriptor(state);

                cmb_assets.Items.Add(item);
            }

            if (cmb_assets.Items.Count > 0)
            {
                cmb_assets.SelectedIndex = 0;
            }
        }

        private void cmb_from_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadMaxAmount();
        }

        private void cmb_assets_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadMaxAmount();
        }

        public string GetErrorMessage()
        {
            string ret = "";
            if (cmb_from.Text.Length == 0)
            {
                ret = StringTable.DATA[iLang, 58];
                return ret;
            }

            try
            {
                Pure.Wallets.Wallet.GetAddressVersion(cmb_from.Text);
            }
            catch
            {
                ret = StringTable.DATA[iLang, 59];
                return ret;
            }

            if (txb_recieve_addr.Text.Length == 0)
            {
                ret = StringTable.DATA[iLang, 60];
                return ret;
            }

            if (txb_recieve_addr.Text == cmb_from.Text)
            {
                ret = StringTable.DATA[iLang, 61];
                return ret;
            }

            try
            {
                Pure.Wallets.Wallet.GetAddressVersion(txb_recieve_addr.Text);
            }
            catch
            {
                ret = StringTable.DATA[iLang, 62];
                return ret;
            }

            if (cmb_assets.SelectedItem == null)
            {
                ret = StringTable.DATA[iLang, 63];
                return ret;
            }

            if (txb_amount.Text.Length == 0)
            {
                ret = StringTable.DATA[iLang, 64];
                return ret;
            }

            if (!Fixed8.TryParse(txb_amount.Text, out Fixed8 amount))
            {
                ret = StringTable.DATA[iLang, 65];
                return ret;
            }
            if (amount == Fixed8.Zero)
            {
                ret = StringTable.DATA[iLang, 66];
                return ret;
            }

            Fixed8 max_balance = Fixed8.Zero;

            string FromAddr = cmb_from.Text;
            string MaxAmount = "0";

            Global.AssetDescriptor asset = cmb_assets.SelectedItem as Global.AssetDescriptor;

            if (asset == null)
            {
                ret = StringTable.DATA[iLang, 67];
                return ret;
            }

            UInt256 AssetID = new UInt256(asset.AssetId.ToArray());

            if (AddressAssetsImp.getInstance().GetList().ContainsKey(FromAddr))
            {
                if (AddressAssetsImp.getInstance().GetList()[FromAddr].Assets.ContainsKey(AssetID))
                {
                    MaxAmount = AddressAssetsImp.getInstance().GetList()[FromAddr].Assets[AssetID];
                }
            }

            max_balance = Fixed8.One * Fixed8.Parse(MaxAmount);

            if (amount > max_balance)
            {
                ret = StringTable.DATA[iLang, 68];
                return ret;
            }

            if (Wallet.GetAddressVersion(cmb_from.Text) == Wallet.AnonymouseAddressVersion ||
                Wallet.GetAddressVersion(txb_recieve_addr.Text) == Wallet.AnonymouseAddressVersion)
            {
                if (Constant.bSnarksParamLoaded == false)
                {
                    ret = StringTable.DATA[iLang, 69];
                    return ret;
                }
            }

            return ret;
        }

        private void btn_send_Click(object sender, EventArgs e)
        {
            string ret = GetErrorMessage();
            if (ret.Length > 0)
            {
                lbl_warning.Text = ret;
                lbl_warning.Show();
            }
            else
            {
                lbl_warning.Hide();
                //SendCoins();
                this.DialogResult = DialogResult.OK;
            }
        }

        public object GetAssetID()
        {
            Global.AssetDescriptor asset = cmb_assets.SelectedItem as Global.AssetDescriptor;
            return asset;
        }

        public string GetFromAddress()
        {

            return cmb_from.Text;
        }

        public string GetToAddress()
        {
            return txb_recieve_addr.Text;
        }

        public string GetAmount()
        {
            return txb_amount.Text;
        }

        private void SendCoins()
        {
            Global.AssetDescriptor asset = cmb_assets.SelectedItem as Global.AssetDescriptor;
            string fromAddress = cmb_from.Text;
            string toAddress = txb_recieve_addr.Text;
            string strAmount = txb_amount.Text;
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
            if (amount.GetData() % (long)Math.Pow(10, 8 - (cmb_assets.SelectedItem as Global.AssetDescriptor).Precision) != 0)
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
                FormUI.Helper.SignAndShowInformation(tx);
            }
            else if (fromAddrVersion == Wallet.AddressVersion && toAddrVersion == Wallet.AnonymouseAddressVersion)
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

                        IntPtr ptrRoot = SnarkDllApi.GetCMRoot(Blockchain.Default.GetCmMerkleTree());

                        byte[] byRoot = new byte[32];
                        System.Runtime.InteropServices.Marshal.Copy(ptrRoot, byRoot, 0, 32);
                        UInt256 anchor = new UInt256(byRoot);

                        tx = Constant.CurrentWallet.Perform_JoinSplit(ctx_, info, joinSplitPubKey_, joinSplitPrivKey_, (UInt256)asset.AssetId, w, anchor);

                        int dstOffset = 0;
                        byte[] byJsBody = new byte[ctx_.byJoinSplit.GetListLength()];
                        for (int index = 0; index < ctx_.byJoinSplit.Count; index++)
                        {
                            Buffer.BlockCopy(ctx_.byJoinSplit[index], 0, byJsBody, dstOffset, ctx_.byJoinSplit[index].Length);
                            dstOffset += ctx_.byJoinSplit[index].Length;
                        }

                        UInt256 jsHash = new UInt256(Crypto.Default.Hash256(byJsBody));

                        ctx_.joinSplitSig = Sodium.PublicKeyAuth.SignDetached(jsHash.ToArray(), joinSplitPrivKey_);

                        if (!Sodium.PublicKeyAuth.VerifyDetached(ctx_.joinSplitSig, jsHash.ToArray(), joinSplitPubKey_.ToArray()))
                        {
                            return;
                        }
                    }
                }

                FormUI.Helper.SignAndShowInformation(tx);
            }
            else if (fromAddrVersion == Wallet.AnonymouseAddressVersion && toAddrVersion == Wallet.AddressVersion)
            {
                UInt256 joinSplitPubKey_;
                byte[] joinSplitPrivKey_;

                List<TransactionAttribute> attributes = new List<TransactionAttribute>();

                tx = new AnonymousContractTransaction();

                Fixed8 vpubNewTarget = Fixed8.Zero;
                Fixed8 totalAmount = amount;

                tx.Attributes = attributes.ToArray();

                Sodium.KeyPair keyPair;
                keyPair = Sodium.PublicKeyAuth.GenerateKeyPair();

                joinSplitPubKey_ = new UInt256(keyPair.PublicKey);
                joinSplitPrivKey_ = keyPair.PrivateKey;

                ((AnonymousContractTransaction)tx).joinSplitPubKey = joinSplitPubKey_;

                // Do process the transparent outputs.
                TransactionOutput outPut = new TransactionOutput();
                outPut.ScriptHash = Wallet.ToScriptHash(toAddress);
                outPut.Value = amount;
                outPut.AssetId = (UInt256)asset.AssetId;
                tx.Outputs = new TransactionOutput[1];
                tx.Outputs[0] = outPut;

                tx.Scripts = new Witness[0];

                vpubNewTarget = amount;

                AsyncJoinSplitInfo info = new AsyncJoinSplitInfo();
                info.vpub_old = Fixed8.Zero;
                info.vpub_new = Fixed8.Zero;

                Fixed8 jsInputValue = Fixed8.Zero;
                

                IntPtr ptrRoot = SnarkDllApi.GetCMRoot(Blockchain.Default.GetCmMerkleTree());

                byte[] byRoot = new byte[32];
                System.Runtime.InteropServices.Marshal.Copy(ptrRoot, byRoot, 0, 32);
                UInt256 jsAnchor = new UInt256(byRoot);

                if (tx is AnonymousContractTransaction ctx)
                {
                    tx = Constant.CurrentWallet.MakeAandTTransaction(ctx, fromAddress, info);
                  
                    IntPtr vectorWitness = SnarkDllApi.Witnesses_Create();

                    int jsIndex = 0;
                    Fixed8 current_inputed_amount = Fixed8.Zero;
                    Fixed8 rest_amount = totalAmount;

                    for (int i = 0; i < info.vjsin.Count; i ++)
                    {
                        IntPtr witness = SnarkDllApi.CmWitness_Create();

                        SnarkDllApi.SetCMWitnessFromBinary(witness, info.vjsin[i].witness, info.vjsin[i].witness.Length);
                        SnarkDllApi.Witnesses_Add(vectorWitness, witness);

                        IntPtr ptrWitness = SnarkDllApi.GetCMRootFromWitness(witness);
                        byte[] byWRoot = new byte[32];
                        System.Runtime.InteropServices.Marshal.Copy(ptrRoot, byWRoot, 0, 32);
                        UInt256 wAnchor = new UInt256(byWRoot);

                        if (jsAnchor != wAnchor)
                        {
                            throw new InvalidOperationException("Anchor is not correct");
                        }

                        current_inputed_amount += info.vjsin[i].note.value;

                        jsIndex++;

                        if (jsIndex == 2 && i != info.vjsin.Count - 1)
                        {
                            AsyncJoinSplitInfo jsInfo = new AsyncJoinSplitInfo();
                            jsInfo.vjsin.Add(info.vjsin[i - 1]);
                            jsInfo.vjsin.Add(info.vjsin[i]);

                            jsInfo.notes.Add(info.notes[i - 1]);
                            jsInfo.notes.Add(info.notes[i]);

                            rest_amount -= jsInfo.notes[0].value;
                            rest_amount -= jsInfo.notes[1].value;

                            if (rest_amount > Fixed8.Zero)
                            {
                                jsInfo.vpub_new = info.vjsin[i - 1].note.value + info.vjsin[i].note.value;
                                jsInfo.vpub_old = Fixed8.Zero;
                            }
                            else
                            {
                                //JSOutput jso = new JSOutput(Wallet.ToPaymentAddress(toAddress), jsInputedAmount, (UInt256)asset.AssetId);
                                //JSOutput jso_remain = new JSOutput(Wallet.ToPaymentAddress(fromAddress), -rest_amount, (UInt256)asset.AssetId);
                                //jsInfo.vjsout.Add(jso);
                                //jsInfo.vjsout.Add(jso_remain);
                            }
                            
                            tx = Constant.CurrentWallet.Perform_JoinSplit(ctx, jsInfo, joinSplitPubKey_, joinSplitPrivKey_, (UInt256)asset.AssetId, vectorWitness, jsAnchor);

                            jsIndex = 0;
                            SnarkDllApi.Witnesses_Clear(vectorWitness);
                        }

                        if (i == info.vjsin.Count - 1)
                        {
                            AsyncJoinSplitInfo jsInfo = new AsyncJoinSplitInfo();

                            Fixed8 jsInputedAmount = Fixed8.Zero;
                            for (int ji = jsIndex - 1; ji > -1; ji --)
                            {
                                jsInfo.vjsin.Add(info.vjsin[i - ji]);
                                jsInfo.notes.Add(info.notes[i - ji]);

                                jsInputedAmount += info.notes[i - ji].value;
                                rest_amount -= info.notes[i - ji].value;
                            }

                            if (rest_amount < Fixed8.Zero)
                            {
                                JSOutput jso_remain = new JSOutput(Wallet.ToPaymentAddress(fromAddress), -rest_amount, (UInt256)asset.AssetId);

                                jsInfo.vpub_new = jsInputedAmount + rest_amount;
                                jsInfo.vpub_old = Fixed8.Zero;

                                jsInfo.vjsout.Add(jso_remain);
                            }

                            tx = Constant.CurrentWallet.Perform_JoinSplit(ctx, jsInfo, joinSplitPubKey_, joinSplitPrivKey_, (UInt256)asset.AssetId, vectorWitness, jsAnchor);

                            if (tx.Inputs == null)
                            {
                                tx.Inputs = new CoinReference[0];
                            }
                            
                            jsIndex = 0;
                            SnarkDllApi.Witnesses_Clear(vectorWitness);
                        }
                    }

                    int dstOffset = 0;
                    byte[] byJsBody = new byte[ctx.byJoinSplit.GetListLength()];
                    for (int index = 0; index < ctx.byJoinSplit.Count; index++)
                    {
                        Buffer.BlockCopy(ctx.byJoinSplit[index], 0, byJsBody, dstOffset, ctx.byJoinSplit[index].Length);
                        dstOffset += ctx.byJoinSplit[index].Length;
                    }

                    UInt256 jsHash = new UInt256(Crypto.Default.Hash256(byJsBody));

                    ctx.joinSplitSig = Sodium.PublicKeyAuth.SignDetached(jsHash.ToArray(), joinSplitPrivKey_);

                    if (!Sodium.PublicKeyAuth.VerifyDetached(ctx.joinSplitSig, jsHash.ToArray(), joinSplitPubKey_.ToArray()))
                    {
                        return;
                    }
                }
                FormUI.Helper.SignAndShowInformation(tx);
            }
            else if (fromAddrVersion == Wallet.AnonymouseAddressVersion && toAddrVersion == Wallet.AnonymouseAddressVersion)
            {
                UInt256 joinSplitPubKey_;
                byte[] joinSplitPrivKey_;

                List<TransactionAttribute> attributes = new List<TransactionAttribute>();

                tx = new AnonymousContractTransaction();

                Fixed8 totalAmount = amount;

                tx.Scripts = new Witness[0];

                tx.Attributes = attributes.ToArray();

                Sodium.KeyPair keyPair;
                keyPair = Sodium.PublicKeyAuth.GenerateKeyPair();

                joinSplitPubKey_ = new UInt256(keyPair.PublicKey);
                joinSplitPrivKey_ = keyPair.PrivateKey;

                ((AnonymousContractTransaction)tx).joinSplitPubKey = joinSplitPubKey_;

                AsyncJoinSplitInfo info = new AsyncJoinSplitInfo();
                info.vpub_old = Fixed8.Zero;
                info.vpub_new = Fixed8.Zero;

                JSOutput jsOut = new JSOutput(Wallet.ToPaymentAddress(toAddress), amount, (UInt256)asset.AssetId);

                info.vjsout.Add(jsOut);

                Fixed8 jsInputValue = Fixed8.Zero;


                IntPtr ptrRoot = SnarkDllApi.GetCMRoot(Blockchain.Default.GetCmMerkleTree());

                byte[] byRoot = new byte[32];
                System.Runtime.InteropServices.Marshal.Copy(ptrRoot, byRoot, 0, 32);
                UInt256 jsAnchor = new UInt256(byRoot);

                if (tx is AnonymousContractTransaction ctx)
                {
                    tx = Constant.CurrentWallet.MakeAandATransaction(ctx, fromAddress, info);

                    IntPtr vectorWitness = SnarkDllApi.Witnesses_Create();

                    int jsIndex = 0;
                    Fixed8 current_inputed_amount = Fixed8.Zero;
                    Fixed8 rest_amount = totalAmount;

                    for (int i = 0; i < info.vjsin.Count; i++)
                    {
                        IntPtr witness = SnarkDllApi.CmWitness_Create();

                        SnarkDllApi.SetCMWitnessFromBinary(witness, info.vjsin[i].witness, info.vjsin[i].witness.Length);
                        SnarkDllApi.Witnesses_Add(vectorWitness, witness);

                        IntPtr ptrWitness = SnarkDllApi.GetCMRootFromWitness(witness);
                        byte[] byWRoot = new byte[32];
                        System.Runtime.InteropServices.Marshal.Copy(ptrRoot, byWRoot, 0, 32);
                        UInt256 wAnchor = new UInt256(byWRoot);

                        if (jsAnchor != wAnchor)
                        {
                            throw new InvalidOperationException("Anchor is not correct");
                        }

                        current_inputed_amount += info.vjsin[i].note.value;

                        jsIndex++;

                        if (jsIndex == 2 && i != info.vjsin.Count - 1)
                        {
                            AsyncJoinSplitInfo jsInfo = new AsyncJoinSplitInfo();
                            jsInfo.vjsin.Add(info.vjsin[i - 1]);
                            jsInfo.vjsin.Add(info.vjsin[i]);

                            jsInfo.notes.Add(info.notes[i - 1]);
                            jsInfo.notes.Add(info.notes[i]);

                            tx = Constant.CurrentWallet.Perform_JoinSplit(ctx, jsInfo, joinSplitPubKey_, joinSplitPrivKey_, (UInt256)asset.AssetId, vectorWitness, jsAnchor);

                            jsIndex = 0;
                            SnarkDllApi.Witnesses_Clear(vectorWitness);
                        }

                        if (i == info.vjsin.Count - 1)
                        {
                            AsyncJoinSplitInfo jsInfo = new AsyncJoinSplitInfo();

                            Fixed8 jsInputedAmount = Fixed8.Zero;
                            for (int ji = jsIndex - 1; ji > -1; ji--)
                            {
                                jsInfo.vjsin.Add(info.vjsin[i - ji]);
                                jsInfo.notes.Add(info.notes[i - ji]);

                                jsInputedAmount += info.notes[i - ji].value;
                                rest_amount -= info.notes[i - ji].value;
                            }

                            for (int jo = 0; jo < info.vjsout.Count; jo ++)
                            {
                                jsInfo.vjsout.Add(info.vjsout[jo]);
                            }

                            tx = Constant.CurrentWallet.Perform_JoinSplit(ctx, jsInfo, joinSplitPubKey_, joinSplitPrivKey_, (UInt256)asset.AssetId, vectorWitness, jsAnchor);

                            if (tx.Inputs == null)
                            {
                                tx.Inputs = new CoinReference[0];
                            }

                            jsIndex = 0;
                            SnarkDllApi.Witnesses_Clear(vectorWitness);
                        }
                    }

                    int dstOffset = 0;
                    byte[] byJsBody = new byte[ctx.byJoinSplit.GetListLength()];
                    for (int index = 0; index < ctx.byJoinSplit.Count; index++)
                    {
                        Buffer.BlockCopy(ctx.byJoinSplit[index], 0, byJsBody, dstOffset, ctx.byJoinSplit[index].Length);
                        dstOffset += ctx.byJoinSplit[index].Length;
                    }

                    UInt256 jsHash = new UInt256(Crypto.Default.Hash256(byJsBody));

                    ctx.joinSplitSig = Sodium.PublicKeyAuth.SignDetached(jsHash.ToArray(), joinSplitPrivKey_);

                    if (!Sodium.PublicKeyAuth.VerifyDetached(ctx.joinSplitSig, jsHash.ToArray(), joinSplitPubKey_.ToArray()))
                    {
                        return;
                    }
                }
                FormUI.Helper.SignAndShowInformation(tx);
            }
        }

        private void btn_copy_Click(object sender, EventArgs e)
        {
            try
            {
                Clipboard.SetText(cmb_from.Text);
            }
            catch (ExternalException) { }
        }
    }
}
