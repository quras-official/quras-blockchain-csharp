﻿using Quras.Core;
using Quras.IO.Json;
using Quras.SmartContract;
using Quras.Wallets;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Quras.Network.RPC
{
    internal class RpcServerWithWallet : RpcServer
    {
        public RpcServerWithWallet(LocalNode localNode, bool isRPC)
            : base(localNode, isRPC)
        {
        }

        protected override JObject Process(string method, JArray _params)
        {
            switch (method)
            {
                case "getbalance":
                    if (Program.Wallet == null)
                        throw new RpcException(-400, "Access denied.");
                    else
                    {
                        UInt256 assetId = UInt256.Parse(_params[0].AsString());
                        IEnumerable<Coin> coins = Program.Wallet.GetCoins().Where(p => !p.State.HasFlag(CoinState.Spent) && p.Output.AssetId.Equals(assetId));
                        JObject json = new JObject();
                        json["balance"] = coins.Sum(p => p.Output.Value).ToString();
                        json["confirmed"] = coins.Where(p => p.State.HasFlag(CoinState.Confirmed)).Sum(p => p.Output.Value).ToString();
                        return json;
                    }
                case "sendtoaddress":
                    if (Program.Wallet == null)
                        throw new RpcException(-400, "Access denied");
                    else
                    {
                        UIntBase assetId = UIntBase.Parse(_params[0].AsString());
                        AssetDescriptor descriptor = new AssetDescriptor(assetId);
                        UInt160 scriptHash = Wallet.ToScriptHash(_params[1].AsString());
                        BigDecimal value = BigDecimal.Parse(_params[2].AsString(), descriptor.Decimals);
                        if (value.Sign <= 0)
                            throw new RpcException(-32602, "Invalid params");
                        Fixed8 fee = _params.Count >= 4 ? Fixed8.Parse(_params[3].AsString()) : Fixed8.Zero;
                        if (fee < Fixed8.Zero)
                            throw new RpcException(-32602, "Invalid params");
                        UInt160 change_address = _params.Count >= 5 ? Wallet.ToScriptHash(_params[4].AsString()) : null;
                        Transaction tx = Program.Wallet.MakeTransaction(null, new[]
                        {
                            new TransferOutput
                            {
                                AssetId = assetId,
                                Value = value,
                                ScriptHash = scriptHash
                            }
                        }, change_address: change_address, fee: fee);
                        if (tx == null)
                            throw new RpcException(-300, "Insufficient funds");
                        ContractParametersContext context = new ContractParametersContext(tx);
                        Program.Wallet.Sign(context);
                        if (context.Completed)
                        {
                            tx.Scripts = context.GetScripts();
                            Program.Wallet.SaveTransaction(tx);
                            LocalNode.Relay(tx);
                            return tx.ToJson();
                        }
                        else
                        {
                            return context.ToJson();
                        }
                    }
                case "sendmany":
                    if (Program.Wallet == null)
                        throw new RpcException(-400, "Access denied");
                    else
                    {
                        JArray to = (JArray)_params[0];
                        if (to.Count == 0)
                            throw new RpcException(-32602, "Invalid params");
                        TransferOutput[] outputs = new TransferOutput[to.Count];
                        for (int i = 0; i < to.Count; i++)
                        {
                            UIntBase asset_id = UIntBase.Parse(to[i]["asset"].AsString());
                            AssetDescriptor descriptor = new AssetDescriptor(asset_id);
                            outputs[i] = new TransferOutput
                            {
                                AssetId = asset_id,
                                Value = BigDecimal.Parse(to[i]["value"].AsString(), descriptor.Decimals),
                                ScriptHash = Wallet.ToScriptHash(to[i]["address"].AsString())
                            };
                            if (outputs[i].Value.Sign <= 0)
                                throw new RpcException(-32602, "Invalid params");
                        }
                        Fixed8 fee = _params.Count >= 2 ? Fixed8.Parse(_params[1].AsString()) : Fixed8.Zero;
                        if (fee < Fixed8.Zero)
                            throw new RpcException(-32602, "Invalid params");
                        UInt160 change_address = _params.Count >= 3 ? Wallet.ToScriptHash(_params[2].AsString()) : null;
                        Transaction tx = Program.Wallet.MakeTransaction(null, outputs, change_address: change_address, fee: fee);
                        if (tx == null)
                            throw new RpcException(-300, "Insufficient funds");
                        ContractParametersContext context = new ContractParametersContext(tx);
                        Program.Wallet.Sign(context);
                        if (context.Completed)
                        {
                            tx.Scripts = context.GetScripts();
                            Program.Wallet.SaveTransaction(tx);
                            LocalNode.Relay(tx);
                            return tx.ToJson();
                        }
                        else
                        {
                            return context.ToJson();
                        }
                    }
                case "getnewaddress":
                    if (Program.Wallet == null)
                        throw new RpcException(-400, "Access denied");
                    else
                    {
                        KeyPair key = (KeyPair)Program.Wallet.CreateKey();
                        VerificationContract contract = Program.Wallet.GetContracts(key.PublicKeyHash).First(p => p.IsStandard);
                        return contract.Address;
                    }
                case "dumpprivkey":
                    if (Program.Wallet == null)
                        throw new RpcException(-400, "Access denied");
                    else
                    {
                        UInt160 scriptHash = Wallet.ToScriptHash(_params[0].AsString());
                        KeyPair key = (KeyPair)Program.Wallet.GetKeyByScriptHash(scriptHash);
                        return key.Export();
                    }
                default:
                    return base.Process(method, _params);
            }
        }
    }
}
