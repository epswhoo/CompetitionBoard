
using Interfaces;
using Models.Messages;
using System.Timers;

namespace CompetitionBoardWpfApp.Helper
{
    public class TimerSvc
    {
        private readonly Timer _readOnlyTimer;
        private readonly Timer _editTimer;

        private readonly IEventAggregator? _eventAggregator;

        public TimerSvc(IEventAggregator? eventAggregator)
        {
            _eventAggregator = eventAggregator;
            _readOnlyTimer = new Timer { Interval = 3000 };
            _editTimer = new Timer { Interval = 10000 };
            //_readOnlyTimer.Elapsed += new ElapsedEventHandler((source, e) => OnTimedEvent());
            //_editTimer.Elapsed += new ElapsedEventHandler((source, e) => OnTimedEvent());
            _readOnlyTimer.Enabled = true;
            _eventAggregator?.Subscribe<IsEditModusMsg>(OnEditModeChanged);
        }

        private void OnEditModeChanged(IsEditModusMsg msg)
        {
            _readOnlyTimer.Enabled = !msg.IsEditModus;
            _editTimer.Enabled = msg.IsEditModus;
        }

        private void OnTimedEvent()
        {
            _eventAggregator?.Publish(new ReloadRnHsMsg());
            _eventAggregator?.Publish(new ReloadTitleMsg());
        }
    }
}
