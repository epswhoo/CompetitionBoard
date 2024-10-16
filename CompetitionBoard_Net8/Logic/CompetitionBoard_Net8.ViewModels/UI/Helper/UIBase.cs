﻿
using CompetitionBoard_Net8.Interfaces;
using CompetitionBoard_Net8.Models.Messages;
using CompetitionBoard_Net8.Models.Results;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace CompetitionBoard_Net8.ViewModels.UI.Helper
{
    public abstract class UIBase : INotifyPropertyChanged
    {
        protected readonly IEventAggregator _eventAggregator;
        protected readonly IRelayCommandCreator _relayCommandCreator;

        private bool _isEditListModus;
        public bool IsEditListModus
        {
            get => _isEditListModus;
            set 
            {
                _isEditListModus = value;
                NotifyPropertyChanged();
            }
        }

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
            _eventAggregator.Subscribe<IsEditListModusMsg>(isEditListModusMsg =>
                IsEditListModus = isEditListModusMsg.IsEditListModus);
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
