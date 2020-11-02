using Quras.Core;
using Quras.Cryptography;
using Quras.IO;
using Quras.Network;
using Quras.Network.Payloads;
using Quras.SmartContract;
using Quras.Wallets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Security.Cryptography;

namespace Quras.Consensus
{
    public class ConsensusService : IDisposable
    {
        public const int MaxTransactionsPerBlock = 18000;

        private ConsensusContext context = new ConsensusContext();
        private LocalNode localNode;
        private Wallet wallet;
        private Timer timer;
        private uint timer_height;
        private byte timer_view;
        private DateTime block_received_time;
        private bool started = false;
        private bool firstchangeview = true;

        public ConsensusService(LocalNode localNode, Wallet wallet)
        {
            this.localNode = localNode;
            this.wallet = wallet;
            this.timer = new Timer(OnTimeout, null, Timeout.Infinite, Timeout.Infinite);
        }

        private bool AddTransaction(Transaction tx, bool verify)
        {
            if (Blockchain.Default.ContainsTransaction(tx.Hash) ||
                (verify && !tx.Verify(context.Transactions.Values)) ||
                !CheckPolicy(tx))
            {
                Log($"reject tx: {tx.Hash}{Environment.NewLine}{tx.ToArray().ToHexString()}");
                RequestChangeView();
                return false;
            }
            context.Transactions[tx.Hash] = tx;
            if (context.TransactionHashes.Length == context.Transactions.Count)
            {
                if (Blockchain.GetConsensusAddress(Blockchain.Default.GetValidators(context.Transactions.Values).ToArray()).Equals(context.NextConsensus))
                {
                    Log($"send perpare response");
                    context.State |= ConsensusState.SignatureSent;
                    context.Signatures[context.MyIndex] = context.MakeHeader().Sign((KeyPair)wallet.GetKey(context.Validators[context.MyIndex]));
                    SignAndRelay(context.MakePrepareResponse(context.Signatures[context.MyIndex]));
                    CheckSignatures();
                }
                else
                {
                    RequestChangeView();
                    return false;
                }
            }
            return true;
        }

        private void Blockchain_PersistCompleted(object sender, Block block)
        {
            Log($"persist block: {block.Hash}");
            block_received_time = DateTime.Now;
            firstchangeview = false;
            InitializeConsensus(0);
        }

        private void CheckExpectedView(byte view_number)
        {
            if (context.ViewNumber == view_number) return;
            if (context.ExpectedView.Count(p => p == view_number) >= context.M)
            {
                InitializeConsensus(view_number);
            }
        }

        protected virtual bool CheckPolicy(Transaction tx)
        {
            return true;
        }

        private void CheckSignatures()
        {
            if (context.Signatures.Count(p => p != null) >= context.M && context.TransactionHashes.All(p => context.Transactions.ContainsKey(p)))
            {
                VerificationContract contract = VerificationContract.CreateMultiSigContract(context.Validators[context.MyIndex].EncodePoint(true).ToScriptHash(), context.M, context.Validators);
                Block block = context.MakeHeader();
                ContractParametersContext sc = new ContractParametersContext(block);
                for (int i = 0, j = 0; i < context.Validators.Length && j < context.M; i++)
                    if (context.Signatures[i] != null)
                    {
                        sc.AddSignature(contract, context.Validators[i], context.Signatures[i]);
                        j++;
                    }
                sc.Verifiable.Scripts = sc.GetScripts();
                block.Transactions = context.TransactionHashes.Select(p => context.Transactions[p]).ToArray();
                Log($"relay block: {block.Hash}");
                if (!localNode.Relay(block))
                    Log($"reject block: {block.Hash}");
                context.State |= ConsensusState.BlockSent;
            }
        }

        public MinerTransaction CreateMinerTransaction(IEnumerable<Transaction> transactions, uint height, ulong nonce)
        {
            Dictionary<UInt256, Fixed8> amount_netfee = Block.CalculateNetFee(transactions);
            List<TransactionOutput> outputs = new List<TransactionOutput>();

            foreach (var key in amount_netfee.Keys)
            {
                AssetState asset = Blockchain.Default.GetAssetState(key);

                if (asset.AssetId == Blockchain.GoverningToken.Hash)
                {
                    TransactionOutput output = new TransactionOutput
                    {
                        AssetId = Blockchain.UtilityToken.Hash,
                        Value = amount_netfee[key],
                        ScriptHash = wallet.GetChangeAddress()
                    };

                    outputs.Add(output);
                }
                else if (asset.AssetId == Blockchain.UtilityToken.Hash)
                {
                    bool isNew = true;
                    for (int i = 0; i < outputs.Count; i++)
                    {
                        if (outputs[i].AssetId == Blockchain.UtilityToken.Hash)
                        {
                            outputs[i].Value += amount_netfee[key];
                            isNew = false;
                        }
                    }
                    if (isNew)
                    {
                        TransactionOutput output = new TransactionOutput
                        {
                            AssetId = Blockchain.UtilityToken.Hash,
                            Value = amount_netfee[key],
                            ScriptHash = wallet.GetChangeAddress()
                        };

                        outputs.Add(output);
                    }
                }
                else
                {
                    Fixed8 consensusFee = Fixed8.Zero;
                    Fixed8 assetOwnerFee = Fixed8.Zero;

                    if (amount_netfee[key] <= Fixed8.Satoshi * 10000000)
                    {
                        consensusFee = amount_netfee[key] * 8 / 10;
                        assetOwnerFee = amount_netfee[key] * 2 / 10;
                    }
                    else if (amount_netfee[key] < Fixed8.FromDecimal(1))
                    {
                        consensusFee = amount_netfee[key] * 75 / 100;
                        assetOwnerFee = amount_netfee[key] * 25 / 100;
                    }
                    else if (amount_netfee[key] < Fixed8.FromDecimal(5))
                    {
                        consensusFee = amount_netfee[key] * 7 / 10;
                        assetOwnerFee = amount_netfee[key] * 3 / 10;
                    }
                    else if (amount_netfee[key] < Fixed8.FromDecimal(10))
                    {
                        consensusFee = amount_netfee[key] * 65 / 100;
                        assetOwnerFee = amount_netfee[key] * 35 / 100;
                    }
                    else
                    {
                        consensusFee = amount_netfee[key] * 6 / 10;
                        assetOwnerFee = amount_netfee[key] * 4 / 10;
                    }

                    bool isNew = true;
                    for (int i = 0; i < outputs.Count; i++)
                    {
                        if (outputs[i].AssetId == Blockchain.UtilityToken.Hash)
                        {
                            outputs[i].Value += consensusFee;
                            isNew = false;
                        }
                    }
                    if (isNew)
                    {
                        TransactionOutput output = new TransactionOutput
                        {
                            AssetId = Blockchain.UtilityToken.Hash,
                            Value = consensusFee,
                            ScriptHash = wallet.GetChangeAddress()
                        };

                        outputs.Add(output);
                    }

                    TransactionOutput ownerOutput = new TransactionOutput
                    {
                        AssetId = Blockchain.UtilityToken.Hash,
                        Value = assetOwnerFee,
                        ScriptHash = asset.FeeAddress
                    };

                    outputs.Add(ownerOutput);
                }
            }

            return new MinerTransaction
            {
                Nonce = (uint)(nonce % (uint.MaxValue + 1ul)),
                Attributes = new TransactionAttribute[0],
                Inputs = new CoinReference[0],
                Outputs = outputs.ToArray(),
                Scripts = new Witness[0]
            };
        }

        public void Dispose()
        {
            Log("OnStop");
            if (timer != null) timer.Dispose();
            if (started)
            {
                Blockchain.PersistCompleted -= Blockchain_PersistCompleted;
                LocalNode.InventoryReceiving -= LocalNode_InventoryReceiving;
                LocalNode.InventoryReceived -= LocalNode_InventoryReceived;
            }
        }

        private static ulong GetNonce()
        {
            byte[] nonce = new byte[sizeof(ulong)];
            RNGCryptoServiceProvider rngCsp = new RNGCryptoServiceProvider();
            rngCsp.GetBytes(nonce);
            return nonce.ToUInt64(0);
        }

        private void InitializeConsensus(byte view_number)
        {
            lock (context)
            {
                if (view_number == 0)
                    context.Reset(wallet);
                else
                    context.ChangeView(view_number);
                if (context.MyIndex < 0) return;
                Log($"initialize: height={context.BlockIndex} view={view_number} index={context.MyIndex} role={(context.MyIndex == context.PrimaryIndex ? ConsensusState.Primary : ConsensusState.Backup)}");
                if (context.MyIndex == context.PrimaryIndex)
                {
                    context.State |= ConsensusState.Primary;
                    timer_height = context.BlockIndex;
                    timer_view = view_number;
                    TimeSpan span = DateTime.Now - block_received_time;
                    if (span >= Blockchain.TimePerBlock)
                        timer.Change(0, Timeout.Infinite);
                    else
                        timer.Change(Blockchain.TimePerBlock - span, Timeout.InfiniteTimeSpan);
                }
                else
                {
                    context.State = ConsensusState.Backup;
                    timer_height = context.BlockIndex;
                    timer_view = view_number;
                    timer.Change(TimeSpan.FromSeconds(Blockchain.SecondsPerBlock << (view_number + 1)), Timeout.InfiniteTimeSpan);
                }
            }
        }

        private void LocalNode_InventoryReceived(object sender, IInventory inventory)
        {
            ConsensusPayload payload = inventory as ConsensusPayload;
            if (payload != null)
            {
                lock (context)
                {
                    if (payload.ValidatorIndex == context.MyIndex) return;
                    if (payload.Version != ConsensusContext.Version)
                        return;
                    if (payload.PrevHash != context.PrevHash || payload.BlockIndex != context.BlockIndex)
                        return;
                    if (payload.ValidatorIndex >= context.Validators.Length) return;
                    ConsensusMessage message;
                    try
                    {
                        message = ConsensusMessage.DeserializeFrom(payload.Data);
                    }
                    catch (Exception)
                    {
                        return;
                    }
                    if (message.ViewNumber != context.ViewNumber && message.Type != ConsensusMessageType.ChangeView)
                        return;
                    switch (message.Type)
                    {
                        case ConsensusMessageType.ChangeView:
                            OnChangeViewReceived(payload, (ChangeView)message);
                            break;
                        case ConsensusMessageType.PrepareRequest:
                            OnPrepareRequestReceived(payload, (PrepareRequest)message);
                            break;
                        case ConsensusMessageType.PrepareResponse:
                            OnPrepareResponseReceived(payload, (PrepareResponse)message);
                            break;
                    }
                }
            }
        }

        private void LocalNode_InventoryReceiving(object sender, InventoryReceivingEventArgs e)
        {
            Transaction tx = e.Inventory as Transaction;
            if (tx != null)
            {
                lock (context)
                {
                    if (!context.State.HasFlag(ConsensusState.Backup) || !context.State.HasFlag(ConsensusState.RequestReceived) || context.State.HasFlag(ConsensusState.SignatureSent) || context.State.HasFlag(ConsensusState.ViewChanging))
                        return;
                    if (context.Transactions.ContainsKey(tx.Hash)) return;
                    if (!context.TransactionHashes.Contains(tx.Hash)) return;
                    AddTransaction(tx, true);
                    e.Cancel = true;
                }
            }
        }

        protected virtual void Log(string message)
        {
        }

        private void OnChangeViewReceived(ConsensusPayload payload, ChangeView message)
        {
            Log($"{nameof(OnChangeViewReceived)}: height={payload.BlockIndex} view={message.ViewNumber} index={payload.ValidatorIndex} nv={message.NewViewNumber}");
            if (message.NewViewNumber == 0xFF)
            {
                // New Start and Restart ChangeViewing.
                InitializeConsensus(0);
            }
            else
            {
                if (message.NewViewNumber <= context.ExpectedView[payload.ValidatorIndex])
                    return;
                context.ExpectedView[payload.ValidatorIndex] = message.NewViewNumber;
                CheckExpectedView(message.NewViewNumber);
            }
        }

        private void OnPrepareRequestReceived(ConsensusPayload payload, PrepareRequest message)
        {
            Log($"{nameof(OnPrepareRequestReceived)}: height={payload.BlockIndex} view={message.ViewNumber} index={payload.ValidatorIndex} tx={message.TransactionHashes.Length}");
            if (!context.State.HasFlag(ConsensusState.Backup) || context.State.HasFlag(ConsensusState.RequestReceived))
                return;
            if (payload.ValidatorIndex != context.PrimaryIndex) return;
            if (payload.Timestamp <= Blockchain.Default.GetHeader(context.PrevHash).Timestamp || payload.Timestamp > DateTime.Now.AddMinutes(10).ToTimestamp())
            {
                Log($"Timestamp incorrect: {payload.Timestamp}");
                return;
            }
            context.State |= ConsensusState.RequestReceived;
            context.Timestamp = payload.Timestamp;
            context.Nonce = message.Nonce;
            context.CurrentConsensus = message.CurrentConsensus;
            context.NextConsensus = message.NextConsensus;
            context.TransactionHashes = message.TransactionHashes;
            if (context.TransactionHashes.Length > MaxTransactionsPerBlock) return;
            context.Transactions = new Dictionary<UInt256, Transaction>();
            if (!Crypto.Default.VerifySignature(context.MakeHeader().GetHashData(), message.Signature, context.Validators[payload.ValidatorIndex].EncodePoint(false))) return;
            context.Signatures = new byte[context.Validators.Length][];
            context.Signatures[payload.ValidatorIndex] = message.Signature;
            Dictionary<UInt256, Transaction> mempool = LocalNode.GetMemoryPool().ToDictionary(p => p.Hash);
            foreach (UInt256 hash in context.TransactionHashes.Skip(1))
            {
                if (mempool.TryGetValue(hash, out Transaction tx))
                {
                    Console.WriteLine("No Verify Add Transaction" + tx.is_consensus_mempool);
                    if (!AddTransaction(tx, true))
                        return;
                }
            }
            if (!AddTransaction(message.MinerTransaction, true)) return;
            if (context.Transactions.Count < context.TransactionHashes.Length)
            {
                LocalNode.AllowHashes(context.TransactionHashes.Except(context.Transactions.Keys));
                UInt256[] hashes = context.TransactionHashes.ToArray(); // get hashes
                InvPayload msg = InvPayload.Create(InventoryType.TX, hashes);  // create message
                foreach (RemoteNode node in localNode.GetRemoteNodes()) // enqueue message
                    node.EnqueueMessage("getdata", msg);
            }
        }

        private void OnPrepareResponseReceived(ConsensusPayload payload, PrepareResponse message)
        {
            Log($"{nameof(OnPrepareResponseReceived)}: height={payload.BlockIndex} view={message.ViewNumber} index={payload.ValidatorIndex}");
            if (context.State.HasFlag(ConsensusState.BlockSent)) return;
            if (context.Signatures[payload.ValidatorIndex] != null) return;
            Block header = context.MakeHeader();
            if (header == null) return;
            header.CurrentConsensus = wallet.GetChangeAddress();

            if (!Crypto.Default.VerifySignature(header.GetHashData(), message.Signature, context.Validators[payload.ValidatorIndex].EncodePoint(false))) return;
            context.Signatures[payload.ValidatorIndex] = message.Signature;
            CheckSignatures();
        }

        private void OnTimeout(object state)
        {
            lock (context)
            {
                if (timer_height != context.BlockIndex || timer_view != context.ViewNumber) return;
                Log($"timeout: height={timer_height} view={timer_view} state={context.State}");
                if (context.State.HasFlag(ConsensusState.Primary) && !context.State.HasFlag(ConsensusState.RequestSent))
                {
                    Log($"send prepare request: height={timer_height} view={timer_view}");
                    context.State |= ConsensusState.RequestSent;
                    if (!context.State.HasFlag(ConsensusState.SignatureSent))
                    {
                        context.Timestamp = Math.Max(DateTime.Now.ToTimestamp(), Blockchain.Default.GetHeader(context.PrevHash).Timestamp + 1);
                        context.Nonce = GetNonce();
                        List<Transaction> transactions = LocalNode.GetMemoryPool().Where(p => CheckPolicy(p)).ToList();

                        Dictionary<UInt160, Transaction> scriptHashDictionary = new Dictionary<UInt160, Transaction>();
                        Console.WriteLine("Mempool count " + transactions.Count.ToString());
                        for (int i = 0; i < transactions.Count; i++)
                        {
                            if (transactions[i].Type == TransactionType.MinerTransaction ||
                                    transactions[i].Type == TransactionType.AnonymousContractTransaction ||
                                    transactions[i].Type == TransactionType.RingConfidentialTransaction)
                            {
                                continue;
                            }
                            if (transactions[i].Inputs == null || transactions[i].Inputs.Length == 0)
                            {
                                continue;
                            }

                            if (transactions[i].References == null)
                            {
                                transactions.RemoveAt(i);
                                i--;
                                continue;
                            }


                            if ( transactions[i].Inputs.Length > 0 
                                 && transactions[i].References.ContainsKey(transactions[i].Inputs[0]) 
                                 && LocalNode.KnownHashes.Count > 0 )
                            {
                                UInt160 scripthash = transactions[i].References[transactions[i].Inputs[0]].ScriptHash;

                                if (scriptHashDictionary.ContainsKey(scripthash))
                                {
                                    Transaction prevTx = scriptHashDictionary[scripthash];
                                    if (LocalNode.KnownHashes.IndexOf(prevTx.Hash) > LocalNode.KnownHashes.IndexOf(transactions[i].Hash))
                                    {
                                        scriptHashDictionary[scripthash] = transactions[i];
                                        transactions.Remove(prevTx);
                                    }
                                    else
                                    {
                                        transactions.RemoveAt(i);
                                    }
                                        
                                    i--;
                                }
                                else
                                {
                                    scriptHashDictionary.Add(scripthash, transactions[i]);
                                }
                            }
                        }


                        Transaction[] tmpool = LocalNode.GetMemoryPool().ToArray();
                        List<Transaction> consensus_transactions = new List<Transaction>();

                        for (int i = 0; i < transactions.Count; i++)
                        {
                            Transaction tx = transactions[i];
                            if (!tx.Verify(tmpool))
                            {
                                Console.Write("Transaction verify failed : ");
                                
                                transactions.RemoveAt(i);
                                LocalNode.RemoveTxFromMempool(tx.Hash);
                                i--;

                                Console.WriteLine(tx.ToJsonString());
                                continue;
                            }

                            UInt160 fromScriptHash = LocalNode.GetFromAddressFromTx(tx); Console.WriteLine("Step_1_2");

                            if (Blockchain.IsConsensusAddress(fromScriptHash))
                            {
                                consensus_transactions.Add(tx);
                                continue;
                            }
                            else if (tx.GetFee() == Fixed8.Zero && fromScriptHash != UInt160.Zero)
                            {
                                if ( LocalNode.freetx_pool.ContainsKey(fromScriptHash)
                                     && LocalNode.freetx_pool[fromScriptHash].Count >= Blockchain.Default.FreeTransactionLimitPerPeriod
                                     && Blockchain.Default.Height - LocalNode.freetx_pool[fromScriptHash].ElementAt(LocalNode.freetx_pool[fromScriptHash].Count - 1) < Blockchain.Default.FreeTransactionBlockPeriodNum )
                                {
                                    UInt256 removalHash = transactions[i].Hash;

                                    Console.WriteLine("TX removed " + removalHash);

                                    LocalNode.RemoveTxFromMempool(removalHash);

                                    transactions.RemoveAt(i);
                                    i--;
                                }
                                else
                                {
                                    localNode.AddFreeTxAddressToPool(fromScriptHash);

                                    foreach (RemoteNode node in LocalNode.Default.GetRemoteNodes()) // enqueue message
                                        node.EnqueueMessage("blockfree", fromScriptHash);
                                }
                            }

                        }

                        if (transactions.Count >= MaxTransactionsPerBlock)
                            transactions = consensus_transactions.Concat(transactions.OrderByDescending(p => p.NetworkFee / p.Size).Take(MaxTransactionsPerBlock - 1 - consensus_transactions.Count).ToList()).ToList();

                        transactions.Insert(0, CreateMinerTransaction(transactions, context.BlockIndex, context.Nonce));
                        context.TransactionHashes = transactions.Select(p => p.Hash).ToArray();
                        context.Transactions = transactions.ToDictionary(p => p.Hash);
                        context.CurrentConsensus = wallet.GetChangeAddress();
                        context.NextConsensus = Blockchain.GetConsensusAddress(Blockchain.Default.GetValidators(transactions).ToArray());
                        context.Signatures[context.MyIndex] = context.MakeHeader().Sign((KeyPair)wallet.GetKey(context.Validators[context.MyIndex]));
                    }
                    SignAndRelay(context.MakePrepareRequest());
                    timer.Change(TimeSpan.FromSeconds(Blockchain.SecondsPerBlock << (timer_view + 1)), Timeout.InfiniteTimeSpan);
                }
                else if ((context.State.HasFlag(ConsensusState.Primary) && context.State.HasFlag(ConsensusState.RequestSent)) || context.State.HasFlag(ConsensusState.Backup))
                {
                    RequestChangeView();
                }
            }
        }

        private void RequestChangeView()
        {
            context.State |= ConsensusState.ViewChanging;
            context.ExpectedView[context.MyIndex]++;
            Log($"request change view: height={context.BlockIndex} view={context.ViewNumber} nv={context.ExpectedView[context.MyIndex]} state={context.State}");
            timer.Change(TimeSpan.FromSeconds(Blockchain.SecondsPerBlock << (context.ExpectedView[context.MyIndex] + 1)), Timeout.InfiniteTimeSpan);
            SignAndRelay(context.MakeChangeView(firstchangeview));
            CheckExpectedView(context.ExpectedView[context.MyIndex]);
            firstchangeview = false;
        }

        private void SignAndRelay(ConsensusPayload payload)
        {
            ContractParametersContext sc;
            try
            {
                sc = new ContractParametersContext(payload);
                wallet.Sign(sc);
            }
            catch (InvalidOperationException)
            {
                return;
            }
            sc.Verifiable.Scripts = sc.GetScripts();
            localNode.RelayDirectly(payload);
        }

        

        public void Start()
        {
            Log("OnStart");
            started = true;
            Blockchain.PersistCompleted += Blockchain_PersistCompleted;
            LocalNode.InventoryReceiving += LocalNode_InventoryReceiving;
            LocalNode.InventoryReceived += LocalNode_InventoryReceived;
            InitializeConsensus(0);
        }
    }
}
