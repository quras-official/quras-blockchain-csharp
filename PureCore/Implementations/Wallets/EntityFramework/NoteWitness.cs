using System;

namespace Pure.Implementations.Wallets.EntityFramework
{
    internal class NoteWitness : IComparable<NoteWitness>
    {
        public byte[] ScriptHash { get; set; }
        public byte[] byWitness { get; set; }
        public int WitnessHeight { get; set; }

        public int CompareTo(NoteWitness other)
        {
            if (ReferenceEquals(this, other)) return 0;

            return ScriptHash.ToHexString().CompareTo(other.ScriptHash.ToHexString());
        }
    }
}
