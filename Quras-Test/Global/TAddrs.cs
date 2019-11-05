using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

using Quras_Test.Structure;

using Pure;
using Pure.Core;

namespace Quras_Test.Global
{
    public class TAddrs
    {
        private List<TAddressType> addresses;
        public static TAddrs instance;

        public static TAddrs GetInstance()
        {
            if (instance == null)
            {
                instance = new TAddrs();
            }

            return instance;
        }

        public TAddrs()
        {
            using (StreamReader r = new StreamReader("Jsons/t_addrs.json"))
            {
                string json = r.ReadToEnd();
                try
                {
                    addresses = JsonConvert.DeserializeObject<List<TAddressType>>(json);
                }
                catch (Exception)
                {
                    addresses = new List<TAddressType>();
                }
            }
        }

        public List<TAddressType> GetAddress()
        {
            return addresses;
        }

        public void SaveAddress(byte[] privKey, string address)
        {
            TAddressType item = new TAddressType(privKey.ToHexString(), address, AddrType.Transparent);
            addresses.Add(item);

            string json = JsonConvert.SerializeObject(addresses.ToArray());
            System.IO.File.WriteAllText(@"Jsons/t_addrs.json", json);
        }

        public void RemoveAddress(string address)
        {
            for (int i = 0; i < addresses.Count; i ++)
            {
                if (addresses[i].address == address)
                {
                    addresses.RemoveAt(i);
                    break;
                }
            }

            string json = JsonConvert.SerializeObject(addresses.ToArray());
            System.IO.File.WriteAllText(@"Jsons/t_addrs.json", json);
        }
    }
}
