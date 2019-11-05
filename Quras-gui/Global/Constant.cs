using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Pure;
using Pure.Core;
using Pure.Network;
using Pure.Implementations.Wallets.EntityFramework;

using Quras_gui.Properties;
namespace Quras_gui.Global
{
    public static class Constant
    {
        public static string PEER_STATE_PATH = "peers.dat";
        public static LocalNode LocalNode;
        public static UserWallet CurrentWallet;
        public static bool bSnarksParamLoaded = false;

        public static int GetLang()
        {
            int iLang = 0;
            if (Settings.Default.Language == "EN")
            {
                iLang = 0;
            }
            else if (Settings.Default.Language == "JP")
            {
                iLang = 1;
            }

            return iLang;
        }
    }

    public class AssetsImp
    {
        private List<AssetInfoStructure> AssetList;
        private static AssetsImp instance;

        public static AssetsImp getInstance()
        {
            if (instance == null)
            {
                instance = new AssetsImp();
            }

            return instance;
        }
        public AssetsImp()
        {
            AssetList = new List<AssetInfoStructure>();
        }

        public List<AssetInfoStructure> GetList()
        {
            return AssetList;
        }

        public void Reset()
        {
            AssetList.Clear();

            AssetInfoStructure qrsAsset;
            AssetInfoStructure qrgAsset;

            qrsAsset.Asset_ID = Blockchain.GoverningToken.Hash;
            qrsAsset.Asset_Name = Blockchain.GoverningToken.Name;
            qrsAsset.Asset_Unit = "QRS";

            qrgAsset.Asset_ID = Blockchain.UtilityToken.Hash;
            qrgAsset.Asset_Name = Blockchain.UtilityToken.Name;
            qrgAsset.Asset_Unit = "QRG";

            AssetList.Add(qrsAsset);
            AssetList.Add(qrgAsset);
        }

        public void AddAsset(UInt256 id, string name, string unit)
        {
            AssetInfoStructure assetItem;

            assetItem.Asset_ID = id;
            assetItem.Asset_Name = name;
            assetItem.Asset_Unit = unit;

            AssetList.Add(assetItem);
        }

        public void RemoveAsset(UInt256 id)
        {
            for (int i = 0; i < AssetList.Count; i++)
            {
                if (AssetList[i].Asset_ID == id)
                {
                    AssetList.RemoveAt(i);
                    break;
                }
            }
        }
    }

    public class AddressAssetsImp
    {
        private Dictionary<string, AddressAssetsInfo> dicAddrAssets_;
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
            dicAddrAssets_ = new Dictionary<string, AddressAssetsInfo>();
        }

        public Dictionary<string, AddressAssetsInfo> GetList()
        {
            return dicAddrAssets_;
        }

        public void Reset()
        {
            dicAddrAssets_.Clear();
        }

        public void RefrshAssets(string address, UInt256 asset_id, string amount)
        {
            if (dicAddrAssets_.ContainsKey(address))
            {
                AddressAssetsInfo item = dicAddrAssets_[address];
                item.AddAssets(asset_id, amount);

                dicAddrAssets_[address] = item;
            }
            else
            {
                AddressAssetsInfo item = new AddressAssetsInfo(address, asset_id, amount);
                dicAddrAssets_[address] = item;
            }
        }
    }
}
