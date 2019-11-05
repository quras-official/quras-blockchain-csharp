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
    public class UT_Helper_RingCT
    {
        [TestMethod]
        public void Test_ToBinaryFormat()
        {
            Fixed8 amount = new Fixed8(100);    // 0110 0100
            List<int> binary = amount.ToBinaryFormat();

            List<int> expectBinary = new List<int>();
            expectBinary.Add(0);
            expectBinary.Add(0);
            expectBinary.Add(1);
            expectBinary.Add(0);
            expectBinary.Add(0);
            expectBinary.Add(1);
            expectBinary.Add(1);
            expectBinary.Add(0);

            for (int i = 0; i < 64 - 8; i++)
            {
                expectBinary.Add(0);
            }

            for (int i = 0; i < 64; i ++)
            {
                binary[i].Should().Be(expectBinary[i]);
            }
        }

        [TestMethod]
        public void ECDH_ENCODE_TEST()
        {
            byte[] RecieverSK = SchnorrNonLinkable.GenerateRandomScalar();
            ECPoint RecieverPK = ECCurve.Secp256r1.G * RecieverSK;

            byte[] u_mask = new byte[32];
            byte[] u_amount = new byte[32];
            ECPoint pt = new ECPoint();

            EcdhTuple unmask = new EcdhTuple(u_mask, u_amount, pt);
            
            EcdhTuple mask = unmask.EcdhEncode(RecieverPK);

            EcdhTuple t_unmask = mask.EcdhDecode(RecieverSK);

            unmask.amount.ToHexString().Should().Be(t_unmask.amount.ToHexString());
            unmask.mask.ToHexString().Should().Be(t_unmask.mask.ToHexString());
        }
    }
}
