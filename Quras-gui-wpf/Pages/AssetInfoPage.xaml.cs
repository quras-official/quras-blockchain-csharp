﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using Quras_gui_wpf.Controls;
using Quras_gui_wpf.Global;
using Quras_gui_wpf.Utils;

namespace Quras_gui_wpf.Pages
{
    /// <summary>
    /// Interaction logic for AssetInfoPage.xaml
    /// </summary>
    public partial class AssetInfoPage : UserControl
    {

        private LANG iLang => Constant.GetLang();

        public AssetInfoPage()
        {
            InitializeComponent();
        }

        public void Refresh(List<AssetItem> items)
        {
            RefreshLanguage();

            spAssetInfoPan.Children.Clear();
            foreach (AssetItem item in items)
            {
                spAssetInfoPan.Children.Add(item);
            }
        }

        public void Reset()
        {
            spAssetInfoPan.Children.Clear();
        }

        public void RefreshLanguage()
        {
            TxbAssetInfoTitle.Text = StringTable.GetInstance().GetString("STR_SP_ASSETS_MARK", iLang);
            TxbComment.Text = StringTable.GetInstance().GetString("STR_SP_ASSETS_COMMENT", iLang);
        }
    }
}
