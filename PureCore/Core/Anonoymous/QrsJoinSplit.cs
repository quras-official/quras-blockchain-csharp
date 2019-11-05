using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;

using PureCore.Wallets.AnonymousKey.Note;
using PureCore.Wallets.AnonymousKey.PRF;
namespace Pure.Core.Anonoymous
{
    public class QrsJoinSplit
    {
        public static void Generate(string r1csPath, string vkPath, string pkPath)
        {

        }

        //public static QrsJoinSplit Prepared(string vkPath, string pkPath)
        //{

        //}

        public static UInt256 h_sig(UInt256 randomSeed, List<UInt256> nullifiers, UInt256 pubKeyHash)
        {
            byte[] block = new byte[randomSeed.Size + nullifiers.Count * 32 + pubKeyHash.Size];
            byte[] salt = new byte[Common.crypto_generichash_blake2b_PERSONALBYTES];
            byte[] personal = { 0x51, 0x75, 0x72, 0x61, 0x73, 0x43, 0x6f, 0x6d, 0x70, 0x75, 0x74, 0x68, 0x53, 0x69, 0x67, 0x93 };
            Buffer.BlockCopy(randomSeed.ToArray(), 0, block, 0, randomSeed.Size);
            for (int i = 0; i < nullifiers.Count; i ++)
            {
                Buffer.BlockCopy(nullifiers[i].ToArray(), 0, block, 32 + i * 32, 32);
            }
            Buffer.BlockCopy(pubKeyHash.ToArray(), 0, block, 32 + nullifiers.Count * 32, pubKeyHash.Size);

            byte[] byOutPut;

            byOutPut = Sodium.GenericHash.HashSaltPersonal(block, null, salt, personal, Common.NOTEENCRYPTION_CIPHER_KEYSIZE);
            return new UInt256(byOutPut);
        }

        public virtual QrsProof prove(
                List<JSInput> inputs,
                List<JSOutput> outputs,
                List<Note> out_notes,
                List<byte[]> out_ciphertexts,
                UInt256 out_ephemeralKey,
                UInt256 pubKeyHash,
                UInt256 out_randomSeed,
                List<UInt256> out_macs,
                List<UInt256> out_nullifiers,
                List<UInt256> out_commitments,
                Fixed8 vpub_old,
                Fixed8 vpub_new,
                UInt256 rt,
                bool computeProof = true,
                // For paymentdisclosure, we need to retrieve the esk.
                // Reference as non-const parameter with default value leads to compile error.
                // So use pointer for simplicity.
                UInt256 out_esk = null
            )
        {
            Fixed8 lhs_value = vpub_old;
            Fixed8 rhs_value = vpub_new;
            for (int i = 0; i < inputs.Count; i ++)
            {
                lhs_value += inputs[i].note.value;
                out_nullifiers.Add(inputs[i].Nullifier());
            }

            out_randomSeed = UInt256.Random();

            UInt256 h_sig = QrsJoinSplit.h_sig(out_randomSeed, out_nullifiers, pubKeyHash);

            UInt252 phi = new UInt252(UInt256.Random());

            for (int i = 0; i < outputs.Count; i ++)
            {
                rhs_value += outputs[i].value;

                UInt256 r = UInt256.Random();

                out_notes.Add(outputs[i].note(phi, r, new Fixed8(i), h_sig));
            }

            if (lhs_value != rhs_value)
            {
                throw new ArgumentException();
            }

            for (int i = 0; i < outputs.Count; i ++)
            {
                out_commitments.Add(out_notes[i].CM());
            }

            {
                NoteEncryption encryptor = new NoteEncryption(h_sig);

                for (int i = 0; i < outputs.Count; i++)
                {
                    NotePlaintext pt = new NotePlaintext(out_notes[i], outputs[i].memo);
                    out_ciphertexts.Add(pt.encrypt(encryptor, outputs[i].addr.pk_enc));
                }

                out_ephemeralKey = encryptor.get_epk();

                out_esk = encryptor.get_esk();
            }

            for (int i = 0; i < inputs.Count; i++)
            {
                out_macs.Add(PRFClass.PRF_pk(inputs[i].key, new Fixed8(i), h_sig));
            }



            return null;
        }

        public virtual bool verify(
            QrsProof proof,
            //ProofVerifier verifier,
            UInt256 pubKeyHash,
            UInt256 randomSeed,
            List<UInt256> hmacs,
            List<UInt256> nullifiers,
            List<UInt256> commitments,
            Fixed8 vpub_old,
            Fixed8 vpub_new,
            UInt256 rt
        )
        {
            return false;
        }
    }
}
