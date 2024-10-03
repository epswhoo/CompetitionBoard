using CompetitionBoard_Net8.Interfaces;
using System.Windows.Input;

namespace CompetitionBoard_Net8.WpfApp.Helper
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
