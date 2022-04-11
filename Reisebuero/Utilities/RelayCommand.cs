using System;
using System.Windows.Input;

namespace Reisebuero.Utilities
{
    public class RelayCommand : ICommand
    {
        readonly Action<object> _execute;
        readonly Predicate<object> _canExecute;

        public RelayCommand(Action<object> execute, Predicate<object> canExecute)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        public RelayCommand(Action<object> execute) : this(execute, null){ }

        public bool CanExecute(object? parameters) => _canExecute?.Invoke(parameters) ?? true;

        public event EventHandler? CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public void Execute(object? parameters) => _execute?.Invoke(parameters);
    }
}
