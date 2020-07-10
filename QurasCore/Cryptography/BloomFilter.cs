﻿using System.Collections;
using System.Linq;

namespace Quras.Cryptography
{
    public class BloomFilter
    {
        private readonly uint[] seeds;
        private readonly BitArray bits;

        public int K => seeds.Length;

        public int M => bits.Length;

        public uint Tweak { get; private set; }

        public BloomFilter(int m, int k, uint nTweak, byte[] elements = null)
        {
            if (m != elements.Length)
                throw new System.Exception("The Elements length does not match with M.");

            this.seeds = Enumerable.Range(0, k).Select(p => (uint)p * 0xFBA4C795 + nTweak).ToArray();
            this.bits = elements == null ? new BitArray(m) : new BitArray(elements);
            this.bits.Length = m;
            this.Tweak = nTweak;
        }

        public void Add(byte[] element)
        {
            foreach (uint i in seeds.AsParallel().Select(s => element.Murmur32(s)))
                bits.Set((int)(i % (uint)bits.Length), true);
        }

        public bool Check(byte[] element)
        {
            foreach (uint i in seeds.AsParallel().Select(s => element.Murmur32(s)))
                if (!bits.Get((int)(i % (uint)bits.Length)))
                    return false;
            return true;
        }
        
        public void GetBits(byte[] newBits)
        {
#if NET461
            bits.CopyTo(newBits, 0);
#else
            // the method 'BitArray.CopyTo' is not available in .Net Core
            for (int i = 0; i < bits.Length; i++)
                if (bits.Get(i))
                    newBits[i / 8] |= (byte)(1 << (i % 8));
#endif
        }
    }
}
