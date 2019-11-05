using System;
using System.Collections.Generic;
using System.Text;

namespace Pure.Core
{
    public class HttpAssetInfo
    {
        public int total { get; set; }
        public List<AssetDetail> assets { get; set; }
    }

    public class AssetDetail
    {
        public int address_count { get; set; }
        public string amount { get; set; }
        public long block_time { get; set; }
        public string hash { get; set; }
        public long issued { get; set; }
        public string name { get; set; }
        public string owner { get; set; }
        public string precision { get; set; }
        public string symbol { get; set; }
        public int transaction_count { get; set; }
        public string type { get; set; }
    }
}
