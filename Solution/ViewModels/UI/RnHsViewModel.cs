
using Interfaces;
using Models.Common;
using Models.Results;
using System.Windows.Input;
using ViewModels.Repos;
using ViewModels.ToViews.Helper;

namespace ViewModels.ToViews
{
    public class RnHsViewModel : UIBase
    {
        private IRnHsRepo _rnHsRepo;

        private ICommand _addNewListCommand;
        public ICommand AddNewListCommand
        {
            get => _addNewListCommand;
            set
            {
                _addNewListCommand = value;
                NotifyPropertyChanged();
            }
        }

        private IEnumerable<RnH> _rnhs;
        public IEnumerable<RnH> RnHs
        {
            get => _rnhs;
            set
            {
                _rnhs = value;
                NotifyPropertyChanged();
            }
        }

        public RnHsViewModel(IEventAggregator eventAggregator, 
            IRelayCommandCreator relayCommandCreator,
            IDBSvc dBSvc)
            : base(eventAggregator, relayCommandCreator)
        {
            _rnHsRepo = new RnHsRepo(dBSvc);
            AddNewListCommand = _relayCommandCreator.CreateCommand(obj => AddNewList(obj));
            Load();
        }

        private void AddNewList(object obj)
        {
            string str = obj as string;
            Result<IEnumerable<RnH>> result = _rnHsRepo.SetNewRnHs(str);
            if (!CheckAndHandleResult(result)) 
                Load();
            else
                RnHs = result.Content;
        }

        private void Load()
        {
            Result<IEnumerable<RnH>> result = _rnHsRepo.ReadAll();
            if (CheckAndHandleResult(result))
            {
                RnHs = result.Content;
            }
        }
    }
}
