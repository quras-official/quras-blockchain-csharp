using Quras.IO;
using Quras.IO.Json;
using System.IO;

namespace Quras.Core
{
    public class Witness : ISerializable
    {
        public byte[] InvocationScript;
        public byte[] VerificationScript;

        public int Size => InvocationScript.GetVarSize() + VerificationScript.GetVarSize();

        void ISerializable.Deserialize(BinaryReader reader)
        {
            InvocationScript = reader.ReadVarBytes(65536);
            VerificationScript = reader.ReadVarBytes(65536);
        }

        void ISerializable.Serialize(BinaryWriter writer)
        {
            writer.WriteVarBytes(InvocationScript);
            writer.WriteVarBytes(VerificationScript);
        }

        public JObject ToJson()
        {
            JObject json = new JObject();
            json["invocation"] = InvocationScript.ToHexString();
            json["verification"] = VerificationScript.ToHexString();
            return json;
        }

        public static Witness FromJson(JObject json)
        {
            Witness wit = new Witness();
            wit.InvocationScript = json["invocation"].AsString().HexToBytes();
            wit.VerificationScript = json["verification"].AsString().HexToBytes();
            return wit;
        }

        public JObject ToJsonString()
        {
            JObject json = new JObject();
            json["invocationScript"] = InvocationScript.ToHexString();
            json["verificationScript"] = VerificationScript.ToHexString();
            return json;
        }

        public static Witness FromJsonString(JObject json)
        {
            Witness wit = new Witness();
            wit.InvocationScript = json["invocationScript"].AsString().HexToBytes();
            wit.VerificationScript = json["verificationScript"].AsString().HexToBytes();
            return wit;
        }
    }
}
