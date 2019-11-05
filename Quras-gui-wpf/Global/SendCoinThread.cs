using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Pure;
using Pure.Core;
using Pure.Core.Anonoymous;
using Pure.Core.RingCT.Types;
using Pure.Core.RingCT.Impls;
using Pure.Cryptography;
using Pure.Wallets;

using Quras_gui_wpf.Utils;

namespace Quras_gui_wpf.Global
{
    public class SendCoinThread
    {
        public string FromAddr;
        public string ToAddr;
        public string Amount;
        public object Asset;
        public string Fee;
        public event EventHandler<TaskMessage> TaskChangedEvent;
        public LANG iLang => Constant.GetLang();

        public void DoWork()
        {
            #region Checking Fields
            Global.AssetDescriptor asset = Asset as Global.AssetDescriptor;
            string fromAddress = FromAddr;
            string toAddress = ToAddr;
            string strAmount = Amount;
            string strFee = Fee;
            byte toAddrVersion;
            byte fromAddrVersion;

            if (asset == null)
            {
                throw new InvalidOperationException("Anchor is not correct");
            }

            if (!Fixed8.TryParse(strAmount, out Fixed8 amount))
            {
                throw new InvalidOperationException("Anchor is not correct");
            }
            if (amount == Fixed8.Zero)
            {
                throw new InvalidOperationException("Anchor is not correct");
            }
            if (amount.GetData() % (long)Math.Pow(10, 8 - (Asset as Global.AssetDescriptor).Precision) != 0)
            {
                throw new InvalidOperationException("Anchor is not correct");
            }

            if (!Fixed8.TryParse(strFee, out Fixed8 fee))
            {
                throw new InvalidOperationException("Anchor is not correct");
            }
            if (fee < Fixed8.Zero)
            {
                throw new InvalidOperationException("Anchor is not correct");
            }

            try
            {
                fromAddrVersion = Wallet.GetAddressVersion(fromAddress);
                toAddrVersion = Wallet.GetAddressVersion(toAddress);
            }
            catch
            {
                throw new InvalidOperationException("Anchor is not correct");
            }

            Transaction tx;
            #endregion
            #region T -> T
            if (toAddrVersion == Wallet.AddressVersion && fromAddrVersion == Wallet.AddressVersion) // T -> T
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
                outPut.Fee = fee;
                tx.Outputs = new TransactionOutput[1];
                tx.Outputs[0] = outPut;
                if (tx is ContractTransaction ctx)
                {
                    tx = Constant.CurrentWallet.MakeTransactionFrom(ctx, fromAddress);
                    if (tx == null)
                    {
                        throw new InvalidOperationException("Anchor is not correct");
                    }
                }

                Helper.SignAndShowInformation(tx);
            }
            #endregion
            #region T -> A
            else if (fromAddrVersion == Wallet.AddressVersion && toAddrVersion == Wallet.AnonymouseAddressVersion)  // T -> A
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

                JSOutput jsOut = new JSOutput(Wallet.ToPaymentAddress(toAddress), amount, fee, (UInt256)asset.AssetId);

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
                        
                        ctx_.joinSplitSig = Sodium.PublicKeyAuth.SignDetached(ctx.JsHash.ToArray(), joinSplitPrivKey_);

                        if (!Sodium.PublicKeyAuth.VerifyDetached(ctx_.joinSplitSig, ctx.JsHash.ToArray(), joinSplitPubKey_.ToArray()))
                        {
                            throw new InvalidOperationException("Anchor is not correct");
                        }
                    }
                    else
                    {
                        throw new InvalidOperationException("Anchor is not correct");
                    }
                }
                else
                {
                    throw new InvalidOperationException("Anchor is not correct");
                }

                Helper.SignAndShowInformation(tx);
            }
            #endregion
            #region A -> T
            else if (fromAddrVersion == Wallet.AnonymouseAddressVersion && toAddrVersion == Wallet.AddressVersion)  // A -> T
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

                    #region Split token types
                    /* ******************************   Split the info into main token and fee token  ********************************** */
                    AsyncJoinSplitInfo mainTokenInfo = new AsyncJoinSplitInfo();
                    AsyncJoinSplitInfo subTokenInfo = new AsyncJoinSplitInfo();
                    Fixed8 main_total_output_amount = Fixed8.Zero;
                    Fixed8 sub_total_output_amount = Fixed8.Zero;

                    for (int i = 0; i < info.vjsin.Count; i++)
                    {
                        if (info.vjsin[i].AssetID == Blockchain.UtilityToken.Hash)
                        {
                            subTokenInfo.vjsin.Add(info.vjsin[i]);
                            subTokenInfo.notes.Add(info.vjsin[i].note);
                        }
                        else
                        {
                            mainTokenInfo.vjsin.Add(info.vjsin[i]);
                            mainTokenInfo.notes.Add(info.vjsin[i].note);
                        }
                    }

                    for (int i = 0; i < info.vjsout.Count; i++)
                    {
                        if (info.vjsout[i].AssetID == Blockchain.UtilityToken.Hash)
                        {
                            subTokenInfo.vjsout.Add(info.vjsout[i]);
                            sub_total_output_amount += info.vjsout[i].value;
                        }
                        else
                        {
                            mainTokenInfo.vjsout.Add(info.vjsout[i]);
                            main_total_output_amount += info.vjsout[i].value;
                        }
                    }
                    /* ******************************                      End                        ********************************** */
                    #endregion

                    IntPtr vectorWitness = SnarkDllApi.Witnesses_Create();

                    int jsIndex = 0;
                    Fixed8 current_inputed_amount = Fixed8.Zero;
                    Fixed8 rest_amount = totalAmount;

                    #region Do Process the Main token part
                    for (int i = 0; i < mainTokenInfo.vjsin.Count; i++)
                    {
                        IntPtr witness = SnarkDllApi.CmWitness_Create();

                        SnarkDllApi.SetCMWitnessFromBinary(witness, mainTokenInfo.vjsin[i].witness, mainTokenInfo.vjsin[i].witness.Length);
                        SnarkDllApi.Witnesses_Add(vectorWitness, witness);

                        IntPtr ptrWitness = SnarkDllApi.GetCMRootFromWitness(witness);
                        byte[] byWRoot = new byte[32];
                        System.Runtime.InteropServices.Marshal.Copy(ptrWitness, byWRoot, 0, 32);
                        UInt256 wAnchor = new UInt256(byWRoot);

                        if (jsAnchor != wAnchor)
                        {
                            throw new InvalidOperationException("Anchor is not correct");
                        }

                        current_inputed_amount += mainTokenInfo.vjsin[i].note.value;

                        jsIndex++;

                        if (jsIndex == 2 && i != mainTokenInfo.vjsin.Count - 1)
                        {
                            AsyncJoinSplitInfo jsInfo = new AsyncJoinSplitInfo();
                            jsInfo.vjsin.Add(mainTokenInfo.vjsin[i - 1]);
                            jsInfo.vjsin.Add(mainTokenInfo.vjsin[i]);

                            jsInfo.notes.Add(mainTokenInfo.notes[i - 1]);
                            jsInfo.notes.Add(mainTokenInfo.notes[i]);

                            var vInputsSum = jsInfo.vjsin[0].note.value + jsInfo.vjsin[1].note.value;

                            for (int oti = 0; oti < mainTokenInfo.vjsout.Count; oti++)
                            {
                                if (mainTokenInfo.vjsout[oti].value >= vInputsSum)
                                {
                                    JSOutput jsOut1 = new JSOutput(mainTokenInfo.vjsout[oti].addr, vInputsSum, mainTokenInfo.vjsout[oti].AssetID);
                                    jsInfo.vjsout.Add(jsOut1);
                                    mainTokenInfo.vjsout[oti].value -= vInputsSum;

                                    vInputsSum = Fixed8.Zero;
                                    break;
                                }

                                if (mainTokenInfo.vjsout[oti].value < vInputsSum)
                                {
                                    JSOutput jsOut1 = new JSOutput(mainTokenInfo.vjsout[oti].addr, mainTokenInfo.vjsout[oti].value, mainTokenInfo.vjsout[oti].AssetID);
                                    jsInfo.vjsout.Add(jsOut1);

                                    vInputsSum = vInputsSum - mainTokenInfo.vjsout[oti].value;
                                    mainTokenInfo.vjsout[oti].value = Fixed8.Zero;
                                }
                            }

                            if (vInputsSum >= Fixed8.Zero)
                            {
                                jsInfo.vpub_new = vInputsSum;
                            }

                            tx = Constant.CurrentWallet.Perform_JoinSplit(ctx, jsInfo, joinSplitPubKey_, joinSplitPrivKey_, (UInt256)jsInfo.vjsin[0].AssetID, vectorWitness, jsAnchor);

                            jsIndex = 0;
                            SnarkDllApi.Witnesses_Clear(vectorWitness);
                        }

                        if (i == mainTokenInfo.vjsin.Count - 1)
                        {
                            AsyncJoinSplitInfo jsInfo = new AsyncJoinSplitInfo();

                            Fixed8 jsInputedAmount = Fixed8.Zero;

                            for (int ji = jsIndex - 1; ji > -1; ji--)
                            {
                                jsInfo.vjsin.Add(mainTokenInfo.vjsin[i - ji]);
                                jsInfo.notes.Add(mainTokenInfo.notes[i - ji]);

                                jsInputedAmount += mainTokenInfo.notes[i - ji].value;
                                rest_amount -= mainTokenInfo.notes[i - ji].value;
                            }

                            for (int oti = 0; oti < mainTokenInfo.vjsout.Count; oti++)
                            {
                                if (mainTokenInfo.vjsout[oti].value >= jsInputedAmount)
                                {
                                    JSOutput jsOut1 = new JSOutput(mainTokenInfo.vjsout[oti].addr, jsInputedAmount, mainTokenInfo.vjsout[oti].AssetID);
                                    jsInfo.vjsout.Add(jsOut1);
                                    mainTokenInfo.vjsout[oti].value -= jsInputedAmount;

                                    jsInputedAmount = Fixed8.Zero;
                                    break;
                                }

                                if (mainTokenInfo.vjsout[oti].value < jsInputedAmount)
                                {
                                    JSOutput jsOut1 = new JSOutput(mainTokenInfo.vjsout[oti].addr, mainTokenInfo.vjsout[oti].value, mainTokenInfo.vjsout[oti].AssetID);
                                    jsInfo.vjsout.Add(jsOut1);

                                    jsInputedAmount = jsInputedAmount - mainTokenInfo.vjsout[oti].value;
                                    mainTokenInfo.vjsout[oti].value = Fixed8.Zero;
                                }
                            }

                            if (jsInputedAmount != Fixed8.Zero)
                            {
                                jsInfo.vpub_new = jsInputedAmount;
                            }

                            try
                            {
                                tx = Constant.CurrentWallet.Perform_JoinSplit(ctx, jsInfo, joinSplitPubKey_, joinSplitPrivKey_, (UInt256)jsInfo.vjsin[0].AssetID, vectorWitness, jsAnchor);
                            }
                            catch (Exception ex)
                            {
                                string strException = ex.Message;
                                throw new InvalidOperationException("JoinSplit Errors");
                            }

                            if (tx.Inputs == null)
                            {
                                tx.Inputs = new CoinReference[0];
                            }

                            jsIndex = 0;
                            SnarkDllApi.Witnesses_Clear(vectorWitness);
                        }
                    }
                    #endregion
                    #region Do Process the Fee token part, And when sending the QRG token, then there is no Main token part.
                    for (int i = 0; i < subTokenInfo.vjsin.Count; i++)
                    {
                        IntPtr witness = SnarkDllApi.CmWitness_Create();

                        SnarkDllApi.SetCMWitnessFromBinary(witness, subTokenInfo.vjsin[i].witness, subTokenInfo.vjsin[i].witness.Length);
                        SnarkDllApi.Witnesses_Add(vectorWitness, witness);

                        IntPtr ptrWitness = SnarkDllApi.GetCMRootFromWitness(witness);
                        byte[] byWRoot = new byte[32];
                        System.Runtime.InteropServices.Marshal.Copy(ptrWitness, byWRoot, 0, 32);
                        UInt256 wAnchor = new UInt256(byWRoot);

                        if (jsAnchor != wAnchor)
                        {
                            throw new InvalidOperationException("Anchor is not correct");
                        }

                        current_inputed_amount += subTokenInfo.vjsin[i].note.value;

                        jsIndex++;

                        if (jsIndex == 2 && i != subTokenInfo.vjsin.Count - 1)
                        {
                            AsyncJoinSplitInfo jsInfo = new AsyncJoinSplitInfo();
                            jsInfo.vjsin.Add(subTokenInfo.vjsin[i - 1]);
                            jsInfo.vjsin.Add(subTokenInfo.vjsin[i]);

                            jsInfo.notes.Add(subTokenInfo.notes[i - 1]);
                            jsInfo.notes.Add(subTokenInfo.notes[i]);

                            var vInputsSum = jsInfo.vjsin[0].note.value + jsInfo.vjsin[1].note.value;

                            for (int oti = 0; oti < subTokenInfo.vjsout.Count; oti++)
                            {
                                if (subTokenInfo.vjsout[oti].value >= vInputsSum)
                                {
                                    JSOutput jsOut1 = new JSOutput(subTokenInfo.vjsout[oti].addr, vInputsSum, subTokenInfo.vjsout[oti].AssetID);
                                    jsInfo.vjsout.Add(jsOut1);
                                    subTokenInfo.vjsout[oti].value -= vInputsSum;

                                    vInputsSum = Fixed8.Zero;
                                    break;
                                }

                                if (subTokenInfo.vjsout[oti].value < vInputsSum)
                                {
                                    JSOutput jsOut1 = new JSOutput(subTokenInfo.vjsout[oti].addr, subTokenInfo.vjsout[oti].value, subTokenInfo.vjsout[oti].AssetID);
                                    jsInfo.vjsout.Add(jsOut1);

                                    vInputsSum = vInputsSum - subTokenInfo.vjsout[oti].value;
                                    subTokenInfo.vjsout[oti].value = Fixed8.Zero;
                                }
                            }

                            if (vInputsSum >= Fixed8.Zero)
                            {
                                jsInfo.vpub_new = vInputsSum;
                            }

                            tx = Constant.CurrentWallet.Perform_JoinSplit(ctx, jsInfo, joinSplitPubKey_, joinSplitPrivKey_, (UInt256)jsInfo.vjsin[0].AssetID, vectorWitness, jsAnchor);

                            jsIndex = 0;
                            SnarkDllApi.Witnesses_Clear(vectorWitness);
                        }

                        if (i == subTokenInfo.vjsin.Count - 1)
                        {
                            AsyncJoinSplitInfo jsInfo = new AsyncJoinSplitInfo();

                            Fixed8 jsInputedAmount = Fixed8.Zero;

                            for (int ji = jsIndex - 1; ji > -1; ji--)
                            {
                                jsInfo.vjsin.Add(subTokenInfo.vjsin[i - ji]);
                                jsInfo.notes.Add(subTokenInfo.notes[i - ji]);

                                jsInputedAmount += subTokenInfo.notes[i - ji].value;
                                rest_amount -= subTokenInfo.notes[i - ji].value;
                            }

                            for (int oti = 0; oti < subTokenInfo.vjsout.Count; oti++)
                            {
                                if (subTokenInfo.vjsout[oti].value >= jsInputedAmount)
                                {
                                    JSOutput jsOut1 = new JSOutput(subTokenInfo.vjsout[oti].addr, jsInputedAmount, subTokenInfo.vjsout[oti].AssetID);
                                    jsInfo.vjsout.Add(jsOut1);
                                    subTokenInfo.vjsout[oti].value -= jsInputedAmount;

                                    jsInputedAmount = Fixed8.Zero;
                                    break;
                                }

                                if (subTokenInfo.vjsout[oti].value < jsInputedAmount)
                                {
                                    JSOutput jsOut1 = new JSOutput(subTokenInfo.vjsout[oti].addr, subTokenInfo.vjsout[oti].value, subTokenInfo.vjsout[oti].AssetID);
                                    jsInfo.vjsout.Add(jsOut1);

                                    jsInputedAmount = jsInputedAmount - subTokenInfo.vjsout[oti].value;
                                    subTokenInfo.vjsout[oti].value = Fixed8.Zero;
                                }
                            }

                            if (jsInputedAmount != Fixed8.Zero)
                            {
                                jsInfo.vpub_new = jsInputedAmount;
                            }

                            try
                            {
                                tx = Constant.CurrentWallet.Perform_JoinSplit(ctx, jsInfo, joinSplitPubKey_, joinSplitPrivKey_, (UInt256)jsInfo.vjsin[0].AssetID, vectorWitness, jsAnchor);
                            }
                            catch (Exception ex)
                            {
                                string strException = ex.Message;
                                throw new InvalidOperationException("JoinSplit Errors");
                            }

                            if (tx.Inputs == null)
                            {
                                tx.Inputs = new CoinReference[0];
                            }

                            jsIndex = 0;
                            SnarkDllApi.Witnesses_Clear(vectorWitness);
                        }
                    }
                    #endregion
                    
                    ctx.joinSplitSig = Sodium.PublicKeyAuth.SignDetached(ctx.JsHash.ToArray(), joinSplitPrivKey_);

                    if (!Sodium.PublicKeyAuth.VerifyDetached(ctx.joinSplitSig, ctx.JsHash.ToArray(), joinSplitPubKey_.ToArray()))
                    {
                        throw new InvalidOperationException("Anchor is not correct");
                    }
                }
                else
                {
                    throw new InvalidOperationException("Anchor is not correct");
                }
                Helper.SignAndShowInformation(tx);
            }
            #endregion
            #region A -> A
            else if (fromAddrVersion == Wallet.AnonymouseAddressVersion && toAddrVersion == Wallet.AnonymouseAddressVersion)    // A -> A
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
                    #region Split token type
                    /* ******************************   Split the info into main token and fee token  ********************************** */
                    AsyncJoinSplitInfo mainTokenInfo = new AsyncJoinSplitInfo();
                    AsyncJoinSplitInfo subTokenInfo = new AsyncJoinSplitInfo();
                    Fixed8 main_total_output_amount = Fixed8.Zero;
                    Fixed8 sub_total_output_amount = Fixed8.Zero;

                    for (int i = 0; i < info.vjsin.Count; i ++)
                    {
                        if (info.vjsin[i].AssetID == Blockchain.UtilityToken.Hash)
                        {
                            subTokenInfo.vjsin.Add(info.vjsin[i]);
                            subTokenInfo.notes.Add(info.vjsin[i].note);
                        }
                        else
                        {
                            mainTokenInfo.vjsin.Add(info.vjsin[i]);
                            mainTokenInfo.notes.Add(info.vjsin[i].note);
                        }
                    }

                    for (int i = 0; i < info.vjsout.Count; i ++)
                    {
                        if (info.vjsout[i].AssetID == Blockchain.UtilityToken.Hash)
                        {
                            subTokenInfo.vjsout.Add(info.vjsout[i]);
                            sub_total_output_amount += info.vjsout[i].value;
                        }
                        else
                        {
                            mainTokenInfo.vjsout.Add(info.vjsout[i]);
                            main_total_output_amount += info.vjsout[i].value;
                        }
                    }
                    /* ******************************                      End                        ********************************** */
                    #endregion
                    IntPtr vectorWitness = SnarkDllApi.Witnesses_Create();

                    int jsIndex = 0;
                    Fixed8 current_inputed_amount = Fixed8.Zero;
                    Fixed8 rest_amount = totalAmount;

                    #region Do Process the Main token part
                    for (int i = 0; i < mainTokenInfo.vjsin.Count; i ++)
                    {
                        IntPtr witness = SnarkDllApi.CmWitness_Create();

                        SnarkDllApi.SetCMWitnessFromBinary(witness, mainTokenInfo.vjsin[i].witness, mainTokenInfo.vjsin[i].witness.Length);
                        SnarkDllApi.Witnesses_Add(vectorWitness, witness);

                        IntPtr ptrWitness = SnarkDllApi.GetCMRootFromWitness(witness);
                        byte[] byWRoot = new byte[32];
                        System.Runtime.InteropServices.Marshal.Copy(ptrWitness, byWRoot, 0, 32);
                        UInt256 wAnchor = new UInt256(byWRoot);

                        if (jsAnchor != wAnchor)
                        {
                            throw new InvalidOperationException("Anchor is not correct");
                        }

                        current_inputed_amount += mainTokenInfo.vjsin[i].note.value;

                        jsIndex++;

                        if (jsIndex == 2 && i != mainTokenInfo.vjsin.Count - 1)
                        {
                            AsyncJoinSplitInfo jsInfo = new AsyncJoinSplitInfo();
                            jsInfo.vjsin.Add(mainTokenInfo.vjsin[i - 1]);
                            jsInfo.vjsin.Add(mainTokenInfo.vjsin[i]);

                            jsInfo.notes.Add(mainTokenInfo.notes[i - 1]);
                            jsInfo.notes.Add(mainTokenInfo.notes[i]);

                            var vInputsSum = jsInfo.vjsin[0].note.value + jsInfo.vjsin[1].note.value;

                            for (int oti = 0; oti < mainTokenInfo.vjsout.Count; oti++)
                            {
                                if (mainTokenInfo.vjsout[oti].value >= vInputsSum)
                                {
                                    JSOutput jsOut1 = new JSOutput(mainTokenInfo.vjsout[oti].addr, vInputsSum, mainTokenInfo.vjsout[oti].AssetID);
                                    jsInfo.vjsout.Add(jsOut1);
                                    mainTokenInfo.vjsout[oti].value -= vInputsSum;

                                    vInputsSum = Fixed8.Zero;
                                    break;
                                }

                                if (mainTokenInfo.vjsout[oti].value < vInputsSum)
                                {
                                    JSOutput jsOut1 = new JSOutput(mainTokenInfo.vjsout[oti].addr, mainTokenInfo.vjsout[oti].value, mainTokenInfo.vjsout[oti].AssetID);
                                    jsInfo.vjsout.Add(jsOut1);
                                    
                                    vInputsSum = vInputsSum - mainTokenInfo.vjsout[oti].value;
                                    mainTokenInfo.vjsout[oti].value = Fixed8.Zero;
                                }
                            }

                            tx = Constant.CurrentWallet.Perform_JoinSplit(ctx, jsInfo, joinSplitPubKey_, joinSplitPrivKey_, (UInt256)jsInfo.vjsin[0].AssetID, vectorWitness, jsAnchor);

                            jsIndex = 0;
                            SnarkDllApi.Witnesses_Clear(vectorWitness);
                        }

                        if (i == mainTokenInfo.vjsin.Count - 1)
                        {
                            AsyncJoinSplitInfo jsInfo = new AsyncJoinSplitInfo();

                            Fixed8 jsInputedAmount = Fixed8.Zero;

                            for (int ji = jsIndex - 1; ji > -1; ji--)
                            {
                                jsInfo.vjsin.Add(mainTokenInfo.vjsin[i - ji]);
                                jsInfo.notes.Add(mainTokenInfo.notes[i - ji]);

                                jsInputedAmount += mainTokenInfo.notes[i - ji].value;
                                rest_amount -= mainTokenInfo.notes[i - ji].value;
                            }

                            for (int oti = 0; oti < mainTokenInfo.vjsout.Count; oti++)
                            {
                                if (mainTokenInfo.vjsout[oti].value >= jsInputedAmount)
                                {
                                    JSOutput jsOut1 = new JSOutput(mainTokenInfo.vjsout[oti].addr, jsInputedAmount, mainTokenInfo.vjsout[oti].AssetID);
                                    jsInfo.vjsout.Add(jsOut1);
                                    mainTokenInfo.vjsout[oti].value -= jsInputedAmount;

                                    jsInputedAmount = Fixed8.Zero;
                                    break;
                                }

                                if (mainTokenInfo.vjsout[oti].value < jsInputedAmount)
                                {
                                    JSOutput jsOut1 = new JSOutput(mainTokenInfo.vjsout[oti].addr, mainTokenInfo.vjsout[oti].value, mainTokenInfo.vjsout[oti].AssetID);
                                    jsInfo.vjsout.Add(jsOut1);

                                    jsInputedAmount = jsInputedAmount - mainTokenInfo.vjsout[oti].value;
                                    mainTokenInfo.vjsout[oti].value = Fixed8.Zero;
                                }
                            }

                            if (jsInputedAmount != Fixed8.Zero)
                            {
                                jsInfo.vpub_new = jsInputedAmount;
                            }

                            try
                            {
                                tx = Constant.CurrentWallet.Perform_JoinSplit(ctx, jsInfo, joinSplitPubKey_, joinSplitPrivKey_, (UInt256)jsInfo.vjsin[0].AssetID, vectorWitness, jsAnchor);
                            }
                            catch (Exception ex)
                            {
                                string strException = ex.Message;
                                throw new InvalidOperationException("JoinSplit Errors");
                            }

                            if (tx.Inputs == null)
                            {
                                tx.Inputs = new CoinReference[0];
                            }

                            jsIndex = 0;
                            SnarkDllApi.Witnesses_Clear(vectorWitness);
                        }
                    }
                    #endregion
                    #region Do Process the Fee token part, And when sending the QRG token, then there is no Main token part.
                    for (int i = 0; i < subTokenInfo.vjsin.Count; i++)
                    {
                        IntPtr witness = SnarkDllApi.CmWitness_Create();

                        SnarkDllApi.SetCMWitnessFromBinary(witness, subTokenInfo.vjsin[i].witness, subTokenInfo.vjsin[i].witness.Length);
                        SnarkDllApi.Witnesses_Add(vectorWitness, witness);

                        IntPtr ptrWitness = SnarkDllApi.GetCMRootFromWitness(witness);
                        byte[] byWRoot = new byte[32];
                        System.Runtime.InteropServices.Marshal.Copy(ptrWitness, byWRoot, 0, 32);
                        UInt256 wAnchor = new UInt256(byWRoot);

                        if (jsAnchor != wAnchor)
                        {
                            throw new InvalidOperationException("Anchor is not correct");
                        }

                        current_inputed_amount += subTokenInfo.vjsin[i].note.value;

                        jsIndex++;

                        if (jsIndex == 2 && i != subTokenInfo.vjsin.Count - 1)
                        {
                            AsyncJoinSplitInfo jsInfo = new AsyncJoinSplitInfo();
                            jsInfo.vjsin.Add(subTokenInfo.vjsin[i - 1]);
                            jsInfo.vjsin.Add(subTokenInfo.vjsin[i]);

                            jsInfo.notes.Add(subTokenInfo.notes[i - 1]);
                            jsInfo.notes.Add(subTokenInfo.notes[i]);

                            var vInputsSum = jsInfo.vjsin[0].note.value + jsInfo.vjsin[1].note.value;

                            for (int oti = 0; oti < subTokenInfo.vjsout.Count; oti++)
                            {
                                if (subTokenInfo.vjsout[oti].value >= vInputsSum)
                                {
                                    JSOutput jsOut1 = new JSOutput(subTokenInfo.vjsout[oti].addr, vInputsSum, subTokenInfo.vjsout[oti].AssetID);
                                    jsInfo.vjsout.Add(jsOut1);
                                    subTokenInfo.vjsout[oti].value -= vInputsSum;

                                    vInputsSum = Fixed8.Zero;
                                    break;
                                }

                                if (subTokenInfo.vjsout[oti].value < vInputsSum)
                                {
                                    JSOutput jsOut1 = new JSOutput(subTokenInfo.vjsout[oti].addr, subTokenInfo.vjsout[oti].value, subTokenInfo.vjsout[oti].AssetID);
                                    jsInfo.vjsout.Add(jsOut1);
                                    
                                    vInputsSum = vInputsSum - subTokenInfo.vjsout[oti].value;
                                    subTokenInfo.vjsout[oti].value = Fixed8.Zero;
                                }
                            }
                            
                            tx = Constant.CurrentWallet.Perform_JoinSplit(ctx, jsInfo, joinSplitPubKey_, joinSplitPrivKey_, (UInt256)jsInfo.vjsin[0].AssetID, vectorWitness, jsAnchor);

                            jsIndex = 0;
                            SnarkDllApi.Witnesses_Clear(vectorWitness);
                        }

                        if (i == subTokenInfo.vjsin.Count - 1)
                        {
                            AsyncJoinSplitInfo jsInfo = new AsyncJoinSplitInfo();

                            Fixed8 jsInputedAmount = Fixed8.Zero;

                            for (int ji = jsIndex - 1; ji > -1; ji--)
                            {
                                jsInfo.vjsin.Add(subTokenInfo.vjsin[i - ji]);
                                jsInfo.notes.Add(subTokenInfo.notes[i - ji]);

                                jsInputedAmount += subTokenInfo.notes[i - ji].value;
                                rest_amount -= subTokenInfo.notes[i - ji].value;
                            }

                            for (int oti = 0; oti < subTokenInfo.vjsout.Count; oti++)
                            {
                                if (subTokenInfo.vjsout[oti].value >= jsInputedAmount)
                                {
                                    JSOutput jsOut1 = new JSOutput(subTokenInfo.vjsout[oti].addr, jsInputedAmount, subTokenInfo.vjsout[oti].AssetID);
                                    jsInfo.vjsout.Add(jsOut1);
                                    subTokenInfo.vjsout[oti].value -= jsInputedAmount;

                                    jsInputedAmount = Fixed8.Zero;
                                    break;
                                }

                                if (subTokenInfo.vjsout[oti].value < jsInputedAmount)
                                {
                                    JSOutput jsOut1 = new JSOutput(subTokenInfo.vjsout[oti].addr, subTokenInfo.vjsout[oti].value, subTokenInfo.vjsout[oti].AssetID);
                                    jsInfo.vjsout.Add(jsOut1);
                                    
                                    jsInputedAmount = jsInputedAmount - subTokenInfo.vjsout[oti].value;
                                    subTokenInfo.vjsout[oti].value = Fixed8.Zero;
                                }
                            }

                            if (jsInputedAmount != Fixed8.Zero)
                            {
                                jsInfo.vpub_new = jsInputedAmount;
                            }

                            try
                            {
                                tx = Constant.CurrentWallet.Perform_JoinSplit(ctx, jsInfo, joinSplitPubKey_, joinSplitPrivKey_, (UInt256)jsInfo.vjsin[0].AssetID, vectorWitness, jsAnchor);
                            }
                            catch (Exception ex)
                            {
                                string strException = ex.Message;
                                throw new InvalidOperationException("JoinSplit Errors");
                            }

                            if (tx.Inputs == null)
                            {
                                tx.Inputs = new CoinReference[0];
                            }

                            jsIndex = 0;
                            SnarkDllApi.Witnesses_Clear(vectorWitness);
                        }
                    }
                    #endregion

                    ctx.joinSplitSig = Sodium.PublicKeyAuth.SignDetached(ctx.JsHash.ToArray(), joinSplitPrivKey_);

                    if (!Sodium.PublicKeyAuth.VerifyDetached(ctx.joinSplitSig, ctx.JsHash.ToArray(), joinSplitPubKey_.ToArray()))
                    {
                        throw new InvalidOperationException("Anchor is not correct");
                    }
                }
                else
                {
                    throw new InvalidOperationException("Anchor is not correct");
                }
                Helper.SignAndShowInformation(tx);
            }
            #endregion
            #region S -> S
            else if (fromAddrVersion == Wallet.StealthAddressVersion && toAddrVersion == Wallet.StealthAddressVersion)
            {
                List<TransactionAttribute> attributes = new List<TransactionAttribute>();

                tx = new RingConfidentialTransaction();

                tx.Attributes = attributes.ToArray();

                if (tx is RingConfidentialTransaction ctx)
                {
                    List<RCTransactionOutput> rctOutput = new List<RCTransactionOutput>();
                    Pure.Wallets.StealthKey.StealthKeyPair fromKeyPair = null;
                    foreach (KeyPairBase key in Constant.CurrentWallet.GetKeys())
                    {
                        if (key is Pure.Wallets.StealthKey.StealthKeyPair rctKey)
                        {
                            if (Wallet.ToStealthAddress(rctKey) == fromAddress)
                            {
                                fromKeyPair = rctKey;
                            }
                        }
                    }
                    
                    if (fromKeyPair == null)
                    {
                        throw new InvalidOperationException("From key is not exist.");
                    }

                    byte[] r = SchnorrNonLinkable.GenerateRandomScalar();
                    Pure.Cryptography.ECC.ECPoint R = Pure.Cryptography.ECC.ECCurve.Secp256r1.G * r;

                    Pure.Wallets.StealthKey.StealthPubKeys outPubKey = Wallet.ToStealthKeyPair(toAddress).ToStelathPubKeys();

                    RCTransactionOutput output = new RCTransactionOutput
                    {
                        AssetId = (UInt256)asset.AssetId,
                        PubKey = Pure.Cryptography.ECC.ECPoint.DecodePoint(outPubKey.GenPaymentPubKeyHash(r), Pure.Cryptography.ECC.ECCurve.Secp256r1),
                        Value = amount,
                        ScriptHash = Pure.SmartContract.Contract.CreateRingSignatureRedeemScript(outPubKey.PayloadPubKey, outPubKey.ViewPubKey).ToScriptHash()
                    };

                    rctOutput.Add(output);
                    ctx.Scripts = new Witness[0];

                    tx = Constant.CurrentWallet.MakeRCTransaction(ctx, fromAddress, rctOutput, fromKeyPair, r);

                    if (tx == null)
                    {
                        throw new InvalidOperationException("Anchor is not correct");
                    }
                }

                Helper.SignAndShowInformation(tx);
            }
            #endregion
            #region T -> S
            else if (fromAddrVersion == Wallet.AddressVersion && toAddrVersion == Wallet.StealthAddressVersion)
            {
                tx = new RingConfidentialTransaction();

                List<TransactionAttribute> attributes = new List<TransactionAttribute>();
                tx.Attributes = attributes.ToArray();

                if (tx is RingConfidentialTransaction rtx)
                {
                    List<RCTransactionOutput> rctOutput = new List<RCTransactionOutput>();

                    byte[] r = SchnorrNonLinkable.GenerateRandomScalar();
                    Pure.Cryptography.ECC.ECPoint R = Pure.Cryptography.ECC.ECCurve.Secp256r1.G * r;

                    Pure.Wallets.StealthKey.StealthPubKeys outPubKey = Wallet.ToStealthKeyPair(toAddress).ToStelathPubKeys();

                    RCTransactionOutput output = new RCTransactionOutput
                    {
                        AssetId = (UInt256)asset.AssetId,
                        PubKey = Pure.Cryptography.ECC.ECPoint.DecodePoint(outPubKey.GenPaymentPubKeyHash(r), Pure.Cryptography.ECC.ECCurve.Secp256r1),
                        Value = amount,
                        ScriptHash = Pure.SmartContract.Contract.CreateRingSignatureRedeemScript(outPubKey.PayloadPubKey, outPubKey.ViewPubKey).ToScriptHash()
                    };

                    rctOutput.Add(output);

                    tx = Constant.CurrentWallet.MakeRCTransaction(rtx, fromAddress, rctOutput, null, r);

                    if (tx == null)
                    {
                        throw new InvalidOperationException("Anchor is not correct");
                    }
                }

                Helper.SignAndShowInformation(tx);
            }
            #endregion
            #region S -> T
            else if (fromAddrVersion == Wallet.StealthAddressVersion && toAddrVersion == Wallet.AddressVersion)
            {
                List<TransactionAttribute> attributes = new List<TransactionAttribute>();

                tx = new RingConfidentialTransaction();

                tx.Attributes = attributes.ToArray();

                if (tx is RingConfidentialTransaction ctx)
                {
                    List<RCTransactionOutput> rctOutput = new List<RCTransactionOutput>();
                    Pure.Wallets.StealthKey.StealthKeyPair fromKeyPair = null;
                    foreach (KeyPairBase key in Constant.CurrentWallet.GetKeys())
                    {
                        if (key is Pure.Wallets.StealthKey.StealthKeyPair rctKey)
                        {
                            if (Wallet.ToStealthAddress(rctKey) == fromAddress)
                            {
                                fromKeyPair = rctKey;
                            }
                        }
                    }

                    if (fromKeyPair == null)
                    {
                        throw new InvalidOperationException("From key is not exist.");
                    }

                    byte[] r = SchnorrNonLinkable.GenerateRandomScalar();
                    Pure.Cryptography.ECC.ECPoint R = Pure.Cryptography.ECC.ECCurve.Secp256r1.G * r;

                    ctx.Scripts = new Witness[0];

                    TransactionOutput outPut = new TransactionOutput();
                    outPut.ScriptHash = Wallet.ToScriptHash(toAddress);
                    outPut.Value = amount;
                    outPut.AssetId = (UInt256)asset.AssetId;
                    tx.Outputs = new TransactionOutput[1];
                    tx.Outputs[0] = outPut;

                    tx = Constant.CurrentWallet.MakeRCTransaction(ctx, fromAddress, rctOutput, fromKeyPair, r);

                    if (tx == null)
                    {
                        throw new InvalidOperationException("Anchor is not correct");
                    }
                }

                Helper.SignAndShowInformation(tx);
            }
            #endregion
        }
    }
}
