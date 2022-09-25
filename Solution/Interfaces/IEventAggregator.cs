
namespace Interfaces
{
    public interface IEventAggregator
    {
        void Publish<TData>(TData data);
        void Subscribe<TData>(Action<TData> todo);
    }
}