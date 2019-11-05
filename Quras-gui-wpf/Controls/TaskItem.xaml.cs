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

using Quras_gui_wpf.Global;
using Quras_gui_wpf.Utils;

namespace Quras_gui_wpf.Controls
{
    /// <summary>
    /// Interaction logic for TaskItem.xaml
    /// </summary>
    public partial class TaskItem : UserControl
    {
        TaskMessage task;
        private LANG iLang => Constant.GetLang();

        public TaskItem()
        {
            InitializeComponent();
        }

        public TaskItem(TaskMessage task)
        {
            InitializeComponent();
            this.task = task;

            RefreshInterface();
        }

        public void RefreshInterface()
        {
            switch(task.type)
            {
                case TaskType.LoadKey:
                    {
                        LoadKeyTaskMessage loadTask = task as LoadKeyTaskMessage;
                        tbTime.Text = loadTask.time.ToString();
                        tbMessage.Text = loadTask.message;

                        if (loadTask.color == TaskColor.Red)
                        {
                            bdTask.BorderBrush = new SolidColorBrush(Colors.Red);
                            tbTime.Foreground = new SolidColorBrush(Colors.Red);
                        }
                        else
                        {
                            bdTask.BorderBrush = new SolidColorBrush(Colors.YellowGreen);
                            tbTime.Foreground = new SolidColorBrush(Colors.YellowGreen);
                        }
                        break;
                    }
                case TaskType.TxSend:
                    {
                        SendTaskMessage sendTask = task as SendTaskMessage;
                        tbTime.Text = sendTask.time.ToString();

                        tbMessage.Text = String.Format(StringTable.GetInstance().GetString("STR_TASK_MESSAGE_SEND_TX_MESSAGE", iLang), sendTask.message, sendTask.from, sendTask.to, sendTask.amount, sendTask.assetID);

                        if (sendTask.color == TaskColor.Red)
                        {
                            bdTask.BorderBrush = new SolidColorBrush(Colors.Red);
                            tbTime.Foreground = new SolidColorBrush(Colors.Red);
                        }
                        else
                        {
                            bdTask.BorderBrush = new SolidColorBrush(Colors.YellowGreen);
                            tbTime.Foreground = new SolidColorBrush(Colors.YellowGreen);
                        }
                        break;
                    }
            }
        }
    }
}
