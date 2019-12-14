using Quras.IO;
using System.IO;

namespace Quras.Network.Payloads
{
    internal class AddrPayload : ISerializable
    {
        public NetworkAddressWithTime[] AddressList;

        public int Size => AddressList.GetVarSize();

        public static AddrPayload Create(params NetworkAddressWithTime[] addresses)
        {
            return new AddrPayload
            {
                AddressList = addresses
            };
        }

        void ISerializable.Deserialize(BinaryReader reader)
        {
            this.AddressList = reader.ReadSerializableArray<NetworkAddressWithTime>(200);
        }

        void ISerializable.Serialize(BinaryWriter writer)
        {
            writer.Write(AddressList);
        }
    }
}
