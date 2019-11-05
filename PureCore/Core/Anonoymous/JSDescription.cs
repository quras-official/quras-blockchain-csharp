using System;
using System.Collections.Generic;
using System.Text;
using Pure;
namespace Pure.Core.Anonoymous
{
    public class JSDescription
    {
        public Fixed8 vpub_old;
        public Fixed8 vpub_new;

        public UInt256 anchor;
        public List<UInt256> nullifiers;
        public List<UInt256> commitments;

        public UInt256 ephermeralKey;

        public List<byte[]> ciphertexts;

        public UInt256 randomSeed;

        public List<UInt256> macs;

        public QrsProof proof;
        public JSDescription()
        {
            vpub_new = new Fixed8(0);
            vpub_old = new Fixed8(0);

            anchor = new UInt256();
            nullifiers = new List<UInt256>();
            commitments = new List<UInt256>();
            ephermeralKey = new UInt256();
            ciphertexts = new List<byte[]>();
            randomSeed = new UInt256();
            macs = new List<UInt256>();
        }

        public JSDescription(
                QrsJoinSplit qrsParams,
                UInt256 pubKeyHash,
                UInt256 anchor,
                List<JSInput> inputs,
                List<JSOutput> outputs,
                Fixed8 vpub_old,
                Fixed8 vpub_new,
                bool computeProof,
                UInt256 esk
            )
        {
            List<Note> notes = new List<Note>();
            anchor = new UInt256();
            nullifiers = new List<UInt256>();
            commitments = new List<UInt256>();
            ephermeralKey = new UInt256();
            ciphertexts = new List<byte[]>();
            randomSeed = new UInt256();
            macs = new List<UInt256>();

            proof = qrsParams.prove(
                inputs,
                outputs,
                notes,
                ciphertexts,
                ephermeralKey,
                pubKeyHash,
                randomSeed,
                macs,
                nullifiers,
                commitments,
                vpub_old,
                vpub_new,
                anchor,
                computeProof,
                esk);
        }

        public static JSDescription Randomized(
            QrsJoinSplit qrsParams,
            UInt256 pubKeyHash,
            UInt256 anchor,
            List<JSInput> inputs,
            List<JSOutput> outputs,
            Fixed8 vpub_old,
            Fixed8 vpub_new,
            bool computeProof,
            UInt256 esk
            )
        {
            return new JSDescription(
                qrsParams, pubKeyHash, anchor, inputs, outputs,
                vpub_old, vpub_new, computeProof,
                esk // payment disclosure
            );
        }
    }
}
