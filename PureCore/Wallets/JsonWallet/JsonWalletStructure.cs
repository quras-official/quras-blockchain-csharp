using System;
using System.Collections.Generic;
using System.Text;

namespace Pure.Wallets.JsonWallet
{
    public class JsonWalletStructure
    {
        public string key_type { set; get; }
        public string p1_hash { get; set; } // password hash
        public string p2_hash { get; set; } // IV
        public string p3_hash { get; set; } // MasterKey
        public string p4_hash { get; set; } // PrivateKey Hash
    }
}
