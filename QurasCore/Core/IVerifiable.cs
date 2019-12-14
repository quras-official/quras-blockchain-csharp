using Quras.IO;
using Quras.VM;
using System.IO;

namespace Quras.Core
{
    public interface IVerifiable : ISerializable, IScriptContainer
    {
        Witness[] Scripts { get; set; }
        
        void DeserializeUnsigned(BinaryReader reader);

        UInt160[] GetScriptHashesForVerifying();
        
        void SerializeUnsigned(BinaryWriter writer);
    }
}
