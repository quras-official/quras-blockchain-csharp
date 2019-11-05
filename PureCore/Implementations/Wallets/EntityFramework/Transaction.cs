using Pure.Core;
using System;

namespace Pure.Implementations.Wallets.EntityFramework
{

    internal class Transaction : IComparable<Transaction>
    {
        public byte[] Hash { get; set; }
        public TransactionType Type { get; set; }
        public byte[] RawData { get; set; }
        public uint? Height { get; set; }
        public DateTime Time { get; set; }

        public int CompareTo(Transaction other)
        {
            if (ReferenceEquals(this, other)) return 0;
            
            if (Time < other.Time)
            {
                return -1;
            }
            else
            {
                return 1;
            }
        }
    }
}
