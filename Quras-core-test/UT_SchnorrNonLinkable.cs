using System;
using System.Linq;
using System.Numerics;
using System.Collections.Generic;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Pure;
using Pure.Cryptography.ECC;
using Pure.Core.RingCT.Types;
using Pure.Core.RingCT.Impls;

namespace Quras_core_test
{
    [TestClass]
    public class UT_SchnorrNonLinkable
    {
        [TestMethod]
        public void BigIntegerTest()
        {
            byte[] a = SchnorrNonLinkable.GenerateRandomScalar();

            BigInteger b_a = new BigInteger(a.Reverse().Concat(new byte[1]).ToArray());

            for (int i = 0; i < 32; i ++)
            {
                a[i].Should().Be(b_a.ToByteArray().Reverse().ToArray()[i]);
            }
        }

        [TestMethod]
        public void SchnorrNonLinkableAllowTest()
        {
            for (int i = 0; i < 64; i++)
            {
                byte[] x1 = SchnorrNonLinkable.GenerateRandomScalar();
                byte[] x2 = SchnorrNonLinkable.GenerateRandomScalar();

                ECPoint P1 = ECCurve.Secp256r1.G * x1;
                ECPoint P2 = ECCurve.Secp256r1.G * x2;

                Random rnd = new Random(i);
                int iRand = rnd.Next(2);

                if (iRand == 0)
                {
                    SchnorrSignatureType sig = SchnorrNonLinkable.Generate(x1, P1, P2, 0);

                    SchnorrNonLinkable.Verify(P1, P2, sig).Should().Be(true);
                }
                else
                {
                    SchnorrSignatureType sig = SchnorrNonLinkable.Generate(x2, P1, P2, 1);

                    SchnorrNonLinkable.Verify(P1, P2, sig).Should().Be(true);
                }
            }
        }

        [TestMethod]
        public void SchnorrNonLinkableNotAllowTest01()
        {
            byte[] x1 = SchnorrNonLinkable.GenerateRandomScalar();
            byte[] x2 = SchnorrNonLinkable.GenerateRandomScalar();

            ECPoint P1 = ECCurve.Secp256r1.G * x1;
            ECPoint P2 = ECCurve.Secp256r1.G * x2;

            SchnorrSignatureType sig = SchnorrNonLinkable.Generate(x1, P1, P2, 1);

            SchnorrNonLinkable.Verify(P1, P2, sig).Should().Be(false);
        }

        [TestMethod]
        public void SchnorrNonLinkableNotAllowTest02()
        {
            byte[] x1 = SchnorrNonLinkable.GenerateRandomScalar();
            byte[] x11 = SchnorrNonLinkable.GenerateRandomScalar();
            byte[] x2 = SchnorrNonLinkable.GenerateRandomScalar();

            ECPoint P1 = ECCurve.Secp256r1.G * x1;
            ECPoint P2 = ECCurve.Secp256r1.G * x2;

            SchnorrSignatureType sig = SchnorrNonLinkable.Generate(x11, P1, P2, 0);

            SchnorrNonLinkable.Verify(P1, P2, sig).Should().Be(false);
        }
    }
}