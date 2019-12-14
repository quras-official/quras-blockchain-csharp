using System;
using CoreTransaction = Quras.Core.Transaction;

namespace Quras.Implementations.Wallets.EntityFramework
{
    public class TransactionInfo
    {
        public CoreTransaction Transaction;
        public uint? Height;
        public DateTime Time;
    }
}
