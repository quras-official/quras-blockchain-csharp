using System;
using System.Collections.Generic;
using System.Text;

namespace Pure.Core.RingCT.Types
{
    public class RingSignature
    {
        public static readonly int RING_SIZE = 4;
        public readonly byte[] KeyImage;
        public readonly List<byte[]> C;
        public readonly List<byte[]> R;

        public RingSignature(byte[] keyImage, List<byte[]> c, List<byte[]> r)
        {
            this.KeyImage = keyImage;
            this.C = c;
            this.R = r;
        }
    }
}
