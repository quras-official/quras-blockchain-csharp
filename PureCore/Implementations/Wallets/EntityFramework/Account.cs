using System;
namespace Pure.Implementations.Wallets.EntityFramework
{
    internal class Account : IComparable<Account>
    {
        public int nVersion { get; set; }
        public byte[] PrivateKeyEncrypted { get; set; }
        public byte[] PublicKeyHash { get; set; }
        public byte[] PrivateViewKeyEncrypted { get; set; }

        public int CompareTo(Account other)
        {
            if (ReferenceEquals(this, other)) return 0;

            return PublicKeyHash.ToHexString().CompareTo(other.PublicKeyHash.ToHexString());
        }
    }
}
