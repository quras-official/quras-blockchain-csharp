using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Pure;

namespace Quras_gui_wpf.Global
{
    public enum TaskType
    {
        TxSend,
        LoadKey
    }
    public enum TaskColor
    {
        Red,
        Green
    }
    public class TaskMessage
    {
        public TaskType type;
        public DateTime time;
        public TaskColor color;

        public TaskMessage(TaskType type, DateTime time, TaskColor color)
        {
            this.type = type;
            this.time = time;
            this.color = color;
        }
    }

    public class SendTaskMessage : TaskMessage
    {
        public string from;
        public string to;
        public string amount;
        public string assetID;
        public string message;

        public SendTaskMessage(string from, string to, string amount, string assetID, DateTime time, string message, TaskColor color = TaskColor.Green)
            : base(TaskType.TxSend, time, color)
        {
            this.from = from;
            this.to = to;
            this.amount = amount;
            this.message = message;
            this.assetID = assetID;
        }
    }

    public class LoadKeyTaskMessage : TaskMessage
    {
        public string message;

        public LoadKeyTaskMessage(string message, DateTime time, TaskColor color = TaskColor.Green)
            : base(TaskType.LoadKey, time, color)
        {
            this.message = message;
        }
    }
}
