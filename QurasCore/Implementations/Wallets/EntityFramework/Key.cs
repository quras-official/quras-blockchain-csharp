using System;
namespace Quras.Implementations.Wallets.EntityFramework
{
    internal class Key : IComparable<Key>
    {
        public string Name { get; set; }
        public byte[] Value { get; set; }

        public int CompareTo(Key other)
        {
            if (ReferenceEquals(this, other)) return 0;

            return Name.CompareTo(other.Name);
        }
    }
}
