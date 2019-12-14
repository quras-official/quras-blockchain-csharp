using Quras.VM;

namespace Quras.SmartContract
{
    internal class StorageContext : IInteropInterface
    {
        public UInt160 ScriptHash;

        public byte[] ToArray()
        {
            return ScriptHash.ToArray();
        }
    }
}
