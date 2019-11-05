using System;

using Pure.Core;

namespace Pure.Implementations.Wallets.EntityFramework
{
    internal class RCTCoin : IComparable<RCTCoin>
    {
        public byte[] TxId { get; set; }
        public byte[] TxRCTHash { get; set; }
        public ushort RctID { get; set; }
        public ushort Index { get; set; }

        public byte[] AssetId { get; set; }
        public long Value { get; set; }

        public byte[] PubKey { get; set; }

        public byte[] ScriptHash { get; set; }

        public CoinState State { get; set; }

        public int CompareTo(RCTCoin other)
        {
            if (ReferenceEquals(this, other)) return 0;

            return TxId.ToHexString().CompareTo(other.TxId.ToHexString());
        }
    }
}
