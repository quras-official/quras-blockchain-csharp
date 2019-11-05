using System;
using System.Linq;
using System.Collections.Generic;

using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Pure;
using Pure.Core.RingCT.Types;
using Pure.Core.RingCT.Impls;
using Pure.Cryptography.ECC;

namespace Quras_core_test
{
    [TestClass]
    public class UT_ASNLSignature
    {
        [TestMethod]
        public void ASNL_Allow_Test()
        {
            List<byte[]> x = new List<byte[]>();
            List<ECPoint> P1 = new List<ECPoint>();
            List<ECPoint> P2 = new List<ECPoint>();
            List<int> indices = new List<int>();
            
            for (int i = 0; i < ASNLRingSignature.AMOUNT_SIZE; i++)
            {
                //byte[] x1 = SchnorrNonLinkable.GenerateRandomScalar();
                byte[] x1 = new byte[32];
                byte[] x2 = SchnorrNonLinkable.GenerateRandomScalar();

                ECPoint p1 = ECCurve.Secp256r1.G * x1;
                ECPoint p2 = ECCurve.Secp256r1.G * x2;

                P1.Add(p1);
                P2.Add(p2);

                Random rnd = new Random(i);
                int iRand = rnd.Next(2);

                if (iRand == 0)
                {
                    x.Add(x1);
                    indices.Add(0);
                }
                else
                {
                    x.Add(x2);
                    indices.Add(1);
                }
            }

            ASNLSignatureType sig = ASNLRingSignature.Generate(x, P1, P2, indices);

            ASNLRingSignature.Verify(P1, P2, sig).Should().Be(true);
        }
    }
}
