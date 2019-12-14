using Quras.Core;
using Quras.Cryptography;
using Quras.SmartContract;
using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

using QurasCore.Wallets.AnonymousKey.Key;
using Quras.Core.Anonoymous;

namespace Quras.Wallets
{ 
    public class WitnessInfo : IEquatable<WitnessInfo>
    {
        public byte[] ScriptHash;
        public IntPtr Witness;
        public int WitnessHeight;

        public WitnessInfo(byte[] sHash, IntPtr w, int nHeight)
        {
            ScriptHash = sHash;
            Witness = w;
            WitnessHeight = nHeight;
        }

        public WitnessInfo(byte[] sHash, byte[] byWitness, int nHeight)
        {
            Witness = SnarkDllApi.CmWitness_Create();
            SnarkDllApi.SetCMWitnessFromBinary(Witness, byWitness, byWitness.Length);

            ScriptHash = sHash;
            WitnessHeight = nHeight;
        }

        public bool Equals(WitnessInfo other)
        {
            if (ReferenceEquals(this, other)) return true;
            if (ReferenceEquals(null, other)) return false;
            return Witness.Equals(Witness);
        }
    }
}
