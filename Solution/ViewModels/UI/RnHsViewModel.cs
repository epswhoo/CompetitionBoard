
using Interfaces;
using Models.Common;
using Models.Messages;
using Models.Results;
using System.Windows.Input;
using ViewModels.Repos;
using ViewModels.Svcs;
using ViewModels.UI.Helper;

namespace ViewModels.UI
{
    public class RnHsViewModel : UIBase
    {
        private IRnHsRepo _rnHsRepo;

        private string _newListStr;
        public string NewListStr
        {
            get => _newListStr;
            set 
            { 
                _newListStr = value;
                NotifyPropertyChanged();
            }
        }

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

        private IEnumerable<RnHViewModel> _rnhs1;
        public IEnumerable<RnHViewModel> RnHs1
        {
            get => _rnhs1;
            set
            {
                _rnhs1 = value;
                NotifyPropertyChanged();
            }
        }

        private IEnumerable<RnHViewModel> _rnhs2;
        public IEnumerable<RnHViewModel> RnHs2
        {
            get => _rnhs2;
            set
            {
                _rnhs2 = value;
                NotifyPropertyChanged();
            }
        }

        private IEnumerable<RnHViewModel> _rnhs3;
        public IEnumerable<RnHViewModel> RnHs3
        {
            get => _rnhs3;
            set
            {
                _rnhs3 = value;
                NotifyPropertyChanged();
            }
        }

        private IEnumerable<RnHViewModel> _rnhs4;
        public IEnumerable<RnHViewModel> RnHs4
        {
            get => _rnhs4;
            set
            {
                _rnhs4 = value;
                NotifyPropertyChanged();
            }
        }

        private IEnumerable<RnHViewModel> _rnhs5;
        public IEnumerable<RnHViewModel> RnHs5
        {
            get => _rnhs5;
            set
            {
                _rnhs5 = value;
                NotifyPropertyChanged();
            }
        }

        public RnHsViewModel(IEventAggregator eventAggregator, 
            IRelayCommandCreator relayCommandCreator,
            IDBSvc dBSvc)
            : base(eventAggregator, relayCommandCreator)
        {
            _rnHsRepo = new RnHsRepo(dBSvc);
            AddNewListCommand = _relayCommandCreator.CreateCommand(obj => AddNewList());
            Load();
            _eventAggregator.Subscribe<IsEditModusMsg>(OnEditModeChanged);
            eventAggregator.Subscribe<ReloadRnHsMsg>(msg => ReLoad());
        }

        private void ReLoad()
        {
            Load();
            //_eventAggregator.Publish(new IsEditModusMsg { IsEditModus = IsEditModus });
        }

        private void UpdateNewListStr()
        {
            List<RnHViewModel> rnhvms = GetAllRnhVm();
            string newList = string.Empty;
            foreach (RnHViewModel rnhvm in rnhvms)
            {
                newList += $"{rnhvm.HorseNo}, "; 
            }
            int length = newList.Length;
            if (length > 2)
            {
                newList = newList.Substring(0, length - 2);
            }
            NewListStr = newList;
        }

        private List<RnHViewModel> GetAllRnhVm()
        {
            List<RnHViewModel> rnhvms = new List<RnHViewModel>();
            rnhvms.AddRange(RnHs1);
            rnhvms.AddRange(RnHs2);
            rnhvms.AddRange(RnHs3);
            rnhvms.AddRange(RnHs4);
            rnhvms.AddRange(RnHs5);
            return rnhvms;
        }

        private void OnEditModeChanged(IsEditModusMsg isEditModusMsg)
        {
            //if (!isEditModusMsg.IsEditModus)
            //{
            //    Load();
            //}
        }

        private void AddNewList()
        {
            Result<IEnumerable<RnH>> result = _rnHsRepo.SetNewRnHs(NewListStr);
            _ = CheckAndHandleResult(result);
            Load();
        }

        private void Load()
        {
            Result<IEnumerable<RnH>> result = _rnHsRepo.ReadAll();
            if (CheckAndHandleResult(result))
            {
                SetRnHsLists(result.Content);
            }
            UpdateNewListStr();
            _eventAggregator.Publish(new IsEditModusMsg { IsEditModus = IsEditModus });
        }

        private void SetRnHsLists(IEnumerable<RnH> total)
        {
            List<RnH> totalOrdered = total.OrderBy(r => r.Order).ToList();
            int totalCount = total.Count();
            int usedColumnCount;
            if (totalCount > 20)
            {
                usedColumnCount = 5;
            }
            else
            {
                usedColumnCount = 4;
            }
            int rowCount = GausNo.Get(totalCount, usedColumnCount);
            for (int currentColumn = 1; currentColumn < 6; currentColumn++)
            {
                List<RnHViewModel> rnhs = new List<RnHViewModel>();
                int start = (currentColumn - 1) * rowCount;
                for (int currentRow = 0; currentRow < rowCount; currentRow++)
                {
                    if (start + currentRow < totalCount)
                    {
                        RnH rnh = totalOrdered[start + currentRow];
                        RnHViewModel rnhVm = new RnHViewModel(rnh, _rnHsRepo,
                            _eventAggregator, _relayCommandCreator);
                        rnhs.Add(rnhVm);
                    }   
                }
                if (currentColumn == 1)
                {
                    RnHs1 = rnhs;
                }
                else if (currentColumn == 2)
                {
                    RnHs2 = rnhs;
                }
                else if (currentColumn == 3)
                {
                    RnHs3 = rnhs;
                }
                else if (currentColumn == 4)
                {
                    RnHs4 = rnhs;
                }
                else if (currentColumn == 5)
                {
                    RnHs5 = rnhs;
                }
            }
        }
    }
}
