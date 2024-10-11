using CompetitionBoard_Net8.Interfaces;
using CompetitionBoard_Net8.Models.Messages;
using CompetitionBoard_Net8.WpfApp.Helper;
using System.Windows;
using CompetitionBoard_Net8.ViewModels.UI;
using CompetitionBoard_Net8.WpfApp.Helper.Messages;
using CompetitionBoard_Net8.WpfApp.Configs;
using System.Text.Json;
using System.IO;

namespace CompetitionBoard_Net8.WpfApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly ErrorMessageSvc _errorMessageSvc;
        private readonly TimerSvc _timerSvc;

        public MainWindow()
        {
            InitializeComponent();
            string configjson = File.ReadAllText("config.json");
            Config config = JsonSerializer.Deserialize<Config>(configjson);
            _errorMessageSvc = new ErrorMessageSvc();
            Interfaces.IEventAggregator? eventAggregator = new Helper.EventAggregator();
            IRelayCommandCreator relayCommandCreator = new RelayCommandCreator();
            DataContext = new CompetitionBoardViewModel(eventAggregator,
                relayCommandCreator, config.Db);
            eventAggregator.Subscribe<ErrorMsg>(HandleError);
            eventAggregator.Subscribe<ResetErrorMsg>(HandleResetError);
            _timerSvc = new TimerSvc(eventAggregator, config.UI);
        }

        private void HandleError(ErrorMsg errorMsg)
        {
            string msg = $"{errorMsg.ErrorCode}: {_errorMessageSvc.GetMessage(errorMsg.ErrorCode)}";
            if (errorMsg.Exception != null)
            {
                msg += $"\r\n\r\n{errorMsg.Exception.Message}";
                if (errorMsg.Exception.InnerException != null)
                {
                    msg += $"\r\n\r\n{errorMsg.Exception.InnerException}";
                }
            }
            Dispatcher.BeginInvoke(new Action(() => ErrorLabel.Content = msg));
        }

        private void HandleResetError(ResetErrorMsg resetErrorMsg)
        {
            Dispatcher.BeginInvoke(new Action(() => ErrorLabel.Content = string.Empty));
        }
    }
}