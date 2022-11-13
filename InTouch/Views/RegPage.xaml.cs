using InTouch.ViewModels;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace InTouch.Views
{
    /// <summary>
    /// Interaction logic for RegPage.xaml
    /// </summary>
    public partial class RegPage : Page
    {
        private readonly MainWindow _mainWindow;
        private readonly RegVM _vm = new();

        public RegPage(MainWindow mainWindow)
        {
            InitializeComponent();

            _mainWindow = mainWindow;

            DataContext = _vm;
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e) => Application.Current.Shutdown();

        private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = e.Uri.AbsoluteUri,
                UseShellExecute = true
            });

            e.Handled = true;
        }

        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            if (PasswordBox.Password == PasswordBoxRepeat.Password)
            {
                _vm.User.Password = PasswordBox.Password;

                Task.Run(() => _vm.RegistrateUser());
            }

            _mainWindow.StartFrame.Navigate(new MainMenu(_vm));
        }

        private void AuthorizateLink_Click(object sender, RoutedEventArgs e)
        {
            _mainWindow.StartFrame.Navigate(new LogPage(_mainWindow));
        }
    }
}
