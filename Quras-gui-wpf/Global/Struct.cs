using Pure;
using System.Collections.Generic;

namespace Quras_gui_wpf.Global
{
    public struct AddrStruct
    {
        public string Address;
        public string AmountQRS;
        public string AmountQRG;
    }

    public struct AssetInfoStructure
    {
        public UInt256 Asset_ID;
        public string Asset_Name;
        public string Asset_Unit;
    }

    public class AddressAssetsInfo
    {
        public string Address;
        public Dictionary<UInt256, string> Assets;

        public AddressAssetsInfo(string addr)
        {
            InitInstance();
            Address = addr;
        }

        public AddressAssetsInfo(string addr, UInt256 asset_id, string amount)
        {
            InitInstance();

            Address = addr;
            Assets[asset_id] = amount;
        }

        private void InitInstance()
        {
            Assets = new Dictionary<UInt256, string>();
        }

        public void AddAssets(UInt256 asset_id, string amount)
        {
            Assets[asset_id] = amount;
        }

    }
}
