
namespace CompetitionBoard_Net8.Interfaces
{
    public interface IEventAggregator
    {
        void Publish<TData>(TData data);
        void Subscribe<TData>(Action<TData> todo);
    }
}