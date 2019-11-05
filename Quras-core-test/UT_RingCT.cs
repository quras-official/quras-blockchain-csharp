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
    public class UT_RingCT
    {
        [TestMethod]
        public void Test_ProveRctMG()
        {
            List<List<CTKey>> pubs = new List<List<CTKey>>();
            List<CTCommitment> inSK = new List<CTCommitment>();
            List<CTCommitment> outSK = new List<CTCommitment>();
            List<CTKey> outPK = new List<CTKey>();
            int index;

            List<CTKey> pub_index = new List<CTKey>();
            byte[] out_amount = new byte[32];
            byte[] out_mask = new byte[32];
            for (int i = 0; i < 2; i++)
            {
                byte[] sk = SchnorrNonLinkable.GenerateRandomScalar();
                ECPoint pk = ECCurve.Secp256r1.G * sk;

                byte[] mask = SchnorrNonLinkable.GenerateRandomScalar();

                Fixed8 amount = Fixed8.One * 100;
                byte[] b_amount = amount.ToBinaryFormat().ToBinary();

                ECPoint C_i_in = RingCTSignature.GetCommitment(mask, b_amount);

                out_amount = ScalarFunctions.Add(out_amount, b_amount);
                out_mask = ScalarFunctions.Add(out_mask, mask);

                CTKey key = new CTKey(pk, C_i_in);

                CTCommitment i_insk = new CTCommitment(sk, mask);
                inSK.Add(i_insk);

                pub_index.Add(key);
            }
            pubs.Add(pub_index);

            for (int i = 0; i < 2; i++)
            {
                List<CTKey> pub_i = new List<CTKey>();
                for (int j = 0; j < 2; j++)
                {
                    CTKey key = new CTKey(SchnorrNonLinkable.GenerateRandomPoint(), SchnorrNonLinkable.GenerateRandomPoint());
                    pub_i.Add(key);
                }
                pubs.Add(pub_i);
            }

            // Out PK, outSK
            byte[] out_secretKey = SchnorrNonLinkable.GenerateRandomScalar();
            ECPoint out_publicKey = ECCurve.Secp256r1.G * out_secretKey;

            byte[] out_z = SchnorrNonLinkable.GenerateRandomScalar();
            out_mask = ScalarFunctions.Sub(out_mask, out_z);

            ECPoint C_i_out = RingCTSignature.GetCommitment(out_mask, out_amount);

            CTCommitment i_outsk = new CTCommitment(new byte[32], out_mask);
            outSK.Add(i_outsk);

            CTKey out_key = new CTKey(out_publicKey, C_i_out);
            outPK.Add(out_key);

            MLSAGSignatureType sig = RingCTSignature.ProveRctMG(pubs, inSK, outSK, outPK, Fixed8.Zero, 0);
            RingCTSignature.VerRctMG(sig, pubs, outPK, Fixed8.Zero).Should().Be(true);
        }

        [TestMethod]
        public void RingCT_Test()
        {
            List<CTCommitment> inSK = new List<CTCommitment>();
            List<CTKey> inPK = new List<CTKey>();
            List<MixRingCTKey> inPKIndex = new List<MixRingCTKey>();
            List<ECPoint> destinations = new List<ECPoint>();
            List<Fixed8> amounts = new List<Fixed8>();
            int mixin = 3;

            byte[] out_amount = new byte[32];
            byte[] out_mask = new byte[32];

            for (int i = 0; i < 2; i++)
            {
                byte[] sk = new byte[32]; //SchnorrNonLinkable.GenerateRandomScalar();
                ECPoint pk = ECCurve.Secp256r1.G * sk;

                byte[] mask = new byte[32]; //SchnorrNonLinkable.GenerateRandomScalar();    /// Get from Tx

                Fixed8 amount = Fixed8.One * 100;
                byte[] b_amount = amount.ToBinaryFormat().ToBinary();

                ECPoint C_i_in = RingCTSignature.GetCommitment(mask, b_amount);

                out_amount = ScalarFunctions.Add(out_amount, b_amount);
                out_mask = ScalarFunctions.Add(out_mask, mask);

                CTKey key = new CTKey(pk, C_i_in);
                UInt256 genesisHash = new UInt256(new byte[32]);
                MixRingCTKey mixRingKey = new MixRingCTKey(genesisHash, 0xff, 0xff);

                CTCommitment i_insk = new CTCommitment(sk, mask);
                inSK.Add(i_insk);

                inPK.Add(key);
                inPKIndex.Add(mixRingKey);
            }

            byte[] dest_sk = SchnorrNonLinkable.GenerateRandomScalar();
            ECPoint dest_pk = ECCurve.Secp256r1.G * dest_sk;
            destinations.Add(dest_pk);

            amounts.Add(Fixed8.One * 201);

            RingCTSignatureType sig = RingCTSignature.Generate(inSK, inPKIndex, destinations, amounts, -Fixed8.One, mixin, Pure.Core.Blockchain.GoverningToken.Hash, Fixed8.Zero);
            RingCTSignature.Verify(sig, Fixed8.Zero).Should().Be(true);

            byte[] tmask;
            Fixed8 dstAmount = RingCTSignature.DecodeRct(sig, dest_sk, 0, out tmask);
            dstAmount.Should().Be(Fixed8.One * 201);
        }

        [TestMethod]
        public void RingCT_Not_Allowed_Test()
        {
            List<CTCommitment> inSK = new List<CTCommitment>();
            List<CTKey> inPK = new List<CTKey>();
            List<MixRingCTKey> inPKIndex = new List<MixRingCTKey>();
            List<ECPoint> destinations = new List<ECPoint>();
            List<Fixed8> amounts = new List<Fixed8>();
            int mixin = 3;

            byte[] out_amount = new byte[32];
            byte[] out_mask = new byte[32];

            for (int i = 0; i < 2; i++)
            {
                byte[] sk = SchnorrNonLinkable.GenerateRandomScalar();
                ECPoint pk = ECCurve.Secp256r1.G * sk;

                byte[] mask = SchnorrNonLinkable.GenerateRandomScalar();

                Fixed8 amount = Fixed8.One * 150;
                byte[] b_amount = amount.ToBinaryFormat().ToBinary();

                ECPoint C_i_in = RingCTSignature.GetCommitment(mask, b_amount);

                out_amount = ScalarFunctions.Add(out_amount, b_amount);
                out_mask = ScalarFunctions.Add(out_mask, mask);

                CTKey key = new CTKey(pk, C_i_in);
                MixRingCTKey mixRingKey = new MixRingCTKey(Pure.Core.Blockchain.GenesisBlock.Hash, 0xff, 0xff);

                CTCommitment i_insk = new CTCommitment(sk, mask);
                inSK.Add(i_insk);

                inPK.Add(key);
                inPKIndex.Add(mixRingKey);
            }

            byte[] dest_sk = SchnorrNonLinkable.GenerateRandomScalar();
            ECPoint dest_pk = ECCurve.Secp256r1.G * dest_sk;
            destinations.Add(dest_pk);

            amounts.Add(Fixed8.One * 200);

            RingCTSignatureType sig = RingCTSignature.Generate(inSK, inPKIndex, destinations, amounts, Fixed8.Zero, mixin, Pure.Core.Blockchain.GoverningToken.Hash, Fixed8.Zero);
            RingCTSignature.Verify(sig, Fixed8.Zero).Should().Be(false);

            byte[] tmask;
            Fixed8 dstAmount = RingCTSignature.DecodeRct(sig, dest_sk, 0, out tmask);
            dstAmount.Should().Be(Fixed8.One * 200);
        }
    }
}
