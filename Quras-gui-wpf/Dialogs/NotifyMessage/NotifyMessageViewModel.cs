using System;
using System.Windows.Input;

namespace Quras_gui_wpf.Dialogs.NotifyMessage
{
    public class NotifyMessageViewModel
    {
        private readonly NotifyMessage  content;
        private readonly AnimatedLocation location;
        private readonly Action closedAction;

        private ICommand clickCommand;
        private ICommand closeCommand;

        public NotifyMessageViewModel(NotifyMessage content, AnimatedLocation location, Action closedAction)
        {
            this.content = content;
            this.location = location;
            this.closedAction = closedAction;
        }

        public NotifyMessage Message
        {
            get { return content; }
        }

        
        public ICommand ClickCommand
        {
            get { return (clickCommand ?? (clickCommand = new DelegateCommand((_) => content.OnClick()))); }
        }

        
        public ICommand CloseCommand
        {
            get { return (closeCommand ?? (closeCommand = new DelegateCommand((_) => closedAction()))); }
        }

        public AnimatedLocation Location
        {
            get { return location; }
        }
    }
}
