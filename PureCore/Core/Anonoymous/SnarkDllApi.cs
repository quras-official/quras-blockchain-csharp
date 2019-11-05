using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Reflection;

namespace Pure.Core.Anonoymous
{
    public struct Temp
    {
        public IntPtr m_iVal;
        public string m_strVal;
    }

    public class SnarkDllApi
    {
        [DllImport("./crypto/Quras_snarks.dll", EntryPoint = "Do_TEST")]
        public static extern void Snark_Do_TEST(out Temp i); //For Windows

        [DllImport("./crypto/Quras_snarks.dll", EntryPoint = "MakeProof")]
        public static extern bool Snark_MakeProof(byte[] pubKeyHash, byte[] anchor, Pure.Core.Anonoymous.JSInput[] input, Pure.Core.Anonoymous.JSOutput[] output, long vpub_old, long vpub_new); //For Windows

        [DllImport("./crypto/Quras_snarks.dll", EntryPoint = "DLL_INIT")]
        public static extern int Snark_DllInit(int nMode, char[] vkPath, char[] pkPath); //For Windows

        /// <summary>
        /// AsyncJoinSplit moduel
        /// </summary>
        /// <returns></returns>

        [DllImport("./crypto/Quras_snarks.dll", EntryPoint = "AsyncJoinSplitInfo_Create")]
        public static extern IntPtr Snark_AsyncJoinSplitInfo_Create(); //For Windows

        [DllImport("./crypto/Quras_snarks.dll", EntryPoint = "AsyncJoinSplitInfo_Delete")]
        public static extern void Snark_AsyncJoinSplitInfo_Delete(IntPtr param);

        [DllImport("./crypto/Quras_snarks.dll", EntryPoint = "AsyncJoinSplitInfo_Add_JSOutput")]
        public static extern void Snark_AsyncJoinSplitInfo_Add_JSOutput(IntPtr asyncJSI, IntPtr jsoutput);

        [DllImport("./crypto/Quras_snarks.dll", EntryPoint = "AsyncJoinSplitInfo_Add_JSInput")]
        public static extern void Snark_AsyncJoinSplitInfo_Add_JSInput(IntPtr asyncJSI, IntPtr jsinput);

        [DllImport("./crypto/Quras_snarks.dll", EntryPoint = "AsyncJoinSplitInfo_Add_Notes")]
        public static extern void Snark_AsyncJoinSplitInfo_Add_Notes(IntPtr asyncJSI, IntPtr note);

        [DllImport("./crypto/Quras_snarks.dll", EntryPoint = "AsyncJoinSplitInfo_Add_Amount")]
        public static extern void Snark_AsyncJoinSplitInfo_Add_Amounts(IntPtr asyncJSI, long vpub_old, long vpub_new, byte[] Asset_ID);

        // JSOutput
        [DllImport("./crypto/Quras_snarks.dll", EntryPoint = "Jsoutput_Create")]
        public static extern IntPtr Snark_Jsoutput_Create();

        [DllImport("./crypto/Quras_snarks.dll", EntryPoint = "Jsoutput_Delete")]
        public static extern void Snark_Jsoutput_Delete(IntPtr pJsOutput);

        [DllImport("./crypto/Quras_snarks.dll", EntryPoint = "Jsoutput_Init")]
        public static extern void Snark_Jsoutput_Init(IntPtr pJsOutput, byte[] a_pk, byte[] pk_enc, long value, byte[] memo, byte[] assetID);

        // Note
        [DllImport("./crypto/Quras_snarks.dll", EntryPoint = "Note_Create")]
        public static extern IntPtr Note_Create();

        [DllImport("./crypto/Quras_snarks.dll", EntryPoint = "Note_Delete")]
        public static extern void Note_Delete(IntPtr pNote);

        [DllImport("./crypto/Quras_snarks.dll", EntryPoint = "Note_Init")]
        public static extern void Note_Init(IntPtr pNote, byte[] a_pk, byte[] rho, byte[] r, long value, byte[] assetID);

        [DllImport("./crypto/Quras_snarks.dll", EntryPoint = "GetNullifier")]
        public static extern void GetNullifier(byte[] a_pk_, byte[] rho_, byte[] r_, long value, byte[] a_sk_, byte[] out_nullifier);

        // Perform Joinsplit
        [DllImport("./crypto/Quras_snarks.dll", EntryPoint = "Perform_joinsplit")]
        public static extern IntPtr Snark_Perform_joinsplit(IntPtr pAsyncJSI, byte[] jsPubkey, byte[] jsPrivKey, int[] out_length, byte[] spendingkey, byte[] byAnchor, IntPtr w);

        [DllImport("./crypto/Quras_snarks.dll", EntryPoint = "JSVerify")]
        public static extern bool Snark_JSVerify(byte[] bytes, byte[] jsPubkey);

        // Check JoinSplit
        [DllImport("./crypto/Quras_snarks.dll", EntryPoint = "FindMyNotes")]
        public static extern IntPtr Snark_FindMyNotes(byte[] jsdescription, byte[] private_key, byte[] join_split_pub_key);

        // Key
        [DllImport("./crypto/Quras_snarks.dll", EntryPoint = "ReceivingKey_Pk_enc")]
        public static extern IntPtr Key_ReceivingKey_Pk_enc(byte[] receiving_key);

        [DllImport("./crypto/Quras_snarks.dll", EntryPoint = "SpendingKey_ReceivingKey")]
        public static extern IntPtr Key_SpendingKey_ReceivingKey(byte[] spending_key);

        [DllImport("./crypto/Quras_snarks.dll", EntryPoint = "SpendingKey_Viewing_key")]
        public static extern IntPtr Key_SpendingKey_Viewing_key(byte[] spending_key);

        [DllImport("./crypto/Quras_snarks.dll", EntryPoint = "SpendingKey_Address")]
        public static extern IntPtr Key_SpendingKey_Address(byte[] spending_key);

        // CMMerkleTree
        [DllImport("./crypto/Quras_snarks.dll", EntryPoint = "CmMerkleTree_Create")]
        public static extern IntPtr CmMerkleTree_Create();

        [DllImport("./crypto/Quras_snarks.dll", EntryPoint = "GetCMTreeInBinary")]
        public static extern IntPtr GetCMTreeInBinary(IntPtr tree, int[] outlen);

        [DllImport("./crypto/Quras_snarks.dll", EntryPoint = "SetCMTreeFromBinary")]
        public static extern bool SetCMTreeFromBinary(IntPtr tree, byte[] bnTree, int outlen);

        [DllImport("./crypto/Quras_snarks.dll", EntryPoint = "AppendCommitment")]
        public static extern bool AppendCommitment(IntPtr tree, byte[] cm);

        [DllImport("./crypto/Quras_snarks.dll", EntryPoint = "GetCMRoot")]
        public static extern IntPtr GetCMRoot(IntPtr tree);

        // Witness
        [DllImport("./crypto/Quras_snarks.dll", EntryPoint = "CmWitness_Create")]
        public static extern IntPtr CmWitness_Create();

        [DllImport("./crypto/Quras_snarks.dll", EntryPoint = "SetCMTreeFromOthers")]
        public static extern bool SetCMTreeFromOthers(IntPtr dst, IntPtr src);

        [DllImport("./crypto/Quras_snarks.dll", EntryPoint = "GetWitnessFromMerkleTree")]
        public static extern void GetWitnessFromMerkleTree(IntPtr dst, IntPtr src);

        [DllImport("./crypto/Quras_snarks.dll", EntryPoint = "GetCMWitnessInBinary")]
        public static extern IntPtr GetCMWitnessInBinary(IntPtr tree, int[] outlen);

        [DllImport("./crypto/Quras_snarks.dll", EntryPoint = "SetCMWitnessFromBinary")]
        public static extern bool SetCMWitnessFromBinary(IntPtr tree, byte[] bnTree, int outlen);

        [DllImport("./crypto/Quras_snarks.dll", EntryPoint = "AppendCommitmentInWitness")]
        public static extern bool AppendCommitmentInWitness(IntPtr tree, byte[] cm);

        [DllImport("./crypto/Quras_snarks.dll", EntryPoint = "GetCMRootFromWitness")]
        public static extern IntPtr GetCMRootFromWitness(IntPtr tree);

        [DllImport("./crypto/Quras_snarks.dll", EntryPoint = "GetWitnessInBytes")]
        public static extern IntPtr GetWitnessInBytes(IntPtr tree, int[] outlen);

        [DllImport("./crypto/Quras_snarks.dll", EntryPoint = "AddCmInWitness")]
        public static extern IntPtr AddCmInWitness(byte[] byWitness, int size, byte[] cm, int[] out_length);

        [DllImport("./crypto/Quras_snarks.dll", EntryPoint = "Witnesses_Create")]
        public static extern IntPtr Witnesses_Create();

        [DllImport("./crypto/Quras_snarks.dll", EntryPoint = "Witnesses_Add")]
        public static extern void Witnesses_Add(IntPtr witnessVec, IntPtr witness);

        [DllImport("./crypto/Quras_snarks.dll", EntryPoint = "Witnesses_Clear")]
        public static extern void Witnesses_Clear(IntPtr witnessVec);

        // Free Memory
        [DllImport("./crypto/Quras_snarks.dll", EntryPoint = "FreeMemory")]
        public static extern void Snark_FreeMeomory(IntPtr memory);
    }
}
