using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Pure;

namespace Quras_gui_wpf.Global
{
    public class AssetsManager
    {
        private Dictionary<UInt256, string> dicAssets;
        private static AssetsManager instance;
        
        public AssetsManager()
        {
            dicAssets = new Dictionary<UInt256, string>();

            InitInstance();
        }

        public static AssetsManager GetInstance()
        {
            if (instance == null)
            {
                instance = new AssetsManager();
            }

            return instance;
        }

        public void InitInstance()
        {
            // Add QRS Asset
            // UInt256 qrsAsset = Pure.Core.Blockchain.GoverningToken.Hash;
            // dicAssets[qrsAsset] = "XQC";
            // Add QRG Asset
            // UInt256 qrgAsset = Pure.Core.Blockchain.UtilityToken.Hash;
            // dicAssets[qrgAsset] = "XQG";
        }

        public void Reset()
        {
            dicAssets.Clear();
        }

        public bool AddAssets(UInt256 assetID, string name)
        {
            if (!dicAssets.ContainsKey(assetID))
            {
                dicAssets[assetID] = name;
                return true;
            }
            else
            {
                return false;
            }
        }

        public string GetName(UInt256 assetID)
        {
            return dicAssets[assetID];
        }

        public UInt256 GetAssetID(string name)
        {
            if (dicAssets.ContainsValue(name))
            {
                foreach (UInt256 key in dicAssets.Keys)
                {
                    if (dicAssets[key] == name)
                    {
                        return key;
                    }
                }

                return new UInt256();
            }
            else
            {
                return new UInt256();
            }
        }
    }
}
