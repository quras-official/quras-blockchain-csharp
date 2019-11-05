using Pure.IO;
using System.Collections.Generic;
using System.IO;

namespace Pure.Core
{
    public class NullifierState : StateBase
    {
        public UInt256 Nullifier;
        public override int Size => base.Size + Nullifier.Size;

        public override void Deserialize(BinaryReader reader)
        {
            base.Deserialize(reader);
            Nullifier = reader.ReadSerializable<UInt256>();
        }

        public override void Serialize(BinaryWriter writer)
        {
            base.Serialize(writer);
            writer.Write(Nullifier);
        }
    }
}
