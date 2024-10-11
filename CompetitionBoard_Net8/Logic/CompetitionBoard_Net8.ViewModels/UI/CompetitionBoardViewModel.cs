
using CompetitionBoard_Net8.Interfaces;
using CompetitionBoard_Net8.Models.IDBSvc;
using CompetitionBoard_Net8.Models.Messages;
using CompetitionBoard_Net8.ViewModels.UI.Helper;

namespace CompetitionBoard_Net8.ViewModels.UI
{
    public class CompetitionBoardViewModel : UIBase
    {
        private bool _isEditModusActivated;
        public bool IsEditModusActivated
        {
            get => _isEditModusActivated;
            set
            {
                _isEditModusActivated = value;
                NotifyPropertyChanged();
                IsEditModusMsg isEditModusMsg = new IsEditModusMsg { IsEditModus = _isEditModusActivated };
                _eventAggregator.Publish(isEditModusMsg);
            }
        }

        private TitleViewModel _titleViewModel;
        public TitleViewModel TitleViewModel
        {
            get => _titleViewModel;
            set
            {
                _titleViewModel = value;
                NotifyPropertyChanged();
            }
        }

        private RnHsViewModel _rnHsViewModel;
        public RnHsViewModel RnHsViewModel
        {
            get => _rnHsViewModel;
            set
            {
                _rnHsViewModel = value;
                NotifyPropertyChanged();
            }
        }

        public CompetitionBoardViewModel(IEventAggregator eventAggregator,
            IRelayCommandCreator relayCommandCreator,
            DBConnectionSettings dbSettings)
            : base(eventAggregator, relayCommandCreator)
        {
            IDBSvc dbSvc = new DBSvc.DBSvc();
            Connect(dbSvc, dbSettings);
            TitleViewModel = new TitleViewModel(eventAggregator, relayCommandCreator,
                dbSvc);
            RnHsViewModel = new RnHsViewModel(eventAggregator, relayCommandCreator, dbSvc);
        }

        private void Connect(IDBSvc dBSvc, DBConnectionSettings dbSettings)
        {
            CheckAndHandleResult(dBSvc.SetDBSettings(dbSettings));
        }
    }
}
