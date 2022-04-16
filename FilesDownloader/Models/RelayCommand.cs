using System;
using System.Windows.Input;

namespace FilesDownloader.Models
{
	public class RelayCommand : ICommand
	{
		readonly Action<object> _execute;
		readonly Func<object, bool> _canExecute;

		public RelayCommand(Action<object> execute, Func<object, bool> canExecute)
		{
			if (execute == null)
				throw new ArgumentNullException("execute");

			_execute = execute;
			_canExecute = canExecute;
		}

		public bool CanExecute(object parameter)
		{
			bool x = _canExecute == null || _canExecute(parameter);
			return x;
		}

		public event EventHandler CanExecuteChanged
		{
			add { CommandManager.RequerySuggested += value; }
			remove { CommandManager.RequerySuggested -= value; }
		}

		public void Execute(object parameter)
		{
			_execute(parameter);
		}
	}
}
