using Interfaces;
using System;
using System.Windows.Input;

namespace ViewModels.UI.Helper
{
    public class RelayCommandCreator : IRelayCommandCreator
    {
        public ICommand CreateCommand(Predicate<object> canExecute, Action<object> execute)
        {
            return new RelayCommand(canExecute, execute);
        }

        public ICommand CreateCommand(Action<object> execute)
        {
            return new RelayCommand(obj => true, execute);
        }
    }
}
