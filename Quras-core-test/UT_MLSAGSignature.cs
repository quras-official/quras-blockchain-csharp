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
    public class UT_MLSAGSignature
    {
        [TestMethod]
        public void MLSAG_Allow_Test()
        {
            for (int Test_Count = 0; Test_Count < 1; Test_Count++)
            {
                List<byte[]> x = new List<byte[]>();
                List<List<ECPoint>> PK = new List<List<ECPoint>>();

                for (int i = 0; i < 5; i++)
                {
                    List<ECPoint> subPK = new List<ECPoint>();

                    for (int j = 0; j < 2; j++)
                    {
                        byte[] privX = SchnorrNonLinkable.GenerateRandomScalar();
                        ECPoint pubKey = ECCurve.Secp256r1.G * privX;

                        if (i == 3)
                        {
                            x.Add(privX);
                        }

                        subPK.Add(pubKey);
                    }

                    PK.Add(subPK);
                }

                MLSAGSignatureType sig = MLSAGSignature.Generate(PK, x, 3);

                MLSAGSignature.Verify(PK, sig).Should().Be(true);
            }
            
        }

        [TestMethod]
        public void MLSAG_Not_Allow_Test()
        {
            for (int Test_Count = 0; Test_Count < 1; Test_Count++)
            {
                List<byte[]> x = new List<byte[]>();
                List<List<ECPoint>> PK = new List<List<ECPoint>>();

                for (int i = 0; i < 2; i++)
                {
                    List<ECPoint> subPK = new List<ECPoint>();

                    for (int j = 0; j < 2; j++)
                    {
                        byte[] privX = SchnorrNonLinkable.GenerateRandomScalar();
                        ECPoint pubKey = ECCurve.Secp256r1.G * privX;

                        subPK.Add(pubKey);
                    }

                    PK.Add(subPK);
                }

                for (int i = 0; i < 2; i++)
                {
                    x.Add(SchnorrNonLinkable.GenerateRandomScalar());
                }

                MLSAGSignatureType sig = MLSAGSignature.Generate(PK, x, 1);

                MLSAGSignature.Verify(PK, sig).Should().Be(false);
            }

        }
    }
}
