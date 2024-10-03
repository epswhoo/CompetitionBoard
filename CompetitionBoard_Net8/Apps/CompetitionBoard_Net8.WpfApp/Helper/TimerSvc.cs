
using CompetitionBoard_Net8.Models.Messages;
using CompetitionBoard_Net8.Models.Results;
using CompetitionBoard_Net8.WpfApp.Helper.Messages;
using System.Timers;

namespace CompetitionBoard_Net8.WpfApp.Helper
{
    public class TimerSvc
    {
        private readonly System.Timers.Timer _readOnlyTimer;
        private readonly System.Timers.Timer _editTimer;

        private readonly Interfaces.IEventAggregator? _eventAggregator;

        public TimerSvc(Interfaces.IEventAggregator? eventAggregator)
        {
            _eventAggregator = eventAggregator;
            _readOnlyTimer = new System.Timers.Timer { Interval = 3000 };
            _editTimer = new System.Timers.Timer { Interval = 10000 };
            _readOnlyTimer.Elapsed += new ElapsedEventHandler((source, e) => OnTimedEvent());
            _editTimer.Elapsed += new ElapsedEventHandler((source, e) => OnTimedEvent());
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
            _eventAggregator.Publish(new ResetErrorMsg());
            _eventAggregator?.Publish(new ReloadRnHsMsg());
            _eventAggregator?.Publish(new ReloadTitleMsg());
        }
    }
}
