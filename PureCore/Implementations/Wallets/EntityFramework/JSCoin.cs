using Pure.Core;
using System;

namespace Pure.Implementations.Wallets.EntityFramework
{
    internal class JSCoin : IComparable<JSCoin>
    {
        public byte[] TxId { get; set; }
        public ushort JsId { get; set; }
        public ushort Index { get; set; }
        public byte[] AssetId { get; set; }
        public long Value { get; set; }
        public byte[] ScriptHash { get; set; }
        public byte[] r { get; set; }
        public byte[] rho { get; set; }
        public byte[] Witness { get; set; }
        public long WitnessHeight { get; set; }
        public long CMTreeHeight { get; set; }
        public CoinState State { get; set; }
        public Address Address { get; set; }

        public int CompareTo(JSCoin other)
        {
            if (ReferenceEquals(this, other)) return 0;

            return TxId.ToHexString().CompareTo(other.TxId.ToHexString());
        }
    }
}
