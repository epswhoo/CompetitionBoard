using System.Windows.Input;

namespace Interfaces
{
    public interface IRelayCommandCreator
    {
        ICommand CreateCommand(Predicate<object> canExecute, Action<object> execute);

        ICommand CreateCommand(Action<object> execute);
    }
}
