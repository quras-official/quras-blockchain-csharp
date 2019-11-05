using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Pure;
using Pure.Core;
using Pure.Core.Anonoymous;
using Pure.Cryptography;
using Pure.Wallets;

using Quras_gui.FormUI;

namespace Quras_gui.Global
{
    class SendCoinThread
    {
        public string FromAddr;
        public string ToAddr;
        public string Amount;
        public object Asset;

        public void DoWork()
        {
            Global.AssetDescriptor asset = Asset as Global.AssetDescriptor;
            string fromAddress = FromAddr;
            string toAddress = ToAddr;
            string strAmount = Amount;
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
            if (amount.GetData() % (long)Math.Pow(10, 8 - (Asset as Global.AssetDescriptor).Precision) != 0)
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

                        /*
                        int dstOffset = 0;
                        byte[] byJsBody = new byte[ctx_.byJoinSplit.GetListLength()];
                        for (int index = 0; index < ctx_.byJoinSplit.Count; index++)
                        {
                            Buffer.BlockCopy(ctx_.byJoinSplit[index], 0, byJsBody, dstOffset, ctx_.byJoinSplit[index].Length);
                            dstOffset += ctx_.byJoinSplit[index].Length;
                        }

                        UInt256 jsHash = new UInt256(Crypto.Default.Hash256(byJsBody));
                        */

                        ctx_.joinSplitSig = Sodium.PublicKeyAuth.SignDetached(ctx.JsHash.ToArray(), joinSplitPrivKey_);

                        if (!Sodium.PublicKeyAuth.VerifyDetached(ctx_.joinSplitSig, ctx.JsHash.ToArray(), joinSplitPubKey_.ToArray()))
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
                            for (int ji = jsIndex - 1; ji > -1; ji--)
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

                    /*
                    int dstOffset = 0;
                    byte[] byJsBody = new byte[ctx.byJoinSplit.GetListLength()];
                    for (int index = 0; index < ctx.byJoinSplit.Count; index++)
                    {
                        Buffer.BlockCopy(ctx.byJoinSplit[index], 0, byJsBody, dstOffset, ctx.byJoinSplit[index].Length);
                        dstOffset += ctx.byJoinSplit[index].Length;
                    }

                    UInt256 jsHash = new UInt256(Crypto.Default.Hash256(byJsBody));
                    */

                    //UInt256 tmp_jsHash = ctx.JsHash;

                    ctx.joinSplitSig = Sodium.PublicKeyAuth.SignDetached(ctx.JsHash.ToArray(), joinSplitPrivKey_);

                    if (!Sodium.PublicKeyAuth.VerifyDetached(ctx.joinSplitSig, ctx.JsHash.ToArray(), joinSplitPubKey_.ToArray()))
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

                            for (int jo = 0; jo < info.vjsout.Count; jo++)
                            {
                                jsInfo.vjsout.Add(info.vjsout[jo]);
                            }

                            try
                            {
                                tx = Constant.CurrentWallet.Perform_JoinSplit(ctx, jsInfo, joinSplitPubKey_, joinSplitPrivKey_, (UInt256)asset.AssetId, vectorWitness, jsAnchor);
                            }
                            catch (Exception ex)
                            {
                                string strException = ex.Message;
                            }

                            if (tx.Inputs == null)
                            {
                                tx.Inputs = new CoinReference[0];
                            }

                            jsIndex = 0;
                            SnarkDllApi.Witnesses_Clear(vectorWitness);
                        }
                    }

                    /*
                    int dstOffset = 0;
                    byte[] byJsBody = new byte[ctx.byJoinSplit.GetListLength()];
                    for (int index = 0; index < ctx.byJoinSplit.Count; index++)
                    {
                        Buffer.BlockCopy(ctx.byJoinSplit[index], 0, byJsBody, dstOffset, ctx.byJoinSplit[index].Length);
                        dstOffset += ctx.byJoinSplit[index].Length;
                    }

                    UInt256 jsHash = new UInt256(Crypto.Default.Hash256(byJsBody));
                    */

                    ctx.joinSplitSig = Sodium.PublicKeyAuth.SignDetached(ctx.JsHash.ToArray(), joinSplitPrivKey_);

                    if (!Sodium.PublicKeyAuth.VerifyDetached(ctx.joinSplitSig, ctx.JsHash.ToArray(), joinSplitPubKey_.ToArray()))
                    {
                        return;
                    }
                }
                FormUI.Helper.SignAndShowInformation(tx);
            }
        }

    }
}
