using System.Windows.Input;

namespace CompetitionBoard_Net8.Interfaces
{
    public interface IRelayCommandCreator
    {
        ICommand CreateCommand(Predicate<object> canExecute, Action<object> execute);

        ICommand CreateCommand(Action<object> execute);
    }
}
