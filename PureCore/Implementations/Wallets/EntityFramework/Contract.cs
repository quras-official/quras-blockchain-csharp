using System;
namespace Pure.Implementations.Wallets.EntityFramework
{
    internal class Contract : IComparable<Contract>
    {
        public byte[] RawData { get; set; }
        public byte[] ScriptHash { get; set; }
        public byte[] PublicKeyHash { get; set; }
        public Account Account { get; set; }
        public Address Address { get; set; }

        public int CompareTo(Contract other)
        {
            if (ReferenceEquals(this, other)) return 0;

            return ScriptHash.ToHexString().CompareTo(other.ScriptHash.ToHexString());
        }
    }
}
