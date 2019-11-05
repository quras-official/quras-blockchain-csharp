using Pure.Core;
using System;
namespace Pure.Implementations.Wallets.EntityFramework
{
    internal class Coin : IComparable<Coin>
    {
        public byte[] TxId { get; set; }
        public ushort Index { get; set; }
        public byte[] AssetId { get; set; }
        public long Value { get; set; }
        public byte[] ScriptHash { get; set; }
        public CoinState State { get; set; }
        public Address Address { get; set; }

        public int CompareTo(Coin other)
        {
            if (ReferenceEquals(this, other)) return 0;

            return TxId.ToHexString().CompareTo(other.TxId.ToHexString());
        }
    }
}
