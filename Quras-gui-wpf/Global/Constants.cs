using System;
using System.Xml.Linq;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Pure;
using Pure.Core;
using Pure.Network;
using Pure.Implementations.Wallets.EntityFramework;

using Quras_gui_wpf.Utils;
using Quras_gui_wpf.Properties;
using Quras_gui_wpf.Dialogs.NotifyMessage;

namespace Quras_gui_wpf.Global
{
    public static class Constant
    {
        public static string PEER_STATE_PATH = "peers.dat";
        public static LocalNode LocalNode;
        public static UserWallet CurrentWallet;
        public static bool bSnarksParamLoaded = false;
        public static NotifyMessageManager NotifyMessageMgr;
        public static string UpdateUrl = "http://13.230.62.42/quras/update/update.xml";
        public static List<TaskMessage> TaskMessages = new List<TaskMessage>();
        public static bool isLoadedVK = false;
        public static bool isLoadedPK = false;

        public static LANG GetLang()
        {
            LANG iLang = LANG.EN;
            
            if (Settings.Default.Language == "EN")
            {
                iLang = LANG.EN;
            }
            else if (Settings.Default.Language == "JP")
            {
                iLang = LANG.JP;
            }
            
            return iLang;
        }

        public static Version GetNewestWalletVersionFromServer()
        {
            Version minimum = new Version("1.0.0.1");
            XDocument xdoc = null;
            try
            {
                xdoc = XDocument.Load(UpdateUrl);
            }
            catch { }
            if (xdoc != null)
            {
                minimum = Version.Parse(xdoc.Element("update").Attribute("latest").Value);
            }
            return minimum;
        }

        public static Version GetLocalWalletVersion()
        {
            Version version = Assembly.GetExecutingAssembly().GetName().Version;

            return version;
        }
    }

    public class AddressAssetsImp
    {
        private Dictionary<string, AddressAssetsInfo> dicAddrAssets;
        private static AddressAssetsImp instance;

        public static AddressAssetsImp getInstance()
        {
            if (instance == null)
            {
                instance = new AddressAssetsImp();
            }

            return instance;
        }
        public AddressAssetsImp()
        {
            dicAddrAssets = new Dictionary<string, AddressAssetsInfo>();
        }

        public Dictionary<string, AddressAssetsInfo> GetList()
        {
            return dicAddrAssets;
        }

        public void Reset()
        {
            dicAddrAssets.Clear();
        }

        public void RefrshAssets(string address, UInt256 asset_id, string amount)
        {
            if (dicAddrAssets.ContainsKey(address))
            {
                AddressAssetsInfo item = dicAddrAssets[address];
                item.AddAssets(asset_id, amount);

                dicAddrAssets[address] = item;
            }
            else
            {
                AddressAssetsInfo item = new AddressAssetsInfo(address, asset_id, amount);
                dicAddrAssets[address] = item;
            }
        }
    }
}
