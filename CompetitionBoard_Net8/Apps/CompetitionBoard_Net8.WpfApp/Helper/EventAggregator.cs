
namespace CompetitionBoard_Net8.WpfApp.Helper
{
    public class EventAggregator : Interfaces.IEventAggregator
    {
        private IEventAggregator _eventAggregator;

        public EventAggregator()
        {
            _eventAggregator = new Prism.Events.EventAggregator();
        }

        public void Publish<TData>(TData data)
        {
            _eventAggregator.GetEvent<PubSubEvent<TData>>()
                .Publish(data);
        }

        public void Subscribe<TData>(Action<TData> todo)
        {
            _eventAggregator.GetEvent<PubSubEvent<TData>>()
                .Subscribe(todo);
        }
    }
}
