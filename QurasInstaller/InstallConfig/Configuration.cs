using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QurasInstaller.InstallConfig
{
    class Configuration
    {
        public string InstallPath = "";
        public string ChainPath = "";
        public string CryptoPath = "";
        public static Configuration Default = new Configuration();
    }
}
