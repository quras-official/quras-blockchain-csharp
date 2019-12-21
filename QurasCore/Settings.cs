using Quras.Core;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Quras
{
    internal class Settings
    {
        public uint Magic { get; private set; }
        public byte AddressVersion { get; private set; }
        public byte StealthAddressVersion { get; private set; }
        public byte StealthAddressRingSize { get; private set; }
        public byte AnonymousAddressVersion { get; private set; }
        public string[] StandbyValidators { get; private set; }
        public string[] SeedList { get; private set; }
        public ushort NodePort { get; private set; }
        public ushort WsPort { get; private set; }
        public string[] UriPrefix { get; private set; }
        public string APIPrefix { get; private set; }
        public string NetType { get; private set; }
        public IReadOnlyDictionary<TransactionType, Fixed8> SystemFee { get; private set; }

        public static Settings Default { get; private set; }

        static Settings()
        {
            IConfigurationSection section = new ConfigurationBuilder().AddJsonFile("protocol.json").Build().GetSection("ProtocolConfiguration");
            IConfigurationSection sectionConfig = new ConfigurationBuilder().AddJsonFile("config.json").Build().GetSection("ApplicationConfiguration");
            Default = new Settings
            {
                Magic = uint.Parse(section.GetSection("Magic").Value),
                AddressVersion = byte.Parse(section.GetSection("AddressVersion").Value),
                AnonymousAddressVersion = byte.Parse(section.GetSection("AnonymousAddressVersion").Value),
                StealthAddressVersion = byte.Parse(section.GetSection("StealthAddressVersion").Value),
                StealthAddressRingSize = byte.Parse(section.GetSection("RingSize").Value),
                StandbyValidators = section.GetSection("StandbyValidators").GetChildren().Select(p => p.Value).ToArray(),
                SeedList = section.GetSection("SeedList").GetChildren().Select(p => p.Value).ToArray(),
                SystemFee = section.GetSection("SystemFee").GetChildren().ToDictionary(p => (TransactionType)Enum.Parse(typeof(TransactionType), p.Key, true), p => Fixed8.Parse(p.Value)),

                NodePort = ushort.Parse(sectionConfig.GetSection("NodePort").Value),
                WsPort = ushort.Parse(sectionConfig.GetSection("WsPort").Value),
                UriPrefix = sectionConfig.GetSection("UriPrefix").GetChildren().Select(p => p.Value).ToArray(),
                APIPrefix = sectionConfig.GetSection("ApiPrefix").Value,
                NetType = section.GetSection("NetType").Value
            };
        }
    }
}
