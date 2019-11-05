using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Pure;
using Pure.Core;
using Pure.Network;
using Pure.Implementations.Wallets.EntityFramework;

namespace Quras_gui_SP.Global
{
    public enum WalletStatus
    {
        Empty,
        Opened
    }

    public static class Constant
    {
        public static string PEER_STATE_PATH = "peers.dat";
        public static LocalNode LocalNode;
        public static UserWallet CurrentWallet;
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
            for (int i = 0; i < AssetList.Count; i ++)
            {
                if (AssetList[i].Asset_ID == id)
                {
                    AssetList.RemoveAt(i);
                    break;
                }
            }
        }
    }
}
