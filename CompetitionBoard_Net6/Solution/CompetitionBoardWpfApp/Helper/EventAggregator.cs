using Interfaces;
using Prism.Events;
using System;

namespace CompetitionBoardWpfApp.Helper
{
    public class EventAggregator : Interfaces.IEventAggregator
    {
        private Prism.Events.IEventAggregator _eventAggregator;

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
