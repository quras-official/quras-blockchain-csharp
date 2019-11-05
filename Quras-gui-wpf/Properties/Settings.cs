using Microsoft.Extensions.Configuration;
using System.Linq;

using Pure;

namespace Quras_gui_wpf.Properties
{
    internal sealed partial class SettingsConfig
    {
        public string DataDirectoryPath { get; }
        public string CertCachePath { get; }
        public ushort NodePort { get; }
        public ushort WsPort { get; }
        public BrowserSettings Urls { get; }
        public ContractSettings Contracts { get; }
        public string ApiPrefix { get; set; }

        // Anonymouse config
        public string PkKeyPath { get; }
        public string VkKeyPath { get; }

        // Others config
        public string AddrbookPath { get; set; }

        public static SettingsConfig instance;

        public static SettingsConfig Default
        {
            get
            {
                if (instance == null)
                    instance = new SettingsConfig();

                return instance;
            }
        }

        public SettingsConfig()
        {
            if (Settings.Default.NeedUpgrade)
            {
                Settings.Default.Upgrade();
                Settings.Default.NeedUpgrade = false;
                Settings.Default.Save();
            }
            IConfigurationSection section = new ConfigurationBuilder().AddJsonFile("config.json").Build().GetSection("ApplicationConfiguration");
            this.DataDirectoryPath = section.GetSection("DataDirectoryPath").Value;
            this.CertCachePath = section.GetSection("CertCachePath").Value;
            this.NodePort = ushort.Parse(section.GetSection("NodePort").Value);
            this.WsPort = ushort.Parse(section.GetSection("WsPort").Value);
            this.Urls = new BrowserSettings(section.GetSection("Urls"));
            this.Contracts = new ContractSettings(section.GetSection("Contracts"));
            this.ApiPrefix = section.GetSection("ApiPrefix").Value;

            IConfigurationSection AnonymousSection = new ConfigurationBuilder().AddJsonFile("config.json").Build().GetSection("AnonymousModule");

            this.PkKeyPath = AnonymousSection.GetSection("PkPath").Value;
            this.VkKeyPath = AnonymousSection.GetSection("VkPath").Value;

            IConfigurationSection OthersSection = new ConfigurationBuilder().AddJsonFile("config.json").Build().GetSection("Others");
            this.AddrbookPath = OthersSection.GetSection("AddrBookPath").Value;
        }
    }

    internal class BrowserSettings
    {
        public string AddressUrl { get; }
        public string AssetUrl { get; }
        public string TransactionUrl { get; }

        public BrowserSettings(IConfigurationSection section)
        {
            this.AddressUrl = section.GetSection("AddressUrl").Value;
            this.AssetUrl = section.GetSection("AssetUrl").Value;
            this.TransactionUrl = section.GetSection("TransactionUrl").Value;
        }
    }

    internal class ContractSettings
    {
        public UInt160[] NEP5 { get; }

        public ContractSettings(IConfigurationSection section)
        {
            this.NEP5 = section.GetSection("NEP5").GetChildren().Select(p => UInt160.Parse(p.Value)).ToArray();
        }
    }
}
