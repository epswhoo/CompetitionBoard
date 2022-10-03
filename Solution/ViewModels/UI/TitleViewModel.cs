﻿
using Interfaces;
using Models.Results;
using System.Windows.Input;
using ViewModels.Repos;
using ViewModels.ToViews.Helper;

namespace ViewModels.ToViews
{
    public class TitleViewModel : UIBase
    {
        private ITitleRepo _titleRepo;

        private string _title;
        public string Title
        {
            get => _title;
            set
            {
                _title = value;
                NotifyPropertyChanged();
            }
        }

        private ICommand _saveCommand;
        public ICommand SaveCommand
        {
            get => _saveCommand;
            set
            {
                _saveCommand = value;
                NotifyPropertyChanged();
            }
        }

        public TitleViewModel(IEventAggregator eventAggregator, 
            IRelayCommandCreator relayCommandCreator,
            IDBSvc dBSvc)
            : base(eventAggregator, relayCommandCreator)
        {
            _titleRepo = new TitleRepo(dBSvc);
            SaveCommand = _relayCommandCreator.CreateCommand(obj => Save());
            Load();
        }

        private void Save()
        {
            Result<string> result = _titleRepo.Save(Title);
            if (!CheckAndHandleResult(result))
            {
                Load();
            }
        }

        private void Load()
        {
            Result<string> result = _titleRepo.Load();
            if (CheckAndHandleResult(result))
            {
                Title = result.Content;
            }
        }
    }
}