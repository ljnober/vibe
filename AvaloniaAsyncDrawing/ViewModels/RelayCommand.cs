// AvaloniaAsyncDrawing/ViewModels/RelayCommand.cs
using System;
using System.Windows.Input;

namespace AvaloniaAsyncDrawing.ViewModels
{
    /// <summary>
    /// 通用命令实现，支持 MVVM 命令绑定。
    /// </summary>
    public class RelayCommand : ICommand
    {
        private readonly Action<object?> _execute;
        private readonly Func<object?, bool>? _canExecute;

        public RelayCommand(Action<object?> execute, Func<object?, bool>? canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        public bool CanExecute(object? parameter) => _canExecute == null || _canExecute(parameter);

        public void Execute(object? parameter) => _execute(parameter);

        public event EventHandler? CanExecuteChanged
        {
            add { }
            remove { }
        }

        public void RaiseCanExecuteChanged() { }
    }
}