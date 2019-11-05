using System;
namespace Pure.Implementations.Wallets.EntityFramework
{
    internal class Address : IComparable<Address>
    {
        public byte[] ScriptHash { get; set; }

        public int CompareTo(Address other)
        {
            if (ReferenceEquals(this, other)) return 0;

            return ScriptHash.ToHexString().CompareTo(other.ScriptHash.ToHexString());
        }
    }
}
