
using Interfaces;
using Models.Messages;
using Models.Results;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ViewModels.ToViews.Helper
{
    public abstract class UIBase : INotifyPropertyChanged
    {
        protected readonly IEventAggregator _eventAggregator;
        protected readonly IRelayCommandCreator _relayCommandCreator;

        private bool _isEditModus;
        public bool IsEditModus
        {
            get => _isEditModus;
            set 
            {
                _isEditModus = value;
                NotifyPropertyChanged();
            }
        }

        public UIBase(IEventAggregator eventAggregator,
            IRelayCommandCreator relayCommandCreator)
        {
            _eventAggregator = eventAggregator;
            _eventAggregator.Subscribe<IsEditModusMsg>(isEditModusMsg => 
                IsEditModus = isEditModusMsg.IsEditModus);
            _relayCommandCreator = relayCommandCreator;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool CheckAndHandleResult<T>(Result<T> result)
        {
            if (result.ErrorCode != 0)
            {
                _eventAggregator.Publish(new ErrorMsg
                {
                    ErrorCode = result.ErrorCode,
                    Exception = result.Exception
                });
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
