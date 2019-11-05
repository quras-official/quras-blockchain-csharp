using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quras_Test.Structure
{
    public enum AddrType
    {
        Transparent,
        Anonymous
    }
    public class TAddressType
    {
        public string privKey;
        public string address;
        public AddrType type;

        public TAddressType(string privKey, string address, AddrType type)
        {
            this.privKey = privKey;
            this.address = address;
            this.type = type;
        }
    }
}
