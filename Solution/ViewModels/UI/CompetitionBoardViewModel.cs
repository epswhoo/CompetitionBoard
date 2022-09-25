using DBSvcs;
using Interfaces;
using Models.Messages;
using ViewModels.ToViews.Helper;

namespace ViewModels.ToViews
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
            IRelayCommandCreator relayCommandCreator)
            : base(eventAggregator, relayCommandCreator)
        {
            IDBSvc dbSvc = new DBSvc();
            Connect(dbSvc);
            TitleViewModel = new TitleViewModel(eventAggregator, relayCommandCreator,
                dbSvc);
            RnHsViewModel = new RnHsViewModel(eventAggregator, relayCommandCreator, dbSvc);
        }

        private void Connect(IDBSvc dBSvc)
        {
            Models.IDBSvc.DBConnectionSettings dbConnectionSettings =
                new Models.IDBSvc.DBConnectionSettings
            {
                Server = "DESKTOP-34KSSOT\\SQLEXPRESS",
                DB = "CompetitionBoardDB",
                Username = "CompetitionBoard",
                Password = "Tafeltafel0#"
            };
            CheckAndHandleResult(dBSvc.SetDBSettings(dbConnectionSettings));
        }
    }
}
