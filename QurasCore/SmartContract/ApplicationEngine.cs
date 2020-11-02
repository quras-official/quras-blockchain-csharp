﻿using Quras.Core;
using Quras.Cryptography.ECC;
using Quras.IO.Caching;
using Quras.VM;
using System;
using System.Text;

namespace Quras.SmartContract
{
    public class ApplicationEngine : ExecutionEngine
    {
        private const long ratio = 100000;
        private const long gas_free = 10 * 100000000;
        private readonly long gas_amount;
        private long gas_consumed = 0;
        private readonly bool testMode;

        public TriggerType Trigger { get; }
        public Fixed8 GasConsumed => new Fixed8(gas_consumed);

        public ApplicationEngine(TriggerType trigger, IScriptContainer container, IScriptTable table, InteropService service, Fixed8 gas, bool testMode = false)
            : base(container, Cryptography.Crypto.Default, table, service)
        {
            this.gas_amount = gas_free + gas.GetData();
            this.testMode = testMode;
            this.Trigger = trigger;
        }

        private bool CheckArraySize()
        {
            const uint MaxArraySize = 1024;
            if (CurrentContext.InstructionPointer >= CurrentContext.Script.Length)
                return true;
            OpCode opcode = CurrentContext.NextInstruction;
            switch (opcode)
            {
                case OpCode.PACK:
                case OpCode.NEWARRAY:
                    {
                        if (EvaluationStack.Count == 0) return false;
                        int size = (int)EvaluationStack.Peek().GetBigInteger();
                        if (size > MaxArraySize) return false;
                        return true;
                    }
                default:
                    return true;
            }
        }

        private bool CheckInvocationStack()
        {
            const uint MaxStackSize = 1024;
            if (CurrentContext.InstructionPointer >= CurrentContext.Script.Length)
                return true;
            OpCode opcode = CurrentContext.NextInstruction;
            switch (opcode)
            {
                case OpCode.CALL:
                case OpCode.APPCALL:
                    if (InvocationStack.Count >= MaxStackSize) return false;
                    return true;
                default:
                    return true;
            }
        }

        private bool CheckItemSize()
        {
            const uint MaxItemSize = 1024 * 1024;
            if (CurrentContext.InstructionPointer >= CurrentContext.Script.Length)
                return true;
            OpCode opcode = CurrentContext.NextInstruction;
            switch (opcode)
            {
                case OpCode.PUSHDATA4:
                    {
                        if (CurrentContext.InstructionPointer + 4 >= CurrentContext.Script.Length)
                            return false;
                        uint length = CurrentContext.Script.ToUInt32(CurrentContext.InstructionPointer + 1);
                        if (length > MaxItemSize) return false;
                        return true;
                    }
                case OpCode.CAT:
                    {
                        if (EvaluationStack.Count < 2) return false;
                        int length;
                        try
                        {
                            length = EvaluationStack.Peek(0).GetByteArray().Length + EvaluationStack.Peek(1).GetByteArray().Length;
                        }
                        catch (NotSupportedException)
                        {
                            return false;
                        }
                        if (length > MaxItemSize) return false;
                        return true;
                    }
                default:
                    return true;
            }
        }

        private bool CheckStackSize()
        {
            const uint MaxStackSize = 2 * 1024;
            if (CurrentContext.InstructionPointer >= CurrentContext.Script.Length)
                return true;
            int size = 0;
            OpCode opcode = CurrentContext.NextInstruction;
            if (opcode <= OpCode.PUSH16)
                size = 1;
            else
                switch (opcode)
                {
                    case OpCode.DEPTH:
                    case OpCode.DUP:
                    case OpCode.OVER:
                    case OpCode.TUCK:
                        size = 1;
                        break;
                    case OpCode.UNPACK:
                        StackItem item = EvaluationStack.Peek();
                        if (!item.IsArray) return false;
                        size = item.GetArray().Length;
                        break;
                }
            if (size == 0) return true;
            size += EvaluationStack.Count + AltStack.Count;
            if (size > MaxStackSize) return false;
            return true;
        }

        public new bool Execute()
        {
            while (!State.HasFlag(VMState.HALT) && !State.HasFlag(VMState.FAULT))
            {
                try
                {
                    gas_consumed = checked(gas_consumed + GetPrice() * ratio);
                    if (!testMode && gas_consumed > gas_amount) return false;
                    if (!CheckItemSize()) return false;
                    if (!CheckStackSize()) return false;
                    if (!CheckArraySize()) return false;
                    if (!CheckInvocationStack()) return false;
                }
                catch
                {
                    return false;
                }
                StepInto();
            }
            return !State.HasFlag(VMState.FAULT);
        }

        protected virtual long GetPrice()
        {
            if (CurrentContext.InstructionPointer >= CurrentContext.Script.Length)
                return 0;
            OpCode opcode = CurrentContext.NextInstruction;
            if (opcode <= OpCode.PUSH16) return 0;
            switch (opcode)
            {
                case OpCode.NOP:
                    return 0;
                case OpCode.APPCALL:
                case OpCode.TAILCALL:
                    return 10;
                case OpCode.SYSCALL:
                    return GetPriceForSysCall();
                case OpCode.SHA1:
                case OpCode.SHA256:
                    return 10;
                case OpCode.HASH160:
                case OpCode.HASH256:
                    return 20;
                case OpCode.CHECKSIG:
                    return 100;
                case OpCode.CHECKMULTISIG:
                    {
                        if (EvaluationStack.Count == 0) return 1;
                        int n = (int)EvaluationStack.Peek().GetBigInteger();
                        if (n < 1) return 1;
                        return 100 * n;
                    }
                default: return 1;
            }
        }

        protected virtual long GetPriceForSysCall()
        {
            if (CurrentContext.InstructionPointer >= CurrentContext.Script.Length - 3)
                return 1;
            byte length = CurrentContext.Script[CurrentContext.InstructionPointer + 1];
            if (CurrentContext.InstructionPointer > CurrentContext.Script.Length - length - 2)
                return 1;
            string api_name = Encoding.ASCII.GetString(CurrentContext.Script, CurrentContext.InstructionPointer + 2, length);
            switch (api_name)
            {
                case "Quras.Runtime.CheckWitness":
                    return 42200L;
                case "Quras.Blockchain.GetHeader":
                    return 21100L;
                case "Quras.Blockchain.GetBlock":
                    return 42200L;
                case "Quras.Blockchain.GetTransaction":
                    return 21100L;
                case "Quras.Blockchain.GetAccount":
                    return 21100L;
                case "Quras.Blockchain.GetValidators":
                    return 42200L;
                case "Quras.Blockchain.GetAsset":
                    return 21100L;
                case "Quras.Blockchain.GetContract":
                    return 21100L;
                case "Quras.Transaction.GetReferences":
                    return 42200L;
                case "Quras.Account.SetVotes":
                    return 211000L;
                case "Quras.Validator.Register":
                    return 211000L * 100000000L / ratio;
                case "Quras.Asset.Create":
                    return 5000L * 100000000L / ratio;
                case "Quras.Asset.Renew":
                    return (byte)EvaluationStack.Peek(1).GetBigInteger() * 5000L * 100000000L / ratio;
                case "Quras.Contract.Create":
                case "Quras.Contract.Migrate":
                    return 5000L * 100000000L / ratio;
                case "Quras.Storage.Get":
                    return 21100L;
                case "Quras.Storage.Put":
                    return ((EvaluationStack.Peek(1).GetByteArray().Length + EvaluationStack.Peek(2).GetByteArray().Length - 1) / 1024 + 1) * 211000L;
                case "Quras.Storage.Delete":
                    return 21100L;
                default:
                    return 210L;
            }
        }

        public static ApplicationEngine Run(byte[] script, IScriptContainer container = null)
        {
            DataCache<UInt160, AccountState> accounts = Blockchain.Default.CreateCache<UInt160, AccountState>();
            DataCache<ECPoint, ValidatorState> validators = Blockchain.Default.CreateCache<ECPoint, ValidatorState>();
            DataCache<UInt256, AssetState> assets = Blockchain.Default.CreateCache<UInt256, AssetState>();
            DataCache<UInt160, ContractState> contracts = Blockchain.Default.CreateCache<UInt160, ContractState>();
            DataCache<StorageKey, StorageItem> storages = Blockchain.Default.CreateCache<StorageKey, StorageItem>();
            CachedScriptTable script_table = new CachedScriptTable(contracts);
            using (StateMachine service = new StateMachine(accounts, validators, assets, contracts, storages))
            {
                ApplicationEngine engine = new ApplicationEngine(TriggerType.Application, container, script_table, service, Fixed8.Zero, true);
                engine.LoadScript(script, false);
                engine.Execute();
                return engine;
            }
        }
    }
}
