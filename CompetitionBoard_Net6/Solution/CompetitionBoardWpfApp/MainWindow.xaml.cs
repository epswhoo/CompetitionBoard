
using CompetitionBoardWpfApp.Helper;
using Interfaces;
using Models.Messages;
using System.Windows;
using ViewModels.UI;
using ViewModels.UI.Helper;

namespace CompetitionBoardWpfApp
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
            _errorMessageSvc = new ErrorMessageSvc();
            IEventAggregator? eventAggregator = new EventAggregator();
            IRelayCommandCreator relayCommandCreator = new RelayCommandCreator();
            DataContext = new CompetitionBoardViewModel(eventAggregator,
                relayCommandCreator);
            eventAggregator.Subscribe<ErrorMsg>(HandleError);
            _timerSvc = new TimerSvc(eventAggregator);
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
            MessageBox.Show(msg, "Fehler");
        }
    }
}
