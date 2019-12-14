using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Quras.Implementations.Wallets.EntityFramework;
using Quras.Shell;
namespace Quras
{
    class Program
    {
        internal static UserWallet Wallet;
        static void Main(string[] args)
        {
            new MainService().Run(args);
        }
    }
}
