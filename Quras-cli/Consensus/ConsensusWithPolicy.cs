using Quras.Core;
using Quras.Network;
using Quras.Wallets;
using System;
using System.IO;
using System.Linq;

namespace Quras.Consensus
{
    internal class ConsensusWithPolicy : ConsensusService
    {
        private static string log_dictionary;

        private static readonly object LOG_LOCK = new object();

        public ConsensusWithPolicy(LocalNode localNode, Wallet wallet, string log_dictionary)
            : base(localNode, wallet)
        {
            ConsensusWithPolicy.log_dictionary = log_dictionary;
        }

        protected override bool CheckPolicy(Transaction tx)
        {
            switch (Policy.Default.PolicyLevel)
            {
                case PolicyLevel.AllowAll:
                    return true;
                case PolicyLevel.AllowList:
                    return tx.Scripts.All(p => Policy.Default.List.Contains(p.VerificationScript.ToScriptHash())) || tx.Outputs.All(p => Policy.Default.List.Contains(p.ScriptHash));
                case PolicyLevel.DenyList:
                    return tx.Scripts.All(p => !Policy.Default.List.Contains(p.VerificationScript.ToScriptHash())) && tx.Outputs.All(p => !Policy.Default.List.Contains(p.ScriptHash));
                default:
                    return base.CheckPolicy(tx);
            }
        }

        protected override void Log(string message)
        {
            try
            {
                DateTime now = DateTime.Now;
                string line = $"[{now.TimeOfDay:hh\\:mm\\:ss}] {message}";
                Console.WriteLine(line);
                if (string.IsNullOrEmpty(log_dictionary)) return;
                lock (log_dictionary)
                {
                    Directory.CreateDirectory(log_dictionary);
                    string path = Path.Combine(log_dictionary, $"{now:yyyy-MM-dd}.log");
                    lock (LOG_LOCK)
                    {
                        File.AppendAllLines(path, new[] { line });
                    }
                }
            }
            catch
            {

            }
            
        }

        public void RefreshPolicy()
        {
            Policy.Default.Refresh();
        }
    }
}
