
using CompetitionBoardWpfApp.Helper;
using Interfaces;
using Models.Messages;
using System.Windows;
using ViewModels.ToViews;
using ViewModels.UI.Helper;

namespace CompetitionBoardWpfApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            IEventAggregator eventAggregator = new EventAggregator();
            IRelayCommandCreator relayCommandCreator = new RelayCommandCreator();
            DataContext = new CompetitionBoardViewModel(eventAggregator,
                relayCommandCreator);
            eventAggregator.Subscribe<ErrorMsg>(HandleError);
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void HandleError(ErrorMsg errorMsg)
        {
            string msg = $"{errorMsg.ErrorCode}";
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
