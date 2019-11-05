using System;
using System.Windows.Input;

namespace Quras_gui_wpf.Dialogs.NotifyMessage
{
    public class DelegateCommand : ICommand
    {
        private readonly Predicate<object> canExecute;
        private readonly Action<object> execute;

        public event EventHandler CanExecuteChanged;

        /// <summary>
		/// Constructs an instance of <c>DelegateCommand</c>.
		/// </summary>
		/// <remarks>
		/// This constructor creates the command without a delegate for determining whether the command can execute. Therefore, the
		/// command will always be eligible for execution.
		/// </remarks>
		/// <param name="execute">
		/// The delegate to invoke when the command is executed.
		/// </param>
		public DelegateCommand(Action<object> execute)
            : this(execute, null)
        {
        }

        /// <summary>
		/// Constructs an instance of <c>DelegateCommand</c>.
		/// </summary>
		/// <param name="execute">
		/// The delegate to invoke when the command is executed.
		/// </param>
		/// <param name="canExecute">
		/// The delegate to invoke to determine whether the command can execute.
		/// </param>
		public DelegateCommand(Action<object> execute, Predicate<object> canExecute)
        {
            this.execute = execute;
            this.canExecute = canExecute;
        }

        public bool CanExecute(object parameter)
        {
            if (canExecute == null)
            {
                return true;
            }

            return canExecute(parameter);
        }

        public void Execute(object parameter)
        {
            execute(parameter);
        }
    }
}
