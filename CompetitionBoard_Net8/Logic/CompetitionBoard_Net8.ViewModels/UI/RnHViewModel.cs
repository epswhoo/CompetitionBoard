using CompetitionBoard_Net8.Interfaces;
using CompetitionBoard_Net8.Models.Common;
using CompetitionBoard_Net8.Models.Errors;
using CompetitionBoard_Net8.Models.Messages;
using CompetitionBoard_Net8.Models.Results;
using System.Windows.Input;
using CompetitionBoard_Net8.ViewModels.UI.Helper;

namespace CompetitionBoard_Net8.ViewModels.UI
{
    public class RnHViewModel : UIBase
    {
        private readonly IRnHsRepo _rnHsRepo;

        private readonly RnH _rnh;

        private ICommand _saveMarkCommand;
        public ICommand SaveMarkCommand
        {
            get => _saveMarkCommand;
            set
            {
                _saveMarkCommand = value;
                NotifyPropertyChanged();
            }
        }

        private ICommand _saveHorseNoCommand;
        public ICommand SaveHorseNoCommand
        {
            get => _saveHorseNoCommand;
            set
            {
                _saveHorseNoCommand = value;
                NotifyPropertyChanged();
            }
        }

        private ICommand _addFollowingCommand;
        public ICommand AddFollowingCommand
        {
            get => _addFollowingCommand;
            set
            {
                _addFollowingCommand = value;
                NotifyPropertyChanged();
            }
        }

        private ICommand _addPreviousCommand;
        public ICommand AddPreviousCommand
        {
            get => _addPreviousCommand;
            set
            {
                _addPreviousCommand = value;
                NotifyPropertyChanged();
            }
        }

        private ICommand _deleteCommand;
        public ICommand DeleteCommand
        {
            get => _deleteCommand;
            set
            {
                _deleteCommand = value;
                NotifyPropertyChanged();
            }
        }

        public int HorseNo
        {
            get => _rnh.HorseNo;
            set
            {
                _rnh.HorseNo = value;
                NotifyPropertyChanged();
            }
        }

        public RnHStatus Status
        {
            get => _rnh.Status;
            set
            {
                _rnh.Status = value;
                NotifyPropertyChanged();
                SaveStatus();
            }
        }

        public double Mark
        {
            get => _rnh.Mark;
            set
            {
                _rnh.Mark = value;
                NotifyPropertyChanged();
            }
        }

        private string _markStr;
        public string MarkStr
        {
            get => _markStr;
            set
            {
                _markStr = value;
                NotifyPropertyChanged();
            }
        }

        public bool IsDisqualificated
        {
            get => _rnh.IsDisqualificated;
            set
            {
                _rnh.IsDisqualificated = value;
                NotifyPropertyChanged();
                SaveIsDisqualificated();
            }
        }

        public bool IsRanked
        {
            get => _rnh.IsRanked;
            set
            {
                bool old = _rnh.IsRanked;
                _rnh.IsRanked = value;
                NotifyPropertyChanged();
                SaveIsRanked();
            }
        }

        public RnHViewModel(RnH rnh, IRnHsRepo rnhsRepo,
            IEventAggregator ea, IRelayCommandCreator rcc) : base(ea, rcc)
        {
            _rnHsRepo = rnhsRepo;
            _rnh = rnh;
            SaveMarkCommand = _relayCommandCreator.CreateCommand(obj => SaveMark());
            SaveHorseNoCommand = _relayCommandCreator.CreateCommand(obj => SaveHorseNo());
            AddPreviousCommand = _relayCommandCreator.CreateCommand(obj => Add(true));
            AddFollowingCommand = _relayCommandCreator.CreateCommand(obj => Add(false));
            DeleteCommand = _relayCommandCreator.CreateCommand(obj => Delete());
            _eventAggregator.Subscribe<IsEditModusMsg>(OnEditModeChanged);
        }

        private void OnEditModeChanged(IsEditModusMsg msg)
        {
            SetMarkStr();
        }

        private void SetMarkStr()
        {
            MarkStr = Mark.ToString();
        }

        private void Add(bool isprevious)
        {
            int position = isprevious ? _rnh.Order : _rnh.Order + 1;
            Result<RnH> result = _rnHsRepo.InsertNewWithOrder(position);
            _ = CheckAndHandleResult(result);
            _eventAggregator.Publish(new ReloadRnHsMsg());
        }

        private void Delete()
        {
            Result<bool> result = _rnHsRepo.Delete(_rnh);
            _ = CheckAndHandleResult(result);
            _eventAggregator.Publish(new ReloadRnHsMsg());
        }

        private void SaveIsRanked()
        {
            Result<RnH> result = _rnHsRepo.SaveIsRanked(_rnh);
            _ = CheckAndHandleResult(result);
            _eventAggregator.Publish(new ReloadRnHsMsg());
        }

        private void SaveIsDisqualificated()
        {
            Result<RnH> result = _rnHsRepo.SaveIsDisqualificated(_rnh);
            _ = CheckAndHandleResult(result);
            _eventAggregator.Publish(new ReloadRnHsMsg());
        }

        private void SaveMark()
        {
            if (!double.TryParse(MarkStr, out double mark))
            {
                Result<RnH> markResult = new Result<RnH>
                {
                    Content = _rnh,
                    ErrorCode = int.Parse(ErrorCodes.RnHViewModelMarkIsNoDouble)
                };
                _ = !CheckAndHandleResult(markResult);
                return;
            }
            _rnh.Mark = mark;
            Result<RnH> result = _rnHsRepo.SaveMark(_rnh);
            _ = CheckAndHandleResult(result);
            _eventAggregator.Publish(new ReloadRnHsMsg());
        }

        private void SaveHorseNo()
        {
            Result<RnH> result = _rnHsRepo.SaveHorseNo(_rnh);
            _ = CheckAndHandleResult(result);
            _eventAggregator.Publish(new ReloadRnHsMsg());
        }

        private void SaveStatus()
        {
            Result<RnH> result = _rnHsRepo.SaveStatus(_rnh);
            _ = CheckAndHandleResult(result);
            _eventAggregator.Publish(new ReloadRnHsMsg());
        }
    }
}
