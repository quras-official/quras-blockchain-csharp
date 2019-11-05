using System;
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
    /// Interaction logic for History.xaml
    /// </summary>
    public partial class HistoryPage : UserControl
    {
        private LANG iLang => Constant.GetLang();

        public HistoryPage()
        {
            InitializeComponent();
            RefreshLanguage();
        }

        public void RefreshLanguage()
        {
            TxbHistoryTitle.Text = StringTable.GetInstance().GetString("STR_HP_TITLE", iLang);

            foreach (HistoryItem item in stackHistoryPan.Children)
            {
                item.RefreshLanguage();
            }
        }

        public void Reset()
        {
            stackHistoryPan.Children.Clear();
        }

        public void Refresh(List<HistoryItem> items)
        {
            stackHistoryPan.Children.Clear();
            foreach(HistoryItem item in items)
            {
                stackHistoryPan.Children.Insert(0, item);
            }
        }

        public void Add(HistoryItem item)
        {
            //stackHistoryPan.Children.Add(item);
            stackHistoryPan.Children.Insert(0, item);
        }
    }
}
